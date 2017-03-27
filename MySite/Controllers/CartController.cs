using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using MySite.Models;

namespace MySite.Controllers
{

    public class CartController : Controller
    {
        private TaDaaTieDyeTPCEntities db = new TaDaaTieDyeTPCEntities();
        //
        // GET: /Cart/
        public CartFactory factory = new CartFactory();
        public ICartRepository CurrentCart;

        public ActionResult Index()
        {

            return ShowCart();
        }

        public ActionResult AddToCart(string id)
        {
            var user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            CurrentCart = factory.createCart(user);

            TempData["UserMessage"] = CurrentCart.AddToCart(id, 1);
            string retUrl = HttpContext.Session["Sender"].ToString();
            return RedirectToLocal(retUrl);
        }

        public ActionResult CombineCarts()
        {


            return RedirectToAction("CombineCarts", "Cart");
        }

        [HttpPost]
        public ActionResult Update(CartItems item)
        {
            var user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            CurrentCart = factory.createCart(user);

            TempData["UserMessage"] = CurrentCart.Update(item);
            string retUrl = HttpContext.Session["Sender"].ToString();
            return RedirectToAction("ShowCart");
        }

        [HttpPost]
        public ActionResult Remove(string id)
        {
            var user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            CurrentCart = factory.createCart(user);
            TempData["UserMessage"] = CurrentCart.Remove(id);
            return RedirectToAction("ShowCart");
        }

        public ActionResult ShowCart()
        {
            var user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            CurrentCart = factory.createCart(user);
            var items = CurrentCart.GetCart(user);
            decimal total = 0;
            foreach (var item in items)
            {
                total += item.subTotal;
            }
            ViewBag.Total = $"Your total is: ${total}.";
            return View(items);

        }



        public ActionResult RedirectToLocal(string returnUrl)
        {
            TempData["UserMessage"] = TempData["UserMessage"];
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult ContinueShopping()
        {
            return RedirectToLocal(Session["Sender"].ToString());
        }

        //
        // GET: /Cart/
        public ActionResult Checkout(IEnumerable<CartItems> cart)
        {
            var user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                CurrentCart = new SessionCart();
            }
            else
            {
                CurrentCart = new DBCart(user);
            }
            var items = CurrentCart.GetCart(user);
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
                new KeyValuePair<string, string>("RETURNURL", "http://tadaatiedye.com/Cart/CheckoutComplete"),
                new KeyValuePair<string, string>("CANCELURL", "http://tadaatiedye.com/Cart/CheckoutCancelled"),
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


    }
}