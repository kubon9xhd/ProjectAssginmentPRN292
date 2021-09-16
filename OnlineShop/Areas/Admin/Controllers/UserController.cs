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
    public class UserController : BaseController
    {
        private IUserDAO _userDAO;
        public UserController(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }
        // GET: Admin/User
        public ActionResult Index(string search,int page = 1, int pageSize = 10)
        {
            var listUser = _userDAO.ListAllPaging(search,page,pageSize);
            ViewBag.SearchString = search;
            return View(listUser);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            string message = "";
            if (ModelState.IsValid) { 
                if(_userDAO.GetByUsername(user.Username) == null)
                {
                    if (!_userDAO.IsExistEmail(user.Email))
                    {
                        var encryptMd5Pass = Encrypter.MD5Hash(user.Password);
                        user.Password = encryptMd5Pass;
                        long id = _userDAO.Insert(user);
                        if (id > 0)
                        {
                            SetMessage("Create new user done","success");
                            return RedirectToAction("Index", "User");
                        }
                        else
                        {
                            message = "Error!, Add User fail.";
                        }
                    }
                    else
                    {
                        message = "Error!, Email already exists.";
                    }
                }
                else
                {
                    message = "Error!, Username already exists.";
                }

            }
            SetMessage(message, "warning");
            return View("Create");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var user = _userDAO.GetByID(id);
            if(user == null)
            {
                SetMessage("Error!, Account does not exists.", "error");
                return RedirectToAction("Index", "User");
            }
            return View(user);
        }
        [HttpPost]
        public ActionResult Edit(User user)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                var _user = _userDAO.GetByID((int)user.ID);
                if (_user != null)
                {
                    string currentEmail = _user.Email;
                    if (!string.IsNullOrEmpty(user.Password))
                    {
                        var encryptMd5Pass = Encrypter.MD5Hash(user.Password);
                        user.Password = encryptMd5Pass;
                    }
                    bool check = false;
                    if (!string.IsNullOrEmpty(currentEmail))
                    {
                        if (!currentEmail.Equals(user.Email))
                        {
                            if (_userDAO.IsExistEmail(user.Email))
                            {
                                message = "Error!, Email is already exist.";
                                check = true;
                            }
                        }
                       
                        
                    }
                    bool isUpdate = false;
                    if(check == false)
                    {
                        isUpdate = _userDAO.Update(user);
                        if (isUpdate)
                        {
                            SetMessage("Update user done", "success");
                            return RedirectToAction("Index", "User");
                        }
                        else
                        {
                            message = "Error!, Update User fail.";
                        }
                    }
                                   
                }
                else
                {
                    message =  "Error!, Account does not exist.";
                }
            }
            SetMessage(message, "warning");
            return View(user);
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _userDAO.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var user = _userDAO.GetByID((int)id);
            if (user != null)
            {
                bool result = _userDAO.ChangeStatus(id);
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