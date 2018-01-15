using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using Abp.UI;

namespace MyCompanyName.AbpZeroTemplate.Suppliers
{
    [Table("AppSupplier")]
    public class Supplier : FullAuditedEntity<long>, IHasCreationTime,IMustHaveTenant
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public virtual DateTime Date { get; protected set; }

        public virtual bool IsCancelled { get; protected set; }

        public virtual int TenantId { get; set; }

        public static Supplier Create(string address,string phone,string type)
        {
            var @supplier = new Supplier
            {
                Address = address,
                Phone = phone,
                Type = type,
                Date = DateTime.Now,
                IsCancelled=false
            };
            return @supplier;
        }

        public bool IsInPast()
        {
            return Date < Clock.Now;
        }

        public bool IsAllowedCancellationTimeEnded()
        {
            return Date.Subtract(Clock.Now).TotalHours <= 2.0; //2 hours can be defined as Event property and determined per event
        }

        public void ChangeDate(DateTime date)
        {
            if (date == Date)
            {
                return;
            }

            SetDate(date);

        }

        internal void Cancel()
        {
            AssertNotInPast();
            IsCancelled = true;
        }

        private void SetDate(DateTime date)
        {
            AssertNotCancelled();

            if (date < Clock.Now)
            {
                throw new UserFriendlyException("Can not set an event's date in the past!");
            }

            if (date <= Clock.Now.AddHours(3)) //3 can be configurable per tenant
            {
                throw new UserFriendlyException("Should set an event's date 3 hours before at least!");
            }

            Date = date;
        }

        private void AssertNotInPast()
        {
            if (IsInPast())
            {
                throw new UserFriendlyException("This event was in the past");
            }
        }

        private void AssertNotCancelled()
        {
            if (IsCancelled)
            {
                throw new UserFriendlyException("This event is canceled!");
            }
        }

    }


}
