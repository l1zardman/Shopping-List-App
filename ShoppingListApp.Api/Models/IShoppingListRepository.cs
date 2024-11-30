using ShoppingListApp.Models;

namespace ShoppingListApp.Api.Models;

public interface IShoppingListRepository {
    // Product-related methods
    Task<IEnumerable<Product>> GetAllProducts();
    Task<Product> GetProductById(long id);
    // Product GetProductByListId(long id);
    Task AddProduct(Product product);
    void UpdateProduct(Product product);
    void RemoveProduct(Product product);
    
    // ShoppingList-related methods
    Task<IEnumerable<global::ShoppingListApp.Models.ShoppingList>> GetAllShoppingLists();
    Task<global::ShoppingListApp.Models.ShoppingList> GetShoppingListById(long id);
    Task AddShoppingList(global::ShoppingListApp.Models.ShoppingList shoppingList);
    void UpdateShoppingList(global::ShoppingListApp.Models.ShoppingList shoppingList);
    void RemoveShoppingList(global::ShoppingListApp.Models.ShoppingList shoppingList);
    
    // Commit changes to the database
    Task SaveChanges();
}