//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ВсеКомплектующие
    {
        public int КодКомплектующего { get; set; }
        public string Марка { get; set; }
        public string ФирмаПроизводитель { get; set; }
        public string СтранаПроизводитель { get; set; }
        public Nullable<System.DateTime> ДатаВыпуска { get; set; }
        public string Характеристики { get; set; }
        public Nullable<int> СрокГарантии { get; set; }
        public string Описание { get; set; }
        public Nullable<double> Цена { get; set; }
        public string Вид { get; set; }
    }
}
