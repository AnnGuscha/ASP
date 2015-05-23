using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.Models
{
    public class ListUslugDTO
    {
        private IList<Заказ> zakaz;
        private IList<Услуга> usluga;

        public ListUslugDTO(IList<Заказ> zakaz, IList<Услуга> usluga)
        {
            this.zakaz = zakaz;
            this.usluga = usluga;
        }

        public IList<Заказ> Zakaz
        {
            get { return zakaz; }
            set { zakaz = value; }
        }

        public IList<Услуга> Usluga
        {
            get { return usluga; }
            set { usluga = value; }
        }
    }
}