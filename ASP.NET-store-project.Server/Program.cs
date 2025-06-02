using ASP.NET_store_project.Server.Controllers.IdentityController;
using ASP.NET_store_project.Server.Data;
using ASP.NET_store_project.Server.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configures clients meant for communicating with Suppliers' external APIs
// All requests are handled through json
var configSupplier = (string uriAdress) => (HttpClient httpClient) =>
{
    httpClient.BaseAddress = new Uri(uriAdress);

    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/json");
};
// Supliers' request adresses
builder.Services.AddHttpClient("SupplierA", configSupplier("https://localhost:5173/api/supplier/[A]/"));
builder.Services.AddHttpClient("SupplierB", configSupplier("https://localhost:5173/api/supplier/[B]/"));
builder.Services.AddHttpClient("SupplierC", configSupplier("https://localhost:5173/api/supplier/[C]/"));

// Configures authentication/authorization method (JWT)
builder.Services.AddSingleton<TokenProvider>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!)),
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            ClockSkew = TimeSpan.Zero,
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.ContainsKey("Token"))
                    context.Token = context.Request.Cookies["Token"];
                return Task.CompletedTask;
            }
        };
    });

    builder.Services.AddAuthorizationBuilder()
    .AddPolicy(IdentityData.RegularUserPolicyName, policy =>
        policy.RequireClaim(IdentityData.RegularUserClaimName, "true"))
    .AddPolicy(IdentityData.AdminUserPolicyName, policy =>
        policy.RequireClaim(IdentityData.AdminUserClaimName, "true"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database configuration (this program uses postgres), it retrieves data from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("StoreDatabase")));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

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

app.MapFallbackToFile("/index.html");

app.Run();
