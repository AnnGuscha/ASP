﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.Models
{
    public class ListKomplektDTO
    {
        private IList<Заказ> zakaz;
        private IList<Комплектующее> komplekt;

        public ListKomplektDTO(IList<Заказ> zakaz, IList<Комплектующее> komplekt)
        {
            this.zakaz = zakaz;
            this.komplekt = komplekt;
        }

        public IList<Заказ> Zakaz
        {
            get { return zakaz; }
            set { zakaz = value; }
        }

        public IList<Комплектующее> Komplekt
        {
            get { return komplekt; }
            set { komplekt = value; }
        }
    }
}