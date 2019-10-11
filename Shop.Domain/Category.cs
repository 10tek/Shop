using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.Domain
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        // опционально коллекция товаров.
    }
}
