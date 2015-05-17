//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Комплектующее
    {
        public Комплектующее()
        {
            this.СписокКомплектующих = new HashSet<СписокКомплектующих>();
        }
        [DisplayName("Комплектующее")]
        public int КодКомплектующего { get; set; }
        [DisplayName("Вид")]
        public int КодВида { get; set; }
        public string Марка { get; set; }
        [DisplayName("Фирма производитель")]
        public string ФирмаПроизводитель { get; set; }
        [DisplayName("Страна производитель")]
        public string СтранаПроизводитель { get; set; }
        [DisplayName("Дата выпуска")]
        public Nullable<System.DateTime> ДатаВыпуска { get; set; }
        public string Характеристики { get; set; }
        [DisplayName("Срок гарантии")]
        public Nullable<int> СрокГарантии { get; set; }
        public string Описание { get; set; }
        public Nullable<double> Цена { get; set; }
        
        public virtual ВидКомплектующих ВидКомплектующих { get; set; }
        public virtual ICollection<СписокКомплектующих> СписокКомплектующих { get; set; }
    }
}
