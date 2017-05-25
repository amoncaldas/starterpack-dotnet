using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace StarterPack.Core.Mail
{
    /// <summary>
    /// Classe que representa um email
    /// Contém todos os campos que podem ser utilizados no envio de um email.
    /// </summary>
    public class SPMail
    {
        public List<MailAddress> To;
        public List<MailAddress> From;

        public List<KeyValuePair<string, Stream>> Attachments;

        public string Subject;

        public bool Plain;

        public string Encoding;

        public string Body;

        public string Template;

        public dynamic TemplateData;

        public bool Rendered;

        public SPMail() {
            Encoding = "uft-8";
            Plain = false;
            To = new List<MailAddress>();
            From = new List<MailAddress>();
            Attachments = new List<KeyValuePair<string, Stream>>();
            TemplateData = new ExpandoObject();
            Rendered = false;
        }

    }
}
