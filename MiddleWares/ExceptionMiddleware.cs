using Microsoft.AspNetCore.Mvc; // <-- İşte o hatayı çözen sihirli satır (ProblemDetails burada yaşar)
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace SantiyeAPI.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // Her şey yolundaysa sistemi akışına bırak
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            // Hata çıkarsa araya gir, logla ve standart bir cevap dön
            _logger.LogError(ex, "Sistemde beklenmedik bir hata oluştu: {Message}", ex.Message);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json"; // Standart hata formatı
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = context.Response.StatusCode,
            Title = "Sunucu İçi Hata",
            Type = "https://datatracker.ietf.org/doc/html/rfc7807",
            Detail = "İşlem sırasında beklenmeyen bir hata oluştu. Lütfen şantiye yöneticisine başvurun.",
            Instance = context.Request.Path
        };

        // Geliştirici (Senin) için arka planda teknik hata detayları
        problemDetails.Extensions["traceId"] = context.TraceIdentifier;
        problemDetails.Extensions["exceptionMessage"] = exception.Message;

        return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}