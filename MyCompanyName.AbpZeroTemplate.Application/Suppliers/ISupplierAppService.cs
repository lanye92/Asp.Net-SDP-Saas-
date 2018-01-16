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
        Task<PagedResultDto<SupplierListDto>> GetList(GetSupplierListInput input);



        Task<GetSupplierForEditOutput> GetSupplierForEdit(NullableIdDto<long> input);


        Task CreateOrUpdateSupplier(CreateSupplierInput input);


        Task DeleteSupplier(EntityDto<long> input);

    }
}
