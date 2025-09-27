using Lifenix.API.Data;
using Lifenix.API.Data.Common;
using Lifenix.API.Data.Common.Repositories;
using Lifenix.API.Data.Models;
using Lifenix.API.Data.Repositories;
using Lifenix.API.Data.Seeding;
using Lifenix.API.Services;
using Lifenix.API.Services.Interfaces;
using Lifenix.API.Services.Mapping;
using Lifenix.API.Web.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ICookieService, CookieService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IDbQueryRunner, DbQueryRunner>();

builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>

{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context => 
        {
            if (context.Request.Cookies.ContainsKey("jwt")) 
            {
                context.Token = context.Request.Cookies["jwt"];
            }

            return Task.CompletedTask;
        }
    };
})
.AddGoogle("Google", opt =>
{
    opt.ClientId = builder.Configuration["Google:ClientId"];
    opt.ClientSecret = builder.Configuration["Google:ClientSecret"];
    opt.CallbackPath = "/signin-google";
});


var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
}

AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();