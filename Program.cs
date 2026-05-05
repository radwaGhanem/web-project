using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using WebApplication1.Data;
using WebApplication1.filters;
using WebApplication1.Middlewares;
using WebApplication1.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => { options.Filters.Add<LogActivityFilter>(); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT token with Bearer prefix, e.g. Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var jwtKey = builder.Configuration["JwtSettings:Key"] ?? "ThisIsASecretKeyForJwtTokenGeneration123!";
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "TaskApi";
var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? "TaskApiUsers";
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
IssuerSigningKey = signingKey,
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<ApplicationDB>(options => options.UseSqlServer(
    "Server=(localdb)\\MSSQLLocalDB;Database=task;Trusted_Connection=True;MultipleActiveResultSets=true"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDB>();
    db.Database.EnsureCreated();

    if (!db.users.Any())
    {
        var adminUser = new AppUser
        {
            Name = "Admin User",
            Email = "admin@task.local",
            Role = "Admin",
            PasswordHash = AuthService.HashPassword("Admin123!"),
            Profile = new UserProfile
            {
                Bio = "System administrator",
                PhoneNumber = "0000000000"
            }
        };

        var regularUser = new AppUser
        {
            Name = "Task User",
            Email = "user@task.local",
            Role = "User",
            PasswordHash = AuthService.HashPassword("User123!"),
            Profile = new UserProfile
            {
                Bio = "Task manager user",
                PhoneNumber = "1112223333"
            }
        };

        db.users.AddRange(adminUser, regularUser);
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<ProfilingMiddleware>();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseMiddleware<CorsHeaderMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
