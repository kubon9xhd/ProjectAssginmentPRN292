using Assiginment.Areas.Admin.Models;
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
    public class ContentController : BaseController
    {
        private ICagetoryDAO _categoryDAO;
        private IContentDAO _contentDAO;
        public ContentController(ICagetoryDAO cagetoryDAO, IContentDAO contentDAO)
        {
            _categoryDAO = cagetoryDAO;
            _contentDAO = contentDAO;
        }
        // GET: Admin/Content
        public ActionResult Index(string search, int page = 1, int pageSize = 10)
        {
            var listContent = _contentDAO.ListAllPaging(search, page, pageSize);
            ViewBag.SearchString = search;
            return View(listContent);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Content content)
        {
            if (ModelState.IsValid)
            {
                content.CreatedBy = (Session[CommonConstants.USER_SESSION] as UserLogin).Username;
                content.CreatedDate = DateTime.Now;
                long saveCheck = _contentDAO.Insert(content);
                if(saveCheck > 0)
                {
                    SetMessage("Create Content done","success");
                    return RedirectToAction("Create");
                }
                else
                {
                    SetMessage("Error! Can not insert with data", "warning");
                }
            }
            SetViewBag();
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var content = _contentDAO.GetByID(id);
            if(content == null)
            {
                return RedirectToAction("Index");
            }
            SetViewBag(id);
            return View(content); 
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Content content)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                var contentDB = _contentDAO.GetByID(content.ID);
                if(contentDB != null)
                {
                    content.ModifyBy = (Session[CommonConstants.USER_SESSION] as UserLogin).Username;
                    content.ModifyDate = DateTime.Now;
                    bool check = _contentDAO.Update(content);
                    if (check)
                    {
                        SetMessage("Edit Content done","success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        message = "Error! Update fail";
                    }
                }
                else
                {
                    message = "Error! Content does not exist";
                }
            }
            SetMessage(message, "warning");
            SetViewBag(content.ID);
            return View(content);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _contentDAO.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var content = _contentDAO.GetByID((int)id);
            if (content != null)
            {
                bool result = _contentDAO.ChangeStatus(id);
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
        public void SetViewBag(long? idSelected = null)
        {
           
            ViewBag.CategoryID = new SelectList(_categoryDAO.ListAll(),"ID","Name",idSelected);
        }
    }
}