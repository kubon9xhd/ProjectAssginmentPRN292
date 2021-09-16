using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class OrderDetaildAO : AbstractDAO, IOrderDetailDAO
    {
        public OrderDetail GetByID(int id)
        {
            return db.OrderDetails.Find(id);
        }
        public bool Insert(OrderDetail orderDetail)
        {
            try
            {
                db.OrderDetails.Add(orderDetail);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<OrderDetail> ListAll(long id)
        {
            return db.OrderDetails.Where(x => x.OrderId == id).ToList();
        }
       
    }

}
