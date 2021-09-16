using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class ContactDAO : AbstractDAO, IContactDAO
    {
        public long InsertFeedBack(FeedBack feedBack)
        {
            db.FeedBacks.Add(feedBack);
            db.SaveChanges();
            return feedBack.ID;
        }
    }
}
