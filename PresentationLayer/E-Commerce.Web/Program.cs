
using E_Commerce.Domain.Contracts;
using E_Commerce.Persistence.Data.DataSeed;
using E_Commerce.Persistence.DbContexts;
using E_Commerce.Persistence.Repository;
using E_Commerce.Services;
using E_Commerce.Services.Profiles;
using E_Commerce.Services_Abstraction;
using E_Commerce.Web.CustomMiddleWares;
using E_Commerce.Web.Extensions;
using E_Commerce.Web.Factories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Create Host And Add services to the container.
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); 

            builder.Services.AddScoped<IDataInatializer,DataInatializer>();
            builder.Services.AddAutoMapper(typeof(ServicesAsssemblyReference).Assembly);
           
            builder.Services.AddSingleton<IConnectionMultiplexer>(SP =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationResponse;
            });
            
            #endregion 

            var app = builder.Build();

            #region Data Seeding
            await app.MigrateDatabaseAsync();
            await app.SeedDatabaseAsync();
            #endregion


            #region Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionHandlerMiddleWare>(); 

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllers();
            

            #endregion
            await app.RunAsync();
        }
    }
}
