using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using StarterPack.Core.Mail;
using StarterPack.Core.Renders;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

// using https://github.com/jstedfast/MailKit

namespace StarterPack.Core
{
    public class MailSender
    {
        /// <summary>
        /// Envia um email utilizando como sender o email padrão do sistema configurado no arquivo config
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task SendEmailAsync(string toEmail, string subject, string message)
        {  
            MailAddress to = new MailAddress(toEmail, toEmail);
            await SendEmailAsync(to, subject, message);
        }

        /// <summary>
        /// Envia um email pondendo-se especificar o email do destinaŕio e o nome do mesmo, utilizando como sender o email padrão do sistema configurado no arquivo config
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="toName"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task SendEmailAsync(MailAddress to, string subject, string message)
        {
            string defaultMailFrom = Env.Config("MAIL_FROM");
            string defaultMailName = Env.Config("MAIL_NAME");  
            MailAddress defaultFrom = new MailAddress(defaultMailFrom, defaultMailName);           
            await SendEmailAsync(defaultFrom, to, subject, message);
        }

        /// <summary>
        /// Envia um email utilizando como sender um email informado
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task SendEmailAsync(MailAddress from, MailAddress to, string subject, string message)
        {
            List<MailAddress> toList = new List<MailAddress>(){to};                    
            await SendEmailAsync(toList, subject, message, from);
        }

        /// <summary>
        /// Envia um email utilizando um template razor sendo necessário passar os dados a serem populados no template
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="template"></param>
        /// <param name="ViewData"></param>
        /// <returns></returns>
        public static async Task SendEmailAsync(string toEmail, string subject, string template, ViewDataDictionary ViewData)
        {
            MailAddress to = new MailAddress(toEmail, toEmail);            
            await SendEmailAsync(to, subject, template, ViewData);
        }

        /// <summary>
        /// Envia um email utilizando um template razor sendo necessário passar os dados a serem populados no template
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="template"></param>
        /// <param name="ViewData"></param>
        /// <returns></returns>
        public static async Task SendEmailAsync(MailAddress to, string subject, string template, ViewDataDictionary ViewData)
        {
            string defaultMailFrom = Env.Config("MAIL_FROM");
            string defaultMailName = Env.Config("MAIL_NAME");  
            MailAddress defaultFrom = new MailAddress(defaultMailFrom, defaultMailName);
            await SendEmailAsync(defaultFrom, to,subject, template, ViewData);
        }
        
        /// <summary>
        /// Envia um email utilizando como sender um email informado recebendo um template razor os dados a serem populados no template
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="template"></param>
        /// <param name="ViewData"></param>
        /// <returns></returns>
        public static async Task SendEmailAsync(MailAddress from, MailAddress to, string subject, string template, ViewDataDictionary ViewData)
        {
            List<MailAddress> toList = new List<MailAddress>(){to};                      
            string message = await RazorRender.service.RenderToStringAsync(template, ViewData);
            await SendEmailAsync(toList, subject, message, from);
        }
       
        
        /// <summary>
        /// Envie um email para um ou mais destinatários, podendo ser especificado o email que envia a mensagem
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        public static async Task SendEmailAsync(List<MailAddress> toList, string subject, string message, MailAddress from = null, bool plain = false)
        {
            string defaultMailFrom = Env.Config("MAIL_FROM");
            string defaultMailName = Env.Config("MAIL_NAME");  
            
            if(from == null && defaultMailFrom == "DEFAULT_FROM_MAIL")  {
                 throw new System.Exception("You must either pass a from KeyValuePair or set the MAIL_FROM and the MAIL_NAME in config");
            }
           
            SPMail emailMessage =  new SPMail();
            emailMessage.Plain = plain;
            MailAddress defaultFrom = new MailAddress(defaultMailFrom, defaultMailName);
            if(from != null) {               
                emailMessage.From.Add(from);
            }
            else {
                emailMessage.From.Add(defaultFrom);
            }
            
            emailMessage.To.AddRange(toList);

            emailMessage.Subject = subject;           
            emailMessage.Body = message;          
            await SendEmailAsync(emailMessage);            
        }

         /// <summary>
        /// Envia um SPMail, que pode especificar destinários, sender, subject, body e anexos
        /// </summary>
        /// <param name="emailMessage"></param>
        /// <returns></returns>
        public static async Task SendEmailAsync(SPMail emailMessage)
        {
            string mailHost = Env.Config("MAIL_HOST");
            string mailDriver = Env.Config("MAIL_DRIVER");                      
            string mailPassword = Env.Config("MAIL_PASSWORD");
            string mailEncryption = Env.Config("MAIL_ENCRYPTION"); 

            // Here we convert our custom SPMail Object to a MailKit.MimeMessage
            // We do this to avoid a coupling with MailKit. If in the future we decide to replace MailKit 
            // no external class will be affected

            MimeMessage message = new MimeMessage();
            var builder = new BodyBuilder ();
           
            if(emailMessage.Plain) {
                builder.TextBody =  emailMessage.Body;
            }
            else {
                builder.HtmlBody = emailMessage.Body;
            }
            
            message.Subject = emailMessage.Subject; 

            emailMessage.To.ForEach(t => {
                message.To.Add(new MailboxAddress(t.Name, t.Email));
            });

            emailMessage.From.ForEach(f => {
                message.From.Add(new MailboxAddress(f.Name, f.Email));
            });      

            emailMessage.Attachments.ForEach(a => {
                builder.Attachments.Add(a.Key, a.Value);               
            });

            // End SPMail to MailKit.MimeMessage
            
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                int mailPort = 587;
                
                try{
                   mailPort = Int32.Parse(Env.Config("MAIL_PORT"));
                }
                catch(System.Exception){
                    throw new System.Exception("Invalid smtp mail port in config file");
                }

                client.LocalDomain = mailHost;
                await client.ConnectAsync(mailHost, mailPort, SecureSocketOptions.None).ConfigureAwait(false);
                // TODO: falta colocar aqui a possibilidade de envio autenticado, se definido use e pass no config
                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }       
    }
    
}