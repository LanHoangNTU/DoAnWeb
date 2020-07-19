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
using PagedList;

namespace DoAnWeb.Controllers
{
    public class MATHANGsController : Controller
    {
        private MTPKEntities1 db = new MTPKEntities1();

        // GET: MATHANGs
        public ActionResult Index(int? page)
        {
            var mATHANGs = db.MATHANGs.Include(m => m.LOAIMATHANG).Include(m => m.NHASANXUAT);
            mATHANGs = mATHANGs.OrderByDescending(s => s.TENMH);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(mATHANGs.ToPagedList(pageNumber, pageSize));
        }

        // GET: MATHANGs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MATHANG mATHANG = db.MATHANGs.Find(id);
            if (mATHANG == null)
            {
                return HttpNotFound();
            }
            return View(mATHANG);
        }

        // GET: MATHANGs/Create
        public ActionResult Create()
        {
            ViewBag.MALOAI = new SelectList(db.LOAIMATHANGs, "MALOAI", "TENLOAI");
            ViewBag.MANSX = new SelectList(db.NHASANXUATs, "MANSX", "TENNSX");
            return View();
        }

        public string GetNewId()
        {
            if (db.MATHANGs.Count() <= 0)
            {
                return "MH00000000";
            }
            var id = db.MATHANGs.Select(m => m.MAMH).Max();
            return "MH" + (long.Parse(id.Substring(2, 8)) + 1).ToString("00000000");
        }
        // POST: MATHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAMH,MALOAI,MANSX,TENMH,ANH,DONGIA,MAU,GIAMGIA,GHICHU,GIOITHIEUMH")] MATHANG mATHANG)
        {
            var imgNV = Request.Files["Avatar"];
            //Lấy thông tin từ input type=file có tên Avatar
            string postedFileName = System.IO.Path.GetFileName(imgNV.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/Products/" + postedFileName);
            imgNV.SaveAs(path);
            if (ModelState.IsValid)
            {
                mATHANG.MAMH = GetNewId();
                mATHANG.ANH = imgNV.FileName;
                db.MATHANGs.Add(mATHANG);
                db.SaveChanges();
                return RedirectToAction("Index", "QuanTri");
            }

            ViewBag.MALOAI = new SelectList(db.LOAIMATHANGs, "MALOAI", "TENLOAI", mATHANG.MALOAI);
            ViewBag.MANSX = new SelectList(db.NHASANXUATs, "MANSX", "TENNSX", mATHANG.MANSX);
            return View(mATHANG);
        }

        // GET: MATHANGs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MATHANG mATHANG = db.MATHANGs.Find(id);
            if (mATHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.MALOAI = new SelectList(db.LOAIMATHANGs, "MALOAI", "TENLOAI", mATHANG.MALOAI);
            ViewBag.MANSX = new SelectList(db.NHASANXUATs, "MANSX", "TENNSX", mATHANG.MANSX);
            ViewBag.ANH = mATHANG.ANH;
            return View(mATHANG);
        }

        // POST: MATHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAMH,MALOAI,MANSX,TENMH,ANH,DONGIA,MAU,GIAMGIA,GHICHU,GIOITHIEUMH")] MATHANG mATHANG)
        {
            var imgNV = Request.Files["Avatar"];
            var target = db.MATHANGs.Find(mATHANG.MAMH);
            string postedFileName = System.IO.Path.GetFileName(imgNV.FileName);
            //Lu hình đại diện về Server
            var path = Server.MapPath("/Images/Products/" + postedFileName);
            if (postedFileName != null && !postedFileName.IsNullOrWhiteSpace())
                imgNV.SaveAs(path);
            else
                postedFileName = db.MATHANGs.Find(mATHANG.MAMH).ANH;
            if (ModelState.IsValid)
            {
                mATHANG.ANH = postedFileName;
                db.Entry(target).CurrentValues.SetValues(mATHANG);
                db.Entry(target).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "QuanTri");
            }
            ViewBag.MALOAI = new SelectList(db.LOAIMATHANGs, "MALOAI", "TENLOAI", mATHANG.MALOAI);
            ViewBag.MANSX = new SelectList(db.NHASANXUATs, "MANSX", "TENNSX", mATHANG.MANSX);
            return View(mATHANG);
        }

        // GET: MATHANGs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MATHANG mATHANG = db.MATHANGs.Find(id);
            if (mATHANG == null)
            {
                return HttpNotFound();
            }
            return View(mATHANG);
        }

        // POST: MATHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MATHANG mATHANG = db.MATHANGs.Find(id);
            db.MATHANGs.Remove(mATHANG);
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
