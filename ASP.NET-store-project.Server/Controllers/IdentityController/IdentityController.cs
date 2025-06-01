using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Data.DataRevised;
using ASP.NET_store_project.Server.Models;
using ASP.NET_store_project.Server.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP.NET_store_project.Server.Controllers.IdentityController
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController(AppDbContext context) : ControllerBase
    {
        private readonly string TokenSecret = "StoreItWithAzureKeyVaultOrSomethingSimilar";

        // Creates new user
        [HttpPost("/api/account/create")]
        public IActionResult CreateUser([FromForm] SignUpModel credentials)
        {
            // Check if user credentials are valid and strong enough
            var validationResult = new SignUpValidator().Validate(credentials);
            if (!validationResult.IsValid) return BadRequest(new { Errors = validationResult.ToDictionary() });

            // Check if user of given username already exists
            if (context.Users.Any(customer => customer.UserName == credentials.UserName))
                return BadRequest("User already exists.");
            
            // Hash password and create new User record in the database
            var hashedPassword = new SimplePasswordHasher().HashPassword(credentials.PassWord);
            context.Add(new User(credentials.UserName, hashedPassword));
            context.SaveChanges();

            return Ok("Account created succesfully");
        }

        // Authorizes user
        [HttpPost("/api/account/login")]
        public IActionResult LogIn([FromForm] SignInModel credentials)
        {
            // Checks if user exists
            var user = context.Users
                .SingleOrDefault(customer => customer.UserName == credentials.UserName);
            if (user == null)
                return BadRequest("Username or password is incorrect.");

            // Verify password
            try { new SimplePasswordHasher().VerifyHash(user.PassWord, credentials.PassWord); }
            catch (UnauthorizedAccessException) { return BadRequest("Username or password is incorrect."); }

            // Add claims based on user's privileges
            List<CustomClaim> claims = [new(IdentityData.RegularUserClaimName, "true", ClaimValueTypes.Boolean)];
            if (user.IsAdmin)
                claims.Add(new(IdentityData.AdminUserClaimName, "true", ClaimValueTypes.Boolean));

            // Create JWT token that will be attached to every request (user is logged in)
            var token = GenerateToken(credentials.UserName, claims);
            Response.Cookies.Append("Token", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok("Successfuly logged in.");
        }

        // Logs out user
        [HttpGet("/api/account/logout")]
        public IActionResult LogOut()
        {
            // Changes authorized JWT token with a blank
            var token = GenerateToken("Anonymous", []);
            Response.Cookies.Append("Token", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok("Successfuly logged out.");
        }

        // Identifies user's privileges
        [HttpGet("/api/account/identity")]
        public IActionResult Identity()
        {
            // If there is no token - anonymous
            if (!Request.Cookies.TryGetValue("Token", out var token))
                return Ok(IdentityData.AnonymousUserPolicyName);

            // If there is no username included in the token - anonymous
            var username = new JwtSecurityToken(token).Subject;
            if (username == null) 
                return Ok(IdentityData.AnonymousUserPolicyName);

            // If user of given username doesn't exists - anonymous
            var user = context.Users
                .SingleOrDefault(customer => customer.UserName == username);
            if (user == null)
                return Ok(IdentityData.AnonymousUserPolicyName);

            // Otherwise answers based on data in the database
            return Ok(user.IsAdmin 
                ? IdentityData.AdminUserPolicyName
                : IdentityData.RegularUserPolicyName);
        }

        // JWT token generator
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
