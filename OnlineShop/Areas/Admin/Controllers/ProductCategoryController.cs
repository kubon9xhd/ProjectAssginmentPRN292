using Model.DAO;
using Model.EF;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private IProductCategory _productCategoryDAO;
        public ProductCategoryController(IProductCategory productCategoryDAO)
        {
            _productCategoryDAO = productCategoryDAO;
        }
        // GET: Admin/ProductCategory
        public ActionResult Index(string search, int page = 1, int pageSize = 10)
        {
            var listCategory = _productCategoryDAO.ListAllPaging(search,page,pageSize);
            ViewBag.SearchString = search;
            return View(listCategory);
        }
        // GET: Admin/ProductCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProductCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                productCategory.CreatedBy = (Session[CommonConstants.USER_SESSION] as UserLogin).Username;
                productCategory.CreatedDate = DateTime.Now;
                long id = _productCategoryDAO.Insert(productCategory);
                if (id > 0)
                {
                    SetMessage("Create new category done", "success");
                    return RedirectToAction("Index");
                }
            }
            SetMessage("Create new category fail", "warning");
            return View(productCategory);
        }

        // GET: Admin/ProductCategory/Edit/5
        public ActionResult Edit(int id)
        {
            var productCategory = _productCategoryDAO.GetByID(id);
            if (productCategory != null)
            {
                return View(productCategory);
            }
            else
            {
                SetMessage("Id does not exist", "error");
            }
            return RedirectToAction("Index");
        }

        // POST: Admin/ProductCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory productCategory)
        {
            var category = _productCategoryDAO.GetByID(productCategory.ID);
            if (category != null)
            {
                category.ModifyBy = (Session[CommonConstants.USER_SESSION] as UserLogin).Username;
                category.ModifyDate = DateTime.Now;
                bool isUpdate = _productCategoryDAO.Update(productCategory);
                if (isUpdate)
                {
                    SetMessage("Update product category done", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    SetMessage("Update product category fail", "warning");

                }
            }
            else
            {
                SetMessage("Category does not exist", "error");
            }
            return View(productCategory);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _productCategoryDAO.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var category = _productCategoryDAO.GetByID((int)id);
            if (category != null)
            {
                bool result = _productCategoryDAO.ChangeStatus(id);
                return Json(new
                {
                    status = result
                });
            }
            return Json(new
            {
                status = false
            });
        }

    }
}
