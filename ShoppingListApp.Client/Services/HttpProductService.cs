using System.Net.Http.Json;
using ShoppingListApp.Models;

namespace ShoppingListApp.Client.Services;

public class HttpProductService : IProductService
{
    private readonly HttpClient _client;

    public HttpProductService(HttpClient client)
    {
        _client = client;
    }

    // Fetch all products
    public async Task<List<Product>> GetAllProducts()
    {
        try
        {
            var products = await _client.GetFromJsonAsync<List<Product>>("/api/products");
            return products ?? new List<Product>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching products: {ex.Message}");
            return new List<Product>();
        }
    }

    // Fetch a product by ID
    public async Task<Product?> GetProductById(long id)
    {
        try
        {
            return await _client.GetFromJsonAsync<Product>($"/api/products/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching product with ID {id}: {ex.Message}");
            return null;
        }
    }

    // Create a new product
    public async Task<Product?> CreateProduct(Product product)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("/api/products", product);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Product>();
            }

            throw new Exception($"Failed to create product. Status code: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating product: {ex.Message}");
            throw;
        }
    }

    // Update an existing product
    public async Task UpdateProduct(Product product)
    {
        try
        {
            var response = await _client.PutAsJsonAsync($"/api/products/{product.ProductId}", product);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update product with ID {product.ProductId}. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating product with ID {product.ProductId}: {ex.Message}");
            throw;
        }
    }

    // Delete a product by ID
    public async Task DeleteProduct(long id)
    {
        try
        {
            var response = await _client.DeleteAsync($"/api/products/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to delete product with ID {id}. Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting product with ID {id}: {ex.Message}");
            throw;
        }
    }
}
