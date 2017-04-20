using System.Collections.Generic;
using System.IO;

namespace StarterPack.Core.Mail
{
    public class SPMail
    {
        public List<MailAddress> To;
        public List<MailAddress> From;

        public List<KeyValuePair<string, Stream>> Attachments;

        public string Subject;

        public string message;

        public object Body;

        public SPMail() {
            To = new List<MailAddress>();
            From = new List<MailAddress>();
            Attachments = new List<KeyValuePair<string, Stream>>();
        }
        
    }
}