using MailKit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Repsitory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// ====== DbContext ======
builder.Services.AddDbContext<AppDbContext>(u =>
{
    u.UseSqlServer(builder.Configuration.GetConnectionString("PeterConnection"));
});

// ====== Identity ======
builder.Services.AddIdentity<App_User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// ====== Authentication (JWT + Google) ======
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie() // ÷—Ê—Ì ⁄·‘«‰ Google OAuth callback
.AddJwtBearer("JWT", opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Keys:JwtKey"])),
        ValidateAudience = false,
        ValidateIssuer = false
    };
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["GoogleAuth:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["GoogleAuth:ClientSecret"];
    googleOptions.CallbackPath = "/Account/callBack"; // Endpoint ·«” ﬁ»«· —œ Google
});

// ====== Authorization ======
builder.Services.AddAuthorization();

// ====== AutoMapper ======
builder.Services.AddAutoMapper(typeof(Program));

// ====== Custom Services & Repositories ======
builder.Services.AddScoped<App_User>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<CarRent>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IcustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<mailService>();

// ====== Swagger ======
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter: Bearer {your JWT token}"
    });

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
});

var app = builder.Build();

// ====== Middleware ======
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
