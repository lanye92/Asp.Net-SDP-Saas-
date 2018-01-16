using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.UI;
using MyCompanyName.AbpZeroTemplate.Suppliers.Dtos;
using Abp.Extensions;
using MyCompanyName.AbpZeroTemplate.Authorization;
using System.Diagnostics;

namespace MyCompanyName.AbpZeroTemplate.Suppliers
{
    [AbpAuthorize]
    public class SupplierAppService : AbpZeroTemplateAppServiceBase, ISupplierAppService
    {
        private readonly IRepository<Supplier, long> _supplierRepository;
        private readonly ISupplierManage _supplierManager;


        public SupplierAppService(
         ISupplierManage supplierManager,
         IRepository<Supplier, long> supplierRepository)
        {
            _supplierManager = supplierManager;
            _supplierRepository = supplierRepository;
        }


        public async Task<PagedResultDto<SupplierListDto>> GetList(GetSupplierListInput input)
        {
            var query = _supplierRepository.GetAll()
             .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(input.Filter) ||
                        u.Address.Contains(input.Filter) ||
                        u.Phone.Contains(input.Filter) ||
                        u.Type.Contains(input.Filter)
                );
            var userCount = await query.CountAsync();
            try
            {
                var users = await query
               .OrderBy(input.Sorting)
               .PageBy(input)
               .ToListAsync();

                var userListDtos = users.MapTo<List<SupplierListDto>>();

                return new PagedResultDto<SupplierListDto>(
                    userCount,
                    userListDtos
                    );
            }
            catch (Exception e)
            {
                var err = e;
            }
            return null;


        }

        [AbpAuthorize(AppPermissions.Pages_Tenants_Supplier_Create)]
        public virtual async Task CreateSupplierAsync(CreateSupplierInput input)
        {
            var @event = Supplier.Create(input.UserName, input.Address, input.Phone, input.Type);
            await _supplierManager.CreateAsync(@event);
        }


        [AbpAuthorize(AppPermissions.Pages_Tenants_Supplier_Create, AppPermissions.Pages_Tenants_Supplier_Edit)]
        public async Task<GetSupplierForEditOutput> GetSupplierForEdit(NullableIdDto<long> input)
        {
            var output = new GetSupplierForEditOutput();

            if (input.Id.HasValue)
            {
                var @supplier = await _supplierManager.GetAsync(input.Id.Value);
                output = @supplier.MapTo<GetSupplierForEditOutput>();

            }
            return output;
        }


        [AbpAuthorize(AppPermissions.Pages_Tenants_Supplier_Edit)]
        public virtual async Task UpdateSupplierAsync(CreateSupplierInput input)
        {
            try
            {
                Debug.Assert(input.Id != null, "input.User.Id should be set.");

                var supplier = await _supplierManager.GetAsync(input.Id.Value);

                //Update user properties
                input.MapTo(supplier); //Passwords is not mapped (see mapping configuration)
                await _supplierRepository.UpdateAsync(supplier);
            }
            catch (Exception ex)
            {
                var e = ex;
            }


        }


        public async Task CreateOrUpdateSupplier(CreateSupplierInput input)
        {
            try
            {
                if (input.Id.HasValue)
                {
                    await UpdateSupplierAsync(input);
                }
                else
                {
                    await CreateSupplierAsync(input);
                }
            }
            catch (Exception ex)
            {
                var e = ex;
            }
        }


        [AbpAuthorize(AppPermissions.Pages_Tenants_Supplier_Delete)]
        public async Task DeleteSupplier(EntityDto<long> input)
        {
            var supplier = await _supplierManager.GetAsync(input.Id);
            await _supplierRepository.DeleteAsync(supplier);
        }
    }
}
