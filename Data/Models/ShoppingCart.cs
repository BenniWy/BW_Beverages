using BW_Beverages.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BW_Beverages.Data.Models{
    public class ShoppingCart{
        private readonly AppDbContext _appDbContext;
        private ShoppingCart (AppDbContext appDbContext){
            _appDbContext = appDbContext;
        }
        public string? ShoppingCartId { get; set; }
        public List<ShoppingCartItem>? ShoppingCartItems { get; set; }

        public static ShoppingCart GetCart(IServiceProvider services)
        {

            var httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();

            ISession? session = httpContextAccessor?.HttpContext?.Session;

            var context = services.GetService<AppDbContext>();

            if (context != null)
            {
                string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

                session?.SetString("CartId", cartId);

                return new ShoppingCart(context) { ShoppingCartId = cartId };
            }
            else
            {
                throw new InvalidOperationException("AppDbContext is null.");
            }
        }


        public void AddToCart(Drink drink, int amount)
        {
            var shoppingCartItem =
                _appDbContext.ShoppingCartItems.SingleOrDefault(
                    s => s.Drink != null && s.Drink.DrinkId == drink.DrinkId && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Drink = drink,
                    Amount = 1
                };

                _appDbContext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _appDbContext.SaveChanges();
        }

        public int RemoveFromCart(Drink drink)
        {
            var shoppingCartItem =
                _appDbContext.ShoppingCartItems.SingleOrDefault(
                    s => s.Drink != null && s.Drink.DrinkId == drink.DrinkId && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }

                _appDbContext.SaveChanges();
            }

            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems =
                       _appDbContext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(s => s.Drink)
                           .ToList());
        }

        public void ClearCart()
        {
            var cartItems = _appDbContext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _appDbContext.ShoppingCartItems.RemoveRange(cartItems);

            _appDbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var items = _appDbContext.ShoppingCartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .ToList();

            var total = items
                .Select(c => c.Drink?.Price * c.Amount)
                .Sum();

            return total ?? 0m;
        }
    }
}