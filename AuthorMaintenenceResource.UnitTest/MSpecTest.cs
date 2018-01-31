namespace AuthorMaintenenceResource.UnitTest
{
    using AuthorMaintenanceResource.Services;
    using AuthorMaintenenceResource.Data;
    using AuthorMaintenenceResource.Data.Entities;
    using Machine.Specifications;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using FluentAssertions;

    [Subject("Authentication")]
    public class WhenCreatingACustomer
    {
        private static Mock<IRepository<Customer>>  _mockRepository;
        private static CustomerService _customerService;
        private static Customer _customer, _result;

        Establish context = () => {

            _customer = new Customer(1, "jcoxhead");
            _mockRepository = new Mock<IRepository<Customer>>();
            _mockRepository.Setup<Customer>(q => q.Save(Moq.It.Is<Customer>(fn => fn.Name == "jcoxhead"))).Returns(() => _customer);

            _customerService = new CustomerService(_mockRepository.Object);
           
        };

        Because of = () => {
            _result = _customerService.Save(_customer);                  
        };

        Machine.Specifications.It should_have_persisted_customer = () => {
            _mockRepository.Verify(q => q.Save(Moq.It.Is<Customer>(fn => fn.Name.Equals(_result.Name) && fn.Id == _result.Id)), Times.Once);
        };

        Machine.Specifications.It should_instantiate_customer_service = () => {
            _customerService.Should().NotBeNull();
        };
    }
}
