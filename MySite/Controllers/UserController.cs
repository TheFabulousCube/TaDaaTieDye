using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MySite.Helpers;
using MySite.Models;

namespace MySite.Controllers
{
    public class UserController : Controller
    {
        private TaDaaTieDyeTPCEntities db = new TaDaaTieDyeTPCEntities();
        private bool NewLogin = false;
        //
        // GET: /User/

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl, string HeadingMessage = "Login to your account:")
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.PageHeading = HeadingMessage;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Users user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (user.IsValid(user.Username, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.Username, user.RememberMe);
                    NewLogin = false;
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", user.Password);
                }
            }
            return View(user);
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


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }




        //
        // GET: /User/Create
        public ActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Users user, string returnUrl = "")
        {
            if (!db.Users.Any(u => u.Username == user.Username))
            {


                if (ModelState.IsValid)
                {
                    Users temp = new Users();
                    temp.Address = user.Address;
                    temp.City = user.City;
                    temp.Email = user.Email;
                    temp.Fname = user.Fname;
                    temp.Lname = user.Lname;
                    temp.Password = SHA1.Encode(user.Password);
                    temp.Role = "u";
                    temp.State = (user.State == null) ? null : (user.State).Substring(0, 2).ToUpper();
                    temp.Username = user.Username;
                    temp.Zip = user.Zip;
                    db.Users.Add(temp);
                    db.SaveChanges();
                    NewLogin = true;

                    return RedirectToAction("Login", new { HeadingMessage = "Please Login to your New account:" });
                }
            }
            else
            {
            ModelState.AddModelError("Username", "Sorry, that user name is taken, Please pick something else!");
            }
            ViewBag.returnUrl = returnUrl;
            return View(user);
        }

        //
        // GET: /User/Manage Link page to manage the account

        public ActionResult Manage()
        {
            var user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }

        //
        // GET: /User/Edit
        [HttpGet]
        [Authorize]
        public ActionResult Edit(int id)
        {
            var user = db.Users.Find(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                FormsAuthentication.SignOut();
                ViewBag.Message = "Dang.  Please log in again:";
                return View("Login");
            }
        }

        //
        // POST: /User/Edit/

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Users user)
        {

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        //
        // Get: 
        [HttpGet]
        [Authorize]
        public ActionResult Password(int id)
        {
            Users user = db.Users.Find(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                FormsAuthentication.SignOut();
                ViewBag.Message = "Dang.  Please log in again:";
                return View("Login");
            }

        }

        //
        // Post:
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Password(Users user)
        { //check for confirm pass & ModelState
            // Users user = db.Users.Where(u => u.Username == User.Identity.Name).FirstOrDefault();

            if (!user.IsValid(user.Username, user.oldPass))
            {
                ModelState.AddModelError("Password", "Your current password is incorrect.");
            }
            else
            {

                user.Password = user.newPass;
                if (TryValidateModel(user))
                {

                    user.Password = SHA1.Encode(user.Password);
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = "Sweet! Now log in to your account with your new password:";
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Login");

                }
                // ModelState isn't valid
                //ModelState.AddModelError("newPass", "Please check your new passwords.");
            }
            // user entered incorrrect current password (but newPass & confirmPass match)
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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