using Microsoft.EntityFrameworkCore;
using Models;

namespace ShoppingListApi.Models;

public class ShoppingListDbContext : DbContext {
    public ShoppingListDbContext(DbContextOptions<ShoppingListDbContext> opts) : base(opts) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<ShoppingList> ShoppingLists => Set<ShoppingList>();
    public DbSet<DefaultProduct> DefaultProducts => Set<DefaultProduct>();
    
    
}