namespace ASP.NET_store_project.Server.Controllers.IdentityController;

using Data;
using Data.DataRevised;
using Extentions;
using Models;
using Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class IdentityController(AppDbContext context, TokenProvider tokenProvider) : ControllerBase
{
    // Creates new user
    [HttpPost("/api/account/create")]
    public IActionResult CreateUser([FromForm] SignUpModel credentials)
    {
        // Checks if user credentials are valid
        var validationResult = new SignUpValidator().Validate(credentials);
        if (!validationResult.IsValid) return BadRequest(new { Errors = validationResult.ToDictionary() });

        // Checks whether user with selected username already exists
        if (context.Users.Any(customer => customer.UserName == credentials.UserName))
            return this.SingleErrorBadRequest("UserName", "This username is already taken.");
        
        // Creates new record with hashed password
        var user = new User(credentials.UserName, "");
        PasswordHasher<User> passwordHasher = new();
        var hashedPassword = passwordHasher.HashPassword(user, credentials.PassWord);
        user.PassWord = hashedPassword;

        // Adds user to the database
        context.Add(user);
        context.SaveChanges();

        return Ok("Account created succesfully");
    }

    // Authenticates the user
    [HttpPost("/api/account/login")]
    public IActionResult LogIn([FromForm] SignInModel credentials)
    {
        // Identifies user
        var user = context.Users
            .SingleOrDefault(customer => customer.UserName == credentials.UserName);
        if (user == null)
            return this.SingleErrorBadRequest("UserName", "Username or password is incorrect.");

        // Compares hashed database password to provided one
        PasswordHasher<User> passwordHasher = new();
        var result = passwordHasher.VerifyHashedPassword(user, user.PassWord, credentials.PassWord);
        if (result == PasswordVerificationResult.Failed)
            return this.SingleErrorBadRequest("UserName", "Username or password is incorrect.");

        // Adds claims based on user's privileges
        List<Claim> claims = [new(IdentityData.RegularUserClaimName, "true", ClaimValueTypes.Boolean)];
        if (user.IsAdmin)
            claims.Add(new(IdentityData.AdminUserClaimName, "true", ClaimValueTypes.Boolean));

        // Creates JWT token that will be attached to every other request (user is logged in)
        var token = tokenProvider.Create(user.Id, claims);
        Response.Cookies.Append("Token", token, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict
        });

        return Ok("Successfuly logged in.");
    }

    // Logs out the user
    [HttpGet("/api/account/logout")]
    public IActionResult LogOut()
    {
        // Replaces authorized JWT token with a blank
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
        // If there is no token or user's ID is not included - anonymous
        if (!Request.Cookies.TryGetValue("Token", out var token))
            return Ok(IdentityData.AnonymousUserPolicyName);
        var userId = new JsonWebToken(token).Subject;
        if (userId == null || userId == "") 
            return Ok(IdentityData.AnonymousUserPolicyName);

        // Identifies user
        var user = context.Users
            .SingleOrDefault(customer => customer.Id == Guid.Parse(userId));
        if (user == null)
            return Ok(IdentityData.AnonymousUserPolicyName);

        // Returnes user's priviliges state based on database record
        return Ok(user.IsAdmin 
            ? IdentityData.AdminUserPolicyName
            : IdentityData.RegularUserPolicyName);
    }
}
