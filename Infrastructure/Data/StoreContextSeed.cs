using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{

    public static async Task SeedAsync(StoreContext context){

            if(!context.Products.Any())
            {
                var proudctsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(proudctsData);

                if(products is null)
                return ;

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

            if(!context.DeliveryMethods.Any())
            {
                var dmData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData); 

                if(methods is null) return ;

                context.DeliveryMethods.AddRange(methods);
                await context.SaveChangesAsync();
            }

             

    }

}
