using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.Controllers
{
    public static class SortHelper<T,V>
    {
        public static IEnumerable<T> Order( string sortDirection, IEnumerable<T> filtered, Func<T, V> orderingFunction)
        {
            if (sortDirection == "asc")
                filtered = filtered.OrderBy(orderingFunction);
            else
                filtered = filtered.OrderByDescending(orderingFunction);
            return filtered;
        }
        //public static IEnumerable<T> Order(string sortDirection, IEnumerable<T> filtered, Func<T, int?> orderingFunction)
        //{
        //    if (sortDirection == "asc")
        //        filtered = filtered.OrderBy(orderingFunction);
        //    else
        //        filtered = filtered.OrderByDescending(orderingFunction);
        //    return filtered;
        //}
        //public static IEnumerable<T> Order(string sortDirection, IEnumerable<T> filtered, Func<T, string> orderingFunction)
        //{
        //    if (sortDirection == "asc")
        //        filtered = filtered.OrderBy(orderingFunction);
        //    else
        //        filtered = filtered.OrderByDescending(orderingFunction);
        //    return filtered;
        //}

       
          
         

    }
}