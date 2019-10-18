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
        private const int PRODUCT_COUNT_IN_PAGE = 5;
        private readonly ShopContext context;

        public SearchService(ShopContext context)
        {
            this.context = context;
        }

        public int ShowCategories(int pageNumber = 1)
        {
            var categories = context.Categories.Skip(PRODUCT_COUNT_IN_PAGE * --pageNumber).Take(PRODUCT_COUNT_IN_PAGE).ToList();
            var pageCount = (int)Math.Ceiling(categories.Count / (double)PRODUCT_COUNT_IN_PAGE);

            foreach(var category in categories)
            {
                Console.WriteLine($"Category name: {category.Name}\n");
            }
            Console.WriteLine($"{++pageNumber} | {pageCount}");
            return pageCount;
        }


    }
}
