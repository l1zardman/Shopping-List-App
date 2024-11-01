using Models;

namespace ShoppingListApi.Models;

public class ShoppingListRepository : IRepository {
    private ShoppingListDbContext _context;

    public ShoppingListRepository(ShoppingListDbContext context) {
        _context = context;
    }

    public IQueryable<Product> Products => _context.Products;
    public IQueryable<ShoppingList> ShoppingLists => _context.ShoppingLists;
    public IQueryable<DefaultProduct> DefaultProducts => _context.DefaultProducts;
}