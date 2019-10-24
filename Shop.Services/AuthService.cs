using Shop.DataAccess;
using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Services
{
    public class AuthService
    {
        private readonly ShopContext context;
        BcryptHasher bcryptHasher = new BcryptHasher();

        public AuthService(ShopContext context)
        {
            this.context = context;
        }

        public void SignUp(string phoneNumber, string password)
        {
            context.Add(new User
            {
                PhoneNumber = phoneNumber,
                Password = bcryptHasher.EncryptPassword(password)
            });
        }

        public bool SignIn(string phoneNumber, string password)
        {

        }
    }
}
