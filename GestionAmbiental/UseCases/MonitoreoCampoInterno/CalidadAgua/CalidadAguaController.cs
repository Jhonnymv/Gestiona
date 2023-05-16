using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using GestionAmbiental.Domain.Enum;
using ImageMagick;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Renci.SshNet;
using Serilog;

namespace GestionAmbiental.UseCases.MonitoreoCampoInterno.CalidadAgua
{
    public class CalidadAguaController : Controller
    {
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly Application.UseCases.MonitoreoCampoInterno.CalidadAgua.Items.ISelectItems _selectItems;
        private readonly Application.UseCases.MonitoreoCampoInterno.CalidadAgua.ListarRegistros.IListarRegistros _listarRegistros;
        private readonly Application.UseCases.MonitoreoCampoInterno.CalidadAgua.NuevoRegistro.INuevoRegistro _nuevoRegistro;

        public CalidadAguaController(
            IGetInfo_SGA info_SGA,
            IWebHostEnvironment webHostEnvironment,
            Application.UseCases.MonitoreoCampoInterno.CalidadAgua.Items.ISelectItems selectItems,
            Application.UseCases.MonitoreoCampoInterno.CalidadAgua.ListarRegistros.IListarRegistros listarRegistros,
            Application.UseCases.MonitoreoCampoInterno.CalidadAgua.NuevoRegistro.INuevoRegistro nuevoRegistro

            )
        {
            _info_SGA = info_SGA;
            _hostingEnvironment = webHostEnvironment;
            _selectItems = selectItems;
            _listarRegistros = listarRegistros;
            _nuevoRegistro = nuevoRegistro;

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

        [HttpPost]
        public async Task<IActionResult> listarRegistros(int pagina, int rpp)
        {
            var datosTabla = await _listarRegistros.Execute(pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;
            if (datosTabla.NRegistros > 0)
            {
                foreach (var fila in datosTabla.Registros)
                {
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"#\"></a>" +
                        "<button type =\"button\" class=\"btn btn-mc\" onclick=\"OpenModal(" + fila.Id + ")\">Detalle</button>" + " </td>";
                    //filasTabla += "<td><span>" + fila.Id + "</span></br>" + "</td>";
                    filasTabla += "<td>" + fila.Fecha.ToString().Split(" ")[0] + " - " + fila.Hora + "</td>";
                    filasTabla += "<td>" + fila.Cod_Estacion + "</td>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Ubicacion + "</td>";
                    filasTabla += "<td>" + fila.NombreResponsable + " " + fila.ApellidoResponsable + "</td>";
                    filasTabla += "<td>" + fila.TemperaturaResult + " (" + fila.TemperaturaEquipo + ")</td>";
                    filasTabla += "<td>" + fila.PhResult + " (" + fila.PhEquipo + ")</td>";
                    filasTabla += "<td>" + fila.ConductividadResult + " (" + fila.ConductividadEquipo + ")</td>";
                    filasTabla += "<td>" + fila.OxigenoResult + " (" + fila.OxigenoEquipo + ")</td>";
                    filasTabla += "<td>" + fila.TurbiedadResult + " (" + fila.TurbiedadEquipo + ")</td>";
                    filasTabla += "<td>" + fila.CloroResult + " (" + fila.CloroEquipo + ")</td>";
                    filasTabla += "</tr>";
                }
                //Paginador
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
        public async Task<IActionResult> NuevoRegistro()
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
                ViewBag.Fecha = DateTime.Now.Date.ToString("yyyy-MM-dd");
                ViewBag.Hora = DateTime.Now.ToString("hh:mm");

                var items = await _selectItems.Execute();
                var UsuariosItems = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres }).ToList();
                UsuariosItems.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

                ViewBag.Usuarios = UsuariosItems;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> NuevoRegistro(CalidadAguaModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _nuevoRegistro.Execute(model.Fecha, model.Hora, model.Cod_Estacion, model.Descripcion, model.Ubicacion, model.Id_Usuario, model.TemperaturaResult,
                    model.TemperaturaEquipo, model.PhResult, model.PhEquipo, model.ConductividadResult, model.ConductividadEquipo, model.OxigenoResult, model.OxigenoEquipo,
                    model.TurbiedadResult, model.TurbiedadEquipo, model.CloroResult, model.CloroEquipo);
                return RedirectToAction("Resumen");
            }
            return View();
        }

        public async Task<ActionResult> GetRegisterById(int id)
        {
            var datos = await _listarRegistros.BuscarPorId(id);

            dynamic response = new
            {
                Data = datos
            };
            return Ok(response);
        }

    }
}
