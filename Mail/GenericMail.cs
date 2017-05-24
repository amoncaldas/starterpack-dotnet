
using System.Collections.Generic;
using StarterPack.Core;
using StarterPack.Core.Mail;
using StarterPack.Models;

namespace StarterPack.Mail
{
	public class GenericMail : Mailable
	{
        private MailView _mail;

        public GenericMail(MailView mail)
        {
            _mail = mail;
        }

		public override SPMail Build()
		{
			SPMail mail = new SPMail();

            mail.To = _mail.Users;
            mail.Subject = _mail.Subject;
            mail.TemplateData.message = _mail.Message;
            mail.TemplateData.baseUrl = Env.Config("APP_URL");
            mail.TemplateData.appName = Env.Config("APP_NAME");
            mail.Template = "GenericMail";

            return mail;
		}
	}
}
