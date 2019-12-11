using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using GestionAmbiental.Application.UseCases.Inspecciones.AmpliarFechaObservacion;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarCabeceras;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.EditarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.Registro.RegistrarInspeccion;
using GestionAmbiental.Application.UseCases.Inspecciones.SelectItems;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.Registro
{
    public class InspeccionesController : Controller
    {
        private readonly IConsultarCabeceras _consultarCabeceras;
        private readonly IConsultarCabecera _consultarCabecera;
        private readonly ISelectItems _selectItems;
        private readonly IRegistrarInspeccionCabecera _registrarInspeccionCabecera;
        private readonly IRegistrarInspeccionObservacion _registrarInspeccionObservacion;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConsultarInspeccionDetalle _consultarInspeccionDetalle;
        private readonly IEditarInspeccionCabecera _editarInspeccionCabecera;
        private readonly IAmpliarFechaObservacion _ampliarFechaObservacion;

        private const string CarpetaEvidencias = "ftp://127.0.0.1:2020/InspeccionesAmbientales/EvidenciasRegistro/";
        public InspeccionesController(
            IConsultarCabeceras consultarCabeceras,
            ISelectItems selectItems,
            IRegistrarInspeccionCabecera registrarInspeccionCabecera,
            IHostingEnvironment hostingEnvironment,
            IRegistrarInspeccionObservacion registrarInspeccionObservacion,
            IConsultarInspeccionDetalle consultarInspeccionDetalle,
            IConsultarCabecera consultarCabecera,
            IEditarInspeccionCabecera editarInspeccionCabecera,
            IAmpliarFechaObservacion ampliarFechaObservacion)
        {
            _consultarCabeceras = consultarCabeceras;
            _selectItems = selectItems;
            _registrarInspeccionCabecera = registrarInspeccionCabecera;
            _hostingEnvironment = hostingEnvironment;
            _registrarInspeccionObservacion = registrarInspeccionObservacion;
            _consultarInspeccionDetalle = consultarInspeccionDetalle;
            _consultarCabecera = consultarCabecera;
            _editarInspeccionCabecera = editarInspeccionCabecera;
            _ampliarFechaObservacion = ampliarFechaObservacion;
        }
        public async Task<IActionResult> Resumen()
        {
            var selectItemsResumen = await _selectItems.ItemVistaResumen();

            var filtroGerencias = selectItemsResumen.GerenciaEjecutora.OrderBy(x=>x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();
            filtroGerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.Filtro = filtroGerencias;
            return View();
        }
        public async Task<IActionResult> GetTablaResumenFiltrada(string filtroFecha, int filtroGerencia, string filtroCodigo,int pagina,int rpp)
        {
            DateTime fecha;
            if (filtroFecha != null)
                fecha = new DateTime(Int32.Parse(filtroFecha.Split("-")[0]), Int32.Parse(filtroFecha.Split("-")[1]), Int32.Parse(filtroFecha.Split("-")[2]));
            else
                fecha = (DateTime)SqlDateTime.MinValue;

            filtroCodigo = filtroCodigo ?? "";
            var datosTabla = await _consultarCabeceras.Execute(pagina, rpp, fecha, filtroGerencia, filtroCodigo);
            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.nCabeceras%rpp!=0 ? (datosTabla.nCabeceras/rpp)+1 : datosTabla.nCabeceras/rpp;
            if (datosTabla.nCabeceras > 0)
            {
                foreach (var fila in datosTabla.cabeceras)
                {

                    filasTabla += "<tr>";
                    /*switch (fila.Estado)
                    {
                        case "Pendiente": filasTabla += "<td class=\"alert-column\"><img src=\"~/img/icons/alert-icon.png\"></td>"; break;
                        case "Vencido": filasTabla += "<td class=\"alert-column\"><img src=\"~/img/icons/pending-icon.png\"></td>"; break;
                        default: filasTabla += "<td class=\"alert-column\"></td>"; break;
                    }*/
                    double avance = fila.NObsImplementadas!=0? Math.Round((float)fila.NObsImplementadas / fila.NObservaciones * 100,1) : 0;
                    filasTabla += "<td><a href=\"/Inspecciones/EditarInspeccion?IdCabecera=" + fila.IdCabecera + "\">"+fila.Codigo+"</a></td>";
                    filasTabla += "<td>" + fila.GerenEjecMar + "</td>";
                    filasTabla += "<td>" + fila.EmpContratMarc + "</td>";
                    filasTabla += "<td>" + fila.LugarInspec + "</td>";
                    filasTabla += "<td>" + fila.FechaInspec.ToString() + "</td>";
                    filasTabla += "<td>" + fila.NObservaciones + "</td>";
                    filasTabla += "<td><div class=\"progress\"><div class=\"progress-bar bg-info\" role=\"progressbar\" style=\"width:" + avance+ "%; color: black;font-weight: bold;\" aria-valuemin=\"0\" aria-valuemax=\"100\">" + avance + "%</div></div></td>";
                    filasTabla += "</tr>";
                }
                
                paginador += "<ul class=\"pagination\">";
                for (int i = 1; i <= nPaginas; i++)
                {
                    if (i == pagina)
                        paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    else
                        paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar("+ i +")\">" + i + "</a></li>";
                }
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
        public async Task<IActionResult> NuevaInspeccion()
        {
            //Cargar observaciones si se tiene
            
            var selectItemsResumen = await _selectItems.ItemVistaNuevaInspeccion();

            var gerenciasEjecutorasMC = selectItemsResumen.GerenciaEjecutora.OrderBy(x=>x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();
            gerenciasEjecutorasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
            gerenciasEjecutorasMC.RemoveAt(1);
            
            var empresasMC = selectItemsResumen.EmpContratMarcobre.OrderBy(x=>x.Razon_Social).Select(g => new SelectListItem() { Value = g.Id_Empresa_Contratista.ToString(), Text = g.Razon_Social }).ToList();
            empresasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
                        
            var responsables = selectItemsResumen.Usuarios.OrderBy(x=>x.Nombres).Select(g => new SelectListItem() { Value = g.Id_Usuario.ToString(), Text = g.Nombres + " "+ g.Apellidos }).ToList();
            responsables.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var tiposInspeccion = new List<SelectListItem>();
            tiposInspeccion.Insert(0, new SelectListItem("OTRO", "OTRO", false));
            tiposInspeccion.Insert(0, new SelectListItem("NO PLANEADA", "NO PLANEADA", false));
            tiposInspeccion.Insert(0, new SelectListItem("PLANEADA", "PLANEADA", false));
            tiposInspeccion.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

            var idCabecera = Convert.ToInt32(TempData["idCabecera"]);
            if (idCabecera != 0)
            {
                var filaObservacion = "";
                var observaciones = await _consultarInspeccionDetalle.Execute(idCabecera);
                ViewBag.nObservacioness = observaciones.Count;
               
                foreach (var observacion in observaciones)
                {
                    var evidencia = "";
                    for (int i = 0; i < observacion.Evidencia_Condicion.Count; i++)
                    {
                        if (i == 0)
                            evidencia = "<a href=\"" + observacion.Evidencia_Condicion[i] + "\">Evidencia " + (i+1) + "</a>";
                        else
                            evidencia += "<a href=\"" + observacion.Evidencia_Condicion[i] + "\">, Evidencia " + (i+1) + "</a>";
                    }
                    filaObservacion += "<tr>";
                    filaObservacion += "<td style=\"display: none; \">" + observacion.Id_Inspeccion_Detalle + "</td>";
                    filaObservacion += "<td>" + observacion.Descripcion_Condicion + "</td>";
                    filaObservacion += "<td>" + observacion.Tipo_Condicion + "</td>";
                    filaObservacion += "<td>" + evidencia + "</td>";
                    filaObservacion += "<td>" + observacion.Nivel_Riesgo_Ambiental + "</td>";
                    filaObservacion += "<td>" + observacion.Descripcion_Accion + "</td>";
                    filaObservacion += "<td>" + observacion.Responsable_Marcobre + "</td>";
                    filaObservacion += "<td>" + observacion.Responsable_Contratista + "</td>";
                    filaObservacion += "<td>" + observacion.Fecha_Programada + "</td>";
                    filaObservacion += "</tr>";

                }
                ViewBag.ContenidoTabla = filaObservacion;
            }

            ViewBag.GerenciaMC = gerenciasEjecutorasMC;
            ViewBag.EmpresasMC = empresasMC;
            ViewBag.Responsables = responsables;
            ViewBag.Tipo_Inspeccion = tiposInspeccion;
            return View(new NuevaInspeccionModel());
        }
        [HttpPost]
        public async Task<IActionResult> NuevaInspeccion(NuevaInspeccionModel model)
        {
            
            if (ModelState.IsValid)
            {
                int idCabecera = await _registrarInspeccionCabecera.Execute(
                model.Objetivo,User.Identity.Name, model.Id_Gerencia_Ejecutora, model.Id_Empresa_Contratista, model.Lugar_Inspeccionado, model.Fecha_Inspeccion, model.Hora_Inspeccion,
                model.Tipo_Inspeccion, model.Id_Responsable_Lugar,model.Responsable_Inspeccion, model.Inspectores,model.opcion);
                if(model.opcion.Equals("Observacion"))
                    return RedirectToAction("NuevaObservacion", new { IdCabecera =  idCabecera});
            }
                return RedirectToAction("Resumen");
        }
        
        public async Task<IActionResult> NuevaObservacion(int IdCabecera)
        {

            var selectItemsResumen = await _selectItems.ItemVistaNuevaObservacion();

            var tiposActoMA = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x =>x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Condicion.ToString(), Text = g.Descripcion }).ToList();
            tiposActoMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var tiposCondicionMA = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Condicion.ToString(), Text = g.Descripcion }).ToList();
            tiposCondicionMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var tiposActoSEG = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Condicion.ToString(), Text = g.Descripcion }).ToList();
            tiposActoSEG.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var tiposCondicionSEG = selectItemsResumen.TipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSS.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Tipo_Condicion.ToString(), Text = g.Descripcion }).ToList();
            tiposCondicionSEG.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var responsables = selectItemsResumen.Usuarios.OrderBy(x=>x.Nombres).Select(g => new SelectListItem() { Value = g.Id_Usuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            responsables.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var asesoresMA = selectItemsResumen.Usuarios.Where(x=>x.Id_Rol.Equals((int)InspeccionRol.VerificadorMAP)).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.Id_Usuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            asesoresMA.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var asesoresSeguridad = selectItemsResumen.Usuarios.Where(x => x.Id_Rol.Equals((int)InspeccionRol.VerificadorSSO)).OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.Id_Usuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            asesoresSeguridad.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            ViewBag.TiposActoMA = tiposActoMA;
            ViewBag.TiposCondicionMA = tiposCondicionMA;
            ViewBag.TiposActoSEG = tiposActoSEG;
            ViewBag.TiposCondicionSEG = tiposCondicionSEG;

            ViewBag.Responsables = responsables;
            ViewBag.idCabecera = IdCabecera;

            ViewBag.AsesoresMA = asesoresMA;
            ViewBag.AsesoresSeguridad = asesoresSeguridad;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevaObservacion(NuevaObservacionModel model)
        {
            if (ModelState.IsValid)
            {
                var evidencias = new List<string>();
                if (model.Evidencias != null)
                {
                    for (int i = 0; i < model.Evidencias.Count; i++)
                    {                       
                        var newfileName = CreateUniqueFileName(i + 1)+".zip";
                        var filePath = UploadZipFile(model.Evidencias[i], newfileName);
                        evidencias.Add(filePath);
                    }
                }
                var idObservacion = await _registrarInspeccionObservacion.Execute(
                            model.idCabecera,
                            model.TipoInspeccion,
                            model.Asesor_MA_Seguridad,
                            model.CondicionIdentificada,
                            model.TipoCondicion,
                            evidencias,
                            model.NiveRiesgo,
                            model.AccionTomada,
                            model.ResponsableMarc,
                            model.ResponsableCont,
                            model.FechaProgramada,
                            User.Identity.Name);
                return RedirectToActionPermanent("EditarInspeccion", new { IdCabecera = model.idCabecera});
            }
            else
                return RedirectToAction("NuevaObservacion", model);            
        }
        public async Task<IActionResult> EditarInspeccion(int IdCabecera)
        {
            var selectItemsResumen = await _selectItems.ItemVistaEditarInspeccion();

            var gerenciasEjecutorasMC = selectItemsResumen.GerenciaEjecutora.OrderBy(x=>x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Gerencia.ToString(), Text = g.Descripcion }).ToList();
            gerenciasEjecutorasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));
            gerenciasEjecutorasMC.RemoveAt(1);

            var empresasMC = selectItemsResumen.EmpContratMarcobre.OrderBy(x=>x.Razon_Social).Select(g => new SelectListItem() { Value = g.Id_Empresa_Contratista.ToString(), Text = g.Razon_Social }).ToList();
            empresasMC.Insert(0, new SelectListItem("No Aplica", "1"));
            empresasMC.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var responsables = selectItemsResumen.Usuarios.OrderBy(x=>x.Nombres).Select(g => new SelectListItem() { Value = g.Id_Usuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            responsables.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var tiposInspeccion = new List<SelectListItem>();
            tiposInspeccion.Insert(0, new SelectListItem("OTRO", "OTRO", false));
            tiposInspeccion.Insert(0, new SelectListItem("NO PLANEADA", "NO PLANEADA", false));
            tiposInspeccion.Insert(0, new SelectListItem("PLANEADA", "PLANEADA", false));
            tiposInspeccion.Insert(0, new SelectListItem("--SELECCIONE--", "", true));

            if (IdCabecera != 0)
            {
                var filaObservacion = "";
                var observaciones = await _consultarInspeccionDetalle.Execute(IdCabecera);
                ViewBag.nObservacioness = observaciones.Count;

                foreach (var observacion in observaciones)
                {
                    var evidencia = "";
                    for (int i = 0; i < observacion.Evidencia_Condicion.Count; i++)
                    {
                      if (i == 0)
                            evidencia = evidencia+ "<a href=\"/Inspecciones/Download?fileName=" + observacion.Evidencia_Condicion[i] + "\">Evidencia</a>";
                        else
                            evidencia = evidencia+ "<a href=\"/Inspecciones/Download?fileName=" + observacion.Evidencia_Condicion[i] + "\">, Evidencia</a>";
                    }
                    filaObservacion += "<tr>";
                    filaObservacion += "<td style=\"display: none; \">" + observacion.Id_Inspeccion_Detalle + "</td>";
                    filaObservacion += "<td>" + observacion.Tipo_Inspeccion + "</td>";
                    filaObservacion += "<td>" + observacion.Asesor_MA_Seguridad + "</td>";
                    filaObservacion += "<td>" + observacion.Descripcion_Condicion + "</td>";
                    filaObservacion += "<td>" + observacion.Tipo_Condicion + "</td>";
                    filaObservacion += "<td>" + evidencia + "</td>";
                    switch (observacion.Nivel_Riesgo_Ambiental)
                    {
                        case "Tolerable": filaObservacion += "<td><span class=\"badge badge-success\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                        case "Alerta": filaObservacion += "<td><span class=\"badge badge-warning\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                        case "No Aceptable": filaObservacion += "<td><span class=\"badge badge-danger\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                        case "Bajo": filaObservacion += "<td><span class=\"badge badge-success\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                        case "Medio": filaObservacion += "<td><span class=\"badge badge-warning\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                        case "Alto": filaObservacion += "<td><span class=\"badge badge-danger\">" + observacion.Nivel_Riesgo_Ambiental + "</span></td>"; break;
                        default: filaObservacion += "<td><span class=\"badge badge-secondary\">" + observacion.Nivel_Riesgo_Ambiental + "</span>"; break;
                    }
                    filaObservacion += "<td>" + observacion.Descripcion_Accion + "</td>";
                    filaObservacion += "<td>" + observacion.Responsable_Marcobre + "</td>";
                    filaObservacion += "<td>" + observacion.Responsable_Contratista + "</td>";
                    filaObservacion += "<td>" + observacion.Fecha_Programada.ToShortDateString() + "</td>";
                    if (observacion.Estado.Equals(EstadosInspeccion.Vencido.ToString()))
                    {
                        filaObservacion += "<td><a class=\"btn btn-primary\" style=\"color:white\" onclick=\"ampliarInspeccion("+observacion.Id_Inspeccion_Detalle+")\">Ampliar Fecha</a></td>";
                    }
                    filaObservacion += "</tr>";

                }
                ViewBag.ContenidoTabla = filaObservacion;
            }


            ViewBag.GerenciaMC = gerenciasEjecutorasMC;
            ViewBag.EmpresasMC = empresasMC;
            ViewBag.Responsables = responsables;
            ViewBag.Tipo_Inspeccion = tiposInspeccion;
            var cabecera = await _consultarCabecera.Execute(IdCabecera);
            var model = new NuevaInspeccionModel();

            if (cabecera != null)
            {
                model.Id_Inspeccion_Cabecera = IdCabecera;
                model.Codigo = cabecera.Codigo;
                model.Responsable_Inspeccion = cabecera.Responsable_Inspeccion;
                model.Tipo_Inspeccion = cabecera.Tipo_Inspeccion;
                model.Id_Empresa_Contratista = cabecera.idEmpContratMarc;
                model.Id_Gerencia_Ejecutora = cabecera.idGerenciaEjecMar;
                model.Lugar_Inspeccionado = cabecera.LugarInspec;
                model.Id_Responsable_Lugar = cabecera.idResponsableLugar;
                model.Fecha_Inspeccion = cabecera.FechaInspec.Year + "-" + (cabecera.FechaInspec.Month.ToString().Length>1? cabecera.FechaInspec.Month.ToString():"0"+ cabecera.FechaInspec.Month.ToString()) + "-" + (cabecera.FechaInspec.Day.ToString().Length > 1 ? cabecera.FechaInspec.Day.ToString() : "0" + cabecera.FechaInspec.Day.ToString());
                model.Hora_Inspeccion = (cabecera.FechaInspec.Hour.ToString().Length > 1 ? cabecera.FechaInspec.Hour.ToString() : "0" + cabecera.FechaInspec.Hour.ToString()) + ":" + (cabecera.FechaInspec.Minute.ToString().Length > 1 ? cabecera.FechaInspec.Minute.ToString() : "0" + cabecera.FechaInspec.Minute.ToString());
                model.Objetivo = cabecera.Objetivo;
                model.Inspectores = cabecera.Inspectores;
                model.Estado = cabecera.Estado;                
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditarInspeccion(NuevaInspeccionModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Estado.Equals(EstadosInspeccion.Pendiente.ToString()))
                {
                    await _editarInspeccionCabecera.Execute(
                    model.Id_Inspeccion_Cabecera,
                    model.Objetivo,
                    model.Id_Gerencia_Ejecutora,
                    model.Id_Empresa_Contratista,
                    model.Lugar_Inspeccionado,
                    model.Fecha_Inspeccion,
                    model.Hora_Inspeccion,
                    model.Id_Responsable_Lugar,
                    model.Inspectores,
                    model.opcion,
                    User.Identity.Name);
                    if (model.opcion.Equals("Observacion"))
                        return RedirectToAction("NuevaObservacion", new { IdCabecera = model.Id_Inspeccion_Cabecera });
                    else if (model.opcion.Equals("Guardar"))
                        return RedirectToAction("NotificarInspección","Notificaciones", new { IdCabecera = model.Id_Inspeccion_Cabecera });
                }                
            }
            return RedirectToAction("Resumen");
        }
        [HttpPost]
        public async Task<IActionResult> AmpliarFechaObservacion(int idCabecera, int idObservacion, string fecha)
        {
            await _ampliarFechaObservacion.Execute(idObservacion, fecha,User.Identity.Name);
            return RedirectToAction("EditarInspeccion", new { IdCabecera = idCabecera });
        }
        private string CreateUniqueFileName(int nFile)
        {
            var fileName = "Evidencia__Registro_Observacion_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            return fileName;
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
        public IActionResult Download(string fileName)
        {
            try
            {
                /* Create an FTP Request */
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(CarpetaEvidencias + fileName);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential("SGA", "Pa$$W0rd");
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                var ftpStream = ftpResponse.GetResponseStream();


                return File(ftpStream, GetContentType(fileName),Path.GetFileName(fileName));
            }
            catch (Exception)
            {
                return Content("No se encontró el archivo");
            }
        }
        public string UploadFTPFile(string path, string newFileName)
        {            
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(CarpetaEvidencias + newFileName);

            ftpRequest.Credentials = new NetworkCredential("SGA", "Pa$$W0rd");
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;

            using (FileStream fs = System.IO.File.OpenRead(path))
            {

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                Stream requestStream = ftpRequest.GetRequestStream();
                requestStream.WriteAsync(buffer, 0, buffer.Length);
                requestStream.FlushAsync();
                requestStream.Close();
            }
            return newFileName;
        }
        public string UploadZipFile(IFormFile file,string newFileName)
        {
            var tempPath = Path.GetTempPath();
            var SGATempPath = tempPath+"SGA-Marcobre/";
            if (!Directory.Exists(SGATempPath))
            {
                Directory.CreateDirectory(SGATempPath);
            }
            var filePath = SGATempPath + "submission/"+User.Identity.Name.Split("\\")[1]+"/";
            var archiveFile = SGATempPath + "zip/"+ User.Identity.Name.Split("\\")[1] + "/archive.zip";
            var archivePath = SGATempPath + "zip/"+ User.Identity.Name.Split("\\")[1] + "/";

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
            return UploadFTPFile(archiveFile, newFileName);
        }
    }
}