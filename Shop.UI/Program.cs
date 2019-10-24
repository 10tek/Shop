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
                //Category monitors = new Category
                //{
                //    Name = "Мониторы",
                //    ImagePath = "C:/Categories/m"
                //};
                //Category tech = new Category
                //{
                //    Name = "Бытовая техника",
                //    ImagePath = "C:/Categories/b"
                //};
                //Category cond = new Category
                //{
                //    Name = "Кондиционеры",
                //    ImagePath = "C:/Categories/k"
                //};
                //Category freez = new Category
                //{
                //    Name = "Холодильники",
                //    ImagePath = "C:/Categories/f"
                //};
                //Category plits = new Category
                //{
                //    Name = "Газовые плиты",
                //    ImagePath = "C:/Categories/p"
                //};
                ////context.AddRange(monitors, tech, cond, freez, plits);

                //Item item = new Item
                //{
                //    Category = monitors,
                //    Description = "Красивый монитор",
                //    ImagePath = "d:/monitors",
                //    Name = "LG-123123",
                //    Price = 39990
                //};

                //User firstUser = new User
                //{
                //    Password = "zazazel123",
                //    PhoneNumber = "123123123",
                //};

                //User secondUser = new User
                //{
                //    Password = "zazazel123",
                //    PhoneNumber = "123123123",
                //};

                //User thirdUser = new User
                //{
                //    Password = "zaza2223",
                //    PhoneNumber = "12123123",
                //};

                //Comment comment = new Comment
                //{
                //    Item = item,
                //    Rating = 5,
                //    Text = "Купил, вроде збс",
                //    User = firstUser,
                //};

                //Comment secondComment = new Comment
                //{
                //    Item = item,
                //    Rating = 3,
                //    Text = "Купил, фигня",
                //    User = secondUser
                //};

                ////context.Add(item);
                ////context.Add(firstUser);
                ////context.Add(secondUser);
                ////context.Add(comment);
                ////context.Add(secondComment);

                //Comment thirdComment = new Comment
                //{
                //    Item = item,
                //    Rating = 1,
                //    Text = "Купил, govno",
                //    User = thirdUser
                //};
                //context.Add(thirdComment);
                context.SaveChanges();
                #endregion

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
