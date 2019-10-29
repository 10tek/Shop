using Microsoft.EntityFrameworkCore;
using Shop.DataAccess;
using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Services
{
    public class SearchService
    {
        private const int COUNT_IN_PAGE = 3;
        private readonly ShopContext context;

        public SearchService(ShopContext context)
        {
            this.context = context;
        }

        public void ShowItems()
        {
            var pageNumber = 1;
            var items = context.Items.ToList();
            ShowOnePage(items);
            while (pageNumber != 0)
            {
                Console.Write("Введите номер страницы или цифру 0 для выхода: ");
                if (int.TryParse(Console.ReadLine(), out pageNumber) && GetPageCount(items) >= pageNumber && pageNumber > 0)
                {
                    ShowOnePage(items, pageNumber);
                }
                else
                {
                    Console.Write("Неккоректный ввод! Повторите попытку: ");
                }
            }
        }

        public void ShowCategoryItems(Category category)
        {
            var pageNumber = 1;
            var items = context.Items.Where(x => x.Category == category).ToList();
            ShowOnePage(items);
            while (pageNumber != 0)
            {
                Console.Write("Введите номер страницы или цифру 0 для выхода: ");
                if (int.TryParse(Console.ReadLine(), out pageNumber) && GetPageCount(items) >= pageNumber && pageNumber > 0)
                {
                    ShowOnePage(items, pageNumber);
                }
                else
                {
                    Console.Write("Неккоректный ввод! Повторите попытку: ");
                }
            }
        }

        public void SearchByName(string name)
        {
            var pageNumber = 1;
            var items = context.Items.Where(x => x.Name.Contains(name)).ToList();
            if(items.Count == 0)
            {
                Console.WriteLine("Таких товаров в магазине еще нет, приходите завтра!");
                return;
            }
            ShowOnePage(items);
            while (pageNumber != 0)
            {
                Console.Write("Введите номер страницы или цифру 0 для выхода: ");
                if (int.TryParse(Console.ReadLine(), out pageNumber) && GetPageCount(items) >= pageNumber && pageNumber > 0)
                {
                    ShowOnePage(items, pageNumber);
                }
                else
                {
                    Console.Write("Неккоректный ввод! Повторите попытку: ");
                }
            }
        }

        private void ShowOnePage(List<Item> items, int pageNumber = 1)
        {
            var onePageItems = items.Skip(COUNT_IN_PAGE * --pageNumber).Take(COUNT_IN_PAGE).ToList();
            Console.Clear();
            onePageItems.ForEach(x => Console.WriteLine($"Name: {x.Name}\nDescription: {x.Description}\nAvg rating: {x.AvgRating}\n"));
            ShowPages(items,++pageNumber);
        }

        private void ShowPages(List<Item> items, int pageNumber = 1)
        {
            Console.WriteLine($" {pageNumber} | {GetPageCount(items)}");
        }

        private int GetPageCount(List<Item> items)
        {
            return (int)Math.Ceiling(items.Count / (double)COUNT_IN_PAGE);
        }
    }
}
