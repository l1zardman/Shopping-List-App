using ShoppingListApp.Models;

namespace ShoppingListApp.Api.DatabaseAccess;

public interface IShoppingListRepository {
    // Product-related methods
    Task<IEnumerable<Product>> GetAllProducts();
    Task<Product?> GetProductById(long id);
    // Product GetProductByListId(long id);
    Task AddProduct(Product product);
    void UpdateProduct(Product product);
    void RemoveProduct(Product product);
    
    // ShoppingList-related methods
    Task<IEnumerable<ShoppingList>> GetAllShoppingLists();
    Task<ShoppingList?> GetShoppingListById(long id);
    Task AddShoppingList(ShoppingList shoppingList);
    void UpdateShoppingList(ShoppingList shoppingList);
    void RemoveShoppingList(ShoppingList shoppingList);
    
    // Commit changes to the database
    Task SaveChanges();
}