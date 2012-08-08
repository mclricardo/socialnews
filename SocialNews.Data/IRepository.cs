using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocialNews.Domain.Model;
using SocialNews.Domain;

namespace SocialNews.Data
{
    public interface IRepository<T> where T : Entity
    {
        T Get(int id);
        IList<T> GetByExample(T example);
        IList<T> GetAll();
        void Add(T T);
        void Delete(int id);
        void DeleteAll();
    }
}
