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

    public async Task<List<string>> GetShoppingListsNames() {
        var response = await _client.GetFromJsonAsync<List<string>>("/api/ShoppingList/Names");

        return response ?? new List<string>();
    }

    // Fetch a shopping list by ID
    public async Task<ShoppingList?> GetShoppingListById(long id) {
        try {
            return await _client.GetFromJsonAsync<ShoppingList>($"/api/ShoppingList/{id}");
        }
        catch (Exception ex) {
            Console.WriteLine($"Error fetching shopping list with ID {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<ShoppingList?> CreateShoppingList(ShoppingList shoppingList) {
        try {
            var response = await _client.PostAsJsonAsync("/api/ShoppingList", shoppingList);

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadFromJsonAsync<ShoppingList>();
            }

            throw new Exception($"Failed to create shopping list. Status code: {response.StatusCode}");
        }
        catch (Exception ex) {
            Console.WriteLine($"Error creating shopping list: {ex.Message}");
            throw;
        }
    }

    public async Task<ShoppingList?> UpdateShoppingList(long id, ShoppingList shoppingList) {
        try {
            var response = await _client.PutAsJsonAsync($"/api/ShoppingList/{id}", shoppingList);

            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadFromJsonAsync<ShoppingList>();
            }

            throw new Exception($"Failed to update shopping list with ID {id}. Status code: {response.StatusCode}");
        }
        catch (Exception ex) {
            Console.WriteLine($"Error updating shopping list with ID {id}: {ex.Message}");
            throw;
        }
    }
}