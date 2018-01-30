using System;
using System.Collections.Generic;
using System.Text;

namespace AuthorMaintenenceResource.Data.Entities
{
    public class Customer
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Customer()
        {
        }

        public Customer(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
