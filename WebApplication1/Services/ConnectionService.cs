using Microsoft.AspNetCore.SignalR.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApplication1.Services
{
    public class ConnectionService
    {
        private HubConnection connection;

        public List<Claim> GetClaimsFromJwt(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();

            
            var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

            var claims = jwtSecurityToken.Claims.ToList();

            return claims;
        }
        public async Task <bool> EstablishConnection(string reciverid,string message,string sender_id)
        {


            connection = new HubConnectionBuilder().WithUrl("https://localhost:44383/ChatHub").WithAutomaticReconnect().Build();


           

                try
                {
                    
                    await connection.StartAsync();
                    Console.WriteLine("✅ Connected to the hub");

                    await connection.InvokeAsync("sendmessage", message, reciverid, sender_id);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                return false;
                }
             connection.On<string, string>("RecieveMessage", (userId, message) =>
                {
                    Console.WriteLine($"{userId}: {message}");
                });
            return true;
            
        }
    }
}
