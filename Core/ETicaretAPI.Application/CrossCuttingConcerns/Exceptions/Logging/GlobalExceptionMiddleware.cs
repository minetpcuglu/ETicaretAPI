using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.CrossCuttingConcerns.Exceptions.Logging
{
    //log ortak bir yerde kurgulayıp tüm projede aktif hale getirildi
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _request;
        private static readonly ILogger _logger = Log.ForContext<GlobalExceptionMiddleware>();
        public GlobalExceptionMiddleware(RequestDelegate request)
        {
            _request = request;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _request(httpContext);
            }
            catch (Exception ex)
            {
                // Kalabalığı engellemek için hata logic'ini bir metoda taşıyorum.
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            _logger.Error($"{DateTime.Now.ToString("HH:mm:ss")} : {ex}");
            httpContext.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                StatusCode=httpContext.Response.StatusCode, //alınan hatanın status kodu
                Message = ex, //alınan hata
                Title="Hata Alındı Log tablosunu inceleyiniz" //messasge
            }));
            return Task.CompletedTask;
        }
    }
}

