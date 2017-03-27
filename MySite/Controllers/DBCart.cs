using MySite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Web.UI.WebControls;
using MySite.Controllers;

namespace MySite
{
    public class DBCart : ICartRepository
    {
        private TaDaaTieDyeTPCEntities db = new TaDaaTieDyeTPCEntities();
        private Users _user { get; set; }

        public DBCart(Users user)
        {
            // TODO: Complete member initialization
            this._user = user;
        }

        public List<CartItems> GetCart(Users user)
        {
            TaDaaTieDyeTPCEntities db = new TaDaaTieDyeTPCEntities();
            var mags = from c in db.Cart
                       join m in db.Magnets on c.ProdID equals m.ProdId
                       where c.UserId == user.Id
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

            var cloth = from c in db.Cart
                        join cl in db.Clothing on c.ProdID equals cl.ProdId
                        where c.UserId == user.Id
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
                    Users user = db.Users.Where(u => u.Username == _user.Username).FirstOrDefault();

                    // User is logged in, check if this line already exists
                    line = db.Cart.Where(c => c.UserId == user.Id && c.ProdID == id).FirstOrDefault();
                    if (line != null)
                    { // line already exists, simply add to the Qty
                        line.Qty += qty;
                    }
                    else
                    { // new item for the user, create new line and qty
                        Cart tempCart = new Cart();
                        string ProdId = id;
                        tempCart.UserId = user.Id;
                        tempCart.ProdID = ProdId;
                        tempCart.Qty = qty;
                        db.Cart.Add(tempCart);
                        db.SaveChanges();

                    }
                        return $"Added {qty} {id}(s) to your cart.";

                }
                return $"Sorry, there was a problem adding to your cart.";



        }

        public string Remove(string id)
        {
            string item = id;

                // line already exists in cart
                Cart cart = db.Cart.Find(_user.Id, item);
                db.Cart.Remove(cart);
                db.SaveChanges();
                //TempData["UserMessage"] = "Removed " + item + "(s) in your cart.";
                return $"Item {id} has been removed from your cart";

            //}
            // User isn't logged in
           // return "Sorry, there was a problem removing your item from the cart.";
        }

        public string Update(CartItems item)
        {
            string userMessage = "";
            /**************************************************
             * 
             * 
             * Remember to come back and check avail Qty before submitting
             * to cart.  Decide if users can reserve items or only
             * check available quantity at checkout.
             * 
             * ***********************************************/
            if (item.ItemQty <= 0)
            {
                return Remove(item.ItemId);
            }

            if (item.ItemQty > item.AvailQty)
            {
                userMessage = $"Sorry, there are only {item.AvailQty} {item.ItemId}(s) available.\n";
                item.ItemQty = item.AvailQty;
            }
                if (db.Cart.Any(c => c.UserId == _user.Id && c.ProdID == item.ItemId))
                {
                    // line already exists in cart
                    Cart cart = db.Cart.Find(_user.Id, item.ItemId);
                    cart.Qty = item.ItemQty;
                    db.Entry(cart).State = EntityState.Modified;
                    db.SaveChanges();
                    userMessage += $"Adjusted {cart.Qty} {cart.ProdID}(s) in your cart.";
                }
                else
                { // I don't know how the line could not exist, but check anyway.

                    userMessage = $"Ummm . . I added (s) to your cart.";
                }
            
            // User isn't logged in
            return userMessage;
        }

        public ActionResult Checkout()
        {
            throw new NotImplementedException();
        }
    }

}