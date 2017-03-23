using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySite.Models;

namespace MySite.Controllers
{
    public class MagnetsController : Controller
    {
        private TaDaaTieDyeTPCEntities db = new TaDaaTieDyeTPCEntities();

        //
        // GET: /Magnets/

        public ActionResult Index(int current = 0, int perpage = 5)
        {   // _PageNavPartial will need these:
            ViewBag.current = current;
            ViewBag.perpage = perpage;
            ViewBag.total = db.Magnets.Count();

            var set = db.Magnets.OrderBy(m => m.ProdId).Skip(current).Take(perpage).ToList();
            return View(db.Magnets.OrderBy(m => m.ProdId).Skip(current).Take(perpage).ToList());
        }

        //
        // GET: /Magnets/Details/5

        public ActionResult Details(string id = null)
        {
            Magnets magnets = db.Magnets.Find(id);
            if (magnets == null)
            {
                return HttpNotFound();
            }
            return View(magnets);
        }

        //
        // GET: /Magnets/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Magnets/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Magnets magnets)
        {
            if (ModelState.IsValid)
            {
                db.Magnets.Add(magnets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(magnets);
        }

        //
        // GET: /Magnets/Edit/5

        public ActionResult Edit(string id = null)
        {
            Magnets magnets = db.Magnets.Find(id);
            if (magnets == null)
            {
                return HttpNotFound();
            }
            return View(magnets);
        }

        //
        // POST: /Magnets/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Magnets magnets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(magnets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(magnets);
        }

        //
        // GET: /Magnets/Delete/5

        public ActionResult Delete(string id = null)
        {
            Magnets magnets = db.Magnets.Find(id);
            if (magnets == null)
            {
                return HttpNotFound();
            }
            return View(magnets);
        }

        //
        // POST: /Magnets/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Magnets magnets = db.Magnets.Find(id);
            db.Magnets.Remove(magnets);
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