using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScrumApp.Data;
using ScrumApp.Models;

namespace ScrumApp.Controllers
{
    [Authorize]
    public class ChatMessageController : Controller
    {
        private readonly ScrumApplicationContext context;
        private readonly UserManager<AppUser> userManager;
        public ChatMessageController(ScrumApplicationContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public IActionResult GetNextChatMessages(string userSlug, string projectSlug, int? lastMessageId = null)
        {
            
            int messagePageSize = 5;

            AppUser user = context.Users.Where(x => x.UserNameSlug == userSlug).FirstOrDefault();

            Project project = context.Projects
                .Where(x => x.Author == user)
                .Where(y => y.Slug == projectSlug)
                .FirstOrDefault();


            if (lastMessageId != null)
            {
                IQueryable<ChatMessage> messagesToSkip = context.ChatMessages
                .Where(message => message.ProjectId == project.ProjectId)
                .Where(message => message.ChatMessageId >= lastMessageId);

                var chatMessages = context.ChatMessages
                .Where(message => message.ProjectId == project.ProjectId)
                .OrderByDescending(message => message.ChatMessageId)
                .Skip(messagesToSkip.Count())
                .Take(messagePageSize)
                .ToList();

                //int newLowestId = chatMessages[messagesToSkip.Count() - 1].ChatMessageId;

                List<Dictionary<string, string>> _list = new List<Dictionary<string, string>>();

                foreach (var m in chatMessages)
                {
                    AppUser u = context.Users.Find(m.AuthorId);

                    Dictionary<string, string> innerDict = new Dictionary<string, string>();
                    innerDict.Add("messageText", m.MessageText);
                    innerDict.Add("messageId", m.ChatMessageId.ToString());
                    innerDict.Add("AuthorName", u.UserName);
                    innerDict.Add("AuthorId", m.AuthorId);
                    innerDict.Add("userImage", u.ProfilePicture);

                    _list.Add(innerDict);
                }


                return Ok(_list);
            }

            else
            {
                var chatMessages = context.ChatMessages
                .Where(message => message.ProjectId == project.ProjectId)
                .OrderByDescending(message => message.ChatMessageId)
                .Take(messagePageSize)
                .ToList();

                //int newLowestId = chatMessages[messagePageSize - 1].ChatMessageId;

                List<Dictionary<string, string>> _list = new List<Dictionary<string, string>>();

                foreach (var m in chatMessages)
                {
                    AppUser u = context.Users.Find(m.AuthorId);

                    Dictionary<string, string> innerDict = new Dictionary<string, string>();
                    innerDict.Add("messageText", m.MessageText);
                    innerDict.Add("messageId", m.ChatMessageId.ToString());
                    innerDict.Add("AuthorName", u.UserName);
                    innerDict.Add("AuthorId", m.AuthorId);
                    innerDict.Add("userImage", u.ProfilePicture);

                    _list.Add(innerDict);
                }

                return Ok(_list);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ChatMessage messageTemplate)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            ChatMessage chatMessage = new ChatMessage
            {

                
                MessageText = messageTemplate.MessageText,
                Project = context.Projects.Find(messageTemplate.ProjectId),
                Author = user,
                AuthorId = user.Id,
                TimeSent = DateTime.Now

            };

            await context.ChatMessages.AddAsync(chatMessage);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}