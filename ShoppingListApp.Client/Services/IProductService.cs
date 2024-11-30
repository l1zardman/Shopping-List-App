using ShoppingListApp.Models;

namespace ShoppingListApp.Client.Services;

public interface IProductService {
    Task<List<Product>> GetAllProducts();
    Task<Product?> GetProductById(long id);
    Task<Product?> CreateProduct(Product product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(long id);
}