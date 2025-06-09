namespace ASP.NET_store_project.Server.Services;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

// Called by the controller to create authorization tokens
public sealed class TokenProvider(IConfiguration configuration)
{
    // Creates simple ID-storing token that expires after a set amount of time (check appsettings.json)
    public string Create(Guid? userId, IEnumerable<Claim>? specialClaims = null)
    {
        string secretKey = configuration["JWT:Secret"]!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                ..specialClaims ?? [],
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString() ?? "")
            ]),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JWT:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["JWT:Issuer"],
            Audience = configuration["JWT:Audience"],
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}
