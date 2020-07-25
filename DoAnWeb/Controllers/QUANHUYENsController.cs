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
    public class QUANHUYENsController : Controller
    {
        private MTPKEntities1 db = new MTPKEntities1();

        // GET: QUANHUYENs
        public ActionResult Index()
        {
            var qUANHUYENs = db.QUANHUYENs.Include(q => q.THANHPHO);
            return View(qUANHUYENs.ToList());
        }

        // GET: QUANHUYENs/Details/5
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
            QUANHUYEN qUANHUYEN = db.QUANHUYENs.Find(id);
            if (qUANHUYEN == null)
            {
                return HttpNotFound();
            }
            return View(qUANHUYEN);
        }

        public string GetNewId()
        {
            if (db.QUANHUYENs.Count() <= 0)
            {
                return "QH00000000";
            }
            string id = db.QUANHUYENs.Select(m => m.MAQH).Max();
            int parseId = int.Parse(id.Substring(2, 8)) + 1;
            return "QH" + parseId.ToString("00000000");
        }

        // GET: QUANHUYENs/Create
        public ActionResult Create(string id)
        {
            ViewBag.MATP = db.THANHPHOes.Find(id);
            ViewBag.MAQH = GetNewId();
            return View();
        }

        // POST: QUANHUYENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAQH,MATP,TENQH")] QUANHUYEN qUANHUYEN)
        {
            if (ModelState.IsValid)
            {
                qUANHUYEN.MAQH = GetNewId();
                db.QUANHUYENs.Add(qUANHUYEN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MATP = db.THANHPHOes.Find(qUANHUYEN.MATP);
            ViewBag.MAQH = GetNewId();
            return View(qUANHUYEN);
        }

        // GET: QUANHUYENs/Edit/5
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
            QUANHUYEN qUANHUYEN = db.QUANHUYENs.Find(id);
            if (qUANHUYEN == null)
            {
                return HttpNotFound();
            }
            ViewBag.MATP = db.THANHPHOes.Find(id);
            return View(qUANHUYEN);
        }

        // POST: QUANHUYENs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAQH,MATP,TENQH")] QUANHUYEN qUANHUYEN)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qUANHUYEN).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "THANHPHOes", new { id = qUANHUYEN.MATP});
            }
            ViewBag.MATP = db.THANHPHOes.Find(qUANHUYEN.MATP);
            return View(qUANHUYEN);
        }

        // GET: QUANHUYENs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QUANHUYEN qUANHUYEN = db.QUANHUYENs.Find(id);
            if (qUANHUYEN == null)
            {
                return HttpNotFound();
            }
            return View(qUANHUYEN);
        }

        // POST: QUANHUYENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            QUANHUYEN qUANHUYEN = db.QUANHUYENs.Find(id);
            db.QUANHUYENs.Remove(qUANHUYEN);
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
