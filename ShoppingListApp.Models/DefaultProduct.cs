using System.ComponentModel.DataAnnotations;

namespace ShoppingListApp.Models;

public class DefaultProduct {
    [Key]
    public long DefaultProductId { get; set; }
    
    [Required(ErrorMessage = "Name of DefaultProduct is required")]
    [MaxLength(50, ErrorMessage = "Name of DefaultProduct must be less than 50 characters")]
    public string? Name { get; set; }
}