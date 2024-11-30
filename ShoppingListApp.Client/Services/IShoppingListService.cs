using ShoppingListApp.Models;

namespace ShoppingListApp.Client.Services;

public interface IShoppingListService {
    Task<List<ShoppingList>> GetAllShoppingLists();
    Task DeleteShoppingList(string? id);
}