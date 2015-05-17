using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.Models
{
    public class ZakazDTO
    {
        public ZakazDTO(IList<Сотрудник> sotrudnik, IList<Заказчик> zakazchik)
        {
            this.sotrudnik = sotrudnik;
            this.zakazchik = zakazchik;
        }

        public IList<Сотрудник> Sotrudnik
        {
            get { return sotrudnik; }
            set { sotrudnik = value; }
        }

        public IList<Заказчик> Zakazchik
        {
            get { return zakazchik; }
            set { zakazchik = value; }
        }

        private IList<Сотрудник> sotrudnik;
        private IList<Заказчик> zakazchik;
    }
}