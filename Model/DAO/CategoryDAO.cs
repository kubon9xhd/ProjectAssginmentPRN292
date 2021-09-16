using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class CategoryDAO : ICagetoryDAO
    {
        OnlineShopDBContext db = null;
        public CategoryDAO()
        {
            db = new OnlineShopDBContext();
        }
        public List<Caetgory> ListAll()
        {
            return db.Caetgories.Where(p => p.Status == true).ToList();
        }
        public long Insert(Caetgory caetgory)
        {
            try
            {
                db.Caetgories.Add(caetgory);
                db.SaveChanges();
                return caetgory.ID;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public bool Update(Caetgory caetgory)
        {
            try
            {
                Caetgory original = db.Caetgories.SingleOrDefault(x=> x.ID == caetgory.ID);
                if(original != null)
                {
                    db.Entry(original).CurrentValues.SetValues(caetgory);
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

        public Caetgory GetCaetgoryById(long id)
        {
            var category = db.Caetgories.SingleOrDefault(x => x.ID == id);
            return (category != null) ? category : null;
        }
        public Caetgory GetCaetgoryByIdParent(long id)
        {
            var category = db.Caetgories.SingleOrDefault(x => x.ParentID == id);
            return (category != null) ? category : null;
        }
        public bool Delete(int id)
        {
            try
            {
                var category = db.Caetgories.Find(id);
                if (category != null)
                {
                    db.Caetgories.Remove(category);
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

        public IEnumerable<Caetgory> ListAllPaging(string search, int page, int pageSize)
        {
            try
            {
                IQueryable<Caetgory> categorys = db.Caetgories;
                if (!string.IsNullOrEmpty(search))
                {
                    categorys = db.Caetgories.Where(x => x.Name.Contains(search));

                }
                return categorys.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool ChangeStatus(long id)
        {
            try
            {
                var category = db.Caetgories.Find(id);
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
