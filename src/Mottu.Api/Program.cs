using Microsoft.OpenApi.Models;
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


var builder = WebApplication.CreateBuilder(args);

var location = Assembly.GetExecutingAssembly().Location;

var directory = Path.GetDirectoryName(location);

var configurationPath = Path.Combine(directory, "appsettings.json");

builder.Configuration.AddJsonFile(configurationPath, optional: false, reloadOnChange: true);


builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

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

     options.AddPolicy("AdminOrDeliverymanPolicy", policy =>
            policy.RequireAssertion(context =>
                context.User.IsInRole(UserRole.Admin.ToString()) ||
                context.User.IsInRole(UserRole.Deliveryman.ToString())
            ));
});

builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mottu API V1");
        c.RoutePrefix = "swagger"; 
        
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