using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MySite.Models;

namespace MySite.Controllers
{
    public class SessionCart : CartController, ICart
    {
            
    }
    public class CartController : Controller
    {
        private TaDaaTieDyeTPCEntities db = new TaDaaTieDyeTPCEntities();
        //
        // GET: /Cart/

        public ActionResult Index()
        {
            var user = db.Users.Where(u => u.Username == User.Identity.Name);

            return View();

        }

        [Authorize]
        public ActionResult ShowCart(string returnUrl)
        {
            // routing:
            ViewBag.returnUrl = returnUrl;
            var user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            var items = GetCart(user);
            decimal total = 0;
            foreach (var item in items)
            {
                total += item.subTotal;
            }
            ViewBag.Total = "Your total is: $" + total + ".";
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


        //
        // GET: /Cart/AddToCart
        [Authorize]
        public ActionResult AddToCart(string id = null, int qty = 1, string returnURL = "Index")
        {
            int CartIndex = 1;
            Session["FirstName{CartIndex}"] = "david";
            /**************************************************
             * 
             * Remember to come back and check avail Qty before submitting
             * to cart.  Decide if users can reserve items or only
             * check available quantity at checkout.
             * 
             * ***********************************************/
            if (qty <= 0)
            {
                RedirectToAction("Remove", id);
            }
            if (Request.IsAuthenticated)
            {
                Cart line = new Cart();
                if (id != null)
                {
                    Users user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();

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

                    }
                    if (ModelState.IsValid)
                    {
                        db.SaveChanges();
                        TempData["UserMessage"] = "Added " + qty + " " + id + "(s) to your cart.";
                        return RedirectToLocal(returnURL);
                    }
                }


                TempData["UserMessage"] = "Sorry, there was a problem adding to your cart.";
                return RedirectToLocal(returnURL);

            }
            // User isn't logged in
            return RedirectToAction("Login", "User", returnURL);
        }

        //
        // POST: /Cart/UpdateCart
        [HttpPost]
        [Authorize]
        public ActionResult Update(CartItems item)
        {
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
                TempData["UserMessage"] = "Sorry, there are only " + item.AvailQty + " " + item.ItemId + "(s) available.\n";
                item.ItemQty = item.AvailQty;
            }
            if (Request.IsAuthenticated)
            {// user is logged in
                var user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
                if (db.Cart.Any(c => c.UserId == user.Id && c.ProdID == item.ItemId))
                {
                    // line already exists in cart
                    Cart cart = db.Cart.Find(user.Id, item.ItemId);
                    cart.Qty = item.ItemQty;
                    db.Entry(cart).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["UserMessage"] += "Adjusted " + cart.Qty + " " + cart.ProdID + "(s) in your cart.";
                }
                else
                { // I don't know how the line could not exist, but check anyway.

                    TempData["UserMessage"] = "Ummm . . I added (s) to your cart.";
                }
                if (ModelState.IsValid)
                {
                    //db.SaveChanges();
                    return RedirectToAction("ShowCart");
                }
                TempData["UserMessage"] = "Sorry, there was a problem adding to your cart.";
                return RedirectToAction("ShowCart");
            }
            // User isn't logged in
            return RedirectToAction("Login", "User", "~/Cart/ShowCart");
        }

        //
        // POST: /Cart/RemoveFromCart
        [Authorize]
        [HttpPost]
        public ActionResult Remove(string id)
        {
            string item = id;
            if (Request.IsAuthenticated)
            {// user is logged in
                var user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();

                // line already exists in cart
                Cart cart = db.Cart.Find(user.Id, item);
                db.Cart.Remove(cart);
                db.SaveChanges();
                TempData["UserMessage"] = "Removed " + item + "(s) in your cart.";
                return RedirectToAction("ShowCart");

            }
            // User isn't logged in
            return RedirectToAction("Login", "User", "~/Cart/ShowCart");
        }

        public ActionResult RedirectToLocal(string returnUrl)
        {

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        //
        // GET: /Cart/
        public ActionResult Checkout(IEnumerable<CartItems> cart)
        {

            var user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            var items = GetCart(user);
            //check avail qty, then send to view.  Let the view handle hidden fields for PayPal
            decimal total = 0;
            foreach (var item in items)
            {
                item.ItemQty = (item.ItemQty > item.AvailQty) ? item.AvailQty : item.ItemQty;
                total += item.subTotal;
            }
            ViewBag.total = total;
            return View(items);
        }



        public ActionResult CheckoutComplete()
        {
            ViewBag.Message = "PayPal Transaction Completed";
            return View("Checkout");
        }


        public ActionResult CheckoutCancelled()
        {
            ViewBag.Message = "PayPal Transaction CANCELLED";
            return View("Checkout");
        }

        private async void SendRequest()
        {
            //Cart cart = db.Cart.Find();
            //if (cart == null)
            //{
            //    return HttpNotFound();
            //}






            //endpoint:
            //https://api-3t.sandbox.paypal.com/nvp


            //x-www-form-urlencoded

            //    Name-Value Pairs (NVP)

            // API setup
            var client = new HttpClient();
            string User = "cube - facilitator_api1.tadaatiedye.com";
            string Pass = "8WXRZ5LNE78CHVBB";
            string Sig = "AFcWxV21C7fd0v3bYYYRCpSSRl31AjlS0ifh2u2kKxC0wgmpDQJdyPGj";
            //static AppID value for the Sandbox:
            //    APP-80W284485P519543T

            // Required variables from cart:
            string total = "4.95";

            // API Operation
            var requestOperation = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("METHOD","SetExpressCheckout"),
                new KeyValuePair<string, string>("VERSION","204"),
                new KeyValuePair<string, string>("PAYMENTREQUEST_0_AMT", total),
                new KeyValuePair<string, string>("RETURNURL", "http://tadaatiedye.com/clothing/clothing.php"),
                new KeyValuePair<string, string>("CANCELURL", "http://tadaatiedye.com/statemagnets/statemagnets.php"),
                new KeyValuePair<string, string>("NOSHIPPING", "1"),
                new KeyValuePair<string, string>("ALLOWNOTE", "1"),
                new KeyValuePair<string, string>("BRANDNAME", "TaDaa Tie Dye")});



            // API Credentials
            var requestCredentials = new FormUrlEncodedContent(new[] { 
                new KeyValuePair<string, string>("USER", User), 
                new KeyValuePair<string, string>("PWD", Pass), 
                new KeyValuePair<string, string>("SIGNATURE", Sig) });

            // Create the HttpContent for the form to be posted.
            var requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("item_name_1", "Arizona"),
                new KeyValuePair<string, string>("item_number_1", "SMAZ"),
                new KeyValuePair<string, string>("quantity_1", "3"),
                new KeyValuePair<string, string>("amount_1", "1.99"),
   
                new KeyValuePair<string, string>("item_number_2", "SMTN"),
                new KeyValuePair<string, string>("item_name_2", "Tenneessee"),
                new KeyValuePair<string, string>("quatity_2", "2"),
                new KeyValuePair<string, string>("amount_2", "1.99"),});

            // Get the response.
            HttpResponseMessage response = await client.PostAsync("https://api-3t.sandbox.paypal.com/nvp", requestContent);

            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                Console.WriteLine(reader.ReadToEndAsync());
                // EXPECTED:
                //ACK: Success, Failure, each with warning
                // CORRELATIONID
                // TIMESTAMP
                // VERSION
                // BUILD
            }

        }


        //
        // GET: /Cart/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Cart cart = db.Cart.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        //
        // POST: /Cart/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Cart.Find(id);
            db.Cart.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}