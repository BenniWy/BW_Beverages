using BW_Beverages.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BW_Beverages.Data.Models;

namespace BW_Beverages.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ShoppingCart _shoppingCart;


        public OrderRepository(AppDbContext appDbContext, ShoppingCart shoppingCart)
        {
            _appDbContext = appDbContext;
            _shoppingCart = shoppingCart;
        }


        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.UtcNow;

            _appDbContext.Orders.Add(order);

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;

            if (shoppingCartItems != null)
            {
                foreach (var shoppingCartItem in shoppingCartItems)
                {
                    if (shoppingCartItem.Drink != null)
                    {
                        var orderDetail = new OrderDetail()
                        {
                            Amount = shoppingCartItem.Amount,
                            DrinkId = shoppingCartItem.Drink.DrinkId,
                            OrderId = order.OrderId,
                            Price = shoppingCartItem.Drink.Price
                        };
                        order.OrderDetails.Add(orderDetail);
                        _appDbContext.OrderDetails.Add(orderDetail);
                    }
                }
            }
            
            _appDbContext.SaveChanges();
        }
    }

}