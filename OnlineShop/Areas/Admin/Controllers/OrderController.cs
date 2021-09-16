using Model.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private IOrderDAO _orderDAO;
        public OrderController(IOrderDAO orderDAO)
        {
            _orderDAO = orderDAO;
        }
        // GET: Order
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var model = _orderDAO.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _orderDAO.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var content = _orderDAO.GetOrderByID((int)id);
            if (content != null)
            {
                bool result = _orderDAO.ChangeStatus(id);
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