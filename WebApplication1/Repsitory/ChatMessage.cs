using WebApplication1.Models;

namespace WebApplication1.Repsitory
{
    public class ChatMessage : IchatRepository
    {
        public AppDbContext app_context;
        public ChatMessage(AppDbContext app_context)
        {
            this.app_context = app_context;
        }
        public void StoreMessage(Message message)
        {
            app_context.message.Add(message);
            app_context.SaveChanges();
        }
    }
}
