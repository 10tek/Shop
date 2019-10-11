using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain
{
    public class User : Entity
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string VerificationCode { get; set; }

        // TODO
        // покупки, комментарии, рейтинги и тд;
    }
}
