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

namespace Airport.DataAccess
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
        /// <param name="item"></param>
        public void Add(T item)
        {
            using (DbCommand dbCommand = connection.CreateCommand())
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"INSERT into {type.Name} (");
                DbParameter parameter;

                foreach (var propertyInfo in type.GetProperties())
                {
                    if (IsNullOrEmpty(propertyInfo, item))
                    {
                        stringBuilder.Append($"[{propertyInfo.Name}], ");
                    }
                }
                stringBuilder.Remove(stringBuilder.Length - 2, 1);
                stringBuilder.Append(") Values (");

                foreach (var propertyInfo in type.GetProperties())
                {
                    if (IsNullOrEmpty(propertyInfo, item))
                    {
                        parameter = providerFactory.CreateParameter();

                        parameter.DbType = GetDbType(propertyInfo.PropertyType.ToString());
                        parameter.ParameterName = $"@{propertyInfo.Name}";
                        parameter.Value = propertyInfo.GetValue(item);

                        dbCommand.Parameters.Add(parameter);
                        stringBuilder.Append($"{parameter.ParameterName}, ");
                    }
                }
                stringBuilder.Remove(stringBuilder.Length - 2, 1);
                stringBuilder.Append(");");

                dbCommand.CommandText = stringBuilder.ToString();

                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        dbCommand.Transaction = transaction;
                        dbCommand.ExecuteNonQuery();
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
        }

        /// <summary>
        /// Принимает Guid элемента и удаляет его из бд
        /// </summary>
        /// <param name="elementId"></param>
        public void Delete(Guid elementId)
        {
            using (DbCommand dbCommand = connection.CreateCommand())
            {
                var query = $"update [{type.Name}] set DeletedDate = '{DateTime.Now.ToShortDateString()}' where Id = '{elementId}';";
                dbCommand.CommandText = query;
                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        dbCommand.Transaction = transaction;
                        dbCommand.ExecuteNonQuery();
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

        public ICollection<T> GetAll()
        {
            using (DbCommand dbCommand = connection.CreateCommand())
            {
                List<T> table = new List<T>();
                var name = typeof(T).Name.ToString();
                var query = $"Select * From {name};";
                dbCommand.CommandText = query;

                connection.Open();
                var dbDataReader = dbCommand.ExecuteReader();

                string json = $"[";
                while (dbDataReader.Read())
                {
                    json += "{";
                    for (int i = 0; i < dbDataReader.FieldCount; i++)
                    {
                        json += $@" {dbDataReader.GetName(i)} : {(dbDataReader.GetValue(i) is null ? "null" : $@"""{dbDataReader.GetValue(i)}""")}";
                        if (i != dbDataReader.FieldCount - 1)
                        {
                            json += ",";
                        }
                    }
                    json += "},";
                }
                json += "]";
                json = json.Replace(@"""""", "null");
                var format = "dd/MM/yyyy HH:mm:ss";
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
                table = JsonConvert.DeserializeObject<List<T>>(json, dateTimeConverter);
                return table;
            }
        }

        public void Update(T element)
        {
            using (DbCommand dbCommand = connection.CreateCommand())
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"Update [{type.Name}] Set ");
                var propCount = type.GetProperties().Length;
                var dbParameters = new List<DbParameter>();
                DbParameter parameter;
                foreach (var propertyInfo in type.GetProperties())
                {
                    if (IsNullOrEmpty(propertyInfo, element))
                    {
                        parameter = providerFactory.CreateParameter();

                        parameter.DbType = GetDbType(propertyInfo.PropertyType.ToString());
                        parameter.ParameterName = $"@{propertyInfo.Name}";
                        parameter.Value = propertyInfo.GetValue(element);
                        parameter.IsNullable = (propertyInfo.PropertyType.ToString().Contains("Nullable") ? true : false);

                        dbParameters.Add(parameter);
                    }
                }
                for (int i = 0; i < propCount; i++)
                {
                    stringBuilder.Append(dbParameters[i].Value is null ? "" : $"[{dbParameters[i].ParameterName.Substring(1)}] = {dbParameters[i].ParameterName}");
                    if (i != propCount - 1)
                    {
                        stringBuilder.Append(",");
                    }
                }
                stringBuilder.Append($" Where Id = {element.Id};");
                dbCommand.CommandText = stringBuilder.ToString();

                using (DbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        dbCommand.Transaction = transaction;
                        dbCommand.ExecuteNonQuery();
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