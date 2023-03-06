using System.Net;
using System.Net.Mail;

namespace MoodReboot.Helpers
{
    public class HelperMail
    {
        private readonly IConfiguration configuration;

        public HelperMail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private MailMessage ConfigureMailMessage(string para, string asunto, string mensaje)
        {
            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");

            MailMessage mail = new()
            {
                From = new MailAddress(user),
                Body = mensaje,
                Subject = asunto,
                IsBodyHtml = true,
            };

            mail.To.Add(new MailAddress(para));

            return mail;
        }

        private MailMessage ConfigureMailMessage(string para, string asunto, string mensaje, string path)
        {
            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");

            MailMessage mail = new()
            {
                From = new MailAddress(user),
                Body = mensaje,
                Subject = asunto,
                IsBodyHtml = true,
            };

            mail.To.Add(new MailAddress(para));

            Attachment attachment = new(path);
            mail.Attachments.Add(attachment);

            return mail;
        }

        private MailMessage ConfigureMailMessage(string para, string asunto, string mensaje, List<string> paths)
        {
            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");

            MailMessage mail = new()
            {
                From = new MailAddress(user),
                Body = mensaje,
                Subject = asunto,
                IsBodyHtml = true,
            };

            mail.To.Add(new MailAddress(para));

            foreach (string path in paths)
            {
                Attachment attachment = new(path);
                mail.Attachments.Add(attachment);
            }

            return mail;
        }

        private SmtpClient CofigureSmtpClient()
        {
            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");
            string password = this.configuration.GetValue<string>("MailSettings:Credentials:Password");
            string hostName = this.configuration.GetValue<string>("MailSettings:Smtp:Host");
            int port = this.configuration.GetValue<int>("MailSettings:Smtp:Port");
            bool enableSSL = this.configuration.GetValue<bool>("MailSettings:Smtp:EnableSSL");
            bool defaultCredentials = this.configuration.GetValue<bool>("MailSettings:Smtp:DefaultCredentials");

            SmtpClient smtpClient = new()
            {
                Host = hostName,
                Port = port,
                EnableSsl = enableSSL,
                UseDefaultCredentials = defaultCredentials,
                Credentials = new NetworkCredential(user, password)
            };

            return smtpClient;
        }

        public Task SendMailAsync(string para, string asunto, string mensaje)
        {
            MailMessage mail = this.ConfigureMailMessage(para, asunto, mensaje);
            SmtpClient client = this.CofigureSmtpClient();
            return client.SendMailAsync(mail);
        }

        public Task SendMailAsync(string para, string asunto, string mensaje, string path)
        {
            MailMessage mail = this.ConfigureMailMessage(para, asunto, mensaje, path);
            SmtpClient client = this.CofigureSmtpClient();
            return client.SendMailAsync(mail);
        }

        public Task SendMailAsync(string para, string asunto, string mensaje, List<string> paths)
        {
            MailMessage mail = this.ConfigureMailMessage(para, asunto, mensaje, paths);
            SmtpClient client = this.CofigureSmtpClient();
            return client.SendMailAsync(mail);
        }
    }
}
