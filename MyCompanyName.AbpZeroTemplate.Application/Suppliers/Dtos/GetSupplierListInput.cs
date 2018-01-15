using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Suppliers.Dtos
{
    public class GetSupplierListInput
    {
        public int id { get; set; }
        public string key { get; set; }

        public bool IncludeCanceledSuppliers { get; set; }

    }
}
