using BW_Beverages.Data.Interfaces;
using BW_Beverages.Data.Models;
using BW_Beverages.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BW_Beverages.Controllers
{
    [Route("api/[controller]")]
    public class DrinkDataController : Controller
    {
        private readonly IDrinkRepository _drinkRepository;

        public DrinkDataController(IDrinkRepository drinkRepository)
        {
            _drinkRepository = drinkRepository;
        }

        [HttpGet]
        public IEnumerable<DrinkViewModel> LoadMoreDrinks()
        {
            IEnumerable<Drink> dbDrinks = _drinkRepository.Drinks.OrderBy(p => p.DrinkId).Take(10);

            if (dbDrinks != null)
            {
                List<DrinkViewModel> drinks = new List<DrinkViewModel>();

                foreach (var dbDrink in dbDrinks)
                {
                    drinks.Add(MapDbDrinkToDrinkViewModel(dbDrink));
                }

                return drinks;
            }

            return Enumerable.Empty<DrinkViewModel>();
        }


        private DrinkViewModel MapDbDrinkToDrinkViewModel(Drink dbDrink) => new DrinkViewModel()
        {
            DrinkId = dbDrink.DrinkId,
            Name = dbDrink.Name,
            Price = dbDrink.Price,
            ShortDescription = dbDrink.ShortDescription,
            ImageThumbnailUrl = dbDrink.ImageThumbnailUrl
        };

    }
}