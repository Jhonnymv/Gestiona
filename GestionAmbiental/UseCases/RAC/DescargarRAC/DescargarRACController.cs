using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.RAC.DescargarRAC;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Net;
using GestionAmbiental.Application.UseCases.Info_SGA;
using System.IO.Compression;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Renci.SshNet;
using GestionAmbiental.Application.UseCases.RAC.Verificacion.ConsultarRACs;
using System.Data.SqlTypes;
using GestionAmbiental.Application.UseCases.Reportes.RAC.GetRACsDescarga;
using GestionAmbiental.UseCases.Util.Interfaces;

namespace GestionAmbiental.UseCases.RAC.DescargarRAC
{
    public class DescargarRACController : Controller
    {
        private readonly IGestorArchivos _gestorArchivos;
        private readonly IGestorEncriptado _gestorEncriptado;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IGetRACDescarga _getRACDescarga;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IGetRACsDescarga _getRACsDescarga;
        private readonly IDataProtector _protector;

        public DescargarRACController(
            IGestorArchivos gestorArchivos,
            IGestorEncriptado gestorEncriptado,
            IWebHostEnvironment hostingEnvironment,
            IGetRACDescarga getRACDescarga,
            IGetRACsDescarga getRACsDescarga,
            IGetInfo_SGA info_SGA,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _gestorArchivos = gestorArchivos;
            _gestorEncriptado = gestorEncriptado;
            _hostingEnvironment = hostingEnvironment;
            _getRACsDescarga = getRACsDescarga;
            _getRACDescarga = getRACDescarga;
            _info_SGA = info_SGA;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.RACIdRouteValue);
        }

        public async Task<IActionResult> DescargaRAC(string IdRAC)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdRAC=" + IdRAC });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                var IdDesencriptado = Int32.Parse(_protector.Unprotect(IdRAC));
                var rac = await _getRACDescarga.Execute(IdDesencriptado);

                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "RAC");
                string sFileName = @"JU-MAP-PRO-004.F02.xlsx";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

                // Load the template into a NPOI workbook
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheet = templateWorkbook.GetSheet("RAC");

                // Insert data into template
                //->Nombres y apellidos
                sheet.GetRow(7).GetCell(2).SetCellValue(rac.Nombres_Apellidos);
                //->Area de Marcobre
                sheet.GetRow(7).GetCell(9).SetCellValue(rac.Gerencia);
                //->Empresa
                sheet.GetRow(9).GetCell(2).SetCellValue(rac.Empresa);
                //->Otra Empresa
                sheet.GetRow(9).GetCell(9).SetCellValue(rac.Otra_Empresa);
                //->Empresa Observada
                sheet.GetRow(13).GetCell(2).SetCellValue(rac.Empresa_Ob);
                //->Gerencia a la que pertenece (Marcobre)
                sheet.GetRow(13).GetCell(9).SetCellValue(rac.Gerencia_Obs);
                //->Ubicación
                sheet.GetRow(15).GetCell(2).SetCellValue(rac.Ubicacion);
                //->Fecha
                sheet.GetRow(15).GetCell(9).SetCellValue(rac.Fecha.ToString("dd/MM/yyyy"));
                //->Hora
                sheet.GetRow(15).GetCell(14).SetCellValue(rac.Fecha.ToString("HH:mm"));
                //->Observación SSO/MA
                sheet.GetRow(17).GetCell(2).SetCellValue(rac.SSO_MA);
                //->Clasificación
                sheet.GetRow(17).GetCell(7).SetCellValue(rac.Clasificacion);
                //->Nivel de Riesgo
                sheet.GetRow(17).GetCell(13).SetCellValue(rac.NivelRiesgo);
                //->Tipo
                sheet.GetRow(19).GetCell(2).SetCellValue(rac.TipoCondicion);
                //->Descripción del acto o condición subestándar
                sheet.GetRow(21).GetCell(2).SetCellValue(rac.Descripcion_Acto_Cond);
                //->Medidas de Control inmediata
                sheet.GetRow(23).GetCell(2).SetCellValue(rac.Medida_Control);
                //->¿Se planteó una primera acción correctiva?
                sheet.GetRow(25).GetCell(2).SetCellValue(rac.PAC);
                //->Responsable de Marcobre del cierre de la observación
                sheet.GetRow(25).GetCell(6).SetCellValue(rac.Responsable_Cierre);
                //->Fecha de cierre programada
                sheet.GetRow(25).GetCell(9).SetCellValue(rac.Fecha_Cierre.ToString("dd/MM/yyyy"));
                //->Supervisor de SSO/MA (Marcobre) a notificar
                sheet.GetRow(25).GetCell(14).SetCellValue(rac.Supervisor);

                ImageProperties Evidencia_Cierre = new ImageProperties();
                ImageProperties Evidencia_Obs = new ImageProperties();

                //índice de la primera imagen a insertar en la colección de images del workbook
                int imageIndex = 1;
                short rowHeight = 850;
                IRow row = sheet.GetRow(28);
                //Combinar celdas
                //sheet.AddMergedRegion(new CellRangeAddress(27, 27, 1, 7));

                if (rac.Evidencia_Obs != "" && rac.Evidencia_Obs != null)
                {
                    Evidencia_Obs = await AddImageToCellAsync(templateWorkbook, sheet, rac.Evidencia_Obs, 28, 1, rowHeight, imageIndex, "Observacion", false);
                    rowHeight = rowHeight > Evidencia_Obs.rowHeight ? rowHeight : Evidencia_Obs.rowHeight;
                    imageIndex++;
                }
                //sheet.AddMergedRegion(new CellRangeAddress(27, 27, 8, 14));
                if (rac.Evidencia_Cierre != "" && rac.Evidencia_Cierre != null)
                {
                    Evidencia_Cierre = await AddImageToCellAsync(templateWorkbook, sheet, rac.Evidencia_Cierre, 28, 8, rowHeight, imageIndex, "Cierre", false);
                    rowHeight = rowHeight > Evidencia_Cierre.rowHeight ? rowHeight : Evidencia_Cierre.rowHeight;
                    imageIndex++;
                }

                //adaptar el tamaño de la fila al mayor determinado por la imagen
                row.Height = rowHeight;
                //rezise images

                if (Evidencia_Obs.extension != null &&
                    (Evidencia_Obs.extension.ToLower() == ".jpg" ||
                    Evidencia_Obs.extension.ToLower() == ".png" ||
                    Evidencia_Obs.extension.ToLower() == ".jpeg") &&
                    Evidencia_Obs.rowHeight != 0 &&
                    Evidencia_Obs.rowHeight < row.Height)
                {
                    Evidencia_Obs.image.Resize(1.0, (double)Evidencia_Obs.rowHeight / row.Height);
                }

                if (Evidencia_Cierre.extension != null &&
                    (Evidencia_Cierre.extension.ToLower() == ".jpg" ||
                    Evidencia_Cierre.extension.ToLower() == ".png" ||
                    Evidencia_Cierre.extension.ToLower() == ".jpeg") &&
                    Evidencia_Cierre.rowHeight != 0 &&
                    Evidencia_Cierre.rowHeight < row.Height)
                {
                    Evidencia_Cierre.image.Resize(1.0, (double)Evidencia_Cierre.rowHeight / row.Height);
                }

                // Force formulas to update with new data we added
                sheet.ForceFormulaRecalculation = true;
                // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
                MemoryStream ms = new MemoryStream();
                templateWorkbook.Write(ms);

                // Send the memory stream to the browser
                ms.Position = 0;
                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteRAC-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
            }
        }
        public async Task<IActionResult> DescargarFechas(int IdGerencia, int IdEmpresa, int IdUsuario, string FechaInicio, string FechaFin)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdEmpresa=" + IdEmpresa + "&IdGerencia=" + IdGerencia + "&IdUsuario=" + IdUsuario + "&FechaInicio=" + FechaInicio + "&FechaFin=" + FechaFin });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                DateTime fechaInicio;
                DateTime fechaFin;
                if (FechaInicio != null)
                    fechaInicio = Convert.ToDateTime(FechaInicio);
                else
                    fechaInicio = (DateTime)SqlDateTime.MinValue;
                if (FechaFin != null)
                    fechaFin = Convert.ToDateTime(FechaFin);
                else
                    fechaFin = DateTime.Now;

                var racs = await _getRACsDescarga.Execute(IdGerencia, IdEmpresa, IdUsuario, FechaInicio, FechaFin);
                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "RAC");
                string sFileName = @"DescargaRACs.xlsx";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);
                // Load the template into a NPOI workbook
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheet = templateWorkbook.GetSheet("Observaciones");
                //->Fila observaciones en el formato
                int fila = 2;
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

                racs = racs.OrderBy(x => x.Fecha).ToList();

                //índice de la primera imagen a insertar en la colección de images del workbook
                int imageIndex = -1;
                //indice de observacion
                int obsIndex = 1;

                foreach (var rac in racs)
                {
                    var estadoObs = "";
                    switch (rac.Estado)
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
                            //if (rac.DescripcionCondicion.Equals("Buenas Prácticas"))
                            //    estadoObs = "Rechazado";
                            //else
                            estadoObs = "Pendiente de Implementación";
                            break;
                    }


                    short rowHeight = 850;
                    IRow row = sheet.CreateRow(fila);

                    //Aplicar cell style a una fila de observación
                    for (int i = 1; i <= 20; i++)
                    {
                        row.CreateCell(i);
                        row.GetCell(i).CellStyle = estiloNormal;
                    }

                    ImageProperties evidencia_condicion = new ImageProperties();
                    ImageProperties evidencia_inspeccion = new ImageProperties();

                    //adaptar el tamaño de la fila al mayor determinado por la imagen
                    row.Height = rowHeight;

                    if (rac.Evidencia_Obs != "[]" && rac.Evidencia_Obs != null)
                    {
                        evidencia_condicion = await AddImageToCellAsync(templateWorkbook, sheet, rac.Evidencia_Obs, fila, 6, rowHeight, imageIndex, "Observacion", true);
                        rowHeight = rowHeight > evidencia_condicion.rowHeight ? rowHeight : evidencia_condicion.rowHeight;
                        imageIndex++;
                    }
                    if (rac.Evidencia_Cierre != "[]" && rac.Evidencia_Cierre != null)
                    {
                        evidencia_inspeccion = await AddImageToCellAsync(templateWorkbook, sheet, rac.Evidencia_Cierre, fila, 13, rowHeight, imageIndex, "Cierre", true);
                        rowHeight = rowHeight > evidencia_inspeccion.rowHeight ? rowHeight : evidencia_inspeccion.rowHeight;
                        imageIndex++;
                    }
                    //adaptar el tamaño de la fila al mayor determinado por la imagen
                    row.Height = rowHeight;
                    //rezise images

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
                    int startIndex = 1;
                    //->Index
                    row.GetCell(startIndex).SetCellValue(obsIndex);
                    //->SSO/MA
                    row.GetCell(startIndex + 1).SetCellValue(rac.SSO_MA);
                    //->Empresa Observada
                    row.GetCell(startIndex + 2).SetCellValue(rac.Empresa_Ob.RazonSocial);
                    //->Gerencia Observada
                    row.GetCell(startIndex + 3).SetCellValue(rac.Gerencia_Obs.Descripcion);
                    //->Clasificación
                    row.GetCell(startIndex + 4).SetCellValue(rac.Clasificacion);
                    //->Descripción de Acto o Condición Subestándar
                    row.GetCell(startIndex + 6).SetCellValue(rac.Descripcion_Acto_Cond);
                    //->Tipo de Condición / Acto Subestándar
                    row.GetCell(startIndex + 7).SetCellValue(rac.TipoCondicion.Descripcion);
                    //->Nivel de Riesgo
                    row.GetCell(startIndex + 8).SetCellValue(rac.NivelRiesgo);
                    //->Medidas de Control Inmediata
                    row.GetCell(startIndex + 9).SetCellValue(rac.Medida_Control);
                    //->Responsable Levantamiento
                    row.GetCell(startIndex + 10).SetCellValue(rac.Responsable_Cierre.Nombres + " " + rac.Responsable_Cierre.Apellidos);
                    //->Fecha Programada
                    row.GetCell(startIndex + 11).SetCellValue(rac.Fecha.ToString("dd/MM/yyyy"));
                    //->Fecha de Cierre
                    row.GetCell(startIndex + 13).SetCellValue(rac.Fecha_Cierre.ToString("dd/MM/yyyy"));
                    //->Supervisor de SSO/MA
                    row.GetCell(startIndex + 14).SetCellValue(rac.Supervisor.Nombres + " " + rac.Supervisor.Apellidos);
                    //-> Estado
                    row.GetCell(startIndex + 15).SetCellValue(estadoObs);
                    var estilo = estilos.FirstOrDefault(x => x.Estado.Equals(estadoObs));
                    if (estilo != null)
                        row.GetCell(startIndex + 15).CellStyle = estilo.EstiloCelda;
                    //-> Reportante
                    row.GetCell(startIndex + 16).SetCellValue(rac.Nombres_Apellidos);
                    //-> Área Marcobre de Reportante
                    row.GetCell(startIndex + 17).SetCellValue(rac.Gerencia.Descripcion);
                    //-> Empresa Reportante
                    row.GetCell(startIndex + 18).SetCellValue(rac.Empresa.RazonSocial);
                    //-> N° RAC GESTIONA
                    row.GetCell(startIndex + 19).SetCellValue(rac.Codigo);
                    XSSFHyperlink UrlLinkLugReg = new XSSFHyperlink(HyperlinkType.Url)
                    {
                        Address = "https://gestiona.marcobre.com:9190/RegistroRAC/DetalleRACNotificacion?IdRAC=" + rac.Id_RAC
                    };
                    row.GetCell(startIndex + 19).Hyperlink = (UrlLinkLugReg);

                    obsIndex++;
                    fila++;
                }
                // Force formulas to update with new data we added
                sheet.ForceFormulaRecalculation = true;
                // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
                MemoryStream ms = new MemoryStream();
                templateWorkbook.Write(ms);

                // Send the memory stream to the browser
                ms.Position = 0;
                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteRAC-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
            }
        }

        private double GetRowHeight(int image_width, int image_height, double cellWidth)
        {
            return ((image_height * cellWidth) / image_width) / 3;
        }
        //Retorna el tamaño de la fila para la imagen agregada
        private async Task<ImageProperties> AddImageToCellAsync(XSSFWorkbook templateWorkbook, ISheet sheet, string path, int row, int column, short rowHeight, int imageIndex, string tipoEvidencia, bool reporte)
        {

            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var output = new ImageProperties();
            var evidencia = path;

            var imgNCol = 0;

            //1. Vaciar carpeta temporal del usuario 
            var tempPath = Path.GetTempPath();
            var SGATempPath = tempPath + "Descarga Temporal Gestiona-Pruebas\\";
            if (!Directory.Exists(SGATempPath))
            {
                Directory.CreateDirectory(SGATempPath);
            }
            var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
            var archiveFile = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\archive.zip";
            var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";

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

            //2. Descargar .zip de ftp y guardar en carpeta temporal zip
            Tipo_Info_SGA directorio;
            string host = "", port = "", user = "", pass = "", dir = "";
            var serverInfo = new Info_SGAOutput();
            if (tipoEvidencia == "Observacion")
            {
                if (!reporte)
                    imgNCol = 7;
                directorio = Tipo_Info_SGA.FTPConnectionEvidenciaRACR;
            }
            else
            {
                if (!reporte)
                    imgNCol = 7;
                directorio = Tipo_Info_SGA.FTPConnectionEvidenciaRACI;
            }

            var downloadedFile = await _gestorArchivos.DownloadFTPFileAsync(evidencia.ToString(), userInfo.NombreUsuario, directorio);
            //3. Desencriptar archivo
            var pathDesencriptado = archivePath + "desencriptado.zip";
            await _gestorEncriptado.DecryptFileAsync(downloadedFile, pathDesencriptado);
            //4. descomprimir en carpeta temporal submission
            if (!filePath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                filePath += Path.DirectorySeparatorChar;

            using (ZipArchive archive = ZipFile.OpenRead(pathDesencriptado))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    filePath = Path.GetFullPath(Path.Combine(filePath, entry.FullName));
                    if (filePath.StartsWith(filePath, StringComparison.Ordinal))
                        entry.ExtractToFile(filePath);
                }
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
        private class ImageProperties
        {
            public short rowHeight { get; set; }
            public IPicture image { get; set; }
            public string extension { get; set; }
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
    }
}