using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA.EditarInfo_SGA;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGAs;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Info_SGA
{
    public class GestorInfo_SGAController : Controller
    {
        private IGetInfo_SGAs _getInfo_SGAs;
        private IGetInfo_SGA _getInfo_SGA;
        private IEditarInfo_SGA _editarInfo_SGA;

        public GestorInfo_SGAController(IGetInfo_SGAs getInfo_SGAs, IGetInfo_SGA getInfo_SGA, IEditarInfo_SGA editarInfo_SGA)
        {
            _getInfo_SGAs = getInfo_SGAs;
            _getInfo_SGA = getInfo_SGA;
            _editarInfo_SGA = editarInfo_SGA;
        }

        public IActionResult Info_SGA()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home");
                else
                {
                    ViewBag.Rol = userInfo.Rol.ToString();
                    return View();
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> Info_SGA(string filtro, int pagina, int rpp)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home");
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home");
                else
                {
                    var data = await _getInfo_SGAs.Execute(filtro, pagina, rpp);
                    var filasTabla = "";
                    var paginador = "";
                    int nPaginas = data.nInfo_SGAs % rpp != 0 ? (data.nInfo_SGAs / rpp) + 1 : data.nInfo_SGAs / rpp;
                    if (data.nInfo_SGAs > 0)
                    {
                        foreach (var info in data.Info_SGAs)
                        {

                            filasTabla += "<tr>";
                            filasTabla += "<td>" + info.Tipo + "</td>";
                            filasTabla += "<td><a href=\"/GestorInfo_SGA/DetalleInfo_SGA?IdInfo=" + info.IdInfo + "\">Detalle</a></td>";
                            filasTabla += "</tr>";
                        }

                        paginador += "<ul class=\"pagination pagination-sm\">";
                        if (pagina >= 4 && nPaginas > 5)
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                        for (int i = pagina - 2; i <= pagina + 2; i++)
                        {
                            if (i > 0 && i <= nPaginas)
                            {
                                if (i == pagina)
                                    paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                                else
                                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                            }
                        }
                        if (pagina <= nPaginas - 4 && nPaginas > 5)
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                        paginador += "</ul>";
                    }
                    else
                    {
                        filasTabla = "Sin datos";
                    }
                    dynamic response = new
                    {
                        Data = filasTabla,
                        Paginador = paginador
                    };
                    return Ok(response);
                }
            }
        }
        public async Task<IActionResult> DetalleInfo_SGA(int IdInfo)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdInfo=" + IdInfo });
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdInfo=" + IdInfo });
                else
                {
                    ViewBag.Rol = userInfo.Rol.ToString();

                    var info = (Tipo_Info_SGA)Enum.ToObject(typeof(Tipo_Info_SGA), IdInfo);
                    if (info.Equals(Tipo_Info_SGA.LDAPConnection))
                    {
                        var data = await _getInfo_SGA.ExecuteLDAPConnection(info);
                        var infoJson = "";
                        infoJson = JsonConvert.SerializeObject(data);
                        //infoJson += "{";
                        //foreach (var item in data.Info)
                        //{
                        //    infoJson += "\"" + item.Key + "\"";
                        //    infoJson += ":\"" + item.Value + "\",";

                        //}
                        //infoJson = infoJson.Remove(infoJson.Length - 1);
                        //infoJson += "}";
                        var model = new Info_SGAModel
                        {
                            IdInfo = data.IdInfo,
                            Info = infoJson,
                            Tipo = data.Tipo
                        };
                        return View(model);
                    }
                    else
                    {
                        var data = await _getInfo_SGA.Execute(info);
                        var infoJson = "";
                        infoJson = JsonConvert.SerializeObject(data);
                        //infoJson += "{";
                        //foreach (var item in data.Info)
                        //{
                        //    infoJson += "\"" + item.Key + "\"";
                        //    infoJson += ":\"" + item.Value + "\",";

                        //}
                        //infoJson = infoJson.Remove(infoJson.Length - 1);
                        //infoJson += "}";
                        var model = new Info_SGAModel
                        {
                            IdInfo = data.IdInfo,
                            Info = infoJson,
                            Tipo = data.Tipo
                        };
                        return View(model);

                    }
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarInfo_SGA(Info_SGAModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home");
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home");
                else
                {
                    if (ModelState.IsValid)
                    {
                        var objSFTPInfo = JsonConvert.DeserializeObject<NewSFTPInfoObject>(model.Info);
                        var objNotificationInfo = JsonConvert.DeserializeObject<NewNotificationInfoObject>(model.Info);
                        var objLDAPInfo = JsonConvert.DeserializeObject<NewLDAPInfoObject>(model.Info);
                        var objEncriptionInfo = JsonConvert.DeserializeObject<NewEncriptionInfoObject>(model.Info);

                        if (objSFTPInfo.Info != null && objSFTPInfo.Info.host != null)
                            await _editarInfo_SGA.Execute(model.IdInfo, model.Tipo, JsonConvert.SerializeObject(objSFTPInfo.Info));
                        if (objSFTPInfo.Info != null && objNotificationInfo.Info.smtp != null)
                            await _editarInfo_SGA.Execute(model.IdInfo, model.Tipo, JsonConvert.SerializeObject(objNotificationInfo.Info));
                        if (objLDAPInfo.Path != null)
                            await _editarInfo_SGA.Execute(model.IdInfo, model.Tipo, JsonConvert.SerializeObject(objLDAPInfo));
                        if (objSFTPInfo.Info != null && objEncriptionInfo.Info.iv != null)
                            await _editarInfo_SGA.Execute(model.IdInfo, model.Tipo, JsonConvert.SerializeObject(objEncriptionInfo.Info));
                    }
                    return RedirectToAction("DetalleInfo_SGA", new { IdInfo = model.IdInfo });
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnviarCorreoPrueba(string email)
        {
            dynamic response;
            try
            {
                var credenciales = await _getInfo_SGA.Execute(Tipo_Info_SGA.NotificationAccount);
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

                msg.To.Add(new MailAddress(email));

                msg.Subject = "Mensaje Prueba Gestiona";
                msg.SubjectEncoding = Encoding.UTF8;
                msg.IsBodyHtml = true;
                msg.From = new MailAddress(mailFrom, @"Sistema de Gestión Ambiental");
                msg.BodyEncoding = Encoding.UTF8;
                msg.Body = "MENSAJE DE PRUEBA GESTIONA";

                await mail.SendMailAsync(msg);
                msg.Attachments.Dispose();
                mail.Dispose();
                msg.Dispose();

                response = new
                {
                    success = true,
                    msg = "Mensaje enviado"
                };
            }
            catch (Exception ex)
            {
                response = new
                {
                    success = true,
                    msg = "Error al enviar mensaje: " + ex.Message,
                    inner = JsonConvert.SerializeObject(ex.InnerException)
                };
            }
            return Ok(response);
        }

        private class NewSFTPInfoObject
        {
            public int IdInfo { get; set; }
            public NewSFTPInfo Info { get; set; }
            public string Tipo { get; set; }
        }
        private class NewSFTPInfo
        {
            public string host { get; set; }
            public string port { get; set; }
            public string directory { get; set; }
            public string user { get; set; }
            public string password { get; set; }
        }
        private class NewNotificationInfoObject
        {
            public int IdInfo { get; set; }
            public NewNotificationInfo Info { get; set; }
            public string Tipo { get; set; }
        }
        private class NewNotificationInfo
        {
            public string smtp { get; set; }
            public string port { get; set; }
            public string ntwkuser { get; set; }
            public string ntwkpassword { get; set; }
            public string mailfrom { get; set; }
        }
        private class NewEncriptionInfoObject
        {
            public int IdInfo { get; set; }
            public NewNEncriptionInfo Info { get; set; }
            public string Tipo { get; set; }
        }
        private class NewNEncriptionInfo
        {
            public string key { get; set; }
            public string iv { get; set; }
        }
        private class NewLDAPInfoObject
        {
            public int IdInfo { get; set; }
            public string Path { get; set; }
            public string Domain { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public string[] Groups { get; set; }
            public string Tipo { get; set; }
        }
    }
}