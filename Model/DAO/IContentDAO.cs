using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public interface IContentDAO
    {
        IEnumerable<Content> ListAllPaging(string search, int page, int pageSize);
        Content GetByID(long id);
        long Insert(Content content);
        bool Update(Content content);
        bool Delete(int id);
        bool ChangeStatus(long id);
    }
}
