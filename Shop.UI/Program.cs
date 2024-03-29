﻿/*
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

using Airport.DataAccess;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using Shop.Domain;
using System.IO;
using System.Linq;

namespace Shop.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            #region
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot configurationRoot = builder.Build();
            var connectionString = configurationRoot.GetConnectionString("DebugConnectionString");
            var providerName = configurationRoot.GetSection("AppConfig").GetChildren().Single(item => item.Key == "ProviderName").Value;

            DbProviderFactories.RegisterFactory(providerName, SqlClientFactory.Instance);

            Repository<Category> repository = new Repository<Category>(connectionString, providerName);



            #endregion
            #region Repository Test
            //Category category2 = new Category
            //{
            //    Name = "Мышки",
            //    ImagePath = @"C:/data",
            //};

            //Repository<Category> repository = new Repository<Category>(configurationRoot.GetConnectionString("DebugConnectionString"));
            //repository.Add(category2);
            //var res = repository.GetAll();

            //Repository<User> repository = new Repository<User>("");
            //repository.Add(new User
            //{ 
            //    Address = "Abay st. 129",
            //    Email = "10tek3@mail.com",
            //    Password = "123",
            //    PhoneNumber = "+77786226134",
            //    VerificationCode = "123"
            //});
            #endregion


        }
    }
}
