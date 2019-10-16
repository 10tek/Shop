using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Shop.DataAccess
{
    class UserRepository : IDisposable
    {
        private readonly DbConnection connection;
        private readonly DbProviderFactory providerFactory;

        public UserRepository(string providerName, string connectionString)
        {
            providerFactory = DbProviderFactories.GetFactory(providerName);
            connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
        }

        public void Add (User user)
        {

        }

        public void Delete(Guid id)
        {

        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}
