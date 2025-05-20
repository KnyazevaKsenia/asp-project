using System.Reflection;
using System.Text;
using AspProject.Api.Endpoints;
using AspProject.Configurations;
using AspProject.Configurations.Mapper;
using AspProject.Domain.Abstractions;
using AspProject.Domain.Abstractions.Auth;
using AspProject.Domain.Abstractions.IExamImitation;
using AspProject.Domain.Entities;
using AspProject.Domain.Services;
using AspProject.Domain.Services.ExamImitation;
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
builder.Services.AddAntiforgery();
string connectionString = builder.Configuration.GetConnectionString("DbConnection");

builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IMaterialsService, MaterialsService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();

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
            ValidateIssuerSigningKey = true, 
            ValidateLifetime = true, 
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
    expression.AddProfile<TicketMapperProfile>();
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();
app.UseCors("AllowFrontend");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapGet("/", () => "Server started");
app.MapMaterials();
app.MapAddMaterialEndpoint();
app.MapMyPageEndpoint();
app.MapExamImitEndpoint();
app.MapAuthEndpoint();
app.MapFavoriteEndpoints();

app.Run();

