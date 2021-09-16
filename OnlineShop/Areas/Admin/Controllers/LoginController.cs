using Assiginment.Areas.Admin.Models;
using Model.DAO;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private IUserDAO _userDAO;
        public LoginController(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }
        // GET: Admin/Login
        public ActionResult Index()
        {
            ViewBag.isLogin = true;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _userDAO.Login(model.Username, Encrypter.MD5Hash(model.Password), true);
                if (result == 1)
                {
                    var user = _userDAO.GetByUsername(model.Username);
                    var userSession = new UserLogin();
                    userSession.Username = user.Username;
                    userSession.UserId = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    TempData["AlertMessage"] = "Login successfully";
                    TempData["AlertType"] = "success";
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    TempData["AlertMessage"] = "Login fail, account or password does not exist.!";
                }
                else if(result == -1)
                {
                    TempData["AlertMessage"] = "Login fail, account was locked.!";
                }
                else if (result == -2)
                {
                    TempData["AlertMessage"] = "Login fail, does not permisstion.!";
                }
            }
            TempData["AlertType"] = "error";
            return View("Index");
        }
    }
}
