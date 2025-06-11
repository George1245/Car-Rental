
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repsitory
{
    public class AccountRepository : IAccountRepository
    {
        public UserManager<App_User> _userManager;

        public SignInManager<App_User> _signInManager;
        public RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _appDbContext;
        public App_User _user;
        public AccountRepository(UserManager<App_User> _userManager, SignInManager<App_User> _signInManager, RoleManager<IdentityRole> _roleManager,AppDbContext appDbContext,App_User _user)
        { 
            this._userManager = _userManager;
            this._signInManager = _signInManager;   
            this._roleManager = _roleManager;   
            this._appDbContext = appDbContext;
            this._user =_user ;
        }
        public async Task<bool> Register(userRegisterDTO userRegister)
        {

            if (userRegister != null)
            { 
             _user.Email = userRegister.Email;
            _user.UserName = userRegister.userName;
            _user.PhoneNumber= userRegister.phoneNumber;
                IdentityResult userRegisterResult = await _userManager.CreateAsync(_user,userRegister.Password);
                if (userRegisterResult.Succeeded)
                {
                    
                   IdentityResult roleResult = await _userManager.AddToRoleAsync(_user,"User");
                    if (roleResult.Succeeded)
                    { 
                    return true;
                    }
                    return false;
                }
                return false;

            }

            return false;   

        }
        public Task<bool> LogIn(UserLoginDTO userLogin) 
        {
            return Task.FromResult(true);

        }
        public Task<bool> LogOut()
        {
            return Task.FromResult(true);

        }
    }
}
