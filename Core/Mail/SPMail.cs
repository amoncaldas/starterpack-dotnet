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

        public bool Plain;

        public string Encoding;

        public string Body;

        public SPMail() {
            Encoding = "uft-8";
            Plain = false;
            To = new List<MailAddress>();
            From = new List<MailAddress>();
            Attachments = new List<KeyValuePair<string, Stream>>();
        }
        
    }
}