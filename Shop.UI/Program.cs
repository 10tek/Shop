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

/* TRIGGER
use ShopEF;
GO
CREATE TRIGGER Products_INSERT
ON Comments
AFTER INSERT
AS
UPDATE Items
Set AvgRating = (select AVG(Rating) from Comments as q where ItemId = q.ItemId);*/

using Microsoft.Extensions.Configuration;
using Shop.DataAccess;
using Shop.Domain;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;

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

            using (var context = new ShopContext(connectionString))
            {
                #region Заполнение
                Category monitors = new Category
                {
                    Name = "Мониторы",
                    ImagePath = "C:/Categories/m"
                };
                Category tech = new Category
                {
                    Name = "Бытовая техника",
                    ImagePath = "C:/Categories/b"
                };
                Category cond = new Category
                {
                    Name = "Кондиционеры",
                    ImagePath = "C:/Categories/k"
                };
                Category freez = new Category
                {
                    Name = "Холодильники",
                    ImagePath = "C:/Categories/f"
                };
                Category plits = new Category
                {
                    Name = "Газовые плиты",
                    ImagePath = "C:/Categories/p"
                };

                Item item = new Item
                {
                    Category = monitors,
                    Description = "Красивый монитор",
                    ImagePath = "d:/monitors",
                    Name = "LG-123123",
                    Price = 39990
                };

                Item itemSecond = new Item
                {
                    Category = monitors,
                    Description = "Маленький монитор",
                    ImagePath = "d:/monitors",
                    Name = "LG-small edition",
                    Price = 39990
                };

                //context.AddRange(monitors, tech, cond, freez, plits);
                //context.Add(item);
                context.Add(itemSecond);
                context.SaveChanges();
                #endregion
                var authUI = new AuthUI(context);
                User currentUser = null;
                var categories = new List<Category>();
                Console.WriteLine("1 - Вход");
                Console.WriteLine("2 - Регистрация");
                if (int.TryParse(Console.ReadLine(), out var menu))
                {
                    switch (menu)
                    {
                        case 1: currentUser = authUI.SignIn(); break;
                        case 2: currentUser = authUI.SignUp(); break;
                        default: Console.WriteLine("Неккоретный выбор пункта меню!"); break;
                    }
                }
                if (currentUser != null)
                {
                    var searchService = new SearchService(context);
                    var index = 0;
                    var isExit = false;
                    while (!isExit)
                    {
                        Console.Clear();
                        Console.WriteLine("1 - Вывод всех товаров");
                        Console.WriteLine("2 - Выбор категории");
                        Console.WriteLine("3 - Поиск товара по имени");
                        Console.WriteLine("0 - Выход");
                        if (int.TryParse(Console.ReadLine(), out menu))
                        {
                            switch (menu)
                            {
                                case 0: isExit = true; break;
                                case 1: searchService.ShowItems(); break;
                                case 2:
                                    Console.WriteLine("Выберите категорию: ");
                                    categories = context.Categories.ToList();
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

            #region Pagination
            //using (var context = new ShopContext(connectionString))
            //{
            //    SearchService searchService = new SearchService(context);
            //    var pageNumber = 1;
            //    while (true)
            //    {
            //        var key = Console.ReadKey().Key;
            //        if (key == ConsoleKey.RightArrow)
            //        {
            //            ++pageNumber;
            //        }
            //        else if (key == ConsoleKey.LeftArrow)
            //        {
            //            --pageNumber;
            //        }
            //        else
            //        {
            //            Console.WriteLine("Введите номер страницы ");
            //            if(int.TryParse(Console.ReadLine(), out pageNumber){

            //            }
            //            searchService.ShowCategories(pageNumber);
            //        }
            //    }
            //}
            #endregion
        }
    }
}
