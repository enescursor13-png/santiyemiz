# --- 1. AŞAMA: Derleme ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SantiyeAPI.csproj ./
RUN dotnet restore SantiyeAPI.csproj

COPY . .
RUN dotnet publish SantiyeAPI.csproj -c Release -o /app/publish --no-restore

# --- 2. AŞAMA: Çalıştırma (küçük, sadece runtime içeren imaj) ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Npgsql'in SSL/SCRAM el sıkışması için ihtiyaç duyduğu kütüphane; minimal
# runtime imajında yok, yoksa "libgssapi_krb5.so.2 bulunamadı" uyarısı çıkar.
RUN apt-get update && apt-get install -y --no-install-recommends libkrb5-3 \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Railway PORT ortam değişkenini kendisi enjekte eder, Program.cs bunu okuyup dinliyor.
ENTRYPOINT ["dotnet", "SantiyeAPI.dll"]
