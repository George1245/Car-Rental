using Microsoft.AspNetCore.SignalR.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApplication1.Services
{
    public class ConnectionService
    {
        private HubConnection connection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ConnectionService(IHttpContextAccessor _httpContextAccessor)
        { 
        this._httpContextAccessor = _httpContextAccessor;
        }

        public List<Claim> GetClaimsFromJwt(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();

            
            var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

            var claims = jwtSecurityToken.Claims.ToList();

            return claims;
        }
        public async Task <bool> EstablishConnection(string reciverid,string message,string sender_id)
        {

            string tokenHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            string token = tokenHeader.StartsWith("Bearer ") ? tokenHeader.Substring(7) : tokenHeader;
            connection = new HubConnectionBuilder()
             .WithUrl("http://localhost:5201/ChatHub", options =>
             {
                 options.AccessTokenProvider = () => Task.FromResult(token);
             })
             .WithAutomaticReconnect()
             .Build();


           
            connection.On<string, string>("RecieveMessage", (senderId, message) =>
            {
               
                Console.WriteLine($"{senderId}: {message}");
            });
            try
            {
                    
                    await connection.StartAsync();
                    Console.WriteLine("✅ Connected to the hub");

                    await connection.InvokeAsync("sendmessage", message, reciverid);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                return false;
                }
            
            return true;
            
        }
    }
}
