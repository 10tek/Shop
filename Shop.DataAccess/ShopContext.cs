using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.DataAccess
{
    public class ShopContext : IDisposable
    {
        public Repository<Category> Categories { get; set; }
        public Repository<Item> Items { get; set; }
        public Repository<User> Users { get; set; }
        
        public ShopContext(string connectionString, string providerInvariantName)
        {
            Categories = new Repository<Category>(connectionString, providerInvariantName);
            Items = new Repository<Item>(connectionString, providerInvariantName);
            Users = new Repository<User>(connectionString, providerInvariantName);
        }

        public void Dispose()
        {
            Categories.Dispose();
            Items.Dispose();
            Users.Dispose();
        }
    }
}
