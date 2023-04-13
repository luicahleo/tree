using log4net;
using NCrontab;
using System;
using System.Reflection;
using OpenSmtp;
using OpenSmtp.Mail;
using System.IO;
using HtmlAgilityPack;

namespace TreeCore
{
    public static class Email
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string GetPlantillaEmailAddUser(string sNombre, string sEmail, string sPass)
        {
            string PlaEmail = "";
            
            try
            {                
                string directoryName = DirectoryMapping.GetTemplatesDirectory();
                directoryName = Path.Combine(directoryName, "MailBienvenida.html");

                var doc = new HtmlDocument();
                doc.Load(directoryName);
                doc.GetElementbyId("Nombre").InnerHtml = sNombre;
                doc.GetElementbyId("Correo").InnerHtml = sEmail;
                doc.GetElementbyId("Password").InnerHtml = sPass;
                //doc.GetElementbyId("Nombre").ChildNodes.Add(HtmlTextNode.CreateNode(sNombre));
                //doc.GetElementbyId("Correo").ChildNodes.Add(HtmlTextNode.CreateNode(sEmail));
                //doc.GetElementbyId("Password").ChildNodes.Add(HtmlTextNode.CreateNode(sPass));
                PlaEmail = doc.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                PlaEmail = "";
            }

            return PlaEmail;
        }

        public static string SendMail(string Para, string desdeNombre, string reply, string asunto, string cuerpo, string cuerpoHTML, string Adjunto)
        {

#if SERVICESETTINGS
            string Servidor = ServerConnectionChooser.getStoredString(System.Configuration.ConfigurationManager.AppSettings["SMTP_Servidor"]);
            string Usuario = ServerConnectionChooser.getStoredString(System.Configuration.ConfigurationManager.AppSettings["SMTP_Usuario"]);
            string Clave = ServerConnectionChooser.getStoredString(System.Configuration.ConfigurationManager.AppSettings["SMTP_Clave"]);
            string EmailDesde = ServerConnectionChooser.getStoredString(System.Configuration.ConfigurationManager.AppSettings["SMTP_Mail"]);
#elif TREEAPI
        string Servidor = ServerConnectionChooser.getStoredString(TreeAPI.Properties.Settings.Default.SMTP_Servidor);
        string Usuario = ServerConnectionChooser.getStoredString(TreeAPI.Properties.Settings.Default.SMTP_Usuario);
        string Clave = ServerConnectionChooser.getStoredString(TreeAPI.Properties.Settings.Default.SMTP_Clave);
        string EmailDesde = ServerConnectionChooser.getStoredString(TreeAPI.Properties.Settings.Default.SMTP_Mail);
#else
        string Servidor = ServerConnectionChooser.getStoredString(TreeCore.Properties.Settings.Default.SMTP_Servidor);
        string Usuario = ServerConnectionChooser.getStoredString(TreeCore.Properties.Settings.Default.SMTP_Usuario);
        string Clave = ServerConnectionChooser.getStoredString(TreeCore.Properties.Settings.Default.SMTP_Clave);
        string EmailDesde = ServerConnectionChooser.getStoredString(TreeCore.Properties.Settings.Default.SMTP_Mail);
#endif

            OpenSmtp.Mail.MailMessage msg = new OpenSmtp.Mail.MailMessage();
            msg.Subject = asunto;
            msg.Body = cuerpo;
            msg.HtmlBody = cuerpoHTML;
            msg.From = new EmailAddress(EmailDesde, desdeNombre);
            msg.ReplyTo = new EmailAddress(reply, reply);
            if (Para.Contains(";"))
            {
                string[] aux = null;
                aux = Para.Split(Convert.ToChar(";"));

                bool primera = true;
                foreach (string correo in aux)
                {
                    msg.AddRecipient(new EmailAddress(correo, correo), (primera ? AddressType.To : AddressType.Cc));
                    primera = false;
                }
            }
            else
            {
                msg.AddRecipient(new EmailAddress(Para, Para), AddressType.To);
            }

            if (Adjunto.Length > 0)
            {
                FileInfo info = new FileInfo(Adjunto);
                string nombre = info.Name;
                info = null;
                msg.AddAttachment(new Attachment(new FileStream(Adjunto, FileMode.Open, FileAccess.Read), nombre));
            }

            msg.Charset = "ISO-8859-1";
            msg.Priority = MailPriority.Normal;

            try
            {
                OpenSmtp.Mail.Smtp smtp = new OpenSmtp.Mail.Smtp(Servidor, Usuario, Clave);
                smtp.SendMail(msg);
                return "";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return ex.Message;
            }
        }

    }
}