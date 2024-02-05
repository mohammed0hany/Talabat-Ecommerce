using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public class StoreContextSeed
    {
       public async static Task SeedAsync(StoreContext _dbcontext)
        {
            if (_dbcontext.ProductBrands.Count() == 0)
            {
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                if (Brands is not null && Brands.Count > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        _dbcontext.Set<ProductBrand>().Add(Brand);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }

            if(_dbcontext.ProductCategories.Count() == 0)
            {
                var CategoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoriesData);

                if(Categories is not null && Categories.Count > 0)
                {
                    foreach (var Category in Categories)
                    {
                        _dbcontext.Set<ProductCategory>().Add(Category);
                    }

                    await _dbcontext.SaveChangesAsync();
                }
            }

            if (_dbcontext.Products.Count() == 0)
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products is not null && Products.Count > 0)
                {
                    foreach (var product in Products)
                    {
                        _dbcontext.Set<Product>().Add(product);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }
            if(_dbcontext.DeliveryMethods.Count() == 0)
            {
                var DeliverMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliverMethodsData);
                if(DeliveryMethods is not null && DeliveryMethods.Count > 0)
                {
                    foreach(var item in DeliveryMethods)
                    {
                        _dbcontext.Set<DeliveryMethod>().Add(item);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }
        }
    }
}
