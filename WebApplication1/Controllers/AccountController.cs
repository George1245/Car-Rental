using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Repsitory;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IAccountRepository _accountRepo;
        public AccountController(IAccountRepository _accountRepo) { 
        this._accountRepo = _accountRepo;   
  
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
             if(await _accountRepo.LogIn(_user)!=null)
                {
                    return Ok(await _accountRepo.LogIn(_user));
                }

            }
 
            return BadRequest();
        }
        [Authorize]
        [HttpGet(nameof(Test))]
        public ActionResult Test()
        {
            return Ok("you are authorzed");
        }


    }
}
