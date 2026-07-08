using Microsoft.EntityFrameworkCore;
using SantiyeAPI.Data;
using SantiyeAPI.Services;
using SantiyeAPI.Middlewares;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using FluentValidation;
using SantiyeAPI.Validators;
using AutoMapper;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using SantiyeApp.Services;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SantiyeAPI.Helpers;



var builder = WebApplication.CreateBuilder(args);

// 🚀 PORT AYARI: Railway/bulut ortamları PORT ortam değişkenini kendisi enjekte eder
// ve dışarıdan 0.0.0.0 üzerinden erişilmesini bekler. Yerelde (masaüstü/kiosk
// modunda) PORT tanımlı değilse eski davranış (localhost:5095) korunur.
var railwayPort = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(railwayPort))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{railwayPort}");
}
else
{
    builder.WebHost.UseUrls("http://localhost:5095");
}

// 🚀 ZIRH 1: DDOS ve Çift Tıklama Koruması
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("PuantajLimiter", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(1);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    // 🚀 BRUTE-FORCE ZIRHI: Login denemelerini IP başına sınırla.
    // Sınırsız şifre denemesini engeller (dakikada 5 deneme, kuyruksuz reddediyor).
    options.AddPolicy("AuthLimiter", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "bilinmeyen",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(
            new { mesaj = "Çok fazla deneme yaptınız. Lütfen bir dakika sonra tekrar deneyin." }, token);
    };
});

builder.Services.AddScoped<IIsciService, IsciService>();
builder.Services.AddScoped<IKasaService, KasaService>();
builder.Services.AddScoped<SiteService>();

// 🚀 Sadece appsettings.json'da tanımlı (bilinen) kaynaklara izin ver.
// "null" değeri, SantiyeEkrani gibi file:// üzerinden açılan sayfaların
// tarayıcıda gönderdiği Origin başlığını karşılar.
var izinliOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "null", "http://localhost:5095" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("BabaninEkrani", policy =>
    {
        policy.WithOrigins(izinliOrigins).AllowAnyHeader().AllowAnyMethod();
    });
});

// 🔐 JWT Kimlik Doğrulama
// 🔐 JWT anahtarı bilinçli olarak appsettings.json'da DEĞİL — repo public olduğu için
// kaynak kodda durursa herkes geçerli token sahtesi üretebilir. Yerelde
// `dotnet user-secrets set "Jwt:Key" "..."` ile, Railway'de ise "Jwt__Key" ortam
// değişkeniyle sağlanmalı.
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key tanımlı değil! Yerelde 'dotnet user-secrets set \"Jwt:Key\" \"...\"', Railway'de ise Jwt__Key ortam değişkeni ile ayarlayın.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;
var jwtAudience = builder.Configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var hatalar = context.ModelState.Values
                .Where(v => v.Errors.Count > 0)
                .Select(v => v.Errors.First().ErrorMessage)
                .ToList();
            return new BadRequestObjectResult(new { mesaj = string.Join("<br><br>", hatalar) });
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// 🐘 POSTGRESQL BAĞLANTISI
// Öncelik: Railway'in kendi ürettiği "DATABASE_URL" (postgres://user:pass@host:port/db
// şeklinde bir URI, .NET connection string formatında değil) — bunu Npgsql'in anlayacağı
// formata otomatik çeviriyoruz. Tanımlı değilse appsettings.json / ConnectionStrings__
// DefaultConnection ortam değişkenine (yerel geliştirme için) düşüyoruz.
// NOT: appsettings.json'da her zaman bir yerel-geliştirme varsayılanı olduğu için
// GetConnectionString() ASLA null dönmez — bu yüzden DATABASE_URL'i önce kontrol
// etmek zorundayız, yoksa Railway'de yanlışlıkla o yerel varsayılana bağlanmaya çalışılır.
string connectionString = PostgresBaglantiUret(Environment.GetEnvironmentVariable("DATABASE_URL"))
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Veritabanı bağlantı bilgisi bulunamadı! ConnectionStrings:DefaultConnection veya DATABASE_URL ayarlanmalı.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString, o =>
        o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddMemoryCache();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IsciCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<IsciCreateDtoValidator>();


var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("BabaninEkrani");
app.UseRateLimiter(); // DDOS korumasını çalıştırır

app.UseAuthentication();
app.UseAuthorization();

// 🚀 ZIRH 2: DİKKAT! Yorum satırlarını sildik ki Babanın bilgisayarında HTML dosyaları (arayüz) açılsın!
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();



// 🚀 ZIRH 3: Veritabanı şemasını migration'larla güncelle ve Şirketi "0 Jeton" ile ekle!
// EnsureCreated() yerine Migrate() kullanıyoruz: EnsureCreated ileride şema
// değişince mevcut (canlı) veritabanlarını güncellemez, sessizce uyuşmazlık
// yaratır. Migrate() ise hem ilk kurulumda şemayı oluşturur hem de sonraki
// sürümlerde eklenen değişiklikleri var olan veritabanına güvenle uygular.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.Migrate();

        // 👑 Program açılırken 1 kere çalışır, kimseyle yarışmaz!
        bool sirketVarMi = dbContext.Companies.Any();
        if (!sirketVarMi)
        {
            dbContext.Companies.Add(new Company
            {
                Name = "Bizim Şantiye A.Ş.",
                AllowedActiveSiteCount = 0,
                DonanimKimligi = "BEKLIYOR",
                SonIslemTarihi = ZamanMotoru.SimdiTurkiye()
            });
            dbContext.SaveChanges();
            Console.WriteLine("✅ Şantiye veritabanı kuruldu ve lisans sistemi hazırlandı!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"DB Hatası: {ex.Message}");
    }
}

// 🚀 ZIRH 6: KALP ATIŞI (HEARTBEAT) PROTOKOLÜ
DateTime? sonKalpAtisi = null;
DateTime sunucuAcilisZamani = DateTime.Now;

// Babanın ekranından (JS) sürekli bu adrese ping gelecek
// OLMASI GEREKEN
app.MapGet("/api/kalpatisi", () =>
{
    sonKalpAtisi = DateTime.Now; // ← Bu satır eksikti!
    return Results.Ok();
})
.RequireCors("BabaninEkrani");

// 🚀 KİOSK MODU: Bu iki davranış (tarayıcıyı otomatik açma + heartbeat kesilince
// kendi kendini kapatma) SADECE "her müşteri kendi bilgisayarında tek başına
// çalıştırır" masaüstü modelinde anlamlıdır. Bulutta (Railway vb.) barındırılırsa
// appsettings.json'da "KioskMode:Enabled": false yapılmalı — aksi halde tek bir
// kullanıcının sekmesi arka planda kalıp heartbeat kesilirse TÜM müşterilerin
// API'si kapanır!
bool kioskModuAktif = builder.Configuration.GetValue("KioskMode:Enabled", true);

if (kioskModuAktif)
{
    // 🚀 ZIRH 5 & 6 BİRLEŞİMİ: Tarayıcıyı Açma ve Ölüm Kontrolü
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        // 1. Görev: Tarayıcıyı Edge ile aç
        Task.Run(async () =>
        {
            var url = "http://localhost:5095";
            using var http = new HttpClient();
            for (int i = 0; i < 10; i++)
            {
                try { await http.GetAsync(url); break; }
                catch { await Task.Delay(500); }
            }
            bool launched = false;
            if (OperatingSystem.IsWindows())
            {
                launched = TryLaunch("msedge", $"--app={url}");
            }
            else if (OperatingSystem.IsMacOS())
            {
                launched = TryLaunch("open", $"-a \"Microsoft Edge\" --args --app={url}");
                if (!launched) launched = TryLaunch("open", url);
            }
            if (!launched) TryLaunch(url, null);
        });

        // 2. Görev: Çarpıya basıldığını (Sinyalin kesildiğini) denetle
        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(3000); // 3 saniyede bir durumu kontrol et

                // Senaryo A: Uygulama açıldı ama babanın bilgisayarında tarayıcı hiç açılamadı.
                // Arkada sonsuza kadar açık kalmasın, 15 saniye sonra sistemi kapat.
                if (sonKalpAtisi == null && (DateTime.Now - sunucuAcilisZamani).TotalSeconds > 180)
                {
                    app.Lifetime.StopApplication();
                    break;
                }

                // Senaryo B: Tarayıcı açıktı ama sinyal kesildi (Baban çarpıya bastı).
                // 6 saniye tahammül et (sayfayı yenilerse yani F5 atarsa yanlışlıkla kapanmasın diye),
                // hala ses yoksa arkadaki hayalet API'yi tamamen öldür!
                if (sonKalpAtisi != null && (DateTime.Now - sonKalpAtisi.Value).TotalSeconds > 1200)
                {
                    Console.WriteLine("Çarpıya basıldı, arka plan temizlenip kapanıyor...");
                    app.Lifetime.StopApplication();
                    break;
                }
            }
        });
    });
}

app.Run();

// --- TryLaunch metodu olduğu gibi kalacak ---



// ---

static bool TryLaunch(string fileName, string? args)
{
    try
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = fileName,
            Arguments = args ?? "",
            UseShellExecute = true
        });
        return true;
    }
    catch { return false; }
}

// 🐘 Railway'in "postgres://user:pass@host:port/dbname" formatındaki DATABASE_URL'ini
// Npgsql'in beklediği "Host=...;Port=...;Database=...;Username=...;Password=..." formatına çevirir.
static string? PostgresBaglantiUret(string? databaseUrl)
{
    if (string.IsNullOrWhiteSpace(databaseUrl)) return null;

    try
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2);
        var kullanici = Uri.UnescapeDataString(userInfo[0]);
        var sifre = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : "";
        var veritabani = uri.AbsolutePath.TrimStart('/');
        var port = uri.Port > 0 ? uri.Port : 5432;

        return $"Host={uri.Host};Port={port};Database={veritabani};Username={kullanici};Password={sifre};SSL Mode=Require;Trust Server Certificate=true";
    }
    catch (UriFormatException)
    {
        return null;
    }
}