using Shop.DataAccess;
using Shop.Domain;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.UI
{
    public class AuthUI
    {
        private AuthService authService;
        private EmailVerification emailVerification;
        private string email;
        private string password;

        public AuthUI(ShopContext context)
        {
            authService = new AuthService(context);
            emailVerification = new EmailVerification();
        }

        public User SignUp()
        {
            int codeFromUser;
            Console.Write("  Введите почтовый адрес: ");
            email = Console.ReadLine();
            User user;
            int verificationCode = emailVerification.SendCode(email);
            Console.Write("Введите код подтверждения: ");
            if (int.TryParse(Console.ReadLine(), out codeFromUser) && verificationCode == codeFromUser)
            {
                Console.Write("Код правильный! Придумайте пароль: ");
                MaskPassword();
                Console.WriteLine("Успешная регистрация, нажмите любую клавишу что бы выйти в меню!");
                user = authService.SignUp(email, password);
                Console.ReadKey();
            }
            else
            {
                Console.Write("Упс неправильно!");
                user = null;
            }
            return user;
        }

        public User SignIn()
        {
            Console.Write("Введите почтовый адрес: ");
            email = Console.ReadLine();
            Console.Write("         Введите пароль: ");
            MaskPassword();
            var user = authService.SignIn(email, password);
            if(user != null)
            {
                Console.WriteLine("Успешный вход!");
                return user;
            }
            else
            {
                Console.WriteLine("Некорректные данные!");
                return null;
            }
        }

        private string MaskPassword()
        {
            password = string.Empty;
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        Console.Write("\n");
                        break;
                    }
                }
            } while (true);
            return password;
        }
    }
}
