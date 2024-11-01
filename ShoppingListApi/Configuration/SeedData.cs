using Microsoft.EntityFrameworkCore;
using Models;
using ShoppingListApi.Models;

namespace ShoppingListApi.Configuration;

public class SeedData {
    public static void EnsurePopulated(IApplicationBuilder app) {
        var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ShoppingListDbContext>();

        if (context.Database.GetPendingMigrations().Any()) {
            context.Database.Migrate();
        }

        if (!context.DefaultProducts.Any()) {
            context.DefaultProducts.AddRange(
                new DefaultProduct { Name = "Milk" },
                new DefaultProduct { Name = "Bread" },
                new DefaultProduct { Name = "Eggs" }
            );
            
            context.SaveChanges();
        }
    }
}