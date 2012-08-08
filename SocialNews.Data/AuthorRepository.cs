using Db4objects.Db4o;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Query;
using SocialNews.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace SocialNews.Data
{
    public class AuthorRepository : BaseRepository<Author>
    {
        static string sharedSecret = "social!@news";
        public bool ValidateUser(string userName, string password)
        {
            var authors = this.GetAllFilteredBy(x => x.Login.Equals(userName) && x.Password.Equals(password));
            return (authors.Count() == 1);
        }
    }
}
