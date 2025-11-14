using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MovieExplorer.Config;
using proyecto_prog4.Config;
using proyecto_prog4.Repositories;
using proyecto_prog4.Services;
using proyecto_prog4.Utils;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Cinedex",
        Description = "Biblioteca de peliculas",
        TermsOfService = new Uri("http://cinedex.com"),
    });
    options.AddSecurityDefinition("Token", new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Description = "Json Web Token, Authorization header using the Bearer scheme.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Name = "Authorization",
        Scheme = "bearer"
    });
    options.OperationFilter<AuthOperationFilter>();
});

// Services
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<MovieServices>();
builder.Services.AddScoped<GenresServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<IEncoderServices, EncoderServices>();
builder.Services.AddScoped<RolServices>();
builder.Services.AddScoped<FavoritesServices>();

// Repositories
builder.Services.AddScoped<IMovieRepository ,MovieRepository>();
builder.Services.AddScoped<IUserRepository ,UserRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();

// AutoMapper
builder.Services.AddAutoMapper(opts => { }, typeof(Mapping));

// Database
var connectionString = builder.Configuration.GetConnectionString("devConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// JWT
var jwtSecret = builder.Configuration.GetSection("Secrets")?.GetSection("JWT")?.Value?.ToString() ?? null!;
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    var key = Encoding.UTF8.GetBytes(jwtSecret);
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = "movieapp",
        ValidateAudience = true,
        ValidAudience = "movieapp-users",
        ValidateLifetime = true
    };
}).AddCookie(opts =>
{
    opts.Cookie.HttpOnly = true;
    opts.Cookie.SameSite = SameSiteMode.None;
    opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    opts.ExpireTimeSpan = TimeSpan.FromDays(1);
});

var app = builder.Build();


// CORS
app.UseCors(opts =>
{
    opts.AllowAnyMethod();
    opts.AllowAnyHeader();
    opts.WithOrigins("http://localhost:5173");
    opts.AllowCredentials();
});

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
