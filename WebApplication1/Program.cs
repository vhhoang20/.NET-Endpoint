using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using WebApplication1.IdentityServer;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNetCore.Diagnostics;
using IdentityModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection String
var ConnectionString = builder.Configuration.GetConnectionString("ConnStr");

// EF
builder.Services.AddDbContext<APIDbContext>(options => options.UseSqlServer(ConnectionString));

// Identity
builder.Services.AddIdentity<User, IdentityRole>(option =>
{
    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 3;
    option.Password.RequiredUniqueChars = 0;
    option.Password.RequireLowercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
})
                .AddEntityFrameworkStores<APIDbContext>()
                .AddDefaultTokenProviders();


var jwtSecret = builder.Configuration["JwtSecret"];
builder.Services.AddSingleton(jwtSecret);

// Identity Server
builder.Services.AddIdentityServer()
        .AddAspNetIdentity<User>()
        .AddDeveloperSigningCredential()
        .AddInMemoryPersistedGrants()
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryIdentityResources(Config.GetIdentityResources())
        .AddInMemoryApiResources(Config.ApiResources)
        .AddInMemoryClients(Config.Clients);

// Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "admin"));
    options.AddPolicy("Customer", policy => policy.RequireClaim("Role", "customer"));
});

// Authentication
builder.Services.AddAuthentication("Bearer")
        .AddIdentityServerAuthentication(options =>
        {
            options.Authority = "http://localhost:5157";
            options.RequireHttpsMetadata = false;
            options.ApiName = "myApi";
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
