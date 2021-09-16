using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Model.DAO;
using Unity.Mvc4;

namespace OnlineShop
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    
            container.RegisterType<IUserDAO, UserDAO>();
            container.RegisterType<ICagetoryDAO, CategoryDAO>();
            container.RegisterType<IContentDAO, ContentDAO>();
            container.RegisterType<ICagetoryDAO, CategoryDAO>();
            container.RegisterType<IMenuDAO, MenuDAO>();
            container.RegisterType<ISystemAbout, SystemAboutDAO>();
            container.RegisterType<IFollowUs, FollowUsDAO>();
            container.RegisterType<IProductCategory, ProductCategoryDAO>();
            container.RegisterType<IProductDAO, ProductDAO>();
            container.RegisterType<ISlideDAO, SlideDAO>();
            container.RegisterType<IOrderDAO, OrderDAO>();
            container.RegisterType<IOrderDetailDAO, OrderDetaildAO>();
            container.RegisterType<IContactDAO, ContactDAO>();

            RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
    
    }
  }
}