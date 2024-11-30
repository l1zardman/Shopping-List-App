using System.Net.Http.Json;
using ShoppingListApp.Models;

namespace ShoppingListApp.Client.Services;

public class HttpShoppingListService : IShoppingListService {
    private readonly HttpClient _client;

    public HttpShoppingListService(HttpClient client) {
        _client = client;
    }


    public async Task<List<ShoppingList>> GetAllShoppingLists() {
        try {
            var shoppingLists = await _client.GetFromJsonAsync<List<ShoppingList>>("/api/ShoppingList");
            return shoppingLists ?? new List<ShoppingList>();
        }
        catch (Exception ex) {
            Console.WriteLine($"Error fetching shopping lists: {ex.Message}");
            return new List<ShoppingList>();
        }
    }

    // Delete a shopping list by ID
    public async Task DeleteShoppingList(string? id) {
        if (string.IsNullOrWhiteSpace(id)) {
            throw new ArgumentException("Invalid shopping list ID.", nameof(id));
        }

        var response = await _client.DeleteAsync($"/api/ShoppingList/{id}");

        if (!response.IsSuccessStatusCode) {
            throw new Exception($"Failed to delete shopping list with ID {id}. Status code: {response.StatusCode}");
        }
    }
}