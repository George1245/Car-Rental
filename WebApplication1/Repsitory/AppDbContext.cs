using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication1.Models;

namespace WebApplication1.Repsitory
{
    public class AppDbContext: IdentityDbContext<App_User>
    {
    
        public AppDbContext(DbContextOptions options) :base(options)
        {

        }

        public virtual DbSet <Car>  Cars{ get; set; }
        public virtual DbSet <CarRent> Rents{ get; set; }
    }
}
