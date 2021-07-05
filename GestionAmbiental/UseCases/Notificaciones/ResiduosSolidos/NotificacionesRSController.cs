using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.Notificaciones.ResiduosSolidos.GetUsuariosNotificar;
using GestionAmbiental.Application.UseCases.Notificaciones.ResiduosSolidos.ManifiestoRSRegistrado;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace GestionAmbiental.UseCases.Notificaciones.ResiduosSolidos
{
    public class NotificacionesRSController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IGetManifiestoRSRegistrado _consultarManifiestoRSCierre;
        private readonly IGetUsuariosNotificar _getUsuariosNotificar;
        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;

        public NotificacionesRSController(
            IHostingEnvironment hostingEnvironment,
            IGetManifiestoRSRegistrado consultarManifiestoRSCierre,
            IDataProtectionProvider dataProtectionProvider,
            IGetUsuariosNotificar getUsuariosNotificar,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings, IGetInfo_SGA info_SGA)
        {
            _hostingEnvironment = hostingEnvironment;
            _consultarManifiestoRSCierre = consultarManifiestoRSCierre;
            _getUsuariosNotificar = getUsuariosNotificar;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.IngresoResiduosPeligrosSIdRouteValue);
            _info_SGA = info_SGA;
        }
        public async Task<IActionResult> NotificarRegistroRS(int Id_Manifiesto)
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
            var info = await _consultarManifiestoRSCierre.Execute(Id_Manifiesto);
            var usuarios = await _getUsuariosNotificar.Execute();
            var IdEncriptado = _protector.Protect(info.Id_Manifiesto_RS.ToString());
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "ResiduosSolidos", "NuevoRegistroManifiesto.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            var msgBody = string.Format(templateReaded,
                img_url,
                r,
                g,
                b,
                info.Item,
                info.Serie_Manifiesto,
                info.Num_Manifiesto,
                info.Fecha_Emision_Manifiesto,
                info.Observacion_Manifiesto,
                info.Residuo_Solido,
                info.Cantidad_RS_Evacuado,
                info.EO_RS_Salida,
                info.Registro_EO_RS_Salida,
                info.Fecha_Vencimiento_Registro_Salida,
                info.Num_Autorizacion_Municipal_Salida,
                info.Num_Autorizacion_Hoja_Ruta,
                info.Nombre_Conductor,
                info.Num_Licencia_Condutor,
                info.Placa_Vehiculo,
                info.Capacidad_Vehiculo,
                info.EO_RS_Llegada,
                info.Registro_EO_RS_Llegada,
                info.Fecha_Vencimiento_Registro_Llegada,
                info.Num_Autorizacion_Sanitaria_Llegada,
                info.Num_Autorizacion_Municipal_Llegada,
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
            msg.Subject = "Nuevo Registro de Manifiesto de Residuo Sólido Peligroso";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;
            msg.Body = msgBody;

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

            return RedirectToAction("Resumen", "ManifiestoRS");
        }
    }
}