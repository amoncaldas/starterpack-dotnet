using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarterPack.Core.Mail
{
    public abstract class Mailable
    {
        public abstract SPMail Build();

        public void Send(MailAddress to){
            SendMail(new List<MailAddress>() { to });
        }

		 public void Send(List<MailAddress> tos){
            SendMail(tos);
        }

		 public void Send(){
            SendMail(null);
        }

		public Task SendAsync(MailAddress to){
            return SendMailAsync(new List<MailAddress>() { to });
        }

		 public Task SendAsync(List<MailAddress> tos){
            return SendMailAsync(tos);
        }

		 public Task SendAsync(){
            return SendMailAsync(null);
        }

		private async void SendMail(List<MailAddress> tos)
		{
			await SendMailAsync(tos);
		}

		private Task SendMailAsync(List<MailAddress> to)
		{
			SPMail sPMail = Build();

			if (to != null)
			{
				sPMail.To = to;
			}
			return MailSender.SendEmailAsync(sPMail);
		}

	}
}
