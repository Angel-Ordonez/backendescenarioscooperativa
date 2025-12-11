using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cooperativa.App.Engine
{
    public class NotificacionesEngine
    {

        public static void CrearCorreo(List<string> correo, string Body, string Subject)
        {
            MailMessage email = new MailMessage();
            correo.ForEach(s =>
            {
                email.To.Add(s);
            });
            email.From = new MailAddress("angelvesta2022@gmail.com");
            email.Subject = Subject;
            AlternateView plainView = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
            string html = Body;
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, "text/html");

            email.AlternateViews.Add(plainView);
            email.AlternateViews.Add(htmlView);
            email.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("angelvesta2022@gmail.com", "mrey cffg iucx gwya");
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Send(email);
        }
        
        
        
        
        
        
        public static void CrearCorreo2(List<string> correo, string Body, string Subject)
        {

            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
            StringBuilder bodytmp = new StringBuilder();
            MailMessage mailmsg = new MailMessage();
            mailmsg.From = new MailAddress("angelvesta2022@gmail.com", "Project Vesta");
            //mailmsg.To.Add(to);
            correo.ForEach(s =>
            {
                mailmsg.To.Add(s);
            });

            mailmsg.Subject = Subject;
            //mailmsg.Attachments.Add(new Attachment(@"J:\Files1\TrackingTerrestre\HND\ValesImpresosUltimaMilla\" + fileName + extencion));



            bodytmp.Append(Body);


            AlternateView avHTML = AlternateView.CreateAlternateViewFromString(bodytmp.ToString(), null, MediaTypeNames.Text.Html);

            //LinkedResource yourPictureRes = new LinkedResource(@"c:\inetpub\wwwroot\TrackingTerrestre\Images\LogoVesta.jpg", MediaTypeNames.Image.Jpeg);
            //LinkedResource yourPictureRes = new LinkedResource(@"J:\Files1\TrackingTerrestre\HND\ValesImpresosUltimaMilla\LogoVesta.jpg", MediaTypeNames.Image.Jpeg);
            //yourPictureRes.ContentId = mailFrom;
            //avHTML.LinkedResources.Add(yourPictureRes);

            mailmsg.AlternateViews.Add(avHTML);

            mailmsg.IsBodyHtml = true;



            var smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("angelvesta2022@gmail.com", "20161002109");
            smtpClient.Send(mailmsg);

        }
       
        
        
        
        
        
        
        
        public static string ConvertirStringBase64(string base64EncodedData)
        {
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(base64EncodedData);
            var strModified = Convert.ToBase64String(byt);
            return strModified;
        }










    }
}
