using SocialNews.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SocialNews.Domain;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
namespace SocialNews.Data
{
    public abstract class BaseRepository<T> : IRepository<T> where T : Entity
    {
        static ISession session = null;

        public T Get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetByExample(T example)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll()
        {
            IList<T> items = null;

            CreateAndOpenSession();

            items = session.CreateCriteria(typeof(T))
                .List<T>();

            return items;
        }

        public IList<T> GetAllFilteredBy(Func<T, bool> filter)
        {
            IList<T> items = null;

            CreateAndOpenSession();

            items = session.CreateCriteria(typeof(T))
                .List<T>().Where(filter).ToList();

            return items;
        }

        public IList<T> GetAllFilteredByAndOrderedBy<TKey>(Func<T, bool> filter, Func<T, TKey> orderBy)
        {
            IList<T> items = null;

            CreateAndOpenSession();

            items = session.CreateCriteria(typeof(T))
                .List<T>().Where(filter).OrderBy(orderBy).ToList();

            return items;
        }

        public IList<T> GetAllOrderedBy<TKey>(Func<T, TKey> orderBy)
        {
            IList<T> items = null;

            CreateAndOpenSession();

            items = session.CreateCriteria(typeof(T))
                .List<T>().OrderBy(orderBy).ToList();

            return items;
        }

        public void Add(T t)
        {
        }

        public void Delete(int id)
        {
        }

        public void DeleteAll()
        {
        }

        private static void CreateAndOpenSession()
        {
            if (session == null)
            {
                var sessionFactory = CreateSessionFactory();
                session = sessionFactory.OpenSession();
            }
        }

        protected static ISessionFactory CreateSessionFactory()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();

            return Fluently.Configure()
                .Database(MsSqlCeConfiguration.Standard
                    .ConnectionString(connectionString))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<SocialNews.Domain.Model.Author>())
                .BuildSessionFactory();
        }
    }
}
