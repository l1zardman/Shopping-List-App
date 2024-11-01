using Models;

namespace ShoppingListApi.Models;

public interface IRepository {
    public IQueryable<Product> Products { get; }
    public IQueryable<ShoppingList> ShoppingLists { get; }
    public IQueryable<DefaultProduct> DefaultProducts { get; }
}