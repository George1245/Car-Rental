using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


using Microsoft.Extensions.Configuration;

using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Repsitory;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(u =>
{
    u.UseSqlServer(builder.Configuration.GetConnectionString("PeterConnection"));
});

builder.Services.AddIdentity<App_User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme="JWT";
    option.DefaultChallengeScheme= "JWT";
    option.DefaultScheme= "JWT";
}

).AddJwtBearer("JWT", opt =>
{
    opt.TokenValidationParameters=new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        IssuerSigningKey= new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Keys:JwtKey"])),
        ValidateAudience=false,
        ValidateIssuer=false
    };
});
builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<App_User>();
builder.Services.AddScoped<ICarRepository,CarRepository>();
builder.Services.AddScoped<CarRent>();

builder.Services.AddScoped<IAccountRepository,AccountRepository>();
builder.Services.AddScoped<IcustomerRepository,CustomerRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // 2a. Define the security scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter: Bearer {your JWT token}"
    });

    // 2b. Apply globally (so the Authorize button works)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
            {
              Type = ReferenceType.SecurityScheme,
              Id = "Bearer"
            }
          },
          new string[] { }
        }
    });

    // If using Swashbuckle.Filters for finer control per-operation:
    // c.OperationFilter<SecurityRequirementsOperationFilter>();
});
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
