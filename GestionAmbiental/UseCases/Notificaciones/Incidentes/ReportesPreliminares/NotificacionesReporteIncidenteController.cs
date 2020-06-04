using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.Notificaciones.Incidentes.GetUsuariosNotificar;
using GestionAmbiental.Application.UseCases.Notificaciones.Incidentes.NuevoReportePreliminar.GetReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.Incidentes.DescargarReportePreliminar;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Notificaciones.Incidentes.ReportesPreliminares
{
    public class NotificacionesReporteIncidenteController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IGetReportePreliminar _getReportePreliminar;
        private readonly IGetUsuariosNotificarIncidente _getUsuariosNotificarIncidente;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IDataProtector _protector;
        private readonly DescargarReportePreliminarController _reportePreliminarController;


        public NotificacionesReporteIncidenteController(
            IHostingEnvironment hostingEnvironment,
            IGetReportePreliminar getReportePreliminar, 
            IGetUsuariosNotificarIncidente getUsuariosNotificarIncidente,
            IGetInfo_SGA getInfo_SGA, 
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings, 
            DescargarReportePreliminarController reportePreliminarController
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _getReportePreliminar = getReportePreliminar;
            _getUsuariosNotificarIncidente = getUsuariosNotificarIncidente;
            _info_SGA = getInfo_SGA;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.ReportePreliminarIdRouteValue);
            _reportePreliminarController = reportePreliminarController;
        }

        public async Task<IActionResult> NotificarNuevoReporte(int IdReporte)
        {
            var info = await _getReportePreliminar.Execute(IdReporte);
            var IdEncriptado = _protector.Protect(info.Id_Reporte_Preliminar.ToString());
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Incidentes","ReportePreliminar", "NuevaReportePreliminar.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var msgBody = string.Format(templateReaded,
                IdEncriptado,
                info.Codigo,
                info.Fecha_Incidente.Value.ToString("dd/MM/yyyy HH:mm"),
                info.Tipo_Incidente,
                info.Nivel_Incidente,
                info.Descripcion_Incidente,
                info.Gerencia_Responsable,
                info.Supervisor_MA
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
            
            msg.To.Add(new MailAddress(info.Email_Supervisor_MA));
            msg.Subject = "Nuevo Reporte Preliminar de Incidente";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;
            
            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("Resumen", "ReportePreliminar");
        }
        public async Task<IActionResult> DifundirNuevoReporte(int IdReporte)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var info = await _getReportePreliminar.Execute(IdReporte);
            var usuarios = await _getUsuariosNotificarIncidente.Execute();
            var IdEncriptado = _protector.Protect(info.Id_Reporte_Preliminar.ToString());
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Incidentes", "ReportePreliminar", "NuevaReportePreliminarMA.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var msgBody = string.Format(templateReaded,
                IdEncriptado,
                info.Codigo,
                info.Fecha_Incidente.Value.ToString("dd/MM/yyyy HH:mm"),
                info.Tipo_Incidente,
                info.Nivel_Incidente,
                info.Descripcion_Incidente,
                info.Gerencia_Responsable,
                info.Supervisor_MA
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
            foreach (var item in usuarios)
                msg.To.Add(new MailAddress(item.Email));
            msg.Subject = "Nuevo Reporte Preliminar de Incidente";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;
            
            var archivoReporte = await _reportePreliminarController.PrepararReportePreliminar(IdReporte, userInfo.NombreUsuario);
            msg.Attachments.Add(new Attachment(archivoReporte, "ReportePreliminarIncidente-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx"));

            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("Resumen", "ReportePreliminarMA");
        }
    }
}