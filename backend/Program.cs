using backend.Data;
using backend.Filters;
using backend.Repositories;
using backend.Services;
using backend.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(options =>
{
    options.Filters.Add<SessionManagementFilter>();
    options.Filters.Add<PermissionFilter>();
});

// Database Context
builder.Services.AddScoped<IDatabaseContext, DatabaseContext>();

builder.Services.AddScoped<SessionManagementFilter>();
// Repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();

builder.Services.AddScoped<IUserManagementRepository, UserManagementRepository>();
builder.Services.AddScoped<ILevelRepository, LevelRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IAcademicProgramRepository, AcademicProgramRepository>();

builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<IProfilePictureRepository, ProfilePictureRepository>();
//builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseOfferingRepository, CourseOfferingRepository>();
builder.Services.AddScoped<ITeacherAssignmentRepository, TeacherAssignmentRepository>();
builder.Services.AddScoped<IPrerequisiteRepository, PrerequisiteRepository>();
builder.Services.AddScoped<ICreditLimitPolicyRepository, CreditLimitPolicyRepository>();
builder.Services.AddScoped<ICreditLimitOverrideRepository, CreditLimitOverrideRepository>();
builder.Services.AddScoped<ICourseHistoryRepository, CourseHistoryRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();



// Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();


builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<ILevelService, LevelService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IAcademicProgramService, AcademicProgramService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<IProfilePictureRepository, ProfilePictureRepository>();
//builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseOfferingService, CourseOfferingService>();
builder.Services.AddScoped<ITeacherAssignmentService, TeacherAssignmentService>();

builder.Services.AddScoped<IPrerequisiteService, PrerequisiteService>();
builder.Services.AddScoped<ICreditLimitPolicyService, CreditLimitPolicyService>();
builder.Services.AddScoped<ICreditLimitOverrideService, CreditLimitOverrideService>();
builder.Services.AddScoped<ICourseHistoryService, CourseHistoryService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

// Utilities
builder.Services.AddSingleton<IJwtHandler, JwtHandler>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IImageProcessor, ImageProcessor>();
builder.Services.AddSingleton<IJsonFileProcessor, JsonFileProcessor>();

// Authentication - Modified configuration
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        // Add these crucial lines:
        NameClaimType = "id", // Maps to your "id" claim
        RoleClaimType = "profile" // Maps to your "profile" claim instead of standard Role claim

      
    };

    // Move the event handler inside here
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var authRepository = context.HttpContext.RequestServices.GetRequiredService<IAuthRepository>();

            var jti = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);
            if (string.IsNullOrEmpty(jti))
            {
                context.Fail("Token is missing JTI claim");
                return;
            }

            var isRevoked = await authRepository.CheckSessionRevoked(jti);
            if (isRevoked)
            {
                context.Fail("Token has been revoked");
            }
        }
    };
});

// Swagger configuration remains the same
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LMS API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token **only**. Don't include the 'Bearer' prefix."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();