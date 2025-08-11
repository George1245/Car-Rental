using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class UserConnection
    {
        public int Id { get; set; }
        public string ConnectionId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public App_User User { get; set; }


    }
}
