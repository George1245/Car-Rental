using MimeKit;
using MailKit.Net.Smtp;

namespace WebApplication1.Services
{
    public class mailService
    {
        IConfiguration _config;
        public mailService(IConfiguration configuration)
        {
            _config=configuration;
        }
        public async Task<bool> sendEmail(string email,string message,string recieverName,string subject)
        {
            var Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Admin", _config["AdminAccount:Email"].ToString()));
            Message.To.Add(new MailboxAddress(recieverName, email));
            Message.Subject = subject;
            Message.Body = new TextPart("plain")
            {
                Text = message
            };

            using var client = new SmtpClient();
            client.Connect(_config["EmailSettings:SmtpServer"], int.Parse(_config["EmailSettings:SmtpPort"]), false);
            client.Authenticate(_config["EmailSettings:SenderEmail"], _config["EmailSettings:SenderPassword"]);
           string checkSend= client.Send(Message);
            if (checkSend != null)
            {
                return true;
            }
            else
            {
                return false;
            }
                client.Dispose();
        }
    }
}
