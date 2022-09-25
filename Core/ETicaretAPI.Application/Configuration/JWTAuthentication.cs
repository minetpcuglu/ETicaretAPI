using ETicaretAPI.Application.Utilities.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Configuration
{
     public static  class JWTAuthentication
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMyJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                { //dogrulanması gereken özellikler
                    ValidateAudience = true, //oluşturulcak token degerini hangi sitelerin kukllanıcı belirledigimiz değer 
                    ValidateIssuer = true, //oluşturulcak token değerinin kimin dağıttıgının ifade edecegimiz alandır.
                    ValidateLifetime = true, //oluşturuluan token degerinin süresini kontrol eden 
                    ValidateIssuerSigningKey = true, //üretilcek token degerinin uygulamaya ait oldugunu ifade eden security key verisinin dogrulaması

                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
                    //IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                };
            });
            return services;
        }
    }
}
