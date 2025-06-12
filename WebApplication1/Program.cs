using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Repsitory;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(u =>
{
    u.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<App_User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(option => option.DefaultAuthenticateScheme = "JWT").AddJwtBearer(opt =>
{
    opt.TokenValidationParameters=new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        IssuerSigningKey= new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Keys:JwtKey"])),
        ValidateAudience=false,
        ValidateIssuer=false
    };
});

builder.Services.AddScoped<App_User>();

builder.Services.AddScoped<IAccountRepository,AccountRepository>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
