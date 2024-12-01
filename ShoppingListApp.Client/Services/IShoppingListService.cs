using ShoppingListApp.Models;

namespace ShoppingListApp.Client.Services;

public interface IShoppingListService {
    Task<List<ShoppingList>> GetAllShoppingLists();
    Task DeleteShoppingList(string? id);
    Task<List<string>> GetShoppingListsNames();
    Task<ShoppingList?> GetShoppingListById(long id);
    Task<ShoppingList?> CreateShoppingList(ShoppingList shoppingList);
    Task<ShoppingList?> UpdateShoppingList(long id, ShoppingList shoppingList);
}