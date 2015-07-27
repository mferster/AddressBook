using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unit1
{
    class Address
    {
        public Address(string street, string city, string state, string zipcode)
        {
            this.Street = street;
            this.City = city;
            this.State = state;
            this.Zipcode = zipcode;
        }

        public override bool Equals(object other)
        {
            Address secondParam = other as Address;
            if (this.Street.Equals(secondParam.Street) &&
                this.City.Equals(secondParam.City) &&
                this.State.Equals(secondParam.State) &&
                this.Zipcode.Equals(secondParam.Zipcode))
                return true;
            else
                return false;
        }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
}
