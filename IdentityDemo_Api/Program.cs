using IdentityDemo_Api.Models;
using IdentityDemo_Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(option =>
                    option.UseSqlServer(
                        builder.Configuration
                            .GetConnectionString("DefoultConnection")));
// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 5;

        }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(auth => 
         {
               auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         }).AddJwtBearer(options => {
             options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidAudience = builder.Configuration.GetConnectionString("AuthSettings:Audience")/*["AuthSettings:Audience"]*/,
                 ValidIssuer = builder.Configuration.GetConnectionString("AuthSettings:Issuer")/*["AuthSettings:Issuer"]*/,
                 RequireExpirationTime = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetConnectionString("AuthSettings:Key")/*["AuthSettings:Key"]*/)),
                 ValidateIssuerSigningKey = true
             };
         });
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IMailServise, MailGridServise>();


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
