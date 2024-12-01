using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Models;

namespace ShoppingListApp.Api.DatabaseAccess;

public class ShoppingListShoppingListRepository : IShoppingListRepository {
    private ShoppingListDbContext _context;

    public ShoppingListShoppingListRepository(ShoppingListDbContext context) {
        _context = context;
    }

    // Products
    public async Task<IEnumerable<Product>> GetAllProducts() {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetProductById(long id) {
        return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task AddProduct(Product product) {
        await _context.Products.AddAsync(product);
    }

    public void UpdateProduct(Product product) {
        _context.Products.Update(product);
    }

    public void RemoveProduct(Product product) {
        _context.Products.Remove(product);
    }

    // ShoppingLists
    public async Task<IEnumerable<ShoppingList>> GetAllShoppingLists() {
        return await _context.ShoppingLists
            .Include(p=>p.Products)
            .ToListAsync();
    }

    public async Task<ShoppingList?> GetShoppingListById(long id) {
        return await _context.ShoppingLists
            .FirstOrDefaultAsync(sl => sl.ShoppingListId == id);
    }

    public async Task AddShoppingList(ShoppingList shoppingList) {
        await _context.ShoppingLists.AddAsync(shoppingList);
    }

    public void UpdateShoppingList(ShoppingList shoppingList) {
        _context.ShoppingLists.Update(shoppingList);
    }

    public void RemoveShoppingList(ShoppingList shoppingList) {
        _context.ShoppingLists.Remove(shoppingList);
    }

    public async Task SaveChanges() {
        await _context.SaveChangesAsync();
    }
}