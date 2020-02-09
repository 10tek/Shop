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
        private BcryptHasher bcryptHasher = new BcryptHasher();
        private EmailVerification emailVerification = new EmailVerification();

        public AuthService(ShopContext context)
        {
            this.context = context;
        }

        public User SignUp(string email, string password)
        {
            if (Authentication(email)) return null;
            var user = new User
            {
                Email = email,
                Password = bcryptHasher.EncryptPassword(password)
            };

            context.Add(user);
            context.SaveChanges();
            return user;
        }

        public User SignIn(string email, string password)
        {
            var user = context.Users.SingleOrDefault(x => x.Email == email);
            if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.Password)) 
                return null;
            return user;
        }

        private bool Authentication(string email)
        {
            if (context.Users.SingleOrDefault(x => x.Email == email) is null)
            {
                return false;
            }
            return true;
        }
    }
}
