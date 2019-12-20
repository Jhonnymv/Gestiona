using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA;
using GestionAmbiental.Application.UseCases.Notificaciones.NuevaInspeccion;
using GestionAmbiental.Application.UseCases.Notificaciones.NuevaObservacion;
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
        private readonly IDataProtector _protector;
        private readonly IGetInfo_SGA _info_SGA;

        public NotificacionesController(
            IHostingEnvironment hostingEnvironment, 
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
                info.Gerencia,
                info.Empresa,
                info.AreaInspeccionada,
                info.TipoInspeccion,
                info.FechaInspeccion,
                info.ResponsableArea.Nombres+" "+info.ResponsableArea.Apellidos,
                info.ResponsableInspeccion,
                info.Participantes,
                info.Objetivo,
                observacion.TipoObservacion,
                observacion.Asesor.Nombres+" "+ observacion.Asesor.Apellidos,
                observacion.DescripcionCondicion,
                observacion.TipoCondicion.Descripcion,
                observacion.NivelRiesgo,
                observacion.DescripcionAccion,
                observacion.ResponsableMarcobre.Nombres+ " "+ observacion.ResponsableMarcobre.Apellidos,
                observacion.ResponsableContratista,
                observacion.FechaProgramada.ToShortDateString(),
                info.Codigo,
                IdCabecera
                );

                var evidencias_condicion = JArray.Parse(observacion.EvidenciaCondicion);
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
                    msg.To.Add(new MailAddress(observacion.ResponsableMarcobre.Email));
                    msg.To.Add(new MailAddress(observacion.Asesor.Email));
                    msg.Subject = "Nueva Observacion Asignada";
                    msg.SubjectEncoding = Encoding.UTF8;
                    msg.IsBodyHtml = true;
                    var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
                    string user = "", pass = "";
                    foreach(var item in credenciales.Info)
                    {
                        switch (item.Key)
                        {
                            case "user":user = item.Value; break;
                            case "password": pass = item.Value; break;
                        }
                    }
                    msg.From = new MailAddress(user, @"Sistema de Control - Marcobre");
                    msg.BodyEncoding = Encoding.UTF8;
                    msg.Body = msgBody;
                    mail.UseDefaultCredentials = false;
                    NetworkCredential MyCredentials = new NetworkCredential(user, pass);
                    mail.Credentials = MyCredentials;
                    mail.EnableSsl = true;
                    //await mail.SendMailAsync(msg);
                    msg.Attachments.Dispose();
                    mail.Dispose();
                    msg.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            var IdEncriptado = _protector.Protect(IdCabecera.ToString());
            return RedirectToAction("EditarInspeccion", "Inspecciones", new { IdCabecera = IdEncriptado });
        }
        public async Task<IActionResult> NotificarImplementacionObservacion(string IdObservacion)
        {
            var IdObsDesencriptado = Convert.ToInt32(_protector.Unprotect(IdObservacion));
            var info = await _getObservacionImplementada.Execute(IdObsDesencriptado);
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
                info.IdInspeccionCabecera
                );
            try
            {
                SmtpClient MyMail = new SmtpClient();
                MailMessage MyMsg = new MailMessage();
                MyMail.Host = "smtp.gmail.com";
                MyMsg.Priority = MailPriority.Normal;
                MyMail.EnableSsl = true;
                MyMsg.To.Add(new MailAddress(info.EmailAsesor));
                MyMsg.Subject = "Observación Implementada";
                MyMsg.SubjectEncoding = Encoding.UTF8;
                MyMsg.IsBodyHtml = true;

                var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
                string user = "", pass = "";
                foreach (var item in credenciales.Info)
                {
                    switch (item.Key)
                    {
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                    }
                }

                MyMsg.From = new MailAddress(user, @"Sistema de Gestión Ambiental");
                MyMsg.BodyEncoding = Encoding.UTF8;
                MyMsg.Body = msgBody;
                MyMail.UseDefaultCredentials = false;
                NetworkCredential MyCredentials = new NetworkCredential(user, pass);
                MyMail.Credentials = MyCredentials;
                MyMail.EnableSsl = true;
                //await MyMail.SendMailAsync(MyMsg);
                MyMsg.Attachments.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            var IdEncriptado = _protector.Protect(info.IdInspeccionCabecera.ToString());
            return RedirectToAction("ImplementarInspeccion", "InspeccionesImplementacion", new { IdCabecera = IdEncriptado });
        }
        public async Task<IActionResult> NotificarObservacionRechazada(string IdObservacion)
        {
            var IdObsDesencriptado = Convert.ToInt32(_protector.Unprotect(IdObservacion));
            var info = await _getObservacionRechazada.Execute(IdObsDesencriptado);
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
                info.IdInspeccionCabecera
                );
            try
            {
                SmtpClient MyMail = new SmtpClient();
                MailMessage MyMsg = new MailMessage();
                MyMail.Host = "smtp.gmail.com";
                MyMsg.Priority = MailPriority.Normal;
                MyMail.EnableSsl = true;
                MyMsg.To.Add(new MailAddress(info.EmailResponsableMarcobre));
                MyMsg.Subject = "Observación Rechazada";
                MyMsg.SubjectEncoding = Encoding.UTF8;
                MyMsg.IsBodyHtml = true;

                var credenciales = await _info_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
                string user = "", pass = "";
                foreach (var item in credenciales.Info)
                {
                    switch (item.Key)
                    {
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                    }
                }

                MyMsg.From = new MailAddress(user, @"Sistema de Gestión Ambiental");
                MyMsg.BodyEncoding = Encoding.UTF8;
                MyMsg.Body = msgBody;
                MyMail.UseDefaultCredentials = false;
                NetworkCredential MyCredentials = new NetworkCredential(user, pass);
                MyMail.Credentials = MyCredentials;
                //await MyMail.SendMailAsync(MyMsg);
                MyMsg.Attachments.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            var IdEncriptado = _protector.Protect(info.IdInspeccionCabecera.ToString());
            return RedirectToAction("VerificarInspeccion", "InspeccionesVerificacionMA", new { IdCabecera = IdEncriptado });
        }
    }
}