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

            Category category = new Category
            {
                Name = "ПК",
                ImagePath = "C:/data/pc"
            };
            using (var context = new ShopContext(connectionString))
            {
                context.Categories.Add(new Category
                {
                    Name = "Мониторы",
                    ImagePath = "C:/Categories/m"
                });
                context.Categories.Add(new Category
                {
                    Name = "Бытовая техника",
                    ImagePath = "C:/Categories/b"
                });
                context.Categories.Add(new Category
                {
                    Name = "Кондиционеры",
                    ImagePath = "C:/Categories/k"
                });
                context.Categories.Add(new Category
                {
                    Name = "Холодильники",
                    ImagePath = "C:/Categories/f"
                });
                context.Categories.Add(new Category
                {
                    Name = "Газовые плиты",
                    ImagePath = "C:/Categories/p"
                });

            }

            using (var context = new ShopContext(connectionString))
            {
                SearchService searchService = new SearchService(context);
                var pageNumber = 1;
                while (true)
                {
                    var key = Console.ReadKey().Key;
                    if (key == ConsoleKey.RightArrow)
                    {
                        ++pageNumber;
                    }
                    else if (key == ConsoleKey.LeftArrow)
                    {
                        --pageNumber;
                    }
                    else
                    {
                        Console.WriteLine("Введите номер страницы ");
                        if(int.TryParse(Console.ReadLine(), out pageNumber){

                        }
                        searchService.ShowCategories(pageNumber);
                    }
                }

            }

            

        }



        static void ProcessCollections()
        {
            List<string> cityNames = new List<string>
            {
                "Алматы", "Анкара", "Борисвиль", "Нур-Султан", "Ялта"
            };

            List<string> processedCityNames = new List<string>();
            foreach (var name in cityNames)
            {
                if (name.Contains('-'))
                {
                    processedCityNames.Add(name);
                }
            }
            var result = from name in cityNames where name.Contains('-') select name;
            processedCityNames = cityNames.Where(name => name.Contains('-')).ToList();
            

        
        }
    }
}
