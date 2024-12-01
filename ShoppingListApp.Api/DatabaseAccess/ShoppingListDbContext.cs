using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Models;

namespace ShoppingListApp.Api.DatabaseAccess;

public class ShoppingListDbContext : DbContext {
    public ShoppingListDbContext(DbContextOptions<ShoppingListDbContext> opts) : base(opts) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ShoppingList> ShoppingLists => Set<ShoppingList>();
    // public DbSet<DefaultProduct> DefaultProducts => Set<DefaultProduct>();
    
    
}