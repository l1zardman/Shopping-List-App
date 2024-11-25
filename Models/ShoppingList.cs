using System.ComponentModel.DataAnnotations;

namespace Models;

public class ShoppingList {
    public long ShoppingListId { get; set; }

    public string Name { get; set; } = "new";
    
    public DateOnly Date { get; set; }

    public List<Product> Products { get; set; } = new();
}