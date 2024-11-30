using Microsoft.AspNetCore.Mvc;
using ShoppingListApp.Api.Models;

namespace ShoppingListApp.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ShoppingListController : ControllerBase {
    private IShoppingListRepository _shoppingListRepository;

    public ShoppingListController(IShoppingListRepository repo) {
        _shoppingListRepository = repo;
    }

    // GET: /api/shoppinglists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<global::ShoppingListApp.Models.ShoppingList>>> GetAll() {
        var shoppingLists = await _shoppingListRepository.GetAllShoppingLists();
        return Ok(shoppingLists);
    }

    // GET: /api/shoppinglists/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<global::ShoppingListApp.Models.ShoppingList>> GetById(long id) {
        var shoppingList = await _shoppingListRepository.GetShoppingListById(id);

        if (shoppingList == null) {
            return NotFound($"ShoppingList with ID {id} not found.");
        }

        return Ok(shoppingList);
    }

    // POST: /api/shoppinglists
    [HttpPost]
    public async Task<ActionResult<global::ShoppingListApp.Models.ShoppingList>> Create(
        global::ShoppingListApp.Models.ShoppingList shoppingList) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        await _shoppingListRepository.AddShoppingList(shoppingList);
        await _shoppingListRepository.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = shoppingList.ShoppingListId }, shoppingList);
    }

    // PUT: /api/shoppinglists/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<global::ShoppingListApp.Models.ShoppingList>> Update(long id,
        global::ShoppingListApp.Models.ShoppingList shoppingList) {
        if (id != shoppingList.ShoppingListId) {
            return BadRequest("ShoppingList ID mismatch.");
        }

        var existingShoppingList = await _shoppingListRepository.GetShoppingListById(id);

        if (existingShoppingList == null) {
            return NotFound($"ShoppingList with ID {id} not found.");
        }

        existingShoppingList.Name = shoppingList.Name;
        existingShoppingList.Date = shoppingList.Date;
        existingShoppingList.Products = shoppingList.Products;

        _shoppingListRepository.UpdateShoppingList(existingShoppingList);
        await _shoppingListRepository.SaveChanges();

        return Ok(existingShoppingList);
    }

    // DELETE: /api/shoppinglists/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<global::ShoppingListApp.Models.ShoppingList>> Delete(long id) {
        var shoppingList = await _shoppingListRepository.GetShoppingListById(id);

        if (shoppingList == null) {
            return NotFound($"ShoppingList with ID {id} not found.");
        }

        _shoppingListRepository.RemoveShoppingList(shoppingList);
        await _shoppingListRepository.SaveChanges();

        return NoContent();
    }
}