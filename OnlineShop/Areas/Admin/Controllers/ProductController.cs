using Model.DAO;
using Model.EF;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private IProductDAO _productDAO;
        private IProductCategory _productCategory;
        public ProductController(IProductDAO productDAO, IProductCategory productCategory)
        {
            _productDAO = productDAO;
            _productCategory = productCategory;
        }
        // GET: Admin/Product
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var model = _productDAO.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedBy = (Session[CommonConstants.USER_SESSION] as UserLogin).Username;
                product.CreatedDate = DateTime.Now;
                long saveCheck = _productDAO.Insert(product);
                if (saveCheck > 0)
                {
                    SetMessage("Create Product done", "success");
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
            var content = _productDAO.ViewDetail(id);
            if (content == null)
            {
                return RedirectToAction("Index");
            }
            SetViewBag(id);
            return View(content);
        }
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                var contentDB = _productDAO.ViewDetail(product.ID);
                if (contentDB != null)
                {
                    product.ModifyBy = (Session[CommonConstants.USER_SESSION] as UserLogin).Username;
                    product.ModifyDate = DateTime.Now;
                    bool check = _productDAO.Update(product);
                    if (check)
                    {
                        SetMessage("Edit Product done", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        message = "Error! Update fail";
                    }
                }
                else
                {
                    message = "Error! Product does not exist";
                }
            }
            SetMessage(message, "warning");
            SetViewBag(product.CategoryId);
            return View(product);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _productDAO.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var content = _productDAO.ViewDetail((int)id);
            if (content != null)
            {
                bool result = _productDAO.ChangeStatus(id);
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
        public JsonResult LoadImages(long id)
        {
            var product = _productDAO.ViewDetail(id);
            var images = product.MoreImages;
            XElement xImages = XElement.Parse(images);
            List<string> listImagesReturn = new List<string>();

            foreach (XElement element in xImages.Elements())
            {
                listImagesReturn.Add(element.Value);
            }
            return Json(new
            {
                data = listImagesReturn
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveImages(long id, string images)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var listImages = serializer.Deserialize<List<string>>(images);

            XElement xElement = new XElement("Images");

            foreach (var item in listImages)
            {
                var subStringItem = item.Substring(21);
                xElement.Add(new XElement("Image", subStringItem));
            }
            try
            {
                _productDAO.UpdateImages(id, xElement.ToString());
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false
                });
            }

        }
        public void SetViewBag(long? selectedId = null)
        {
            ViewBag.CategoryID = new SelectList(_productCategory.ListAll(), "ID", "Name", selectedId);
        }
    }
}
