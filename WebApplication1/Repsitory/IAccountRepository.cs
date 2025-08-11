using System.Security.Claims;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repsitory
{
    public interface IAccountRepository
    {
        public Task<bool> Register(userRegisterDTO userRegister);
        public Task<string> LogIn(UserLoginDTO userLogin);
        public Task<string> generateToken(List<Claim> Claims);
        public void AddConnectionIdToUser(UserConnection userConnection);
        public void RemoveConnectionFromUser(string connectionid);
        public  void LogOut();

    }
}
