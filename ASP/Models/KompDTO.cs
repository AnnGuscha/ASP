using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.Models
{
    public class KompDTO
    {
        public KompDTO(IList<ВидКомплектующих> vid)
        {
            Vid = vid;
        }

        public IList<ВидКомплектующих> Vid
        {
            get { return vid; }
            set { vid = value; }
        }

        //private IList<Комплектующее> Comp;
        private IList<ВидКомплектующих> vid;
    }
}