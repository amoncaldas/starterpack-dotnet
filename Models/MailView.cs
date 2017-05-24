using System.Collections.Generic;
using StarterPack.Core.Mail;

namespace StarterPack.Models
{
    public class MailView
    {
        public string Message { get; set; }

        public string Subject { get; set; }

        public List<MailAddress> Users {get;set;}
    }
}
