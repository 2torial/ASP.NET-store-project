using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Data.DataRevised;
using ASP.NET_store_project.Server.Models;
using ASP.NET_store_project.Server.Utilities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace ASP.NET_store_project.Server.Controllers.IdentityController
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController(AppDbContext context, TokenProvider tokenProvider) : ControllerBase
    {
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
            
            var user = new User(credentials.UserName, "");
            PasswordHasher<User> passwordHasher = new();
            var hashedPassword = passwordHasher.HashPassword(user, credentials.PassWord);
            user.PassWord = hashedPassword;

            context.Add(user);
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

            PasswordHasher<User> passwordHasher = new();
            var result = passwordHasher.VerifyHashedPassword(user, user.PassWord, credentials.PassWord);
            if (result == PasswordVerificationResult.Failed)
                return BadRequest("Username or password is incorrect.");

            // Add claims based on user's privileges
            List<Claim> claims = [new(IdentityData.RegularUserClaimName, "true", ClaimValueTypes.Boolean)];
            if (user.IsAdmin)
                claims.Add(new(IdentityData.AdminUserClaimName, "true", ClaimValueTypes.Boolean));

            // Create JWT token that will be attached to every request (user is logged in)
            var token = tokenProvider.Create(user.Id, claims);
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
            var token = tokenProvider.Create(null);
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
            var userId = new JsonWebToken(token).Subject;

            Console.WriteLine("AAAAAA");
            Console.WriteLine(new JsonWebToken(token).GetClaim("user").ToString());

            if (userId == null || userId == "") 
                return Ok(IdentityData.AnonymousUserPolicyName);

            // If user of given username doesn't exists - anonymous
            var user = context.Users
                .SingleOrDefault(customer => customer.Id == Guid.Parse(userId));
            if (user == null)
                return Ok(IdentityData.AnonymousUserPolicyName);

            // Otherwise answers based on data in the database
            return Ok(user.IsAdmin 
                ? IdentityData.AdminUserPolicyName
                : IdentityData.RegularUserPolicyName);
        }

    }
}
