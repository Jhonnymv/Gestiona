using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.Notificaciones.ImplementacionRAC;
using GestionAmbiental.Application.UseCases.Notificaciones.NuevoRAC;
using GestionAmbiental.Application.UseCases.Notificaciones.RacRechazado;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Notificaciones.RAC
{
    public class NotificacionesRACController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IGetNuevoRACInfo _getNuevoRACInfo;
        private readonly IGetImplementacionRACInfo _getImplementacionRACInfo;
        private readonly IGetRACRechazadoInfo _getRACRechazadoInfo;
        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;
        public NotificacionesRACController(
            IHostingEnvironment hostingEnvironment,
            IGetNuevoRACInfo getNuevoRACInfo,
            IGetImplementacionRACInfo getImplementacionRACInfo,
            IGetRACRechazadoInfo getRACRechazadoInfo,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings,
            IGetInfo_SGA info_SGA)
        {
            _hostingEnvironment = hostingEnvironment;
            _getNuevoRACInfo = getNuevoRACInfo;
            _getImplementacionRACInfo = getImplementacionRACInfo;
            _getRACRechazadoInfo = getRACRechazadoInfo;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.RACIdRouteValue);
            _info_SGA = info_SGA;
        }
        public async Task<IActionResult> NotificarNuevoRAC(int IdRAC)
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
                var dataModulo = prop.Find(x => x.modulo.Equals("RAC"));
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
            var info = await _getNuevoRACInfo.Execute(IdRAC);
            var IdEncriptado = _protector.Protect(info.Id_RAC.ToString());
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "RAC", "NuevoRAC.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var msgBody = string.Format(templateReaded,
                img_url,
                r,
                g,
                b,
                info.Codigo,
                info.Nombres_Apellidos,
                info.Gerencia,
                info.Empresa,
                info.Otra_Empresa != "" ? "" : "No Aplica",
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
            mail.EnableSsl = true;
            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;
            msg.To.Add(new MailAddress(info.Responsable_Cierre_Email));
            msg.Subject = "Nuevo Reporte de Condición y Acto Subestándar";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;
            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("DetalleRAC", "RegistroRAC", new { IdRAC = IdEncriptado });
        }
        public async Task<IActionResult> NotificarImplementacionRAC(int IdRAC)
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
                var dataModulo = prop.Find(x => x.modulo.Equals("RAC"));
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
            var info = await _getImplementacionRACInfo.Execute(IdRAC);
            var IdEncriptado = _protector.Protect(info.Id_RAC.ToString());
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "RAC", "ImplementacionRAC.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var msgBody = string.Format(templateReaded,
                img_url,
                r,
                g,
                b,
                info.Codigo,
                info.Nombres_Apellidos,
                info.Gerencia,
                info.Empresa,
                info.Otra_Empresa != "" ? "" : "No Aplica",
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
                info.Comentario_Implementacion,
                IdEncriptado
                );

            var evidencias_condicion = info.Evidencia_Obs;

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
            mail.EnableSsl = true;
            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;
            msg.To.Add(new MailAddress(info.Supervisor_Email));
            msg.Subject = "Nuevo Reporte de Condición y Acto Subestándar";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;
            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("ImplementarRAC", "ImplementacionRAC", new { IdRAC = IdEncriptado });
        }
        public async Task<IActionResult> NotificarRACRechazado(int IdRAC)
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
                var dataModulo = prop.Find(x => x.modulo.Equals("RAC"));
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
            var info = await _getRACRechazadoInfo.Execute(IdRAC);
            var IdEncriptado = _protector.Protect(info.Id_RAC.ToString());
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "RAC", "RACRechazado.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var msgBody = string.Format(templateReaded,
                 img_url,
                 r,
                 g,
                 b,
                 info.Codigo,
                 info.Nombres_Apellidos,
                 info.Gerencia,
                 info.Empresa,
                 info.Otra_Empresa != "" ? "" : "No Aplica",
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
                 info.Comentario_Implementacion,
                 info.Comentario_Verificacion,
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
            mail.EnableSsl = true;
            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;
            msg.To.Add(new MailAddress(info.Responsable_Cierre_Email));
            msg.Subject = "Reporte de Condición y Acto Subestándar Rechazado";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;
            await mail.SendMailAsync(msg);
            msg.Attachments.Dispose();
            mail.Dispose();
            msg.Dispose();

            return RedirectToAction("Resumen", "VerificacionRAC");
        }
    }
}
