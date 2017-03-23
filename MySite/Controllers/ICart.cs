using MySite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MySite.Controllers
{
    public interface ICart : Controller
    {
        public ActionResult AddToCart();
        public ActionResult Remove();
        public ActionResult ShowCart();
        public List<CartItems> GetCart();
        public ActionResult Update();
        public ActionResult Checkout();
    }

    public class DBCart : ICart
    {
        private TaDaaTieDyeTPCEntities db = new TaDaaTieDyeTPCEntities();
        public ActionResult ShowCart(string returnUrl)
        {
            // routing:
            //ViewBag.returnUrl = returnUrl;
            var user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            var items = GetCart(user);
            decimal total = 0;
            foreach (var item in items)
            {
                total += item.subTotal;
            }
            //ViewBag.Total = "Your total is: $" + total + ".";
            return View(items);

        }
        private List<CartItems> GetCart(Users user)
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

        public ActionResult AddToCart()
        {
            throw new NotImplementedException();
        }

        public ActionResult Remove()
        {
            throw new NotImplementedException();
        }

        public ActionResult ShowCart()
        {
            throw new NotImplementedException();
        }

        public List<CartItems> GetCart()
        {
            throw new NotImplementedException();
        }

        public ActionResult Update()
        {
            throw new NotImplementedException();
        }

        public ActionResult Checkout()
        {
            throw new NotImplementedException();
        }
    }

    public class SessionCart : ICart
    {

        public ActionResult ShowCart(string returnUrl)
        {
            // routing:
            //ViewBag.returnUrl = returnUrl;
            Session["myStuff"] = "first";
            var items = GetCart();
            decimal total = 0;
            foreach (var item in items)
            {
                total += item.subTotal;
            }
            //ViewBag.Total = "Your total is: $" + total + ".";
            return View(items);

        }
        private List<CartItems> GetCart()
        {
            TaDaaTieDyeTPCEntities db = new TaDaaTieDyeTPCEntities();
            var mags = from c in db.Cart
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

            var cloth = from c in db.Cart
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
        public ActionResult AddToCart(string id = null, int qty = 1, string returnURL = "Index")
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
                //RedirectToAction("Remove", id);
            }

                Cart line = new Cart();
                if (id != null)
                {

                    // User is logged in, check if this line already exists
                    //line = Cart.Where(c.ProdID == id).FirstOrDefault();
                    if (line != null)
                    { // line already exists, simply add to the Qty
                        line.Qty += qty;
                    }
                    else
                    { // new item for the user, create new line and qty
                        Cart tempCart = new Cart();
                        string ProdId = id;
                        tempCart.ProdID = ProdId;
                        tempCart.Qty = qty;
                        //db.Cart.Add(tempCart);

                    }
                    if (ModelState.IsValid)
                    {
                        //db.SaveChanges();
                        TempData["UserMessage"] = "Added " + qty + " " + id + "(s) to your cart.";
                        return RedirectToLocal(returnURL);
                    }
                }


                TempData["UserMessage"] = "Sorry, there was a problem adding to your cart.";
                return RedirectToLocal(returnURL);

            }
        public ActionResult Remove()
        {
            throw new NotImplementedException();
        }

        public ActionResult Update()
        {
            throw new NotImplementedException();
        }

        public ActionResult Checkout()
        {
            throw new NotImplementedException();
        }
    }
}
