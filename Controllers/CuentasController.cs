using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Módulo_7.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Módulo_7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class CuentasController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public CuentasController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;

        }

        [HttpPost("Crear")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo userInfo)
        {
            var user = new ApplicationUser(userInfo.Email, userInfo.Email);
            var result = await _userManager.CreateAsync(user, userInfo.Password);

            if (result.Succeeded)
            {
                return BuildToken(userInfo, new List<string>());
            }
            else
            {
                return BadRequest("Username or password invalid.");
            }
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password);

            if (result.Succeeded)
            {
                var usuario = await _userManager.FindByNameAsync(userInfo.Email);
                var roles = await _userManager.GetRolesAsync(usuario);

                return BuildToken(userInfo, roles);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid atempt error.");
                return BadRequest(ModelState);
            }
        }


        private UserToken BuildToken(UserInfo userInfo, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("mi value", "anything"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var key = SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler.WriteToken(token),
                Expiration = expiration
            };


        }
    }

}