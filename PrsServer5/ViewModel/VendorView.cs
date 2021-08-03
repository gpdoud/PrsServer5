using System;
using PrsServer5.Models;

namespace PrsServer5.ViewModel {
    public class VendorView {

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public VendorView(Vendor vendor) {
            Id = vendor.Id;
            Code = vendor.Code;
            Name = vendor.Name;
            Address = vendor.Address;
            City = vendor.City;
            State = vendor.State;
            Zip = vendor.Zip;
            PhoneNumber = vendor.PhoneNumber;
            Email = vendor.Email;
        }

    }
}
