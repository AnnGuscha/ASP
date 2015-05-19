using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.Models
{
    public class ZakazDTO
    {
        private IList<Сотрудник> sotrudnik;
        private IList<Заказчик> zakazchik;
        private IList<Комплектующее> komplect;

        public ZakazDTO(IList<Сотрудник> sotrudnik, IList<Заказчик> zakazchik, IList<Комплектующее> komplect )
        {
            this.sotrudnik = sotrudnik;
            this.zakazchik = zakazchik;
            this.komplect = komplect;
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

        public IList<Комплектующее> Komplect
        {
            get { return komplect; }
            set { komplect = value; }
        }
    }
}