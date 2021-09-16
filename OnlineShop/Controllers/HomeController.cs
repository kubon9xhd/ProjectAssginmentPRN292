using Model.DAO;
using Model.EF;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        private IMenuDAO _menuDAO;
        private IFollowUs _followUS;
        private ISystemAbout _systemAbout;
        private ISlideDAO _slideDAO;
        private IProductDAO _productDAO;
        public HomeController(IMenuDAO menuDAO, IFollowUs followUs, ISystemAbout systemAbout, ISlideDAO slideDAO, IProductDAO productDAO)
        {
            _menuDAO = menuDAO;
            _followUS = followUs;
            _systemAbout = systemAbout;
            _slideDAO = slideDAO;
            _productDAO = productDAO;
        }
        // GET: Home
        public ActionResult Index()
        {
            var slides = _slideDAO.ListAll();
            var productNewsDAO = _productDAO.ListNewProduct(5);
            var producFeatureDAO = _productDAO.ListFeatureProduct(5);
            ViewBag.Slides = slides;
            ViewBag.NewProducts = productNewsDAO;
            ViewBag.FeatureDAO = producFeatureDAO;
            return View();
        }
        [ChildActionOnly]
        public ActionResult TopBar()
        {
            SystemAbout systemAbout = _systemAbout.GetInfo();
            return PartialView(systemAbout);
        }
        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            List<Menu> model = _menuDAO.ListByGroupId(1);
            return PartialView(model);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult BottomMenuLeft()
        {
            List<Menu> model = _menuDAO.ListByGroupId(2);
            return PartialView(model);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult BottomMenuRight()
        {
            List<Menu> model = _menuDAO.ListByGroupId(3);
            return PartialView(model);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult FollowUs()
        {
            List<FollowU> followUs = _followUS.ListAll();
           return PartialView(followUs);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult GetTouch()
        {
            SystemAbout systemAbout = _systemAbout.GetInfo();
            return PartialView(systemAbout);
        }
        [ChildActionOnly]
        public ActionResult HeaderCart()
        {
            var cart = Session[CommonConstants.CART_SESSION];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return PartialView(list);
        }
    }
}