using Models;

namespace ShoppingListApi.Models;

public class ShoppingListShoppingListRepository : IShoppingListRepository {
    private ShoppingListDbContext _context;

    public ShoppingListShoppingListRepository(ShoppingListDbContext context) {
        _context = context;
    }

    // Products
    public IEnumerable<Product> GetAllProducts() {
        return _context.Products.ToList();
    }

    public Product GetProductById(long id) {
        return _context.Products.FirstOrDefault(p => p.ProductId == id);
    }

    public Product GetProductByListId(long id) {
        return _context.Products.FirstOrDefault(p => p.ShoppingListId == id);
    }

    public void AddProduct(Product product) {
        _context.Products.Add(product);
    }

    public void UpdateProduct(Product product) {
        _context.Products.Update(product);
    }

    public void RemoveProduct(Product product) {
        _context.Products.Remove(product);
    }

    // ShoppingLists
    public IEnumerable<ShoppingList> GetAllShoppingLists() {
        return _context.ShoppingLists.ToList();
    }

    public ShoppingList GetShoppingListById(long id) {
        return _context.ShoppingLists.FirstOrDefault(sl => sl.ShoppingListId == id);
    }

    public void AddShoppingList(ShoppingList shoppingList) {
        _context.ShoppingLists.Add(shoppingList);
    }

    public void UpdateShoppingList(ShoppingList shoppingList) {
        _context.ShoppingLists.Update(shoppingList);
    }

    public void RemoveShoppingList(ShoppingList shoppingList) {
        _context.ShoppingLists.Remove(shoppingList);
    }

    public void SaveChanges() {
        _context.SaveChanges();
    }
}