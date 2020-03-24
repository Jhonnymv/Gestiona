using System;
using System.Collections.Generic;
using System.Linq;
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
                return RedirectToAction("NoAutorizado", "Home");
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

                        paginador += "<ul class=\"pagination\">";
                        for (int i = 1; i <= nPaginas; i++)
                        {
                            if (i == pagina)
                                paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                            else
                                paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                        }
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
                return RedirectToAction("NoAutorizado", "Home");
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home");
                else
                {
                    ViewBag.Rol = userInfo.Rol.ToString();

                    var info = (Tipo_Info_SGA)Enum.ToObject(typeof(Tipo_Info_SGA), IdInfo);
                    var data = await _getInfo_SGA.Execute(info);
                    var infoJson = "";
                    infoJson += "{";
                    foreach (var item in data.Info)
                    {
                        infoJson += "\"" + item.Key + "\"";
                        infoJson += ":\"" + item.Value + "\",";

                    }
                    infoJson = infoJson.Remove(infoJson.Length - 1);
                    infoJson += "}";
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
                        await _editarInfo_SGA.Execute(model.IdInfo, model.Tipo, model.Info);
                    }
                    return RedirectToAction("DetalleInfo_SGA", new { IdInfo = model.IdInfo });
                }
            }
        }
    }
}