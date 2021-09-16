using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ProductDAO : AbstractDAO, IProductDAO
    {
       
        public List<Product> ListNewProduct(int top)
        {
            return db.Products.Where(x=> x.Status == true).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.Status == true).Where(x=>x.TopHot != null && x.TopHot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }
        public List<Product> ListRelatedProduct(long productID)
        {
            var product = db.Products.Find(productID);
            return db.Products.Where(x => x.ID != productID && x.CategoryId == product.CategoryId && x.Status == true).ToList();
        }
        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }

        public List<Product> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 3, string search = "")
        {
            IQueryable<Product> model = db.Products;
            var category = new ProductCategoryDAO().ViewDetail(categoryID);
            if(category != null)
            {
                if (category.ParentID.GetValueOrDefault(0) != 0)
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        model = model.Where(x => x.Name.Contains(search) && x.SubCategoryId == categoryID && x.Status == true);
                        totalRecord = db.Products.Where(x => x.Name.Contains(search) && x.SubCategoryId == categoryID).Count();
                    }
                    else
                    {
                        model = model.Where(x => x.SubCategoryId == categoryID && x.Status == true);
                        totalRecord = db.Products.Where(x => x.SubCategoryId == categoryID).Count();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        model = model.Where(x => x.Name.Contains(search) && (x.CategoryId == categoryID || x.SubCategoryId == categoryID) && x.Status == true);
                        totalRecord = db.Products.Where(x => x.Name.Contains(search) && (x.CategoryId == categoryID || x.SubCategoryId == categoryID) && x.Status == true).Count();
                    }
                    else
                    {
                        model = model.Where(x => x.CategoryId == categoryID);
                        totalRecord = db.Products.Where(x => (x.CategoryId == categoryID || x.SubCategoryId == categoryID) && x.Status == true).Count();
                    }
                }
            }
            var result = model.OrderBy(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return (result != null)? result : null;
        }
        public List<Product> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 3)
        {
            totalRecord = db.Products.Where(x => x.Name.Contains(keyword) && x.Status == true).Count();
            IQueryable<Product> model = db.Products;
            model = model.Where(x => x.Name.Contains(keyword) && x.Status ==true);
            var result = model.OrderBy(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return (result != null) ? result : null;
        }

        public List<String> ListName(string keyWord)
        {
            return db.Products.Where(x => x.Name.Contains(keyWord) && x.Status == true).Select(x => x.Name).ToList();
        }
        public IEnumerable<Product> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Products;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) && x.Status == true);
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
        public void UpdateImages(long productId, string images)
        {
            var product = db.Products.Find(productId);
            product.MoreImages = images;
            db.SaveChanges();
        }

        public long Insert(Product product)
        {
            try
            {
                db.Products.Add(product);
                db.SaveChanges();
                return product.ID;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Update(Product product)
        {
            try
            {
                var original = db.Products.Find(product.ID);
                if (original != null)
                {
                    db.Entry(original).CurrentValues.SetValues(product);
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
                var product = db.Products.Find(id);
                if (product != null)
                {
                    db.Products.Remove(product);
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
                var product = db.Products.Find(id);
                product.Status = !product.Status;
                db.SaveChanges();
                return (bool)product.Status;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
