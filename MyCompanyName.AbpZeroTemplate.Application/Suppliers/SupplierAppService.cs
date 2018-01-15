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
    public class SupplierAppService
    {
        private readonly IRepository<Supplier, int> _supplierRepository;
        private readonly SupplierManage _eventManager;

        public async Task<ListResultDto<SupplierListDto>> GetList(GetSupplierListInput input)
        {
            var events = await _supplierRepository
                .GetAll()
                .WhereIf(!input.IncludeCanceledSuppliers,e=>!e.IsCancelled)
                .OrderByDescending(e => e.Date)
                .Take(64)
                .ToListAsync();

            return new ListResultDto<SupplierListDto>(events.MapTo<List<SupplierListDto>>());
        }


        public async Task Create(CreateSupplierInput input)
        {
            var @event = Supplier.Create(input.Address,input.Phone,input.Type);
            await _eventManager.CreateAsync(@event);
        }

        public async Task Cancel(EntityDto<int> input)
        {
            var @supplier = await _eventManager.GetAsync(input.Id);
            _eventManager.Cancel(@supplier);
        }

        public async Task<EventRegisterOutput> Register(EntityDto<int> input)
        {
            var registration = await RegisterAndSaveAsync(
                await _eventManager.GetAsync(input.Id),
                await GetCurrentUserAsync()
                );

            return new EventRegisterOutput
            {
                RegistrationId = registration.Id
            };
        }

        public async Task CancelRegistration(EntityDto<Guid> input)
        {
            await _eventManager.CancelRegistrationAsync(
                await _eventManager.GetAsync(input.Id),
                await GetCurrentUserAsync()
                );
        }

        private async Task<EventRegistration> RegisterAndSaveAsync(Event @event, User user)
        {
            var registration = await _eventManager.RegisterAsync(@event, user);
            await CurrentUnitOfWork.SaveChangesAsync();
            return registration;
        }
    }
}
