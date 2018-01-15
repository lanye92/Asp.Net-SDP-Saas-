using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Suppliers.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Suppliers
{
    public interface ISupplierAppService : IApplicationService
    {
        Task<ListResultDto<SupplierListDto>> GetList(GetSupplierListInput input);


        Task Create(CreateSupplierInput input);

        Task Cancel(EntityDto<int> input);

    }
}
