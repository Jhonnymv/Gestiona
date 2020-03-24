using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS.AgregarConductorRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS.EditarConductorRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS.GetConductoresRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS.GetConductorRS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS.AgregarEO_RS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS.EditarEO_RS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS.GetEO_RS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS.GetEO_RSs;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuarios;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_RS.AgregarUsuario_RS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_RS.EditarUsuario_RS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_RS.GetUsuario_RS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuarios_Notificar_RS.GetUsuarios_RS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS.AgregarVehiculo_RS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS.EditarVehiculo_RS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS.GetVehiculo_RS;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Vehiculo_RS.GetVehiculos_RS;
using GestionAmbiental.Application.UseCases.GestorUsuarios.DeleteUsuarios;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ResiduosSolidos.ControlManifiestoRS
{
    public class GestorManifiestoRSController : Controller
    {
        //EORS
        private readonly IAgregarEO_RS _agregarEO_RS;
        private readonly IEditarEO_RS _editarEO_RS;
        private readonly IGetEO_RS _getEO_RS;
        private readonly IGetEO_RSs _getEO_RSs;

        //Conductor
        private readonly IAgregarConductorRS _agregarConductorRS;
        private readonly IEditarConductorRS _editarConductorRS;
        private readonly IGetConductoresRS _getConductoresRS;
        private readonly IGetConductorRS _getConductorRS;

        //Vehiculo
        private readonly IAgregarVehiculo_RS _agregarVehiculo_RS;
        private readonly IEditarVehiculo_RS _editarVehiculo_RS;
        private readonly IGetVehiculo_RS _getVehiculo_RS;
        private readonly IGetVehiculos_RS _getVehiculos_RS;

        //Usuarios
        private readonly IAgregarUsuario_RS _agregarUsuario_RS;
        private readonly IEditarUsuario_RS _editarUsuario_RS;
        private readonly IGetUsuario_RS _getUsuario_RS;
        private readonly IGetUsuarios_RS _getUsuarios_RS;
        private readonly IGetUsuarios _getUsuarios;
        private readonly IDeleteUsuario _deleteUsuario;

        public GestorManifiestoRSController(IAgregarEO_RS agregarEO_RS, IEditarEO_RS editarEO_RS, IGetEO_RS getEO_RS, IGetEO_RSs getEO_RSs, IAgregarConductorRS agregarConductorRS, IEditarConductorRS editarConductorRS, IGetConductoresRS getConductoresRS, IGetConductorRS getConductorRS, IAgregarVehiculo_RS agregarVehiculo_RS, IEditarVehiculo_RS editarVehiculo_RS, IGetVehiculo_RS getVehiculo_RS, IGetVehiculos_RS getVehiculos_RS, IAgregarUsuario_RS agregarUsuario_RS, IEditarUsuario_RS editarUsuario_RS, IGetUsuario_RS getUsuario_RS, IGetUsuarios_RS getUsuarios_RS, IGetUsuarios getUsuarios, IDeleteUsuario deleteUsuarios)
        {
            _agregarEO_RS = agregarEO_RS;
            _editarEO_RS = editarEO_RS;
            _getEO_RS = getEO_RS;
            _getEO_RSs = getEO_RSs;
            _agregarConductorRS = agregarConductorRS;
            _editarConductorRS = editarConductorRS;
            _getConductoresRS = getConductoresRS;
            _getConductorRS = getConductorRS;
            _agregarVehiculo_RS = agregarVehiculo_RS;
            _editarVehiculo_RS = editarVehiculo_RS;
            _getVehiculo_RS = getVehiculo_RS;
            _getVehiculos_RS = getVehiculos_RS;
            _agregarUsuario_RS = agregarUsuario_RS;
            _editarUsuario_RS = editarUsuario_RS;
            _getUsuario_RS = getUsuario_RS;
            _getUsuarios_RS = getUsuarios_RS;
            _getUsuarios = getUsuarios;
            _deleteUsuario = deleteUsuarios;
        }

        public IActionResult Resumen()
        {
            return View();
        }
        public async Task<IActionResult> FiltrarEO_RS(string filtro, int pagina, int rpp)
        {
            var data = await _getEO_RSs.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Razón Social</th>";
            tabla += "<th>N° Registro</th>";
            tabla += "<th>Fecha de Vencimiento de Registro</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NEO_RSs % rpp != 0 ? (data.NEO_RSs / rpp) + 1 : data.NEO_RSs / rpp;
            if (data.NEO_RSs > 0)
            {
                foreach (var fila in data.EO_RSs)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Razon_Social + "</td>";
                    filasTabla += "<td>" + fila.Registro + "</td>";
                    filasTabla += "<td>" + fila.Fecha_Vencimiento_Registro.ToString("dd/MM/yyyy") + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorManifiestoRS/DetalleEO_RS?IdEO_RS=" + fila.Id_EO_RS + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                for (int i = 1; i <= nPaginas; i++)
                {
                    if (i == pagina)
                        paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarEO_RS(" + i + ")\">" + i + "</a></li>";
                    else
                        paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarEO_RS(" + i + ")\">" + i + "</a></li>";
                }
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
        public async Task<IActionResult> DetalleEO_RS(int IdEO_RS)
        {
            var EO_RS = await _getEO_RS.Execute(IdEO_RS);
            var model = new EO_RSModel();
            model.Id_EO_RS = EO_RS.Id_EO_RS;
            model.Razon_Social = EO_RS.Razon_Social;
            model.Registro = EO_RS.Registro;
            model.Fecha_Vencimiento_Registro = EO_RS.Fecha_Vencimiento_Registro.ToString("yyyy-MM-dd");
            model.Estado = EO_RS.Estado;
            return View(model);
        }
        public async Task<IActionResult> EditarEO_RS(EO_RSModel model)
        {
            if (ModelState.IsValid)
                await _editarEO_RS.Execute(model.Id_EO_RS,model.Razon_Social,model.Registro,model.Fecha_Vencimiento_Registro, model.Estado);
            return RedirectToAction("DetalleEO_RS", new { IdEO_RS = model.Id_EO_RS });
        }
        public async Task<IActionResult> AgregarEO_RS()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AgregarEO_RS(EO_RSModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarEO_RS.Execute(model.Razon_Social,model.Registro,model.Fecha_Vencimiento_Registro);
                return RedirectToAction("DetalleEO_RS", new { IdEO_RS = id });
            }
            return RedirectToAction("AgregarEO_RS");
        }
        public async Task<IActionResult> FiltrarConductor(string filtro, int pagina, int rpp)
        {
            var data = await _getConductoresRS.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Nombres</th>";
            tabla += "<th>Apellidos</th>";
            tabla += "<th>N° Licencia</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NConductores % rpp != 0 ? (data.NConductores / rpp) + 1 : data.NConductores / rpp;
            if (data.NConductores > 0)
            {
                foreach (var fila in data.Conductores)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Nombres + "</td>";
                    filasTabla += "<td>" + fila.Apellidos + "</td>";
                    filasTabla += "<td>" + fila.NLicencia + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorManifiestoRS/DetalleConductor?IdConductor=" + fila.Id_Conductor_RS + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                for (int i = 1; i <= nPaginas; i++)
                {
                    if (i == pagina)
                        paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarConductor(" + i + ")\">" + i + "</a></li>";
                    else
                        paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarConductor(" + i + ")\">" + i + "</a></li>";
                }
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
        public async Task<IActionResult> DetalleConductor(int IdConductor)
        {
            var Conductor = await _getConductorRS.Execute(IdConductor);
            var model = new ConductorModel();
            model.Id_Conductor_RS = Conductor.Id_Conductor_RS;
            model.Nombres = Conductor.Nombres;
            model.Apellidos = Conductor.Apellidos;
            model.NLicencia = Conductor.NLicencia;
            model.Estado = Conductor.Estado;
            return View(model);
        }
        public async Task<IActionResult> EditarConductor(ConductorModel model)
        {
            if (ModelState.IsValid)
                await _editarConductorRS.Execute(model.Id_Conductor_RS, model.Nombres, model.Apellidos, model.NLicencia, model.Estado);
            return RedirectToAction("DetalleConductor", new { IdConductor = model.Id_Conductor_RS });
        }
        public async Task<IActionResult> AgregarConductor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AgregarConductor(ConductorModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarConductorRS.Execute(model.Nombres, model.Apellidos, model.NLicencia);
                return RedirectToAction("DetalleConductor", new { IdConductor = id });
            }
            return RedirectToAction("AgregarConductor");
        }
        public async Task<IActionResult> FiltrarVehiculo(string filtro, int pagina, int rpp)
        {
            var data = await _getVehiculos_RS.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Nombres</th>";
            tabla += "<th>Apellidos</th>";
            tabla += "<th>N° Licencia</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.NVehiculos % rpp != 0 ? (data.NVehiculos / rpp) + 1 : data.NVehiculos / rpp;
            if (data.NVehiculos > 0)
            {
                foreach (var fila in data.Vehiculos)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Placa + "</td>";
                    filasTabla += "<td>" + fila.Capacidad + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorManifiestoRS/DetalleVehiculo?IdVehiculo=" + fila.Id_Vehiculo_RS + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                for (int i = 1; i <= nPaginas; i++)
                {
                    if (i == pagina)
                        paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarVehiculo(" + i + ")\">" + i + "</a></li>";
                    else
                        paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarVehiculo(" + i + ")\">" + i + "</a></li>";
                }
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
        public async Task<IActionResult> DetalleVehiculo(int IdVehiculo)
        {
            var Vehiculo = await _getVehiculo_RS.Execute(IdVehiculo);
            var model = new VehiculoModel();
            model.Id_Vehiculo_RS = Vehiculo.Id_Vehiculo_RS;
            model.Placa = Vehiculo.Placa;
            model.Capacidad = Vehiculo.Capacidad;
            model.Estado = Vehiculo.Estado;
            return View(model);
        }
        public async Task<IActionResult> EditarVehiculo(VehiculoModel model)
        {
            if (ModelState.IsValid)
                await _editarVehiculo_RS.Execute(model.Id_Vehiculo_RS,model.Placa,model.Capacidad, model.Estado);
            return RedirectToAction("DetalleVehiculo", new { IdVehiculo = model.Id_Vehiculo_RS });
        }
        public async Task<IActionResult> AgregarVehiculo()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AgregarVehiculo(VehiculoModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarVehiculo_RS.Execute(model.Placa, model.Capacidad);
                return RedirectToAction("DetalleVehiculo", new { IdVehiculo = id });
            }
            return RedirectToAction("AgregarVehiculo");
        }
        public async Task<IActionResult> FiltrarUsuario(string filtro, int pagina, int rpp)
        {
            var data = await _getUsuarios_RS.Execute(filtro, pagina, rpp);
            var tabla = "<table class=\"table\">";
            tabla += "<thead class=\"thead-mcolor\">";
            tabla += "<tr>";
            tabla += "<th>Nombres</th>";
            tabla += "<th>Apellidos</th>";
            tabla += "<th>Usuario de Red</th>";
            tabla += "<th>Email</th>";
            tabla += "<th>Rol</th>";
            tabla += "<th>Estado</th>";
            tabla += "<th></th>";
            tabla += "</tr>";
            tabla += "</thead>";
            tabla += "<tbody>";
            var filasTabla = "";
            var paginador = "";
            int nPaginas = data.nUsuarios % rpp != 0 ? (data.nUsuarios / rpp) + 1 : data.nUsuarios / rpp;
            if (data.nUsuarios > 0)
            {
                foreach (var fila in data.Usuarios)
                {

                    //var IdEncriptado = _protector.Protect(fila.IdCabecera.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td>" + fila.Nombres + "</td>";
                    filasTabla += "<td>" + fila.Apellidos + "</td>";
                    filasTabla += "<td>" + fila.UsuarioRed + "</td>";
                    filasTabla += "<td>" + fila.Email + "</td>";
                    filasTabla += "<td>" + fila.Rol + "</td>";
                    filasTabla += "<td>" + fila.Estado + "</td>";
                    filasTabla += "<td><a href=\"/GestorManifiestoRS/DetalleUsuario?IdUsuario=" + fila.IdUsuario + "\">Detalle</a></td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                for (int i = 1; i <= nPaginas; i++)
                {
                    if (i == pagina)
                        paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"FiltrarUsuario(" + i + ")\">" + i + "</a></li>";
                    else
                        paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"FiltrarUsuario(" + i + ")\">" + i + "</a></li>";
                }
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
        public async Task<IActionResult> DetalleUsuario(int IdUsuario)
        {

            var enumList = Enum.GetValues(typeof(RSRoles)).OfType<RSRoles>().ToList();

            var roles = enumList.Select(g => new SelectListItem() { Value = ((int)g).ToString(), Text = g.ToString() }).ToList();
            roles.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.Roles = roles;

            var Usuario = await _getUsuario_RS.Execute(IdUsuario);
            var model = new UsuarioModel();
            model.Id_Usuario_Notificar_RS = Usuario.IdUsuario;
            model.Id_Rol = Usuario.IdRol;
            model.Nombres = Usuario.Nombres;
            model.Apellidos = Usuario.Apellidos;
            model.usuario_red = Usuario.UsuarioRed;
            model.Email = Usuario.Email;
            model.Estado = Usuario.Estado;
            return View(model);
        }
        public async Task<IActionResult> EditarUsuario(UsuarioModel model)
        {
            if (ModelState.IsValid)
                await _editarUsuario_RS.Execute(model.Id_Usuario_Notificar_RS,model.Nombres,model.Apellidos,model.Id_Rol,model.Email,model.usuario_red, model.Estado);
            return RedirectToAction("DetalleUsuario", new { IdUsuario = model.Id_Usuario_Notificar_RS });
        }
        public async Task<IActionResult> AgregarUsuario()
        {
            var UsuariosAD = await _getUsuarios.Execute();

            var usuarios = UsuariosAD.Usuarios.Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres+" "+g.Apellidos }).ToList();
            usuarios.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
            
            ViewBag.ListUsuarios = usuarios;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AgregarUsuario(UsuarioModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _agregarUsuario_RS.Execute(model.Id_Usuario_Notificar_RS);
                return RedirectToAction("DetalleUsuario", new { IdUsuario = id });
            }
            return RedirectToAction("AgregarUsuario");
        }
        public async Task<IActionResult> QuitarUsuario(int Id_Usuario)
        {
            await _deleteUsuario.Execute(Id_Usuario);
            return RedirectToAction("Resumen");
        }
    }
}