using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain
{
    public class User : Entity
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        // TODO
        //public string Email { get; set; }
        //public string VerificationCode { get; set; }
        //public string Address { get; set; }
        // покупки, комментарии, рейтинги и тд;
    }
}
