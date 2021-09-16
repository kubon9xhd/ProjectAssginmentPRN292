using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineShop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
              name: "Product Category",
              url: "product/{metatitle}-{id}",
              defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                  new[] { "OnlineShop.Controllers" }
          );
            routes.MapRoute(
               name: "Product Detail",
               url: "product-details/{metatitle}-{id}",
               defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
                   new[] { "OnlineShop.Controllers" }
           );
            routes.MapRoute(
                name: "About",
                url: "about",
                defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "Contact",
                url: "contact",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
               name: "Add Cart",
               url: "add-to-cart",
               defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
               new[] { "OnlineShop.Controllers" }
           );
            routes.MapRoute(
                name: "CheckOut",
                url: "check-out",
                defaults: new { controller = "Cart", action = "CheckOut", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "OrderDetail",
                url: "order-detail/{metatitle}-{id}",
                defaults: new { controller = "Cart", action = "OrderDetail", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "Register",
                url: "account/register",
                defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "Login Get",
                url: "account/login",
                defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "Logout",
                url: "account/logut",
                defaults: new { controller = "User", action = "Logout", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "Setting",
                url: "account/{username}-{id}",
                defaults: new { controller = "User", action = "Setting", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "Product Search",
                url: "product/search",
                defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
                new[] { "OnlineShop.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                    new[] { "OnlineShop.Controllers" }
            );
        }
    }
}
