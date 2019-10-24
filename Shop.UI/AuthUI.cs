using Shop.DataAccess;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.UI
{
    public class AuthUI
    {
        private SmsVerification smsVerification = new SmsVerification();
        private AuthService authService;
        private string phoneNumber;
        private string password;

        public AuthUI(ShopContext context)
        {
            authService = new AuthService(context);
        }

        public void SignUp()
        {
            int codeFromUser;
            Console.Write("   Введите номер телефона: ");
            phoneNumber = Console.ReadLine();
            int verificationCode = smsVerification.SendCode(phoneNumber);
            Console.Write("Введите код подтверждения: ");
            if (int.TryParse(Console.ReadLine(), out codeFromUser) && verificationCode == codeFromUser)
            {
                Console.Write("Код правильный! Придумайте пароль: ");
                password = Console.ReadLine();
                authService.SignUp(phoneNumber, password);
            }
            else
            {
                Console.Write("Упс неправильно!");
            }
        }

        public bool SignIn()
        {
            Console.Write("Введите номер телефона: ");
            phoneNumber = Console.ReadLine();
            Console.Write("        Введите пароль: ");
            password = Console.ReadLine();
            if(bc)
        }
    }
}
