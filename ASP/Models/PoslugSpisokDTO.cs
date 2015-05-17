using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.Models
{
    public class PoslugSpisokDTO
    {
        public PoslugSpisokDTO(IList<Сотрудник> sotr, IList<Должность> dolgn)
        {
            this.sotr = sotr;
            this.dolgn = dolgn;
        }

        public IList<Сотрудник> Sotr
        {
            get { return sotr; }
            set { sotr = value; }
        }

        public IList<Должность> Dolgn
        {
            get { return dolgn; }
            set { dolgn = value; }
        }

        private IList<Сотрудник> sotr;
        private IList<Должность> dolgn;
    }
}