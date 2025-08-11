using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Tls;
using System.Security.Claims;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Repsitory;
using WebApplication1.Services;

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

        public async Task SendMessage(string message, string receiverId,string SenderId )
        {
            var senderId = SenderId;

           
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

            if (context.Request.Cookies.ContainsKey("YourAppAuthCookie"))
            {
                claims = connectionservice.GetClaimsFromJwt(context.Request.Cookies["YourAppAuthCookie"]);
            }
            
            
            _userConnection.ConnectionId = Context.ConnectionId;
            _userConnection.UserId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


            _accountRepository.AddConnectionIdToUser(_userConnection);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
             _accountRepository.RemoveConnectionFromUser(Context.ConnectionId);

            await base.OnDisconnectedAsync(exception);
        }
    }

}
