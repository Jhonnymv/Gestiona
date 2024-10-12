using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.AgregarNombreComun;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.EditarNombreComun;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.GetNombreComun;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.NombreComun.GetNombresComunes;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion.AgregarMiembroComiteInspeccion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion.EditarMiembroComiteInspeccion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion.GetMiembroComiteInspeccion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.MiembroComiteInspeccion.GetMiembrosComiteInspeccion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion.AgregarTipoInspeccion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion.EditarTipoInspeccion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion.GetTipoInspeccion;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Inspecciones.TipoInspeccion.GetTiposInspeccion;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.GestorTablasMaestras.AvistamientoFauna.Model;
using GestionAmbiental.UseCases.GestorTablasMaestras.Inspecciones.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.Inspecciones
{
    public class GestorInspeccionesController : Controller
    {
        private readonly IAgregarTipoInspeccion _agregarTipoInspeccion;
        private readonly IEditarTipoInspeccion _editarTipoInspeccion;
        private readonly IGetTipoInspeccion _getTipoInspeccion;
        private readonly IGetTiposInspeccion _getTiposInspeccion;

        private readonly IAgregarMiembroComiteInspeccion _agregarMiembroComiteInspeccion;
        private readonly IEditarMiembroComiteInspeccion _editarMiembroComiteInspeccion;
        private readonly IGetMiembroComiteInspeccion _getMiembroComiteInspeccion;
        private readonly IGetMiembrosComiteInspeccion _getMiembrosComiteInspeccion;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public GestorInspeccionesController(
            IAgregarTipoInspeccion agregarTipoInspeccion,
            IEditarTipoInspeccion editarTipoInspeccion,
            IGetTiposInspeccion getTiposInspeccion,
            IGetTipoInspeccion getTipoInspeccion,

            IAgregarMiembroComiteInspeccion agregarMiembroComiteInspeccion,
            IEditarMiembroComiteInspeccion editarMiembroComiteInspeccion,
            IGetMiembroComiteInspeccion getMiembroComiteInspeccion,
            IGetMiembrosComiteInspeccion getMiembrosComiteInspeccion,

            IWebHostEnvironment hostingEnvironment)
        {
            _agregarTipoInspeccion = agregarTipoInspeccion;
            _editarTipoInspeccion = editarTipoInspeccion; 
            _getTipoInspeccion = getTipoInspeccion; 
            _getTiposInspeccion = getTiposInspeccion;

            _agregarMiembroComiteInspeccion = agregarMiembroComiteInspeccion;
            _editarMiembroComiteInspeccion = editarMiembroComiteInspeccion;
            _getMiembroComiteInspeccion = getMiembroComiteInspeccion;
            _getMiembrosComiteInspeccion = getMiembrosComiteInspeccion;

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

        //Tipo Inspecciones
        [HttpPost]
        public async Task<IActionResult> FiltrarTipoInspeccion(string filtro, int pagina, int rpp)
        {
            var data = await _getTiposInspeccion.Execute(filtro, pagina, rpp);
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
            int nPaginas = data.NTiposInspeccion % rpp != 0 ? (data.NTiposInspeccion / rpp) + 1 : data.NTiposInspeccion / rpp;
            if (data.NTiposInspeccion > 0)
            {
                foreach (var fila in data.TiposInspeccion)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td><a href=\"/GestorInspecciones/DetalleTipoInspeccion?IdTipoInspeccion=" + fila.IdTipoInspeccion + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoInspeccion(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarTipoInspeccion(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoInspeccion(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarTipoInspeccion(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
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
        public async Task<IActionResult> DetalleTipoInspeccion(int IdTipoInspeccion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdTipoInspeccion=" + IdTipoInspeccion });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getTipoInspeccion.Execute(IdTipoInspeccion);
                var model = new TipoInspeccionModel();
                model.IdTipoInspeccion = data.IdTipoInspeccion;
                model.Descripcion = data.Descripcion;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarTipoInspeccion(TipoInspeccionModel model)
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
                    await _editarTipoInspeccion.Execute(model.IdTipoInspeccion, model.Descripcion);                    
                return RedirectToAction("DetalleTipoInspeccion", new { IdTipoInspeccion = model.IdTipoInspeccion });
            }
        }
        public IActionResult AgregarTipoInspeccion()
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
        public async Task<IActionResult> AgregarTipoInspeccion(TipoInspeccionModel model)
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
                        await _agregarTipoInspeccion.Execute(model.Descripcion);                        
                    }
                    return RedirectToAction("AgregarTipoInspeccion");
                }
            }
        }

        //MiembroComite
        [HttpPost]
        public async Task<IActionResult> FiltrarMiembroComite(string filtro, int pagina, int rpp)
        {
            var data = await _getMiembrosComiteInspeccion.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Nombres</th>";
            tabla += "<th>Apellidos</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NMiembrosInspeccion % rpp != 0 ? (data.NMiembrosInspeccion / rpp) + 1 : data.NMiembrosInspeccion / rpp;
            if (data.NMiembrosInspeccion > 0)
            {
                foreach (var fila in data.MiembrosComiteInspeccion)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Nombres + "</td>";
                    filasTabla += "<td>" + fila.Apellidos + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorInspecciones/DetalleMiembroComite?IdMiembroComite=" + fila.IdMiembroComite + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarMiembroComite(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarMiembroComite(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarMiembroComite(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarMiembroComite(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
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
        public async Task<IActionResult> DetalleMiembroComite(int IdMiembroComite)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdMiembroComite=" + IdMiembroComite });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var data = await _getMiembroComiteInspeccion.Execute(IdMiembroComite);
                var model = new MiembroComiteModel();
                model.IdMiembroComite = data.IdMiembroComite;
                model.Nombres = data.Nombres;
                model.Apellidos = data.Apellidos;
                model.Estado = data.Estado;
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditarMiembroComite(MiembroComiteModel model)
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
                    await _editarMiembroComiteInspeccion.Execute(model.IdMiembroComite, model.Nombres,model.Apellidos, model.Estado);
                return RedirectToAction("DetalleMiembroComite", new { IdMiembroComite = model.IdMiembroComite });
            }
        }
        public IActionResult AgregarMiembroComite()
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
        public async Task<IActionResult> AgregarMiembroComite(MiembroComiteModel model)
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
                        await _agregarMiembroComiteInspeccion.Execute(model.Nombres, model.Apellidos);
                    }
                    return RedirectToAction("AgregarMiembroComite");
                }
            }
        }
    }
}
