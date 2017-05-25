using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarterPack.Core.Mail
{
    public abstract class Mailable
    {
        public abstract SPMail Build();

        /// <summary>
        /// Envia um email de forma síncrona para um destinatário.
        /// </summary>
        /// <param name="to">destinatário</param>
        public void Send(MailAddress to){
            SendMail(new List<MailAddress>() { to });
        }

        /// <summary>
        /// Envia um email de forma síncrona para vários destinatário(s).
        /// </summary>
        /// <param name="to">destinatário(s)</param>
        public void Send(List<MailAddress> tos){
            SendMail(tos);
        }

        /// <summary>
        /// Envia o email de forma síncrona.
        /// Neste caso na construção do email deve já ter adicionado o destinatário.
        /// </summary>
        public void Send(){
            SendMail(null);
        }

        /// <summary>
        /// Envia um email de forma assíncrona para um destinatário.
        /// </summary>
        /// <param name="to">destinatário</param>
		public Task SendAsync(MailAddress to){
            return SendMailAsync(new List<MailAddress>() { to });
        }

        /// <summary>
        /// /// Envia um email de forma assíncrona para vários destinatários.
        /// </summary>
        /// <param name="to">destinatários</param>
		public Task SendAsync(List<MailAddress> tos){
            return SendMailAsync(tos);
        }

        /// <summary>
        /// Envia o email de forma assíncrona.
        /// Neste caso na construção do email deve já ter adicionado o destinatário.
        /// </summary>
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
