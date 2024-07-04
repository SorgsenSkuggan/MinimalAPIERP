using ERP;
using ERP.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;

namespace MinimalAPIERP.Data{
    public static class DbInitializer{
        public static void Initialize(AppDbContext context) {
            if (context.Stores.Any() && context.Categories.Any()) {
                return;
            }

            Store[] stores = new Store[]{
                new Store{
                    StoreId = 1, StoreGuid = new Guid(), Name = "Store 1"
                },
                new Store{
                    StoreId = 2, StoreGuid = new Guid(), Name = "Store 2"
                },
                new Store{
                    StoreId = 3, StoreGuid = new Guid(), Name = "Store 3"
                },
                new Store{
                    StoreId = 4, StoreGuid = new Guid(), Name = "Store 4"
                },
                new Store{
                    StoreId = 5, StoreGuid = new Guid(), Name = "Store 5"
                },
                new Store{
                    StoreId = 6, StoreGuid = new Guid(), Name = "Store 6"
                },
                new Store{
                    StoreId = 7, StoreGuid = new Guid(), Name = "Store 7"
                }
            };

            foreach (Store store in stores){
                context.Stores.Add(store);
            }
            context.SaveChanges();

            Category[] categories = new Category[] {
                new Category{ 
                    CategoryId = 1, CategoryGuid = new Guid(), Name = "Category 1", 
                    Description = "Test category 1", ImageUrl = null
                },
                new Category{
                    CategoryId = 2, CategoryGuid = new Guid(), Name = "Category 2",
                    Description = "Test category 1", ImageUrl = null
                },
                new Category{
                    CategoryId = 3, CategoryGuid = new Guid(), Name = "Category 3",
                    Description = "Test category 3", ImageUrl = null
                }
                ,new Category{
                    CategoryId = 4, CategoryGuid = new Guid(), Name = "Category 4",
                    Description = "Test category 4", ImageUrl = null
                },
            };

            foreach (Category categoria in categories) {
                context.Categories.Add(categoria);
            }
            context.SaveChanges();
        }
    }
}