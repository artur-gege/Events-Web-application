using Microsoft.Extensions.DependencyInjection;
using ModsenAPI.Data;
using Microsoft.EntityFrameworkCore;
using ModsenAPI.Repositories.Interfaces;
using ModsenAPI.Repositories.Implementations;
using ModsenAPI.Services.Interfaces;
using ModsenAPI.Services.Implementations;
using ModsenAPI.UnitOfWorks;
using ModsenAPI.AutoMappers;
using ModsenAPI.Validators;
using FluentValidation;
using ModsenAPI.ModelsDTO;
using ModsenAPI.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;


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
            builder.Services.AddScoped<IValidator<EventDto>, EventValidator>();
            builder.Services.AddScoped<IValidator<ParticipantDto>, ParticipantValidator>();

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
            
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IParticipantService, ParticipantService>();


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
