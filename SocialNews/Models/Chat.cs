using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;
using SocialNews.Hubs;

namespace SocialNews.Models
{
    public class Chat
    {
        private readonly static Lazy<Chat> _instance = new Lazy<Chat>(() => new Chat());
        public static List<Client> Clients = new List<Client>();
        
        public static Chat Instance
        {
            get
            {
                return _instance.Value;
            }
        }
    }
}