using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.AgregarACR_CMRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.EditarACR_CMRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.GetACR_CMRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.GetACR_CMRSs;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.AgregarMotivo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.EditarMotivo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.GetMotivo;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.GetMotivos;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.AgregarResiduoSolido;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.EditarResiduoSolido;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.GetResiduoSolido;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ResiduoSolido.GetResiduosSolidos;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.AgregarResponsable_ACR_CMRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.EditarResponsable_ACR_CMRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.GetResponsable_ACR_CMRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.GetResponsables_ACR_CMRS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ResiduosSolidos.ControlIngresoSalida
{
    public class GestorIngresoSalidaRSController : Controller
    {

        //ACR_CMRS
        private readonly IAgregarACR_CMRS _agregarACR_CMRS;
        private readonly IEditarACR_CMRS _editarACR_CMRS;
        private readonly IGetACR_CMRS _getACR_CMRS;
        private readonly IGetACR_CMRSs _getACR_CMRSs;

        //MotivoIngresoSalida
        private readonly IAgregarMotivo _agregarMotivo;
        private readonly IEditarMotivo _editarMotivo;
        private readonly IGetMotivo _getMotivo;
        private readonly IGetMotivos _getMotivos;

        //Tipo Residuo
        private readonly IAgregarResiduoSolido _agregarResiduoSolido;
        private readonly IEditarResiduoSolido _editarResiduoSolido;
        private readonly IGetResiduoSolido _getResiduoSolido;
        private readonly IGetResiduosSolidos _getResiduosSolidos;

        //Responsable ACR_CMRS
        private readonly IAgregarResponsableACR_CMRS _agregarResponsableACR_CMRS;
        private readonly IEditarResponsableARC_CMRS _editarResponsableARC_CMRS;
        private readonly IGetResponsableACR_CMRS _getResponsableACR_CMRS;
        private readonly IGetResponsablesACR_CMRS _getResponsablesACR_CMRS;

        public GestorIngresoSalidaRSController(IAgregarACR_CMRS agregarACR_CMRS, IEditarACR_CMRS editarACR_CMRS, IGetACR_CMRS getACR_CMRS, IGetACR_CMRSs getACR_CMRSs, IAgregarMotivo agregarMotivo, IEditarMotivo editarMotivo, IGetMotivo getMotivo, IGetMotivos getMotivos, IAgregarResiduoSolido agregarResiduoSolido, IEditarResiduoSolido editarResiduoSolido, IGetResiduoSolido getResiduoSolido, IGetResiduosSolidos getResiduosSolidos, IAgregarResponsableACR_CMRS agregarResponsableACR_CMRS, IEditarResponsableARC_CMRS editarResponsableARC_CMRS, IGetResponsableACR_CMRS getResponsableACR_CMRS, IGetResponsablesACR_CMRS getResponsablesACR_CMRS)
        {
            _agregarACR_CMRS = agregarACR_CMRS;
            _editarACR_CMRS = editarACR_CMRS;
            _getACR_CMRS = getACR_CMRS;
            _getACR_CMRSs = getACR_CMRSs;
            _agregarMotivo = agregarMotivo;
            _editarMotivo = editarMotivo;
            _getMotivo = getMotivo;
            _getMotivos = getMotivos;
            _agregarResiduoSolido = agregarResiduoSolido;
            _editarResiduoSolido = editarResiduoSolido;
            _getResiduoSolido = getResiduoSolido;
            _getResiduosSolidos = getResiduosSolidos;
            _agregarResponsableACR_CMRS = agregarResponsableACR_CMRS;
            _editarResponsableARC_CMRS = editarResponsableARC_CMRS;
            _getResponsableACR_CMRS = getResponsableACR_CMRS;
            _getResponsablesACR_CMRS = getResponsablesACR_CMRS;
        }

        public IActionResult Resumen()
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
        public async Task<IActionResult> FiltrarACR_CMRS(string filtro, int pagina, int rpp)
        {
            var data = await _getACR_CMRSs.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla+="<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>ACR/CMRS</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.nACR_CMRSs % rpp != 0 ? (data.nACR_CMRSs / rpp) + 1 : data.nACR_CMRSs / rpp;
            if (data.nACR_CMRSs > 0)
            {
                foreach (var fila in data.ACR_CMRSs)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIngresoSalidaRS/DetalleACR_CMRS?IdACR_CMRS=" + fila.Id_ACR_CMRS +"\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarACR_CMRS(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarACR_CMRS(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarACR_CMRS(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarACR_CMRS(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
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
        public async Task<IActionResult> DetalleACR_CMRS(int IdACR_CMRS)
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
                var acr_cmrs = await _getACR_CMRS.Execute(IdACR_CMRS);
                var model = new ACR_CMRSModel();
                model.Id_ACR_CMRS = acr_cmrs.Id_ACR_CMRS;
                model.Descripcion = acr_cmrs.Descripcion;
                model.Estado = acr_cmrs.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarACR_CMRS(ACR_CMRSModel model)
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
                    await _editarACR_CMRS.Execute(model.Id_ACR_CMRS, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleACR_CMRS", new { IdACR_CMRS = model.Id_ACR_CMRS });
            }
        }
        public async Task<IActionResult> AgregarACR_CMRS()
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
        public async Task<IActionResult> AgregarACR_CMRS(ACR_CMRSModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarACR_CMRS.Execute(model.Descripcion);
                return RedirectToAction("DetalleACR_CMRS", new { IdACR_CMRS = id });
            }
            return RedirectToAction("AgregarACR_CMRS");
        }
        public async Task<IActionResult> FiltrarMotivo(string filtro, int pagina, int rpp)
        {
            var data = await _getMotivos.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla+="<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NMotivos % rpp != 0 ? (data.NMotivos / rpp) + 1 : data.NMotivos / rpp;
            if (data.NMotivos > 0)
            {
                foreach (var fila in data.Motivos)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIngresoSalidaRS/DetalleMotivo?IdMotivo=" + fila.Id_Motivo + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarMotivo(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarMotivo(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarMotivo(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarMotivo(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
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
        public async Task<IActionResult> DetalleMotivo(int IdMotivo)
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
                var motivo = await _getMotivo.Execute(IdMotivo);
                var model = new MotivoModel();
                model.Id_Motivo = motivo.Id_Motivo;
                model.Descripcion = motivo.Descripcion;
                model.Estado = motivo.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarMotivo(MotivoModel model)
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
                    await _editarMotivo.Execute(model.Id_Motivo, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleMotivo", new { IdMotivo = model.Id_Motivo });
            }
        }
        public async Task<IActionResult> AgregarMotivo()
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
        public async Task<IActionResult> AgregarMotivo(MotivoModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarMotivo.Execute(model.Descripcion);
                return RedirectToAction("DetalleMotivo", new { IdMotivo = id });
            }
            return RedirectToAction("AgregarMotivo");
        }
        public async Task<IActionResult> FiltrarResiduosSolidos(string filtro, int pagina, int rpp)
        {
            var data = await _getResiduosSolidos.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla+="<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Descripción</th>";
            tabla += "<th>Tipo de Residuo</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NResiduos % rpp != 0 ? (data.NResiduos / rpp) + 1 : data.NResiduos / rpp;
            if (data.NResiduos > 0)
            {
                foreach (var fila in data.ResiduosSolidos)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Descripcion + "</td>";
                    filasTabla += "<td>" + fila.Tipo_Residuo + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIngresoSalidaRS/DetalleResiduosSolidos?IdResiduoSolido=" + fila.Id_Residuo_Solido + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarResiduosSolidos(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarResiduosSolidos(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarResiduosSolidos(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarResiduosSolidos(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
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
        public async Task<IActionResult> DetalleResiduosSolidos(int IdResiduoSolido)
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
                var residuo_solido = await _getResiduoSolido.Execute(IdResiduoSolido);
                var model = new ResiduosSolidosModel();
                model.Id_Residuo_Solido = residuo_solido.Id_Residuo_Solido;
                model.Id_Tipo_Residuo = residuo_solido.Id_Tipo_Residuo;
                model.Descripcion = residuo_solido.Descripcion;
                model.Estado = residuo_solido.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarResiduosSolidos(ResiduosSolidosModel model)
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
                    await _editarResiduoSolido.Execute(model.Id_Residuo_Solido, model.Id_Tipo_Residuo, model.Descripcion, model.Estado);
                return RedirectToAction("DetalleResiduosSolidos", new { IdResiduoSolido = model.Id_Residuo_Solido });
            }
        }
        public async Task<IActionResult> AgregarResiduosSolidos()
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
        public async Task<IActionResult> AgregarResiduosSolidos(ResiduosSolidosModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarResiduoSolido.Execute(model.Id_Tipo_Residuo,model.Descripcion);
                return RedirectToAction("DetalleResiduosSolidos", new { IdResiduoSolido = id });
            }
            return RedirectToAction("AgregarResiduosSolidos");
        }
        public async Task<IActionResult> FiltrarResponsablesACR_CRMS(string filtro, int pagina, int rpp)
        {
            var data = await _getResponsablesACR_CMRS.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla+="<thead class=\"thead-mcolor\">";
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
            int nPaginas = data.NResponsables % rpp != 0 ? (data.NResponsables / rpp) + 1 : data.NResponsables / rpp;
            if (data.NResponsables > 0)
            {
                foreach (var fila in data.responsables)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Nombres + "</td>";
                    filasTabla += "<td>" + fila.Apellidos + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorIngresoSalidaRS/DetalleResponsablesACR_CRMS?IdResponsable=" + fila.Id_Responsable_ACR_CRMS + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarResponsablesACR_CRMS(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarResponsablesACR_CRMS(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarResponsablesACR_CRMS(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarResponsablesACR_CRMS(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
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
        public async Task<IActionResult> DetalleResponsablesACR_CRMS(int IdResponsable)
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

                var responsable = await _getResponsableACR_CMRS.Execute(IdResponsable);
                var model = new ResponsablesACR_CRMSModel();
                model.Id_Responsable_ACR_CRMS = responsable.Id_Responsable_ACR_CRMS;
                model.Nombres = responsable.Nombres;
                model.Apellidos = responsable.Apellidos;
                model.Estado = responsable.Estado;
                return View(model);
            }
        }
        public async Task<IActionResult> EditarResponsablesACR_CRMS(ResponsablesACR_CRMSModel model)
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
                    await _editarResponsableARC_CMRS.Execute(model.Id_Responsable_ACR_CRMS, model.Nombres, model.Apellidos, model.Estado);
                return RedirectToAction("DetalleResponsablesACR_CRMS", new { IdResponsable = model.Id_Responsable_ACR_CRMS });
            }
        }
        public async Task<IActionResult> AgregarResponsablesACR_CRMS()
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
        public async Task<IActionResult> AgregarResponsablesACR_CRMS(ResponsablesACR_CRMSModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarResponsableACR_CMRS.Execute(model.Nombres,model.Apellidos);
                return RedirectToAction("DetalleResponsablesACR_CRMS", new { IdResponsable = id });
            }
            return RedirectToAction("AgregarResponsablesACR_CRMS");
        }
    }
}