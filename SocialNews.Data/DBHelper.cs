using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocialNews.Domain.Model;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernate;

namespace SocialNews.Data
{
    public class DBHelper
    {
        public static void Generate()
        {
            // create our NHibernate session factory
            var sessionFactory = CreateSessionFactory();

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    var commonPassword = "123456";
                    var leonard = new Author { Login = "leonard", Password = commonPassword, Name = "Leonard Hofstadter" };
                    var sheldon = new Author { Login = "sheldon", Password = commonPassword, Name = "Sheldon Cooper" };
                    var raj = new Author { Login = "raj", Password = commonPassword, Name = "Rajesh Koothrappali" };
                    var howard = new Author { Login = "howard", Password = commonPassword, Name = "Howard Wollowitz" };
                    var penny = new Author { Login = "penny", Password = commonPassword, Name = "Penny" };

                    AddAuthors(session, leonard, sheldon, raj, howard, penny);

                    //AddMessageToAuthor();
                    var author = raj;

                    var time = DateTime.Now.Add(new TimeSpan(-40, 0, 0));
                    var msg = AddMessage(author, "raj (Entering dressed as Thor): Hey. Sorry I’m late, but my hammer got stuck in the door on the bus.", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(leonard, msg, "You went with Thor?", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(raj, msg, "What? Just because I’m Indian I can’t be a Norse God? No, no, no, Raj has to be an Indian God. That’s racism. I mean, look at Wolowitz, he’s not English, but he’s dressed like Peter Pan. Sheldon(entering in a body suit featuring black and white vertical lines) is neither sound nor light, but he’s obviously the Doppler Effect.", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(howard, msg, "I’m not Peter Pan, I’m Robin Hood.", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(raj, msg, "Really, because I saw Peter Pan, and you’re dressed exactly like Cathy Rigby. She was a little bigger than you, but it’s basically the same look, man.", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(leonard, msg, "Hey, Sheldon, there’s something I want to talk to you about before we go to the party.", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(sheldon, msg, "I don’t care if anybody gets it, I’m going as the Doppler Effect.", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(leonard, msg, "No, it’s not…", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(sheldon, msg, "If I have to, I can demonstrate. Neeeeoooowwwww!", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(leonard, msg, "Terrific. Um, this party is my first chance for Penny to see me in the context of her social group, and I need you not to embarrass me tonight.", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(sheldon, msg, "Well, what exactly do you mean by embarrass you?", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(leonard, msg, "For example, tonight no-one needs to know that my middle name is Leakey.", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(sheldon, msg, "Well, there’s nothing embarrassing about that, your father worked with Lewis Leakey, a great anthropologist. It had nothing to do with your bed-wetting.", time);
                    time = time.Add(new TimeSpan(0, 149, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(leonard, msg, "All I’m saying is that this party is the perfect opportunity for Penny to see me as a member of her peer group. A potential close friend and… perhaps more. I don’t want to look like a dork.", time);
                    session.SaveOrUpdate(msg);

                    time = DateTime.Now.Add(new TimeSpan(-1, 0, 0));
                    msg = AddMessage(author, "Water Demon.", time);
                    time = time.Add(new TimeSpan(0, 2, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(howard, msg, "Ice Dragon.", time);
                    time = time.Add(new TimeSpan(0, 3, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(leonard, msg, "Lesser Warlord of Ka’a.", time);
                    time = time.Add(new TimeSpan(0, 1, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(sheldon, msg, "Not so fast. Infinite Sheldon.", time);
                    time = time.Add(new TimeSpan(0, 7, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(leonard, msg, "Infinite Sheldon?", time);
                    //time = time.Add(new TimeSpan(0, 2, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(sheldon, msg, "Yes, Infinite Sheldon defeats all other cards and does not violate the rule against homemade cards because I made it at work.", time);
                    //time = time.Add(new TimeSpan(0, 2, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(leonard, msg, "Do you understand why people don’t want to play with you?", time);
                    //time = time.Add(new TimeSpan(0, 1, 0));
                    session.SaveOrUpdate(msg);
                    AddMessage(sheldon, msg, "No, although it’s a question I’ve been pondering since preschool.", time);
                    session.SaveOrUpdate(msg);
                    transaction.Commit();
                }
            }
        }

        private static Message AddMessage(Author author, string text, DateTime createdOn)
        {
            var msg = author.AddMessage(new Message() { Text = text, CreatedOn = createdOn });
            return msg;
        }

        private static Message AddMessage(Author author, Message msg, string text, DateTime createdOn)
        {
            return msg.AddMessage(author, new Message() { Text = text, CreatedOn = createdOn });
        }

        private static void AddAuthors(ISession session, params Author[] authors)
        {
            foreach (var author in authors)
            {
                session.SaveOrUpdate(author);
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString"].ToString();

            return Fluently.Configure()
                .Database(MsSqlCeConfiguration.Standard
                    .ConnectionString(connectionString))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<SocialNews.Domain.Model.Author>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
                .Create(false, true);
        }

        public static FluentNHibernate.Automapping.AutoPersistenceModel CreateAutomappings { get; set; }
    }
}
