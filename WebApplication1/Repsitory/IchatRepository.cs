using WebApplication1.Models;

namespace WebApplication1.Repsitory
{
    public interface IchatRepository
    {
        public void StoreMessage(Message message);
    }
}
