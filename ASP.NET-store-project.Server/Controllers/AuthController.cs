using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthController(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration
    ) : ControllerBase
    {
        public UserManager<IdentityUser> _userManager = userManager;

        public RoleManager<IdentityRole> _roleManager = roleManager;

        public SignInManager<IdentityUser> _signInManager = signInManager;

        private readonly IConfiguration _configuration = configuration;

        [HttpPost("account/create")]
        public async Task<IActionResult> CreateUser([FromBody] InboundUser inboundUser)
        {
            try
            {
                var user = new IdentityUser { UserName = inboundUser.UserName };

                if (!await _roleManager.RoleExistsAsync(RoleType.User.ToString()))
                    await _roleManager.CreateAsync(new IdentityRole(RoleType.User.ToString()));

                var result = await _userManager.CreateAsync(user, inboundUser.PassWord);
                await _userManager.AddToRoleAsync(user, RoleType.User.ToString());

                if (!result.Succeeded)
                    return BadRequest(result.Errors.Select(e => e.Description));

                var token = BuildToken(inboundUser, [RoleType.User]);
                return token != null
                    ? Ok(token)
                    : BadRequest("Email or password invalid!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("account/login")]
        public async Task<IActionResult> Login([FromBody] InboundUser userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userInfo.UserName, 
                userInfo.PassWord,
                isPersistent: false, 
                lockoutOnFailure: false);

            return result.Succeeded
                ? Ok(BuildToken(userInfo, [RoleType.User]))
                : BadRequest("Username or password invalid!");
        }

        private async Task<string> BuildToken(InboundUser userInfo, RoleType[] roleTypes)
        {
            var user = await _userManager.FindByNameAsync(userInfo.UserName);
            if (user == null) return null;

            List<Claim> claims = [
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];

            foreach (var role in roleTypes)
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));

            foreach (var role in await _userManager.GetRolesAsync(user))
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);
            var token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //[ActionName("SignIn")]
        //public bool SignIn(HttpContext httpContext)
        //{
        //    var formData = Request.Form;
        //    var user = formData.TryGetValue("UserName", out var username)
        //        ? formData.TryGetValue("PassWord", out var password)
        //            ? context.Customers
        //                .Where(customer => customer.UserName == username.ToString())
        //                .Where(customer => customer.PassWord == password.ToString())
        //                .FirstOrDefault()
        //            : null
        //        : null;
        //    if (user == null) return false;

        //}
    }
    public enum RoleType { User, Admin }
}
