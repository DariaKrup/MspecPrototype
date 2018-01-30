namespace AuthorMaintenenceResource.UnitTest
{
    using AuthorMaintenanceResource.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using AuthorMaintenenceResource.Data;
    using AuthorMaintenenceResource.Data.Entities;
    using FizzWare.NBuilder;
    using System;

    [TestClass]
    public class WhenCreateCustomer
    {
        [TestMethod]
        public void customer_is_created()
        {
            var customer = new Customer(1, "jcoxhead");
            //var customer = Builder<Customer>.CreateNew()                
            //    .Build();

            var mockRepository = new Mock<IRepository<Customer>>();
            mockRepository.Setup<Customer>(q => q.Save(It.Is<Customer>(fn => fn.Name == "jcoxhead"))).Returns(() => customer);

            var customerService = new CustomerService(mockRepository.Object);            
            var result = customerService.Save(customer);

            Assert.IsNotNull(customerService);
            Assert.AreEqual(customer.Name, result.Name);

            //mockRepository.Verify(q => q.Save(It.IsAny<Customer>()), Times.Once);
            mockRepository.Verify(q => q.Save(It.Is<Customer>(fn => fn.Name.Equals(result.Name) && fn.Id == result.Id)), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ApplicationException))]
        public void customer_throws_exception()
        {
            Customer customer = null;       
            var mockRepository = new Mock<IRepository<Customer>>();

            mockRepository.Setup(q => q.Save(It.IsAny<Customer>()))
                .Throws<ApplicationException>();

            var customerService = new CustomerService(mockRepository.Object);
            var result = customerService.Save(customer);
        }

        [TestMethod]        
        public void customer_checks_context_inuse()
        {          
            var mockRepository = new Mock<IRepository<Customer>>();

            //mockRepository.SetupSet(foo => foo.ContextInUse = true);

            var customer = new Customer(1, "jcoxhead");

            var customerService = new CustomerService(mockRepository.Object);
            var result = customerService.Save(customer);

            // or verify the setter directly
            mockRepository.VerifySet(foo => foo.ContextInUse = true);
        }

        [TestMethod]
        public void customer_raises_notification_event()
        {
            var mockRepository = new Mock<IRepository<Customer>>();

            //mockRepository.SetupSet(foo => foo.ContextInUse = true);

            var customer = new Customer(1, "jcoxhead");

            var customerService = new CustomerService(mockRepository.Object);
            mockRepository.Setup(q => q.Save(It.IsAny<Customer>()))
                .Returns(() => customer)                
                .Callback(() => mockRepository.Raise(f => f.CustomerNotificationEvent += null, new CustomerNotificationArgs("Hello XXX")));

            var result = customerService.Save(customer);

            // or verify the setter directly          
        }

        private void MyMethod(object sender, CustomerNotificationArgs e)
        {
            int x = 10;
        }
    }
}
