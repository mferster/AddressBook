using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unit1
{
    class Company : Entity<Company>
    {
        public Company(string name, Address address)
        {
            this.Name = name;
            this.Address = address;
        }

        public Company(string name, Address address, string Id)
        {
            this.Name = name;
            this.Address = address;
            this.Id = Id;
        }

        public override bool Equals(object other)
        {
            Company secondParam = other as Company;
            if (this.Name.Equals(secondParam.Name) &&
                this.Address.Equals(secondParam.Address) &&
                this.Id.Equals(secondParam.Id))
                return true;
            else
                return false;
        }

        public string Name { get; set; }
        public Address Address { get; set; }
    }
}
