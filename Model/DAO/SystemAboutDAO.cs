using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class SystemAboutDAO : ISystemAbout
    {
        OnlineShopDBContext db = null;
        public SystemAboutDAO()
        {
            db = new OnlineShopDBContext();
        }
        public SystemAbout GetInfo()
        {
            return db.SystemAbouts.Find(1);
        }
    }
}
