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
using System;
using Shop.Services;
using System.Collections.Generic;

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
                var category = new Category
                {
                    Name = "Монитор",
                    ImagePath = "D:/1/123"
                };

                context.Categories.Add(category);

                context.Categories.Add(new Category
                {
                    Name = "Компьютерная переферия",
                    ImagePath = "D:/1/123"
                });

                context.Categories.Add(new Category
                {
                    Name = "Клавиатуры",
                    ImagePath = "D:/1/123"
                });

                context.Users.Add(new User
                {
                    PhoneNumber = "123123",
                    Password = "123123"
                });

                context.Items.Add(new Item
                {
                    Name = "LG-001",
                    Description = "Монитор для мониторинга",
                    ImagePath = "C:/2/1",
                    Price = 1111,
                    CategoryId = category.Id
                });
                context.Items.Add(new Item
                {
                    Name = "LG-002",
                    Description = "Монитор не для мониторинга",
                    ImagePath = "C:/2/2",
                    Price = 1112,
                    CategoryId = category.Id
                });
                context.Items.Add(new Item
                {
                    Name = "LG-003",
                    Description = "Монитор",
                    ImagePath = "C:/2/3",
                    Price = 1113,
                    CategoryId = category.Id
                });
                context.Items.Add(new Item
                {
                    Name = "LG-004",
                    Description = "Монитор для игр",
                    ImagePath = "C:/2/4",
                    Price = 1114,
                    CategoryId = category.Id
                });
                context.Items.Add(new Item
                {
                    Name = "LG-005",
                    Description = "Монитор красивый",
                    ImagePath = "C:/2/5",
                    Price = 1115,
                    CategoryId = category.Id
                });

                var categories = context.Categories.GetAll().ToList();
                category.ImagePath = "C:/2/535";
                context.Categories.Update(category);
                var searchService = new SearchUI(context);
                var index = 0;
                var isExit = false;
                while (!isExit)
                {
                    Console.Clear();
                    Console.WriteLine("1 - Вывод всех товаров");
                    Console.WriteLine("2 - Выбор категории");
                    Console.WriteLine("3 - Поиск товара по имени");
                    Console.WriteLine("0 - Выход");
                    if (int.TryParse(Console.ReadLine(), out var menu))
                    {
                        switch (menu)
                        {
                            case 0: isExit = true; break;
                            case 1: searchService.ShowItems(); break;
                            case 2:
                                Console.WriteLine("Выберите категорию: ");
                                categories = context.Categories.GetAll().ToList();
                                index = 0;
                                categories.ForEach(x => Console.WriteLine($"{++index} - {x.Name}"));
                                if (int.TryParse(Console.ReadLine(), out index) && index <= categories.Count && index > 0)
                                {
                                    searchService.ShowCategoryItems(categories[--index]);
                                }
                                else
                                {
                                    Console.WriteLine("Неккоректный выбор пункта меню! Нажмите любую клавишу что бы вернуться в меню.");
                                    Console.ReadKey();
                                }
                                break;
                            case 3:
                                Console.WriteLine("Введите имя товара или часть его названия: ");
                                searchService.SearchByName(Console.ReadLine());
                                break;
                            default: Console.WriteLine("Неккоретный выбор пункта меню! Нажмите любую клавишу что бы вернуться в меню."); break;
                        }
                    }
                }
            }

        }
    }
}
