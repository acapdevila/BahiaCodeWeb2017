using System.Net.Mail;
using Bh.Web.ViewModels;

namespace Bh.Web.Services
{
   public class EmailService
   {
       public void SendContactMessage(ContactViewModel model)
       {
           var oMsg = new MailMessage
               {
                   From = new MailAddress("info@buclenet.com", model.Name),
                   Subject =  "e-mail from Bahía Code Website: <" + model.Email + ">",
                   Body = model.Message + "\r\n\r\nNom: " + model.Name + "\r\nE-mail: " + model.Email + "\r\nTelèfon: " + model.Phone
               };

           oMsg.To.Add("laura@bahiacode.com");
           oMsg.ReplyToList.Add(model.Email);
          
           var smtp = new SmtpClient();

           smtp.Send(oMsg);

       }

    
    }
}
