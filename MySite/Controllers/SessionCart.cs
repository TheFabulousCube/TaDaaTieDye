using MySite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MySite.Controllers
{
    /***************************************************
     * SessionCart saves to:
     * Session[{id}] = qty, 
     * **************************************************/

    public class SessionCart :  ICartRepository
    {
        public SessionCart()
        {
            if (HttpContext.Current == null ||
    HttpContext.Current.Session == null ||
    HttpContext.Current.Session["Cart"] == null)
            {
                //HttpContext.Current.Session["Cart"] = "true";
            }
        }

        public List<CartItems> GetCart(Users user)
        {

            var db = new TaDaaTieDyeTPCEntities();
            List<Cart> things = new List<Cart>();
            foreach (string id in HttpContext.Current.Session.Keys)
            {
                if ((db.Magnets.FirstOrDefault(m => m.ProdId == id) != null) || (db.Clothing.FirstOrDefault(c => c.ProdId == id) != null))
                things.Add(new Cart{UserId = 0, ProdID = id, Qty = Convert.ToInt32(HttpContext.Current.Session[id].ToString())});
            }

            var mags = from c in things
                       join m in db.Magnets on c.ProdID equals m.ProdId
                       select new CartItems
                       {
                           ItemId = m.ProdId,
                           ItemPicture = m.ProdPicture,
                           ItemCatagory = m.Catagory,
                           ItemName = m.ProdName,
                           ItemCost = m.ProdPrice,
                           ItemQty = c.Qty,
                           AvailQty = m.ProdQty
                       };

            var cloth = from c in things
                        join cl in db.Clothing on c.ProdID equals cl.ProdId
                        select new CartItems
                        {
                            ItemId = cl.ProdId,
                            ItemPicture = cl.ProdPicture,
                            ItemCatagory = cl.Catagory,
                            ItemName = cl.Sleeve_Lookup.Sleeve_Length + " " + cl.Size_Lookup.Size,
                            ItemCost = cl.ProdPrice,
                            ItemQty = cl.ProdQty,
                            AvailQty = cl.ProdQty
                        };

            var stuff = mags.Concat(cloth);

            return stuff.ToList();

        }
        [Authorize]
        public string AddToCart(string id = null, int qty = 1)
        {
            /**************************************************
             * 
             * Remember to come back and check avail Qty before submitting
             * to cart.  Decide if users can reserve items or only
             * check available quantity at checkout.
             * 
             * ***********************************************/
            if (qty <= 0)
            {
                this.Remove(id);
            }

            Cart line = new Cart();
            if (id != null)
            {
                // Check if this line already exists
                if (HttpContext.Current.Session[id] != null)
                { // line already exists, simply add to the Qty
                    HttpContext.Current.Session[id] = ((Convert.ToInt32(HttpContext.Current.Session[id])) + 1).ToString();
                }
                else
                { // new item for the user, create new line and qty
                    HttpContext.Current.Session[id] = 1;

                }
            }
            return $"Added {qty} {id}(s) to your cart.";
        }


        public string Remove(string id)
        {
            HttpContext.Current.Session.Remove(id);
            return $"Item {id} has been removed from your cart";
        }

        public string Update(CartItems item)
        {
            string userMessage = "";
            if (item.ItemQty <= 0)
            {
                return Remove(item.ItemId);
            }

            if (item.ItemQty > item.AvailQty)
            {
                userMessage = $"Sorry, there are only {item.AvailQty} {item.ItemId}(s) available.\n";
                item.ItemQty = item.AvailQty;
            }
            HttpContext.Current.Session[item.ItemId] = item.ItemQty;
            userMessage += $"Adjusted {item.ItemQty} {item.ItemId}(s) in your cart.";
            
            return userMessage;
        }

        public ActionResult Checkout()
        {
            throw new NotImplementedException();
        }
    }
}