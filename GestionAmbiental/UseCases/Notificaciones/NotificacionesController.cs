using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Notificaciones.NuevaInspeccion;
using GestionAmbiental.Application.UseCases.Notificaciones.NuevaObservacion;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace GestionAmbiental.UseCases.Notificaciones
{
    public class NotificacionesController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IGetNuevaObservacionInfo _getNuevaObservacionInfo;
        private readonly IGetNuevaInspeccionInfo _getNuevaInspeccionInfo;
        public NotificacionesController(
            IHostingEnvironment hostingEnvironment,
            IGetNuevaObservacionInfo getNuevaObservacionInfo,
            IGetNuevaInspeccionInfo getNuevaInspeccionInfo
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _getNuevaObservacionInfo = getNuevaObservacionInfo;
            _getNuevaInspeccionInfo = getNuevaInspeccionInfo;
        }

        public async Task<IActionResult> NotificarNuevaObservacionAsignada(int IdObservacion)
        {
            var info = await _getNuevaObservacionInfo.Execute(IdObservacion);
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", GetTemplate("NuevaObservacion"));
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            /*
             Gerencia Inspeccionada Marcobre{0}
             Empresa Contratista Marcobre{1}
             Gerencia Inspeccionada EPCM{2}
             Empresa Contratista EPCM{3}
             Lugar Inspeccionado{4}
             Fecha Inspección{5}
             Responsable Lugar Inspeccionado{6}
             Responsable Medio Ambiente{7}
             Inspector(es){8}
             Objetivo de la Inspección{9}
             Acto / Condición Subestándar Identificada{10}
             Tipo de Condición / Acto Subestándar{11}
             Nivel de Riesgo del Aspecto Ambiental{12}
             Acción Preventiva / Correctiva Tomada{13}
             Responsable Marcobre{14}
             Responsable Contratista{15}
             Fecha Programada{16}
             */

            var msgBody = string.Format(templateReaded,
                info.Gerencia_Inspeccionada_Marcobre,
                info.Empresa_Contratista_Marcobre,
                info.Gerencia_Inspeccionada_EPCM,
                info.Empresa_Contratista_EPCM,
                info.Lugar_Inspeccionado,
                info.Fecha_Inspeccion,
                info.Responsable_Lugar,
                info.Responsable_MA,
                info.Inspectores,
                info.Objetivo,
                info.Descripcion_Condicion,
                info.Tipo_Condicion,
                info.Nivel_Riesgo_Ambiental,
                info.Descripcion_Accion,
                info.Responsable_Marcobre,
                info.Responsable_Contratista,
                info.Fecha_Programada.ToShortDateString()
                );

            try
            {
                SmtpClient MyMail = new SmtpClient();
                MailMessage MyMsg = new MailMessage();
                MyMail.Host = "smtp.gmail.com";
                MyMsg.Priority = MailPriority.Normal;
                MyMail.EnableSsl = true;
                MyMsg.To.Add(new MailAddress(info.Email_Responsable_Marcobre));
                MyMsg.Subject = "Nueva Observacion Asignada";
                MyMsg.SubjectEncoding = Encoding.UTF8;
                MyMsg.IsBodyHtml = true;
                MyMsg.From = new MailAddress("kopruebasmail@gmail.com", @"Sistema de Control - Marcobre");
                MyMsg.BodyEncoding = Encoding.UTF8;
                MyMsg.Body = msgBody;
                foreach(var evidencia in info.Evidencia_Condicion)
                {
                    MyMsg.Attachments.Add(new Attachment(evidencia));
                }
                MyMail.UseDefaultCredentials = false;
                NetworkCredential MyCredentials = new NetworkCredential("kopruebasmail@gmail.com", "Wildones6!");
                MyMail.Credentials = MyCredentials;
                MyMail.EnableSsl = true;
                //await MyMail.SendMailAsync(MyMsg);
                MyMsg.Attachments.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("EditarInspeccion", "Inspecciones",new { IdCabecera = info.Id_Inspeccion_Cabecera});
        }
        public async Task<IActionResult> NotificarInspección(int IdCabecera)
        {
            /*
            var info = await _getNuevaInspeccionInfo.Execute(IdCabecera);

            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", GetTemplate("NuevaObservacion"));
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            foreach (var observacion in info.Observaciones)
            {
                var msgBody = string.Format(templateReaded,
                info.GerenEjecMar,
                info.EmpContratMarc,
                info.GerenEjecEPCM,
                info.EmpContratEPCM,
                info.LugarInspec,
                info.FechaInspec,
                info.Responsable_Lugar.Nombres + " " + info.Responsable_Lugar.Apellidos,
                info.Inspectores,
                info.Objetivo,
                observacion.Descripcion_Condicion,
                observacion.Tipo_Condicion.Descripcion,
                observacion.Nivel_Riesgo_Ambiental,
                observacion.Descripcion_Accion,
                observacion.Responsable_Marcobre.Nombres+" " + observacion.Responsable_Marcobre.Apellidos,
                observacion.Responsable_Contratista,
                observacion.Fecha_Programada.ToShortDateString());

                var evidencias_condicion = JArray.Parse(observacion.Evidencia_Condicion);
                var listEvidencias_condicion = new List<string>();
                foreach (var evidencia in evidencias_condicion)
                {
                    listEvidencias_condicion.Add((string)evidencia);
                }

                try
                {
                    SmtpClient mail = new SmtpClient();
                    MailMessage msg = new MailMessage();
                    mail.Host = "smtp.gmail.com";
                    msg.Priority = MailPriority.Normal;
                    mail.EnableSsl = true;
                    msg.To.Add(new MailAddress(observacion.Responsable_Marcobre.Email));
                    msg.Subject = "Nueva Observacion Asignada";
                    msg.SubjectEncoding = Encoding.UTF8;
                    msg.IsBodyHtml = true;
                    msg.From = new MailAddress("kopruebasmail@gmail.com", @"Sistema de Control - Marcobre");
                    msg.BodyEncoding = Encoding.UTF8;
                    msg.Body = msgBody;
                    foreach (var evidencia in listEvidencias_condicion)
                    {
                        msg.Attachments.Add(new Attachment(evidencia));
                    }
                    mail.UseDefaultCredentials = false;
                    NetworkCredential MyCredentials = new NetworkCredential("kopruebasmail@gmail.com", "Wildones6!");
                    mail.Credentials = MyCredentials;
                    mail.EnableSsl = true;
                    await mail.SendMailAsync(msg);
                    msg.Attachments.Dispose();
                    mail.Dispose();
                    msg.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }*/
            return RedirectToAction("EditarInspeccion", "Inspecciones", new { IdCabecera = IdCabecera });
        }
        private string GetTemplate(string accion)
        {
            var nombreTemplate = "";
            switch (accion)
            {
                case "NuevaObservacion": nombreTemplate = "NuevaObservacion.html"; break;
            }
            return nombreTemplate;
        }
    }
}