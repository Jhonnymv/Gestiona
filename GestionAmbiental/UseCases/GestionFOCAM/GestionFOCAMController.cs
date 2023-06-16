using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.FOCAM.Tipo.ListarRegistros;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestionFOCAM
{
    public class GestionFOCAMController : Controller
    {
        private readonly Application.UseCases.FOCAM.Tipo.ListarRegistros.IListarRegistros _listarTipos;
        private readonly Application.UseCases.FOCAM.Documentos.NuevoRegistro.INuevoRegistro _nuevoRegitroDocumento;
        private readonly Application.UseCases.FOCAM.Documentos.ListarRegistros.IListarRegistros _listarDocumentos;

        public GestionFOCAMController(
           Application.UseCases.FOCAM.Tipo.ListarRegistros.IListarRegistros listarTipos,
           Application.UseCases.FOCAM.Documentos.NuevoRegistro.INuevoRegistro nuevoRegitroDocumento,
           Application.UseCases.FOCAM.Documentos.ListarRegistros.IListarRegistros listarDocumentos

            )
        {
            _listarTipos = listarTipos;
            _nuevoRegitroDocumento = nuevoRegitroDocumento;
            _listarDocumentos = listarDocumentos;
        }

        public async Task<IActionResult> Resumen()
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

        public async Task<IActionResult> NuevoRegistro()
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
            }
            else
            {
                var tipos = await _listarTipos.SelectItems();
                var tipoItems = tipos.Registros.OrderBy(x => x.Nombre).Select(g => new SelectListItem()
                {
                    Value = g.Id.ToString(),
                    Text = g.Nombre
                }).ToList();
                tipoItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));
                ViewBag.tipos = tipoItems;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevoRegistro(GestionFOCAMModel model)
        {
            //if (ModelState.IsValid)
            //{
            var id = await _nuevoRegitroDocumento.Execute(
                model.IdTipo,
                model.Codigo,
                model.Titulo,
                model.Version,
                Convert.ToDateTime(model.FechaAprobacion),
                model.Estado,
                model.Comentarios,
                Convert.ToDateTime(model.FechaTentaivaVersion),
                model.UrlDocumento
                );
            //}
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ListarDocumentos(int pagina, int rpp)
        {
            var datosTabla = await _listarDocumentos.Execute(pagina, rpp);
            var filasTabla = "";
            var paginador = "";

            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;

            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Registros)
                {
                    filasTabla += "<tr>";
                    //filasTabla += "<td><span>" + fila.Id + "</span></br>" +
                    //    "<a href=\"/CompromisosMCB/EditarRegistro?id=" + fila.Id + "\">" +
                    //    "<button type =\"button\" class=\"btn btn-mc\">Editar</button>" +
                    //    "</a>" +
                    //    "<button type =\"button\" class=\"btn btn-mc\" onclick=\"OpenModal(" + fila.Id + ")\">Detalle</button>" +
                    //    "</td>";
                    filasTabla += "<td>" + fila.Id + "</td>";
                    filasTabla += "<td>" + fila.NombreTipo + "</td>";
                    filasTabla += "<td>" + fila.Codigo + "</td>";
                    filasTabla += "<td>" + fila.Titulo + "</td>";
                    filasTabla += "<td>" + fila.Version + "</td>";
                    filasTabla += "<td>" + fila.FechaAprobacion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td>" + fila.Comentarios + "</td>";
                    filasTabla += "<td>" + fila.FechaTentaivaVersion + "</td>";
                    //filasTabla += "<td>" + fila.Texto_compromiso + "</td>";
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
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
                filasTabla += "</tr>";
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
