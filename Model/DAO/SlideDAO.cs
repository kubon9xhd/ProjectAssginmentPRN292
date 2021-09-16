using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class SlideDAO : AbstractDAO, ISlideDAO
    {
        public List<Slide> ListAll()
        {
            List<Slide> slides = db.Slides.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
            return (slides != null) ? slides : null;
        }
    }
}
