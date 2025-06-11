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

            if (await _accountRepo.Register(_user))
            {
                return Ok();

            }

            return BadRequest();
        }


    }
}
