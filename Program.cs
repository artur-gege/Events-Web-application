using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ModsenAPI.Application.Repositories.Implementations;
using ModsenAPI.Application.Repositories.Interfaces;
using ModsenAPI.Application.UseCases.Implementations;
using ModsenAPI.Application.UseCases.Interfaces;
using ModsenAPI.Application.UnitOfWorks;
using ModsenAPI.Infrastructure.Data;
using ModsenAPI.Application.Services.Implementations;
using ModsenAPI.Domain.ModelsDTO.ParticipantDTO;
using ModsenAPI.Domain.ModelsDTO.EventDTO;
using ModsenAPI.Domain.ModelsDTO.UserDTO;
using ModsenAPI.Application.MappingProfiles;
using ModsenAPI.Application.Validators;
using ModsenAPI.Infrastructure.Middleware;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.OpenApi.Models;

namespace ModsenAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMvc(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            //validators
            builder.Services.AddScoped<IValidator<EventRequestDto>, EventValidator>();
            builder.Services.AddScoped<IValidator<ParticipantRequestDto>, ParticipantValidator>();
            builder.Services.AddScoped<IValidator<UserRegisterRequestDto>, UserRegisterValidator>();
            builder.Services.AddScoped<IValidator<UserLoginRequestDto>, UserLoginValidator>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } }
                });
            });

            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, Domain.Enums.UserRole.admin.ToString()));
            });

            //automappers
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            //DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //Repositories
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
            
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            builder.Services.AddScoped<IEventUseCase, EventUseCase>();
            builder.Services.AddScoped<IParticipantUseCase, ParticipantUseCase>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            
            app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
