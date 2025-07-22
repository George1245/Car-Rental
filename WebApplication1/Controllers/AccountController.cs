using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repsitory;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IAccountRepository _accountRepo;
        public UserManager<App_User> _userManager;
        public AccountController(IAccountRepository _accountRepo, UserManager<App_User> _userManager) { 
        this._accountRepo = _accountRepo;   
        this._userManager = _userManager;
        }
        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register(userRegisterDTO _user)
        {
            if (_user != null)
            {
                if (await _accountRepo.Register(_user))
                {
                    return Ok();

                }
            }


            return BadRequest();
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(UserLoginDTO _user)
        {
            if (_user != null)
            {
                App_User User = await _userManager.FindByNameAsync(_user.userName);
                if (User != null)
                {
                    if (await _userManager.IsEmailConfirmedAsync(User))
                    {
                        if (await _accountRepo.LogIn(_user) != null)
                        {
                            return Ok(await _accountRepo.LogIn(_user));
                        }
                    }
                    else
                    {
                        return Unauthorized("please confirm your email!");
                    }
              
                }

            }
 
            return BadRequest();
        }

        [Authorize(Roles ="Admin")]
        [HttpGet(nameof(Test))]
        public ActionResult Test()
        {
            return Ok("you are authorzed");
        }

        [HttpGet(nameof(confirmemail))]
        public async Task<ActionResult> confirmemail(string userId, string token)
        {
            App_User user =await _userManager.FindByIdAsync(userId);
            if (user != null) { 
                var decodedToken =Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
                IdentityResult checkEmail = await _userManager.ConfirmEmailAsync(user, decodedToken);
                if (checkEmail.Succeeded)
                {
                    return Ok("your Email is confirmed");
                }
                else
                {
                    return BadRequest("your Email is not confirmed");
                }
            }
            return BadRequest("your Email is not confirmed");
        }
        [HttpGet(nameof(callBack))]
        public async Task<ActionResult> callBack()
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

            if (!result.Succeeded)
                return Unauthorized(result);

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new App_User
                {
                    UserName = name.Replace(" ", ""),
                    Email = email,
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                    return BadRequest("Failed to create user from Google login");
            }

            var Claims = new List<Claim>();
            Claims.Add(new Claim(ClaimTypes.Email, user.Email));
            Claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            var Token = _accountRepo.generateToken(Claims);
            return Ok(Token);
 

        }
        [HttpGet(nameof(googleLogIn))]
        public async Task<ActionResult> googleLogIn()
        {
            var Prorperties = new AuthenticationProperties
            {

                RedirectUri = Url.Action("callBack")
            };

            return Challenge(Prorperties, GoogleDefaults.AuthenticationScheme);
        }  


    }
}