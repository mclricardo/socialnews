using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SocialNews.Domain.Model;

namespace SocialNews.Domain.Mappings
{
    public class AuthorMap : BaseMap<Author>
    {
        public AuthorMap()
        {
            Map(x => x.Name);
            Map(x => x.Login);
            Map(x => x.Password);
            HasMany(x => x.Messages)
                .Cascade.All()
                .Inverse();
        }
    }
}
