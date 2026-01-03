using DAL.ApplicationContext;
using DAL.Interfaces;
using DAL.Repositories;
using DotNetCoreWithIdentityServer.Models;
using Duende.IdentityServer.Models;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Start - add Identity and Duende identity server
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer(options =>
{
    options.EmitStaticAudienceClaim = true;
    options.KeyManagement.Enabled = false;
})
.AddAspNetIdentity<ApplicationUser>()
.AddInMemoryApiScopes(IdentityConfig.ApiScopes)
.AddInMemoryClients(IdentityConfig.Clients)
.AddDeveloperSigningCredential();

// END - add Identity and Duende identity server

// implement jwt

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options => 
//    {
//        options.Authority = "https://localhost:5001";
//        options.Audience = "myapi"; // match the ApiScope
//        options.RequireHttpsMetadata = false; // set false only in dev
//    });

// end of implement jwt

// Add services to the container.
builder.Services.AddControllers();

// implement NSwag for API documentation
builder.Services.AddOpenApiDocument();


// register custom services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IAccountServices, AccountServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseOpenApi();      // /swagger/v1/swagger.json
app.UseSwaggerUi();    // /swagger

// Redoc HTML endpoint at /redoc
app.MapGet("/redoc", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(@"
<!DOCTYPE html>
<html>
  <head>
    <title>API Docs - Redoc</title>
  </head>
  <body>
    <redoc spec-url='/swagger/v1/swagger.json'></redoc>

    <!-- Load Redoc from CDN -->
    <script src=""https://cdn.redoc.ly/redoc/latest/bundles/redoc.standalone.js""></script>
  </body>
</html>");
});

app.UseHttpsRedirection();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();