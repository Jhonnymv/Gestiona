using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.GetFlashReport;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.Notificaciones.Incidentes.GetUsuariosNotificar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.Incidentes.DescargarFlashReport;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Notificaciones.Incidentes.FlashReport
{
    public class NotificacionesFlashReportController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IGetFlashReport _getFlashReport;
        private readonly IGetUsuariosNotificarIncidente _getUsuariosNotificarIncidente;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IDataProtector _protector;
        private readonly DescargarFlashReportController _descargarFlashReportController;

        public NotificacionesFlashReportController(
            IWebHostEnvironment hostingEnvironment, 
            IGetFlashReport getFlashReport, 
            IGetUsuariosNotificarIncidente getUsuariosNotificarIncidente, 
            IGetInfo_SGA info_SGA, 
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
            DescargarFlashReportController descargarFlashReportController)
        {
            _hostingEnvironment = hostingEnvironment;
            _getFlashReport = getFlashReport;
            _getUsuariosNotificarIncidente = getUsuariosNotificarIncidente;
            _info_SGA = info_SGA;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.FlashReportIdRouteValue);
            _descargarFlashReportController = descargarFlashReportController;
        }

        public async Task<IActionResult> NotificarNuevoReporte(int IdFlashReport)
        {
            var r = 76;
            var g = 193;
            var b = 220;
            var img_url = "/img/back-inspection.png";
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("Incidentes"));
                if (dataModulo != null)
                {
                    r = dataModulo.r;
                    g = dataModulo.g;
                    b = dataModulo.b;
                    img_url = dataModulo.img_url;
                }
            }
            catch (Exception e)
            {
                Log.Logger = new LoggerConfiguration().WriteTo.File(Path.Combine(_hostingEnvironment.ContentRootPath, "logs/exeption-error-log-" + DateTime.Today.ToString("yyyyMMdd") + ".log")).CreateLogger();
                Log.Error(e.Message);
            }
            var info = await _getFlashReport.Execute(IdFlashReport);
            var IdEncriptado = _protector.Protect(info.Id_Flash_Report.ToString());
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Incidentes", "FlashReport", "NuevaFlashReport.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();
            var msgBody = string.Format(templateReaded,
                $"#{r:X2}{g:X2}{b:X2}",
                info.Codigo,
                info.Fecha_Hora.ToString("dd/MM/yyyy HH:mm"),
                info.Empresa_Nombre,
                info.Gerencia_Nombre,
                info.Nivel_Incidente_Descripcion,
                info.Descripcion,
                info.Supervisor_Marcobre_Nombre,
                IdEncriptado
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
            SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
            MailMessage msg = new MailMessage();
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = false;

            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;

            msg.To.Add(new MailAddress(info.Supervisor_Marcobre_Email));
            msg.Subject = "Flash Report de Incidente";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;

            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("Resumen", "FlashReport");
        }
        public async Task<IActionResult> DifundirNuevoReporte(int IdFlashReport)
        {
            var r = 76;
            var g = 193;
            var b = 220;
            var img_url = "/img/back-inspection.png";
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("Incidentes"));
                if (dataModulo != null)
                {
                    r = dataModulo.r;
                    g = dataModulo.g;
                    b = dataModulo.b;
                    img_url = dataModulo.img_url;
                }
            }
            catch (Exception e)
            {
                Log.Logger = new LoggerConfiguration().WriteTo.File(Path.Combine(_hostingEnvironment.ContentRootPath, "logs/exeption-error-log-" + DateTime.Today.ToString("yyyyMMdd") + ".log")).CreateLogger();
                Log.Error(e.Message);
            }

            UserInfo userInfo;
            if(HttpContext.Session.GetString("SessionUser")!=null)
            {
                userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            }
            else
            {
                userInfo = new UserInfo
                {
                    NombreUsuario = "DescargasGestiona",
                    Rol = InspeccionRol.Admin
                };
            }


                var info = await _getFlashReport.Execute(IdFlashReport);
            var usuarios = await _getUsuariosNotificarIncidente.Execute();
            var IdEncriptado = _protector.Protect(info.Id_Flash_Report.ToString());
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Incidentes", "FlashReport", "NuevaFlashReportMA.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var msgBody = string.Format(templateReaded,
                $"#{r:X2}{g:X2}{b:X2}",
                info.Codigo,
                info.Fecha_Hora.ToString("dd/MM/yyyy HH:mm"),
                info.Empresa_Nombre,
                info.Gerencia_Nombre,
                info.Nivel_Incidente_Descripcion,
                info.Descripcion,
                info.Supervisor_Marcobre_Nombre,
                IdEncriptado
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
            SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
            MailMessage msg = new MailMessage();
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = false;
            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;

            msg.Subject = "Flash Report de Incidente";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;

            var archivoReporte = await _descargarFlashReportController.PrepararFlashReport(IdFlashReport, userInfo.NombreUsuario);
            msg.Attachments.Add(new Attachment(archivoReporte, "FlashReportIncidente-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx"));

            var correosUsuarios = new List<MailAddress>();
            for (int i = 0; i < usuarios.Count; i++)
            {
                msg.To.Add(new MailAddress(usuarios[i].Email));
                correosUsuarios.Add(new MailAddress(usuarios[i].Email));
                if (i > 0 && i % 20 == 0)
                {
                    await mail.SendMailAsync(msg);

                    foreach (var item in correosUsuarios)
                        msg.To.Remove(item);

                    correosUsuarios = new List<MailAddress>();

                    Thread.Sleep(60000);
                }
            }
            if (correosUsuarios.Count > 0)
                await mail.SendMailAsync(msg);

            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("Resumen", "FlashReportMA");
        }
    }
}
