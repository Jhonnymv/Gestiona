using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.Notificaciones.NuevaInspeccion;
using GestionAmbiental.Application.UseCases.Notificaciones.ObservacionImplementada;
using GestionAmbiental.Application.UseCases.Notificaciones.ObservacionRechazada;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace GestionAmbiental.UseCases.Notificaciones.Inspecciones
{
    public class NotificacionesInspeccionesController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IGetNuevaInspeccionInfo _getNuevaInspeccionInfo;
        private readonly IGetObservacionImplementada _getObservacionImplementada;
        private readonly IGetObservacionRechazada _getObservacionRechazada;
        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;

        public NotificacionesInspeccionesController(
            IWebHostEnvironment hostingEnvironment, 
            IGetNuevaInspeccionInfo getNuevaInspeccionInfo, 
            IGetObservacionImplementada getObservacionImplementada,
            IGetObservacionRechazada getObservacionRechazada, 
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
            IGetInfo_SGA info_SGA)
        {
            _hostingEnvironment = hostingEnvironment;
            _getNuevaInspeccionInfo = getNuevaInspeccionInfo;
            _getObservacionImplementada = getObservacionImplementada;
            _getObservacionRechazada = getObservacionRechazada;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InspeccionIdRouteValue);
            _info_SGA = info_SGA;
        }

        public async Task<IActionResult> NotificarInspeccion(int IdCabecera)
        {
            //Obtener colores e imagen de fondo
            var r = 45;
            var g = 100;
            var b = 150;
            var img_url = "/img/back-inspection.png";
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("Inspection"));
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
            var info = await _getNuevaInspeccionInfo.Execute(IdCabecera);
            var template = "";
            foreach (var observacion in info.Observaciones)
            {
                if (!observacion.DescripcionCondicion.Equals("Buenas Prácticas"))
                    template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Inspecciones", "NuevaObservacion.html");
                else
                    template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Inspecciones", "ObservacionImplementada.html");
                StreamReader SourceReader = System.IO.File.OpenText(template);
                var templateReaded = SourceReader.ReadToEnd();
                var msgBody = "";
                if (!observacion.DescripcionCondicion.Equals("Buenas Prácticas"))
                {
                    msgBody = string.Format(templateReaded,
                        $"#{r:X2}{g:X2}{b:X2}",
                        info.Codigo,
                        info.Gerencia,
                        info.Empresa,
                        info.GerenciaInspeccionada,
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
                        observacion.TipoCondicion != null ? observacion.TipoCondicion.Descripcion : "No Aplica",
                        observacion.DescripcionActoCondicion,
                        observacion.NivelRiesgo != null ? observacion.NivelRiesgo : "No Aplica",
                        observacion.DescripcionAccion != null ? observacion.DescripcionAccion : "No Aplica",
                        observacion.ResponsableMarcobre.Nombres + " " + observacion.ResponsableMarcobre.Apellidos,
                        observacion.ResponsableContratista,
                        (observacion.FechaProgramada.HasValue ? observacion.FechaProgramada.Value.ToShortDateString() : "No Aplica"),
                        _protector.Protect(IdCabecera.ToString())
                        );
                }
                else
                {
                    msgBody = string.Format(templateReaded,
                        $"#{r:X2}{g:X2}{b:X2}",
                        info.Codigo,
                        info.Gerencia,
                        info.Empresa,
                        info.GerenciaInspeccionada,
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
                        observacion.TipoCondicion != null ? observacion.TipoCondicion.Descripcion : "No Aplica",
                        observacion.DescripcionActoCondicion,
                        observacion.NivelRiesgo != null ? observacion.NivelRiesgo : "No Aplica",
                        observacion.DescripcionAccion != null ? observacion.DescripcionAccion : "No Aplica",
                        observacion.ResponsableMarcobre.Nombres + " " + observacion.ResponsableMarcobre.Apellidos,
                        observacion.ResponsableContratista,
                        (observacion.FechaProgramada.HasValue ? observacion.FechaProgramada.Value.ToShortDateString() : "No Aplica"),
                        "No Aplica", //Comentario Implementacion
                        "No Aplica", //Fecha Implementacion             
                        _protector.Protect(IdCabecera.ToString())
                        );
                }
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
                if(!observacion.DescripcionCondicion.Equals("Buenas Prácticas"))
                    msg.To.Add(new MailAddress(observacion.ResponsableMarcobre.Email));
                else
                    msg.To.Add(new MailAddress(observacion.Asesor.Email));
                msg.Subject = "Nueva Observacion Asignada";
                msg.SubjectEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
                msg.BodyEncoding = Encoding.UTF8;
                msg.Body = msgBody;
                await mail.SendMailAsync(msg);
                mail.Dispose();
                msg.Dispose();
                SourceReader.Close();
            }
            var IdEncriptado = _protector.Protect(IdCabecera.ToString());
            return RedirectToAction("DetalleInspeccion", "Inspecciones", new { IdCabecera = IdEncriptado });
        }
        public async Task<IActionResult> NotificarImplementacionObservacion(string IdObservacion)
        {
            var r = 45;
            var g = 100;
            var b = 150;
            var img_url = "/img/back-inspection.png";
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("Inspection"));
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
            var IdObsDesencriptado = Convert.ToInt32(_protector.Unprotect(IdObservacion));
            var info = await _getObservacionImplementada.Execute(IdObsDesencriptado);
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Inspecciones", "ObservacionImplementada.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();            
            var msgBody = string.Format(templateReaded,
                $"#{r:X2}{g:X2}{b:X2}",
                info.Codigo,
                info.AreaEjecutora,
                info.Empresa,
                info.GerenciaInspeccionada,
                info.AreaInspeccionada,
                info.TipoInspeccion,
                info.FechaInspeccion,
                info.ResponsableLugar,
                info.ResponsableInspeccion,
                info.Participantes,
                info.Objetivo,
                info.TipoObservacion,
                info.DescripcionActoCondicion,
                info.Asesor,
                info.DescripcionCondicion,
                info.TipoCondicion,
                info.NivelRiesgo,
                info.DescripcionAccion,
                info.ResponsableMarcobre,
                info.ResponsableContratista,
                info.FechaProgramada.HasValue ? info.FechaProgramada.Value.ToShortDateString() : "No Aplica",     
                info.ComentarioImplementacion,
                info.FechaCierre,
                _protector.Protect(info.IdInspeccionCabecera.ToString())
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
            msg.To.Add(new MailAddress(info.EmailAsesor));
            msg.Subject = "Observación Implementada";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;
            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            var IdEncriptado = _protector.Protect(info.IdInspeccionCabecera.ToString());
            return RedirectToAction("ImplementarInspeccion", "InspeccionesImplementacion", new { IdCabecera = IdEncriptado });
        }
        public async Task<IActionResult> NotificarObservacionRechazada(string IdObservacion)
        {
            var r = 45;
            var g = 100;
            var b = 150;
            var img_url = "/img/back-inspection.png";
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("Inspection"));
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
            var IdObsDesencriptado = Convert.ToInt32(_protector.Unprotect(IdObservacion));
            var info = await _getObservacionRechazada.Execute(IdObsDesencriptado);
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "Inspecciones", "ObservacionRechazada.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();
            var msgBody = string.Format(templateReaded,
                $"#{r:X2}{g:X2}{b:X2}",
                info.Codigo,
                info.AreaEjecutora,
                info.Empresa,
                info.GerenciaInspeccionada,
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
                info.DescripcionActoCondicion,
                info.NivelRiesgo,
                info.DescripcionAccion,
                info.ResponsableMarcobre,
                info.ResponsableContratista,
                info.FechaProgramada.HasValue ? info.FechaProgramada.Value.ToShortDateString() : "No Aplica",
                info.ComentarioImplementacion,
                info.FechaCierre,
                info.ComentarioVerificacion,
                info.FechaVerificacion,
                _protector.Protect(info.IdInspeccionCabecera.ToString())
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
            msg.To.Add(new MailAddress(info.EmailResponsableMarcobre));
            msg.Subject = "Observación Rechazada";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;
            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            var IdEncriptado = _protector.Protect(info.IdInspeccionCabecera.ToString());
            return RedirectToAction("VerificarInspeccion", "InspeccionesVerificacionMA", new { IdCabecera = IdEncriptado });
        }
    }
}