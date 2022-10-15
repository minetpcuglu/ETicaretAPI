using ETicaretAPI.Application.Utilities.Security.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Utilities.Security.Token
{
    public class TokenHandler : ITokenHandler
    {
       readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AccessToken CreateAccessToken(int second)
        {
            AccessToken accessToken = new();
            //Security key ın simetrigini alıyoruz
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["TokenOptions:SecurityKey"]));
            //Şifrlenmiş kimliği oluşturuyoruz
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256) ;
            //Oluşturulcak token ayarlarını veriyoruz
            accessToken.Expiration = DateTime.Now.AddSeconds(second);
            JwtSecurityToken securityToken = new( //token üreme
                audience: _configuration["TokenOptions:Audience"],
                issuer: _configuration["TokenOptions:Issuer"],
                expires: accessToken.Expiration,
                notBefore: DateTime.Now, //token üretildikten ne zaman sonra devreye girsin
                signingCredentials:signingCredentials 
                );
                //token oluşturucu sınıfından bir örnek alalım
                JwtSecurityTokenHandler tokenHandler = new();
                accessToken.Token = tokenHandler.WriteToken(securityToken);

                accessToken.RefreshToken= CreateRefreshToken(); //refresh token olusturuldu
                return accessToken;
        }
        public string CreateRefreshToken()
        {
            byte[] number = new byte[32]; //kompleks bir deger üretilmesi
            using RandomNumberGenerator random = RandomNumberGenerator.Create(); //rastgele deger üretilme //üsing ile dispose edilebilir yani işi bittignde  otomatik dispose edilir
            random.GetBytes(number); //ilgili byte random aktarma
            return Convert.ToBase64String(number);  //metinsel hale alma
        }

      
    }
}
