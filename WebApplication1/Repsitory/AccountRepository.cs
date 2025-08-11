
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Xml;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Repsitory
{
    public class AccountRepository : IAccountRepository
    {
        public UserManager<App_User> _userManager;

        public SignInManager<App_User> _signInManager;
        public RoleManager<IdentityRole> _roleManager;
       
        private readonly AppDbContext _appDbContext;
        public IConfiguration Configuration;
        private mailService _mailService;
        public AccountRepository(UserManager<App_User> _userManager, SignInManager<App_User> _signInManager, RoleManager<IdentityRole> _roleManager,AppDbContext appDbContext, IConfiguration _Configuration, mailService _mailService)
        { 
            this._userManager = _userManager;
            this._signInManager = _signInManager;   
            this._roleManager = _roleManager;   
            this._appDbContext = appDbContext;
            Configuration=_Configuration;
            this._mailService = _mailService;
        }   

      
        public async Task<bool> Register(userRegisterDTO userRegister)
        {

            if (userRegister != null && await _userManager.FindByNameAsync(userRegister.userName) == null)
            {
                var app_User = new App_User {
                 Email = userRegister.Email,
                   UserName = userRegister.userName,
                  PhoneNumber = userRegister.phoneNumber,
                };
                 
  
                IdentityResult userRegisterResult = await _userManager.CreateAsync(app_User,userRegister.Password);


                if (userRegisterResult.Succeeded)
                {
                    IdentityResult roleResult;
                    if (userRegister.userName == Configuration["AdminAccount:UserName"])
                    {
                         roleResult = await _userManager.AddToRoleAsync(app_User, "Admin");


                    }
                    else
                    {
                         roleResult = await _userManager.AddToRoleAsync(app_User, "User");

                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(app_User);
                        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                        string confirmationLink = $"{Configuration["serverRun:localHost"]}Account/confirmemail?userId={app_User.Id}&token={encodedToken}";
                        await _mailService.sendEmail(app_User.Email, "please open confirmation Link: " + confirmationLink, app_User.UserName, "Email Confirmation");
                    }
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
            
                if (userLogin.userName == Configuration["AdminAccount:UserName"] && userLogin.Password == Configuration["AdminAccount:Password"])
                {
                    string adminUserName = Configuration["AdminAccount:UserName"];
                    string adminEmail = Configuration["AdminAccount:Email"];
                    string adminPassword = Configuration["AdminAccount:Password"];
                    string adminPhoneNumber = Configuration["AdminAccount:PhoneNumber"];
                    await Register(new userRegisterDTO { userName = adminUserName, Email = adminEmail, Password = adminPassword, phoneNumber = adminPhoneNumber });
                }
            
          
                // Use only UserManager, not direct DbContext access
                var user = await _userManager.FindByNameAsync(userLogin.userName);
                if (user == null) return null;

                var passwordValid = await _userManager.CheckPasswordAsync(user, userLogin.Password);
                if (!passwordValid) return null;

                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

   
            return await generateToken(claims);
           
        }

        public async void LogOut()
        {
            await _signInManager.SignOutAsync();
           
        }

        public async Task<string> generateToken(List<Claim> Claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Keys:JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: Claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public void AddConnectionIdToUser(UserConnection userConnection)
        {
            _appDbContext.userconnection.Add(userConnection);
            _appDbContext.SaveChanges();
        }
        public void RemoveConnectionFromUser(string connectionid)
        {
            UserConnection userConnection = _appDbContext.userconnection.FirstOrDefault(x => x.ConnectionId == connectionid);
            _appDbContext.userconnection.Remove(userConnection);
            _appDbContext.SaveChanges();
        }
    }
}
