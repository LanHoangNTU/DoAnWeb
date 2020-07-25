using DoAnWeb.Models;
using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace DoAnWeb.Controllers
{
    public class QuanTriController : Controller
    {
        // GET: QuanTri
        private MTPKEntities1 db = new MTPKEntities1();
        public ActionResult Home()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LoginForm", "QuanTri");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Index(int? page,  string name, string MALOAI, string id3)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LoginForm", "QuanTri");
            }
            var list = db.MATHANGs.AsQueryable();
            if (!name.IsNullOrWhiteSpace())
            {
                list = list.Where(m => m.TENMH.ToLower().Contains(name.ToLower()));
            }

            if (!MALOAI.IsNullOrWhiteSpace())
            {
                list = list.Where(m => m.MALOAI.Equals(MALOAI));
            }

            if (!id3.IsNullOrWhiteSpace())
            {
                list = list.Where(m => m.MANSX.Equals(id3));
            }
            ViewBag.NAME = name;
            ViewBag.MALOAI = new SelectList(db.LOAIMATHANGs, "MALOAI", "TENLOAI");
            ViewBag.LOAI = MALOAI;
            return View(list.OrderBy(m => m.MAMH).ToPagedList(page ?? 1, 6));
        }

        public ActionResult LoginForm()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["admin"] = null;
            return RedirectToAction("LoginForm");
        }

        public ActionResult Verify(FormCollection collection)
        {
            string id = collection["login"];
            string pass = collection["password"];
            var list = db.QUANTRIVIENs;
            foreach (var item in list)
            {
                if(id.Equals(item.TAIKHOAN) && pass.Equals(item.MATKHAU))
                {
                    Session["admin"] = db.QUANTRIVIENs.Find(id);
                    return RedirectToAction("Home", "QuanTri", new { page = 1});
                }
            }

            return RedirectToAction("LoginForm", "QuanTri");
        }

        public ActionResult ProductDetails(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            MATHANG mh = db.MATHANGs.Find(id);

            return View(mh);
        }

        public ActionResult DeleteProduct(string id)
        {
            MATHANG mh = db.MATHANGs.Find(id);
            db.MATHANGs.Remove(mh);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult TechInfo(string id)
        {
            var list = db.THONGSOKYTHUATs.Where(m => m.MAMH.Equals(id));
            ViewBag.MAMH = id;
            return View(list.ToList());
        }

        public ActionResult DeleteTech(string id)
        {
            var item = db.THONGSOKYTHUATs.Find(id);
            string MAMH = item.MAMH;
            db.THONGSOKYTHUATs.Remove(item);
            db.SaveChanges();
            return RedirectToAction("TechInfo", "QuanTri", new { id = MAMH});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTech(string id)
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

        private int GetNewPicId()
        {
            if (db.ANHMINHHOAs.Count() <= 0)
            {
                return 0;
            }

            return db.ANHMINHHOAs.Select(m => m.MAANH).Max() + 1;
        }

        public ActionResult ShowPictures(string id)
        {
            var list = db.ANHMINHHOAs.Where(m => m.MAMH.Equals(id));
            ViewBag.MAMH = id;
            return View(list.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPictures(string id)
        {
            ViewBag.MAMH = db.MATHANGs.Find(id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPictures([Bind(Include = "MAANH,MAMH,TENANH")] ANHMINHHOA aNHMINHHOA)
        {
            var imgNV = Request.Files["Avatar"];
            string postedFileName = System.IO.Path.GetFileName(imgNV.FileName);
            var path = Server.MapPath("/Images/Carousel/" + aNHMINHHOA.MAMH);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Server.MapPath("/Images/Carousel/" + aNHMINHHOA.MAMH + "/" + postedFileName);
            imgNV.SaveAs(path);
            if (ModelState.IsValid)
            {
                aNHMINHHOA.MAANH = GetNewPicId();
                aNHMINHHOA.TENANH = imgNV.FileName;
                db.ANHMINHHOAs.Add(aNHMINHHOA);
                db.SaveChanges();
                return RedirectToAction("ShowPictures", "QuanTri", new { id = aNHMINHHOA.MAMH});
            }
            
            return View(aNHMINHHOA);
        }

        [HttpGet]
        public ActionResult Bills(int? page, string name = "")
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LoginForm", "QuanTri");
            }
            ViewBag.Search = name;
            var list = db.GIOHANGs.Where(m => !m.TRANGTHAI);
            if (!name.IsNullOrWhiteSpace())
            {
                list = list.Where(m => m.MAGH.ToLower().Contains(name.ToLower()));
            }

            return View(list.ToList());
        }

        [ValidateAntiForgeryToken]
        public ActionResult XacNhan(string id)
        {

            GIOHANG gh = db.GIOHANGs.Find(id);
            gh.TRANGTHAI = true;
            gh.NGAYXUAT = DateTime.Now.Date;

            var list = db.CTGIOHANGs.Where(x => x.MAGH == id);

            db.Entry(gh).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Bills", "QuanTri");
        }

        public ActionResult DeletePicture(int id)
        {
            var anh = db.ANHMINHHOAs.Find(id);
            var mh = anh.MAMH;
            ViewBag.MAMH = mh;
            db.ANHMINHHOAs.Remove(anh);
            db.SaveChanges();
            return RedirectToAction("ShowPictures", new { id = mh});
        }

        public ActionResult Summary(DateTime? x = null, DateTime? y = null)
        {
            var list = db.GIOHANGs.AsQueryable();
            IEnumerable<IGrouping<DateTime, GIOHANG>> gropus = null;
            if (x.HasValue && y.HasValue)
            {
                gropus = list.ToList().Where(m => m.NGAYXUAT >= x && m.NGAYXUAT <= y
                && m.TRANGTHAI).GroupBy(m => m.NGAYXUAT);
            }
            else
            {
                gropus = list.ToList().Where(m => m.NGAYXUAT >= DateTime.Now.Date.AddDays(-30) 
                && m.NGAYXUAT <= DateTime.Now.Date && m.TRANGTHAI)
                    .GroupBy(m => m.NGAYXUAT);
            }
            var result = new Hashtable();
            foreach (var item in gropus)
            {
                long val = 0;
                foreach (var i in item)
                {
                    val += i.TONGTIEN;
                }
                result.Add(item.Key, val);
            }
            ViewBag.SUM = result;
            return View();
        }
    }
}