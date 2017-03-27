using MySite.Models;
using System;

namespace MySite.Controllers
{
    public class CartFactory 
    {
        public ICartRepository createCart(Users user)
        {
            ICartRepository CurrentCart;

            if (user == null)
            {
                CurrentCart = new SessionCart();
            }
            else
            {
                CurrentCart = new DBCart(user);
            }
            return CurrentCart;
        }
    }
}