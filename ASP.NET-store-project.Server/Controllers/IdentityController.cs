using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController(AppDbContext context) : ControllerBase
    {
        private readonly string TokenSecret = "StoreItWithAzureKeyVaultOrSomethingSimilar";

        [HttpPost("/api/account/create")]
        public IActionResult CreateUser()
        {
            var userInfo =
                Request.Form.TryGetValue("UserName", out var username)
                && Request.Form.TryGetValue("PassWord", out var password)
                    ? new { UserName = username.ToString(), PassWord = password.ToString() }
                    : null;
            if (userInfo == null) return BadRequest("Username or password is missing.");

            var alreadyExists = context.Customers
                .Where(customer => customer.UserName == username).Any();
            if (alreadyExists)
                return BadRequest("User already exists.");

            // Include data validation here

            context.Customers.Add(new Customer(userInfo.UserName, userInfo.PassWord));
            context.SaveChanges();
            return Ok("Account created succesfully");
        }

        [HttpPost("/api/account/login")]
        public IActionResult Login()
        {
            var userInfo =
                Request.Form.TryGetValue("UserName", out var username)
                && Request.Form.TryGetValue("PassWord", out var password)
                    ? new { UserName = username.ToString(), PassWord = password.ToString() }
                    : null;
            if (userInfo == null) return BadRequest("Username or password is missing.");

            var customer = context.Customers
                .Where(customer => customer.UserName == userInfo.UserName);
            if (!customer.Any() || customer.Single().PassWord != userInfo.PassWord)
                return BadRequest("Username or password is incorrect.");

            List<CustomClaim> claims = [new(IdentityData.RegularUserClaimName, "true", ClaimValueTypes.Boolean)];
            if (customer.Single().IsAdmin)
                claims.Add(new(IdentityData.AdminUserClaimName, "true", ClaimValueTypes.Boolean));

            var token = GenerateToken(userInfo.UserName, claims);
            Response.Cookies.Append("Token", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });
            return Ok("Successfuly logged in.");
        }

        private string GenerateToken(string userName, List<CustomClaim> customClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(TokenSecret);

            List<Claim> claims = [
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, userName),
                new(JwtRegisteredClaimNames.Name, userName),
            ];

            foreach (var customClaim in customClaims)
                claims.Add(new Claim(
                    customClaim.Type,
                    customClaim.Value,
                    customClaim.ValueType));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                Issuer = "me",
                Audience = "me",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}