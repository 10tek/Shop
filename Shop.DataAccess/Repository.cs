using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;
using Shop.DataAccess.Abstract;
using Shop.Domain;
using Dapper;
using System.Linq;

namespace Shop.DataAccess
{
    public class Repository<T> : IRepository<T>, IDisposable where T : Entity
    {
        private readonly DbProviderFactory providerFactory;
        private DbConnection connection;
        private Type type = typeof(T);

        public Repository(string connectionString, string providerInvariantName)
        {
            providerFactory = DbProviderFactories.GetFactory(providerInvariantName);
            connection = providerFactory.CreateConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
        }

        /// <summary>
        /// Добавление в БД, принимает любую сущность, если она наследуется от Entity
        /// </summary>
        /// <param name="element"></param>
        public void Add(T element)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"INSERT into [{type.Name}] (");

            foreach (var propertyInfo in type.GetProperties())
            {
                if (IsNullOrEmpty(propertyInfo, element))
                {
                    stringBuilder.Append($"[{propertyInfo.Name}], ");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 1);
            stringBuilder.Append(") Values (");

            foreach (var propertyInfo in type.GetProperties())
            {
                if (IsNullOrEmpty(propertyInfo, element))
                {
                    stringBuilder.Append($"@{propertyInfo.Name}, ");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 1);
            stringBuilder.Append(");");

            var sqlQuery = stringBuilder.ToString();

            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var rowAffected = connection.Execute(sqlQuery, element, transaction);
                    //и так далее тоже самое с другими командами
                    transaction.Commit();
                }
                catch (DbException exception)
                {
                    Console.WriteLine(exception.Message);
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// Принимает Guid элемента и удаляет его из бд
        /// </summary>
        /// <param name="elementId"></param>
        public void Delete(Guid elementId)
        {
            var sqlQuery = $"update [{type.Name}] set DeletedDate = '{DateTime.Now.ToShortDateString()}' where Id = '{elementId}';";
            using (DbTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    var rowAffected = connection.Execute(sqlQuery, new { Id = elementId }, transaction);
                    //и так далее тоже самое с другими командами
                    transaction.Commit();
                }
                catch (DbException exception)
                {
                    Console.WriteLine(exception.Message);
                    transaction.Rollback();
                }
            }
        }

        public ICollection<T> GetAll()
        {
            var sqlQuery = $"Select * From {type.Name.ToString()};";
            var table = connection.Query<T>(sqlQuery).ToList();
            return table;
        }

        public void Update(T element)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Update [{type.Name}] Set ");

            foreach (var propertyInfo in type.GetProperties())
            {
                if (propertyInfo.GetValue(element) is null) continue;
                stringBuilder.Append($"@{propertyInfo.Name.ToString()} = @{propertyInfo.Name.ToString()}, ");
            }

            stringBuilder.Remove(stringBuilder.Length - 2, 1);
            stringBuilder.Append($" Where Id = {element.Id};");
            var sqlQuery = stringBuilder.ToString();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    var rawAffected = connection.Execute(sqlQuery, element, transaction);
                    //и так далее тоже самое с другими командами
                    transaction.Commit();
                }
                catch (DbException exception)
                {
                    Console.WriteLine(exception.Message);
                    transaction.Rollback();
                }
            }
        }

        /// <summary>
        /// Проверка свойства на Nullable и хранится ли в нём значение null
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsNullOrEmpty(PropertyInfo propertyInfo, T item)
        {
            return !(propertyInfo.PropertyType.ToString().Contains("Nullable") && propertyInfo.GetValue(item) is null);
        }

        /// <summary>
        /// Принимает тип C# и возвращает его аналогию в DbType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private DbType GetDbType(string type)
        {
            switch (type)
            {
                case "System.String": return DbType.String;
                case "System.Guid": return DbType.Guid;
                case "System.DateTime": return DbType.Date;
                case "System.Boolean": return DbType.Boolean;
                case "System.Int64": return DbType.Int64;
                case "System.Byte": return DbType.Byte;
                case "System.Byte[]": return DbType.Binary;
                case "System.DateTimeOffset": return DbType.DateTimeOffset;
                case "System.Decimal": return DbType.Decimal;
                case "System.Double": return DbType.Double;
                case "System.Int32": return DbType.Int32;
                case "System.Int16": return DbType.Int16;
                default: return DbType.String;
            }
        }

        public void Dispose()
        {
            connection.Close();
        }
    }
}