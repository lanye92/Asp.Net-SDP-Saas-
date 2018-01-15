using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.AutoMapper;
using Abp.Linq.Extensions;
using Abp.UI;
using MyCompanyName.AbpZeroTemplate.Suppliers.Dtos;

namespace MyCompanyName.AbpZeroTemplate.Suppliers
{
    [AbpAuthorize]
    public class SupplierAppService : AbpZeroTemplateAppServiceBase, ISupplierAppService
    {
        private readonly IRepository<Supplier, long> _supplierRepository;
        private readonly ISupplierManage _eventManager;


        public SupplierAppService(
         ISupplierManage eventManager,
         IRepository<Supplier, long> supplierRepository)
        {
            _eventManager = eventManager;
            _supplierRepository = supplierRepository;
        }


        public async Task<ListResultDto<SupplierListDto>> GetList(GetSupplierListInput input)
        {
            var events = await _supplierRepository
                .GetAll()
                .WhereIf(!input.IncludeCanceledSuppliers, e => !e.IsCancelled)
                .OrderByDescending(e => e.Date)
                .Take(64)
                .ToListAsync();
            return new ListResultDto<SupplierListDto>(events.MapTo<List<SupplierListDto>>());
        }


        public async Task Create(CreateSupplierInput input)
        {
            var @event = Supplier.Create(input.Address, input.Phone, input.Type);
            await _eventManager.CreateAsync(@event);
        }

        public async Task Cancel(EntityDto<int> input)
        {
            var @supplier = await _eventManager.GetAsync(input.Id);
            _eventManager.Cancel(@supplier);
        }

    }
}
