using System;
using PrsServer5.Models;

namespace PrsServer5.DTO {

    public class UserView {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsReviewer { get; set; }
        public bool IsAdmin { get; set; }

        public UserView(User user) {
            Id = user.Id;
            Username = user.Username;
            Firstname = user.Firstname;
            Lastname = user.Lastname;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            IsReviewer = user.IsReviewer;
            IsAdmin = user.IsAdmin;
        }

    }
}
