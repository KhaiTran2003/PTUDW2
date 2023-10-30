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
    public class CategoryController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Index
        public ActionResult Index()
        {
            return View(categoriesDAO.getList("Index"));
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong sau:
                //-----CreateAt
                categories.CreateAt = DateTime.Now;
                //-----CreateBy
                categories.CreateBy = Convert.ToInt32(Session["UserID"]);
                //slug
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentID
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //UpdateAt
                categories.UpdateAt = DateTime.Now;
                //UpdateBy
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);

                categoriesDAO.Insert(categories);
                //hien thi thong bao thanh cong
                TempData["message"] = new XMessage( "success","Thêm mới mẫu tin thành công");

                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");

            return View(categories);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật mẫu tin thất bại");
                return RedirectToAction("Index");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật mẫu tin thất bại");
                return RedirectToAction("Index");
            }
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //cập nhật 1 số trường thông tin tự động
                categories.Slug = XString.Str_Slug(categories.Name);
                //ParentID
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }
                //Order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }
                //UpdateAt
                categories.UpdateAt = DateTime.Now;
                //UpdateBy
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //Status
                //categories.Status = (categories.Status == 1) ? 2 : 1;
                //Update DB
                categoriesDAO.Update(categories);
                //thông báo thành công
                TempData["message"] = new XMessage("success", "Cập nhật mẫu tin thành công");
                return RedirectToAction("Index");
            }
            //ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            //ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("success", "Cập nhật mẫu tin thành công");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("success", "Cập nhật mẫu tin thành công");
                return RedirectToAction("Index");
            }
            return View(categories);
        }
        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            categoriesDAO.Delete(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("danger", "Xóa thành công mẫu tin");
            return RedirectToAction("Trash");
        }
        
        
        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Status/5
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //Thong báo thay đổi Status thất bại
                TempData["message"] = new XMessage("danger", "Thay đổi trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Thay đổi Status thất bại");
                return RedirectToAction("Index");
            }
            //capapj nhật 1 số thông tin cho DB
            //Update At
            categories.UpdateAt = DateTime.Now;
            // Update By
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Status
            categories.Status = (categories.Status == 1) ? 2 : 1;
            //Update DB
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Thay đổi status thành công");
            //tro ve trang Index
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
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            categories.Status = 0;
            //cap nhạt Update At
            categories.UpdateAt = DateTime.Now;
            //cap nhat Update By
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //Update DB
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Index");
        }

        ////////////////////////////////////////////////////////////////////
        // GET: Admin/Category/Trash = luc thung rac
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash")); //CategoryDAO status=0
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
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Không tìm thấy mẫu tin");
                return RedirectToAction("Index");
            }

            //cap nhạt Update At
            categories.UpdateAt = DateTime.Now;
            //cap nhat Update By
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //cap nhat trang thai
            categories.Status = 2;
            //Update DB
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");
            //hien thi trong Trash or Index 
            return RedirectToAction("Trash");
        }
        ///////////////////////////////////////////////////////

        
    }
}