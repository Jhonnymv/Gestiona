using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.AgregarCategoriaTipo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.EditarCategoriaTipo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.GetCategoriasTipos;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.GetCategoriaTipo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.AgregarTipoDocumento;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.EditarTipoDocumento;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.GetTipoDocumento;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.GetTiposDocumento;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.GestorTablasMaestras.SGA_Documentos.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.SGA_Documentos
{
    public class GestorSGADocumentosController : Controller
    {
        private readonly IAgregarCategoriaTipo _agregarCategoriaTipo;
        private readonly IEditarCategoriaTipo _editarCategoriaTipo;
        private readonly IGetCategoriasTipo _getCategoriasTipo;
        private readonly IGetCategoriaTipo _getCategoriaTipo;

        private readonly IAgregarTipoDocumento _agregarTipoDocumento;
        private readonly IEditarTipoDocumento _editarTipoDocumento;
        private readonly IGetTipoDocumento _getTipoDocumento;
        private readonly IGetTiposDocumento _getTiposDocumento;

        private readonly IHostingEnvironment _hostingEnvironment;

        public GestorSGADocumentosController(
            IAgregarCategoriaTipo agregarCategoriaTipo,
            IEditarCategoriaTipo editarCategoriaTipo,
            IGetCategoriasTipo getCategoriasTipo,
            IGetCategoriaTipo getCategoriaTipo,
            IAgregarTipoDocumento agregarTipoDocumento,
            IEditarTipoDocumento editarTipoDocumento,
            IGetTipoDocumento getTipoDocumento,
            IGetTiposDocumento getTiposDocumento,
            IHostingEnvironment hostingEnvironment)
        {
            _agregarCategoriaTipo = agregarCategoriaTipo;
            _editarCategoriaTipo = editarCategoriaTipo;
            _getCategoriasTipo = getCategoriasTipo;
            _getCategoriaTipo = getCategoriaTipo;
            _agregarTipoDocumento = agregarTipoDocumento;
            _editarTipoDocumento = editarTipoDocumento;
            _getTipoDocumento = getTipoDocumento;
            _getTiposDocumento = getTiposDocumento;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Resumen()
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
                return View();
            }
        }

        //Tipo Documento
        [HttpPost]
        public async Task<IActionResult> FiltrarTipoDocumento(string filtro, int pagina, int rpp)
        {
            var data = await _getTiposDocumento.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripcion</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NTipos % rpp != 0 ? (data.NTipos / rpp) + 1 : data.NTipos / rpp;
            if (data.NTipos > 0)
            {
                foreach (var fila in data.TiposDocumento)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorAvistamientoFauna/DetalleTipoDocumento?IdTipoDocumento=" + fila.IdSGATipoDocumento + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarTipoDocumento(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarTipoDocumento(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoDocumento(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarTipoDocumento(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
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
        public async Task<IActionResult> DetalleTipoDocumento(int IdTipoDocumento)
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
                var data = await _getTipoDocumento.Execute(IdTipoDocumento);
                var model = new TipoDocumentoModel();
                model.IdSGATipoDocumento = data.IdSGATipoDocumento;
                model.Descripcion = data.Descripcion;
                model.Codigo = data.Codigo;
                model.Formato = data.Formato;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarTipoDocumento(TipoDocumentoModel model)
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
                    await _editarTipoDocumento.Execute(model.IdSGATipoDocumento, model.Descripcion, model.Codigo,model.Formato, model.Estado);
                return RedirectToAction("DetalleTipoDocumento", new { IdTipoDocumento = model.IdSGATipoDocumento });
            }
        }
        public async Task<IActionResult> AgregarTipoDocumento()
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

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarTipoDocumento(TipoDocumentoModel model)
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
                        var idTipoDocumento = await _agregarTipoDocumento.Execute(model.Descripcion, model.Codigo,model.Formato);
                        return RedirectToAction("DetalleTipoDocumento", new { IdTipoDocumento = idTipoDocumento });
                    }
                    return RedirectToAction("AgregarTipoDocumento");
                }
            }
        }
        //Categoria Tipo
        [HttpPost]
        public async Task<IActionResult> FiltrarCategoriaTipo(string filtro, int pagina, int rpp)
        {
            var data = await _getCategoriasTipo.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Categoria</th>";
            tabla += "<th>Tipo de Documento</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NCategorias % rpp != 0 ? (data.NCategorias / rpp) + 1 : data.NCategorias / rpp;
            if (data.NCategorias > 0)
            {
                foreach (var fila in data.CategoriasTipo)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.SGACategoriaDocumento + "</td>";
                    filasTabla += "<td>" + fila.SGATipoDocumento + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorAvistamientoFauna/DetalleCategoriaTipo?IdCategoriaTipo=" + fila.IdSGACategoriaTipo + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarCategoriaTipo(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarCategoriaTipo(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarCategoriaTipo(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarCategoriaTipo(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
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
        public async Task<IActionResult> DetalleCategoriaTipo(int IdCategoriaTipo)
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
                var data = await _getCategoriaTipo.Execute(IdCategoriaTipo);
                var model = new CategoriaTipoModel();
                model.IdSGACategoriaTipo = data.IdSGACategoriaTipo;
                model.IdCategoriaDocumento = data.IdSGACategoriaDocumento;
                model.CategoriaDocumento = data.SGACategoriaDocumento;
                model.IdTipoDocumento = data.IdSGATipoDocumento;
                model.TipoDocumento = data.SGATipoDocumento;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarCategoriaTipo(CategoriaTipoModel model)
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
                    await _editarCategoriaTipo.Execute(model.IdSGACategoriaTipo, model.IdCategoriaDocumento, model.IdTipoDocumento, model.Estado);
                return RedirectToAction("DetalleCategoriaTipo", new { IdCategoriaTipo = model.IdSGACategoriaTipo });
            }
        }
        public async Task<IActionResult> AgregarCategoriaTipo()
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

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarCategoriaTipo(CategoriaTipoModel model)
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
                        var idCategoriaTipo = await _agregarCategoriaTipo.Execute(model.IdCategoriaDocumento, model.IdTipoDocumento);
                        return RedirectToAction("DetalleCategoriaTipo", new { IdCategoriaTipo = idCategoriaTipo });
                    }
                    return RedirectToAction("AgregarCategoriaTipo");
                }
            }
        }
    }
}
