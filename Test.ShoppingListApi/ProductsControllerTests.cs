using Models;
using ShoppingListApi.Controllers;
using ShoppingListApi.Models;

namespace Test.ShoppingListApi;



public class ProductsControllerTests {
    private readonly Mock<IShoppingListRepository> _mockRepo;
    private readonly ProductsController _controller;

    public ProductsControllerTests() {
        _mockRepo = new Mock<IShoppingListRepository>();
        _controller = new ProductsController(_mockRepo.Object);
    }

    [Fact]
    public void GetAll_ReturnsOkResult_WithListOfProducts() {
        // Arrange
        var products = new List<Product> {
            new Product { ProductId = 1, Name = "Apple", Amount = 10, Weight = 1.5m },
            new Product { ProductId = 2, Name = "Banana", Amount = 5, Weight = 1.0m }
        };
        _mockRepo.Setup(repo => repo.GetAllProducts()).Returns(products);

        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<Product>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public void GetById_ReturnsOkResult_WithProduct() {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Apple", Amount = 10, Weight = 1.5m };
        _mockRepo.Setup(repo => repo.GetProductById(1)).Returns(product);

        // Act
        var result = _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Product>(okResult.Value);
        Assert.Equal("Apple", returnValue.Name);
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenProductDoesNotExist() {
        // Arrange
        _mockRepo.Setup(repo => repo.GetProductById(1)).Returns((Product)null);

        // Act
        var result = _controller.GetById(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public void Create_ReturnsCreatedResult() {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Apple", Amount = 10, Weight = 1.5m };

        // Act
        var result = _controller.Create(product);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<Product>(createdResult.Value);
        Assert.Equal("Apple", returnValue.Name);
    }

    [Fact]
    public void Delete_ReturnsNoContent_WhenProductExists() {
        // Arrange
        var product = new Product { ProductId = 1, Name = "Apple", Amount = 10, Weight = 1.5m };
        _mockRepo.Setup(repo => repo.GetProductById(1)).Returns(product);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public void Delete_ReturnsNotFound_WhenProductDoesNotExist() {
        // Arrange
        _mockRepo.Setup(repo => repo.GetProductById(1)).Returns((Product)null);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}