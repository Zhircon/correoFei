using correoFei.Services.ErrorLog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Net.Mail;
namespace correoFei.Services.Email
{
    public class Email : IEmail
    {
        private readonly IErrorLog _errorlog;
        private readonly IConfiguration Configuration;
        public Email(IErrorLog ErrorLog, IConfiguration configuration){
            _errorlog = ErrorLog;
            Configuration = configuration;
        }
        [HttpPost]
        public Task<bool> EnviaCorreoAsync(string tema,string para, string cc, string bcc,string cuerpo,Attachment adjunto = null){
            bool res = false;
            try{
            
            MailMessage eMail = new ();
            if(para!=null)
                eMail.To.Add(para);
            if(cc!=null)
                eMail.CC.Add(cc);
            if(bcc != null)
                eMail.Bcc.Add(bcc);
            eMail.From = new MailAddress(Configuration["Smtp:SmtpUser"]);
            if(string.IsNullOrEmpty(tema))
                tema= "[Sin titulo]";
            eMail.Subject = tema;
            eMail.Body = cuerpo;
            if(adjunto != null)
                eMail.Attachments.Add(adjunto);
            eMail.BodyEncoding = System.Text.Encoding.UTF8;
            eMail.SubjectEncoding = System.Text.Encoding.UTF8;
            eMail.HeadersEncoding = System.Text.Encoding.UTF8;
            eMail.IsBodyHtml = true;

            SmtpClient clienteSMTP = new(Configuration["Smtp:SmtpServer"]);
            clienteSMTP.Port = Convert.ToInt16(Configuration["Smtp:SmtpPort"]);
            clienteSMTP.EnableSsl = true;
            clienteSMTP.DeliveryMethod = SmtpDeliveryMethod.Network;
            clienteSMTP.UseDefaultCredentials = false;
            clienteSMTP.Credentials = new System.Net.NetworkCredential(Configuration["Smtp:SmtpUser"], Configuration["Smtp:SmtpPwd"]);
            _errorlog.ErrorLogAsync($"Correo para {para} con tema {tema}");
            clienteSMTP.SendAsync(eMail,null);
            res = true;
            }catch(SmtpException ex){
                _errorlog.ErrorLogAsync(ex.Message);
            }
            return Task.FromResult(res);
        }
    }
}