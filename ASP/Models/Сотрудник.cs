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

    public partial class Сотрудник
    {
        public Сотрудник()
        {
            this.Заказ = new HashSet<Заказ>();
            this.ПослужнойСписок = new HashSet<ПослужнойСписок>();
        }
        [DisplayName("Сотрудник")]
        public int КодСотрудника { get; set; }
        [DisplayName("ФИО сотрудника")]
        public string ФИО { get; set; }

        public virtual ICollection<Заказ> Заказ { get; set; }
        public virtual ICollection<ПослужнойСписок> ПослужнойСписок { get; set; }
    }
}
