using MySite.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MySite.Controllers
{
    public interface ICartRepository 
    {
        string AddToCart(string id, int qty = 1);
        string Remove(string id);
        List<CartItems> GetCart(Users user);
        string Update(CartItems item);
        ActionResult Checkout();

    }


}
