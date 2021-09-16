using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class AbstractDAO
    {
        public OnlineShopDBContext db = null;
        public AbstractDAO()
        {
            db = new OnlineShopDBContext();
        }
    }
}
