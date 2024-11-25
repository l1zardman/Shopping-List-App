using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Models;
using ShoppingListApi.Models;
using ShoppingListApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using ShoppingListApi.Models;

namespace Test.ShoppingListApi;

public class ShoppingListsControllerTests {
    private readonly Mock<IShoppingListRepository> _mockRepo;
    private readonly ShoppingListController _controller;

    public ShoppingListsControllerTests() {
        _mockRepo = new Mock<IShoppingListRepository>();
        _controller = new ShoppingListController(_mockRepo.Object);
    }

    [Fact]
    public void GetAll_ReturnsOkResult_WithListOfShoppingLists() {
        // Arrange
        var shoppingLists = new List<ShoppingList> {
            new ShoppingList { ShoppingListId = 1, Name = "Groceries" },
            new ShoppingList { ShoppingListId = 2, Name = "Electronics" }
        };
        _mockRepo.Setup(repo => repo.GetAllShoppingLists()).Returns(shoppingLists);

        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<ShoppingList>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public void GetById_ReturnsOkResult_WithShoppingList() {
        // Arrange
        var shoppingList = new ShoppingList { ShoppingListId = 1, Name = "Groceries" };
        _mockRepo.Setup(repo => repo.GetShoppingListById(1)).Returns(shoppingList);

        // Act
        var result = _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<ShoppingList>(okResult.Value);
        Assert.Equal("Groceries", returnValue.Name);
    }

    [Fact]
    public void Create_ReturnsCreatedResult() {
        // Arrange
        var shoppingList = new ShoppingList { ShoppingListId = 1, Name = "Groceries" };

        // Act
        var result = _controller.Create(shoppingList);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<ShoppingList>(createdResult.Value);
        Assert.Equal("Groceries", returnValue.Name);
    }

    [Fact]
    public void Delete_ReturnsNoContent_WhenShoppingListExists() {
        // Arrange
        var shoppingList = new ShoppingList { ShoppingListId = 1, Name = "Groceries" };
        _mockRepo.Setup(repo => repo.GetShoppingListById(1)).Returns(shoppingList);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public void Delete_ReturnsNotFound_WhenShoppingListDoesNotExist() {
        // Arrange
        _mockRepo.Setup(repo => repo.GetShoppingListById(1)).Returns((ShoppingList)null);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}