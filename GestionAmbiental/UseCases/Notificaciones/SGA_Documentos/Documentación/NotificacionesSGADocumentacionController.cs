using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.SGA_Documentos.ConsultarDocumento;
using GestionAmbiental.Application.UseCases.GestorUsuarios.GetUsuario;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.SGA_Documentos;
using System.Net.Mail;
using System.Net;
using System.Text;
using GestionAmbiental.Domain.Enum;
using System.IO;

namespace GestionAmbiental.UseCases.Notificaciones.SGA_Documentos.Documentación
{
    public class NotificacionesSGADocumentacionController : Controller
    {
        private readonly IConsultarDocumento _consultarDocumento;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IGetUsuario _getUsuario;
        private readonly IDataProtector _protector;
        private IHostingEnvironment _hostingEnvironment;
        public NotificacionesSGADocumentacionController(IConsultarDocumento consultarDocumento,
             IGetInfo_SGA info_SGA,
             IGetUsuario getUsuario,
            IHostingEnvironment hostingEnvironment,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _consultarDocumento = consultarDocumento;
            _info_SGA = info_SGA;
            _getUsuario = getUsuario;
            _hostingEnvironment = hostingEnvironment;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.DocumentosSGAIdRouteValue);
        }

        public async Task<IActionResult> NotificarRevisionAprobada(int IdSGADocumento, string UsuarioNombre, string Comentario)
        {
            var documento = await _consultarDocumento.Execute(IdSGADocumento);
            //----------------------------------------------------------------------------------------------------------------->Validar si no hay mas revisiones
            if (!documento.Estado.Equals(EstadosDocumentosSGA.Firmas.ToString()))
            {
                //----------------------------------------------------------------------------------------------------------------->Notifica Aprobado
                //Buscar documento            
                //Obtener correo del usuario a notificar
                string correo = documento.UsuarioEmail;
                //Editar plantilla del correo
                var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "SGA_Documentos", "RevisionAprobada.html");

                StreamReader SourceReader = System.IO.File.OpenText(template);
                var templateReaded = SourceReader.ReadToEnd();

                var msgBody = string.Format(templateReaded,
                   _protector.Protect(documento.IdSGADocumento.ToString()),
                   documento.DocCodigo,
                   documento.DocNombre,
                   documento.DocVersion.ToString(),
                   documento.Comentario,
                   documento.FechaCreacion.ToString("dd/MM/yyyy"),
                   UsuarioNombre,
                   Comentario
                   );

                var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
                string smtp = "", port = "", user = "", pass = "", mailFrom = "";
                foreach (var item in credenciales.Info)
                {
                    switch (item.Key)
                    {
                        case "smtp": smtp = item.Value; break;
                        case "ntwkuser": user = item.Value; break;
                        case "ntwkpassword": pass = item.Value; break;
                        case "port": port = item.Value; break;
                        case "mailfrom": mailFrom = item.Value; break;
                    }
                }
                //Enviar correo
                SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
                MailMessage msg = new MailMessage();
                msg.Priority = MailPriority.Normal;
                mail.EnableSsl = true;
                mail.UseDefaultCredentials = false;
                NetworkCredential credential = new NetworkCredential(user, pass);
                mail.Credentials = credential;
                msg.Subject = "Documento Aprobado";
                msg.SubjectEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
                msg.BodyEncoding = Encoding.UTF8;
                msg.Body = msgBody;

                msg.To.Add(new MailAddress(correo));

                await mail.SendMailAsync(msg);
                mail.Dispose();
                msg.Dispose();
                //----------------------------------------------------------------------------------------------------------------->Notifica Nuevo Activo

                //Buscar revision activa
                //Obtener correo del usuario a notificar
                string fechaVencimiento = "";
                string usuarioResponsable = "";
                var revisionActiva = documento.Revisores.FirstOrDefault(x => x.Estado.Equals(EstadosRevisionDocSGA.Activo.ToString()));
                if (revisionActiva == null)
                {
                    var aprobacionActiva = documento.Aprobadores.FirstOrDefault(x => x.Estado.Equals(EstadosRevisionDocSGA.Activo.ToString()));
                    //Si no fue la ultima revision, enviar correo de revision pendiente
                    correo = aprobacionActiva.Usuario.Email;
                    fechaVencimiento = aprobacionActiva.FechaLimite.Value.ToString("dd/MM/yyyy");
                    usuarioResponsable = aprobacionActiva.Usuario.Nombres + " " + aprobacionActiva.Usuario.Apellidos;
                }
                else
                {
                    correo = revisionActiva.Usuario.Email;
                    fechaVencimiento = revisionActiva.FechaLimite.Value.ToString("dd/MM/yyyy");
                    usuarioResponsable = revisionActiva.Usuario.Nombres + " " + revisionActiva.Usuario.Apellidos;
                }
                //Editar plantilla del correo
                template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "SGA_Documentos", "NuevaRevision.html");

                SourceReader = System.IO.File.OpenText(template);
                templateReaded = SourceReader.ReadToEnd();

                msgBody = string.Format(templateReaded,
                    _protector.Protect(documento.IdSGADocumento.ToString()),
                    documento.DocCodigo,
                    documento.DocNombre,
                    documento.DocVersion.ToString(),
                    documento.Comentario,
                    documento.FechaCreacion.ToString("dd/MM/yyyy"),
                    usuarioResponsable,
                    fechaVencimiento
                    );

                //Enviar correo
                mail = new SmtpClient(smtp, Convert.ToInt32(port));
                msg = new MailMessage();
                msg.Priority = MailPriority.Normal;
                mail.EnableSsl = true;
                mail.UseDefaultCredentials = false;
                credential = new NetworkCredential(user, pass);
                mail.Credentials = credential;
                msg.Subject = "Nueva Revisión de Documento";
                msg.SubjectEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
                msg.BodyEncoding = Encoding.UTF8;
                msg.Body = msgBody;

                msg.To.Add(new MailAddress(correo));

                await mail.SendMailAsync(msg);
                mail.Dispose();
                msg.Dispose();
            }
            else
            {
                string correo = documento.UsuarioEmail;
                //Editar plantilla del correo
                var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "SGA_Documentos", "DocumentoAprobado.html");

                StreamReader SourceReader = System.IO.File.OpenText(template);
                var templateReaded = SourceReader.ReadToEnd();

                var msgBody = string.Format(templateReaded,
                   _protector.Protect(documento.IdSGADocumento.ToString()),
                   documento.IdSGACategoriaDocumento,
                   documento.DocCodigo,
                   documento.DocNombre,
                   documento.DocVersion.ToString(),
                   documento.Comentario,
                   documento.FechaCreacion.ToString("dd/MM/yyyy"),
                   UsuarioNombre,
                   Comentario
                   );

                var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
                string smtp = "", port = "", user = "", pass = "", mailFrom = "";
                foreach (var item in credenciales.Info)
                {
                    switch (item.Key)
                    {
                        case "smtp": smtp = item.Value; break;
                        case "ntwkuser": user = item.Value; break;
                        case "ntwkpassword": pass = item.Value; break;
                        case "port": port = item.Value; break;
                        case "mailfrom": mailFrom = item.Value; break;
                    }
                }
                //Enviar correo
                SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
                MailMessage msg = new MailMessage();
                msg.Priority = MailPriority.Normal;
                mail.EnableSsl = true;
                mail.UseDefaultCredentials = false;
                NetworkCredential credential = new NetworkCredential(user, pass);
                mail.Credentials = credential;
                msg.Subject = "Documento Aprobado";
                msg.SubjectEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
                msg.BodyEncoding = Encoding.UTF8;
                msg.Body = msgBody;

                msg.To.Add(new MailAddress(correo));

                await mail.SendMailAsync(msg);
                mail.Dispose();
                msg.Dispose();
            }
            return RedirectToAction("VerDocumento", "SGADocumentacion", new { IdDocumento = _protector.Protect(documento.IdSGADocumento.ToString()) });

        }
        public async Task<IActionResult> NotificarRevisionRechazada(int IdSGADocumento, string UsuarioNombre, string Comentario)
        {
            //Buscar documento            
            var documento = await _consultarDocumento.Execute(IdSGADocumento);
            //Obtener correo del usuario a notificar
            string correo = documento.UsuarioEmail;
            //Editar plantilla del correo
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "SGA_Documentos", "RevisionRechazada.html");

            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var msgBody = string.Format(templateReaded,
               _protector.Protect(documento.IdSGADocumento.ToString()),
               documento.DocCodigo,
               documento.DocNombre,
               documento.DocVersion.ToString(),
               documento.Comentario,
               documento.FechaCreacion.ToString("dd/MM/yyyy"),
               UsuarioNombre,
               Comentario
               );

            var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
            string smtp = "", port = "", user = "", pass = "", mailFrom = "";
            foreach (var item in credenciales.Info)
            {
                switch (item.Key)
                {
                    case "smtp": smtp = item.Value; break;
                    case "ntwkuser": user = item.Value; break;
                    case "ntwkpassword": pass = item.Value; break;
                    case "port": port = item.Value; break;
                    case "mailfrom": mailFrom = item.Value; break;
                }
            }
            //Enviar correo
            SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
            MailMessage msg = new MailMessage();
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = true;
            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;
            msg.Subject = "Documento Rechazado";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;

            msg.To.Add(new MailAddress(correo));

            await mail.SendMailAsync(msg);
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("VerDocumento", "SGADocumentacion", new { IdDocumento = _protector.Protect(documento.IdSGADocumento.ToString()) });
        }
    }
}
