using Microsoft.OpenApi.Models;
using MediatR;
using FluentValidation.AspNetCore;
using System.Reflection;
using Mottu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Mottu.Infrastructure;
using Mottu.Application.Common.Interfaces;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Mottu.Domain.Entities;
using Mottu.Application.Interfaces;
using Mottu.Application.Services;
using RabbitMQ.Client;


var builder = WebApplication.CreateBuilder(args);



// builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// Obtém o diretório onde o assembly está localizado
var location = Assembly.GetExecutingAssembly().Location;
var directory = Path.GetDirectoryName(location);

// Constroi o caminho completo para o appsettings.json
var configurationPath = Path.Combine(directory, "appsettings.json");

// builder.Configuration.AddJsonFile("src/Mottu.Api/appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile(configurationPath, optional: false, reloadOnChange: true);


builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddHttpClient();

builder.Services.AddApplicationServices();

var connectionString = builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Mottu API", 
        Version = "v1",
        Description = "API do MeuProjeto",
    });

    c.EnableAnnotations();

     c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }});
});

// Configurar RabbitMQ
// builder.Services.AddSingleton(sp =>
// {
//     var factory = new ConnectionFactory()
//     {
//         HostName = "localhost",
//         UserName = "user",
//         Password = "password",
//         Port = 15672
//     };
//     return factory.CreateConnection();
// });


var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole(UserRole.Admin.ToString()));
    options.AddPolicy("DeliverymanPolicy", policy => policy.RequireRole(UserRole.Deliveryman.ToString()));
});

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configurar o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
    // Ativar o Swagger somente em desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mottu API V1");
        c.RoutePrefix = "swagger"; // Swagger disponível na raiz (url base)
        
    });
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();
app.MapControllers();
app.Run();