using Shop.DataAccess;
using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shop.Services
{
    public class SearchUI
    {
        private const int COUNT_IN_PAGE = 3;
        private readonly ShopContext context;

        public SearchUI(ShopContext context)
        {
            this.context = context;
        }

        public void ShowItems()
        {
            var pageNumber = 1;
            var items = context.Items.GetAll().ToList();
            ShowItems(items);
        }

        private void ShowItems(List<Item> items)
        {
            var pageNumber = 1;
            var isExit = false;
            ShowOnePage(items);
            while (!isExit)
            {
                Console.Write("Введите номер страницы или цифру -1 для выхода: ");
                if (int.TryParse(Console.ReadLine(), out pageNumber))
                {
                    if (GetPageCount(items) >= pageNumber && pageNumber > 0)
                    {
                        ShowOnePage(items, pageNumber);
                        continue;
                    }
                    else if(pageNumber == -1)
                    {
                        isExit = true;
                        break;
                    }
                }
                Console.Write("Неккоректный ввод! Повторите попытку: ");
            }
        }

        public void ShowCategoryItems(Category category)
        {
            var items = context.Items.GetAll().Where(x => x.CategoryId == category.Id).ToList();
            ShowItems(items);
        }

        public void SearchByName(string name)
        {
            var items = context.Items.GetAll().Where(x => x.Name.Contains(name)).ToList();
            if (items.Count == 0)
            {
                Console.WriteLine("Таких товаров в магазине еще нет, приходите завтра!");
                return;
            }
            ShowItems(items);
        }

        private void ShowOnePage(List<Item> items, int pageNumber = 1)
        {
            var onePageItems = items.Skip(COUNT_IN_PAGE * --pageNumber).Take(COUNT_IN_PAGE).ToList();
            Console.Clear();
            onePageItems.ForEach(x => Console.WriteLine($"Name: {x.Name}\nDescription: {x.Description}\nPrice: {x.Price}"));
            ShowPages(items, ++pageNumber);
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
