using Abp.Domain.Repositories;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Suppliers
{
    public class SupplierManage : ISupplierManage
    {
        private readonly IRepository<Supplier, long> _supplierRepository;

        public SupplierManage(IRepository<Supplier, long> supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }




        public async Task<Supplier> GetAsync(long id)
        {
            var @event = await _supplierRepository.FirstOrDefaultAsync(id);
            if (@event == null)
            {
                throw new UserFriendlyException("Could not found the supplier, maybe it's deleted!");
            }

            return @event;
        }



        public async Task CreateAsync(Supplier @event)
        {
            await _supplierRepository.InsertAsync(@event);
        }

        public void Cancel(Supplier @event)
        {
            @event.Cancel();
        }


    }
}
