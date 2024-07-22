using EntityLayer.Concrete;
using MailKit.Net.Smtp;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MimeKit;

namespace LoginUI.Provider
{
    public class SendMail
    {
        public static void Send(string appCode,string receiverName,string receiverMail,string subject, BodyBuilder builder)
        {
            MimeMessage msg = new MimeMessage();
            MailboxAddress fromAdd = new MailboxAddress("Naile Sucu", "demosend19@gmail.com");
            MailboxAddress toAdd = new MailboxAddress(receiverName,receiverMail);
            msg.From.Add(fromAdd);
            msg.To.Add(toAdd);

            msg.Body = builder.ToMessageBody();
            msg.Subject = subject;
            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);

            client.Authenticate("demosend19@gmail.com", appCode);
            client.Send(msg);
            client.Disconnect(true);
        }
    }
}
