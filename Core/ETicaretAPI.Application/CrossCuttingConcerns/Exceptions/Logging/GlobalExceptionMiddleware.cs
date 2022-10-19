using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            return Task.CompletedTask;
        }
    }
}

