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

        //public void SpreadMessage(string message)
        //{
        //    BroadCastMessage(message);
        //}

        //private void BroadCastMessage(string message)
        //{
        //    var clients = Hub.GetClients<SocialHub>();

        //    clients.newMessage(message);
        //    //clients.isAlive();
        //}

        //public void GetClients()
        //{
        //    System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    string sJSON = oSerializer.Serialize(Clients);

        //    var clients = Hub.GetClients<SocialHub>();
        //    clients.userList(sJSON);
        //}

    }
}