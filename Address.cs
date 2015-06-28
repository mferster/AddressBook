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
            this.street = street;
            this.city = city;
            this.state = state;
            this.zipcode = zipcode;
        }

        public override string ToString()
        {
            return String.Format("{0}_{1}_{2}_{3}", street, city, state, zipcode);
        }

        public override bool Equals(object other)
        {
            Address secondParam = other as Address;
            if (this.street.Equals(secondParam.street) &&
                this.city.Equals(secondParam.city) &&
                this.state.Equals(secondParam.state) &&
                this.zipcode.Equals(secondParam.zipcode))
                return true;
            else
                return false;
        }

        private string street { get; set; }
        private string city { get; set; }
        private string state { get; set; }
        private string zipcode { get; set; }
    }
}
