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
                
                Console.WriteLine("0");
                var result = await _userManager.CreateAsync(user, inboundUser.PassWord);
                if (!result.Succeeded)
                    return BadRequest(result.Errors.Select(e => e.Description));

                Console.WriteLine("0.5");
                await _userManager.AddToRoleAsync(user, RoleType.User.ToString());
                Console.WriteLine("2");
                var token = await BuildToken(inboundUser, [RoleType.User]);
                Console.WriteLine("11");
                return token != null
                    ? Ok(token)
                    : BadRequest("Email or password invalid!");
            }
            catch (Exception e)
            {
                return BadRequest("hm?");
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
            Console.WriteLine("3");
            if (user == null) return null;
            Console.WriteLine("4");
            List<Claim> claims = [
                new Claim(JwtRegisteredClaimNames.Name, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            ];
            Console.WriteLine("5");
            foreach (var role in roleTypes)
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            Console.WriteLine("6");
            foreach (var role in await _userManager.GetRolesAsync(user))
                claims.Add(new Claim(ClaimTypes.Role, role));
            Console.WriteLine("7");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]!));
            Console.WriteLine("8");
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            Console.WriteLine("9");
            var expiration = DateTime.UtcNow.AddHours(1);
            var tokenHanlder = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                SigningCredentials = creds
            };
            Console.WriteLine("10");
            return tokenHanlder.WriteToken(tokenHanlder.CreateToken(tokenDescriptor));
        }
    }
    public enum RoleType { User, Admin }
}
