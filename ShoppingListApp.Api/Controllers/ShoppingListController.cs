using Microsoft.AspNetCore.Mvc;
using ShoppingListApp.Api.DatabaseAccess;
using ShoppingListApp.Models;

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
    public async Task<ActionResult<IEnumerable<ShoppingList>>> GetAll() {
        try {
            var shoppingLists = await _shoppingListRepository.GetAllShoppingLists();

            if (shoppingLists is null) {
                return NotFound();
            }

            if (!shoppingLists.Any()) {
                return NoContent();
            }

            return Ok(shoppingLists);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("names")]
    public async Task<ActionResult<IEnumerable<string>>> GetAllNames() {
        var shoppingLists = await _shoppingListRepository.GetAllShoppingLists();
        var namesList = shoppingLists.Select(sl => sl.Name).ToList();

        return Ok(namesList);
    }

    // GET: /api/shoppinglists/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ShoppingList>> GetById(long id) {
        try {
            var shoppingList = await _shoppingListRepository.GetShoppingListById(id);

            if (shoppingList == null) {
                return NotFound($"ShoppingList with ID {id} not found.");
            }

            return Ok(shoppingList);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

    // POST: /api/shoppinglists
    [HttpPost]
    public async Task<ActionResult<ShoppingList>> Create(ShoppingList shoppingList) {
        try {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            await _shoppingListRepository.AddShoppingList(shoppingList);
            await _shoppingListRepository.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = shoppingList.ShoppingListId }, shoppingList);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

    // PUT: /api/shoppinglists/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ShoppingList>> Update(long id, ShoppingList shoppingList) {
        try {
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
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

    // DELETE: /api/shoppinglists/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<ShoppingList>> Delete(long id) {
        try {
            var shoppingList = await _shoppingListRepository.GetShoppingListById(id);

            if (shoppingList == null) {
                return NotFound($"ShoppingList with ID {id} not found.");
            }

            _shoppingListRepository.RemoveShoppingList(shoppingList);
            await _shoppingListRepository.SaveChanges();

            return NoContent();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }
}