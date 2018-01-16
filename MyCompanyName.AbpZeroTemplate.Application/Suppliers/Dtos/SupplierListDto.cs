using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Suppliers.Dtos
{
    [AutoMapFrom(typeof(Supplier))]
    public class SupplierListDto : FullAuditedEntityDto<long>
    {
        public string UserName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
    }
}
