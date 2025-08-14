using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Tls;
using System.Security.Claims;
using System.Text;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Repsitory;
using WebApplication1.Services;
using Newtonsoft.Json;
namespace WebApplication1.Hubs
{
    public class ChatHub : Hub
    {
        public IchatRepository _chatRepository;
        public AppDbContext _context;
        public UserConnection _userConnection;
        public IAccountRepository _accountRepository;
        public ConnectionService connectionservice;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ChatHub(IchatRepository chatRepository, AppDbContext context, UserConnection userConnection, IAccountRepository accountRepository, ConnectionService connectionservice, IHttpContextAccessor _httpContextAccessor)
        {
            _chatRepository = chatRepository;
            _context = context;
            _userConnection = userConnection;
            _accountRepository = accountRepository;
            this.connectionservice = connectionservice;
            this._httpContextAccessor = _httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            List<Claim> claims = null;

            string tokenHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            string token = tokenHeader.StartsWith("Bearer ") ? tokenHeader.Substring(7) : tokenHeader;
            claims = connectionservice.GetClaimsFromJwt(token);

           return claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        }
        public async Task SendMessage(string message, string receiverId)
        {
            string senderId = GetCurrentUserId();
            UserConnection ReciverUserId = _context.userconnection.FirstOrDefault(u => u.UserId == receiverId);
            if (ReciverUserId != null)
                await Clients.User(receiverId).SendAsync("RecieveMessage", senderId, message);
         


                var newMessage = new Message
                {
                    SenderId = senderId,
                    RecieverId = receiverId,
                    MessageContent = message,
                    Date = DateTime.Now
                };

                _chatRepository.StoreMessage(newMessage);
            
        }

        public override async Task OnConnectedAsync()
        {
            var context = _httpContextAccessor.HttpContext;
            var cookies = context.Request.Cookies;
            foreach (var c in cookies)
            {
                Console.WriteLine($"Cookie: {c.Key} = {c.Value}");
            }
            List<Claim> claims=null;

            string UserId = GetCurrentUserId();
            _userConnection.ConnectionId = Context.ConnectionId;
            _userConnection.UserId = UserId;

            _accountRepository.AddConnectionIdToUser(_userConnection);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
             _accountRepository.RemoveConnectionFromUser(Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<string> SendMessageToBot(string message)
        {
            
            string UserId = GetCurrentUserId();
            var webhookUrl = $"https://n8n-qyhxkclo.us-west-1.clawcloudrun.com/webhook/chat?message={message}&UserId={UserId}";

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(
                    JsonConvert.SerializeObject(new { text = message }),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await httpClient.GetAsync(webhookUrl);
                var responseString = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    return responseString;
                }
                else
                {
                    return "error has happenned";
                }
            }
        }

    }

}
