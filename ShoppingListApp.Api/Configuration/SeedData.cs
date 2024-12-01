using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Models;
using ShoppingListApp.Api.Models;

namespace ShoppingListApp.Api.Configuration;

public class SeedData {
    public static void EnsurePopulated(IApplicationBuilder app) {
        var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ShoppingListDbContext>();

        // TODO: Check it
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        context.DefaultProducts.AddRange(
            new DefaultProduct { Name = "Milk" },
            new DefaultProduct { Name = "Bread" },
            new DefaultProduct { Name = "Eggs" }
        );
        

        context.ShoppingLists.AddRange(
            new ShoppingList {
                ShoppingListId = 1,
                Name = "Deafult list 1",
                Products = new List<Product> {
                    new Product() { ProductId = 10, Name = "Pasta", Amount = 1, Weight = 0.75m },
                    new Product() { ProductId = 11, Name = "Rice", Amount = 1, Weight = 1.0m },
                    new Product() { ProductId = 12, Name = "Chicken", Amount = 1, Weight = 1.2m },
                    new Product() { ProductId = 13, Name = "Apples", Amount = 6, Weight = 0.5m },
                    new Product() { ProductId = 14, Name = "Bananas", Amount = 6, Weight = 0.6m },
                    new Product() { ProductId = 15, Name = "Yogurt", Amount = 1, Weight = 0.2m },
                },
                Date = DateOnly.ParseExact("2023-05-24", "O")
            },
            new ShoppingList {
                ShoppingListId = 2,
                Name = "Deafult list 2",
                Products = new List<Product> {
                    new Product() { ProductId = 1, Name = "Milk", Amount = 1, Weight = 0.30m },
                    new Product() { ProductId = 2, Name = "Cheese", Amount = 1, Weight = 0.30m },
                    new Product() { ProductId = 3, Name = "Sugar", Amount = 1, Weight = 1.0m },
                    new Product() { ProductId = 4, Name = "Ham", Amount = 1, Weight = 0.333m },
                    new Product() { ProductId = 5, Name = "Cola", Amount = 1, Weight = 0.40m },
                    new Product() { ProductId = 6, Name = "Bread", Amount = 1, Weight = 0.50m },
                    new Product() { ProductId = 7, Name = "Butter", Amount = 1, Weight = 0.25m },
                    new Product() { ProductId = 8, Name = "Eggs", Amount = 12, Weight = 0.65m },
                    new Product() { ProductId = 9, Name = "Juice", Amount = 1, Weight = 1.0m },
                },
                Date = DateOnly.ParseExact("2023-06-30", "O")
            }
        );
        
        context.SaveChanges();
    }
}