using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShoppingListApp.Models;
using ShoppingListApp.Api.DatabaseAccess;
using ShoppingListApp.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShoppingListApp.Api.UnitTests;

public class ShoppingListsControllerTests {
    private readonly Mock<IShoppingListRepository> _mockRepository;
    private readonly ShoppingListController _controller;

    public ShoppingListsControllerTests() {
        _mockRepository = new Mock<IShoppingListRepository>();
        _controller = new ShoppingListController(_mockRepository.Object);
    }

    // test GetAll method, GET /api/shoppinglist
    [Fact]
    public async Task GetAll_ShoppingListExist_ReturnsOkResult() {
        // Arrange
        var shoppingLists = new List<ShoppingList> {
            new ShoppingList { ShoppingListId = 1, Name = "Groceries" },
        };
        _mockRepository.Setup(repo => repo.GetAllShoppingLists()).ReturnsAsync(shoppingLists);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedLists = Assert.IsAssignableFrom<IEnumerable<ShoppingList>>(okResult.Value);
        Assert.Single(returnedLists);
    }

    [Fact]
    public async Task GetAll_ManyShoppingLists_ReturnsOkResult() {
        // Arrange
        var shoppingLists = new List<global::ShoppingListApp.Models.ShoppingList> {
            new ShoppingList { ShoppingListId = 1, Name = "Groceries" },
            new ShoppingList { ShoppingListId = 2, Name = "Electronics" }
        };

        _mockRepository.Setup(repo => repo.GetAllShoppingLists()).ReturnsAsync(shoppingLists);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<ShoppingList>>(okResult.Value);

        Assert.Equal(2, returnValue.Count);
    }


    [Fact]
    public async Task GetAll_WhenShoppingListsAreEmpty_ReturnsNoContent() {
        // Arrange
        _mockRepository.Setup(repo => repo.GetAllShoppingLists()).ReturnsAsync(new List<ShoppingList>());

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public async Task GetAll_WhenShoppingListsAreNull_ReturnsNotFound() {
        // Arrange
        _mockRepository.Setup(repo => repo.GetAllShoppingLists()).ReturnsAsync((List<ShoppingList>)null);

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetAll_ReturnsInternalServerError_OnException() {
        // Arrange
        _mockRepository.Setup(repo => repo.GetAllShoppingLists()).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetAll();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Internal server error", statusCodeResult.Value);
    }
    
    // test GetById method, GET /api/shoppinglist/{id}
    
    [Fact]
    public async Task GetById_WhenShoppingListExists_ReturnsOk()
    {
        // Arrange
        var shoppingList = new ShoppingList { ShoppingListId = 1, Name = "Groceries" };
        _mockRepository.Setup(repo => repo.GetShoppingListById(1)).ReturnsAsync(shoppingList);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedShoppingList = Assert.IsType<ShoppingList>(okResult.Value);
        Assert.Equal(1, returnedShoppingList.ShoppingListId);
        Assert.Equal("Groceries", returnedShoppingList.Name);
    }

    [Fact]
    public async Task GetById_WhenShoppingListDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetShoppingListById(1)).ReturnsAsync((ShoppingList)null);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("ShoppingList with ID 1 not found.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetById_OnException_ReturnsInternalServerError()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.GetShoppingListById(1)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.GetById(1);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Internal server error", statusCodeResult.Value);
    }


    // test Create method, POST /api/shoppinglist
    
    [Fact]
    public async Task Create_WhenShoppingListIsValid_ReturnsCreatedAtAction()
    {
        // Arrange
        var shoppingList = new ShoppingList { ShoppingListId = 1, Name = "Groceries" };
        _mockRepository.Setup(repo => repo.AddShoppingList(shoppingList)).Returns(Task.CompletedTask);
        _mockRepository.Setup(repo => repo.SaveChanges()).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Create(shoppingList);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedShoppingList = Assert.IsType<ShoppingList>(createdAtActionResult.Value);
        Assert.Equal(nameof(ShoppingListController.GetById), createdAtActionResult.ActionName);
        Assert.Equal("Groceries", returnedShoppingList.Name);
    }

    [Fact]
    public async Task Create_WhenModelStateIsInvalid_ReturnsBadRequest()
    {
        // Arrange
        var shoppingList = new ShoppingList(); // Missing required properties
        _controller.ModelState.AddModelError("Name", "The Name field is required.");

        // Act
        var result = await _controller.Create(shoppingList);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var modelState = Assert.IsType<SerializableError>(badRequestResult.Value);
        Assert.True(modelState.ContainsKey("Name"));
    }

    [Fact]
    public async Task Create_OnException_ReturnsInternalServerError()
    {
        // Arrange
        var shoppingList = new ShoppingList { ShoppingListId = 1, Name = "Groceries" };
        _mockRepository.Setup(repo => repo.AddShoppingList(shoppingList)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Create(shoppingList);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Internal server error", statusCodeResult.Value);
    }
    
    // test Update method, PUT /api/shoppinglist/{id}
    [Fact]
    public async Task Update_ShoppingListIsUpdatedSuccessfully_ReturnsOk()
    {
        // Arrange
        var shoppingListId = 1L;
        var existingShoppingList = new ShoppingList { ShoppingListId = shoppingListId, Name = "Old Name" };
        var updatedShoppingList = new ShoppingList { ShoppingListId = shoppingListId, Name = "New Name" };

        _mockRepository.Setup(repo => repo.GetShoppingListById(shoppingListId)).ReturnsAsync(existingShoppingList);
        _mockRepository.Setup(repo => repo.UpdateShoppingList(existingShoppingList)).Verifiable();
        _mockRepository.Setup(repo => repo.SaveChanges()).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Update(shoppingListId, updatedShoppingList);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedShoppingList = Assert.IsType<ShoppingList>(okResult.Value);
        Assert.Equal("New Name", returnedShoppingList.Name);
        _mockRepository.Verify(repo => repo.UpdateShoppingList(existingShoppingList), Times.Once);
        _mockRepository.Verify(repo => repo.SaveChanges(), Times.Once);
    }

    [Fact]
    public async Task Update_IdDoesNotMatchShoppingListId_ReturnsBadRequest()
    {
        // Arrange
        var shoppingListId = 1L;
        var shoppingList = new ShoppingList { ShoppingListId = 2L, Name = "New Name" };

        // Act
        var result = await _controller.Update(shoppingListId, shoppingList);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("ShoppingList ID mismatch.", badRequestResult.Value);
    }

    [Fact]
    public async Task Update_ShoppingListDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var shoppingListId = 1L;
        var shoppingList = new ShoppingList { ShoppingListId = shoppingListId, Name = "New Name" };

        _mockRepository.Setup(repo => repo.GetShoppingListById(shoppingListId)).ReturnsAsync((ShoppingList)null);

        // Act
        var result = await _controller.Update(shoppingListId, shoppingList);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal($"ShoppingList with ID {shoppingListId} not found.", notFoundResult.Value);
    }

    [Fact]
    public async Task Update_OnException_ReturnsInternalServerError()
    {
        // Arrange
        var shoppingListId = 1L;
        var shoppingList = new ShoppingList { ShoppingListId = shoppingListId, Name = "New Name" };

        _mockRepository.Setup(repo => repo.GetShoppingListById(shoppingListId)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Update(shoppingListId, shoppingList);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Internal server error", statusCodeResult.Value);
    }

    //test Delete method, DELETE /api/shoppinglist/{id}
    [Fact]
    public async Task Delete_ShoppingListExists_ReturnsNoContent() {
        // Arrange
        var shoppingList = new ShoppingList { ShoppingListId = 1, Name = "Groceries" };
        _mockRepository.Setup(repo => repo.GetShoppingListById(1)).ReturnsAsync(shoppingList);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public async Task Delete_ShoppingListDoesNotExist_ReturnsNotFound() {
        // Arrange
        _mockRepository.Setup(repo => repo.GetShoppingListById(1)).ReturnsAsync((ShoppingList)null);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
    
    [Fact]
    public async Task Delete_OnException_ReturnsInternalServerError()
    {
        // Arrange
        var shoppingListId = 1L;
        _mockRepository.Setup(repo => repo.GetShoppingListById(shoppingListId)).ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _controller.Delete(shoppingListId);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Internal server error", statusCodeResult.Value);
    }
}