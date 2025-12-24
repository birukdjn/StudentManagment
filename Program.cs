using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi; 
using StudentManagment.Data;
using StudentManagment.Services;
using StudentManagment.Services.Interfaces;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;


// --- 1. Database and Service Registrations ---

var connectionString = configuration.GetConnectionString("DefaultConnection");

services.AddDbContext<SchoolContext>(options =>
{
    options.UseMySQL(
        connectionString,
        optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(Program).Assembly.FullName)
    );
});

services.AddScoped<IDepartmentService, DepartmentService>();
services.AddScoped<ICourseService, CourseService>();
services.AddScoped<IEnrollmentService, EnrollmentService>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<ITokenService, TokenService>();
services.AddScoped<ICafeAccessService, CafeAccessService>();


// Example ASP.NET Core Program.cs CORS setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- 2. JWT Authentication Configuration ---

// Add Authentication services
services.AddAuthentication(options =>
{
    // Use JWT Bearer as the default scheme for authentication and challenge
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure JWT Bearer to validate tokens
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Require the token to be signed
        ValidateIssuerSigningKey = true,
        // The key used to sign and verify the token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!)),

        // Validate the issuer (who created the token)
        ValidateIssuer = true,
        ValidIssuer = configuration["Jwt:Issuer"],

        // Validate the audience (who the token is intended for)
        ValidateAudience = true,
        ValidAudience = configuration["Jwt:Audience"],

        // Validate the expiration time of the token
        ValidateLifetime = true,

        // Allow a small clock skew (tolerance for time synchronization issues)
        ClockSkew = TimeSpan.Zero
    };
});

// --- 3. Controller, OpenAPI, and Swagger Configurations ---

services.AddControllers()
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
services.AddEndpointsApiExplorer();

// Configure Swagger to include Authorization support
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "StudentManagment API", Version = "v1" });

    // Define the security scheme (Bearer Token)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
    });

});


var app = builder.Build();


// --- 4. HTTP Request Pipeline ---

// UseAuthentication MUST come before UseAuthorization
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("v1/swagger.json", "StudentManagment V1");
        // This makes the "Authorize" button work immediately
        options.EnablePersistAuthorization();
    });
}
app.UseHttpsRedirection();

app.MapControllers();

app.Run();