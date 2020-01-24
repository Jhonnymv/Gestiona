using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.Notificaciones.ImplementacionRAC;
using GestionAmbiental.Application.UseCases.Notificaciones.NuevaInspeccion;
using GestionAmbiental.Application.UseCases.Notificaciones.NuevaObservacion;
using GestionAmbiental.Application.UseCases.Notificaciones.NuevoRAC;
using GestionAmbiental.Application.UseCases.Notificaciones.ObservacionImplementada;
using GestionAmbiental.Application.UseCases.Notificaciones.ObservacionRechazada;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace GestionAmbiental.UseCases.Notificaciones
{
    public class NotificacionesController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IGetNuevaInspeccionInfo _getNuevaInspeccionInfo;
        private readonly IGetObservacionImplementada _getObservacionImplementada;
        private readonly IGetObservacionRechazada _getObservacionRechazada;
        private readonly IGetNuevoRACInfo _getNuevoRACInfo;
        private readonly IGetImplementacionRACInfo _getImplementacionRACInfo;
        private readonly IDataProtector _protector;
        private readonly IDataProtector _protectorRAC;
        private readonly IGetInfo_SGA _info_SGA;

        public NotificacionesController(
            IHostingEnvironment hostingEnvironment, 
            IGetNuevaInspeccionInfo getNuevaInspeccionInfo, 
            IGetObservacionImplementada getObservacionImplementada,
            IGetObservacionRechazada getObservacionRechazada,
            IGetNuevoRACInfo getNuevoRACInfo,
            IGetImplementacionRACInfo getImplementacionRACInfo,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
            IGetInfo_SGA info_SGA)
        {
            _hostingEnvironment = hostingEnvironment;
            _getNuevaInspeccionInfo = getNuevaInspeccionInfo;
            _getObservacionImplementada = getObservacionImplementada;
            _getObservacionRechazada = getObservacionRechazada;
            _getNuevoRACInfo = getNuevoRACInfo;
            _getImplementacionRACInfo = getImplementacionRACInfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InspeccionIdRouteValue);
            _protectorRAC = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.RACIdRouteValue);
            _info_SGA = info_SGA;
        }
        public async Task<IActionResult> NotificarInspeccion(int IdCabecera)
        {
            /*
            Área Ejecutora{0}
            Empresa{1}
            Área Inspeccionada{2}
            Tipo Inspección{3}
            Fecha Inspección{4}
            Responsable Lugar Inspeccionado{5}
            Responsable de la inspección{6}
            Participantes {7}
            Objetivo de la Inspección{8}

            TipoInspeccion{9}
            Asesor SSO/MAP{10}
            Acto / Condición Subestándar Identificada{11}
            Tipo de Condición / Acto Subestándar{12}
            Nivel de Riesgo del Aspecto Ambiental{13}
            Acción Preventiva / Correctiva Tomada{14}
            Responsable Marcobre{15}
            Responsable Contratista{16}
            Fecha Programada{17}
            Codigo {18}
            Id_Cabecera{19}
            */
            
            var info = await _getNuevaInspeccionInfo.Execute(IdCabecera);
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Inspecciones", "NuevaObservacion.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            foreach (var observacion in info.Observaciones)
            {
                var msgBody = string.Format(templateReaded,
                info.Gerencia,
                info.Empresa,
                info.AreaInspeccionada,
                info.TipoInspeccion,
                info.FechaInspeccion,
                info.ResponsableArea.Nombres + " " + info.ResponsableArea.Apellidos,
                info.ResponsableInspeccion,
                info.Participantes,
                info.Objetivo,
                observacion.TipoObservacion,
                observacion.Asesor.Nombres + " " + observacion.Asesor.Apellidos,
                observacion.DescripcionCondicion,
                observacion.TipoCondicion.Descripcion,
                observacion.NivelRiesgo,
                observacion.DescripcionAccion,
                observacion.ResponsableMarcobre.Nombres + " " + observacion.ResponsableMarcobre.Apellidos,
                observacion.ResponsableContratista,
                observacion.FechaProgramada.ToShortDateString(),
                info.Codigo,
                _protector.Protect(IdCabecera.ToString())
                );
                
                var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
                string smtp = "", user = "", pass = "";
                foreach (var item in credenciales.Info)
                {
                    switch (item.Key)
                    {
                        case "smtp": smtp = item.Value; break;
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                    }
                }

                SmtpClient mail = new SmtpClient();
                MailMessage msg = new MailMessage();
                mail.Host = smtp;
                msg.Priority = MailPriority.Normal;
                mail.EnableSsl = true;
                mail.UseDefaultCredentials = false;
                NetworkCredential credential = new NetworkCredential(user, pass);
                mail.Credentials = credential;
                msg.To.Add(new MailAddress(observacion.ResponsableMarcobre.Email));
                msg.To.Add(new MailAddress(observacion.Asesor.Email));
                msg.Subject = "Nueva Observacion Asignada";
                msg.SubjectEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.From = new MailAddress(user, @"Sistema de Control - Marcobre");
                msg.BodyEncoding = Encoding.UTF8;
                msg.Body = msgBody;
                await mail.SendMailAsync(msg);                    
                mail.Dispose();
                msg.Dispose();
            }
            var IdEncriptado = _protector.Protect(IdCabecera.ToString());
            return RedirectToAction("EditarInspeccion", "Inspecciones", new { IdCabecera = IdEncriptado });
        }
        public async Task<IActionResult> NotificarImplementacionObservacion(string IdObservacion)
        {
            var IdObsDesencriptado = Convert.ToInt32(_protector.Unprotect(IdObservacion));
            var info = await _getObservacionImplementada.Execute(IdObsDesencriptado);
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Inspecciones", "ObservacionImplementada.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();
            /*
                        Área Ejecutora{0}
                        Empresa{1}
                        Área Inspeccionada{2}
                        Tipo Inspección{3}
                        Fecha Inspección{4}
                        Responsable Lugar Inspeccionado{5}
                        Responsable de la inspección{6}
                        Participantes {7}
                        Objetivo de la Inspección{8}

                        TipoInspeccion{9}
                        Asesor SSO/MAP{10}
                        Acto / Condición Subestándar Identificada{11}
                        Tipo de Condición / Acto Subestándar{12}
                        Nivel de Riesgo del Aspecto Ambiental{13}
                        Acción Preventiva / Correctiva Tomada{14}
                        Responsable Marcobre{15}
                        Responsable Contratista{16}
                        Fecha Programada{17}
                        Comentario de la Implementación{18}
                        Fecha Implementación {19}
                        Codigo {20}
                        Id_Cabecera{21}
                        */
            var msgBody = string.Format(templateReaded,
            info.AreaEjecutora,
            info.Empresa,
            info.AreaInspeccionada,
            info.TipoInspeccion,
            info.FechaInspeccion,
            info.ResponsableLugar,
            info.ResponsableInspeccion,
            info.Participantes,
            info.Objetivo,
            info.TipoObservacion,
            info.Asesor,
            info.DescripcionCondicion,
            info.TipoCondicion,
            info.NivelRiesgo,
            info.DescripcionAccion,
            info.ResponsableMarcobre,
            info.ResponsableContratista,
            info.FechaProgramada.ToShortDateString(),
            info.ComentarioImplementacion,
            info.FechaCierre,
            info.Codigo,
            _protector.Protect(info.IdInspeccionCabecera.ToString())
            );
            var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
            string smtp = "", user = "", pass = "";
            foreach (var item in credenciales.Info)
            {
                switch (item.Key)
                {
                    case "smtp": smtp = item.Value; break;
                    case "user": user = item.Value; break;
                    case "password": pass = item.Value; break;
                }
            }

            SmtpClient mail = new SmtpClient();
            MailMessage msg = new MailMessage();
            mail.Host = smtp;
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = true;
            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;
            msg.To.Add(new MailAddress(info.EmailAsesor));
            msg.Subject = "Observación Implementada";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;

            msg.From = new MailAddress(user, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;
            mail.UseDefaultCredentials = false;
            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            var IdEncriptado = _protector.Protect(info.IdInspeccionCabecera.ToString());
            return RedirectToAction("ImplementarInspeccion", "InspeccionesImplementacion", new { IdCabecera = IdEncriptado });
        }
        public async Task<IActionResult> NotificarObservacionRechazada(string IdObservacion)
        {
            var IdObsDesencriptado = Convert.ToInt32(_protector.Unprotect(IdObservacion));
            var info = await _getObservacionRechazada.Execute(IdObsDesencriptado);
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates","Inspecciones", "ObservacionRechazada.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();
            /*
                        Área Ejecutora{0}
                        Empresa{1}
                        Área Inspeccionada{2}
                        Tipo Inspección{3}
                        Fecha Inspección{4}
                        Responsable Lugar Inspeccionado{5}
                        Responsable de la inspección{6}
                        Participantes {7}
                        Objetivo de la Inspección{8}

                        TipoInspeccion{9}
                        Asesor SSO/MAP{10}
                        Acto / Condición Subestándar Identificada{11}
                        Tipo de Condición / Acto Subestándar{12}
                        Nivel de Riesgo del Aspecto Ambiental{13}
                        Acción Preventiva / Correctiva Tomada{14}
                        Responsable Marcobre{15}
                        Responsable Contratista{16}
                        Fecha Programada{17}
                        Comentario de la Implementación{18}
                        Fecha Implementación {19}                        
                        Comentario de la Verificación{20}
                        Fecha Verificación {21}
                        Codigo {22}
                        Id_Cabecera{23}
                        */
            var msgBody = string.Format(templateReaded,
                info.AreaEjecutora,
                info.Empresa,
                info.AreaInspeccionada,
                info.TipoInspeccion,
                info.FechaInspeccion,
                info.ResponsableLugar,
                info.ResponsableInspeccion,
                info.Participantes,
                info.Objetivo,
                info.TipoObservacion,
                info.Asesor,
                info.DescripcionCondicion,
                info.TipoCondicion,
                info.NivelRiesgo,
                info.DescripcionAccion,
                info.ResponsableMarcobre,
                info.ResponsableContratista,
                info.FechaProgramada.ToShortDateString(),
                info.ComentarioImplementacion,
                info.FechaCierre,
                info.ComentarioVerificacion,
                info.FechaVerificacion,
                info.Codigo,
                _protector.Protect(info.IdInspeccionCabecera.ToString())
                );
            var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
            string smtp = "", user = "", pass = "";
            foreach (var item in credenciales.Info)
            {
                switch (item.Key)
                {
                    case "smtp": smtp = item.Value; break;
                    case "user": user = item.Value; break;
                    case "password": pass = item.Value; break;
                }
            }
            SmtpClient mail = new SmtpClient();
            MailMessage msg = new MailMessage();
            mail.Host = smtp;
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = true;
            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;
            msg.To.Add(new MailAddress(info.EmailResponsableMarcobre));
            msg.Subject = "Observación Rechazada";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(user, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;
            mail.UseDefaultCredentials = false;
            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            var IdEncriptado = _protector.Protect(info.IdInspeccionCabecera.ToString());
            return RedirectToAction("VerificarInspeccion", "InspeccionesVerificacionMA", new { IdCabecera = IdEncriptado });
        }
        public async Task<IActionResult> NotificarNuevoRAC(int IdRAC)
        {
            var info = await _getNuevoRACInfo.Execute(IdRAC);
            var IdEncriptado = _protectorRAC.Protect(info.Id_RAC.ToString());
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "RAC", "NuevoRAC.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();
            
            var msgBody = string.Format(templateReaded,
                info.Nombres_Apellidos,
                info.Gerencia,
                info.Empresa,
                info.Otra_Empresa,
                info.Empresa_Ob,
                info.Gerencia_Obs,
                info.Ubicacion,
                info.Fecha.ToString("dd/MM/yyyy"),
                info.Fecha.ToString("HH:mm"),
                info.SSO_MA,
                info.Clasificacion,
                info.NivelRiesgo,
                info.TipoCondicion,
                info.Descripcion_Acto_Cond,
                info.Medida_Control,
                info.PAC,
                info.Responsable_Cierre,
                info.Fecha_Cierre.ToString("dd/MM/yyyy"),
                info.Supervisor,
                info.Codigo,
                IdEncriptado
                );
            
            var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
            string smtp = "", user = "", pass = "";
            foreach (var item in credenciales.Info)
            {
                switch (item.Key)
                {
                    case "smtp": smtp = item.Value; break;
                    case "user": user = item.Value; break;
                    case "password": pass = item.Value; break;
                }
            }

            SmtpClient mail = new SmtpClient();
            MailMessage msg = new MailMessage();
            mail.Host = smtp;
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = true;
            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;
            msg.To.Add(new MailAddress(info.Responsable_Cierre_Email));
            msg.Subject = "Nuevo Reporte de Condición y Acto Subestándar";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(user, @"Sistema de Control - Marcobre");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;
            mail.UseDefaultCredentials = false;
            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();
            
            return RedirectToAction("ImplementarRAC", "ImplementacionRAC", new { IdRAC = IdEncriptado });
        }
        public async Task<IActionResult> NotificarImplementacionRAC(int IdRAC)
        {
            var info = await _getImplementacionRACInfo.Execute(IdRAC);
            var IdEncriptado = _protectorRAC.Protect(info.Id_RAC.ToString());
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "RAC", "ImplementacionRAC.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var msgBody = string.Format(templateReaded,
            info.Nombres_Apellidos,
            info.Gerencia,
            info.Empresa,
            info.Otra_Empresa,
            info.Empresa_Ob,
            info.Gerencia_Obs,
            info.Ubicacion,
            info.Fecha.ToString("dd/MM/yyyy"),
            info.Fecha.ToString("HH:mm"),
            info.SSO_MA,
            info.Clasificacion,
            info.NivelRiesgo,
            info.TipoCondicion,
            info.Descripcion_Acto_Cond,
            info.Medida_Control,
            info.PAC,
            info.Responsable_Cierre,
            info.Fecha_Cierre.ToString("dd/MM/yyyy"),
            info.Supervisor,
            info.Codigo, 
            IdEncriptado
            );

            var evidencias_condicion = info.Evidencia_Obs;

            
            var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
            string smtp = "", user = "", pass = "";
            foreach (var item in credenciales.Info)
            {
                switch (item.Key)
                {
                    case "smtp": smtp = item.Value; break;
                    case "user": user = item.Value; break;
                    case "password": pass = item.Value; break;
                }
            }

            SmtpClient mail = new SmtpClient();
            MailMessage msg = new MailMessage();
            mail.Host = smtp;
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = true;
            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;
            msg.To.Add(new MailAddress(info.Responsable_Cierre_Email));
            msg.Subject = "Nuevo Reporte de Condición y Acto Subestándar";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(user, @"Sistema de Control - Marcobre");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;
            mail.UseDefaultCredentials = false;
            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("VerificarRAC", "VerificacionRAC", new { IdRAC = IdEncriptado });
        }
    }
}