using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna.AgregarEstadoFauna;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna.EditarEstadoFauna;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna.GetEstadoFauna;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.EstadoFauna.GetEstadosFauna;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.AgregarNombreComun;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.EditarNombreComun;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.GetNombreComun;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.GetNombresComunes;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento.AgregarTipoAvistamiento;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento.EditarTipoAvistamiento;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento.GetTipoAvistamiento;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoAvistamiento.GetTiposAvistamiento;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna.AgregarTipoFauna;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna.EditarTipoFauna;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna.GetTipoFauna;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.TipoFauna.GetTiposFauna;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.GestorTablasMaestras.AvistamientoFauna.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.AvistamientoFauna
{
    public class GestorAvistamientoFaunaController : Controller
    {
        private readonly IAgregarNombreComun _agregarNombreComun;
        private readonly IEditarNombreComun _editarNombreComun;
        private readonly IGetNombreComun _getNombreComun;
        private readonly IGetNombresComunes _getNombresComunes;

        private readonly IAgregarTipoFauna _agregarTipoFauna;
        private readonly IEditarTipoFauna _editarTipoFauna;
        private readonly IGetTipoFauna _getTipoFauna;
        private readonly IGetTiposFauna _getTiposFauna;

        private readonly IAgregarEstadoFauna _agregarEstadoFauna;
        private readonly IEditarEstadoFauna _editarEstadoFauna;
        private readonly IGetEstadoFauna _getEstadoFauna;
        private readonly IGetEstadosFauna _getEstadosFauna;

        private readonly IAgregarTipoAvistamiento _agregarTipoAvistamiento;
        private readonly IEditarTipoAvistamiento _editarTipoAvistamiento;
        private readonly IGetTipoAvistamiento _getTipoAvistamiento;
        private readonly IGetTiposAvistamiento _getTiposAvistamiento;

        private readonly IHostingEnvironment _hostingEnvironment;

        public GestorAvistamientoFaunaController(
            IAgregarNombreComun agregarNombreComun, 
            IEditarNombreComun editarNombreComun,
            IGetNombreComun getNombreComun, 
            IGetNombresComunes getNombresComunes, 
            IAgregarTipoFauna agregarTipoFauna, 
            IEditarTipoFauna editarTipoFauna,
            IGetTipoFauna getTipoFauna,
            IGetTiposFauna getTiposFauna,
            IAgregarEstadoFauna agregarEstadoFauna,
            IEditarEstadoFauna editarEstadoFauna,
            IGetEstadoFauna getEstadoFauna,
            IGetEstadosFauna getEstadosFauna,
            IAgregarTipoAvistamiento agregarTipoAvistamiento,
            IEditarTipoAvistamiento editarTipoAvistamiento,
            IGetTipoAvistamiento getTipoAvistamiento,
            IGetTiposAvistamiento getTiposAvistamiento,
            IHostingEnvironment hostingEnvironment)
        {
            _agregarNombreComun = agregarNombreComun;
            _editarNombreComun = editarNombreComun;
            _getNombreComun = getNombreComun;
            _getNombresComunes = getNombresComunes;
            _agregarTipoFauna = agregarTipoFauna;
            _editarTipoFauna = editarTipoFauna;
            _getTipoFauna = getTipoFauna;
            _getTiposFauna = getTiposFauna;
            _agregarEstadoFauna = agregarEstadoFauna;
            _editarEstadoFauna = editarEstadoFauna;
            _getEstadoFauna = getEstadoFauna;
            _getEstadosFauna = getEstadosFauna;
            _agregarTipoAvistamiento = agregarTipoAvistamiento;
            _editarTipoAvistamiento = editarTipoAvistamiento;
            _getTipoAvistamiento = getTipoAvistamiento;
            _getTiposAvistamiento = getTiposAvistamiento;
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

        //Tipo Fauna
        [HttpPost]
        public async Task<IActionResult> FiltrarTipoFauna(string filtro, int pagina, int rpp)
        {
            var data = await _getTiposFauna.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NTiposFauna % rpp != 0 ? (data.NTiposFauna / rpp) + 1 : data.NTiposFauna / rpp;
            if (data.NTiposFauna > 0)
            {
                foreach (var fila in data.TiposFauna)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorAvistamientoFauna/DetalleTipoFauna?IdTipoFauna=" + fila.IdTipoFauna + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarTipoFauna(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarTipoFauna(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoFauna(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarTipoFauna(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
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
        public async Task<IActionResult> DetalleTipoFauna(int IdTipoFauna)
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
                var data = await _getTipoFauna.Execute(IdTipoFauna);
                var model = new TipoFaunaModel();
                model.IdTipoFauna = data.IdTipoFauna;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarTipoFauna(TipoFaunaModel model)
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
                    await _editarTipoFauna.Execute(model.IdTipoFauna, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleTipoFauna", new { IdTipoFauna = model.IdTipoFauna });
            }
        }
        public async Task<IActionResult> AgregarTipoFauna()
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
        public async Task<IActionResult> AgregarTipoFauna(TipoFaunaModel model)
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
                        var idTipoFauna = await _agregarTipoFauna.Execute(model.Descripcion);
                        return RedirectToAction("DetalleTipoFauna", new { IdTipoFauna = idTipoFauna });
                    }
                    return RedirectToAction("AgregarTipoFauna");
                }
            }
        }

        //Nombre Comun
        [HttpPost]
        public async Task<IActionResult> FiltrarNombreComun(string filtro, int pagina, int rpp)
        {
            var data = await _getNombresComunes.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Tipo de Fauna</th>";
            tabla += "<th>Foto</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NNombresComunes % rpp != 0 ? (data.NNombresComunes / rpp) + 1 : data.NNombresComunes / rpp;
            if (data.NNombresComunes > 0)
            {
                foreach (var fila in data.NombresComunes)
                {
                    filasTabla += "<tr style=\"vertical-align:middle;\">";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.Descripcion + "</td>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.TipoFauna + "</td>";
                    filasTabla += "<td style=\"width:150px;\"><img src=\""+Path.Combine(_hostingEnvironment.WebRootPath,fila.Foto)+"\" style =\"width:100%;height:auto\"/></td>";
                    filasTabla += "<td style=\"vertical-align:middle;\">" + fila.Estado + "</td>";
                    filasTabla += "<td style=\"vertical-align:middle;\"><a href=\"/GestorAvistamientoFauna/DetalleNombreComun?IdNombreComun=" + fila.IdNombreComunFauna + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarNombreComun(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarNombreComun(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarNombreComun(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarNombreComun(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
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
        public async Task<IActionResult> DetalleNombreComun(int IdNombreComun)
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

                var tiposFauna = await _getTiposFauna.Execute();
               
                var tiposFaunaSelectList = tiposFauna.TiposFauna.Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();
                tiposFaunaSelectList.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.TiposFauna = tiposFaunaSelectList;

                var data = await _getNombreComun.Execute(IdNombreComun);
                var model = new NombreComunModel();
                model.IdNombreComun = data.IdNombreComunFauna;
                model.IdTipoFauna = data.IdTipoFauna;
                model.Descripcion = data.Descripcion;
                model.RutaFoto = data.Foto;
                model.Estado = data.Estado;

                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarNombreComun(NombreComunModel model)
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
                {
                    if (model.Foto != null && model.Foto.Length > 0)
                    {
                        var antiguaImagen = Path.Combine(_hostingEnvironment.WebRootPath, model.RutaFoto.Substring(1));
                        if (System.IO.File.Exists(antiguaImagen))
                        {
                            System.IO.File.Delete(antiguaImagen);
                        }
                        var tipoFauna = await _getTipoFauna.Execute(model.IdTipoFauna);

                        var extension = Path.GetExtension(model.Foto.FileName);

                        var rutaNuevoArchivo = CargarImagen(model.Foto, model.Descripcion, tipoFauna.Descripcion, extension).Result;
                        await _editarNombreComun.Execute(model.IdNombreComun, model.Descripcion, model.IdTipoFauna, rutaNuevoArchivo, model.Estado);
                    }
                    else
                    {
                        await _editarNombreComun.Execute(model.IdNombreComun, model.Descripcion, model.IdTipoFauna, model.RutaFoto, model.Estado);
                    }

                }
                return RedirectToAction("DetalleNombreComun", new { IdNombreComun = model.IdNombreComun });
            }
        }
        public async Task<IActionResult> AgregarNombreComun()
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

                var tiposFauna = await _getTiposFauna.Execute();

                var tiposFaunaSelectList = tiposFauna.TiposFauna.Select(g => new SelectListItem() { Value = g.IdTipoFauna.ToString(), Text = g.Descripcion }).ToList();
                tiposFaunaSelectList.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.TiposFauna = tiposFaunaSelectList;


                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> AgregarNombreComun(NombreComunModel model)
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
                        if (model.Foto != null && model.Foto.Length > 0)
                        {
                            var tipoFauna = await _getTipoFauna.Execute(model.IdTipoFauna);

                            var extension = Path.GetExtension(model.Foto.FileName);

                            var rutaNuevoArchivo = CargarImagen(model.Foto, model.Descripcion, tipoFauna.Descripcion, extension).Result;

                            var idNombreComun = await _agregarNombreComun.Execute(model.Descripcion, model.IdTipoFauna, rutaNuevoArchivo);
                            return RedirectToAction("DetalleNombreComun", new { IdNombreComun = idNombreComun });
                        }
                    }
                    return RedirectToAction("AgregarNombreComun");
                }
            }
        }

        //Estado Fauna
        [HttpPost]
        public async Task<IActionResult> FiltrarEstadoFauna(string filtro, int pagina, int rpp)
        {
            var data = await _getEstadosFauna.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NEstadosFauna % rpp != 0 ? (data.NEstadosFauna / rpp) + 1 : data.NEstadosFauna / rpp;
            if (data.NEstadosFauna > 0)
            {
                foreach (var fila in data.EstadosFauna)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorAvistamientoFauna/DetalleEstadoFauna?IdEstadoFauna=" + fila.IdEstadoFauna + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarEstadoFauna(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarEstadoFauna(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEstadoFauna(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarEstadoFauna(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
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
        public async Task<IActionResult> DetalleEstadoFauna(int IdEstadoFauna)
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
                var data = await _getEstadoFauna.Execute(IdEstadoFauna);
                var model = new EstadoFaunaModel();
                model.IdEstadoFauna = data.IdEstadoFauna;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarEstadoFauna(EstadoFaunaModel model)
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
                    await _editarEstadoFauna.Execute(model.IdEstadoFauna, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleEstadoFauna", new { IdEstadoFauna = model.IdEstadoFauna });
            }
        }
        public async Task<IActionResult> AgregarEstadoFauna()
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
        public async Task<IActionResult> AgregarEstadoFauna(EstadoFaunaModel model)
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
                        var idEstadoFauna = await _agregarEstadoFauna.Execute(model.Descripcion);
                        return RedirectToAction("DetalleEstadoFauna", new { IdEstadoFauna = idEstadoFauna });
                    }
                    return RedirectToAction("AgregarEstadoFauna");
                }
            }
        }

        //Tipo Avistamiento
        [HttpPost]
        public async Task<IActionResult> FiltrarTipoAvistamiento(string filtro, int pagina, int rpp)
        {
            var data = await _getTiposAvistamiento.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NTiposAvistamiento % rpp != 0 ? (data.NTiposAvistamiento / rpp) + 1 : data.NTiposAvistamiento / rpp;
            if (data.NTiposAvistamiento > 0)
            {
                foreach (var fila in data.TiposAvistamiento)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorAvistamientoFauna/DetalleTipoAvistamiento?IdTipoAvistamiento=" + fila.IdTipoAvistamiento + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarTipoAvistamiento(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarTipoAvistamiento(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoAvistamiento(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"FiltrarTipoAvistamiento(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
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
        public async Task<IActionResult> DetalleTipoAvistamiento(int IdTipoAvistamiento)
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
                var data = await _getTipoAvistamiento.Execute(IdTipoAvistamiento);
                var model = new TipoAvistamientoModel();
                model.IdTipoAvistamiento = data.IdTipoAvistamiento;
                model.Descripcion = data.Descripcion;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarTipoAvistamiento(TipoAvistamientoModel model)
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
                    await _editarTipoAvistamiento.Execute(model.IdTipoAvistamiento, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleTipoAvistamiento", new { IdTipoAvistamiento = model.IdTipoAvistamiento });
            }
        }
        public async Task<IActionResult> AgregarTipoAvistamiento()
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
        public async Task<IActionResult> AgregarTipoAvistamiento(TipoAvistamientoModel model)
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
                        var idTipoAvistamiento = await _agregarTipoAvistamiento.Execute(model.Descripcion);
                        return RedirectToAction("DetalleTipoAvistamiento", new { IdTipoAvistamiento = idTipoAvistamiento });
                    }
                    return RedirectToAction("AgregarTipoAvistamiento");
                }
            }
        }

        public async Task<string> CargarImagen(IFormFile archivo, string nombreComun, string tipoFauna,string extension)
        {
            var nuevoNombre = CreateUniqueFileName(nombreComun,tipoFauna) + extension;

            var carpetaArchivo = Path.Combine(_hostingEnvironment.WebRootPath, "img", "uploads", "AvistamientoFauna", tipoFauna);
            if (!Directory.Exists(carpetaArchivo))
            {
                Directory.CreateDirectory(carpetaArchivo);
            }

            var rutaArchivo = Path.Combine(_hostingEnvironment.WebRootPath, "img", "uploads", "AvistamientoFauna", tipoFauna,nuevoNombre);

            using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
            {
                await archivo.CopyToAsync(fileStream);
                
                return "\\"+ Path.Combine("img", "uploads", "AvistamientoFauna", tipoFauna, nuevoNombre);
            }
        }
        private string CreateUniqueFileName(string nombreComun,string tipoFauna)
        {
            return nombreComun+"_"+tipoFauna + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        }
    }
}