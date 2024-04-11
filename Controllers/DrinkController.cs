using BW_Beverages.Data;
using BW_Beverages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BW_Beverages.Controllers;

[ApiController]
[Route("[controller]")]
public class DrinksController : ControllerBase{
    private readonly ILogger<DrinksController> _logger;
    private readonly ApiDbContext _context;

    public DrinksController(
        ILogger<DrinksController> logger,
        ApiDbContext context){
        _logger=logger;
        _context=context;
    }

    [HttpGet(Name ="GetAllDrinks")]
    public async Task<IActionResult> Get(){
        var drink = new Drink(){
            DrinkId = 1234,
            Name = "Water",
            ShortDescription = "H2O",
            LongDescription = "My H2O",
            Price = 4.5m,
            ImageUrl = "www",
            ImageThumbnailUrl = "www",
            IsPreferredDrink = true,
            InStock = true,
            CategoryId = 4321
        };

        _context.Add(drink);
        await _context.SaveChangesAsync();

        var allDrinks = await _context.Drinks.ToListAsync();

        return Ok(allDrinks);

    }
}