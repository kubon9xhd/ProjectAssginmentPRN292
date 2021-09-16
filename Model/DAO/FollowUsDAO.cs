using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class FollowUsDAO : IFollowUs
    {
        OnlineShopDBContext db = null;
        public FollowUsDAO()
        {
            db = new OnlineShopDBContext();
        }

        public List<FollowU> ListAll()
        {
            var list = db.FollowUs.ToList();
            return (list != null) ? list : null;
        }
    }
}
