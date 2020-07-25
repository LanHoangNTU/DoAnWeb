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
    public class THANHPHOesController : Controller
    {
        private MTPKEntities1 db = new MTPKEntities1();

        // GET: THANHPHOes
        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LoginForm", "QuanTri");
            }
            return View(db.THANHPHOes.ToList());
        }

        // GET: THANHPHOes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THANHPHO tHANHPHO = db.THANHPHOes.Find(id);
            if (tHANHPHO == null)
            {
                return HttpNotFound();
            }
            return View(tHANHPHO);
        }

        public string GetNewId()
        {
            if (db.THANHPHOes.Count() <= 0)
            {
                return "TP00000001";
            }

            string id = db.THANHPHOes.Select(m => m.MATP).Max();
            int parsed = int.Parse(id.Substring(2, 8)) + 1;
            return "TP" + parsed.ToString("00000000");
        }

        // GET: THANHPHOes/Create
        public ActionResult Create()
        {
            ViewBag.MATP = GetNewId();
            return View();
        }

        // POST: THANHPHOes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MATP,TENTP")] THANHPHO tHANHPHO)
        {
            ViewBag.MATP = GetNewId();
            if (ModelState.IsValid)
            {
                tHANHPHO.MATP = GetNewId();
                db.THANHPHOes.Add(tHANHPHO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tHANHPHO);
        }

        // GET: THANHPHOes/Edit/5
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
            THANHPHO tHANHPHO = db.THANHPHOes.Find(id);
            if (tHANHPHO == null)
            {
                return HttpNotFound();
            }
            return View(tHANHPHO);
        }

        // POST: THANHPHOes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MATP,TENTP")] THANHPHO tHANHPHO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tHANHPHO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tHANHPHO);
        }

        // GET: THANHPHOes/Delete/5
        public ActionResult Delete(string id)
        {
            THANHPHO tHANHPHO = db.THANHPHOes.Find(id);
            if (tHANHPHO.QUANHUYENs.Count > 0)
            {
                return RedirectToAction("Index");
            }
            db.THANHPHOes.Remove(tHANHPHO);
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
