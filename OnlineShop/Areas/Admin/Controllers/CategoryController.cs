using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using Model.DAO;
using Assiginment.Areas.Admin.Models;
using OnlineShop.Common;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private ICagetoryDAO _cagetoryDAO;
        public CategoryController(ICagetoryDAO cagetoryDAO)
        {
            _cagetoryDAO = cagetoryDAO;
        }
        // GET: Admin/Category
        public ActionResult Index(string search, int page = 1, int pageSize = 10)
        {
            var listCategory = _cagetoryDAO.ListAllPaging(search, page, pageSize);
            ViewBag.SearchString = search;
            return View(listCategory);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Caetgory category)
        {
            if (ModelState.IsValid)
            {
                category.CreatedBy = (Session[CommonConstants.USER_SESSION] as UserLogin).Username;
                category.CreatedDate = DateTime.Now;
                long id = _cagetoryDAO.Insert(category);
                if(id > 0)
                {
                    SetMessage("Create new category done","success");
                    return RedirectToAction("Index");
                }
            }
            SetMessage("Create new category fail", "warning");
            return View(category);
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var category = _cagetoryDAO.GetCaetgoryById(id);
            if(category != null)
            {
                return View(category);
            }
            else
            {
                SetMessage("Id does not exist","error");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Caetgory caetgory) {
            var category = _cagetoryDAO.GetCaetgoryById(caetgory.ID);
            if (category != null) 
            {
                category.ModifyBy = (Session[CommonConstants.USER_SESSION] as UserLogin).Username;
                category.ModifyDate = DateTime.Now;
                bool isUpdate = _cagetoryDAO.Update(caetgory);
                if (isUpdate)
                {
                    SetMessage("Update category done", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    SetMessage("Update category fail", "warning");

                }
            }
            else
            {
                SetMessage("Category does not exist", "error");
            }
            return View(caetgory);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _cagetoryDAO.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var category = _cagetoryDAO.GetCaetgoryById((int)id);
            if (category != null)
            {
                bool result = _cagetoryDAO.ChangeStatus(id);
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