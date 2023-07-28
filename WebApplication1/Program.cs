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

var jwtSecret = builder.Configuration["JwtSecret"];
builder.Services.AddSingleton(jwtSecret);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "http://localhost:5157";
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.Audience = "myApi";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };

    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CustomPolicy", policy => policy.RequireAuthenticatedUser());
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
})
        .AddAspNetIdentity<User>()
        .AddDeveloperSigningCredential()
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryIdentityResources(Config.GetIdentityResources())
        .AddInMemoryApiResources(Config.ApiResources)
        .AddInMemoryClients(Config.Clients);

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
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers().RequireAuthorization("CustomPolicy");
});

app.Run();
