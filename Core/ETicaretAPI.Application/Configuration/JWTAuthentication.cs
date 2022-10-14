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

        //access tokenın ömrü bittginde ama kulklanıcı hala işleme devam ettiginde tekrardan login olma işlemi yapıp kullanıcıyı etkilememek için refresh token denilen(token degeri) token yardımıyla yetkili token uzerınde sure uzatılmasına denir

        //refresh token belirli bir ömre sahip olan access token almamızı sagalayanbir özel ananhtar degeridir sistem giriş yapmamızı vs saglamaz örn accesstoken 45 dk ise refresh token 60 dk dır 

        //refresh token neden kullanırız?
        //-acces tokendan uzun olan refresh token  acces tokenın suresi bittginde kullanıccı islemi bitmediginde refres tokendaki sure zarfında kullanıcı işlemini yapıp sunucuya refresh token gönderilip sunucu refresh tokena gore yeni bir acces token uretip kullanıcının işine devam etmesi saglanır.

        /// <summary>
        /// jwt konfigruasyonu
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddMyJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer("Admin",options =>
            {
                options.TokenValidationParameters = new()
                { //dogrulanması gereken özellikler
                    ValidateAudience = true, //oluşturulcak token degerini hangi sitelerin kukllanıcı belirledigimiz değer 
                    ValidateIssuer = true, //oluşturulcak token değerinin kimin dağıttıgının ifade edecegimiz alandır.
                    ValidateLifetime = true, //oluşturuluan token degerinin süresini kontrol eden 
                    ValidateIssuerSigningKey = true, //üretilcek token degerinin uygulamaya ait oldugunu ifade eden security key verisinin dogrulaması

                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
                    //IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
                    LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters) => expires != null ? expires > DateTime.UtcNow : false //token dakikası gecince yetkisiz olması ıcınnn  expiresiz suresi gectiginden dolayı access token yetkisi düsücektir
                };
            });
            return services;
        }
    }
}
