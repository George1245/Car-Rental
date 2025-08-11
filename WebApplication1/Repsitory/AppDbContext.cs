using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication1.Models;

namespace WebApplication1.Repsitory
{
    public class AppDbContext: IdentityDbContext<App_User>
    {
    
        public AppDbContext(DbContextOptions options  ) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed default role "User"
            string roleId = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = roleId,
                Name = "User",
                NormalizedName = "USER"
            });

            roleId = Guid.NewGuid().ToString();
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = roleId,
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
        }
        public virtual DbSet <Car>  Cars{ get; set; }
        public virtual DbSet <UserConnection>  userconnection{ get; set; }
        public virtual DbSet <Message>  message{ get; set; }
        public virtual DbSet <CarRent> Rents{ get; set; }
    }
}
