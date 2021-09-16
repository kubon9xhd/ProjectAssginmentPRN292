using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public interface IUserDAO
    {
        IEnumerable<User> ListAllPaging(string search,int page, int pageSize);
        long Insert(User entity);
        bool Update(User entity);
        User GetByUsername(string username);
        User GetByID(int id);
        bool IsExistEmail(string email);
        int Login(string username, string password, bool isLoginAdmin = false);
        bool Delete(int id);
        bool ChangeStatus(long id);
    }
}
