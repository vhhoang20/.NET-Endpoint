using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using WebApplication1.IdentityServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var ConnectionString = builder.Configuration.GetConnectionString("ConnStr");

// EF
builder.Services.AddDbContext<APIDbContext>(options => options.UseSqlServer(ConnectionString));

// Identity
builder.Services.AddIdentity<User, IdentityRole>() // để cho nó dùng được UserManger và roleManager
                .AddEntityFrameworkStores<APIDbContext>()
                .AddDefaultTokenProviders();

// add IdentiyServer
builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "IdentityServer.Cookie";
    config.LoginPath = "/Authentication/Login";
    config.LogoutPath = "/Authentication/Logout";
});

builder.Services.AddIdentityServer()
        .AddDeveloperSigningCredential()
        .AddInMemoryApiScopes(Config.GetApiScopes())
        .AddInMemoryIdentityResources(Config.GetIdentityResources())
        .AddInMemoryClients(Config.GetClients());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseIdentityServer();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
