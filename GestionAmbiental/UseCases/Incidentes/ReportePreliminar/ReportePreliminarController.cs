using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Incidentes.Items;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.EditarReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetAccionesIncidente;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetInvolucradosIncidente;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportesPreliminares;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarAccionCorrectivaIncidente;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarInvolucradoIncidente;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarParte1;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarParte2;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Renci.SshNet;

namespace GestionAmbiental.UseCases.Incidentes.ReportePreliminar
{
    public class ReportePreliminarController : Controller
    {

        private readonly IRegistrarParte1 _registrarReporteParte1;
        private readonly IRegistrarParte2 _registrarReporteParte2;
        private readonly IGetReportesPreliminares _getReportesPreliminares;
        private readonly ISelectItems _selectItems;
        private readonly IGetInvolucradosIncidente _getInvolucradosIncidente;
        private readonly IRegistrarInvolucradoIncidente _registrarInvolucradoIncidente;
        private readonly IRegistrarAccionCorrectivaIncidente _registrarAccionCorrectivaIncidente;
        private readonly IGetAccionesIncidente _getAccionesIncidente;
        private readonly IGetReportePreliminar _getReportePreliminar;
        private readonly IGetInfo_SGA _info_SGA;

        public ReportePreliminarController(
            IRegistrarParte1 registrarReporteParte1,
            IRegistrarParte2 registrarReporteParte2,
            IGetReportesPreliminares getReportesPreliminares,
            ISelectItems selectItems,
            IGetInvolucradosIncidente getInvolucradosIncidente,
            IRegistrarInvolucradoIncidente registrarInvolucradoIncidente,
            IRegistrarAccionCorrectivaIncidente registrarAccionCorrectivaIncidente,
            IGetAccionesIncidente getAccionesIncidente,
            IGetReportePreliminar getReportePreliminar,
            IGetInfo_SGA info_SGA)
        {
            _registrarReporteParte1 = registrarReporteParte1;
            _registrarReporteParte2 = registrarReporteParte2;
            _getReportesPreliminares = getReportesPreliminares;
            _selectItems = selectItems;
            _getInvolucradosIncidente = getInvolucradosIncidente;
            _registrarInvolucradoIncidente = registrarInvolucradoIncidente;
            _registrarAccionCorrectivaIncidente = registrarAccionCorrectivaIncidente;
            _getAccionesIncidente = getAccionesIncidente;
            _getReportePreliminar = getReportePreliminar;
            _info_SGA = info_SGA;
        }
        public async Task<IActionResult> Resumen()
        {
            var items = await _selectItems.GetSelectItems();
            var asesores = items.ItemsUsuarioMA.Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " "  +g.Apellidos }).ToList();
            asesores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
            var gerencias = items.ItemsGerencia.Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
            gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.Asesores = asesores;
            ViewBag.Gerencias = gerencias;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetTablaResumenFiltrada(string fecha,int id_usuario,int id_gerencia,int rpp,int pagina)
        {
            var datosTabla = await _getReportesPreliminares.Execute(fecha, id_usuario, id_gerencia, pagina, rpp);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NReportes % rpp != 0 ? (datosTabla.NReportes / rpp) + 1 : datosTabla.NReportes / rpp;
            if (datosTabla.NReportes > 0)
            {
                foreach (var fila in datosTabla.Reportes)
                {
                    //var id_encriptado = _protector.Protect(fila.Id_Ingreso_Salida_RS.ToString());
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"/ReportePreliminar/EditarReportePreliminar?IdReporte=" + fila.Id_Reporte_Preliminar + "\">" + fila.Codigo + "</a></td>";
                    //filasTabla += "<td style=\"width: 115px;\">" + fila.Codigo + "</td>";
                    filasTabla += "<td>" + fila.Tipo_Incidente + "</td>";
                    filasTabla += "<td>" + fila.Nivel_Incidente + "</td>";
                    filasTabla += "<td style=\"width: 150px;\">";
                    filasTabla += fila.Fecha_Incidente != null ? fila.Fecha_Incidente.Value.ToString("dd/MM/yyyy HH:mm") : "";
                    filasTabla += "</td>";
                    filasTabla += "<td>" + fila.Producto_Derramado + "</td>";
                    filasTabla += "<td>" + fila.Cantidad_Derramado+" "+fila.Unidad_Cantidad_Derramado + "</td>";
                    filasTabla += "<td>" + fila.Area_Afectada+ " "+fila.Unidad_Area_Afectada + "</td>";
                    filasTabla += "<td>" + fila.Lugar_Incidente + "</td>";
                    filasTabla += "<td>" + fila.Equipo_Involucrado + "</td>";
                    filasTabla += "<td>" + fila.Gerencia_Responsable + "</td>";
                    filasTabla += "</tr>";
                }

                paginador += "<ul class=\"pagination\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Previous</span></a></li>";

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
                    paginador += "<li class=\"page - item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Next\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Next</span></a></li>";
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
        public async Task<IActionResult> NuevoReporte()
        {
            var items = await _selectItems.GetSelectItems();

            var tipo_Incidente = items.ItemsTipoIncidente.Select(g => new SelectListItem() { Value = g.Id_Tipo_Incidente.ToString(), Text = g.Descripcion }).ToList();
            tipo_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var nivel_Incidente = items.ItemsNivelIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Nivel_Incidente.ToString(), Text = g.Descripcion }).ToList();
            nivel_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var unidad_Derramado = items.ItemsUnidadMedida.Where(
                x => x.Tipo.Equals(TiposUnidadMedida.General.ToString()) || 
                x.Tipo.Equals(TiposUnidadMedida.Sustancia.ToString()) || 
                x.Tipo.Equals(TiposUnidadMedida.Peso.ToString()))
                .OrderBy(x => x.Descripcion)
                .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
            unidad_Derramado.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

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

           

            ViewBag.Tipo_Incidente = tipo_Incidente;
            ViewBag.Nivel_Incidente = nivel_Incidente;
            ViewBag.Unidad_Derramado = unidad_Derramado;
            ViewBag.Unidad_Area = unidad_Area;
            ViewBag.Componente_Ambiental = componente_Ambiental;
            ViewBag.Causa_Incidente = causa_Incidente;
           

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevoReporte(ReporteParte1Model model)
        {
            if (ModelState.IsValid)
            {
                var IdReporte = await _registrarReporteParte1.Execute(
                    model.Num_Trabajadores,
                    model.Razon_Social_Contratista,
                    model.RUC_Contratista,
                    model.Actividad_Contratista,
                    model.Domicilio_Contratista,
                    model.Num_Trabajadores_Contratista,
                    model.Id_Tipo_Incidente,
                    model.Id_Nivel_Incidente,
                    model.Fecha_Incidente,
                    model.Hora_Incidente,
                    model.Producto_Derramado,
                    model.Cantidad_Derramado,
                    model.Id_Unidad_Cantidad_Derramado,
                    model.Area_Afectada,
                    model.Id_Unidad_Area_Afectada,
                    model.Lugar_Incidente,
                    model.Equipo_Involucrado,
                    model.Id_Componente_Ambiental,
                    model.Descripcion_Incidente,
                    model.Id_Causa_Incidente,
                    model.Otra_Causa_Incidente);
                if(model.Opcion.Equals("Guardar"))
                    return RedirectToAction("NuevoReporte");
                else
                    return RedirectToAction("NuevoReporteParte2", new { IdReporte = IdReporte });
            }
            return RedirectToAction("NuevoReporte");
        }
        public async Task<IActionResult> NuevoReporteParte2(int IdReporte)
        {
            var items = await _selectItems.GetSelectItems();

            var gerencias = items.ItemsGerencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
            gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var supervisores = items.ItemsUsuarioMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres+" "+g.Apellidos }).ToList();
            supervisores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var usuario = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            usuario.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.Gerencias = gerencias;
            ViewBag.Supervisor = supervisores;
            ViewBag.Usuario = usuario;

            var model = new ReporteParte2Model();
            model.Id_Reporte_Preliminar = IdReporte;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ListarInvolucrados(int IdReporte)
        {
            var involucrados = await _getInvolucradosIncidente.Execute(IdReporte);
            var filas_involucrados = "";
            foreach (var fila in involucrados)
            {
                filas_involucrados += "<tr>";
                filas_involucrados += "<td>" + fila.Tipo_Involucrado + "</td>";
                filas_involucrados += "<td>" + fila.Nombres + " " + fila.Apellidos + "</td>";
                filas_involucrados += "<td>" + fila.Email + "</td>";
                filas_involucrados += "<td>" + fila.Celular + "</td>";
                filas_involucrados += "<td>" + fila.Puesto_Trabajo + "</td>";
                filas_involucrados += "<td>" + fila.Turno + "</td>";
                filas_involucrados += "</tr>";
            }
            dynamic response = new
            {
                filas_involucrados = filas_involucrados,
                nInvolucrados = involucrados.Count
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> NuevoInvolucrado(int IdReporte, string tipo, int usuario, string nombres,string apellidos, string correo, string puesto, string celular, string turno)
        {
            await _registrarInvolucradoIncidente.Execute(IdReporte, tipo, usuario, nombres,apellidos, correo, puesto, celular, turno);
            return await ListarInvolucrados(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> ListarAcciones(int IdReporte)
        {
            var acciones = await _getAccionesIncidente.Execute(IdReporte);
            var filas_acciones = "";
            foreach (var fila in acciones)
            {
                filas_acciones += "<tr>";
                filas_acciones += "<td>" + fila.Accion_Inmediata + "</td>";
                filas_acciones += "<td>" + fila.Responsable_Marcobre + "</td>";
                filas_acciones += "<td>" + fila.Responsable_Contratista + "</td>";
                filas_acciones += "<td>" + fila.Fecha.Value.Date.ToString("dd/MM/yyyy")+ "</td>";
                filas_acciones += "<td>" + fila.Estatus + "</td>";
                filas_acciones += "</tr>";
            }
            dynamic response = new
            {
                filas_acciones = filas_acciones,
                nAcciones = acciones.Count
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> NuevaAccion(int IdReporte, string descripcion, int usuario, string contratista, string fecha, string estado)
        {
            await _registrarAccionCorrectivaIncidente.Execute(IdReporte, descripcion, usuario, contratista, fecha, estado);
            return await ListarAcciones(IdReporte);
        }
        [HttpPost]
        public async Task<IActionResult> NuevoReporteParte2(ReporteParte2Model model)
        {
            if (ModelState.IsValid)
            {
                var evidencias = new List<string>();
                var max_file_size = 8388608;

                if (model.Evidencias != null)
                {
                    for (int i = 0; i < model.Evidencias.Count; i++)
                    {
                        var extension = Path.GetExtension(model.Evidencias[i].FileName);

                        var allowed_extensions = new[] { ".jpg", ".JPG", ".jpeg", ".JPGE", ".png", ".PNG", ".xls", ".xlsx", ".doc", ".docx", ".pdf" };

                        if (allowed_extensions.Contains(extension) && model.Evidencias[i].Length < max_file_size)
                        {
                            var newfileName = CreateUniqueFileName() + ".zip";
                            var filePath = await UploadZipFileAsync(model.Evidencias[i], newfileName);
                            evidencias.Add(filePath);
                        }
                    }
                }
                await _registrarReporteParte2.Execute(model.Id_Reporte_Preliminar, model.Id_Gerencia_Responsable, evidencias, model.Comentario_Accion, model.Id_Supervisor, model.Opcion);
                return RedirectToAction("Resumen");
            }
            return RedirectToAction("NuevoReporteParte2", new { IdReporte = model.Id_Reporte_Preliminar });
        }
        public async Task<IActionResult> EditarReportePreliminar(int IdReporte)
        {
            var items = await _selectItems.GetSelectItems();

            var tipo_Incidente = items.ItemsTipoIncidente.Select(g => new SelectListItem() { Value = g.Id_Tipo_Incidente.ToString(), Text = g.Descripcion }).ToList();
            tipo_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var nivel_Incidente = items.ItemsNivelIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Nivel_Incidente.ToString(), Text = g.Descripcion }).ToList();
            nivel_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var unidad_Derramado = items.ItemsUnidadMedida.Where(
                x => x.Tipo.Equals(TiposUnidadMedida.General.ToString()) ||
                x.Tipo.Equals(TiposUnidadMedida.Sustancia.ToString()) ||
                x.Tipo.Equals(TiposUnidadMedida.Peso.ToString()))
                .OrderBy(x => x.Descripcion)
                .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
            unidad_Derramado.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

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

            var gerencias = items.ItemsGerencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
            gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var supervisores = items.ItemsUsuarioMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            supervisores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var usuario = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            usuario.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.Tipo_Incidente = tipo_Incidente;
            ViewBag.Nivel_Incidente = nivel_Incidente;
            ViewBag.Unidad_Derramado = unidad_Derramado;
            ViewBag.Unidad_Area = unidad_Area;
            ViewBag.Componente_Ambiental = componente_Ambiental;
            ViewBag.Causa_Incidente = causa_Incidente; 
            ViewBag.Gerencias = gerencias;
            ViewBag.Supervisor = supervisores;
            ViewBag.Usuario = usuario;

            var reporte = await _getReportePreliminar.Execute(IdReporte);
            var model = new EditarReporteModel();
            model.Id_Reporte_Preliminar = reporte.Id_Reporte_Preliminar;
            model.Codigo = reporte.Codigo;
            model.Num_Trabajadores = reporte.Num_Trabajadores;
            model.Razon_Social_Contratista = reporte.Razon_Social_Contratista;
            model.RUC_Contratista = reporte.RUC_Contratista;
            model.Actividad_Contratista = reporte.Actividad_Contratista;
            model.Domicilio_Contratista = reporte.Domicilio_Contratista;
            model.Num_Trabajadores_Contratista = reporte.Num_Trabajadores_Contratista;
            model.Id_Tipo_Incidente = reporte.Id_Tipo_Incidente.HasValue?reporte.Id_Tipo_Incidente.Value:0;
            model.Id_Nivel_Incidente = reporte.Id_Nivel_Incidente.HasValue ? reporte.Id_Nivel_Incidente.Value : 0;
            model.Fecha_Incidente = reporte.Fecha_Incidente !=null ? reporte.Fecha_Incidente.Value.ToString("yyyy-MM-dd") : "";
            model.Hora_Incidente = reporte.Fecha_Incidente != null ? reporte.Fecha_Incidente.Value.ToString("HH:mm") : "";
            model.Producto_Derramado = reporte.Producto_Derramado;
            model.Cantidad_Derramado = reporte.Cantidad_Derramado;
            model.Id_Unidad_Cantidad_Derramado = reporte.Id_Unidad_Cantidad_Derramado.HasValue ? reporte.Id_Unidad_Cantidad_Derramado.Value : 0;
            model.Area_Afectada = reporte.Area_Afectada;
            model.Id_Unidad_Area_Afectada = reporte.Id_Unidad_Area_Afectada.HasValue ? reporte.Id_Unidad_Area_Afectada.Value : 0;
            model.Lugar_Incidente = reporte.Lugar_Incidente;
            model.Equipo_Involucrado = reporte.Equipo_Involucrado;
            model.Id_Componente_Ambiental = reporte.Id_Componente_Ambiental.HasValue ? reporte.Id_Componente_Ambiental.Value : 0;
            model.Descripcion_Incidente = reporte.Descripcion_Incidente;
            model.Id_Causa_Incidente = reporte.Id_Causa_Incidente.HasValue ? reporte.Id_Causa_Incidente.Value : 0;
            model.Otra_Causa_Incidente = reporte.Otra_Causa_Incidente;
            model.Id_Gerencia_Responsable = reporte.Id_Gerencia_Responsable;
            model.Comentario_Accion = reporte.Comentario_Accion;
            model.Id_Supervisor = reporte.Id_Supervisor_MA;

            return View(model);
        }


        //Carga y descarga de archivos
        private string CreateUniqueFileName()
        {
            return "Ev_Inc_Rep_Pre" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
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
        public async Task<IActionResult> Download(string fileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                try
                {
                    var encryptedFile = await DownloadFTPFileAsync(fileName);

                    /* Save encrypted file in temp path*/
                    var tempPath = Path.GetTempPath();

                    var SGATempPath = tempPath + "SGA-Marcobre\\";
                    if (!Directory.Exists(SGATempPath))
                    {
                        Directory.CreateDirectory(SGATempPath);
                    }
                    var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                    var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

                    /*decrypt file*/
                    var decryptedFileName = "desencriptado.zip";
                    await DecryptFileAsync(encryptedFile, archivePath + decryptedFileName);
                    /*read decrypted file*/
                    FileStream stream = new FileStream(archivePath + decryptedFileName, FileMode.Open);

                    return File(stream, GetContentType(decryptedFileName), fileName);
                }
                catch (Exception e)
                {
                    return Content("No se encontró el archivo: " + e.Message);
                }
            }
        }
        public async Task<string> UploadFTPFileAsync(string path, string newFileName)
        {
            /*if (!(HttpContext.Session.GetString("SessionUser") != null))
                return "NoAutorizado";
            else
            {*/
            Tipo_Info_SGA directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncR;

            var serverInfo = await _info_SGA.Execute(directorio);

            string host = "", port = "", user = "", pass = "", dir = "";
            foreach (var item in serverInfo.Info)
            {
                switch (item.Key)
                {
                    case "host": host = item.Value; break;
                    case "port": port = item.Value; break;
                    case "user": user = item.Value; break;
                    case "password": pass = item.Value; break;
                    case "directory": dir = item.Value; break;
                }
            }

            using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
            {
                sftp.Connect();
                sftp.ChangeDirectory(dir);
                using (FileStream fs = System.IO.File.OpenRead(path))
                {
                    sftp.UploadFile(fs, newFileName, true);
                }
                sftp.Dispose();
            }
            return newFileName;
            //}
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
                var SGATempPath = tempPath + "SGA-Marcobre/";
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
                await EncryptFileAsync(archiveFile, archivePath + "encriptado.zip");

                return await UploadFTPFileAsync(encryptedArchiveFile, newFileName);
            }
        }
        public async Task<string> DownloadFTPFileAsync(string fileName)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return "NoAutorizado";
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                Tipo_Info_SGA directorio = Tipo_Info_SGA.FTPConnectionEvidenciaIncR;
                
                var serverInfo = await _info_SGA.Execute(directorio);
                string host = "", port = "", user = "", pass = "", dir = "";
                foreach (var item in serverInfo.Info)
                {
                    switch (item.Key)
                    {
                        case "host": host = item.Value; break;
                        case "port": port = item.Value; break;
                        case "user": user = item.Value; break;
                        case "password": pass = item.Value; break;
                        case "directory": dir = item.Value; break;
                    }
                }

                /* Save file in application server to decrypt */
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre/";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission/" + userInfo.NombreUsuario + "/";
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
                /*New file downloaded name*/
                var downloadedFilePath = archivePath + fileName;
                /*Copy data to new temp file*/
                var fileStream = System.IO.File.Create(downloadedFilePath);
                using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(dir);

                    //Descargar archivo de sftp a nuevo archivo temporal creado
                    sftp.DownloadFile(fileName, fileStream);

                    sftp.Dispose();
                }
                //ftpStream.CopyTo(fileStream);
                fileStream.Close();

                return downloadedFilePath;
            }
        }
        private async Task EncryptFileAsync(string inputFile, string outputFile)
        {
            var dataToEncrypt = await _info_SGA.Execute(Tipo_Info_SGA.FileEncryption);

            var key = new byte[32];
            var IV = new byte[16];

            foreach (var item in dataToEncrypt.Info)
            {
                switch (item.Key)
                {
                    case "key": key = Encoding.UTF8.GetBytes(item.Value); break;
                    case "iv": IV = Encoding.UTF8.GetBytes(item.Value); break;
                }
            }

            string cryptFile = outputFile;
            FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

            RijndaelManaged RMCrypto = new RijndaelManaged();

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateEncryptor(key, IV),
                CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            int data;
            while ((data = fsIn.ReadByte()) != -1)
                cs.WriteByte((byte)data);


            fsIn.Close();
            cs.Close();
            fsCrypt.Close();
        }
        private async Task DecryptFileAsync(string inputFile, string outputFile)
        {
            var dataToEncrypt = await _info_SGA.Execute(Tipo_Info_SGA.FileEncryption);

            var key = new byte[32];
            var IV = new byte[16];

            foreach (var item in dataToEncrypt.Info)
            {
                switch (item.Key)
                {
                    case "key": key = Encoding.UTF8.GetBytes(item.Value); break;
                    case "iv": IV = Encoding.UTF8.GetBytes(item.Value); break;
                }
            }
            RijndaelManaged RMCrypto = new RijndaelManaged();

            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

            CryptoStream cs = new CryptoStream(fsCrypt,
                RMCrypto.CreateDecryptor(key, IV),
                CryptoStreamMode.Read);

            FileStream fsOut = new FileStream(outputFile, FileMode.Create);

            int data;
            while ((data = cs.ReadByte()) != -1)
                fsOut.WriteByte((byte)data);

            fsOut.Close();
            cs.Close();
            fsCrypt.Close();

        }
    }
}