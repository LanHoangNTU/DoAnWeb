using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;
using Microsoft.Ajax.Utilities;

namespace DoAnWeb.Controllers
{
    public class GIOHANGsController : Controller
    {
        private MTPKEntities1 db = new MTPKEntities1();

        // GET: GIOHANGs
        [HttpGet]
        public ActionResult Index(string id = "")
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LoginForm", "QuanTri");
            }
            var gIOHANGs = db.GIOHANGs.Include(g => g.PHUONGTHUCTHANHTOAN).Include(g => g.PHUONGXA);
            ViewBag.MAGH = id;
            if (!id.IsNullOrWhiteSpace())
            {
                gIOHANGs.Where(m => m.MAGH.ToLower().Contains(id.ToLower()));
            }
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
            ViewBag.CTGH = db.CTGIOHANGs.Where(m => m.MAGH == gIOHANG.MAGH).ToList();
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

        public ActionResult Delete(string id)
        {
            GIOHANG gIOHANG = db.GIOHANGs.Find(id);
            db.GIOHANGs.Remove(gIOHANG);
            db.SaveChanges();
            return RedirectToAction("Bills", "QuanTri");
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
