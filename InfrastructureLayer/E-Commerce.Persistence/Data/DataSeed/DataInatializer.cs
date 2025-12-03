using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Data.DataSeed
{
    public class DataInatializer : IDataInatializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInatializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task InatializeAsync()
        {

            try
            {
                var HasProducts = await _dbContext.Products.AnyAsync();
                var HasBrands = await _dbContext.ProductBrands.AnyAsync();
                var HasTypes = await _dbContext.ProductTypes.AnyAsync();
                var HasDelivery = await _dbContext.Set<DeliveryMethod>().AnyAsync();


                if (HasProducts && HasTypes && HasBrands && HasDelivery) return;

                if(!HasBrands)
                    await SeedDataFromJson<ProductBrand, int>("brands.json", _dbContext.ProductBrands);
                

                if (!HasTypes)
                    await SeedDataFromJson<ProductType, int>("types.json", _dbContext.ProductTypes);

                _dbContext.SaveChanges();

                if(!HasProducts)
                    await SeedDataFromJson<Product, int>("products.json", _dbContext.Products);

                if(!HasDelivery)
                    await SeedDataFromJson<DeliveryMethod, int>("delivery.json", _dbContext.Set<DeliveryMethod>());
                
                
                await _dbContext.SaveChangesAsync();



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data Seeding Failed : {ex}");
            }


        }



        private async Task SeedDataFromJson<T,TKey>(string FileName,DbSet<T> DbSet) where  T : BaseEntity<TKey>
        {
            var FilePath = @"..\..\InfrastructureLayer\E-Commerce.Persistence\Data\DataSeed\JsonFiles\" + FileName;

            if (!File.Exists(FilePath)) throw new FileNotFoundException($"File {FileName} is not Exists");
            try
            {
                var DataStream = File.OpenRead(FilePath);

                var Data = await JsonSerializer.DeserializeAsync<List<T>>(DataStream, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                });

                if (Data is not null)
                    await DbSet.AddRangeAsync(Data);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error While Reading File Json : {ex}");
            }

        }
    }
}
