using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace SocialNews.Domain.Model
{
    public class Author : Entity
    {
        public Author()
        {
            Messages = new List<Message>();
        }

        public virtual string Login { get; set; }
        [ScriptIgnore]
        public virtual string Password { get; set; }
        public virtual string Name { get; set; }
        [ScriptIgnore]
        public virtual IList<Message> Messages { get; set; }
        public virtual string MediumPicturePath
        {
            get { return string.Format("/Content/images/actor{0}_medium.gif", Id); }
        }
        public virtual string SmallPicturePath
        {
            get { return string.Format("/Content/images/actor{0}_small.gif", Id); }
        }

        public virtual Message AddMessage(Message message)
        {
            message.Author = this;
            message.NrOrder = Messages.Count() + 1;
            Messages.Add(message);
            return message;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Author;
            if (other == null)
                return false;
            else
                return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
