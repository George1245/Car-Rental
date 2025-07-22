using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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


    }
}