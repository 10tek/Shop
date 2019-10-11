using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Shop.DataAccess.Abstract;

namespace Shop.DataAccess
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private Type type = typeof(T);

        public void Add(T item)
        {

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"insert into {type.Name} values('{item.Id}', '{item.CreationDate.ToString("yyyy-MM-dd HH:mm:ss.fff")}', '{item.DeletedDate}', ");
            foreach (var memberInfo in type.GetMembers())
            {
                if (memberInfo is PropertyInfo)
                {
                    stringBuilder.Append(memberInfo);
                }
            }
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
