using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Suppliers.Dtos
{

    [AutoMapFrom(typeof(Supplier))]
    public class GetSupplierForEditOutput
    {
        public long? Id { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }

        public virtual int TenantId { get; set; }
    }
}
