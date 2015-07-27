using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unit1
{
    class Customer : Entity<Customer>
    {
        public Customer(string firstName, string lastName, Address address)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Address = address;
        }

        public override bool Equals(object other)
        {
            Customer secondParam = other as Customer;

            if (this.FirstName.Equals(secondParam.FirstName) &&
                this.LastName.Equals(secondParam.LastName) &&
                this.Address.Equals(secondParam.Address) &&
                this.Id.Equals(secondParam.Id))
                return true;
            else
                return false;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
    }
}
