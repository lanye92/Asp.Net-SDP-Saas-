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



        Task Cancel(EntityDto<int> input);


        Task CreateSupplierAsync(CreateSupplierInput input);

        Task<GetSupplierForEditOutput> GetSupplierForEdit(NullableIdDto<long> input);

        Task<Supplier> UpdateSupplierAsync(CreateSupplierInput input);

        Task CreateOrUpdateSupplier(CreateSupplierInput input);

    }
}
