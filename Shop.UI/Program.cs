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
using System.Data.Common;
using System.Data.SqlClient;
using Shop.Domain;
using System.IO;
using System.Linq;
using Shop.DataAccess;
using System.Reflection;
using DbUp;

namespace Shop.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
            IConfigurationRoot configurationRoot = builder.Build();
            var connectionString = configurationRoot.GetConnectionString("DebugConnectionString");
            var providerName = configurationRoot.GetSection("AppConfig").GetChildren().Single(item => item.Key == "ProviderName").Value;

            DbProviderFactories.RegisterFactory(providerName, SqlClientFactory.Instance);

            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            upgrader.PerformUpgrade();
            using (var context = new ShopContext(connectionString, providerName))
            {
                //var category = new Category
                //{
                //    Name = "Компьютерная переферия",
                //    ImagePath = "D:/1/123"
                //};

                //context.Categories.Add(category);

                //context.Categories.Add(new Category
                //{
                //    Name = "Мониторы",
                //    ImagePath = "D:/1/123"
                //});

                //context.Categories.Add(new Category
                //{
                //    Name = "Клавиатуры",
                //    ImagePath = "D:/1/123"
                //});

                //context.Users.Add(new User
                //{
                //    PhoneNumber = "123123",
                //    Password = "123123"
                //});

                //var categories = context.Categories.GetAll();
                //category.ImagePath = "C:/2/535";
                //context.Categories.Update(category);
                //context.Categories.Delete(category.Id);
            }

        }
    }
}
