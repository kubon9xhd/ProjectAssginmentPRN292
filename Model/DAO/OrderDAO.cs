using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class OrderDAO : AbstractDAO, IOrderDAO
    {
        public Order GetOrderByID(long id)
        {
            return db.Orders.Find(id);
        }
        public List<Order> GetOrderByCustomID(long id, ref int totalRecord, int pageIndex = 1, int pageSize = 3)
        {
            totalRecord = db.Orders.Where(x => x.CustomerID == id).Count();
            IQueryable<Order> model = db.Orders.Where(x => x.CustomerID == id);
            var result = model.OrderBy(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return result;
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;
        }

        public IEnumerable<Order> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Order> model = db.Orders;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.MetaTitle.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public bool Update(Order order)
        {

            try
            {
                var original = db.Orders.Find(order.ID);
                if (original != null)
                {
                    db.Entry(original).CurrentValues.SetValues(order);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var order = db.Orders.Find(id);
                if (order != null)
                {
                    db.Orders.Remove(order);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeStatus(long id)
        {
            try
            {
                var order = db.Orders.Find(id);
                if(order.Status == 0)
                {
                    order.Status = 1;
                }
                else
                {
                    order.Status = 0;
                }
                db.SaveChanges();
                return (order.Status == 0) ? false : true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
