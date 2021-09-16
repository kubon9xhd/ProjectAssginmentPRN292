using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model.EF;
using PagedList;

namespace Model.DAO
{
    public class UserDAO : IUserDAO
    {
        OnlineShopDBContext db = null;
        public UserDAO()
        {
            db = new OnlineShopDBContext();
        }
        public IEnumerable<User> ListAllPaging(string search, int page,int pageSize)
        {
            try
            {
                IQueryable<User> users = db.Users;
                if (!string.IsNullOrEmpty(search))
                {
                     users =  db.Users.Where(x => x.Name.Contains(search) || x.Username.Contains(search));

                }
                return users.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

            }
            catch (Exception)
            {
                return null;
            }
        }
        public long Insert(User entity)
        {
            try
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public User GetByUsername(string username)
        {
            try
            {
                return db.Users.SingleOrDefault(x => x.Username == username);
            }
            catch (Exception)
            {
                return null;
            }
        }
      
        public User GetByID(int id)
        {
            var user = db.Users.SingleOrDefault(x => x.ID == id);
            return (user != null)? user : null;
        }
        public bool IsExistEmail(string email)
        {
            var user = db.Users.SingleOrDefault(x => x.Email == email);
            return (user != null) ? true : false;
        }
        public int Login(string username, string password, bool isLoginAdmin = false) {
            var result = db.Users.SingleOrDefault(x => x.Username == username && x.Password == password);
            if(result == null)
            {
                return 0;
            }
            else
            {
                if(isLoginAdmin == true )
                {
                    if((result.GroupID == CommonConstant.ADMIN_GROUP || result.GroupID == CommonConstant.MODER_GROUP))
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return -2;
                    }
                   
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                
            }
        }
        public bool Update(User entity)
        {
            try
            {
                // find entity
                User user = db.Users.SingleOrDefault(e => e.ID == entity.ID);
                user.Username = entity.Username;
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Address = entity.Address;
                user.Name = entity.Name;
                user.Email = entity.Email;
                user.Phone = entity.Phone;
                user.ModifyBy = entity.ModifyBy;
                user.ModifyDate = DateTime.Now;
                db.SaveChanges();
                return true;
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
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
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
                var user = db.Users.Find(id);
                user.Status = !user.Status;
                db.SaveChanges();
                return (bool)user.Status;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
