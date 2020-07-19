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
    public class GIOHANGsController : Controller
    {
        private MTPKEntities1 db = new MTPKEntities1();

        // GET: GIOHANGs
        public ActionResult Index()
        {
            var gIOHANGs = db.GIOHANGs.Include(g => g.PHUONGTHUCTHANHTOAN).Include(g => g.PHUONGXA);
            return View(gIOHANGs.ToList());
        }

        // GET: GIOHANGs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIOHANG gIOHANG = db.GIOHANGs.Find(id);
            if (gIOHANG == null)
            {
                return HttpNotFound();
            }
            return View(gIOHANG);
        }

        // GET: GIOHANGs/Create
        public ActionResult Create()
        {
            ViewBag.MAPTTT = new SelectList(db.PHUONGTHUCTHANHTOANs, "MAPTTT", "TENPTTT");
            ViewBag.MATP = new SelectList(db.PHUONGXAs, "MAPX", "MAQH");
            return View();
        }

        // POST: GIOHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAGH,MATP,MAPTTT,TRANGTHAI,NGAYXUAT,TONGTIEN,HOTENKH,SDTKH,GIOITINHKH,LOINHAN,DIACHI")] GIOHANG gIOHANG)
        {
            if (ModelState.IsValid)
            {
                db.GIOHANGs.Add(gIOHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MAPTTT = new SelectList(db.PHUONGTHUCTHANHTOANs, "MAPTTT", "TENPTTT", gIOHANG.MAPTTT);
            ViewBag.MATP = new SelectList(db.PHUONGXAs, "MAPX", "MAQH", gIOHANG.MATP);
            return View(gIOHANG);
        }

        // GET: GIOHANGs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIOHANG gIOHANG = db.GIOHANGs.Find(id);
            if (gIOHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.MAPTTT = new SelectList(db.PHUONGTHUCTHANHTOANs, "MAPTTT", "TENPTTT", gIOHANG.MAPTTT);
            ViewBag.MATP = new SelectList(db.PHUONGXAs, "MAPX", "MAQH", gIOHANG.MATP);
            return View(gIOHANG);
        }

        // POST: GIOHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAGH,MATP,MAPTTT,TRANGTHAI,NGAYXUAT,TONGTIEN,HOTENKH,SDTKH,GIOITINHKH,LOINHAN,DIACHI")] GIOHANG gIOHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gIOHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MAPTTT = new SelectList(db.PHUONGTHUCTHANHTOANs, "MAPTTT", "TENPTTT", gIOHANG.MAPTTT);
            ViewBag.MATP = new SelectList(db.PHUONGXAs, "MAPX", "MAQH", gIOHANG.MATP);
            return View(gIOHANG);
        }

        // GET: GIOHANGs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GIOHANG gIOHANG = db.GIOHANGs.Find(id);
            if (gIOHANG == null)
            {
                return HttpNotFound();
            }
            return View(gIOHANG);
        }

        // POST: GIOHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GIOHANG gIOHANG = db.GIOHANGs.Find(id);
            db.GIOHANGs.Remove(gIOHANG);
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
