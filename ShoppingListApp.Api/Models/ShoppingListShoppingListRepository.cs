using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Models;

namespace ShoppingListApp.Api.Models;

public class ShoppingListShoppingListRepository : IShoppingListRepository {
    private ShoppingListDbContext _context;

    public ShoppingListShoppingListRepository(ShoppingListDbContext context) {
        _context = context;
    }

    // Products
    public async Task<IEnumerable<Product>> GetAllProducts() {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetProductById(long id) {
        return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
    }

    // public Product GetProductByListId(long id) {
    //     return _context.Products.FirstOrDefault(p => p.ShoppingListId == id);
    // }

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
    public async Task<IEnumerable<global::ShoppingListApp.Models.ShoppingList>> GetAllShoppingLists() {
        return await _context.ShoppingLists
            .Include(p=>p.Products)
            .ToListAsync();
    }

    public async Task<global::ShoppingListApp.Models.ShoppingList> GetShoppingListById(long id) {
        return await _context.ShoppingLists
            .FirstOrDefaultAsync(sl => sl.ShoppingListId == id);
    }

    public async Task AddShoppingList(global::ShoppingListApp.Models.ShoppingList shoppingList) {
        await _context.ShoppingLists.AddAsync(shoppingList);
    }

    public void UpdateShoppingList(global::ShoppingListApp.Models.ShoppingList shoppingList) {
        _context.ShoppingLists.Update(shoppingList);
    }

    public void RemoveShoppingList(global::ShoppingListApp.Models.ShoppingList shoppingList) {
        _context.ShoppingLists.Remove(shoppingList);
    }

    public async Task SaveChanges() {
        await _context.SaveChangesAsync();
    }
}