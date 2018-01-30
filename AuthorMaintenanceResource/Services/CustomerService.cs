namespace AuthorMaintenanceResource.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AuthorMaintenenceResource.Data;
    using AuthorMaintenenceResource.Data.Entities;

    public class CustomerService
    {        
        private IRepository<Customer> _repository;

        private CustomerService()
        {          
        }

        public CustomerService(IRepository<Customer> repository)
        {
            _repository = repository;
            _repository.CustomerNotificationEvent += NotifyCustomer;
        }

        private void NotifyCustomer(object sender, CustomerNotificationArgs e)
        {
            int x = 10;
        }

        public Customer Save(Customer customer)
        {
            _repository.ContextInUse = true;

            return _repository.Save(customer);
        }
    }
}
