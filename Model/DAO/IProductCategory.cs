using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public interface IProductCategory
    {
        long Insert(ProductCategory productCategory);
        ProductCategory GetByID(long id);
        bool Update(ProductCategory productCategory);
        bool ChangeStatus(long id);
        bool Delete(int id);
        List<ProductCategory> ListAll();
        ProductCategory ViewDetail(long id);
        List<ProductCategory> ListSubCategory(long id);
        IEnumerable<ProductCategory> ListAllPaging(string search, int page, int pageSize);
    }
}
