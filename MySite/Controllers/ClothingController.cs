using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MySite.Models;

//using System.Data.SqlClient;
//using MySql.Data.MySqlClient;

namespace MySite.Controllers
{
    public class ClothingController : Controller
    {
        private TaDaaTieDyeTPCEntities db = new TaDaaTieDyeTPCEntities();

        //
        // GET: /Clothing/

        public ActionResult Index()
        {

            return View();
        }

        //
        // GET: Clothing/Size

        public ActionResult Size(int current = 0, int perpage = 5, string size = "")
        {   // _PageNavPartial will need these:
            ViewBag.current = current;
            ViewBag.perpage = perpage;
            ViewBag.sender = "Size";

            // Drop down filter will need these:
            ViewBag.sizeQuery = from s in db.Size_Lookup
                                orderby s.Size
                                select s;
            ViewBag.size = size;

            var clothing = db.Clothing.Include(c => c.Catagory_Lookup).Where(c => c.Size.Contains(size)).Include(c => c.Size_Lookup).Include(c => c.Sleeve_Lookup);



            // Don't take the count too early!  It will be wrong!
            ViewBag.total = clothing.Count();

            clothing = clothing.OrderBy(c => c.ProdId).Skip(current).Take(perpage);

            return View(clothing.ToList());
        }

        //
        // GET: Clothing/Type

        public ActionResult Type(int current = 0, int perpage = 5, string catagory = "")
        {   // _PageNavPartial will need these:
            ViewBag.current = current;
            ViewBag.perpage = perpage;
            ViewBag.sender = "Type";


            // Drop down filter will need these:
            ViewBag.catagoryQuery = from c in db.Catagory_Lookup
                                    orderby c.Type
                                    select c;
            ViewBag.catagory = catagory;

            var clothing = db.Clothing.Include(c => c.Catagory_Lookup).Where(c => c.Catagory.Contains(catagory)).Include(c => c.Size_Lookup).Include(c => c.Sleeve_Lookup);
            
            // Don't take the count too early!  It will be wrong!
            ViewBag.total = clothing.Count();

            clothing = clothing.OrderBy(c => c.ProdId).Skip(current).Take(perpage);

            return View(clothing.ToList());
        }

        //
        // GET: /Clothing/Browse

        public ActionResult Browse(int current = 0, int perpage = 10)
        {   // _PageNavPartial will need these:
            ViewBag.current = current;
            ViewBag.perpage = perpage;
            ViewBag.sender = "Browse";

            var clothing = db.Clothing.Include(c => c.Catagory_Lookup).Include(c => c.Size_Lookup).Include(c => c.Sleeve_Lookup);

            ViewBag.total = clothing.Count();

            clothing = clothing.OrderBy(c => c.ProdId).Skip(current).Take(perpage);

            return View(clothing.ToList());
        }

        //
        // GET: /Clothing/Details/5

        public ActionResult Details(string id = null, string returnUrl = "~/Index", string catagory = null)
        {

            ViewBag.sender = Session["Sender"];
            ViewBag.catagory = catagory;
            Clothing clothing = db.Clothing.Find(id);
            if (clothing == null)
            {
                return HttpNotFound();
            }
            return View(clothing);
        }

        public ActionResult BackToList()
        {

            return RedirectToLocal(Session["Sender"].ToString());
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
                return RedirectToAction("Index", "Clothing");
            }

        }

        //
        // GET: /Clothing/Create

        public ActionResult Create()
        {
            ViewBag.Catagory = new SelectList(db.Catagory_Lookup, "CatId", "Type");
            ViewBag.Size = new SelectList(db.Size_Lookup, "SizeId", "Size");
            ViewBag.SleeveLength = new SelectList(db.Sleeve_Lookup, "SleeveID", "Sleeve_Length");
            return View();
        }

        //
        // POST: /Clothing/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Clothing clothing)
        {
            if (ModelState.IsValid)
            {
                db.Clothing.Add(clothing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Catagory = new SelectList(db.Catagory_Lookup, "CatId", "Type", clothing.Catagory);
            ViewBag.Size = new SelectList(db.Size_Lookup, "SizeId", "Size", clothing.Size);
            ViewBag.SleeveLength = new SelectList(db.Sleeve_Lookup, "SleeveID", "Sleeve_Length", clothing.SleeveLength);
            return View(clothing);
        }

        //
        // GET: /Clothing/Edit/5

        public ActionResult Edit(string id = null)
        {
            Clothing clothing = db.Clothing.Find(id);
            if (clothing == null)
            {
                return HttpNotFound();
            }
            ViewBag.Catagory = new SelectList(db.Catagory_Lookup, "CatId", "Type", clothing.Catagory);
            ViewBag.Size = new SelectList(db.Size_Lookup, "SizeId", "Size", clothing.Size);
            ViewBag.SleeveLength = new SelectList(db.Sleeve_Lookup, "SleeveID", "Sleeve_Length", clothing.SleeveLength);
            return View(clothing);
        }

        //
        // POST: /Clothing/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Clothing clothing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clothing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Catagory = new SelectList(db.Catagory_Lookup, "CatId", "Type", clothing.Catagory);
            ViewBag.Size = new SelectList(db.Size_Lookup, "SizeId", "Size", clothing.Size);
            ViewBag.SleeveLength = new SelectList(db.Sleeve_Lookup, "SleeveID", "Sleeve_Length", clothing.SleeveLength);
            return View(clothing);
        }

        //
        // GET: /Clothing/Delete/5

        public ActionResult Delete(string id = null)
        {
            Clothing clothing = db.Clothing.Find(id);
            if (clothing == null)
            {
                return HttpNotFound();
            }
            return View(clothing);
        }

        //
        // POST: /Clothing/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Clothing clothing = db.Clothing.Find(id);
            db.Clothing.Remove(clothing);
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