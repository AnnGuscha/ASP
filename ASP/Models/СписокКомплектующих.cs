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

    public partial class СписокКомплектующих
    {
        [DisplayName("Код")]
        public int КодСписка { get; set; }
        [DisplayName("Заказ")]
        public int КодЗаказа { get; set; }
        [DisplayName("Комплектующее")]
        public int КодКомплектующего { get; set; }

        public virtual Заказ Заказ { get; set; }
        public virtual Комплектующее Комплектующее { get; set; }
    }
}
