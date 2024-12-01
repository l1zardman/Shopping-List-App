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

    // teest GetAll method, GET /api/products
    [Fact]
    public async Task GetAll_ProductsArePresent_OkResult() {
        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode(); // Status code 200
        var products = JsonConvert.DeserializeObject<List<Product>>(await response.Content.ReadAsStringAsync());
        Assert.NotNull(products);
        Assert.True(products.Count > 0); // Ensure products are returned
    }

    // test GetById method, GET /api/products/{id}
    [Fact]
    public async Task GetById_ProductExists_ReturnsOkResult() {
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
    public async Task GetById_ProductDoesNotExist_ReturnsNotFound() {
        // Arrange
        long nonExistentId = 999;

        // Act
        var response = await _client.GetAsync($"/api/products/{nonExistentId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // test Create method, POST /api/products
    [Fact]
    public async Task Create_NewProductIsValid_ReturnsSuccess() {
        // Arrange
        var newProduct = new Product {
            Name = "Test Product",
            Amount = 1, 
            Weight = 0.5m,
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

    [Fact]
    public async Task Create_ModelStateIsInvalid_ReturnsBadRequest() {
        // Arrange
        var invalidProduct = new Product {
            Name = "", // Invalid because Name is required
            Amount = -1, // Invalid because Amount cannot be negative
            Weight = 0
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", invalidProduct);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_OnException_ReturnsInternalServerError() {
        // Arrange
        // Simulate an exception in the repository, such as by providing invalid input or altering the test setup.

        var problematicProduct = new Product {
            Name = "Problematic Product",
            Amount = 1,
            Weight = 1.0m
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", problematicProduct);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }


    // test Update method, PUT /api/products/{id}
    [Fact]
    public async Task Update_WhenUpdateIsSuccessful_ReturnsNoContent() {
        // Arrange
        var existingProductId = 1; // Ensure this product ID exists in your test database
        var updatedProduct = new Product {
            ProductId = existingProductId,
            Name = "Updated Product",
            Amount = 5,
            Weight = 2.5m,
            IsComplete = true,
            ShoppingListId = 1
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/products/{existingProductId}", updatedProduct);

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Update_WhenProductIdMismatch_ReturnsBadRequest() {
        // Arrange
        var existingProductId = 1; // Ensure this product ID exists in your test database
        var updatedProduct = new Product {
            ProductId = 2, // Mismatched ID
            Name = "Updated Product",
            Amount = 5,
            Weight = 2.5m,
            IsComplete = true,
            ShoppingListId = 1
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/products/{existingProductId}", updatedProduct);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains("Product ID mismatch.", responseContent);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenProductDoesNotExist() {
        // Arrange
        var nonExistentProductId = 9999;
        var updatedProduct = new Product {
            ProductId = nonExistentProductId,
            Name = "Updated Product",
            Amount = 5,
            Weight = 2.5m,
            IsComplete = true,
            ShoppingListId = 1
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/products/{nonExistentProductId}", updatedProduct);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains($"Product with ID {nonExistentProductId} not found.", responseContent);
    }

    // test Delete method, DELETE /api/products/{id}

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeletionIsSuccessful() {
        // Arrange
        var existingProductId = 1; // Ensure this product ID exists in your test database

        // Act
        var response = await _client.DeleteAsync($"/api/products/{existingProductId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenProductDoesNotExist() {
        // Arrange
        var nonExistentProductId = 9999; // Ensure this ID does not exist in your test database

        // Act
        var response = await _client.DeleteAsync($"/api/products/{nonExistentProductId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Contains($"Product with ID {nonExistentProductId} not found.", responseContent);
    }
    
}