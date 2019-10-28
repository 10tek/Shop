using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain
{
    public class User : Entity
    {
        public string Password { get; set; }
        public string Email { get; set; }
        //public string PhoneNumber { get; set; }
        // TODO
        //public string VerificationCode { get; set; }
        //public string Address { get; set; }
        // покупки, комментарии, рейтинги и тд;
    }
}
