using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
namespace Model.DAO
{
    public interface ICagetoryDAO
    {
        IEnumerable<Caetgory> ListAllPaging(string search, int page, int pageSize);
        List<Caetgory> ListAll();
        Caetgory GetCaetgoryById(long id);
        long Insert(Caetgory caetgory);
        bool Update(Caetgory caetgory);
        bool Delete(int id);
        bool ChangeStatus(long id);
    }
}
