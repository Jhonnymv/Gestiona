using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.AgregarCategoriaTipo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.EditarCategoriaTipo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.GetCategoriasTipos;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.GetCategoriaTipo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.AgregarTipoDocumento;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.EditarTipoDocumento;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.GetTipoDocumento;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.GetTiposDocumento;
using GestionAmbiental.Application.UseCases.SGA_Documentos.Items;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.GestorTablasMaestras.SGA_Documentos.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        private readonly ISelectItems _selectItems;

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
            ISelectItems selectItems,
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
            _selectItems = selectItems;
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
                    filasTabla += "<td><a href=\"/GestorSGADocumentos/DetalleTipoDocumento?IdTipoDocumento=" + fila.IdSGATipoDocumento + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoDocumento(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

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
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoDocumento(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
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
                model.RutaFormato = data.Formato!=null? data.Formato.Replace("\\","--"):null;
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
                {
                    var rutaDB = Path.Combine("DocTemplates", "SGA", model.Formato.FileName);
                    var rutaArchivo = Path.Combine(_hostingEnvironment.WebRootPath, rutaDB);

                    using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await model.Formato.CopyToAsync(fileStream);
                    }

                    await _editarTipoDocumento.Execute(model.IdSGATipoDocumento, model.Descripcion, model.Codigo, rutaDB, model.Estado);

                    if (model.RutaFormato != null && System.IO.File.Exists(Path.Combine(_hostingEnvironment.WebRootPath, model.RutaFormato)))
                    {
                        System.IO.File.Delete(Path.Combine(_hostingEnvironment.WebRootPath, model.RutaFormato));
                    }
                }
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
                        var rutaDB = Path.Combine("DocTemplates", "SGA", model.Formato.FileName);
                        var rutaArchivo = Path.Combine(_hostingEnvironment.WebRootPath, rutaDB);                        

                        using (var fileStream = new FileStream(rutaArchivo, FileMode.Create))
                        {
                            await model.Formato.CopyToAsync(fileStream);
                        }

                        var idTipoDocumento = await _agregarTipoDocumento.Execute(model.Descripcion, model.Codigo, rutaDB);
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
                    filasTabla += "<td><a href=\"/GestorSGADocumentos/DetalleCategoriaTipo?IdCategoriaTipo=" + fila.IdSGACategoriaTipo + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarCategoriaTipo(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

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
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarCategoriaTipo(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
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

                var items = await _selectItems.Execute();

                var tiposDocumento = items.ItemsTipoDocumento.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdSGATipoDocumento.ToString(), Text = g.Descripcion }).ToList();
                tiposDocumento.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var categoriasDocumento = items.ItemsCategoriaDocumento.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_SGA_Categoria_Documento.ToString(), Text = g.Descripcion }).ToList();
                categoriasDocumento.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.TiposDoc = tiposDocumento;
                ViewBag.CategoriasDoc = categoriasDocumento;

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

                var items = await _selectItems.Execute();

                var tiposDocumento = items.ItemsTipoDocumento.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdSGATipoDocumento.ToString(), Text = g.Descripcion }).ToList();
                tiposDocumento.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                var categoriasDocumento = items.ItemsCategoriaDocumento.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_SGA_Categoria_Documento.ToString(), Text = g.Descripcion }).ToList();
                categoriasDocumento.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.TiposDoc = tiposDocumento;
                ViewBag.CategoriasDoc = categoriasDocumento;

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
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }
        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"}
            };
        }
        public async Task<IActionResult> DescargarFormato(string path)
        {
            try
            {
                var completePath = _hostingEnvironment.WebRootPath + "\\" + path.Replace("--", "\\");
                FileStream stream = new FileStream(completePath, FileMode.Open);
                return File(stream, GetContentType(path), Path.GetFileName(path.Replace("--", "\\")));
            }catch(Exception e)
            {
                return Ok();
            }
        }
    }
}
