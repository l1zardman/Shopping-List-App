using Microsoft.AspNetCore.Mvc;
using Models;
using ShoppingListApi.Models;

namespace ShoppingListApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class ProductsController : ControllerBase {
    private IShoppingListRepository _repository;


    public ProductsController(IShoppingListRepository repository) {
        _repository = repository;
    }

    // GET: /api/products
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll() {
        var products = _repository.GetAllProducts();
        return Ok(products);
    }

    // GET: /api/products/{id}
    [HttpGet("{id}")]
    public ActionResult<Product> GetById(long id) {
        var product = _repository.GetProductById(id);
        if (product == null) {
            return NotFound($"Product with ID {id} not found.");
        }

        return Ok(product);
    }
    
    [HttpGet("list/{id}")]
    public ActionResult<Product> GetByListId(long id) {
        var product = _repository.GetProductByListId(id);
        if (product == null) {
            return NotFound($"Product with ID {id} not found.");
        }

        return Ok(product);
    }

    // POST: /api/products
    [HttpPost]
    public ActionResult<Product> Create(Product product) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        _repository.AddProduct(product);
        _repository.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
    }

    // PUT: /api/products/{id}
    [HttpPut("{id}")]
    public ActionResult<Product> Update(long id, Product product) {
        if (id != product.ProductId) {
            return BadRequest("Product ID mismatch.");
        }

        var existingProduct = _repository.GetProductById(id);
        if (existingProduct == null) {
            return NotFound($"Product with ID {id} not found.");
        }

        existingProduct.Name = product.Name;
        existingProduct.Amount = product.Amount;
        existingProduct.Weight = product.Weight;
        existingProduct.IsComplete = product.IsComplete;
        existingProduct.ShoppingListId = product.ShoppingListId;

        _repository.UpdateProduct(existingProduct);
        _repository.SaveChanges();

        return NoContent();
    }

    // DELETE: /api/products/{id}
    [HttpDelete("{id}")]
    public ActionResult<Product> Delete(long id) {
        var product = _repository.GetProductById(id);
        if (product == null) {
            return NotFound($"Product with ID {id} not found.");
        }

        _repository.RemoveProduct(product);
        _repository.SaveChanges();

        return NoContent();
    }
}