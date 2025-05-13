using System.Reflection;
using System.Text;
using AspProject.Api.Endpoints;
using AspProject.Configurations;
using AspProject.Configurations.Mapper;
using AspProject.Domain.Abstractions;
using AspProject.Domain.Abstractions.Auth;
using AspProject.Domain.Entities;
using AspProject.Domain.Services;
using AspProject.Infrastrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder
    {
        Host = "localhost",
        Port = 5555,
        Username = "postgres",
        Password = "20242424",
        Database = "ExamArchive"
    };
    string connectionString = builder.ConnectionString;
    optionsBuilder.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IMaterialsService, MaterialsService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Audience = builder.Configuration["AuthConfig:Audience"];
        options.ClaimsIssuer = builder.Configuration["AuthConfig:Issuer"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["AuthConfig:Issuer"],
            ValidAudience = builder.Configuration["AuthConfig:Audience"],
            RequireExpirationTime = true,
            RequireAudience = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AuthConfig:IssuerSignKey"]!)),
        };
    });
builder.Services.AddAuthorization();
builder.Services.Configure<AuthConfig>(builder.Configuration.GetSection("AuthConfig"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<AuthConfig>>().Value);

builder.Services.AddAutoMapper(expression =>
{
    expression.AddProfile<MaterialMapperProfile>();
    expression.AddProfile<StudentMappingProfile>();
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowFrontend");
app.UseRouting();
app.MapMaterials();
app.MapAddMaterialEndpoint();
app.MapAuthEndpoint();
app.MapGet("/", () => "Hello World!");

app.Run();

