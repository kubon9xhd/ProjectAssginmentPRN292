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
    public class UserController : BaseController
    {
        private IUserDAO _userDAO;
        private IOrderDAO _orderDAO;
        public UserController(IUserDAO userDAO, IOrderDAO orderDAO)
        {
            _userDAO = userDAO;
            _orderDAO = orderDAO;
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }
        [HttpPost]
        public ActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                if(_userDAO.GetByUsername(registerModel.UserName) != null)
                {
                    SetMessage("Username already exist.", "error");
                }
                else if(_userDAO.IsExistEmail(registerModel.Email))
                {
                    SetMessage("Email already exist.", "error");
                }
                else
                {
                    var user = new User();
                    user.Name = registerModel.Name;
                    user.Username = registerModel.UserName;
                    user.Password = Encrypter.MD5Hash(registerModel.Password);
                    user.Phone = registerModel.Phone;
                    user.Email = registerModel.Email;
                    user.Address = registerModel.Address;
                    user.CreatedDate = DateTime.Now;
                    user.Status = true;
                    var id = _userDAO.Insert(user);
                    if(id > 0)
                    {
                        SetMessage("Register successfully.", "success");
                    }
                    else
                    {
                        SetMessage("Register fail.", "error");
                    }
                }
            }
            return View(registerModel);
        }
        [HttpPost]
        public ActionResult Login(string txtUsername, string txtPassword)
        {
           if(string.IsNullOrEmpty(txtUsername) || string.IsNullOrEmpty(txtPassword))
            {
                SetMessage("Username or Password can not empty","error");
            }
            else
            {
                var result = _userDAO.Login(txtUsername, Encrypter.MD5Hash(txtPassword));
                if (result == 1)
                {
                    var user = _userDAO.GetByUsername(txtUsername);
                    var userSession = new UserLogin();
                    userSession.Username = user.Username;
                    userSession.UserId = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    TempData["AlertMessage"] = "Login successfully";
                    TempData["AlertType"] = "success";
                    return Redirect("/");
                }
                else if (result == 0)
                {
                    TempData["AlertMessage"] = "Login fail, account or password does not exist.!";
                }
                else if (result == -1)
                {
                    TempData["AlertMessage"] = "Login fail, accoutn was locked.!";
                }
            }
            TempData["AlertType"] = "error";
            return Redirect("/account/register");
        }
        public ActionResult Setting(string username,long id,int page = 1, int pageSize = 3)
        {
            var user = (Session[CommonConstants.USER_SESSION] as UserLogin);
            if (user != null)
            {
                int totalRecord = 0;
                var model = _orderDAO.GetOrderByCustomID(user.UserId,ref totalRecord, page, pageSize);
                ViewBag.Total = totalRecord;
                ViewBag.Page = page;
                int maxPage = 5;
                int totalPage = 0;
                totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
                ViewBag.TotalPage = totalPage;
                ViewBag.MaxPage = maxPage;
                ViewBag.First = 1;
                ViewBag.Last = totalPage;
                ViewBag.Next = page + 1;
                ViewBag.Prev = page - 1;
                ViewBag.Username = user.Username;
                ViewBag.UserID = user.UserId;
                var userInfo = _userDAO.GetByID((int)user.UserId);
                ViewBag.UserSession = userInfo;
                return View(model);
            }
            else
            {
                return Redirect("/account/login");
            }
        }
        public JsonResult Update(string username, string name, string email, string phone, string addres)
        {
            var user = (Session[CommonConstants.USER_SESSION] as UserLogin);
            bool status = false;
            string message = "";
            if(user != null)
            {
                var _user = _userDAO.GetByID((int)user.UserId);
                if (_user != null)
                {
                    string currentEmail = _user.Email;
                    string currentUsername = _user.Username;
                    bool check = false;
                    if (!string.IsNullOrEmpty(currentEmail))
                    {
                        if (!currentEmail.Equals(email))
                        {
                            if (_userDAO.IsExistEmail(email))
                            {
                                message = "Error!, Email is already exist.";
                                check = true;
                                status = false;
                            }
                        }


                    }
                    bool isUpdate = false;
                    if (check == false)
                    {
                        bool checkUsername = false;
                        if(!currentUsername.Equals(username))
                        {
                            if (_userDAO.GetByUsername(username) != null)
                            {
                                message = "Error!, Email is already exist.";
                                status = false;
                                checkUsername = true;
                            }
                        }
                        if (checkUsername == false)
                        {
                            User userUpdate = new User();
                            userUpdate.ID = user.UserId;
                            userUpdate.Username = username;
                            userUpdate.Name = name;
                            userUpdate.Email = email;
                            userUpdate.Phone = phone;
                            userUpdate.Address = addres;
                            isUpdate = _userDAO.Update(userUpdate);
                            if (isUpdate)
                            {
                                message = "Update user done";
                                status = true;
                            }
                            else
                            {
                                message = "Error!, Update User fail.";
                                status = false;
                            }
                        }
                        
                    }

                }
                else
                {
                    message = "Error!, Account does not exist.";
                    status = false;
                }

            }
            return Json(new {
                status = status,
                message = message
            });
        }
        public JsonResult ChangePassword(string oldPassword, string newPassword)
        {
            var user = (Session[CommonConstants.USER_SESSION] as UserLogin);
            bool status = false;
            string message = "";
            if (user != null)
            {
                var currentUser = _userDAO.GetByUsername(user.Username);
                if (!Common.Encrypter.MD5Hash(oldPassword).Equals(currentUser.Password))
                {
                    status = false;
                    message = "Error, oldpassword not right.";
                }
                else
                {
                    User userUpdate = new User();
                    userUpdate.ID = user.UserId;
                    userUpdate.Username = currentUser.Username;
                    userUpdate.Password = Common.Encrypter.MD5Hash(newPassword);
                    userUpdate.Name = currentUser.Name;
                    userUpdate.Email = currentUser.Email;
                    userUpdate.Phone = currentUser.Phone;
                    userUpdate.Address = currentUser.Address;
                    _userDAO.Update(userUpdate);
                    status = true;
                    message = "Update password done";
                }
               
            }
            else
            {
                status = false;
                message = "Can not update now";
            }
            return Json(new
            {
                status = status,
                message = message
            });
        }

    }
}