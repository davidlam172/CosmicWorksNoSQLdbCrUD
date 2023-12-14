using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CosmicWorksTest2.Services;
public class TokenService
{
    static string GenerateRandomKey(int lengthInBytes)
    {
        using (var rng = new RNGCryptoServiceProvider())
        {
            byte[] randomBytes = new byte[lengthInBytes];
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
    public string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            // Add more claims as needed
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GenerateRandomKey(32)));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "https://cosmicworkstest220230929010127.azurewebsites.net",
            audience: "https://cosmicworkstest220230929010127.azurewebsites.net/addproduct",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
