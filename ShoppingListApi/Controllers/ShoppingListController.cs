using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Models;
using ShoppingListApi.Models;
using ShoppingListApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using ShoppingListApi.Models;

namespace ShoppingListApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ShoppingListController : ControllerBase {
    private IShoppingListRepository _shoppingListRepository;
    private ShoppingListDbContext _context;

    public ShoppingListController(IShoppingListRepository repo, ShoppingListDbContext context) {
        _shoppingListRepository = repo;
        _context = context;
    }

    // GET: /api/shoppinglists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShoppingList>>> GetAll() {
        var shoppingLists = await _context.ShoppingLists
            .Include(p=>p.Products)
            .ToListAsync();
        
        return Ok(shoppingLists);
    }

    // GET: /api/shoppinglists/{id}
    [HttpGet("{id}")]
    public ActionResult<ShoppingList> GetById(long id) {
        var shoppingList = _shoppingListRepository.GetShoppingListById(id);
        if (shoppingList == null) {
            return NotFound($"ShoppingList with ID {id} not found.");
        }

        return Ok(shoppingList);
    }

    // POST: /api/shoppinglists
    [HttpPost]
    public ActionResult<ShoppingList> Create(ShoppingList shoppingList) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        _shoppingListRepository.AddShoppingList(shoppingList);
        _shoppingListRepository.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = shoppingList.ShoppingListId }, shoppingList);
    }

    // PUT: /api/shoppinglists/{id}
    [HttpPut("{id}")]
    public ActionResult<ShoppingList> Update(long id, ShoppingList shoppingList) {
        if (id != shoppingList.ShoppingListId) {
            return BadRequest("ShoppingList ID mismatch.");
        }

        var existingShoppingList = _shoppingListRepository.GetShoppingListById(id);
        if (existingShoppingList == null) {
            return NotFound($"ShoppingList with ID {id} not found.");
        }

        existingShoppingList.Name = shoppingList.Name;
        existingShoppingList.Date = shoppingList.Date;
        existingShoppingList.Products = shoppingList.Products;

        _shoppingListRepository.UpdateShoppingList(existingShoppingList);
        _shoppingListRepository.SaveChanges();

        return Ok(existingShoppingList);
    }

    // DELETE: /api/shoppinglists/{id}
    [HttpDelete("{id}")]
    public ActionResult<ShoppingList> Delete(long id) {
        var shoppingList = _shoppingListRepository.GetShoppingListById(id);
        if (shoppingList == null) {
            return NotFound($"ShoppingList with ID {id} not found.");
        }

        _shoppingListRepository.RemoveShoppingList(shoppingList);
        _shoppingListRepository.SaveChanges();

        return NoContent();
    }
}