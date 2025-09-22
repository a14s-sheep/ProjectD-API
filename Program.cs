using System.Text;
using FluentValidation;
using ProjectD_API.Data;
using ProjectD_API.Email;
using ProjectD_API.Middlewares;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


/// Services
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", build => build
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetIsOriginAllowed(_ => true)
        .Build()
    );
});

// Email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers().AddNewtonsoftJson();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();


/// DbContext & Identity
// MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GameDBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

//// Identity
//builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<GameDBContext>()
//    .AddDefaultTokenProviders();

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT Key is not configured. Please set it on appsettings.json");
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    }
);

builder.Services.AddAuthorization();

// Swagger
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme,
                    }
                },
                new string[] {}
            }
        });
    });
}


var app = builder.Build();


/// DB Seeds
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GameDBContext>();
    db.Database.Migrate();

    //var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    //foreach (var roleName in RoleManager.All)
    //{
    //    if (!await roleManager.RoleExistsAsync(roleName))
    //        await roleManager.CreateAsync(new IdentityRole(roleName));
    //}
}


/// Middlewares
app.UseCors("CorsPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"));
    app.UseDeveloperExceptionPage();
}

// Add middleware
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();