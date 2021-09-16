using Model.DAO;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShop.Controllers
{
    public class ContactController : Controller
    {
        private IFollowUs _followUS;
        private ISystemAbout _systemAbout;
        private IContactDAO _contactDAO;
        public ContactController(IFollowUs followUs, ISystemAbout systemAbout, IContactDAO contactDAO)
        {
            _followUS = followUs;
            _systemAbout = systemAbout;
            _contactDAO = contactDAO;
        }
        // GET: Contact
        public ActionResult Index()
        {
            SystemAbout systemAbout = _systemAbout.GetInfo();
            ViewBag.FollowUS = _followUS.ListAll();
            return View(systemAbout);
        }
        public JsonResult Send(string name, string email, string phone, string address, string content)
        {
            FeedBack feedBack = new FeedBack();
            feedBack.Email = email;
            feedBack.Name = name;
            feedBack.Phone = phone;
            feedBack.Address = address;
            feedBack.Content = content;
            long id = _contactDAO.InsertFeedBack(feedBack);
            bool status = false;
            if(id > 0)
            {
                status = true;
            }
            return Json(new
            {
                status = status
            });
        }
    }
}