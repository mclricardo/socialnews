using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace SocialNews.Domain.Model
{
    public class Message :  Entity
    {
        public Message()
        {
            Messages = new List<Message>();
            Likes = new List<Author>();
        }

        public virtual int NrOrder  { get; set; }
        public virtual Author Author { get; set; }
        [ScriptIgnore]
        public virtual Message ParentMessage { get; set; }
        public virtual string Text { get; set; }
        public virtual IList<Message> Messages { get; set; }
        public virtual IList<Author> Likes { get; set; }

        public virtual Message AddMessage(Message message)
        {
            message.ParentMessage = this;
            Messages.Add(message);
            return message;
        }

        public virtual Message AddMessage(Author author, Message message)
        {
            message.Author = author;
            message.ParentMessage = this;
            message.NrOrder = Messages.Count() + 1;
            Messages.Add(message);

            author.Messages.Add(message);
            return message;
        }

        public virtual Author AddLike(Author author)
        {
            Likes.Add(author);
            return author;
        }
    }
}
