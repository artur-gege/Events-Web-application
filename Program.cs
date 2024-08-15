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
using ModsenAPI.Application.MappingProfiles;
using ModsenAPI.Application.Validators;
using ModsenAPI.Infrastructure.Middleware;

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

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:5005";
                    options.Audience = "eventsApi";
                    options.RequireHttpsMetadata = false;
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
