using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Project.Application.Extensions;
using Project.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Controllers.v1
{
    /// <summary>
    /// Authentication Controller.
    /// </summary>
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Authentication Controller.
        /// </summary>
        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Register a user.
        /// </summary>
        [ProducesResponseType(typeof(Response<LoginResponse>), 201)]
        [ProducesResponseType(typeof(Response<object>), 500)]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(User user)
        {
            var response = new Response<object>();

            try
            {
                var userIdentity = new IdentityUser
                {
                    UserName = user.Email,
                    Email = user.Email
                };

                var result = await _userManager.CreateAsync(userIdentity, user.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(userIdentity, false);

                    response.Data = await GenerateJwt(userIdentity.Email);

                    return Created(nameof(RegisterAsync), response);
                }
                else
                {
                    string errors = string.Empty;

                    foreach (var e in result.Errors)
                        errors += $"{e.Description} | ";

                    errors = errors[0..^3];

                    throw new Exception(errors);
                }

            }
            catch (Exception e)
            {
                return this.InternalServerError(response, e);
            }
        }

        /// <summary>
        /// Sign in a user.
        /// </summary>
        [ProducesResponseType(typeof(Response<LoginResponse>), 200)]
        [ProducesResponseType(typeof(Response<object>), 500)]
        [HttpPost("signin")]
        public async Task<ActionResult> Login(User user)
        {
            var response = new Response<object>();

            try
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, true);

                if (result.Succeeded)
                {
                    response.Data = await GenerateJwt(user.Email);

                    return Ok(response);
                }
                if (result.IsLockedOut)
                {
                    throw new Exception("Usuário temporariamente bloqueado por tentativas inválidas.");
                }

                throw new Exception("Usuário ou senha incorretos.");
            }
            catch (Exception e)
            {
                return this.InternalServerError(response, e);
            }
        }


        /// <summary>
        /// Generate JWT (JSON Web Token).
        /// </summary>
        private async Task<LoginResponse> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            IdentityModelEventSource.ShowPII = true;
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_appSettings.Expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new LoginResponse
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.Expiration).TotalSeconds,
                UserToken = new UserTokenResponse
                {
                    Id = user.Id,
                    Email = user.Email
                }
            };

            return response;
        }
    }
}
