
using System.Collections.Generic;
using StarterPack.Core;
using StarterPack.Core.Mail;
using StarterPack.Models;

namespace StarterPack.Mail
{
	public class ConfirmRegister : Mailable
	{

        private User _user;
        public ConfirmRegister(User user){
            _user = user;
        }
		public override SPMail Build()
		{ 
			SPMail mail = new SPMail(); 
            MailAddress to = new MailAddress(_user.Email, _user.Name);            
            mail.To = new List<MailAddress>(){to} ; 
            mail.Subject = Lang.Get("mail:registrationData");
            mail.TemplateData.plainPassword = _user.PlainPassword;
            mail.TemplateData.email = _user.Email;
            mail.TemplateData.baseUrl = Env.Config("APP_URL");
            mail.TemplateData.name = _user.Name;
            mail.TemplateData.appName = Env.Config("APP_NAME");
            mail.Template = "ConfirmRegister";
            return mail;
		}
	}
}