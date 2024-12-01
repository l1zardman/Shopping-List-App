using Microsoft.AspNetCore.Mvc;
using ShoppingListApp.Api.Configuration;
using ShoppingListApp.Api.DatabaseAccess;

namespace ShoppingListApp.Api.Controllers;

// [Route("api/[controller]")]
// public class ResetController : ControllerBase {
//     private readonly ShoppingListDbContext _context;
//
//     public ResetController(ShoppingListDbContext context) {
//         _context = context;
//     }
//
//     [HttpPost]
//     public IActionResult Reset() {
//         SeedData.EnsurePopulated(_context);
//         return Ok();
//     }
// }