using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Data.DataRevised;
using ASP.NET_store_project.Server.Models;
using ASP.NET_store_project.Server.Utility;
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
                    ? new { UserName = username.ToString(), HashedPassWord = new SimplePasswordHasher().HashPassword(password.ToString()) }
                    : null;
            if (userInfo == null) return BadRequest("Username or password is missing.");

            var alreadyExists = context.Users
                .Where(customer => customer.UserName == userInfo.UserName).Any();
            if (alreadyExists)
                return BadRequest("User already exists.");

            // Include data validation here

            context.Users.Add(new User(userInfo.UserName, userInfo.HashedPassWord));
            context.SaveChanges();
            return Ok("Account created succesfully");
        }

        [HttpPost("/api/account/login")]
        public IActionResult LogIn()
        {
            var userInfo =
                Request.Form.TryGetValue("UserName", out var username)
                && Request.Form.TryGetValue("PassWord", out var password)
                    ? new { UserName = username.ToString(), HashedPassWord = password.ToString() }
                    : null;
            if (userInfo == null) return BadRequest("Username or password is missing.");

            var customer = context.Users
                .Where(customer => customer.UserName == userInfo.UserName);

            try
            {
                if (!customer.Any())
                    throw new UnauthorizedAccessException();
                new SimplePasswordHasher().VerifyHash(customer.Single().PassWord, userInfo.HashedPassWord);
            } 
            catch (UnauthorizedAccessException)
            {
                return BadRequest("Username or password is incorrect.");
            }

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

        [HttpGet("/api/account/logout")]
        public IActionResult LogOut()
        {
            var token = GenerateToken("Anonymous", []);
            Response.Cookies.Append("Token", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });
            return Ok("Successfuly logged out.");
        }

        [HttpGet("/api/account/identity")]
        public IActionResult Identity()
        {
            if (!Request.Cookies.TryGetValue("Token", out var token))
                return Ok(IdentityData.AnonymousUserPolicyName);

            var username = new JwtSecurityToken(token).Subject;
            if (username == null) 
                return Ok(IdentityData.AnonymousUserPolicyName);

            var user = context.Users
                .Where(customer => customer.UserName == username);
            if (!user.Any())
                return Ok(IdentityData.AnonymousUserPolicyName);

            Console.WriteLine(username);

            return Ok(user.Single().IsAdmin 
                ? IdentityData.AdminUserPolicyName
                : IdentityData.RegularUserPolicyName);
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
