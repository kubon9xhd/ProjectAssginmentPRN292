using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ContentDAO : IContentDAO
    {
        OnlineShopDBContext db = null;
        public ContentDAO()
        {
            db = new OnlineShopDBContext();
        }
        public IEnumerable<Content> ListAllPaging(string search, int page, int pageSize)
        {
            try
            {
                IQueryable<Content> content = db.Contents;
                if (!string.IsNullOrEmpty(search))
                {
                    content = db.Contents.Where(x => x.Name.Contains(search));
                }
                return content.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

            }
            catch (Exception)
            {
                return null;
            }
        }

        public Content GetByID(long id)
        {
            var content = db.Contents.SingleOrDefault(x => x.ID == id);
            return (content != null) ? content : null;
        }

        public long Insert(Content content)
        {
            try
            {
                db.Contents.Add(content);
                db.SaveChanges();
                return content.ID;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool Update(Content content)
        {
            try
            {
                var original = db.Contents.Find(content.ID);
                if (original != null)
                {
                    db.Entry(original).CurrentValues.SetValues(content);
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
                var content = db.Contents.Find(id);
                if (content != null)
                {
                    db.Contents.Remove(content);
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
                var content = db.Contents.Find(id);
                content.Status = !content.Status;
                db.SaveChanges();
                return (bool)content.Status;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
