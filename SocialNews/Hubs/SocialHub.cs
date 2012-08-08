using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;
using SocialNews.Data;
using SocialNews.Models;
using SocialNews.Domain.Model;

namespace SocialNews.Hubs
{
    public class SocialHub : Hub
    {
        private readonly Chat _chat;

        public void SendLikeToServer(int messageId)
        {
            var messageRepository = new MessageRepository();
            messageRepository.AddLike(messageId, Context.User.Identity.Name, (author) =>
                {
                    Clients.updateLike(messageId, new {Id = author.Id, Name = author.Name});
                });
        }

        public void SendUnlikeToServer(int messageId)
        {
            var messageRepository = new MessageRepository();
            messageRepository.Unlike(messageId, Context.User.Identity.Name, (author) =>
            {
                Clients.updateUnlike(messageId, new { Id = author.Id, Name = author.Name });
            });
        }

        public void SendCommentToServer(int? parentMessageId, string comment)
        {
            string userName = Context.User.Identity.Name;
            var authorRepository = new AuthorRepository();
            var author = authorRepository.GetAllFilteredBy(x => x.Login.Equals(userName, StringComparison.InvariantCultureIgnoreCase)).Single();
            var messageRepository = new MessageRepository();
            Message newMessage = messageRepository.AddMessage(parentMessageId, comment, author, messageRepository);
            Clients.addComment(parentMessageId, newMessage.Id, comment, new { Id = author.Id, Name = author.Name, SmallPicturePath = author.SmallPicturePath, MediumPicturePath = author.MediumPicturePath });
        }

        public void Join(string name)
        {
            Chat.Clients.Add(new Client() { Name = name, LastResponse = DateTime.Now });
            Caller.Name = name;
        }
    }
}