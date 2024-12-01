using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingListApp.Api.Controllers;
using ShoppingListApp.Models;
using Xunit;

namespace ShoppingListApp.Api.UnitTests;

public class ProductsControllerTests {
    private readonly Mock<IShoppingListRepository> _mockRepository;
    private readonly ProductsController _controller;

    public ProductsControllerTests() {
        _mockRepository = new Mock<IShoppingListRepository>();
        _controller = new ProductsController(_mockRepository.Object);
    }

    // testing the GetAll method, GET */api/products
    [Fact]
    public async Task GetAll_WhenProductsExist_ReturnsOk() {
        // Arrange
        var products = new List<Product> { new Product { ProductId = 1, Name = "Test Product" } };
        _mockRepository.Setup(repo => repo.GetAllProducts()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Single(returnedProducts);
    }

    [Fact]
    public async Task GetAll_ReturnsAllProducts_ReturnsOk() {
        // Arrange
        var mockProducts = new List<Product> {
            new Product { ProductId = 1, Name = "Milk" },
            new Product { ProductId = 2, Name = "Bread" }
        };
        _mockRepository.Setup(repo => repo.GetAllProducts())
            .ReturnsAsync(mockProducts);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Equal(2, products.Count());
    }

    [Fact]
    public async Task GetAll_WhenNoProductsExist_ReturnsNoContent() {
        // Arrange
        _mockRepository.Setup(repo => repo.GetAllProducts()).ReturnsAsync(new List<Product>());

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public async Task GetAll_WhenRepositoryReturnsNull_ReturnsNoContent() {
        // Arrange
        _mockRepository.Setup(repo => repo.GetAllProducts()).ReturnsAsync((List<Product>)null);

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetAll_InternalServerException_ReturnsInternalServerError() {
        // Arrange
        _mockRepository.Setup(repo =>
            repo.GetAllProducts()).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetAll();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    // testing the GetById method, GET */api/products/{id}
    [Fact]
    public async Task GetById_SearchingExistingId_ReturnsOkProduct() {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Milk" };
        _mockRepository.Setup(repo => repo.GetProductById(1))
            .ReturnsAsync(product);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal("Milk", returnedProduct.Name);
    }

    [Fact]
    public async Task GetById_SearchingNonExistingId_ReturnsNotFound() {
        // Arrange
        _mockRepository.Setup(repo => repo.GetProductById(99))
            .ReturnsAsync((Product)null);

        // Act
        var result = await _controller.GetById(99);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Product with ID 99 not found.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetById_InternalServerException_ReturnsInternalServerError() {
        // Arrange
        var productId = 1;
        _mockRepository.Setup(repo => repo.GetProductById(productId)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetById(productId);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Internal server error", statusCodeResult.Value);
    }

    // testing the Create method, POST */api/products
    [Fact]
    public async Task Create_ValidProduct_ReturnsCreatedAtAction() {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Milk" };
        _mockRepository.Setup(repo => repo.AddProduct(product));
        _mockRepository.Setup(repo => repo.SaveChanges());

        // Act
        var result = await _controller.Create(product);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdProduct = Assert.IsType<Product>(createdAtActionResult.Value);
        Assert.Equal("Milk", createdProduct.Name);
    }

    [Fact]
    public async Task Create_InvalidModelState_ReturnsBadRequest() {
        // Arrange
        _controller.ModelState.AddModelError("Name", "Required");

        // Act
        var result = await _controller.Create(new Product());

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequestResult.Value);
    }
    
    [Fact]
    public async Task Create_InternalServerException_ReturnsInternalServerError() {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Milk" };
        _mockRepository.Setup(repo => repo.AddProduct(product)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Create(product);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Internal server error", statusCodeResult.Value);
    }

    // testing the Update method, PUT */api/products/{id}
    [Fact]
    public async Task Update_ValidUpdate_ReturnsNoContent() {
        // Arrange
        var existingProduct = new Product { ProductId = 1, Name = "Milk" };
        var updatedProduct = new Product { ProductId = 1, Name = "Almond Milk" };

        _mockRepository.Setup(repo => repo.GetProductById(1))
            .ReturnsAsync(existingProduct);

        // Act
        var result = await _controller.Update(1, updatedProduct);

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
        _mockRepository.Verify(repo => repo.UpdateProduct(It.Is<Product>(p => p.Name == "Almond Milk")));
        _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Update_IdMismatch_ReturnsBadRequest() {
        // Arrange
        var updatedProduct = new Product { ProductId = 2, Name = "Almond Milk" };

        // Act
        var result = await _controller.Update(1, updatedProduct);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Product ID mismatch.", badRequestResult.Value);
    }

    [Fact]
    public async Task Update_NonExistingId_ReturnsNotFound() {
        // Arrange
        var updatedProduct = new Product { ProductId = 1, Name = "Almond Milk" };

        _mockRepository.Setup(repo => repo.GetProductById(1))
            .ReturnsAsync((Product)null);

        // Act
        var result = await _controller.Update(1, updatedProduct);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Product with ID 1 not found.", notFoundResult.Value);
    }
    
    [Fact]
    public async Task Update_InternalServerException_ReturnsInternalServerError() {
        // Arrange
        var existingProduct = new Product { ProductId = 1, Name = "Milk" };
        var updatedProduct = new Product { ProductId = 1, Name = "Almond Milk" };

        _mockRepository.Setup(repo => repo.GetProductById(1))
            .ReturnsAsync(existingProduct);
        _mockRepository.Setup(repo => repo.UpdateProduct(existingProduct)).Throws(new Exception("Database error"));

        // Act
        var result = await _controller.Update(1, updatedProduct);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Internal server error", statusCodeResult.Value);
    }

    // testing the Delete method, DELETE */api/products/{id}
    [Fact]
    public async Task Delete_ExistingId_ReturnsNoContent() {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Milk" };
        _mockRepository.Setup(repo => repo.GetProductById(1))
            .ReturnsAsync(product);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
        _mockRepository.Verify(repo => repo.RemoveProduct(product));
        // _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Delete_NonExistingId_ReturnsNotFound() {
        // Arrange
        _mockRepository.Setup(repo => repo.GetProductById(99))
            .ReturnsAsync((Product)null);

        // Act
        var result = await _controller.Delete(99);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Product with ID 99 not found.", notFoundResult.Value);
    }
    
    [Fact]
    public async Task Delete_ReturnsInternalServerError_OnException()
    {
        // Arrange
        var productId = 1;
        _mockRepository.Setup(repo => repo.GetProductById(productId)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Delete(productId);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Internal server error", statusCodeResult.Value);
    }
}