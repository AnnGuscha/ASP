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
    
    public partial class СписокУслуг
    {
        public int КодСписка { get; set; }
        public int КодЗаказа { get; set; }
        public int КодУслуги { get; set; }
    
        public virtual Заказ Заказ { get; set; }
        public virtual Услуга Услуга { get; set; }
    }
}
