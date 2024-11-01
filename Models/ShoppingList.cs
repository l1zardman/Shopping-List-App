using System.ComponentModel.DataAnnotations;

namespace Models;

public class ShoppingList {
    [Key]
    public long ShoppingListId { get; set; }

    public string Name { get; set; } = "new";

    public ICollection<Product>? Products { get; set; }
}