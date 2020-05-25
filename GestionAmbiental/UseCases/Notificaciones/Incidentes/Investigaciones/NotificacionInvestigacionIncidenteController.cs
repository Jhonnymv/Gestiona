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
using GestionAmbiental.Application.UseCases.Notificaciones.Incidentes.NuevaInvestigacion;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.Incidentes.DescargarInvestigacionFinal;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GestionAmbiental.UseCases.Notificaciones.Incidentes.Investigaciones
{
    public class NotificacionInvestigacionIncidenteController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        private readonly IGetInvestigacionFinal _getInvestigacionFinal;
        private readonly IGetUsuariosNotificarIncidente _getUsuariosNotificarIncidente;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IDataProtector _protector;
        private readonly DescargarInvestigacionFinalController _descargarInvestigacionFinalController;

        public NotificacionInvestigacionIncidenteController(
           IHostingEnvironment hostingEnvironment,
           IGetInvestigacionFinal getInvestigacionFinal,
           IGetUsuariosNotificarIncidente getUsuariosNotificarIncidente,
           IGetInfo_SGA getInfo_SGA,
           IDataProtectionProvider dataProtectionProvider,
           DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
           DescargarInvestigacionFinalController descargarInvestigacionFinalController
           )
        {
            _hostingEnvironment = hostingEnvironment;
            _getInvestigacionFinal = getInvestigacionFinal;
            _getUsuariosNotificarIncidente = getUsuariosNotificarIncidente;
            _info_SGA = getInfo_SGA;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.IncidenteIdRouteValue);
            _descargarInvestigacionFinalController = descargarInvestigacionFinalController;
        }
        public async Task<IActionResult> DifundirInvestigacion(int IdInvestigacion)
        {
            var info = await _getInvestigacionFinal.Execute(IdInvestigacion);
            var usuarios = await _getUsuariosNotificarIncidente.Execute();
            var IdEncriptado = _protector.Protect(info.Id_Reporte_Preliminar.ToString());
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Incidentes", "Investigacion", "NuevaInvestigacion.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var msgBody = string.Format(templateReaded,
                IdEncriptado,
                info.Codigo
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
            //mail.EnableSsl = false;
            mail.EnableSsl = true;

            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;

            foreach (var item in usuarios)
                msg.To.Add(new MailAddress(item.Email));

            msg.Subject = "Nueva Investigación de Incidente";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;

            var archivoInvestigacion = await _descargarInvestigacionFinalController.PrepararInvestigacion(IdInvestigacion);
            msg.Attachments.Add(new Attachment(archivoInvestigacion, "InvestigacionIncidente-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx"));

            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("Resumen", "InvestigacionFinal");
        }
    }
}