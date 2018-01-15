using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompanyName.AbpZeroTemplate.Suppliers
{
    public interface ISupplierManage : IDomainService
    {
        Task<Supplier> GetAsync(int id);

        Task CreateAsync(Supplier @event);

        void Cancel(Supplier @event);

    }
}
