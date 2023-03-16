using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.EtapaCMCB.DetalleRegistro;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.EtapaCMCB.EditarRegistro;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.EtapaCMCB.NuevoRegistro;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.CompromisosMCB.EtapaCMCB.ListarRegistros;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GestionAmbiental.UseCases.GestorTablasMaestras.CompromisosMCB.Model;
using GestionAmbiental.Domain.Enum;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.CompromisosMCB
{
    public class GestorCompromisosMCBController : Controller
    {
        private readonly IListarRegistro _listarEtapas;
        private readonly IDetalleRegistro _detalleEtapa;
        private readonly IEditarRegistro _editarEtapa;
        private readonly INuevoRegistro _nuevaEtapa;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public GestorCompromisosMCBController(IListarRegistro listarEtapas, IDetalleRegistro detalleEtapa, IEditarRegistro editarEtapa, INuevoRegistro nuevaEtapa, IWebHostEnvironment hostingEnvironment)
        {
            _listarEtapas = listarEtapas;
            _detalleEtapa = detalleEtapa;
            _editarEtapa = editarEtapa;
            _nuevaEtapa = nuevaEtapa;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Resumen()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                return View();
            }
        }

        //Tipo Avistamiento
        [HttpPost]
        public async Task<IActionResult> FiltrarEtapas(string filtro, int pagina, int rpp)
        {
            var data = await _listarEtapas.Execute(filtro, pagina, rpp);

            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NRegistros % rpp != 0 ? (data.NRegistros / rpp) + 1 : data.NRegistros / rpp;
            if (data.NRegistros > 0)
            {
                foreach (var fila in data.Registros)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Etapa + "</td>";
                    filasTabla += "<td><a href=\"/GestorCompromisosMCB/DetalleEtapa?Id=" + fila.Id + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEtapa(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarEtapa(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEtapa(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEtapa(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }

            tabla += filasTabla;
            tabla += "</tbody>";
            tabla += "</table>";
            dynamic response = new
            {
                Tabla = tabla,
                Paginador = paginador
            };
            return Ok(response);

        }
        public async Task<IActionResult> DetalleEtapa(int Id)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?Id=" + Id });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _detalleEtapa.Execute(Id);
                var model = new EtapaModel();
                model.Id = data.Id;
                model.Etapa = data.Etapa;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarEtapa(EtapaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                if (ModelState.IsValid)
                    await _editarEtapa.Execute(model.Id, model.Etapa);
                return RedirectToAction("DetalleEtapa", new { Id = model.Id });
            }
        }
        public IActionResult AgregarEtapa()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarEtapa(EtapaModel model)
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
                        var id = await _nuevaEtapa.Execute(model.Etapa);
                        return RedirectToAction("DetalleEtapa", new { Id = id });
                    }
                    return RedirectToAction("AgregarEtapa");
                }
            }
        }
    }
}