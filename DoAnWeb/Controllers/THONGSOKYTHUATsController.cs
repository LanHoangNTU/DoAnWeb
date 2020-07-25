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
    public class THONGSOKYTHUATsController : Controller
    {
        private MTPKEntities1 db = new MTPKEntities1();

        // GET: THONGSOKYTHUATs
        public ActionResult Index()
        {
            var tHONGSOKYTHUATs = db.THONGSOKYTHUATs.Include(t => t.MATHANG);
            return View(tHONGSOKYTHUATs.ToList());
        }

        // GET: THONGSOKYTHUATs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONGSOKYTHUAT tHONGSOKYTHUAT = db.THONGSOKYTHUATs.Find(id);
            if (tHONGSOKYTHUAT == null)
            {
                return HttpNotFound();
            }
            return View(tHONGSOKYTHUAT);
        }

        // GET: THONGSOKYTHUATs/Create
        public ActionResult Create(string id)
        {
            ViewBag.MAMH = db.MATHANGs.Find(id);
            return View();
        }

        private string GetTechMaxId()
        {
            if (db.THONGSOKYTHUATs.Count() > 0)
            {
                var id = long.Parse(db.THONGSOKYTHUATs.Select(m => m.MATS).Max()) + 1;
                return id.ToString("0000000000");
            }
            return "0000000000";
        }

        // POST: THONGSOKYTHUATs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MATS,MAMH,TIEUDE,NOIDUNG")] THONGSOKYTHUAT tHONGSOKYTHUAT)
        {
            if (ModelState.IsValid)
            {
                tHONGSOKYTHUAT.MATS = GetTechMaxId();
                db.THONGSOKYTHUATs.Add(tHONGSOKYTHUAT);
                db.SaveChanges();
                return RedirectToAction("TechInfo", "QuanTri", new { id = tHONGSOKYTHUAT.MAMH });
            }

            ViewBag.MAMH = new SelectList(db.MATHANGs, "MAMH", "MALOAI", tHONGSOKYTHUAT.MAMH);
            return View(tHONGSOKYTHUAT);
        }

        // GET: THONGSOKYTHUATs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THONGSOKYTHUAT tHONGSOKYTHUAT = db.THONGSOKYTHUATs.Find(id);
            if (tHONGSOKYTHUAT == null)
            {
                return HttpNotFound();
            }
            ViewBag.MAMH = new SelectList(db.MATHANGs, "MAMH", "MALOAI", tHONGSOKYTHUAT.MAMH);
            return View(tHONGSOKYTHUAT);
        }

        // POST: THONGSOKYTHUATs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MATS,MAMH,TIEUDE,NOIDUNG")] THONGSOKYTHUAT tHONGSOKYTHUAT)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tHONGSOKYTHUAT).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TechInfo", "QuanTri", tHONGSOKYTHUAT.MAMH);
            }
            ViewBag.MAMH = new SelectList(db.MATHANGs, "MAMH", "MALOAI", tHONGSOKYTHUAT.MAMH);
            return View(tHONGSOKYTHUAT);
        }

        public ActionResult Delete(string id)
        {
            var item = db.THONGSOKYTHUATs.Find(id);
            string MAMH = item.MAMH;
            db.THONGSOKYTHUATs.Remove(item);
            db.SaveChanges();
            return RedirectToAction("TechInfo", "QuanTri", new { id = MAMH });
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
