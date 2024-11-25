using System.Collections.Generic;
using Models;

namespace ShoppingListApi.Models;

public interface IShoppingListRepository {
    // Product-related methods
    IEnumerable<Product> GetAllProducts();
    Product GetProductById(long id);
    Product GetProductByListId(long id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void RemoveProduct(Product product);
    
    // ShoppingList-related methods
    IEnumerable<ShoppingList> GetAllShoppingLists();
    ShoppingList GetShoppingListById(long id);
    void AddShoppingList(ShoppingList shoppingList);
    void UpdateShoppingList(ShoppingList shoppingList);
    void RemoveShoppingList(ShoppingList shoppingList);
    
    // Commit changes to the database
    void SaveChanges();
}