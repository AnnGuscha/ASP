//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel;

namespace ASP.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Заказы
    {
        [DisplayName("Заказ")]
        public int КодЗаказа { get; set; }
        [DisplayName("Дата заказа")]
        public Nullable<System.DateTime> ДатаЗаказа { get; set; }
        [DisplayName("Дата исполнения")]
        public Nullable<System.DateTime> ДатаИсполнения { get; set; }
        [DisplayName("Заказчик")]
        public int КодЗаказчика { get; set; }
        public Nullable<double> Предоплата { get; set; }
        public string Отметки { get; set; }
        public Nullable<int> Гарантия { get; set; }
        [DisplayName("Сотрудник")]
        public int КодСотрудника { get; set; }
        public Nullable<double> Стоимость { get; set; }

    }
}
