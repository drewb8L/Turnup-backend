using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Turnup;
using Turnup.Configurations;
using Turnup.Context;
using Turnup.Entities;
using Turnup.Services;
using Turnup.Services.AuthService;
using Turnup.Services.CartService;
using Turnup.Services.EstablishmentService;
using Turnup.Services.OrderService;
using Turnup.Services.ProductService;
using Turnup.Services.ScanService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// uncomment if on windows
//var dbPath = Path.Join(Directory.GetCurrentDirectory(), "turnup.db");
//var conn = new SqliteConnection($"Data Source=C:\\turnupapi\\turnup.db");

builder.Services.AddDbContext<TurnupDbContext>(options =>
{
    // comment if on mac or linux
    options.UseSqlServer("name=DefaultConnection");
    //uncomment if on windows
    //options.UseSqlite(conn);

});

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        
    })
    .AddJwtBearer(jwt =>
    {
        var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);
        
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, // TODO: switch to true in deployed env!
            ValidateAudience = false, //Only in dev
            RequireExpirationTime = false, // Only dev
            // TODO: refresh tokens and set exp
            ValidateLifetime = true,
            
            
        };
    });

builder.Services.AddIdentity<AuthUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        
    })
    .AddEntityFrameworkStores<TurnupDbContext>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "JWTToken_Auth_API", Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciO...\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    //c.SchemaFilter<EnumSchemaFilter>();
});


    builder.Services.AddCors(o =>
    o.AddPolicy("AllowAll", a => 
        a.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));


builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IEstablishmentService, EstablishmentService>();
builder.Services.TryAddScoped<IScanService, ScanService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
   app.UseSwagger();
   app.UseSwaggerUI();
   app.UseHttpsRedirection();

//}

//app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();