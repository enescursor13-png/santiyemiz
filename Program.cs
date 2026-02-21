using Microsoft.EntityFrameworkCore;
using SantiyeAPI.Data;
using SantiyeAPI.Services;
using SantiyeAPI.Middlewares;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Kaydı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=SantiyeDB.db"));

// 2. Service Layer Kaydı (Dependency Injection)
builder.Services.AddScoped<IIsciService, IsciService>();
// SENIOR DOKUNUŞU: Frontend'in (HTML dosyamızın) API'ye erişebilmesi için kapıları açıyoruz.
builder.Services.AddCors(options =>
{
    options.AddPolicy("BabaninEkrani", policy =>
    {
        policy.AllowAnyOrigin() // Şimdilik her yerden erişime açıyoruz
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 3. Controller ve JSON Cycle Koruması
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 4. Global Exception Middleware Devrede
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors("BabaninEkrani"); // CORS politikasını devreye sok
app.UseAuthorization();
app.MapControllers();

app.Run();