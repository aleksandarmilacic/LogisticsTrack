using AutoMapper;
using LogisticsTrack.Database;
using LogisticsTrack.Database.Repository;
using LogisticsTrack.Service;
using LogisticsTrack.Service.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
namespace LogisticsTrack.API
{
    public class Program
    {

        public static void AddServices(IServiceCollection services)
        {
            var asm = typeof(GroundBaseService).Assembly;
            var servicesList = asm.GetTypes().Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(GroundBaseService)));
            foreach (var type in servicesList)
            {
                services.AddScoped(type);
            }
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.AddControllers();
            builder.Services.AddDbContext<LogisticsContext>(options =>
            {
                options.UseInMemoryDatabase("LogisticsTrackDb"); // we use an in-memory database for the sake of simplicity
            });
            builder.Services.AddScoped(typeof(Repository<>));
            // AutoMapper configuration
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<ColumnMappingService>();
            // add services generically
            AddServices(builder.Services);
            builder.Services.ConfigureHttpJsonOptions(opt => { opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; }); // we need to configure the JSON options to ignore cycles - potential infinite self-referencing loops
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            ValidationExtensions.Configure(app.Services); // we need to call this method to configure the validation extensions
            app.Run();
        }
    }
}
