using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones;
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
        private readonly IGetAccionesIncidenteInvestigacion _getAccionesIncidenteInvestigacion;
        private readonly IGetAccioIncidenteInvestigacion _getAccioIncidenteInvestigacion;
        private readonly IGetUsuariosNotificarIncidente _getUsuariosNotificarIncidente;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IDataProtector _protector;
        private readonly DescargarInvestigacionFinalController _descargarInvestigacionFinalController;

        public NotificacionInvestigacionIncidenteController(
           IHostingEnvironment hostingEnvironment,
           IGetInvestigacionFinal getInvestigacionFinal,
           IGetAccionesIncidenteInvestigacion getAccionesIncidenteInvestigacion,
           IGetAccioIncidenteInvestigacion getAccioIncidenteInvestigacion,
           IGetUsuariosNotificarIncidente getUsuariosNotificarIncidente,
           IGetInfo_SGA getInfo_SGA,
           IDataProtectionProvider dataProtectionProvider,
           DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
           DescargarInvestigacionFinalController descargarInvestigacionFinalController
           )
        {
            _hostingEnvironment = hostingEnvironment;
            _getInvestigacionFinal = getInvestigacionFinal;
            _getAccionesIncidenteInvestigacion = getAccionesIncidenteInvestigacion;
            _getAccioIncidenteInvestigacion = getAccioIncidenteInvestigacion;
            _getUsuariosNotificarIncidente = getUsuariosNotificarIncidente;
            _info_SGA = getInfo_SGA;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InvestigacionIdRouteValue);
            _descargarInvestigacionFinalController = descargarInvestigacionFinalController;
        }
        public async Task NotificarAccionInvestigacion(int IdInvestigacion)
        {
            var info = await _getInvestigacionFinal.Execute(IdInvestigacion);
            var acciones = await _getAccionesIncidenteInvestigacion.Execute(IdInvestigacion);
            acciones = acciones.Where(x => !(x.Estatus.Equals("Cerrado") && x.Tipo_Accion.Equals("Acción Inmediata"))).ToList();

            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Incidentes", "Investigacion", "NuevaAccionInvestigacion.html");

            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

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
            foreach (var accion in acciones)
            {
                var IdEncriptado = _protector.Protect(accion.Id_Accion_Correctiva_Incidente.ToString());
                var msgBody = string.Format(templateReaded,
                    IdEncriptado,
                    info.Codigo,
                    info.Fecha_Incidente.Value.ToString("dd/MM/yyyy HH:mm"),
                    info.Tipo_Incidente,
                    info.Nivel_Incidente,
                    info.Gerencia_Responsable,
                    info.Supervisor_MA,
                    accion.Tipo_Accion,
                    accion.Accion_Inmediata,
                    accion.Responsable_Marcobre,
                    accion.Responsable_Contratista,
                    accion.Fecha.Value.ToString("dd/MM/yyyy")
                    );

                SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
                MailMessage msg = new MailMessage();
                msg.Priority = MailPriority.Normal;
                mail.EnableSsl = false;

                mail.UseDefaultCredentials = false;
                NetworkCredential credential = new NetworkCredential(user, pass);
                mail.Credentials = credential;

                msg.To.Add(new MailAddress(accion.Responsable_Marcobre_Email));

                msg.Subject = "Nueva Acción de Incidente";
                msg.SubjectEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
                msg.BodyEncoding = Encoding.UTF8;
                msg.Body = msgBody;

                await mail.SendMailAsync(msg);
                msg.Attachments.Dispose();
                mail.Dispose();
                msg.Dispose();
            }
        }
        public async Task<IActionResult> DifundirInvestigacion(int IdInvestigacion)
        {
            var info = await _getInvestigacionFinal.Execute(IdInvestigacion);
            var usuarios = await _getUsuariosNotificarIncidente.Execute();
            var IdEncriptado = _protector.Protect(info.Id_Investigacion_Final.ToString());
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Incidentes", "Investigacion", "NuevaInvestigacion.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var msgBody = string.Format(templateReaded,
                IdEncriptado,
                info.Codigo,
                info.Fecha_Incidente.Value.ToString("dd/MM/yyyy HH:mm"),
                info.Tipo_Incidente,
                info.Nivel_Incidente,
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

            msg.Subject = "Nueva Investigación de Incidente";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;

            var archivoInvestigacion = await _descargarInvestigacionFinalController.PrepararInvestigacion(IdInvestigacion);
            msg.Attachments.Add(new Attachment(archivoInvestigacion, "InvestigacionIncidente-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx"));

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

            await NotificarAccionInvestigacion(IdInvestigacion);

            return RedirectToAction("Resumen", "InvestigacionFinal");
        }

        public async Task<IActionResult> NotificarAccionImplementada(int IdAccion)
        {
            var accion = await _getAccioIncidenteInvestigacion.Execute(IdAccion);

            var info = await _getInvestigacionFinal.Execute(accion.Id_Investigacion_Final.Value);

            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Incidentes", "Investigacion", "AccionInvestigacionImplementada.html");

            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

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

            var IdEncriptado = _protector.Protect(accion.Id_Accion_Correctiva_Incidente.ToString());
            var msgBody = string.Format(templateReaded,
                    IdEncriptado,
                    info.Codigo,
                    info.Fecha_Incidente.Value.ToString("dd/MM/yyyy HH:mm"),
                    info.Tipo_Incidente,
                    info.Nivel_Incidente,
                    info.Gerencia_Responsable,
                    info.Supervisor_MA,
                    accion.Tipo_Accion,
                    accion.Accion_Inmediata,
                    accion.Responsable_Marcobre,
                    accion.Responsable_Contratista,
                    accion.Fecha.Value.ToString("dd/MM/yyyy"),
                    accion.Comentarios_Implementacion
                    );

            SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
            MailMessage msg = new MailMessage();
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = false;

            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;

            msg.To.Add(new MailAddress(info.Supervisor_MA_Email));

            msg.Subject = "Acción de Incidente Implementada";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;

            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            var IdEncriptadoInv = _protector.Protect(info.Id_Investigacion_Final.ToString());
            return RedirectToAction("DetalleInvestigacion", "InvestigacionFinal", new { IdInvestigacion = IdEncriptadoInv });
        }
        public async Task<IActionResult> NotificarAccionRechazada(int IdAccion)
        {
            var accion = await _getAccioIncidenteInvestigacion.Execute(IdAccion);
            var info = await _getInvestigacionFinal.Execute(accion.Id_Investigacion_Final.Value);

            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Incidentes", "Investigacion", "AccionInvestigacionRechazada.html");

            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

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
            var IdEncriptado = _protector.Protect(accion.Id_Accion_Correctiva_Incidente.ToString());
            var msgBody = string.Format(templateReaded,
                IdEncriptado,
                info.Codigo,
                info.Fecha_Incidente.Value.ToString("dd/MM/yyyy HH:mm"),
                info.Tipo_Incidente,
                info.Nivel_Incidente,
                info.Gerencia_Responsable,
                info.Supervisor_MA,
                accion.Tipo_Accion,
                accion.Accion_Inmediata,
                accion.Responsable_Marcobre,
                accion.Responsable_Contratista,
                accion.Fecha.Value.ToString("dd/MM/yyyy"),
                accion.Comentarios_Implementacion,
                accion.Comentarios_Verificacion
                );

            SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
            MailMessage msg = new MailMessage();
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = false;

            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;

            msg.To.Add(new MailAddress(accion.Responsable_Marcobre_Email));

            msg.Subject = "Acción de Incidente Rechazada";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;

            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            var IdEncriptadoInv = _protector.Protect(info.Id_Investigacion_Final.ToString());
            return RedirectToAction("DetalleInvestigacion", "InvestigacionFinal", new { IdInvestigacion = IdEncriptadoInv });
        }
    }
}