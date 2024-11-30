using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingListApp.Models;
using ShoppingListApp.Api.Models;
using ShoppingListApp.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShoppingListApp.Api.UnitTests;

public class ShoppingListsControllerTests {
    private readonly Mock<IShoppingListRepository> _mockRepo;
    private readonly ShoppingListController _controller;

    public ShoppingListsControllerTests() {
        _mockRepo = new Mock<IShoppingListRepository>();
        _controller = new ShoppingListController(_mockRepo.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfShoppingLists() {
        // Arrange
        var shoppingLists = new List<global::ShoppingListApp.Models.ShoppingList> {
            new ShoppingList { ShoppingListId = 1, Name = "Groceries" },
            new ShoppingList { ShoppingListId = 2, Name = "Electronics" }
        };

        _mockRepo.Setup(repo => repo.GetAllShoppingLists()).ReturnsAsync(shoppingLists);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<ShoppingList>>(okResult.Value);

        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetById_ReturnsOkResult_WithShoppingList() {
        // Arrange
        var shoppingList = new ShoppingList { ShoppingListId = 1, Name = "Groceries" };
        _mockRepo.Setup(repo => repo.GetShoppingListById(1)).ReturnsAsync(shoppingList);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<ShoppingList>(okResult.Value);
        
        Assert.Equal("Groceries", returnValue.Name);
    }

    [Fact]
    public async Task Create_ReturnsCreatedResult() {
        // Arrange
        var shoppingList = new ShoppingList { ShoppingListId = 1, Name = "Groceries" };
    
        // Act
        var result = await _controller.Create(shoppingList);
    
        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<ShoppingList>(createdResult.Value);
        
        Assert.Equal("Groceries", returnValue.Name);
    }
    
    [Fact]
    public async Task Delete_ReturnsNoContent_WhenShoppingListExists() {
        // Arrange
        var shoppingList = new ShoppingList { ShoppingListId = 1, Name = "Groceries" };
        _mockRepo.Setup(repo => repo.GetShoppingListById(1)).ReturnsAsync(shoppingList);
    
        // Act
        var result = await _controller.Delete(1);
    
        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }
    
    [Fact]
    public async Task Delete_ReturnsNotFound_WhenShoppingListDoesNotExist() {
        // Arrange
        _mockRepo.Setup(repo => repo.GetShoppingListById(1)).ReturnsAsync((ShoppingList)null);
    
        // Act
        var result = await _controller.Delete(1);
    
        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}