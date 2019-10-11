using Shop.DataAccess.Abstract;
using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

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
        SqlConnection connection;

        public CategoryRepository(string connectionsString)
        {
            this.connectionString = connectionsString;
            connection = new SqlConnection(connectionString);
        }

        ~CategoryRepository()
        {
            connection.Close();
        }

        public void Add(Category category)
        {
            using (SqlCommand sqlCommand = connection.CreateCommand())
            {
                string query = $"insert into Categories (id, creationDate, name, imagePath)  values(@Id, @CreationDate, @DeletedDate, @Name, @ImagePath);";
                sqlCommand.CommandText = query;

                SqlParameter parameter = new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                    ParameterName = "@Id",
                    Value = category.Id,
                };
                sqlCommand.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.SqlDbType = System.Data.SqlDbType.DateTime;
                parameter.ParameterName = "@CreationDate";
                parameter.Value = category.CreationDate;
                sqlCommand.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                parameter.ParameterName = "@Name";
                parameter.Value = category.Name;
                sqlCommand.Parameters.Add(parameter);

                parameter = new SqlParameter();
                parameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                parameter.ParameterName = "@ImagePath";
                parameter.Value = category.ImagePath;
                sqlCommand.Parameters.Add(parameter);

                connection.Open();

                ExecuteCommandsInTransaction(sqlCommand);
            }
        }

        private void ExecuteCommandsInTransaction(params SqlCommand[] commands)
        {
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    foreach (var command in commands)
                    {
                        command.Transaction = transaction;
                        command.ExecuteNonQuery();
                        //и так далее тоже самое с другими командами
                    }
                    transaction.Commit();
                }
                catch (SqlException exception)
                {
                    transaction.Rollback();
                }
            }
        }

        public void Delete(Guid categoryId)
        {

        }

        public ICollection<Category> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand sqlCommand = connection.CreateCommand())
            {
                string query = $"select * from Categories;";
                sqlCommand.CommandText = query;
                connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

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
