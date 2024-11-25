using System.Globalization;
using Models;

namespace ShoppingListApp.Stubs;

public static class SomeTestingData {
    private static CultureInfo _culture = new CultureInfo("pl-PL");
    
    public static List<Product> AllProducts = new List<Product> {
        new Product() { ProductId = 1, Name = "Milk", Amount = 1, Weight = 0.30m },
        new Product() { ProductId = 2, Name = "Cheese", Amount = 1, Weight = 0.30m },
        new Product() { ProductId = 3, Name = "Sugar", Amount = 1, Weight = 1.0m },
        new Product() { ProductId = 4, Name = "Ham", Amount = 1, Weight = 0.333m },
        new Product() { ProductId = 5, Name = "Cola", Amount = 1, Weight = 0.40m },
        new Product() { ProductId = 6, Name = "Bread", Amount = 1, Weight = 0.50m },
        new Product() { ProductId = 7, Name = "Butter", Amount = 1, Weight = 0.25m },
        new Product() { ProductId = 8, Name = "Eggs", Amount = 12, Weight = 0.65m },
        new Product() { ProductId = 9, Name = "Juice", Amount = 1, Weight = 1.0m },
        new Product() { ProductId = 10, Name = "Pasta", Amount = 1, Weight = 0.75m },
        new Product() { ProductId = 11, Name = "Rice", Amount = 1, Weight = 1.0m },
        new Product() { ProductId = 12, Name = "Chicken", Amount = 1, Weight = 1.2m },
        new Product() { ProductId = 13, Name = "Apples", Amount = 6, Weight = 0.5m },
        new Product() { ProductId = 14, Name = "Bananas", Amount = 6, Weight = 0.6m },
        new Product() { ProductId = 15, Name = "Yogurt", Amount = 1, Weight = 0.2m },
    };

    public static ShoppingList StubShoppingList1 = new ShoppingList {
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
    };

    public static ShoppingList StubShoppingList2 = new ShoppingList {
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
            new Product() { ProductId = 10, Name = "Pasta", Amount = 1, Weight = 0.75m },
        },
        Date = DateOnly.ParseExact("2023-06-30", "O")
    };

    public static readonly List<ShoppingList> ShoppingListsStubs = new List<ShoppingList> {
        StubShoppingList1,
        StubShoppingList2
    };
}