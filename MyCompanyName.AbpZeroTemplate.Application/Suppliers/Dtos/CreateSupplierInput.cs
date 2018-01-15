using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Suppliers.Dtos
{
    public class CreateSupplierInput
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public virtual DateTime Date { get; protected set; }

        public virtual bool IsCancelled { get; protected set; }
    }
}
