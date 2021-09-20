using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumoAgua;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.Notificaciones.ConsumoAgua;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace GestionAmbiental.UseCases.Notificaciones.ConsumoAgua
{
    public class NotificacionesConsumoAguaController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IGetConsumoAgua _getConsumoAgua;
        private readonly IGetUsuariosNotificar _getUsuariosNotificar;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IDataProtector _protector;

        public NotificacionesConsumoAguaController(
           IWebHostEnvironment hostingEnvironment,
           IGetConsumoAgua getConsumoAgua,
           IGetUsuariosNotificar getUsuariosNotificar,
           IGetInfo_SGA getInfo_SGA,
           IDataProtectionProvider dataProtectionProvider,
           DataProtectionPurpouseStrings dataProtectionPurpouseStrings
           )
        {
            _hostingEnvironment = hostingEnvironment;
            _getConsumoAgua = getConsumoAgua;
            _getUsuariosNotificar = getUsuariosNotificar;
            _info_SGA = getInfo_SGA;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.ConsumoAguaMarIdRouteValue);
        }
        public async Task<IActionResult> NotificarLimiteConsumoAguaMarMensual(int IdConsumo, double? limiteMensual, double volumenActual)
        {
            var r = 3;
            var g = 95;
            var b = 165;
            var img_url = "/img/back-inspection.png";
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("Agua"));
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
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "ConsumoAgua", "ConsumoAguaMarMensualExedido.html");
            
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

            var consumo = await _getConsumoAgua.Execute(IdConsumo);
            var idEncriptado = _protector.Protect(IdConsumo.ToString());
            
            var msgBody = string.Format(templateReaded,
                $"#{r:X2}{g:X2}{b:X2}",
                limiteMensual,
                volumenActual,
                consumo.ResolucionConsumoAgua,
                consumo.FuenteAgua,
                consumo.Fecha.ToString("dd/MM/yyyy"),
                consumo.TiempoBombeo,
                consumo.LecturaInicial,
                consumo.LecturaFinal,
                consumo.VolumenBombeado,
                consumo.CaudalPromedio,
                idEncriptado
                );
            
            SmtpClient mail = new SmtpClient(smtp, Convert.ToInt32(port));
            MailMessage msg = new MailMessage();
            msg.Priority = MailPriority.Normal;
            mail.EnableSsl = false;

            mail.UseDefaultCredentials = false;
            NetworkCredential credential = new NetworkCredential(user, pass);
            mail.Credentials = credential;

            msg.Subject = "Consumo Mensual de Agua de Mar Cerca de Límite o Exedido";
            msg.SubjectEncoding = Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
            msg.BodyEncoding = Encoding.UTF8;

            msg.Body = msgBody;

            var usuariosNotificar = await _getUsuariosNotificar.Execute();
            var correosUsuarios = new List<MailAddress>();
            for (int i = 0; i < usuariosNotificar.Count; i++)
            {
                msg.To.Add(new MailAddress(usuariosNotificar[i].Email));
                correosUsuarios.Add(new MailAddress(usuariosNotificar[i].Email));
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

            return RedirectToAction("Resumen", "ConsumoAguaMar");
        }
    }
}
