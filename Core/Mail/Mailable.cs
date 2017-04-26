using System.Collections.Generic;

namespace StarterPack.Core.Mail
{
    public abstract class Mailable
    {       
        public abstract SPMail Build();

        public void Send(MailAddress to = null){       
            List<MailAddress> tos = null;

            if(to != null)
                tos = new List<MailAddress>() { to };

            SendAsync(tos);           
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