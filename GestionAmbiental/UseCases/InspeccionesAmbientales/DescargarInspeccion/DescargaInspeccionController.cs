using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA;
using GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccionDescarga;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Renci.SshNet;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.DescargarInspeccion
{
    public class DescargaInspeccionController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConsultarInspeccionDescarga _consultarInspeccionDescarga;
        private readonly IGetInfo_SGA _info_SGA;
        public DescargaInspeccionController(
            IHostingEnvironment hostingEnvironment,
            IConsultarInspeccionDescarga consultarInspeccionDescarga,
            IGetInfo_SGA info_SGA)
        {
            _hostingEnvironment = hostingEnvironment;
            _consultarInspeccionDescarga = consultarInspeccionDescarga;
            _info_SGA = info_SGA;
        }

        public async Task<IActionResult> DescargaInspeccion(int IdCabecera)
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
                var cabecera = await _consultarInspeccionDescarga.Execute(IdCabecera);

                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "Inspecciones");
                string sFileName = @"JU-MAP-PRO-004.F01_Inspección_V00.xlsx";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

                // Load the template into a NPOI workbook
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheet = templateWorkbook.GetSheet("Inspección");

                // Insert data into template
                //->Codigo
                //sheet.GetRow(2).GetCell(4).SetCellValue(cabecera.Codigo);
                //->Fecha Aprobacion
                //sheet.GetRow(3).GetCell(7).SetCellValue(cabecera.FechaInspec);
                //->ÁREA EJECUTORA:
                sheet.GetRow(11).GetCell(7).SetCellValue(cabecera.Gerencia);
                //->LUGAR INSPECCIONADO
                sheet.GetRow(11).GetCell(15).SetCellValue(cabecera.AreaInspeccionada);
                //->EMPRESA:   
                sheet.GetRow(11).GetCell(24).SetCellValue(cabecera.Empresa);
                //->TIPO DE INSPECCION
                sheet.GetRow(13).GetCell(7).SetCellValue(cabecera.TipoInspeccion);
                //->HORA
                sheet.GetRow(13).GetCell(15).SetCellValue(cabecera.FechaInspeccion.Hour + ":" + cabecera.FechaInspeccion.Minute);
                //->FECHA  
                sheet.GetRow(13).GetCell(18).SetCellValue(cabecera.FechaInspeccion.ToString("dd/MM/yyyy"));
                //->PARTICIPANTES DE LA INSPECCION :  
                sheet.GetRow(13).GetCell(24).SetCellValue(cabecera.Participantes);
                //->RESPONSABLE DEL ÁREA INSPECCIONADA:	
                sheet.GetRow(15).GetCell(7).SetCellValue(cabecera.ResponsableArea.Nombres + " " + cabecera.ResponsableArea.Apellidos);
                //->RESPONSABLE DE LA INSPECCION:	
                sheet.GetRow(15).GetCell(24).SetCellValue(cabecera.ResponsableInspeccion);
                
                //->OBJETIVO DE LA INSPECCIÓN: 	
                sheet.GetRow(17).GetCell(7).SetCellValue(cabecera.Objetivo);

                //->Fila observaciones en el formato
                int fila = 23;
                //->Cell Style
                XSSFCellStyle customCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                customCellStyle.WrapText = true;
                customCellStyle.VerticalAlignment = VerticalAlignment.Center;
                customCellStyle.BorderTop = BorderStyle.Thin;
                customCellStyle.BorderBottom = BorderStyle.Thin;
                customCellStyle.BorderLeft = BorderStyle.Thin;
                customCellStyle.BorderRight = BorderStyle.Thin;
                customCellStyle.Alignment = HorizontalAlignment.Center;


                //índice de la primera imagen a insertar en la colección de images del workbook
                int imageIndex = 4;
                //indice de observacion
                int obsIndex = 1;

                foreach (var observacion in cabecera.Observaciones)
                {
                    short rowHeight = 850;
                    IRow row = sheet.CreateRow(fila);

                    //Aplicar cell style a una fila de observación
                    for (int i = 0; i <= 30; i++)
                    {
                        row.CreateCell(i);
                        row.GetCell(i).CellStyle = customCellStyle;
                    }
                    //Combinar celdas
                    //Nro
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 0, 1));
                    //Acto/Condición subestándar identificada
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 3, 8));
                    //Fotografía de la condición subestándar
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 9, 13));
                    //Tipo de Condición/Acto Subestándar
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 14, 16));
                    //Acción preventiva/correctiva
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 18, 22));
                    //Levantamiento de la Observación
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(fila, fila, 25, 28));
                    //Fecha de cierre
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(fila, fila, 29, 30));
                    //Evidencia condición
                    row.GetCell(9).CellStyle = customCellStyle;
                    //Evidencia implementación
                    row.GetCell(25).CellStyle = customCellStyle;

                    ImageProperties evidencia_condicion = new ImageProperties();
                    ImageProperties evidencia_inspeccion = new ImageProperties();

                    if (observacion.EvidenciaCondicion != "[]" && observacion.EvidenciaCondicion != null)
                    {
                        evidencia_condicion = await AddImageToCellAsync(templateWorkbook, sheet, observacion.EvidenciaCondicion, fila, 9, rowHeight, imageIndex, "condicion");
                        rowHeight = rowHeight > evidencia_condicion.rowHeight ? rowHeight : evidencia_condicion.rowHeight;
                        imageIndex++;
                    }
                    if (observacion.EvidenciaImplementacion != "[]" && observacion.EvidenciaImplementacion != null)
                    {
                        evidencia_inspeccion = await AddImageToCellAsync(templateWorkbook, sheet, observacion.EvidenciaImplementacion, fila, 25, rowHeight, imageIndex, "inspeccion");
                        rowHeight = rowHeight > evidencia_inspeccion.rowHeight ? rowHeight : evidencia_inspeccion.rowHeight;
                        imageIndex++;
                    }
                    //adaptar el tamaño de la fila al mayor determinado por la imagen
                    row.Height = rowHeight;
                    //rezise images

                    if ((evidencia_condicion.extension == ".jpg" ||
                        evidencia_condicion.extension == ".png" ||
                        evidencia_condicion.extension == ".jpge" ||
                        evidencia_condicion.extension == ".JPG" ||
                        evidencia_condicion.extension == ".PNG" ||
                        evidencia_condicion.extension == ".JPGE" &&
                        evidencia_condicion.extension != null) &&
                        evidencia_condicion.rowHeight != 0 &&
                        evidencia_condicion.rowHeight < row.Height)
                    {
                        evidencia_condicion.image.Resize(1.0, (double)evidencia_condicion.rowHeight / row.Height);
                    }

                    if ((evidencia_inspeccion.extension == ".jpg" ||
                        evidencia_inspeccion.extension == ".png" ||
                        evidencia_inspeccion.extension == ".jpge" || 
                        evidencia_inspeccion.extension == ".JPG" ||
                        evidencia_inspeccion.extension == ".PNG" ||
                        evidencia_inspeccion.extension == ".JPGE") &&
                        evidencia_inspeccion.extension != null &&
                        evidencia_inspeccion.rowHeight != 0 &&
                        evidencia_inspeccion.rowHeight < row.Height)
                    {
                        evidencia_inspeccion.image.Resize(1.0, (double)evidencia_inspeccion.rowHeight / row.Height);
                    }

                    //->Index
                    row.GetCell(0).SetCellValue(obsIndex);
                    //->TIPO OBSERVACION
                    row.GetCell(2).SetCellValue(observacion.TipoObservacion);
                    //->Acto/Condición Subestándar Identificada
                    row.GetCell(3).SetCellValue(observacion.DescripcionCondicion);
                    //->Tipo de Condición / Acto Subestándar
                    row.GetCell(14).SetCellValue(observacion.TipoCondicion.Descripcion);
                    //->Nivel de Riesgo
                    row.GetCell(17).SetCellValue(observacion.NivelRiesgo);
                    //->Acción Preventiva / Correctiva Tomada
                    row.GetCell(18).SetCellValue(observacion.DescripcionAccion);
                    //->Responsable del Levanamiento del Acto / Condición Subestándar
                    row.GetCell(23).SetCellValue(observacion.ResponsableMarcobre.Nombres + " " + observacion.ResponsableMarcobre.Apellidos);
                    //->Fecha Programada
                    row.GetCell(24).SetCellValue(observacion.FechaProgramada.ToString("dd/MM/yyyy"));

                    row.GetCell(29).SetCellValue(observacion.FechaCierre.HasValue ? observacion.FechaCierre.Value.ToString("dd/MM/yyyy") : "");
                                        
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
                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteInspección-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
            }
        }        
        private double GetRowHeight(int image_width, int image_height, double cellWidth)
        {
            //double cellWidth = 7.16;//cm
            return ((image_height * cellWidth) / image_width)/3;
        }
        //Retorna el tamaño de la fila para la imagen agregada
        private async Task<ImageProperties> AddImageToCellAsync(XSSFWorkbook templateWorkbook, ISheet sheet,string path,int row, int column, short rowHeight,int imageIndex,string tipoEvidencia)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var output = new ImageProperties();
            var evidenciasCondicion = JArray.Parse(path);
            foreach (var evidencia in evidenciasCondicion)
            {
                var imgNCol = 0;

                //1. Vaciar carpeta temporal del usuario 
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre\\";
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
                string host = "", port = "", user = "", pass = "", dir = "";
                var serverInfo = new Info_SGAOutput();
                if (tipoEvidencia == "condicion")
                {
                    imgNCol = 5;
                    serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaR);
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
                }
                else
                {
                    imgNCol = 4;
                    serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaI);
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
                }

                var fileStream = System.IO.File.Create(archiveFile);
                using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
                {
                    sftp.Connect();
                    sftp.ChangeDirectory(dir);

                    //Descargar archivo de sftp a nuevo archivo temporal creado
                    sftp.DownloadFile(evidencia.ToString(), fileStream);

                    sftp.Dispose();
                }
                fileStream.Close();
                
                //3. Desencriptar archivo
                await DecryptFileAsync(archiveFile, archivePath + "desencriptado.zip");
                var pathDesencriptado = archivePath + "desencriptado.zip";
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
                if (extension.Equals(".jpg") || extension.Equals(".png") || extension.Equals(".jpge") || extension.Equals(".JPG") || extension.Equals(".PNG") || extension.Equals(".JPGE"))
                {                
                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                    PictureType pType;
                    if (extension.Equals(".jpg") || extension.Equals(".jpge") || extension.Equals(".JPG") || extension.Equals(".JPGE"))
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
            }            
           
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