using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Controllers
{
    public class PHUONGXAsController : Controller
    {
        private MTPKEntities1 db = new MTPKEntities1();

        // GET: PHUONGXAs
        public ActionResult Index()
        {
            var pHUONGXAs = db.PHUONGXAs.Include(p => p.QUANHUYEN);
            return View(pHUONGXAs.ToList());
        }

        // GET: PHUONGXAs/Details/5
        public ActionResult Details(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LoginForm", "QuanTri");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHUONGXA pHUONGXA = db.PHUONGXAs.Find(id);
            if (pHUONGXA == null)
            {
                return HttpNotFound();
            }
            return View(pHUONGXA);
        }
        public string GetNewId()
        {
            if (db.PHUONGXAs.Count() <= 0)
            {
                return "PX00000001";
            }
            string id = db.PHUONGXAs.Select(m => m.MAPX).Max();
            int parsed = int.Parse(id.Substring(2, 8)) + 1;
            return "PX" + parsed.ToString("00000000");
        }
        // GET: PHUONGXAs/Create
        public ActionResult Create(string id)
        {
            ViewBag.MAPX = GetNewId();
            ViewBag.MAQH = db.QUANHUYENs.Find(id);
            return View();
        }

        // POST: PHUONGXAs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAPX,MAQH,TENPX")] PHUONGXA pHUONGXA)
        {
            if (ModelState.IsValid)
            {
                pHUONGXA.MAPX = GetNewId();
                db.PHUONGXAs.Add(pHUONGXA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MAPX = GetNewId();
            ViewBag.MAQH = db.QUANHUYENs.Find(pHUONGXA.MAQH);
            return View(pHUONGXA);
        }

        // GET: PHUONGXAs/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LoginForm", "QuanTri");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHUONGXA pHUONGXA = db.PHUONGXAs.Find(id);
            if (pHUONGXA == null)
            {
                return HttpNotFound();
            }
            ViewBag.MAQH = db.QUANHUYENs.Find(pHUONGXA.MAQH);
            return View(pHUONGXA);
        }

        // POST: PHUONGXAs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAPX,MAQH,TENPX")] PHUONGXA pHUONGXA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pHUONGXA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MAQH = db.QUANHUYENs.Find(pHUONGXA.MAQH);
            return View(pHUONGXA);
        }

        // GET: PHUONGXAs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHUONGXA pHUONGXA = db.PHUONGXAs.Find(id);
            if (pHUONGXA == null)
            {
                return HttpNotFound();
            }
            return View(pHUONGXA);
        }

        // POST: PHUONGXAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PHUONGXA pHUONGXA = db.PHUONGXAs.Find(id);
            db.PHUONGXAs.Remove(pHUONGXA);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
