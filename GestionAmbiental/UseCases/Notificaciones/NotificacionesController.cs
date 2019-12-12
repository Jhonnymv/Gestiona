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
using GestionAmbiental.Application.UseCases.Notificaciones.ObservacionImplementada;
using GestionAmbiental.Application.UseCases.Notificaciones.ObservacionRechazada;
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

        public NotificacionesController(IHostingEnvironment hostingEnvironment, IGetNuevaInspeccionInfo getNuevaInspeccionInfo, IGetObservacionImplementada getObservacionImplementada, IGetObservacionRechazada getObservacionRechazada)
        {
            _hostingEnvironment = hostingEnvironment;
            _getNuevaInspeccionInfo = getNuevaInspeccionInfo;
            _getObservacionImplementada = getObservacionImplementada;
            _getObservacionRechazada = getObservacionRechazada;
        }
        public async Task<IActionResult> NotificarInspección(int IdCabecera)
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
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates","NuevaObservacion.html");
            StreamReader SourceReader = System.IO.File.OpenText(template);
            var templateReaded = SourceReader.ReadToEnd();

            foreach (var observacion in info.Observaciones)
            {
                var msgBody = string.Format(templateReaded,
                info.GerenEjecMar,
                info.EmpContratMarc,
                info.LugarInspec,
                info.Tipo_Inspeccion,
                info.FechaInspec,
                info.Responsable_Lugar.Nombres+" "+info.Responsable_Lugar.Apellidos,
                info.Responsable_Inspeccion,
                info.Inspectores,
                info.Objetivo,
                observacion.Tipo_Observacion,
                observacion.Responsable_MA.Nombres+" "+ observacion.Responsable_MA.Apellidos,
                observacion.Descripcion_Condicion,
                observacion.Tipo_Condicion.Descripcion,
                observacion.Nivel_Riesgo_Ambiental,
                observacion.Descripcion_Accion,
                observacion.Responsable_Marcobre.Nombres+ " "+ observacion.Responsable_Marcobre.Apellidos,
                observacion.Responsable_Contratista,
                observacion.Fecha_Programada.ToShortDateString(),
                info.Codigo,
                IdCabecera
                );

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
                    msg.To.Add(new MailAddress(observacion.Responsable_MA.Email));
                    msg.Subject = "Nueva Observacion Asignada";
                    msg.SubjectEncoding = Encoding.UTF8;
                    msg.IsBodyHtml = true;
                    msg.From = new MailAddress("kopruebasmail@gmail.com", @"Sistema de Control - Marcobre");
                    msg.BodyEncoding = Encoding.UTF8;
                    msg.Body = msgBody;
                    /*foreach (var evidencia in listEvidencias_condicion)
                    {
                        msg.Attachments.Add(new Attachment(evidencia));
                    }*/
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
            }
            return RedirectToAction("EditarInspeccion", "Inspecciones", new { IdCabecera = IdCabecera });
        }
        public async Task<IActionResult> NotificarImplementacionObservacion(int IdObservacion)
        {
            var info = await _getObservacionImplementada.Execute(IdObservacion);
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates","ObservacionImplementada.html");
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
                info.Area_Ejecutora,
                info.Empresa,
                info.Area_Inspeccionada,
                info.Tipo_Inspeccion,
                info.Fecha_Inspeccion,
                info.Responsable_Lugar,
                info.ResponsableInspeccion,
                info.Participantes,
                info.Objetivo,
                info.Tipo_Observacion,
                info.Asesor_SSO_MAP,
                info.Descripcion_Condicion,
                info.Tipo_Condicion,
                info.Nivel_Riesgo_Ambiental,
                info.Descripcion_Accion,
                info.Responsable_Marcobre,
                info.Responsable_Contratista,
                info.Fecha_Programada.ToShortDateString(),
                info.Comentario_Implementacion,
                info.Fecha_Cierre,
                info.Codigo,
                info.Id_Inspeccion_Cabecera
                );
            try
            {
                SmtpClient MyMail = new SmtpClient();
                MailMessage MyMsg = new MailMessage();
                MyMail.Host = "smtp.gmail.com";
                MyMsg.Priority = MailPriority.Normal;
                MyMail.EnableSsl = true;
                MyMsg.To.Add(new MailAddress(info.Email_Asesor_MAP_SSO));
                MyMsg.Subject = "Observación Implementada";
                MyMsg.SubjectEncoding = Encoding.UTF8;
                MyMsg.IsBodyHtml = true;
                MyMsg.From = new MailAddress("kopruebasmail@gmail.com", @"Sistema de Gestión Ambiental");
                MyMsg.BodyEncoding = Encoding.UTF8;
                MyMsg.Body = msgBody;
                /*foreach (var evidencia in info.Evidencia_Condicion)
                {
                    MyMsg.Attachments.Add(new Attachment(evidencia));
                }
                foreach (var evidencia in info.Evidencia_Implementacion)
                {
                    MyMsg.Attachments.Add(new Attachment(evidencia));
                }*/
                MyMail.UseDefaultCredentials = false;
                NetworkCredential MyCredentials = new NetworkCredential("kopruebasmail@gmail.com", "Wildones6!");
                MyMail.Credentials = MyCredentials;
                MyMail.EnableSsl = true;
                await MyMail.SendMailAsync(MyMsg);
                MyMsg.Attachments.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("ImplementarInspeccion", "InspeccionesImplementacion", new { IdCabecera = info.Id_Inspeccion_Cabecera });
        }
        public async Task<IActionResult> NotificarObservacionRechazada(int IdObservacion)
        {
            var info = await _getObservacionRechazada.Execute(IdObservacion);
            var template = Path.Combine(_hostingEnvironment.WebRootPath, "MailTemplates", "ObservacionRechazada.html");
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
                info.Area_Ejecutora,
                info.Empresa,
                info.Area_Inspeccionada,
                info.Tipo_Inspeccion,
                info.Fecha_Inspeccion,
                info.Responsable_Lugar,
                info.ResponsableInspeccion,
                info.Participantes,
                info.Objetivo,
                info.Tipo_Observacion,
                info.Asesor_SSO_MAP,
                info.Descripcion_Condicion,
                info.Tipo_Condicion,
                info.Nivel_Riesgo_Ambiental,
                info.Descripcion_Accion,
                info.Responsable_Marcobre,
                info.Responsable_Contratista,
                info.Fecha_Programada.ToShortDateString(),
                info.Comentario_Implementacion,
                info.Fecha_Cierre,
                info.Comentario_Verificacion,
                info.Fecha_Verificacion,
                info.Codigo,
                info.Id_Inspeccion_Cabecera
                );
            try
            {
                SmtpClient MyMail = new SmtpClient();
                MailMessage MyMsg = new MailMessage();
                MyMail.Host = "smtp.gmail.com";
                MyMsg.Priority = MailPriority.Normal;
                MyMail.EnableSsl = true;
                MyMsg.To.Add(new MailAddress(info.Email_Responsable_Marcobre));
                MyMsg.Subject = "Observación Rechazada";
                MyMsg.SubjectEncoding = Encoding.UTF8;
                MyMsg.IsBodyHtml = true;
                MyMsg.From = new MailAddress("kopruebasmail@gmail.com", @"Sistema de Gestión Ambiental");
                MyMsg.BodyEncoding = Encoding.UTF8;
                MyMsg.Body = msgBody;
                /*foreach (var evidencia in info.Evidencia_Condicion)
                {
                    MyMsg.Attachments.Add(new Attachment(evidencia));
                }
                foreach (var evidencia in info.Evidencia_Implementacion)
                {
                    MyMsg.Attachments.Add(new Attachment(evidencia));
                }*/
                MyMail.UseDefaultCredentials = false;
                NetworkCredential MyCredentials = new NetworkCredential("kopruebasmail@gmail.com", "Wildones6!");
                MyMail.Credentials = MyCredentials;
                await MyMail.SendMailAsync(MyMsg);
                MyMsg.Attachments.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("VerificarInspeccion", "InspeccionesVerificacionMA", new { IdCabecera = info.Id_Inspeccion_Cabecera });
        }
    }
}