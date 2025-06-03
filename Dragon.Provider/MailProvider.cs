using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Net.Mail;

namespace Dragon.Provider
{
    [Table($"Config{nameof(EmailConfig)}")]
    public class EmailConfig
    {
        [Key] public int Id { get; set; }
        [Required] public string SenderName { get; set; }
        [Required] public string SenderEmail { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string HostUrl { get; set; }
        [Required] public int Port { get; set; }
        [Required] public bool EnableSsl { get; set; } = false;
        [Required] public bool IsDefault { get; set; } = true;

        public string NetKey { get; set; }
        public string NetPassword { get; set; }

        [NotMapped] public bool IsBodyHtml { get; set; } = true;
        [NotMapped] public string Subject { get; set; }
        [NotMapped] public string ReceiverEmail { get; set; }
        [NotMapped] public string MailBody { get; set; }
    }

    public class MailProvider
    {
        private static SmtpClient GetSmtpClient(EmailConfig mailConfig)
        {
            return new SmtpClient(mailConfig.HostUrl, mailConfig.Port)
            {
                EnableSsl = mailConfig.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(string.IsNullOrWhiteSpace(mailConfig.NetKey) ? mailConfig.SenderEmail : mailConfig.NetKey, string.IsNullOrWhiteSpace(mailConfig.NetPassword) ? mailConfig.Password : mailConfig.NetPassword)
            };
        }
        public static async Task<bool> SendMail(EmailConfig mailConfig)
        {
            MailMessage mailMessage = new() { From = new MailAddress(mailConfig.SenderEmail, mailConfig.SenderName), Subject = mailConfig.Subject, IsBodyHtml = mailConfig.IsBodyHtml };

            mailMessage.To.Add(new MailAddress(mailConfig.ReceiverEmail));
            mailMessage.Body = mailConfig.MailBody;
            SmtpClient client = GetSmtpClient(mailConfig);
            await client.SendMailAsync(mailMessage);

            return true;
        }
    }
}
