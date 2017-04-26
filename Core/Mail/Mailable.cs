using System.Collections.Generic;

namespace StarterPack.Core.Mail
{
    public abstract class Mailable
    {       
        public abstract SPMail Build();

        public void Send(MailAddress to = null){            
            if(to != null) {
                List<MailAddress> tos = new List<MailAddress>(){to};
                Send(to);
            }
            else {
                Send();
            }           
        }
        
		public async void SendAsync(List<MailAddress> to)
		{
			SPMail sPMail = Build();
			if (to != null)
			{
				sPMail.To = to;
			}
			await MailSender.SendEmailAsync(sPMail);
		}

	}
}