using Microsoft.AspNetCore.Mvc;
using ShoppingListApp.Models;
using ShoppingListApp.Api.DatabaseAccess;

namespace ShoppingListApp.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProductsController : ControllerBase {
    private IShoppingListRepository _repository;

    public ProductsController(IShoppingListRepository repository) {
        _repository = repository;
    }

    // GET: /api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll() {
        try {
            var products = await _repository.GetAllProducts();

            if (products is null) {
                return NotFound();
            }

            if (!products.Any()) {
                return NoContent();
            }

            return Ok(products);
        }
        catch (Exception e) {
            return StatusCode(500, "Internal server error");
        }
    }

    // GET: /api/products/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(long id) {
        try {
            var product = await _repository.GetProductById(id);

            if (product == null) {
                return NotFound($"Product with ID {id} not found.");
            }

            return Ok(product);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

    // POST: /api/products
    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product) {
        try {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            await _repository.AddProduct(product);
            await _repository.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }


    // PUT: /api/products/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<Product>> Update(long id, Product product) {
        try {
            if (id != product.ProductId) {
                return BadRequest("Product ID mismatch.");
            }

            var existingProduct = await _repository.GetProductById(id);

            if (existingProduct is null) {
                return NotFound($"Product with ID {id} not found.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Amount = product.Amount;
            existingProduct.Weight = product.Weight;
            existingProduct.IsComplete = product.IsComplete;
            existingProduct.ShoppingListId = product.ShoppingListId;

            _repository.UpdateProduct(existingProduct);
            await _repository.SaveChanges();

            return NoContent();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }

    // DELETE: /api/products/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<Product>> Delete(long id) {
        try {
            var product = await _repository.GetProductById(id);

            if (product == null) {
                return NotFound($"Product with ID {id} not found.");
            }

            _repository.RemoveProduct(product);
            await _repository.SaveChanges();

            return NoContent();
        }
        catch (Exception e) {
            Console.WriteLine(e);
            return StatusCode(500, "Internal server error");
        }
    }
}