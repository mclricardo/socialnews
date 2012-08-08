using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace SocialNews.Domain.Model
{
    public abstract class Entity
    {
        public Entity()
        {
            CreatedOn = DateTime.Now;
        }

        public virtual int Id { get; set; }
        public virtual DateTime CreatedOn { get; set; }
    }
}
