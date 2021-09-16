using Common;
using Model.DAO;
using Model.EF;
using OnlineShop.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OnlineShop.Controllers
{
    public class CartController : BaseController
    {
        private IProductDAO _productDAO;
        private IOrderDAO _orderDAO;
        private IOrderDetailDAO _orderDetailDAO;
        private IUserDAO _userDAO;
        public CartController(IProductDAO productDAO, IOrderDAO orderDAO, IOrderDetailDAO orderDetailDAO, IUserDAO userDAO)
        {
            _productDAO = productDAO;
            _orderDAO = orderDAO;
            _orderDetailDAO = orderDetailDAO;
            _userDAO = userDAO;
        }
        
        // GET: Cart
        public ActionResult Index(int page = 1, int pageSize = 3)
        {
            var cart = Session[CommonConstants.CART_SESSION];
            var list = new List<CartItem>();
            SetPage(ref list, page, pageSize);
            return View(list);
        }
        public JsonResult DeleteAll()
        {
            Session[CommonConstants.CART_SESSION] = null;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Delete(long id, string size)
        {
            var listCart = (List<CartItem>)Session[CommonConstants.CART_SESSION];
            bool status;
            var cartCheck = listCart.Where(x => x.Product.ID == id && x.size == size);
            if (cartCheck != null)
            {
                listCart.RemoveAll(x => x.Product.ID == id && x.size == size);
                Session[CommonConstants.CART_SESSION] = listCart;
                status = true;
            }
            else
            {
                status = false;
            }
            return Json(new
            {
                status = status
            });
        }
        public ActionResult AddItem(long productId, int quantity, string size = "l")
        {
            var session = Session[CommonConstants.CART_SESSION];
            var product = _productDAO.ViewDetail(productId);
            if (product == null)
            {
                SetMessage("The product does not exist", "error");
                return RedirectToAction("/Index", "Home");
            }
            if (size.Equals("s") || size.Equals("l") || size.Equals("m") || size.Equals("xl") || size.Equals("xxl"))
            {
                if (session != null)
                {
                    var list = (List<CartItem>)session;
                    if (list.Exists(x => x.Product.ID == productId) && list.Exists(x => x.size == size))
                    {
                        foreach (var item in list)
                        {
                            if (item.Product.ID == productId)
                            {
                                if ((item.Quantity + quantity) <= 0)
                                {
                                    SetMessage("Error quantity can not is negative integer number", "error");
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    item.Quantity += quantity;
                                }
                            }
                        }
                    }
                    else
                    {
                        var item = new CartItem();
                        if (quantity <= 0)
                        {
                            SetMessage("Error quantity can not is negative integer number", "error");
                            return RedirectToAction("Index");
                        }
                        item.Product = product;
                        item.Quantity = quantity;
                        item.size = size;
                        list.Add(item);
                    }
                    Session[CommonConstants.CART_SESSION] = list;
                }
                else
                {
                    if (quantity <= 0)
                    {
                        SetMessage("Error quantity can not is negative integer number", "error");
                        return RedirectToAction("Index");
                    }
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    item.size = size;
                    var listCart = new List<CartItem>();
                    listCart.Add(item);
                    Session[CommonConstants.CART_SESSION] = listCart;
                }
            }
            else
            {
                SetMessage("The size does not exist", "error");
            }
            return RedirectToAction("Index");
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CommonConstants.CART_SESSION];
            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID && x.size == item.size);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            return Json(new
            {
                status = true
            });
        }
        public ActionResult CheckOut(int page = 1, int pageSize = 3)
        {
            var session = Session[CommonConstants.CART_SESSION];
            var user = (Session[CommonConstants.USER_SESSION] as UserLogin);
            if (user != null)
            {
                var userModel = _userDAO.GetByID((int)user.UserId);
                ViewBag.UserModel = userModel;
            }
            if (session == null)
            {
                SetMessage("No items to pay", "warning");
                return RedirectToAction("/");
            }
            var list = new List<CartItem>();
            SetPage(ref list, page, pageSize);
            return View(list);
        }
        [HttpPost]
        public JsonResult CheckOut(string OrderDetailModel)
        {
            bool status = false;
            string code = "";
            if (string.IsNullOrEmpty(OrderDetailModel))
            {
                status = false;
            }
            else
            {
                var jsonOrderDetailModel = new JavaScriptSerializer().Deserialize<List<OrderDetailModel>>(OrderDetailModel);
                var session = Session[CommonConstants.CART_SESSION];
                if (session == null)
                {
                    status = false;
                }
                else
                {
                    var list = session as List<CartItem>;
                    var order = new Order();
                    var user = (Session[CommonConstants.USER_SESSION] as UserLogin);
                    if(user != null)
                    {
                        order.CustomerID = user.UserId;
                    }
                    order.CreatedDate = DateTime.Now;
                    order.ShipName = jsonOrderDetailModel[0].ShipName;
                    order.ShipEmail = jsonOrderDetailModel[0].ShipEmail;
                    order.ShipMobile = jsonOrderDetailModel[0].ShipMobile;
                    order.ShipAddress = jsonOrderDetailModel[0].ShipAddress;
                    order.ShipCountry = jsonOrderDetailModel[0].ShipCountry;
                    order.ShipCity = jsonOrderDetailModel[0].ShipCity;
                    order.ShipDistrict = jsonOrderDetailModel[0].ShipDistrict;
                    order.ShipVillage = jsonOrderDetailModel[0].ShipVillage;
                    order.MetaTitle = RandomString(15);
                    // insert to db and get id
                    try
                    {
                        var id = _orderDAO.Insert(order);
                        decimal total = 0;
                        foreach (var item in list)
                        {
                            var orderDetail = new OrderDetail();
                            orderDetail.ProductID = item.Product.ID;
                            orderDetail.OrderId = id;
                            orderDetail.Price = item.Product.Price;
                            orderDetail.Quantity = item.Quantity;
                            orderDetail.Size = item.size;
                            _orderDetailDAO.Insert(orderDetail);
                            total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
                        }
                        status = true;
                        code = order.MetaTitle + "-" + id;
                        ViewBag.Code = code;
                        try
                        {
                            string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/web/template/neworder.html"));

                            content = content.Replace("{{CustomerName}}", order.ShipName);
                            content = content.Replace("{{Phone}}", order.ShipMobile);
                            content = content.Replace("{{Email}}", order.ShipEmail);
                            content = content.Replace("{{Address}}", order.ShipAddress);
                            content = content.Replace("{{Total}}", total.ToString("N0"));
                            content = content.Replace("{{Link}}", "https://localhost:44386/order-detail/" + code);
                            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                            new MailHelper().SendMail(order.ShipEmail, "New order from OnlineShop", content);
                            new MailHelper().SendMail(toEmail, "New order from OnlineShop", content);
                        }
                        catch (Exception)
                        {
                            status = true;
                        }
                    }
                    catch (Exception)
                    {
                        status = false;
                    }
                }
               
            }
            return Json(new
            {
                status = status,
                code = code,
            });

        }
        public ActionResult OrderDetail(int id,int page = 1, int pageSize = 3)
        {
            var order = _orderDAO.GetOrderByID(id);
            var list = new List<CartItem>();
            if(order != null)
            {
                var orderDetail = _orderDetailDAO.ListAll(order.ID);
                if(order == null)
                {
                    SetMessage("The order does not exist", "warning");
                    return RedirectToAction("/");
                }
                foreach(var item in orderDetail)
                {
                    var product = _productDAO.ViewDetail(item.ProductID);
                    var cartItem = new CartItem();
                    cartItem.Product = product;
                    cartItem.Quantity = (int)item.Quantity;
                    cartItem.size = item.Size;
                    list.Add(cartItem);
                }
                ViewBag.Order = order;
                SetPage(ref list, page, pageSize);
            }
            else
            {
                SetMessage("Order Detail does not exist", "error");
                return RedirectToAction("/");
            }
           
            return View(list);
        }
        public void SetPage(ref List<CartItem> list, int page, int pageSize)
        {
            var session = Session[CommonConstants.CART_SESSION];
            if (session != null)
            {
                int totalRecord = (session as List<CartItem>).Count();
                list = (session as List<CartItem>).Skip((page - 1) * pageSize).Take(pageSize).ToList();
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

            }
        }
    }
}