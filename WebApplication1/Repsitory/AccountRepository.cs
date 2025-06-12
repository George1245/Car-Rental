
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
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
        public IConfiguration Configuration;
        public AccountRepository(UserManager<App_User> _userManager, SignInManager<App_User> _signInManager, RoleManager<IdentityRole> _roleManager,AppDbContext appDbContext,App_User _user, IConfiguration _Configuration)
        { 
            this._userManager = _userManager;
            this._signInManager = _signInManager;   
            this._roleManager = _roleManager;   
            this._appDbContext = appDbContext;
            this._user =_user ;
            Configuration=_Configuration;
        }   

        public App_User getUserByName(string userName)
        {
            var user = _appDbContext.Users.FirstOrDefault(U => U.UserName == userName);
            return user;
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
        public async Task<string> LogIn(UserLoginDTO userLogin) 
        {
            if (userLogin != null)
            {
                App_User User = getUserByName(userLogin.userName);
                if (User != null)
                {
                    if (await _userManager.CheckPasswordAsync(User, userLogin.Password))
                    {


                        //create Token
                        List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name,User.UserName),
                new Claim(ClaimTypes.MobilePhone,User.PhoneNumber)
                };
                        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Keys:JwtKey"]));
                        var signincredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var Token = new JwtSecurityToken(
                            claims: claims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: signincredentials
                            );
                        string secretToken = new JwtSecurityTokenHandler().WriteToken(Token);
                        return secretToken;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }
        public Task<bool> LogOut()
        {
            return Task.FromResult(true);

        }
    }
}
