using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Message
    {
       public int Id { get; set; }
       public string SenderId { get; set; }
        [ForeignKey(nameof(User))]
       public string RecieverId { get; set; }
       public string MessageContent { get; set; }
       public App_User User { get; set; }
       public DateTime Date { get; set; }
       public int read { get; set; }


    }
}
