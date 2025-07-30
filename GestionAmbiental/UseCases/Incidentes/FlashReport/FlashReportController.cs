using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.EditarFlashReport;
using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.GetFlashReport;
using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.GetFlashReports;
using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.RegistrarFlashReport;
using GestionAmbiental.UseCases.Util.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Incidentes.Items;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.EditarAccionFlashReport;
using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.GetAccionesFlashReport;
using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.GetAccionFlashReport;
using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.RegistrarAccionFlashReport;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones;
using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.EliminarAccionFlashReport;
using System.IO.Compression;
using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.GetEvidenciasFlashReport;

namespace GestionAmbiental.UseCases.Incidentes.FlashReport
{
    public class FlashReportController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IGestorArchivos _gestorArchivos;
        private readonly IGestorEncriptado _gestorEncriptado;

		private readonly IGetEvidenciasFlashReport _getEvidenciasFlashReport;
		private readonly IRegistrarFlashReport _registrarFlashReport;
        private readonly IEditarFlashReport _editarFlashReport;
        private readonly IGetFlashReport _getFlashReport;
        private readonly IGetFlashReports _getFlashReports;

        private readonly IEditarAccionFlashReport _editarAccionFlashReport;
        private readonly IGetAccionesFlashReport _getAccionesFlashReport;
        private readonly IGetAccionFlashReport _getAccionFlashReport;
        private readonly IRegistrarAccionFlashReport _registrarAccionFlashReport;
        private readonly IEliminarAccionFlashReport _eliminarAccionFlashReport;

        private readonly ISelectItems _selectItems;

        private readonly IDataProtector _protector;

        public FlashReportController(
            IWebHostEnvironment hostingEnvironment,
            IGestorArchivos gestorArchivos,
            ISelectItems selectItems,
            IGestorEncriptado gestorEncriptado,
            IRegistrarFlashReport registrarFlashReport,
            IEditarFlashReport editarFlashReport,
            IGetFlashReport getFlashReport,
            IGetFlashReports getFlashReports,
            IEditarAccionFlashReport editarAccionFlashReport,
            IGetAccionesFlashReport getAccionesFlashReport,
            IGetAccionFlashReport getAccionFlashReport,
            IRegistrarAccionFlashReport registrarAccionFlashReport,
            IEliminarAccionFlashReport eliminarAccionIncidente,
			IGetEvidenciasFlashReport getEvidenciasFlashReport,
			IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _hostingEnvironment = hostingEnvironment;
            _selectItems = selectItems;
            _gestorArchivos = gestorArchivos;
            _gestorEncriptado = gestorEncriptado;
            _registrarFlashReport = registrarFlashReport;
            _editarFlashReport = editarFlashReport;
            _editarAccionFlashReport = editarAccionFlashReport;
            _getAccionesFlashReport = getAccionesFlashReport;
            _getAccionFlashReport = getAccionFlashReport;
            _registrarAccionFlashReport = registrarAccionFlashReport;
            _eliminarAccionFlashReport = eliminarAccionIncidente;
			_getEvidenciasFlashReport = getEvidenciasFlashReport;
			_getFlashReport = getFlashReport;
            _getFlashReports = getFlashReports;

            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InvestigacionIdRouteValue);
        }
        public IActionResult PropiedadesModulo()
        {
            dynamic response;
            try
            {
                var propPath = Path.Combine(_hostingEnvironment.WebRootPath, "json", "propiedades_modulos.json");
                var propText = System.IO.File.ReadAllText(propPath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                var prop = JsonConvert.DeserializeObject<List<PropiedadModulos>>(propText);
                var dataModulo = prop.Find(x => x.modulo.Equals("Incidentes"));
                if (dataModulo != null)
                    response = new
                    {
                        success = true,
                        data = JsonConvert.SerializeObject(dataModulo)
                    };
                else
                    response = new
                    {
                        success = false,
                        msg = "No se encontraron las propiedades del módulo"
                    };
            }
            catch (Exception e)
            {
                response = new
                {
                    success = false,
                    msg = e.Message
                };
            }
            return Ok(response);
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

                var items = await _selectItems.GetSelectItems();
                var asesores = items.ItemsUsuarioMA.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres.ToUpper() + " " + g.Apellidos.ToUpper() }).ToList();
                var usuarios = items.ItemsUsuario.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres.ToUpper() + " " + g.Apellidos.ToUpper() }).OrderBy(x => x.Text).ToList();
                var gerencias = items.ItemsGerencia.Where(x => x.Estado.Equals("ACT")).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();

                ViewBag.Asesores = asesores;
                ViewBag.Gerencias = gerencias;
                ViewBag.Usuarios = usuarios;

                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string fecha_inicio, string fecha_fin, int[] gerencia, int[] titular, string[] estado, int[] responsable, int[] asesor, string codigo, int rpp, int pagina, string usuario_red)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

            var datosTabla = await _getFlashReports.Execute(fecha_inicio, fecha_fin, gerencia, titular, estado, responsable, asesor, codigo, pagina, rpp, usuario_red);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NFlashReports % rpp != 0 ? (datosTabla.NFlashReports / rpp) + 1 : datosTabla.NFlashReports / rpp;
            if (datosTabla.NFlashReports > 0)
            {
                foreach (var fila in datosTabla.FlashReports)
                {
                    if (!fila.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) || (fila.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) && userInfo.Rol.Equals(InspeccionRol.Admin)))
                    {
                        var IdEncriptado = _protector.Protect(fila.Id_Flash_Report.ToString());
                        filasTabla += "<tr>";
                        if (!fila.Estado.Equals(EstadosIncidentes.Eliminado.ToString()))
                            filasTabla += "<td style=\"width: 115px;\"><a href=\"/FlashReport/EditarFlashReport?IdFlashReport=" + IdEncriptado + "\">" + fila.Codigo + "</a></td>";
                        else
                            filasTabla += "<td style=\"width: 115px;\"><a href=\"/FlashReport/EditarFlashReport?IdFlashReport=" + IdEncriptado + "\">" + fila.Codigo + " (Eliminado)" + "</a></td>";
                        //filasTabla += "<td>" + fila.Tipo_Incidente + "</td>";
                        filasTabla += "<td>" + fila.Gerencia_Nombre + "</td>";
                        filasTabla += "<td>" + fila.Empresa_Nombre + "</td>";
                        filasTabla += "<td>" + fila.Nivel_Incidente_Descripcion + "</td>";
                        filasTabla += "<td style=\"width: 150px;\">";
                        filasTabla += fila.Fecha_Hora != null ? fila.Fecha_Hora.ToString("dd/MM/yyyy HH:mm") : "";
                        filasTabla += "</td>";
                        filasTabla += "<td>" + fila.Producto_Incidente_Nombre + "</td>";
                        filasTabla += "<td>" + fila.Cantidad_Derramada + " " + fila.Unidad_Cantidad_Derramada_Nombre + "</td>";
                        filasTabla += "<td>" + fila.Supervisor_Marcobre_Nombre + "</td>";
                        filasTabla += "<td>" + fila.Supervisor_Contratista + "</td>";
                        filasTabla += "</tr>";
                    }
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
                var nCol = 10;
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"" + nCol + "\">Sin Datos</td>";
                filasTabla += "</tr>";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }
        public async Task<IActionResult> NuevoFlashReport()
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
                var items = await _selectItems.GetSelectItems();

                var gerencia_incidente = items.ItemsGerencia.Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
                gerencia_incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var empresa_incidente = items.ItemsEmpresa.Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
                empresa_incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var tipo_Incidente = items.ItemsTipoIncidente.Select(g => new SelectListItem() { Value = g.Id_Tipo_Incidente.ToString(), Text = g.Descripcion }).ToList();
                tipo_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var nivel_Incidente = items.ItemsNivelIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Nivel_Incidente.ToString(), Text = g.Descripcion }).ToList();
                nivel_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var producto_derramado_Incidente = items.ItemsProductoIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Producto_Incidente.ToString(), Text = g.Descripcion }).ToList();
                producto_derramado_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var unidad_Derramado = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.General.ToString()) ||
                    x.Tipo.Equals(TiposUnidadMedida.Volumen.ToString()) ||
                    x.Tipo.Equals(TiposUnidadMedida.Peso.ToString()))
                    .OrderBy(x => x.Descripcion)
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Derramado.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var supervisores_marcobre = items.ItemsUsuarioMA.Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres.ToUpper() + " " + g.Apellidos.ToUpper() }).ToList();
                supervisores_marcobre.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuario = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres.ToUpper() + " " + g.Apellidos.ToUpper() }).ToList();
                usuario.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var usuariosJson = JsonConvert.SerializeObject(items.ItemsUsuario.Select(x => new { IdUsuario = x.IdUsuario, NombresApellidos = x.Nombres.ToUpper() + " " + x.Apellidos.ToUpper() }));

                var unidad_Area = items.ItemsUnidadMedida.Where(
                    x => x.Tipo.Equals(TiposUnidadMedida.General.ToString()) ||
                    x.Tipo.Equals(TiposUnidadMedida.Area.ToString()))
                    .OrderBy(x => x.Descripcion)
                    .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
                unidad_Area.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                var componente_Ambiental = items.ItemsCompAmb.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Componente_Ambiental.ToString(), Text = g.Descripcion }).ToList();
                componente_Ambiental.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                //Causa Incidente
                var grupoActo = new SelectListGroup() { Name = "Acto Subestándar", Disabled = false };
                var grupoCondicion = new SelectListGroup() { Name = "Condición Subestándar", Disabled = false };

                var causa_Incidente = items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion, Group = grupoActo }).ToList();
                causa_Incidente.AddRange(items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion, Group = grupoCondicion }).ToList());
                causa_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

                ViewBag.Gerencia_Incidente = gerencia_incidente;
                ViewBag.Empresa_Incidente = empresa_incidente;
                ViewBag.Tipo_Incidente = tipo_Incidente;
                ViewBag.Nivel_Incidente = nivel_Incidente;
                ViewBag.Unidad_Derramado = unidad_Derramado;
                ViewBag.Producto_Derramado = producto_derramado_Incidente;
                ViewBag.Unidad_Area = unidad_Area;
                ViewBag.Usuario = usuario;
                ViewBag.UsuariosJson = usuariosJson;
                ViewBag.Componente_Ambiental = componente_Ambiental;
                ViewBag.Causa_Incidente = causa_Incidente;
                ViewBag.Supervisores_Marcobre = supervisores_marcobre;

                return View();
            }
        }
		public async Task<IActionResult> EditarFlashReport(string IdFlashReport)
		{
			if (!(HttpContext.Session.GetString("SessionUser") != null))
			{
				return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName });
			}
			else
			{
				var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var IdFlashReportInt = Convert.ToInt32(_protector.Unprotect(IdFlashReport));
				var flashReport = await _getFlashReport.Execute(IdFlashReportInt);
                var acciones = await _getAccionesFlashReport.Execute(IdFlashReportInt);

                var model = new EditarFlashReportModel
                {
                    Acciones = JsonConvert.SerializeObject(acciones.AccionesFlashReport),
                    Id_Flash_Report = flashReport.Id_Flash_Report,
                    Id_Gerencia = flashReport.Id_Gerencia,
                    Id_Empresa = flashReport.Id_Empresa,
                    Lugar = flashReport.Lugar,
                    Fecha_Incidente = flashReport.Fecha_Hora.ToString("yyyy-MM-dd"),
                    Hora_Incidente = flashReport.Fecha_Hora.ToString("HH:mm"),
                    Cantidad_Derramada = flashReport.Cantidad_Derramada,
                    Id_Unidad_Cantidad_Derramada = flashReport.Id_Unidad_Cantidad_Derramada,
                    Descripcion = flashReport.Descripcion,
                    Fecha_Hora = flashReport.Fecha_Hora,
                    Nivel_Incidente = flashReport.Nivel_Incidente,
                    Componente_Ambiental_Afectado = flashReport.Componente_Ambiental_Afectado,
                    Id_Producto_Incidente = flashReport.Id_Producto_Incidente,
                    Id_Supervisor_Marcobre = flashReport.Id_Supervisor_Marcobre,
                    Supervisor_Contratista = flashReport.Supervisor_Contratista,
                    Cargo_Supervisor_Marcobre = flashReport.Cargo_Supervisor_Marcobre,
                    Cargo_Supervisor_Contratista = flashReport.Cargo_Supervisor_Contratista,
                    Estado = flashReport.Estado,
                    Evidencias_Completas = flashReport.Registro_Fotografico!=null?JsonConvert.SerializeObject(flashReport.Registro_Fotografico) : null,
					Evidencias_Nombres = flashReport.Archivo_Registro_Fotografico,
				};

				ViewBag.Rol = userInfo.Rol.ToString();
				ViewBag.Usuario = userInfo.NombreUsuario;
				var items = await _selectItems.GetSelectItems();

				var gerencia_incidente = items.ItemsGerencia.Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
				gerencia_incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

				var empresa_incidente = items.ItemsEmpresa.Select(g => new SelectListItem() { Value = g.IdEmpresa.ToString(), Text = g.RazonSocial }).ToList();
				empresa_incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

				var tipo_Incidente = items.ItemsTipoIncidente.Select(g => new SelectListItem() { Value = g.Id_Tipo_Incidente.ToString(), Text = g.Descripcion }).ToList();
				tipo_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

				var nivel_Incidente = items.ItemsNivelIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Nivel_Incidente.ToString(), Text = g.Descripcion }).ToList();
				nivel_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

				var producto_derramado_Incidente = items.ItemsProductoIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Producto_Incidente.ToString(), Text = g.Descripcion }).ToList();
				producto_derramado_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

				var unidad_Derramado = items.ItemsUnidadMedida.Where(
					x => x.Tipo.Equals(TiposUnidadMedida.General.ToString()) ||
					x.Tipo.Equals(TiposUnidadMedida.Volumen.ToString()) ||
					x.Tipo.Equals(TiposUnidadMedida.Peso.ToString()))
					.OrderBy(x => x.Descripcion)
					.Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
				unidad_Derramado.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

				var supervisores_marcobre = items.ItemsUsuarioMA.Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres.ToUpper() + " " + g.Apellidos.ToUpper() }).ToList();
				supervisores_marcobre.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

				var usuario = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres.ToUpper() + " " + g.Apellidos.ToUpper() }).ToList();
				usuario.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

				var usuariosJson = JsonConvert.SerializeObject(items.ItemsUsuario.Select(x => new { IdUsuario = x.IdUsuario, NombresApellidos = x.Nombres.ToUpper() + " " + x.Apellidos.ToUpper() }));

				var unidad_Area = items.ItemsUnidadMedida.Where(
					x => x.Tipo.Equals(TiposUnidadMedida.General.ToString()) ||
					x.Tipo.Equals(TiposUnidadMedida.Area.ToString()))
					.OrderBy(x => x.Descripcion)
					.Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
				unidad_Area.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

				var componente_Ambiental = items.ItemsCompAmb.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Componente_Ambiental.ToString(), Text = g.Descripcion }).ToList();
				componente_Ambiental.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

				//Causa Incidente
				var grupoActo = new SelectListGroup() { Name = "Acto Subestándar", Disabled = false };
				var grupoCondicion = new SelectListGroup() { Name = "Condición Subestándar", Disabled = false };

				var causa_Incidente = items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion, Group = grupoActo }).ToList();
				causa_Incidente.AddRange(items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion, Group = grupoCondicion }).ToList());
				causa_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

				ViewBag.Gerencia_Incidente = gerencia_incidente;
				ViewBag.Empresa_Incidente = empresa_incidente;
				ViewBag.Tipo_Incidente = tipo_Incidente;
				ViewBag.Nivel_Incidente = nivel_Incidente;
				ViewBag.Unidad_Derramado = unidad_Derramado;
				ViewBag.Producto_Derramado = producto_derramado_Incidente;
				ViewBag.Unidad_Area = unidad_Area;
				ViewBag.Usuario = usuario;
				ViewBag.UsuariosJson = usuariosJson;
				ViewBag.Componente_Ambiental = componente_Ambiental;
				ViewBag.Causa_Incidente = causa_Incidente;
				ViewBag.Supervisores_Marcobre = supervisores_marcobre;

				return View(model);
			}
		}
		[HttpPost]
        public async Task<IActionResult> NuevoFlashReport(NuevoFlashReportModel model)
        {
            model.Fecha_Hora = Convert.ToDateTime(model.Fecha_Incidente + " " + model.Hora_Incidente);
			if (model.Opcion != "Finalizar")
				model.Estado = "Registrado";
			else
				model.Estado = "Pendiente";

			var acciones = JsonConvert.DeserializeObject<List<Accion>>(model.Acciones);


            if (ModelState.IsValid)
            {
				var evidencias = new List<string>();
				var evidenciasNombres = new List<NombreEvidencia>();

				if (model.Evidencias != null)
				{
					for (int i = 0; i < model.Evidencias.Count; i++)
					{
						var extension = Path.GetExtension(model.Evidencias[i].FileName);
						var max_file_size = 12000000;

						var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG" };

						if (allowed_extensions.Contains(extension) && model.Evidencias[i].Length < max_file_size)
						{
							var newfileName = CreateUniqueFileName(i + 1) + ".zip";
							var filePath = await UploadZipFileAsync(model.Evidencias[i], newfileName);
							evidencias.Add(filePath);

							evidenciasNombres.Add(new NombreEvidencia
							{
								NombreOrigen = model.Evidencias[i].FileName,
								NombreCarga = filePath,
								ExtOrigen = Path.GetExtension(model.Evidencias[i].FileName),
								ExtCarga = Path.GetExtension(filePath)
							});
						}
					}
				}

				var IdFlashReport = await _registrarFlashReport.Execute(
                    model.Id_Gerencia,
                    model.Id_Empresa,
                    model.Lugar,
                    model.Fecha_Hora,
                    model.Nivel_Incidente,
                    model.Componente_Ambiental_Afectado,
                    model.Id_Producto_Incidente,
                    model.Cantidad_Derramada,
                    model.Id_Unidad_Cantidad_Derramada,
                    model.Descripcion,
					JsonConvert.SerializeObject(evidencias),
					JsonConvert.SerializeObject(evidenciasNombres),
					model.Id_Supervisor_Marcobre,
                    model.Supervisor_Contratista,
                    model.Cargo_Supervisor_Marcobre,
                    model.Cargo_Supervisor_Contratista,
                    model.Estado,
                    model.Opcion
                    );

				foreach (var accion in acciones)
				{
					if (!string.IsNullOrEmpty(accion.Descripcion) && 
                        accion.Usuario > 0 && 
                        !string.IsNullOrEmpty(accion.Contratista) && 
                        accion.Fecha != null && !string.IsNullOrEmpty(accion.Estado))
					{
						await _registrarAccionFlashReport.Execute(IdFlashReport, accion.Descripcion, accion.Usuario, accion.Contratista, accion.Fecha, accion.Estado, "Registrado");
					}
				}

				return RedirectToAction("Resumen");
            }
            return RedirectToAction("NuevoFlashReport");
        }
		[HttpPost]
		public async Task<IActionResult> EditarFlashReport(EditarFlashReportModel model)
		{
			model.Fecha_Hora = Convert.ToDateTime(model.Fecha_Incidente + " " + model.Hora_Incidente);
            if(model.Opcion!= "Finalizar")
                model.Estado = "Registrado";
			else
				model.Estado = "Pendiente";

			var acciones = JsonConvert.DeserializeObject<List<Accion>>(model.Acciones);

			if (ModelState.IsValid)
			{
				var nombresEvidencias = await _getEvidenciasFlashReport.Execute(model.Id_Flash_Report);
				var nombresArchivos = JsonConvert.DeserializeObject<List<NombreEvidencia>>(nombresEvidencias);

				var evidencias = new List<string>();
				var evidenciasNombres = new List<NombreEvidencia>();

				//Existen evidencias anteriores
				if (model.Evidencias_Completas != null)
				{
					var temp = new List<NombreEvidencia>();
					evidencias.AddRange(new List<string>(model.Evidencias_Completas.Split(',')));

                    //Validar nombres archivos tiene categoria papel
                    if (nombresArchivos.Count > 0)
                    {
                        //Eliminar las evidencias que no se encuentre en la lista anterior
                        temp = nombresArchivos.Where(x => evidencias.Contains(x.NombreCarga)).ToList();
                    }
                    else
                    {
                        //Nombres archivos no tiene datos, agregar los existentes
                        if (evidencias != null)
                        {
                            temp = evidencias.Select(x => new NombreEvidencia
                            {
                                NombreOrigen = x,
                                NombreCarga = x,
                                ExtOrigen = Path.GetExtension(x),
                                ExtCarga = Path.GetExtension(x)
                            }).ToList();
                        }
                    }
                    evidenciasNombres.AddRange(temp);
				}
				if (model.Evidencias != null)
				{
					for (int i = 0; i < model.Evidencias.Count; i++)
					{
						var extension = Path.GetExtension(model.Evidencias[i].FileName);
						var max_file_size = 12000000;

						var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPEG", ".png", ".PNG" };

						if (allowed_extensions.Contains(extension) && model.Evidencias[i].Length < max_file_size)
						{
							var newfileName = CreateUniqueFileName(i + 1) + ".zip";
							var filePath = await UploadZipFileAsync(model.Evidencias[i], newfileName);
							evidencias.Add(filePath);

							evidenciasNombres.Add(new NombreEvidencia
							{
								NombreOrigen = model.Evidencias[i].FileName,
								NombreCarga = filePath,
								ExtOrigen = Path.GetExtension(model.Evidencias[i].FileName),
								ExtCarga = Path.GetExtension(filePath)
							});
						}
					}
				}

				var IdFlashReport = await _editarFlashReport.Execute(
                    model.Id_Flash_Report,
					model.Id_Gerencia,
					model.Id_Empresa,
					model.Lugar,
					model.Fecha_Hora,
					model.Nivel_Incidente,
					model.Componente_Ambiental_Afectado,
					model.Id_Producto_Incidente,
					model.Cantidad_Derramada,
					model.Id_Unidad_Cantidad_Derramada,
					model.Descripcion,
					JsonConvert.SerializeObject(evidencias),
					JsonConvert.SerializeObject(evidenciasNombres),
					model.Id_Supervisor_Marcobre,
					model.Supervisor_Contratista,
					model.Cargo_Supervisor_Marcobre,
					model.Cargo_Supervisor_Contratista,
					model.Estado,
					model.Opcion
					);
                //Eliminar o editar
				foreach (var accion in acciones)
				{
					if (!string.IsNullOrEmpty(accion.Descripcion) &&
						accion.Usuario > 0 &&
						!string.IsNullOrEmpty(accion.Contratista) &&
						accion.Fecha != null && !string.IsNullOrEmpty(accion.Estado))
					{
						await _registrarAccionFlashReport.Execute(IdFlashReport, accion.Descripcion, accion.Usuario, accion.Contratista, accion.Fecha, accion.Estado, "Registrado");
					}
				}

				return RedirectToAction("Resumen");
			}
			return RedirectToAction("NuevoFlashReport");
		}
		[HttpGet]
        public async Task<IActionResult> ListarAcciones(int Id_Flash_Report)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

            var acciones = await _getAccionesFlashReport.Execute(Id_Flash_Report);
            var filas_acciones = "";
            foreach (var fila in acciones.AccionesFlashReport)
            {
                filas_acciones += "<tr>";
                if (fila.Descripcion != null)
                    filas_acciones += "<td>" + fila.Descripcion + "</td>";
                else
                    filas_acciones += "<td>_</td>";

                if (fila.Nombres_Usuario != null)
                    filas_acciones += "<td>" + fila.Nombres_Usuario + "</td>";
                else
                    filas_acciones += "<td>_</td>";

                if (fila.Fecha != null)
                    filas_acciones += "<td>" + fila.Fecha.Date.ToString("dd/MM/yyyy") + "</td>";
                else
                    filas_acciones += "<td>_</td>";

                if (fila.Estatus != null)
                    filas_acciones += "<td>" + fila.Estatus + "</td>";
                else
                    filas_acciones += "<td>_</td>";

                if ((fila.Estado.Equals(EstadosIncidentes.PendienteInvestigacion.ToString()) && userInfo.Rol.Equals(InspeccionRol.AsesorMAP)) || userInfo.Rol.Equals(InspeccionRol.Admin))
                {
                    filas_acciones += "<td><button type=\"button\" class=\"btn btn-mc\" onclick=\"EditarAccion(" + fila.Id_Accion_Flash_Report + ")\">Editar</button></td>";
                    filas_acciones += "<td><button type=\"button\" class=\"btn btn-mc-danger\" onclick=\"EliminarAccion(" + fila.Id_Accion_Flash_Report + ")\">Eliminar</button></td>";
                }
                else
                    filas_acciones += "<td></td><td></td>";
                filas_acciones += "</tr>";
            }


            dynamic response = new
            {
                filas_acciones = filas_acciones,
                nAcciones = acciones.NAccionesFlashReports
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> NuevaAccion(int idFlashReport, string descripcion, int usuario, string contratista, string fecha, string estado)
        {
            var fechaAccion = Convert.ToDateTime(fecha);
            await _registrarAccionFlashReport.Execute(idFlashReport, descripcion, usuario, contratista, fechaAccion, estado, "Registrado");
            return await ListarAcciones(idFlashReport);
        }
        [HttpGet]
        public async Task<IActionResult> EditarAccion(int IdAccion)
        {
            var accion = await _getAccionFlashReport.Execute(IdAccion);

            dynamic response = new
            {
                idAccion = accion.Id_Accion_Flash_Report,
                idReporte = accion.Id_Flash_Report,
                idUsuario = accion.IdUsuario,
                responsableMarc = accion.Nombres_Usuario,
                accion = accion.Descripcion,
                responsableCon = accion.Responsable,
                fecha = accion.Fecha.ToString("yyyy-MM-dd"),
                estatus = accion.Estatus
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> EditarAccion(int IdAccion, int IdFlashReport, string descripcion, int usuario, string contratista, string fecha, string estatus, string estado)
        {
            var fechaAccion = Convert.ToDateTime(fecha);
            await _editarAccionFlashReport.Execute(IdAccion, IdFlashReport, descripcion, usuario, contratista, fechaAccion, estatus, estado);
            return await ListarAcciones(IdFlashReport);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarAccion(int IdAccion, int IdFlashReport)
        {
            await _eliminarAccionFlashReport.Execute(IdAccion);
            return await ListarAcciones(IdFlashReport);
        }
        public async Task<IActionResult> Download(string fileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?fileName=" + fileName });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                try
                {
                    var encryptedFile = await _gestorArchivos.DownloadFTPFileAsync(fileName, userInfo.NombreUsuario, Tipo_Info_SGA.FTPConnectionEvidenciaFlashReport);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "Descarga Temporal Gestiona\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                    /*decrypt file*/
                    var decryptedFileName = archivePath + "desencriptado.zip";
                    await _gestorEncriptado.DecryptFileAsync(encryptedFile, decryptedFileName);

					//descomprimir en carpeta temporal submission
					if (!filePath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
						filePath += Path.DirectorySeparatorChar;

					using (ZipArchive archive = ZipFile.OpenRead(decryptedFileName))
					{
						foreach (ZipArchiveEntry entry in archive.Entries)
						{
							filePath = Path.GetFullPath(Path.Combine(filePath, entry.FullName));
							if (filePath.StartsWith(filePath, StringComparison.Ordinal))
								entry.ExtractToFile(filePath);
						}
					}
					byte[] bytes = System.IO.File.ReadAllBytes(filePath);
					var newFileName = fileName.Replace(Path.GetExtension(fileName), Path.GetExtension(filePath));
					///*read decrypted file*/
					//FileStream stream = new FileStream(decryptedFileName, FileMode.Open);

					return File(bytes, GetContentType(filePath), newFileName);
                }
                catch (Exception e)
                {
                    return Content("No se encontró el archivo: " + e.Message);
                }
            }
        }

        public async Task<string> UploadZipFileAsync(IFormFile file, string newFileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return "NoAutorizado";
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "Descarga Temporal Gestiona/";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission/" + userInfo.NombreUsuario + "/";
                var archiveFile = SGATempPath + "zip/" + userInfo.NombreUsuario + "/archive.zip";
                var archivePath = SGATempPath + "zip/" + userInfo.NombreUsuario + "/";

                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                if (Directory.Exists(archivePath))
                {
                    Directory.Delete(archivePath, true);
                }

                Directory.CreateDirectory(filePath);
                Directory.CreateDirectory(archivePath);

                var fileName = filePath + file.FileName;
                if (file.Length > 0)
                {
                    var stream = new FileStream(fileName, FileMode.Create);
                    //CopyToAsync corrompe los archivos
                    file.CopyTo(stream);
                    stream.Close();
                }
                ZipFile.CreateFromDirectory(filePath, archiveFile);

                var encryptedArchiveFile = SGATempPath + "zip/" + userInfo.NombreUsuario + "/encriptado.zip";
                await _gestorEncriptado.EncryptFileAsync(archiveFile, encryptedArchiveFile);

                return await _gestorArchivos.UploadFTPFileAsync(encryptedArchiveFile, newFileName, Tipo_Info_SGA.FTPConnectionEvidenciaFlashReport);
            }
        }
        private string CreateUniqueFileName(int index)
        {
            return "Ev_Inc_Rep_Pre" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + "_" + index;
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
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"},
                {".zip","application/zip" }
            };
        }
        private class Accion
        {
            public string Descripcion { get; set; }
            public int Usuario { get; set; }
            public string Contratista { get; set; }
            public DateTime Fecha { get; set; }
            public string Estado { get; set; }
        }
		private class NombreEvidencia
		{
			public string NombreOrigen { get; set; }
			public string NombreCarga { get; set; }
			public string ExtOrigen { get; set; }
			public string ExtCarga { get; set; }
		}
	}
}
