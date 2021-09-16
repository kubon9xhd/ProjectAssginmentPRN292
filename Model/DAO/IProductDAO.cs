using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public interface IProductDAO
    {
        List<Product> ListNewProduct(int top);
        List<Product> ListFeatureProduct(int top);
        Product ViewDetail(long id);
        List<Product> ListRelatedProduct(long productID);
        List<Product> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 3,string search = "");
        List<String> ListName(string keyWord);
        List<Product> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 3);
        IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize);
        void UpdateImages(long productId, string images);
        long Insert(Product product);
        bool Update(Product product);
        bool Delete(int id);
        bool ChangeStatus(long id);
    }
}
