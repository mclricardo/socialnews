using SocialNews.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace SocialNews.Data
{
    public class MessageRepository : BaseRepository<Message>
    {
        public Message Get(int id)
        {
            var ret = this.GetAllFilteredBy(x => x.Id == id);
            if (ret.Any())
            {
                return ret.First();
            }
            else
            {
                return null;
            }
        }

        public Message AddMessage(int? parentMessageId, string comment, Author author, MessageRepository messageRepository)
        {
            Message newMessage = new Message() { Text = comment, CreatedOn = DateTime.Now };
            // create our NHibernate session factory
            var sessionFactory = CreateSessionFactory();
            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    if (parentMessageId.HasValue)
                    {
                        var parentMessage = messageRepository.Get(parentMessageId.Value);
                        parentMessage.AddMessage(author, newMessage);
                        messageRepository.Add(newMessage);
                    }
                    else
                    {
                        newMessage.Author = author;
                        messageRepository.Add(newMessage);
                    }
                    session.SaveOrUpdate(newMessage);
                    transaction.Commit();
                }
            }
            return newMessage;
        }

        public void AddLike(int messageId, string userName, Action<Author> callback)
        {
            var authorRepository = new AuthorRepository();
            var author = authorRepository.GetAllFilteredBy(x => x.Login.Equals(userName,
                StringComparison.InvariantCultureIgnoreCase)).Single();
            var message = this.Get(messageId);
            if (!message.Likes.Contains(author))
            {
                // create our NHibernate session factory
                var sessionFactory = CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    // populate the database
                    using (var transaction = session.BeginTransaction())
                    {
                        message.Likes.Add(author);
                        transaction.Commit();
                    }
                }
                if (callback != null)
                    callback(author);
            }
        }

        public void Unlike(int messageId, string userName, Action<Author> callback)
        {
            var authorRepository = new AuthorRepository();
            var author = authorRepository.GetAllFilteredBy(x => x.Login.Equals(userName,
                StringComparison.InvariantCultureIgnoreCase)).Single();
            var message = this.Get(messageId);
            if (message.Likes.Contains(author))
            {
                // create our NHibernate session factory
                var sessionFactory = CreateSessionFactory();
                using (var session = sessionFactory.OpenSession())
                {
                    // populate the database
                    using (var transaction = session.BeginTransaction())
                    {
                        message.Likes.Remove(author);
                        transaction.Commit();
                    }
                }
                if (callback != null)
                    callback(author);
            }
        }
    }
}
