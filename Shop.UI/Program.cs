/*
 * 1. Регистрация и вход (смс-код / email код) - сделать до 11 октября
 * 2. История покупок 
 * 3. Категории и товары (картинка в файловой системе)
 * 4. Покупка (Корзина), оплата и доставка (PayPal/Qiwi/etc)
 * 5. Комментарии и рейтинги 
 * 6. Поиск (пагинация)
 * 
 * Кто сделает 3 версии (Подключённый, автономный и EF) получит автомат на экзамене.
 * unit of work
 */

using Microsoft.Extensions.Configuration;
using Shop.DataAccess;
using Shop.DataAccess.Abstract;
using Shop.Domain;
using System;
using System.IO;

namespace Shop.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            #region
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings", false, true);
            //IConfigurationRoot configurationRoot = builder.Build();

            //#region main
            //Category category = new Category
            //{
            //    Name = "Бытовая техника",
            //    ImagePath = @"C:/data",
            //};

            //ICategoryRepository repository = new CategoryRepository(configurationRoot.GetConnectionString("DebugConnectionString"));
            //repository.Add(category);
            //var result = repository.GetAll();


            #endregion
            #region Repository Test
            Repository<User> repository = new Repository<User>();
            repository.Add(new User
            { 
                Address = "Abay st. 129",
                Email = "10tek3@mail.com",
                Password = "123",
                PhoneNumber = "+77786226134",
                VerificationCode = "123"
            });
            #endregion


        }
    }
}
