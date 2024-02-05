using ASP.NET_store_project.Server.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP.NET_store_project.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration
    ) : ControllerBase
    {
        public readonly UserManager<IdentityUser> _userManager = userManager;

        public readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public readonly SignInManager<IdentityUser> _signInManager = signInManager;

        private readonly IConfiguration _configuration = configuration;

        [HttpPost("/api/account/create")]
        public async Task<IActionResult> CreateUser()
        {
            var inboundUser =
                Request.Form.TryGetValue("UserName", out var username)
                && Request.Form.TryGetValue("PassWord", out var password)
                    ? new InboundUser { UserName = username.ToString(), PassWord = password.ToString() }
                    : null;
            if (inboundUser == null) return BadRequest("oof");
            try
            {
                var user = new IdentityUser { UserName = inboundUser.UserName };
                Console.WriteLine(inboundUser.UserName);
                Console.WriteLine(inboundUser.PassWord);

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

        //[HttpPost("account/login")]
        //public async Task<IActionResult> Login([FromBody] InboundUser userInfo)
        //{
        //    var result = await _signInManager.PasswordSignInAsync(
        //        userInfo.UserName, 
        //        userInfo.PassWord,
        //        isPersistent: false, 
        //        lockoutOnFailure: false);
        //    if (!result.Succeeded) return BadRequest("Username or password invalid!");

        //    var user = await _userManager.FindByNameAsync(userInfo.UserName);
        //    var roles = await _userManager.GetRolesAsync(user!);
        //    return Ok(BuildToken(
        //        userInfo,
        //        roles.Contains(RoleType.Admin.ToString())
        //            ? [RoleType.User, RoleType.Admin]
        //            : [RoleType.User]));
        //}

        private async Task<string?> BuildToken(InboundUser userInfo, RoleType[] roleTypes)
        {
            var user = await _userManager.FindByNameAsync(userInfo.UserName);
            if (user == null) return null;

            List<Claim> claims = [
                new Claim(JwtRegisteredClaimNames.Name, userInfo.UserName),
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
    }
    public enum RoleType { User, Admin }
}
