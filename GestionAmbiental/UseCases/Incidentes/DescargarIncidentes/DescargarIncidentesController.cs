using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetInvestigacionesFinales;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.GetAcciones;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportesPreliminares;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.Util.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Incidentes.DescargarIncidentes
{
    public class DescargarIncidentesController : Controller
    {
        private readonly IGestorArchivos _gestorArchivos;
        private readonly IGestorEncriptado _gestorEncriptado;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IGetAccionesIncidenteInvestigacion _getAccionesIncidenteMayor;
        private readonly IGetAccionesIncidente _getAccionesIncidenteMenor;
        private readonly IGetInvestigacionesFinales _getIncidentesMayores;
        private readonly IGetReportesPreliminares _getIncidentesMenores;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IDataProtector _protector;

        public DescargarIncidentesController(
            IGestorArchivos gestorArchivos,
            IGestorEncriptado gestorEncriptado,
            IWebHostEnvironment hostingEnvironment,
            IGetAccionesIncidenteInvestigacion getAccionesIncidenteMayor,
            IGetAccionesIncidente getAccionesIncidenteMenor,
            IGetInvestigacionesFinales getIncidentesMayores,
            IGetReportesPreliminares getIncidentesMenores,
            IGetInfo_SGA info_SGA,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _gestorArchivos = gestorArchivos;
            _gestorEncriptado = gestorEncriptado;
            _hostingEnvironment = hostingEnvironment;
            _getAccionesIncidenteMayor = getAccionesIncidenteMayor;
            _getAccionesIncidenteMenor = getAccionesIncidenteMenor;
            _getIncidentesMayores = getIncidentesMayores;
            _getIncidentesMenores = getIncidentesMenores;
            _info_SGA = info_SGA;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.ReportePreliminarIdRouteValue);
        }

        public async Task<IActionResult> DescargarFechas(int tipoIncidente, int nivelIncidente, int gerencia, int asesor, string FechaInicio, string FechaFin,bool incluirEvidencias)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?tipoIncidente=" + tipoIncidente + "&nivelIncidente=" + nivelIncidente + "&gerencia=" + gerencia + "&asesor=" + asesor + "&FechaInicio=" + FechaInicio + "&FechaFin=" + FechaFin+ "&incluirEvidencias=" + incluirEvidencias});
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;

                var accionesIncidenteMayor = await _getAccionesIncidenteMayor.Execute();
                var accionesIncidenteMenor = await _getAccionesIncidenteMenor.Execute();

                var incidentesMenores = await _getIncidentesMenores.Execute(tipoIncidente, nivelIncidente, gerencia, asesor, FechaInicio, FechaFin);
                var incidentesMayores = await _getIncidentesMayores.Execute(tipoIncidente, nivelIncidente, gerencia, asesor, FechaInicio, FechaFin);

                accionesIncidenteMenor = accionesIncidenteMenor.Where(x => incidentesMenores.Reportes.Exists(y => y.Id_Reporte_Preliminar == x.Id_Reporte_Preliminar)).ToList();
                accionesIncidenteMayor = accionesIncidenteMayor.Where(x => incidentesMayores.Investigaciones.Exists(y => y.Id_Investigacion_Final == x.Id_Investigacion_Final)).ToList();

                var registros = new List<IncidenteRegistro>();
                //Primer recorrido para descargar todas las imágenes en la carpeta temporal
                var listaEvidenciasxAccion = new List<EvidenciaAccion>();
                
                //Asignación de datos de incidente menor
                foreach (var accionMenor in accionesIncidenteMenor)
                {
                    var incidenteMenor = incidentesMenores.Reportes.Find(x => x.Id_Reporte_Preliminar == accionMenor.Id_Reporte_Preliminar);
                    var input = new IncidenteRegistro
                    {
                        IdAccion = accionMenor.Id_Accion_Correctiva_Incidente,
                        IdIncidente = accionMenor.Id_Reporte_Preliminar,
                        Codigo = incidenteMenor.Codigo,
                        Descripcion_Incidente = incidenteMenor.Descripcion_Incidente,
                        Nivel_Incidente = incidenteMenor.Nivel_Incidente,
                        Fecha_Incidente = incidenteMenor.Fecha_Incidente.Value,
                        Lugar_Incidente = incidenteMenor.Lugar_Incidente,
                        Producto_Derramado = incidenteMenor.Producto_Derramado,
                        Accion_Inmediata = accionMenor.Tipo_Accion == "Acción Inmediata" ? accionMenor.Accion_Inmediata : null,
                        Plan_Accion = accionMenor.Tipo_Accion == "Acción Correctiva" ? accionMenor.Accion_Inmediata : null,
                        Responsable_Levantamiento = accionMenor.Responsable_Marcobre,
                        Fecha_Programada = accionMenor.Fecha,
                        Fecha_Cierre = accionMenor.Fecha_Verificacion,
                        Estado = accionMenor.Estado,
                        Empresa = incidenteMenor.Razon_Social_Contratista,
                        Gerencia_Responsable = incidenteMenor.Gerencia_Responsable,
                        Supervisor = incidenteMenor.Supervisor_MA,
                        Evidencia = incidenteMenor.Evidencia.Count > 0 ? incidenteMenor.Evidencia[0]:null,
                        Evidencia_Levantamiento = accionMenor.Evidencia
                    };
                    var evidenciasxAccion = new EvidenciaAccion
                    {
                        IdAccion = accionMenor.Id_Accion_Correctiva_Incidente,
                        IdIncidente = accionMenor.Id_Reporte_Preliminar,
                        Codigo = incidenteMenor.Codigo,
                        IncidenteMenor = true
                    };
                    if (input.Evidencia_Levantamiento != "[]" && input.Evidencia_Levantamiento != null)
                    {
                        evidenciasxAccion.NombreEvidenciaImplementacion = (string)JArray.Parse(input.Evidencia_Levantamiento)[0];
                    }
                    listaEvidenciasxAccion.Add(evidenciasxAccion);
                    registros.Add(input);
                }
                //Asignación de datos de incidente mayor
                foreach (var accionMayor in accionesIncidenteMayor)
                {
                    var incidenteMayor = incidentesMayores.Investigaciones.Find(x => x.Id_Investigacion_Final == accionMayor.Id_Investigacion_Final);
                    var evidenciasIncidente = new List<string>();
                    if (incidenteMayor.Evidencia_Papel != null)
                    {
                        evidenciasIncidente.AddRange(incidenteMayor.Evidencia_Papel);
                    }
                    if (incidenteMayor.Evidencia_Parte != null)
                    {
                        evidenciasIncidente.AddRange(incidenteMayor.Evidencia_Parte);
                    }
                    if (incidenteMayor.Evidencia_Persona != null)
                    {
                        evidenciasIncidente.AddRange(incidenteMayor.Evidencia_Persona);
                    }
                    if (incidenteMayor.Evidencia_Posicion != null)
                    {
                        evidenciasIncidente.AddRange(incidenteMayor.Evidencia_Posicion);
                    }
                    var input = new IncidenteRegistro
                    {
                        IdAccion = accionMayor.Id_Accion_Correctiva_Incidente,
                        IdIncidente = accionMayor.Id_Investigacion_Final.Value,
                        Codigo = incidenteMayor.Codigo,
                        Descripcion_Incidente = incidenteMayor.Descripcion_Incidente,
                        Nivel_Incidente = incidenteMayor.Nivel_Incidente,
                        Fecha_Incidente = incidenteMayor.Fecha_Incidente.Value,
                        Lugar_Incidente = incidenteMayor.Lugar_Incidente,
                        //Producto_Derramado = incidenteMayor.Producto,
                        Accion_Inmediata = accionMayor.Tipo_Accion == "Acción Inmediata" ? accionMayor.Accion_Inmediata : null,
                        Plan_Accion = accionMayor.Tipo_Accion == "Acción Correctiva" ? accionMayor.Accion_Inmediata : null,
                        Responsable_Levantamiento = accionMayor.Responsable_Marcobre,
                        Fecha_Programada = accionMayor.Fecha,
                        Fecha_Cierre = accionMayor.Fecha_Verificacion,
                        Estado = accionMayor.Estado,
                        Empresa = incidenteMayor.Razon_Social_Contratista,
                        Gerencia_Responsable = incidenteMayor.Gerencia_Responsable,
                        Supervisor = incidenteMayor.Supervisor_MA,
                        Evidencia = evidenciasIncidente.Count > 0 ? evidenciasIncidente[0] : null,
                        Evidencia_Levantamiento = accionMayor.Evidencia,
                    };

                    var evidenciasxAccion = new EvidenciaAccion
                    {
                        IdAccion = accionMayor.Id_Accion_Correctiva_Incidente,
                        IdIncidente = accionMayor.Id_Investigacion_Final.Value,
                        Codigo = incidenteMayor.Codigo,
                        IncidenteMenor = false
                    };
                    if (input.Evidencia_Levantamiento != "[]" && input.Evidencia_Levantamiento != null)
                    {
                        evidenciasxAccion.NombreEvidenciaImplementacion = (string)JArray.Parse(input.Evidencia_Levantamiento)[0];
                    }
                    registros.Add(input);
                    listaEvidenciasxAccion.Add(evidenciasxAccion);
                }

                registros = registros.OrderBy(x => x.Codigo).ToList();
                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "Incidentes");
                string sFileName = @"Formato Reporte de IA.xlsx";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);
                // Load the template into a NPOI workbook
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheet = templateWorkbook.GetSheet("IA");
                //->Fila acciones en el formato
                int fila = 1;
                //->Cell Style
                XSSFCellStyle estiloNormal = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                XSSFCellStyle estiloImplementacion = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                XSSFFont fuenteImplementacion = (XSSFFont)templateWorkbook.CreateFont();
                XSSFCellStyle estiloVerificacion = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                XSSFFont fuenteVerificacion = (XSSFFont)templateWorkbook.CreateFont();
                XSSFCellStyle estiloVencido = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                XSSFFont fuenteVencido = (XSSFFont)templateWorkbook.CreateFont();
                XSSFCellStyle estiloAprobado = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                XSSFFont fuenteAprobado = (XSSFFont)templateWorkbook.CreateFont();
                XSSFCellStyle estiloRechazado = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                XSSFFont fuenteRechazado = (XSSFFont)templateWorkbook.CreateFont();

                var estilos = new List<Estilo>();
                estilos.Add(new Estilo("Pendiente de Implementación", templateWorkbook, (byte)255, (byte)240, (byte)128, IndexedColors.DarkYellow.Index));
                estilos.Add(new Estilo("Pendiente de Verificación", templateWorkbook, (byte)128, (byte)255, (byte)240, IndexedColors.DarkBlue.Index));
                estilos.Add(new Estilo("Vencido", templateWorkbook, (byte)255, (byte)103, (byte)103, IndexedColors.DarkRed.Index));
                estilos.Add(new Estilo("Aprobado", templateWorkbook, (byte)159, (byte)255, (byte)128, IndexedColors.DarkGreen.Index));
                estilos.Add(new Estilo("Rechazado", templateWorkbook, (byte)255, (byte)222, (byte)128, IndexedColors.Orange.Index));

                estiloNormal.WrapText = true;
                estiloNormal.VerticalAlignment = VerticalAlignment.Center;
                estiloNormal.BorderTop = BorderStyle.Thin;
                estiloNormal.BorderBottom = BorderStyle.Thin;
                estiloNormal.BorderLeft = BorderStyle.Thin;
                estiloNormal.BorderRight = BorderStyle.Thin;
                estiloNormal.Alignment = HorizontalAlignment.Center;


                //índice de la primera imagen a insertar en la colección de images del workbook
                int imageIndex = -1;
                //indice de observacion
                int obsIndex = 1;

                

                //1. Vaciar carpeta temporal del usuario 
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "Descarga Temporal Gestiona V2\\";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                    Directory.CreateDirectory(filePath);
                }
                var archiveFile = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\archive.zip";
                if (Directory.Exists(archiveFile))
                {
                    Directory.Delete(archiveFile, true);
                    Directory.CreateDirectory(archiveFile);
                }
                var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";
                if (Directory.Exists(archivePath))
                {
                    Directory.Delete(archivePath, true);
                    Directory.CreateDirectory(archivePath);
                }
                /*New file downloaded name*/
                /*Copy data to new temp file*/
                if (incluirEvidencias)
                {
                    //2. Abrir conexiones SFTP para Evidencias Registro e Implementacion
                    var serverInfoIncidenteMenor = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaIncR);
                    var serverInfoIncidenteMayor = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaIncInvAccion);

                    string hostIncMenor = "", portIncMenor = "", userIncMenor = "", passIncMenor = "", dirIncMenor = "";
                    foreach (var item in serverInfoIncidenteMenor.Info)
                    {
                        switch (item.Key)
                        {
                            case "host": hostIncMenor = item.Value; break;
                            case "port": portIncMenor = item.Value; break;
                            case "user": userIncMenor = item.Value; break;
                            case "password": passIncMenor = item.Value; break;
                            case "directory": dirIncMenor = item.Value; break;
                        }
                    }

                    string hostIncMayor = "", portIncMayor = "", userIncMayor = "", passIncMayor = "", dirIncMayor = "";
                    foreach (var item in serverInfoIncidenteMayor.Info)
                    {
                        switch (item.Key)
                        {
                            case "host": hostIncMayor = item.Value; break;
                            case "port": portIncMayor = item.Value; break;
                            case "user": userIncMayor = item.Value; break;
                            case "password": passIncMayor = item.Value; break;
                            case "directory": dirIncMayor = item.Value; break;
                        }
                    }
                    using (SftpClient sftp = new SftpClient(hostIncMenor, Convert.ToInt32(portIncMenor), userIncMenor, passIncMenor))
                    {

                        sftp.Connect();
                        sftp.ChangeDirectory(dirIncMenor);

                        //Descargar archivo de sftp a nuevo archivo temporal creado
                        foreach (var item in listaEvidenciasxAccion)
                        {
                            if (item.NombreEvidenciaImplementacion != null && item.IncidenteMenor)
                            {
                                var downloadedFilePath = archivePath + item.NombreEvidenciaImplementacion;
                                var fileStream = System.IO.File.Create(downloadedFilePath);
                                sftp.DownloadFile(item.NombreEvidenciaImplementacion, fileStream);
                                fileStream.Dispose();

                                //Desencriptar
                                var pathDesencriptado = archivePath + item.NombreEvidenciaImplementacion + "_desencriptado.zip";
                                await _gestorEncriptado.DecryptFileAsync(downloadedFilePath, pathDesencriptado);

                                //Descomprimir
                                var pathDescomprimido = filePath;
                                if (!pathDescomprimido.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                                    pathDescomprimido += Path.DirectorySeparatorChar;

                                using (ZipArchive archive = ZipFile.OpenRead(pathDesencriptado))
                                {
                                    foreach (ZipArchiveEntry entry in archive.Entries)
                                    {
                                        pathDescomprimido = Path.GetFullPath(Path.Combine(pathDescomprimido, "EvLevIncidenteMenor_" + item.IdAccion + "_" + entry.FullName));
                                        if (pathDescomprimido.StartsWith(pathDescomprimido, StringComparison.Ordinal))
                                            entry.ExtractToFile(pathDescomprimido);
                                    }
                                }
                                item.RutaEvidenciaImplementacion = pathDescomprimido;
                            }
                        }

                        sftp.Dispose();
                    }
                    using (SftpClient sftp = new SftpClient(hostIncMayor, Convert.ToInt32(portIncMayor), userIncMayor, passIncMayor))
                    {

                        sftp.Connect();
                        sftp.ChangeDirectory(dirIncMayor);

                        //Descargar archivo de sftp a nuevo archivo temporal creado
                        foreach (var item in listaEvidenciasxAccion)
                        {
                            if (item.NombreEvidenciaImplementacion != null && !item.IncidenteMenor)
                            {
                                var downloadedFilePath = archivePath + item.NombreEvidenciaImplementacion;
                                var fileStream = System.IO.File.Create(downloadedFilePath);
                                sftp.DownloadFile(item.NombreEvidenciaImplementacion, fileStream);
                                fileStream.Dispose();

                                //Desencriptar
                                var pathDesencriptado = archivePath + item.NombreEvidenciaImplementacion + "_desencriptado.zip";
                                await _gestorEncriptado.DecryptFileAsync(downloadedFilePath, pathDesencriptado);

                                //Descomprimir
                                var pathDescomprimido = filePath;
                                if (!pathDescomprimido.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                                    pathDescomprimido += Path.DirectorySeparatorChar;

                                using (ZipArchive archive = ZipFile.OpenRead(pathDesencriptado))
                                {
                                    foreach (ZipArchiveEntry entry in archive.Entries)
                                    {
                                        pathDescomprimido = Path.GetFullPath(Path.Combine(pathDescomprimido, "EvLevIncidenteMayor_" + item.IdAccion + "_" + entry.FullName));
                                        if (pathDescomprimido.StartsWith(pathDescomprimido, StringComparison.Ordinal))
                                            entry.ExtractToFile(pathDescomprimido);
                                    }
                                }
                                item.RutaEvidenciaImplementacion = pathDescomprimido;
                            }
                        }

                        sftp.Dispose();
                    }
                }


                foreach (var acciones in registros)
                {
                    short rowHeight = 850;
                    IRow row = sheet.CreateRow(fila);

                    //Aplicar cell style a una fila de observación
                    for (int i = 0; i <= 15; i++)
                    {
                        row.CreateCell(i);
                        row.GetCell(i).CellStyle = estiloNormal;
                    }
                    if (incluirEvidencias)
                    {

                        ImageProperties evidencia_condicion = new ImageProperties();
                        ImageProperties evidencia_inspeccion = new ImageProperties();

                        //adaptar el tamaño de la fila al mayor determinado por la imagen
                        row.Height = rowHeight;

                        if (acciones.Evidencia_Levantamiento != "[]" && acciones.Evidencia_Levantamiento != null)
                        {

                            //evidencia_condicion = await AddImageToCellAsync(templateWorkbook, sheet, observacion.EvidenciaCondicion, fila, 4, rowHeight, imageIndex, "condicion", true);
                            evidencia_condicion = await AddImageToCellAsyncWithoutDownload(templateWorkbook, sheet, listaEvidenciasxAccion.Find(x => x.Codigo.Equals(acciones.Codigo) && x.IdAccion.Equals(acciones.IdAccion) && x.IdIncidente.Equals(acciones.IdIncidente)).RutaEvidenciaImplementacion, fila, 10, rowHeight, imageIndex, "condicion", true);
                            if (evidencia_condicion != null)
                            {
                                rowHeight = rowHeight > evidencia_condicion.rowHeight ? rowHeight : evidencia_condicion.rowHeight;
                                imageIndex++;
                            }
                        }
                        //adaptar el tamaño de la fila al mayor determinado por la imagen
                        row.Height = rowHeight;
                        //rezise images
                        if (evidencia_condicion != null)
                        {
                            if ((evidencia_condicion.extension == ".jpg" ||
                                evidencia_condicion.extension == ".png" ||
                                evidencia_condicion.extension == ".jpeg" ||
                                evidencia_condicion.extension == ".JPG" ||
                                evidencia_condicion.extension == ".PNG" ||
                                evidencia_condicion.extension == ".JPEG" &&
                                evidencia_condicion.extension != null) &&
                                evidencia_condicion.rowHeight != 0 &&
                                evidencia_condicion.rowHeight < row.Height)
                            {
                                //evidencia_condicion.image.Resize(1.0, (double)evidencia_condicion.rowHeight / row.Height);
                                evidencia_condicion.image.Resize(0.5, 1.0);
                            }
                        }
                        if (evidencia_inspeccion != null)
                        {
                            if ((evidencia_inspeccion.extension == ".jpg" ||
                                evidencia_inspeccion.extension == ".png" ||
                                evidencia_inspeccion.extension == ".jpeg" ||
                                evidencia_inspeccion.extension == ".JPG" ||
                                evidencia_inspeccion.extension == ".PNG" ||
                                evidencia_inspeccion.extension == ".JPEG") &&
                                evidencia_inspeccion.extension != null &&
                                evidencia_inspeccion.rowHeight != 0 &&
                                evidencia_inspeccion.rowHeight < row.Height)
                            {
                                //evidencia_inspeccion.image.Resize(1.0, (double)evidencia_inspeccion.rowHeight / row.Height);
                                evidencia_inspeccion.image.Resize(0.5, 1.0);
                            }
                        }
                    }
                    int startIndex = 0;
                    //->Codigo
                    row.GetCell(startIndex).SetCellValue(acciones.Codigo);
                    //->Descripción Incidente
                    row.GetCell(startIndex + 1).SetCellValue(acciones.Descripcion_Incidente);
                    //->Nivel de Incidente
                    row.GetCell(startIndex + 2).SetCellValue(acciones.Nivel_Incidente);
                    //->Fecha de Incidente
                    row.GetCell(startIndex + 3).SetCellValue(acciones.Fecha_Incidente.ToString("dd/MM/yyyy"));
                    //->Lugar del Incidente
                    row.GetCell(startIndex + 4).SetCellValue(acciones.Lugar_Incidente);
                    //->Producto Derramado
                    row.GetCell(startIndex + 5).SetCellValue(acciones.Producto_Derramado);
                    //->Acción Inmediata / Plan de Acción
                    row.GetCell(startIndex + 6).SetCellValue(acciones.Accion_Inmediata);
                    //->Plan de Acción
                    row.GetCell(startIndex + 7).SetCellValue(acciones.Plan_Accion);
                    //->Responsable del Levantamiento
                    row.GetCell(startIndex + 8).SetCellValue(acciones.Responsable_Levantamiento);
                    //-> Fecha Programada
                    row.GetCell(startIndex + 9).SetCellValue(acciones.Fecha_Programada.HasValue?acciones.Fecha_Programada.Value.ToString("dd/MM/yyyy"):"");
                    //-> Evidencia Levantamiento
                    //row.GetCell(startIndex + 10).SetCellValue(acciones.ResponsableInspeccion);
                    //-> Fecha Cierre
                    row.GetCell(startIndex + 11).SetCellValue(acciones.Fecha_Cierre.HasValue ? acciones.Fecha_Cierre.Value.ToString("dd/MM/yyyy") : "");
                    //-> Estado
                    var estadoObs = "";
                    switch (acciones.Estado)
                    {
                        case "Registrado":
                        case "Ampliado":
                            estadoObs = "Pendiente de Implementación";
                            break;
                        case "Implementado":
                            estadoObs = "Pendiente de Verificación";
                            break;
                        case "Vencido":
                            estadoObs = "Vencido";
                            break;
                        case "Aprobado":
                            estadoObs = "Aprobado";
                            break;
                        case "Rechazado":
                                estadoObs = "Pendiente de Implementación";
                            break;
                    }
                    row.GetCell(startIndex + 12).SetCellValue(estadoObs);
                    //-> Riesgo de Fatalidad
                    row.GetCell(startIndex + 13).SetCellValue(acciones.Empresa);
                    //-> Gerencia Responsable Levantamiento
                    row.GetCell(startIndex + 14).SetCellValue(acciones.Gerencia_Responsable);
                    //-> Gerencia de la inspección
                    row.GetCell(startIndex + 15).SetCellValue(acciones.Supervisor);

                    obsIndex++;
                    fila++;
                }
                // Force formulas to update with new data we added
                //sheet.ForceFormulaRecalculation = true;
                // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
                MemoryStream ms = new MemoryStream();
                templateWorkbook.Write(ms);

                // Send the memory stream to the browser
                ms.Position = 0;

                /*
                //VACIAR CARPETA TEMPORAL
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "Descarga Temporal Gestiona V2\\";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";

                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                //Directory.CreateDirectory(filePath);
                */
                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteIncidentes-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
            }
        }
        private async Task<ImageProperties> AddImageToCellAsyncWithoutDownload(XSSFWorkbook templateWorkbook, ISheet sheet, string filePath, int row, int column, short rowHeight, int imageIndex, string tipoEvidencia, bool reporte)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var output = new ImageProperties();

            var imgNCol = 0;
            if (tipoEvidencia == "condicion")
            {
                if (!reporte)
                    imgNCol = 2;
            }
            else
            {
                if (!reporte)
                    imgNCol = 4;
            }
            //5. validar extension y cargar en una celda
            var extension = Path.GetExtension(filePath);
            if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".png") || extension.ToLower().Equals(".jpeg"))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                PictureType pType;
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg"))
                    pType = PictureType.JPEG;
                else
                {
                    pType = PictureType.PNG;
                }

                templateWorkbook.AddPicture(bytes, pType);

                XSSFClientAnchor anchor = new XSSFClientAnchor();
                anchor.Col1 = column;
                anchor.Row1 = row;
                anchor.Col2 = column + imgNCol;

                IDrawing drawing = sheet.CreateDrawingPatriarch();

                IPicture picture = drawing.CreatePicture(anchor, imageIndex + 1);

                var image_dim = picture.GetImageDimension();
                var mergedCellWidth = 0.0;
                for (int i = column; i <= column + imgNCol; i++)
                {
                    mergedCellWidth += sheet.GetColumnWidth(i);
                }

                rowHeight = Convert.ToInt16(GetRowHeight(image_dim.Width, image_dim.Height, mergedCellWidth));
                picture.Resize(1.0);

                output.rowHeight = rowHeight;
                output.image = picture;
            }
            else
            {
                var pType = PictureType.PNG;
                byte[] bytes;
                switch (Path.GetExtension(filePath))
                {
                    case ".pdf": bytes = System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, "img", "icons", "pdf.png")); ; break;
                    case ".doc": bytes = System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, "img", "icons", "word.png")); break;
                    case ".docx": bytes = System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, "img", "icons", "word.png")); break;
                    case ".xls": bytes = System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, "img", "icons", "excel.png")); break;
                    case ".xlsx": bytes = System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, "img", "icons", "excel.png")); break;
                    case ".csv": bytes = System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, "img", "icons", "excel.png")); break;
                    default: bytes = System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, "img", "icons", "other-file.png")); break;
                }
                templateWorkbook.AddPicture(bytes, pType);

                XSSFClientAnchor anchor = new XSSFClientAnchor();
                anchor.Col1 = column;
                anchor.Row1 = row;
                anchor.Col2 = column + imgNCol;

                IDrawing drawing = sheet.CreateDrawingPatriarch();

                IPicture picture = drawing.CreatePicture(anchor, imageIndex + 1);
                var image_dim = picture.GetImageDimension();
                var mergedCellWidth = 0.0;
                for (int i = column; i <= column + imgNCol; i++)
                {
                    mergedCellWidth += sheet.GetColumnWidthInPixels(i);
                }
                rowHeight = 2000;
                picture.Resize(1.0);

                output.rowHeight = rowHeight;
                output.image = picture;
            }

            output.extension = extension;
            return output;
        }

        private class IncidenteRegistro
        {
            public int IdAccion { get; set; }
            public int IdIncidente { get; set; }
            public string Codigo { get; set; }
            public string Descripcion_Incidente { get; set; }
            public string Nivel_Incidente { get; set; }
            public DateTime Fecha_Incidente { get; set; }
            public string Lugar_Incidente { get; set; }
            public string Producto_Derramado { get; set; }
            public string Accion_Inmediata { get; set; }
            public string Plan_Accion { get; set; }
            public string Responsable_Levantamiento { get; set; }
            public DateTime? Fecha_Programada { get; set; }
            public DateTime? Fecha_Cierre { get; set; }
            public string Estado { get; set; }
            public string Empresa { get; set; }
            public string Gerencia_Responsable { get; set; }
            public string Supervisor { get; set; }
            public string Evidencia { get; set; }
            public string Evidencia_Levantamiento { get; set; }
        }
        private class Estilo
        {
            public string Estado { get; set; }
            public XSSFCellStyle EstiloCelda { get; set; }
            public XSSFFont Fuente { get; set; }
            public Estilo()
            {

            }
            public Estilo(string estado, XSSFWorkbook libro, byte r, byte g, byte b, short colorTexto)
            {
                Estado = estado;
                EstiloCelda = (XSSFCellStyle)libro.CreateCellStyle();
                EstiloCelda.WrapText = true;
                EstiloCelda.VerticalAlignment = VerticalAlignment.Center;
                EstiloCelda.BorderTop = BorderStyle.Thin;
                EstiloCelda.BorderBottom = BorderStyle.Thin;
                EstiloCelda.BorderLeft = BorderStyle.Thin;
                EstiloCelda.BorderRight = BorderStyle.Thin;
                EstiloCelda.Alignment = HorizontalAlignment.Center;
                EstiloCelda.SetFillForegroundColor(new NPOI.XSSF.UserModel.XSSFColor(new byte[] { r, g, b }));
                EstiloCelda.FillPattern = FillPattern.SolidForeground;

                Fuente = (XSSFFont)libro.CreateFont();
                Fuente.Color = colorTexto;

                EstiloCelda.SetFont(Fuente);
            }
        }
        private class ImageProperties
        {
            public short rowHeight { get; set; }
            public IPicture image { get; set; }
            public string extension { get; set; }
        }
        private double GetRowHeight(int image_width, int image_height, double cellWidth)
        {
            //double cellWidth = 7.16;//cm
            return ((image_height * cellWidth) / image_width) / 3;
        }
        private class EvidenciaAccion
        {
            public int IdAccion { get; set; }
            public int IdIncidente { get; set; }
            public string Codigo { get; set; }
            public bool IncidenteMenor { get; set; }
            public string NombreEvidenciaImplementacion { get; set; }
            public string RutaEvidenciaImplementacion { get; set; }
        }
    }
}
