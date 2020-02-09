using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain
{
    public class Item : Entity
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public double AvgRating { get; set; } = 0;
        public Category Category { get; set; }
        // рейтинг, комментарии, категория
    }
}
