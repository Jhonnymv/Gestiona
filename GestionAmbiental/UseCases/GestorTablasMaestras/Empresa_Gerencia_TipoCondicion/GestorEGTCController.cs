using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa.AgregarEmpresa;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa.EditarEmpresa;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa.GetEmpresa;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa.GetEmpresas;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gerencia.AgregarGerencia;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gerencia.EditarGerencia;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gerencia.GetGerencia;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gerencia.GetGerencias;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion.AgregarTipoCondicion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion.EditarTipoCondicion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion.GetTipoCondicion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion.GetTiposCondicion;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Empresa_Gerencia_TipoCondicion
{
    public class GestorEGTCController : Controller
    {
        private readonly IGetEmpresas _getEmpresas;
        private readonly IGetGerencias _getGerencias;
        private readonly IGetTiposCondicion _getTiposCondicion;

        private readonly IGetEmpresa _getEmpresa;
        private readonly IGetGerencia _getGerencia;
        private readonly IGetTipoCondicion _getTipoCondicion;

        private readonly IEditarEmpresa _editarEmpresa;
        private readonly IEditarGerencia _editarGerencia;
        private readonly IEditarTipoCondicion _editarTipoCondicion;

        private readonly IAgregarEmpresa _agregarEmpresa;
        private readonly IAgregarGerencia _agregarGerencia;
        private readonly IAgregarTipoCondicion _agregarTipoCondicion;

        public GestorEGTCController(IGetEmpresas getEmpresas, IGetGerencias getGerencias, IGetTiposCondicion getTiposCondicion, IGetEmpresa getEmpresa, IGetGerencia getGerencia, IGetTipoCondicion getTipoCondicion, IEditarEmpresa editarEmpresa, IEditarGerencia editarGerencia, IEditarTipoCondicion editarTipoCondicion, IAgregarEmpresa agregarEmpresa, IAgregarGerencia agregarGerencia, IAgregarTipoCondicion agregarTipoCondicion)
        {
            _getEmpresas = getEmpresas;
            _getGerencias = getGerencias;
            _getTiposCondicion = getTiposCondicion;
            _getEmpresa = getEmpresa;
            _getGerencia = getGerencia;
            _getTipoCondicion = getTipoCondicion;
            _editarEmpresa = editarEmpresa;
            _editarGerencia = editarGerencia;
            _editarTipoCondicion = editarTipoCondicion;
            _agregarEmpresa = agregarEmpresa;
            _agregarGerencia = agregarGerencia;
            _agregarTipoCondicion = agregarTipoCondicion;
        }

        public IActionResult Resumen()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
                else
                {
                    ViewBag.Rol = userInfo.Rol.ToString();
                    return View();
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> FiltrarEmpresa(string filtro, int pagina, int rpp)
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
                    var data = await _getEmpresas.Execute(filtro, pagina, rpp);
                    var filasTabla = "<table class=\"table\">";
                    filasTabla += "<thead class=\"thead-mcolor\">";
                    filasTabla += "<tr>";
                    filasTabla += "<th>Razón Social</th>";
                    filasTabla += "<th>Estado</th>";
                    filasTabla += "<th></th>";
                    filasTabla += "</tr>";
                    filasTabla += "</thead>";
                    filasTabla += "<tbody>";
                    var paginador = "";
                    int nPaginas = data.nEmpresas % rpp != 0 ? (data.nEmpresas / rpp) + 1 : data.nEmpresas / rpp;
                    if (data.nEmpresas > 0)
                    {
                        foreach (var empresa in data.Empresas)
                        {

                            filasTabla += "<tr>";
                            filasTabla += "<td>" + empresa.RazonSocial + "</td>";
                            filasTabla += "<td>" + empresa.Estado + "</td>";
                            filasTabla += "<td><a href=\"/GestorEGTC/DetalleEmpresa?IdEmpresa=" + empresa.IdEmpresa + "\">Detalle</a></td>";
                            filasTabla += "</tr>";
                        }

                        paginador += "<ul class=\"pagination pagination-sm\">";
                        if (pagina >= 4 && nPaginas > 5)
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEmpresa(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                        for (int i = pagina - 2; i <= pagina + 2; i++)
                        {
                            if (i > 0 && i <= nPaginas)
                            {
                                if (i == pagina)
                                    paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarEmpresa(" + i + ")\">" + i + "</a></li>";
                                else
                                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEmpresa(" + i + ")\">" + i + "</a></li>";
                            }
                        }
                        if (pagina <= nPaginas - 4 && nPaginas > 5)
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEmpresa(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                        paginador += "</ul>";

                    }
                    else
                    {
                        filasTabla = "Sin datos";
                    }
                    filasTabla += "</tbody>";
                    filasTabla += "</table>";
                    dynamic response = new
                    {
                        Tabla = filasTabla,
                        Paginador = paginador
                    };
                    return Ok(response);
                }
            }
        }
        public async Task<IActionResult> DetalleEmpresa(int IdEmpresa)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdEmpresa=" + IdEmpresa });
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdEmpresa=" + IdEmpresa });
                else
                {
                    ViewBag.Rol = userInfo.Rol.ToString();
                    var data = await _getEmpresa.Execute(IdEmpresa);
                    var model = new EmpresaModel();
                    model.IdEmpresa = data.IdEmpresa;
                    model.RazonSocial = data.RazonSocial;
                    model.Estado = data.Estado;
                    return View(model);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarEmpresa(EmpresaModel model)
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
                        await _editarEmpresa.Execute(model.IdEmpresa, model.RazonSocial,model.Estado);
                    }
                    return RedirectToAction("DetalleEmpresa", new { IdEmpresa = model.IdEmpresa });
                }
            }
        }

        public IActionResult AgregarEmpresa()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
                else
                {
                    ViewBag.Rol = userInfo.Rol.ToString();
                    return View();
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarEmpresa(EmpresaModel model)
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
                        var idEmpresa = await _agregarEmpresa.Execute(model.RazonSocial);
                        return RedirectToAction("DetalleEmpresa", new { IdEmpresa = idEmpresa });
                    }
                    return RedirectToAction("AgregarEmpresa");
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> FiltrarGerencia(string filtro, int pagina, int rpp)
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
                    var data = await _getGerencias.Execute(filtro, pagina, rpp);
                    var filasTabla = "<table class=\"table\">";
                    filasTabla += "<thead class=\"thead-mcolor\">";
                    filasTabla += "<tr>";
                    filasTabla += "<th>Gerencia</th>";
                    filasTabla += "<th></th>";
                    filasTabla += "</tr>";
                    filasTabla += "</thead>";
                    filasTabla += "<tbody>";
                    var paginador = "";
                    int nPaginas = data.nGerencias % rpp != 0 ? (data.nGerencias / rpp) + 1 : data.nGerencias / rpp;
                    if (data.nGerencias > 0)
                    {
                        foreach (var empresa in data.Gerencias)
                        {
                            filasTabla += "<tr>";
                            filasTabla += "<td>" + empresa.Descripcion + "</td>";
                            filasTabla += "<td><a href=\"/GestorEGTC/DetalleGerencia?IdGerencia=" + empresa.IdGerencia + "\">Detalle</a></td>";
                            filasTabla += "</tr>";
                        }


                        paginador += "<ul class=\"pagination pagination-sm\">";
                        if (pagina >= 4 && nPaginas > 5)
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarGerencia(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                        for (int i = pagina - 2; i <= pagina + 2; i++)
                        {
                            if (i > 0 && i <= nPaginas)
                            {
                                if (i == pagina)
                                    paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarGerencia(" + i + ")\">" + i + "</a></li>";
                                else
                                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarGerencia(" + i + ")\">" + i + "</a></li>";
                            }
                        }
                        if (pagina <= nPaginas - 4 && nPaginas > 5)
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarGerencia(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                        paginador += "</ul>";
                    }
                    else
                    {
                        filasTabla = "Sin datos";
                    }
                    filasTabla += "</tbody>";
                    filasTabla += "</table>";
                    dynamic response = new
                    {
                        Tabla = filasTabla,
                        Paginador = paginador
                    };
                    return Ok(response);
                }
            }
        }
        public async Task<IActionResult> DetalleGerencia(int IdGerencia)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdGerencia=" + IdGerencia });
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdGerencia=" + IdGerencia });
                else
                {
                    ViewBag.Rol = userInfo.Rol.ToString();
                    var data = await _getGerencia.Execute(IdGerencia);
                    var model = new GerenciaModel();
                    model.IdGerencia = data.IdGerencia;
                    model.Descripcion = data.Descripcion;
                    return View(model);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarGerencia(GerenciaModel model)
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
                        await _editarGerencia.Execute(model.IdGerencia, model.Descripcion);
                    }
                    return RedirectToAction("DetalleGerencia", new { IdGerencia = model.IdGerencia });
                }
            }
        }
        public IActionResult AgregarGerencia()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
                else
                {
                    ViewBag.Rol = userInfo.Rol.ToString();
                    return View();
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarGerencia(GerenciaModel model)
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
                        var idGerencia = await _agregarGerencia.Execute(model.Descripcion);
                        return RedirectToAction("DetalleGerencia", new { IdGerencia = idGerencia });
                    }
                    return RedirectToAction("AgregarGerencia");
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> FiltrarTipoCondicion(string filtro, int pagina, int rpp)
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
                    var data = await _getTiposCondicion.Execute(filtro, pagina, rpp);
                    var filasTabla = "<table class=\"table\">";
                    filasTabla += "<thead class=\"thead-mcolor\">";
                    filasTabla += "<tr>";
                    filasTabla += "<th>Tipo Acto/Condición Subestándar</th>";
                    filasTabla += "<th>Clasificación</th>";
                    filasTabla += "<th></th>";
                    filasTabla += "</tr>";
                    filasTabla += "</thead>";
                    filasTabla += "<tbody>";
                    var paginador = "";
                    int nPaginas = data.nTiposCondicion % rpp != 0 ? (data.nTiposCondicion / rpp) + 1 : data.nTiposCondicion / rpp;
                    if (data.nTiposCondicion > 0)
                    {
                        foreach (var gerencia in data.TiposCondicion)
                        {
                            filasTabla += "<tr>";
                            filasTabla += "<td>" + gerencia.Descripcion + "</td>";
                            filasTabla += "<td>" + gerencia.Grupo + "</td>";
                            filasTabla += "<td><a href=\"/GestorEGTC/DetalleTipoCondicion?IdTipoCondicion=" + gerencia.IdTipoCondicion + "\">Detalle</a></td>";
                            filasTabla += "</tr>";
                        }

                        paginador += "<ul class=\"pagination pagination-sm\">";
                        if (pagina >= 4 && nPaginas > 5)
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoCondicion(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                        for (int i = pagina - 2; i <= pagina + 2; i++)
                        {
                            if (i > 0 && i <= nPaginas)
                            {
                                if (i == pagina)
                                    paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarTipoCondicion(" + i + ")\">" + i + "</a></li>";
                                else
                                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoCondicion(" + i + ")\">" + i + "</a></li>";
                            }
                        }
                        if (pagina <= nPaginas - 4 && nPaginas > 5)
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoCondicion(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                        paginador += "</ul>";
                    }
                    else
                    {
                        filasTabla = "Sin datos";
                    }
                    filasTabla += "</tbody>";
                    filasTabla += "</table>";
                    dynamic response = new
                    {
                        Tabla = filasTabla,
                        Paginador = paginador
                    };
                    return Ok(response);
                }
            }
        }
        public async Task<IActionResult> DetalleTipoCondicion(int IdTipoCondicion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdTipoCondicion=" + IdTipoCondicion });
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdTipoCondicion=" + IdTipoCondicion });
                else
                {
                    ViewBag.Rol = userInfo.Rol.ToString();
                    var data = await _getTipoCondicion.Execute(IdTipoCondicion);
                    var grupos = new List<SelectListItem>
                    {
                        new SelectListItem("--SELECCIONE--", "", true),
                        new SelectListItem("Acto Subestándar MAP",Domain.Enum.TiposCondicion.ASA.ToString(), false),
                        new SelectListItem("Acto Subestándar SSO",Domain.Enum.TiposCondicion.ASS.ToString(), false),
                        new SelectListItem("Condición Subestándar MAP",Domain.Enum.TiposCondicion.CSA.ToString(), false),
                        new SelectListItem("Condición Subestándar SSO",Domain.Enum.TiposCondicion.CSS.ToString(), false),
                    };
                    ViewBag.Grupos = grupos;
                    var model = new TipoCondicionModel();
                    model.IdTipoCondicion = data.IdTipoCondicion;
                    model.Descripcion = data.Descripcion;
                    model.Grupo = data.Grupo;
                    return View(model);
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarTipoCondicion(TipoCondicionModel model)
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
                        await _editarTipoCondicion.Execute(model.IdTipoCondicion, model.Descripcion, model.Grupo);
                    }
                    return RedirectToAction("DetalleTipoCondicion", new { IdTipoCondicion = model.IdTipoCondicion });
                }
            }
        }
        public IActionResult AgregarTipoCondicion()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName  });
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                if (!userInfo.Rol.Equals(InspeccionRol.Admin))
                    return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
                else
                {
                    ViewBag.Rol = userInfo.Rol.ToString();
                    var grupos = new List<SelectListItem>
                    {
                        new SelectListItem("--SELECCIONE--", "", true),
                        new SelectListItem("Acto Subestándar MAP",Domain.Enum.TiposCondicion.ASA.ToString(), false),
                        new SelectListItem("Acto Subestándar SSO",Domain.Enum.TiposCondicion.ASS.ToString(), false),
                        new SelectListItem("Condición Subestándar MAP",Domain.Enum.TiposCondicion.CSA.ToString(), false),
                        new SelectListItem("Condición Subestándar SSO",Domain.Enum.TiposCondicion.CSS.ToString(), false),
                    };
                    ViewBag.Grupos = grupos;
                    return View();
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarTipoCondicion(TipoCondicionModel model)
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
                        var idTipoCondición = await _agregarTipoCondicion.Execute(model.Descripcion, model.Grupo);
                        return RedirectToAction("DetalleTipoCondicion", new { IdTipoCondicion = idTipoCondición });
                    }
                    return RedirectToAction("AgregarTipoCondicion");
                }
            }
        }
    }
}