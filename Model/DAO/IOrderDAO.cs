using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public interface IOrderDAO
    {
        long Insert(Order order);
        Order GetOrderByID(long id);
        List<Order> GetOrderByCustomID(long id, ref int totalRecord, int pageIndex = 1, int pageSize = 3);
        IEnumerable<Order> ListAllPaging(string searchString, int page, int pageSize);
        bool Update(Order order);
        bool Delete(int id);
        bool ChangeStatus(long id);
    }
}
