
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
        public IConfiguration Configuration;
        public AccountRepository(UserManager<App_User> _userManager, SignInManager<App_User> _signInManager, RoleManager<IdentityRole> _roleManager,AppDbContext appDbContext, IConfiguration _Configuration)
        { 
            this._userManager = _userManager;
            this._signInManager = _signInManager;   
            this._roleManager = _roleManager;   
            this._appDbContext = appDbContext;
            Configuration=_Configuration;
        }   

      
        public async Task<bool> Register(userRegisterDTO userRegister)
        {

            if (userRegister != null)
            {
                var app_User = new App_User {
                 Email = userRegister.Email,
                   UserName = userRegister.userName,
                  PhoneNumber = userRegister.phoneNumber,
                };
                IdentityResult userRegisterResult = await _userManager.CreateAsync(app_User,userRegister.Password);
                if (userRegisterResult.Succeeded)
                {
                    
                   IdentityResult roleResult = await _userManager.AddToRoleAsync(app_User,"User");
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
        //
        public async Task<string> LogIn(UserLoginDTO userLogin)
        {
            if (userLogin == null) return null;

            // Use only UserManager, not direct DbContext access
            var user = await _userManager.FindByNameAsync(userLogin.userName);
            if (user == null) return null;

            var passwordValid = await _userManager.CheckPasswordAsync(user, userLogin.Password);
            if (!passwordValid) return null;

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? "")
    };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Keys:JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<bool> LogOut()
        {
            return Task.FromResult(true);

        }
    }
}
