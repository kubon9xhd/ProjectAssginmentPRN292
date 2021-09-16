using Model.DAO;
using Model.EF;
using OnlineShop.Common;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{

    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            ViewBag.Hello = "Hello Admin";
            return View();
        }
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDAO();
                long id = dao.Insert(user);
                if (id > 0)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Add user fail");
                }
            }
            return View("Index");
        }
        [HttpGet]
        [ChildActionOnly]
        public ActionResult HeaderTop()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.User = session;
            return PartialView(session);
        }
        [HttpGet]
        [ChildActionOnly]
        public ActionResult LeftMenu()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.User = session;
            return PartialView(session);
        }
    }
}