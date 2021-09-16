using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.DAO;
namespace OnlineShop.Controllers
{
    public class ProductController : BaseController
    {
        private IProductCategory _productCategory;
        private IProductDAO _productDAO;
        public ProductController(IProductCategory productCategory, IProductDAO productDAO)
        {
            _productCategory = productCategory;
            _productDAO = productDAO;
        }
        // GET: Product
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
        [ChildActionOnly]
        public ActionResult ProductCategory()
        {
            var model = _productCategory.ListAll();
            return PartialView(model);
        }
        public ActionResult Category(long id, int page = 1, int pageSize = 3, string search = "")
        {
            var category = _productCategory.ViewDetail(id);
           if(category != null)
            {
                var productNewsDAO = _productDAO.ListNewProduct(5);
                ViewBag.Category = category;
                int totalRecord = 0;
                var model = _productDAO.ListByCategoryId(id, ref totalRecord, page, pageSize, search);
                ViewBag.Total = totalRecord;
                ViewBag.Page = page;
                var listSubCategory = _productCategory.ListSubCategory(id);
                if (listSubCategory != null)
                {
                    ViewBag.ListSubCategory = listSubCategory;
                }
                int maxPage = 5;
                int totalPage = 0;

                totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
                ViewBag.TotalPage = totalPage;
                ViewBag.MaxPage = maxPage;
                ViewBag.First = 1;
                ViewBag.Last = totalPage;
                ViewBag.Next = page + 1;
                ViewBag.Prev = page - 1;
                ViewBag.NewProducts = productNewsDAO;
                ViewBag.CategoryList = _productCategory.ListAll();
                return View(model);
            }
            else
            {
                SetMessage("Category does not exist","error");
                return RedirectToAction("Index","Home");
            }
        }
        public ActionResult Search(string txtSearch, int page = 1, int pageSize = 3, string search = "")
        {
            if (!string.IsNullOrEmpty(txtSearch))
            {
                int totalRecord = 0;
                var model = _productDAO.Search(txtSearch, ref totalRecord, page, pageSize);
                ViewBag.Total = totalRecord;
                ViewBag.Page = page;
                ViewBag.KeyWord = txtSearch;
                int maxPage = 5;
                int totalPage = 0;

                totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
                ViewBag.TotalPage = totalPage;
                ViewBag.MaxPage = maxPage;
                ViewBag.First = 1;
                ViewBag.Last = totalPage;
                ViewBag.Next = page + 1;
                ViewBag.Prev = page - 1;
                return View(model);
            }
            else
            {
                SetMessage("TxtSearch does not exist", "error");
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Detail(long id)
        {
            var prouduct = _productDAO.ViewDetail(id);
            if (prouduct != null)
            {
                var productNewsDAO = _productDAO.ListNewProduct(5);
                ViewBag.Category = _productCategory.ViewDetail(prouduct.CategoryId);
                ViewBag.RelatedProduct = _productDAO.ListRelatedProduct(prouduct.ID);
                ViewBag.NewProducts = productNewsDAO;
                return View(prouduct);
            }
            else
            {
                SetMessage("The Product does not exist", "error");
                return RedirectToAction("Index","Home");
            }
        }
        public JsonResult ListName(string q)
        {
            var data = _productDAO.ListName(q);
            return Json(new
            {
                data = data,
                status =true,
            },JsonRequestBehavior.AllowGet);
        }
    }
}