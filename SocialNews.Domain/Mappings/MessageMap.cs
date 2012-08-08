using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SocialNews.Domain.Model;
using FluentNHibernate.Mapping;

namespace SocialNews.Domain.Mappings
{
    public class MessageMap : BaseMap<Message>
    {
        public MessageMap()
        {
            Map(x => x.Text).Length(1000);
            Map(x => x.NrOrder);
            References(x => x.ParentMessage, "ParentMessage_id");
            References(x => x.Author, "Author_id");
            HasMany(x => x.Messages)
                .Cascade.All()
                .Inverse()
                .OrderBy("NrOrder");
        }
    }
}
