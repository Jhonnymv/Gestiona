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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Empresa_Gerencia_TipoCondicion
{
    public class GestorEGTCController : Controller
    {

        private IMemoryCache _cache;

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

        public GestorEGTCController(IMemoryCache cache,IGetEmpresas getEmpresas, IGetGerencias getGerencias, IGetTiposCondicion getTiposCondicion, IGetEmpresa getEmpresa, IGetGerencia getGerencia, IGetTipoCondicion getTipoCondicion, IEditarEmpresa editarEmpresa, IEditarGerencia editarGerencia, IEditarTipoCondicion editarTipoCondicion, IAgregarEmpresa agregarEmpresa, IAgregarGerencia agregarGerencia, IAgregarTipoCondicion agregarTipoCondicion)
        {
            _cache = cache;
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

        public IActionResult EGTCs()
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Empresas(string filtro, int pagina, int rpp)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
                {
                    return RedirectToAction("NoAutorizado", "Home");
                }
                else
                {
                    var data = await _getEmpresas.Execute(filtro, pagina, rpp);
                    var filasTabla = "";
                    var paginador = "";
                    int nPaginas = data.nEmpresas % rpp != 0 ? (data.nEmpresas / rpp) + 1 : data.nEmpresas / rpp;
                    if (data.nEmpresas > 0)
                    {
                        foreach (var empresa in data.Empresas)
                        {

                            filasTabla += "<tr>";
                            filasTabla += "<td>" + empresa.RazonSocial + "</td>";
                            filasTabla += "<td><a href=\"/GestorEGTC/DetalleEmpresa?IdEmpresa=" + empresa.IdEmpresa + "\">Detalle</a></td>";
                            filasTabla += "</tr>";
                        }

                        paginador += "<ul class=\"pagination\">";
                        for (int i = 1; i <= nPaginas; i++)
                        {
                            if (i == pagina)
                                paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrarEmpresas(" + i + ")\">" + i + "</a></li>";
                            else
                                paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrarEmpresas(" + i + ")\">" + i + "</a></li>";
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
        public async Task<IActionResult> DetalleEmpresa(int IdEmpresa)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                var data = await _getEmpresa.Execute(IdEmpresa);
                var model = new EmpresaModel();
                model.IdEmpresa = data.IdEmpresa;
                model.RazonSocial = data.RazonSocial;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarEmpresa(EmpresaModel model)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await _editarEmpresa.Execute(model.IdEmpresa, model.RazonSocial);
                }
                return RedirectToAction("DetalleEmpresa", new { IdEmpresa = model.IdEmpresa });
            }
        }

        public IActionResult AgregarEmpresa()
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarEmpresa(EmpresaModel model)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
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
        [HttpPost]
        public async Task<IActionResult> Gerencias(string filtro, int pagina, int rpp)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var data = await _getGerencias.Execute(filtro, pagina, rpp);
                var filasTabla = "";
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

                    paginador += "<ul class=\"pagination\">";
                    for (int i = 1; i <= nPaginas; i++)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrarGerencias(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrarGerencias(" + i + ")\">" + i + "</a></li>";
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
        public async Task<IActionResult> DetalleGerencia(int IdGerencia)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                var data = await _getGerencia.Execute(IdGerencia);
                var model = new GerenciaModel();
                model.IdGerencia = data.IdGerencia;
                model.Descripcion = data.Descripcion;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarGerencia(GerenciaModel model)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await _editarGerencia.Execute(model.IdGerencia, model.Descripcion);
                }
                return RedirectToAction("DetalleGerencia", new { IdGerencia = model.IdGerencia });
            }
        }
        public IActionResult AgregarGerencia()
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                   _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarGerencia(GerenciaModel model)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
                {
                    return RedirectToAction("NoAutorizado", "Home");
                }
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
        public async Task<IActionResult> TiposCondicion(string filtro, int pagina, int rpp)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var data = await _getTiposCondicion.Execute(filtro, pagina, rpp);
                var filasTabla = "";
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

                    paginador += "<ul class=\"pagination\">";
                    for (int i = 1; i <= nPaginas; i++)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrarTipos(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrarTipos(" + i + ")\">" + i + "</a></li>";
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
        public async Task<IActionResult> DetalleTipoCondicion(int IdTipoCondicion)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                var data = await _getTipoCondicion.Execute(IdTipoCondicion);
                var grupos = new List<SelectListItem>
            {
                new SelectListItem("--SELECCIONE--", "", true),
                new SelectListItem("Acto Subéstándar MAP",Domain.Enum.TiposCondicion.ASA.ToString(), false),
                new SelectListItem("Acto Subéstándar SSO",Domain.Enum.TiposCondicion.ASS.ToString(), false),
                new SelectListItem("Condición Subéstándar MAP",Domain.Enum.TiposCondicion.CSA.ToString(), false),
                new SelectListItem("Condición Subéstándar SSO",Domain.Enum.TiposCondicion.CSS.ToString(), false),
            };
                ViewBag.Grupos = grupos;
                var model = new TipoCondicionModel();
                model.IdTipoCondicion = data.IdTipoCondicion;
                model.Descripcion = data.Descripcion;
                model.Grupo = data.Grupo;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarTipoCondicion(TipoCondicionModel model)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    await _editarTipoCondicion.Execute(model.IdTipoCondicion, model.Descripcion, model.Grupo);
                }
                return RedirectToAction("DetalleTipoCondicion", new { IdTipoCondicion = model.IdTipoCondicion });
            }
        }
        public IActionResult AgregarTipoCondicion()
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                ViewBag.Rol = _cache.Get<InspeccionRol>(CacheKeys.RolUsuario).ToString();
                var grupos = new List<SelectListItem>
            {
                new SelectListItem("--SELECCIONE--", "", true),
                new SelectListItem("Acto Subéstándar MAP",Domain.Enum.TiposCondicion.ASA.ToString(), false),
                new SelectListItem("Acto Subéstándar SSO",Domain.Enum.TiposCondicion.ASS.ToString(), false),
                new SelectListItem("Condición Subéstándar MAP",Domain.Enum.TiposCondicion.CSA.ToString(), false),
                new SelectListItem("Condición Subéstándar SSO",Domain.Enum.TiposCondicion.CSS.ToString(), false),
            };
                ViewBag.Grupos = grupos;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarTipoCondicion(TipoCondicionModel model)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0 ||
                _cache.Get<InspeccionRol>(CacheKeys.RolUsuario) != InspeccionRol.Admin)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
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