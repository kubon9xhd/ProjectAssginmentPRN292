using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public interface IOrderDetailDAO
    {
        OrderDetail GetByID(int id);
        bool Insert(OrderDetail orderDetail);
        List<OrderDetail> ListAll(long id);
    }
}
