using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingListApp.Api.DatabaseAccess;
using ShoppingListApp.Models;
using Xunit;

namespace ShoppingListApp.Api.UnitTests;

public class DataAccessLayerTests {
    private readonly DbContextOptions<ShoppingListDbContext> _options;
    private ShoppingListDbContext _context;
    private ShoppingListShoppingListRepository _repository;

    public DataAccessLayerTests() {
        _options = new DbContextOptionsBuilder<ShoppingListDbContext>()
            .UseInMemoryDatabase(databaseName: "ShoppingListTestDb")
            .Options;
        
        _context = new ShoppingListDbContext(_options);
        _repository = new ShoppingListShoppingListRepository(_context);
        
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        SeedTestDatabase();
    }

    private void SeedTestDatabase() {
        var products = new List<Product> {
            new Product { ProductId = 1, Name = "Milk", Amount = 2 },
            new Product { ProductId = 2, Name = "Bread", Amount = 1 },
        };

        var shoppingLists = new List<ShoppingList> {
            new ShoppingList { ShoppingListId = 1, Name = "Groceries", Products = products }
        };

        _context.Products.AddRange(products);
        _context.ShoppingLists.AddRange(shoppingLists);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllProducts_ReturnsAllProducts() {
        
        // Act
        var products = await _repository.GetAllProducts();

        // Assert
        Assert.Equal(2, products.Count());
    }

    [Fact]
    public async Task GetProductById_ExistingId_ReturnsProduct() {
        // Act
        var product = await _repository.GetProductById(1);

        // Assert
        Assert.NotNull(product);
        Assert.Equal("Milk", product.Name);
    }

    [Fact]
    public async Task GetProductById_NonExistingId_ReturnsNull() {
        // Act
        var product = await _repository.GetProductById(99);

        // Assert
        Assert.Null(product);
    }

    [Fact]
    public async Task AddProduct_AddsProductToDatabase() {
        // Arrange
        var newProduct = new Product { ProductId = 3, Name = "Cheese", Amount = 1 };

        // Act
        await _repository.AddProduct(newProduct);
        await _repository.SaveChanges();

        // Assert
        var product = await _repository.GetProductById(3);
        Assert.NotNull(product);
        Assert.Equal("Cheese", product.Name);
    }

    [Fact]
    public async Task UpdateProduct_UpdatesExistingProduct() {
        // Arrange
        var product = await _repository.GetProductById(1);
        product.Name = "Oat Milk";

        // Act
        _repository.UpdateProduct(product);
        await _repository.SaveChanges();

        // Assert
        var updatedProduct = await _repository.GetProductById(1);
        Assert.Equal("Oat Milk", updatedProduct.Name);
    }

    [Fact]
    public async Task RemoveProduct_RemovesProductFromDatabase() {
        // Arrange
        var product = await _repository.GetProductById(1);

        // Act
        _repository.RemoveProduct(product);
        await _repository.SaveChanges();

        // Assert
        var deletedProduct = await _repository.GetProductById(1);
        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task GetAllShoppingLists_ReturnsAllShoppingLists() {
        // Act
        var shoppingLists = await _repository.GetAllShoppingLists();

        // Assert
        Assert.Single(shoppingLists);
        Assert.Equal("Groceries", shoppingLists.First().Name);
    }

    [Fact]
    public async Task GetShoppingListById_ExistingId_ReturnsShoppingList() {
        // Act
        var shoppingList = await _repository.GetShoppingListById(1);

        // Assert
        Assert.NotNull(shoppingList);
        Assert.Equal("Groceries", shoppingList.Name);
    }

    [Fact]
    public async Task GetShoppingListById_NonExistingId_ReturnsNull() {
        // Act
        var shoppingList = await _repository.GetShoppingListById(99);

        // Assert
        Assert.Null(shoppingList);
    }

    [Fact]
    public async Task AddShoppingList_AddsShoppingListToDatabase() {
        // Arrange
        var newShoppingList = new ShoppingList { ShoppingListId = 2, Name = "Party Supplies" };

        // Act
        await _repository.AddShoppingList(newShoppingList);
        await _repository.SaveChanges();

        // Assert
        var shoppingList = await _repository.GetShoppingListById(2);
        Assert.NotNull(shoppingList);
        Assert.Equal("Party Supplies", shoppingList.Name);
    }
}
