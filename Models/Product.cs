using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class Product {
    public long ProductId { get; set; }
    
    [Required(ErrorMessage = "Name of Product is required")]
    [MaxLength(50, ErrorMessage = "Name of Product must be less than 50 characters")]
    public string? Name { get; set; }
    
    [Required(ErrorMessage = "Amount of Product is required")]
    [Range(0, 100, ErrorMessage = "Amount of Product must be between 0 and 100")]
    public int Amount { get; set; }

    [Required(ErrorMessage = "Weight of Product is required")]
    [Range(0, 100, ErrorMessage = "Weight of Product must be between 0 and 100")]
    public decimal Weight { get; set; }

    public bool IsComplete { get; set; } = false;
    
    public long ShoppingListId { get; set; }

    public ShoppingList? ShoppingList { get; set; }
}