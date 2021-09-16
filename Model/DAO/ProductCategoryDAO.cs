using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ProductCategoryDAO : IProductCategory
    {
        OnlineShopDBContext db = null;
        public ProductCategoryDAO()
        {
            db = new OnlineShopDBContext();
        }

        public long Insert(ProductCategory productCategory)
        {
            db.ProductCategories.Add(productCategory);
            db.SaveChanges();
            return productCategory.ID;
        }
        public ProductCategory GetByID(long id)
        {
            return db.ProductCategories.Find(id);
        }

        public List<ProductCategory> ListAll()
        {
            List<ProductCategory> productCategories = db.ProductCategories.Where(x=> x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
            return (productCategories != null) ? productCategories : null;
        }

        public IEnumerable<ProductCategory> ListAllPaging(string search, int page, int pageSize)
        {
            try
            {
                IQueryable<ProductCategory> categorys = db.ProductCategories;
                if (!string.IsNullOrEmpty(search))
                {
                    categorys = db.ProductCategories.Where(x => x.Name.Contains(search));

                }
                return categorys.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ProductCategory> ListSubCategory(long id)
        {
            List<ProductCategory> productCategories = db.ProductCategories.Where(x => x.Status == true && x.ParentID == id).OrderBy(x => x.DisplayOrder).ToList();
            return (productCategories != null) ? productCategories : null;
        }

        public ProductCategory ViewDetail(long id)
        {
            var model = db.ProductCategories.Find(id);
            return (model != null) ? model : null;
        }

        public bool Update(ProductCategory productCategory)
        {
            try
            {
                ProductCategory original = db.ProductCategories.SingleOrDefault(x => x.ID == productCategory.ID);
                if (original != null)
                {
                    db.Entry(original).CurrentValues.SetValues(productCategory);
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
                var category = db.ProductCategories.Find(id);
                if (category != null)
                {
                    db.ProductCategories.Remove(category);
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
                var category = db.ProductCategories.Find(id);
                category.Status = !category.Status;
                db.SaveChanges();
                return (bool)category.Status;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
