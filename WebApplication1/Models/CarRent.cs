using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace WebApplication1.Models
{
    public class CarRent
    {
        
        public int Id { get; set; }

        [ForeignKey("App_User")]
        public string UserId { get; set; }
        public App_User User { get; set; }

        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car car { get; set; }

        public float RentalDuration { get; set; }
        //Rent  Request Status  
        public string requestStatus {  get; set; }
        public double totalCost {  get; set; }
  

    }
}
