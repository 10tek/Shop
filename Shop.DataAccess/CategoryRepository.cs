using Shop.DataAccess.Abstract;
using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Shop.DataAccess
{
    public class CategoryRepository : ICategoryRepository
    {
        /*
         * 1. Открыть подключение
         * 2. Создать запрос
         * 3. Выполнить запрос
         * 4. Закрыть подключение
        */
        private readonly string connectionString;
        private readonly DbProviderFactory providerFactory;

        public CategoryRepository(string connectionsString, string providerInvariantName)
        {
            this.connectionString = connectionsString;
            providerFactory = DbProviderFactories.GetFactory(providerInvariantName);
        }

        public void Add(Category category)
        {
            using (DbConnection connection = providerFactory.CreateConnection())
            using (DbCommand sqlCommand = connection.CreateCommand())
            {
                string query = $"insert into Categories (id, creationDate, name, imagePath)  values(@Id, @CreationDate, @DeletedDate, @Name, @ImagePath);";
                sqlCommand.CommandText = query;

                DbParameter parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.Guid;
                parameter.ParameterName = "@Id";
                parameter.Value = category.Id;

                sqlCommand.Parameters.Add(parameter);

                parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.DateTime;
                parameter.ParameterName = "@CreationDate";
                parameter.Value = category.CreationDate;
                sqlCommand.Parameters.Add(parameter);

                parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.String;
                parameter.ParameterName = "@Name";
                parameter.Value = category.Name;
                sqlCommand.Parameters.Add(parameter);

                parameter = providerFactory.CreateParameter();
                parameter.DbType = System.Data.DbType.String;
                parameter.ParameterName = "@ImagePath";
                parameter.Value = category.ImagePath;
                sqlCommand.Parameters.Add(parameter);

                connection.ConnectionString = connectionString;
                connection.Open();

                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        sqlCommand.Transaction = transaction;
                        sqlCommand.ExecuteNonQuery();
                        //и так далее тоже самое с другими командами
                        transaction.Commit();
                    }
                    catch (DbException exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        //private void ExecuteCommandsInTransaction(params SqlCommand[] commands)
        //{

        //    using (SqlTransaction transaction = connection.BeginTransaction())
        //    {
        //        try
        //        {
        //            foreach (var command in commands)
        //            {
        //                command.Transaction = transaction;
        //                command.ExecuteNonQuery();
        //                //и так далее тоже самое с другими командами
        //            }
        //            transaction.Commit();
        //        }
        //        catch (SqlException exception)
        //        {
        //            transaction.Rollback();
        //        }
        //    }
        //}

        public void Delete(Guid categoryId)
        {

        }

        public ICollection<Category> GetAll()
        {
            using (DbConnection connection = providerFactory.CreateConnection())
            using (DbCommand sqlCommand = connection.CreateCommand())
            {
                string query = $"select * from Categories;";
                sqlCommand.CommandText = query;

                connection.ConnectionString = connectionString;
                connection.Open();
                DbDataReader sqlDataReader = sqlCommand.ExecuteReader();

                List<Category> categories = new List<Category>();
                while (sqlDataReader.Read())
                {
                    categories.Add(new Category
                    {
                        Id = Guid.Parse(sqlDataReader["id"].ToString()),
                        CreationDate = DateTime.Parse(sqlDataReader["creationDate"].ToString()),
                        DeletedDate = DateTime.Parse(sqlDataReader["deletedDate"].ToString()),
                        Name = sqlDataReader["name"].ToString(),
                        ImagePath = sqlDataReader["imagePath"].ToString()
                    });
                }
                return categories;
            }
        }

        public void Update(Category category)
        {
            throw new NotImplementedException();
        }

    }
}
