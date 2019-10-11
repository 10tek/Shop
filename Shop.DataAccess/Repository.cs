using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Shop.DataAccess.Abstract;
using System.Data.SqlClient;
using System.Data;
//("yyyy-MM-dd HH:mm:ss.fff")
namespace Shop.DataAccess
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private Type type = typeof(T);
        public void Add(T item)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"insert into {type.Name} (");
            SqlParameter parameter;

            foreach (var propertyInfo in type.GetProperties())
            {
                stringBuilder.Append($"{propertyInfo.Name},");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(") ");
            foreach (var propertyInfo in type.GetProperties())
            {
                parameter = new SqlParameter
                {
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                    ParameterName = "@Id",
                    //Value = category.Id,
                };
                stringBuilder.Append($"'{propertyInfo.GetValue(item, null)}',");
            }
        }

        private SqlDbType GetSqlType(Type type)
        {
            return SqlDbType.Int;
        }

        public void Delete(T elementId)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(T element)
        {
            throw new NotImplementedException();
        }
    }
}
