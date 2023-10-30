using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using _63CNTT_4.Library;

namespace _63CNTT_4.Areas.Admin.Controllers
{
    public class SuppliderController : Controller
    {
        SuppliersDAO suppliersDAO = new SuppliersDAO();

        // GET: Admin/Supplider
        public ActionResult Index()
        {
            return View("Index");
        }

        // GET: Admin/Supplider/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Xóa thành công mẫu tin");
                return RedirectToAction("Index");
            }
            Suppliers suppliders = suppliersDAO.getRow(id);
            if (suppliders == null)
            {
                TempData["message"] = new XMessage("danger", "Xóa thành công mẫu tin");
                return RedirectToAction("Index");
            }

            return View(suppliders);
        }

        // GET: Admin/Supplider/Create
        public ActionResult Create()
        {
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Suppliers suppliders)
        {
            if (ModelState.IsValid)
            {
               
                //xu ly tu dong cho cac truong sau:
                //-----CreateAt
                suppliders.CreateAt = DateTime.Now;
                //-----CreateBy
                suppliders.CreateBy = Convert.ToInt32(Session["UserID"]);
                //slug
                suppliders.Slug = XString.Str_Slug(suppliders.Name);
                
                                //Order
                if (suppliders.Order == null)
                {
                    suppliders.Order = 1;
                }
                else
                {
                    suppliders.Order += 1;
                }
                //UpdateAt
                suppliders.UpdateAt = DateTime.Now;
                //UpdateBy
                suppliders.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //lưu trữ vào dattabase
                suppliersDAO.Insert(suppliders);
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliders);
        }

        // GET: Admin/Supplider/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Xóa thành công mẫu tin");
                return RedirectToAction("Index");
            }
            Suppliers suppliders = suppliersDAO.getRow(id);
            if (suppliders == null)
            {
                TempData["message"] = new XMessage("danger", "Xóa thành công mẫu tin");
                return RedirectToAction("Index");
            }
            return View(suppliders);
        }

        // POST: Admin/Supplider/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Suppliers suppliders)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong sau:
                //-----CreateBy
                suppliders.CreateBy = Convert.ToInt32(Session["UserID"]);
                //slug
                suppliders.Slug = XString.Str_Slug(suppliders.Name);

                //Order
                if (suppliders.Order == null)
                {
                    suppliders.Order = 1;
                }
                else
                {
                    suppliders.Order += 1;
                }
                //UpdateAt
                suppliders.UpdateAt = DateTime.Now;
                //UpdateBy
                suppliders.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //update DB 
                suppliersDAO.Insert(suppliders);
                TempData["message"] = new XMessage("danger", "Thêm mới nhà cung cấp thành công");
                return RedirectToAction("Index");
            }
            ViewBag.OrderList = new SelectList(suppliersDAO.getList("Index"), "Order", "Name");
            return View(suppliders);
        }

        // GET: Admin/Supplider/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            Suppliers suppliders = suppliersDAO.getRow(id);
            if (suppliders == null)
            {
                return HttpNotFound();
            }
            return View(suppliders);
        }

        // POST: Admin/Supplider/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Suppliers suppliders = suppliersDAO.getRow(id);
            // xóa khỏi DB
            suppliersDAO.Delete(suppliders);
            TempData["message"] = new XMessage("danger", "Xóa nhà cung cấp thành công");
            return RedirectToAction("Index");
        }
        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/DelTrash/5
        // chuyển mẫu tin đang ở status 1 or 2 => 0: ko hiển thị trên  index
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            suppliers.Status = 0;
            //cap nhạt Update At
            suppliers.UpdateAt = DateTime.Now;
            //cap nhat Update By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            suppliersDAO.Update(suppliers);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Index");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Trash = luc thung rac
        public ActionResult Trash()
        {
            return View(suppliersDAO.getList("Trash")); //CategoryDAO status=0
        }
        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Undo/5
        // chuyển mẫu tin đang ở status 1 or 2 => 0: ko hiển thị trên  index
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            Suppliers suppliers = suppliersDAO.getRow(id);
            if (suppliers == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }

            //cap nhạt Update At
            suppliers.UpdateAt = DateTime.Now;
            //cap nhat Update By
            suppliers.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //cap nhat trang thai
            suppliers.Status = 2;
            //Update DB
            suppliersDAO.Update(suppliers);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");
            //hien thi trong Trash or Index 
            return RedirectToAction("Trash");
        }
        ///////////////////////////////////////////////////////
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
