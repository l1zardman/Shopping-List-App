using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using ShoppingListApp.Api;
using ShoppingListApp.Models;
using Xunit;


public class ProductsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>> {
    private readonly HttpClient _client;

    public ProductsControllerIntegrationTests(WebApplicationFactory<Program> factory) {
        _client = factory.CreateClient(); // Create an in-memory test client
    }

    [Fact]
    public async Task GetAll_ReturnsAllProducts() {
        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode(); // Status code 200
        var products = JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(products);
        Assert.True(products.Count > 0); // Ensure products are returned
    }

    [Fact]
    public async Task GetById_ReturnsProduct_WhenProductExists() {
        // Arrange
        long productId = 1;

        // Act
        var response = await _client.GetAsync($"/api/products/{productId}");

        // Assert
        response.EnsureSuccessStatusCode(); // Status code 200
        var product = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(product);
        Assert.Equal(productId, product.ProductId);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist() {
        // Arrange
        long nonExistentId = 999;

        // Act
        var response = await _client.GetAsync($"/api/products/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_AddsNewProduct() {
        // Arrange
        var newProduct = new Product {
            Name = "Test Product", 
            Amount = 1, Weight = 0.5m, 
            IsComplete = false,
            ShoppingListId = 0,
            ShoppingList = new ShoppingList {
                ShoppingListId = 0
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", newProduct);

        // Assert
        response.EnsureSuccessStatusCode(); // Status code 201
        var createdProduct = JsonConvert.DeserializeObject<Product>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(createdProduct);
        Assert.Equal(newProduct.Name, createdProduct.Name);
    }
    
    
}
// [Fact]
    // public async Task Update_UpdatesExistingProduct()
    // {
    //     // Arrange
    //     long productId = 1;
    //     var updatedProduct = new Product { ProductId = productId, Name = "Updated Product", Amount = 2, Weight = 0.8m };
    //
    //     // Act
    //     var response = await _client.PutAsJsonAsync($"/api/products/{productId}", updatedProduct);
    //
    //     // Assert
    //     response.Ensure
