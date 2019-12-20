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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.DescargarInspeccion
{
    public class DescargaInspeccionController : Controller
    {
        private IMemoryCache _cache;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConsultarInspeccionDescarga _consultarInspeccionDescarga;
        private readonly IGetInfo_SGA _info_SGA;


        private const string CarpetaEvidenciasCondicion = "ftp://127.0.0.1:2020/InspeccionesAmbientales/EvidenciasRegistro/";
        private const string CarpetaEvidenciasImplementacion = "ftp://127.0.0.1:2020/InspeccionesAmbientales/EvidenciasImplementacion/";
        public DescargaInspeccionController(
            IMemoryCache cache, 
            IHostingEnvironment hostingEnvironment,
            IConsultarInspeccionDescarga consultarInspeccionDescarga,
            IGetInfo_SGA info_SGA)
        {
            _cache = cache;
            _hostingEnvironment = hostingEnvironment;
            _consultarInspeccionDescarga = consultarInspeccionDescarga;
            _info_SGA = info_SGA;
        }

        public async Task<IActionResult> DescargaInspeccion(int IdCabecera)
        {
            if (_cache.Get<InspeccionRol>(CacheKeys.RolUsuario) == 0)
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var cabecera = await _consultarInspeccionDescarga.Execute(IdCabecera);

                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "Inspecciones");
                string sFileName = @"FormatoInspección.xls";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

                // Load the template into a NPOI workbook
                HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs, true);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheet = templateWorkbook.GetSheet("Formato");

                // Insert data into template
                //->Codigo
                sheet.GetRow(2).GetCell(4).SetCellValue(cabecera.Codigo);
                //->Fecha Aprobacion
                //sheet.GetRow(3).GetCell(7).SetCellValue(cabecera.FechaInspec);
                //->ÁREA EJECUTORA:
                sheet.GetRow(5).GetCell(3).SetCellValue(cabecera.Gerencia);
                //->EMPRESA:   
                sheet.GetRow(5).GetCell(8).SetCellValue(cabecera.Empresa);
                //->LUGAR INSPECCIONADO
                sheet.GetRow(7).GetCell(3).SetCellValue(cabecera.AreaInspeccionada);
                //->TIPO DE INSPECCION
                sheet.GetRow(7).GetCell(7).SetCellValue(cabecera.TipoInspeccion);
                //->HORA
                sheet.GetRow(7).GetCell(9).SetCellValue(cabecera.FechaInspeccion.Hour + ":" + cabecera.FechaInspeccion.Minute);
                //->FECHA  
                sheet.GetRow(7).GetCell(11).SetCellValue(cabecera.FechaInspeccion.Date.ToShortDateString());
                //->RESPONSABLE DEL ÁREA INSPECCIONADA:	
                sheet.GetRow(9).GetCell(3).SetCellValue(cabecera.ResponsableArea.Nombres + " " + cabecera.ResponsableArea.Apellidos);
                //->RESPONSABLE DE LA INSPECCION:	
                sheet.GetRow(9).GetCell(8).SetCellValue(cabecera.ResponsableInspeccion);
                //->PARTICIPANTES DE LA INSPECCION :  
                sheet.GetRow(11).GetCell(3).SetCellValue(cabecera.Participantes);
                //->OBJETIVO DE LA INSPECCIÓN: 	
                sheet.GetRow(13).GetCell(3).SetCellValue(cabecera.Objetivo);

                //->Fila observaciones en el formato
                int fila = 16;
                //->Cell Style
                HSSFCellStyle customCellStyle = (HSSFCellStyle)templateWorkbook.CreateCellStyle();
                customCellStyle.WrapText = true;
                customCellStyle.VerticalAlignment = VerticalAlignment.Center;
                customCellStyle.BorderTop = BorderStyle.Thin;
                customCellStyle.BorderBottom = BorderStyle.Thin;
                customCellStyle.BorderLeft = BorderStyle.Thin;
                customCellStyle.BorderRight = BorderStyle.Thin;
                customCellStyle.Alignment = HorizontalAlignment.Center;


                //índice de la primera imagen a insertar en la colección de images del workbook
                int imageIndex = 3;
                //indice de observacion
                int obsIndex = 1;

                foreach (var observacion in cabecera.Observaciones)
                {
                    short rowHeight = 850;
                    IRow row = sheet.CreateRow(fila);
                    //Evidencia condición
                    row.CreateCell(4);
                    row.GetCell(4).CellStyle = customCellStyle;
                    //Evidencia implementación
                    row.CreateCell(10);
                    row.GetCell(10).CellStyle = customCellStyle;

                    ImageProperties evidencia_condicion = new ImageProperties();
                    ImageProperties evidencia_inspeccion = new ImageProperties();

                    if (observacion.EvidenciaCondicion != "[]" && observacion.EvidenciaCondicion != null)
                    {
                        evidencia_condicion = await AddImageToCellAsync(templateWorkbook, sheet, observacion.EvidenciaCondicion, fila, 4, rowHeight, imageIndex, "condicion");
                        rowHeight = rowHeight > evidencia_condicion.rowHeight ? rowHeight : evidencia_condicion.rowHeight;
                        imageIndex++;
                    }
                    if (observacion.EvidenciaImplementacion != "[]" && observacion.EvidenciaImplementacion != null)
                    {
                        evidencia_inspeccion = await AddImageToCellAsync(templateWorkbook, sheet, observacion.EvidenciaImplementacion, fila, 10, rowHeight, imageIndex, "inspeccion");
                        rowHeight = rowHeight > evidencia_inspeccion.rowHeight ? rowHeight : evidencia_inspeccion.rowHeight;
                        imageIndex++;
                    }
                    //adaptar el tamaño de la fila al mayor determinado por la imagen
                    row.Height = rowHeight;
                    //rezise images
                    if (evidencia_condicion.extension != ".jpg" &&
                        evidencia_condicion.extension != ".png" &&
                        evidencia_condicion.extension != ".jpge" &&
                        evidencia_condicion.extension != ".JPG" &&
                        evidencia_condicion.extension != ".PNG" &&
                        evidencia_condicion.extension != ".JPGE" &&
                        evidencia_condicion.extension != null &&
                        evidencia_condicion.rowHeight != 0 &&
                        evidencia_condicion.rowHeight < row.Height
                        )
                        evidencia_condicion.image.Resize(0.6, 0.2);

                    if (evidencia_inspeccion.extension != ".jpg" &&
                        evidencia_inspeccion.extension != ".png" &&
                        evidencia_inspeccion.extension != ".jpge" &&
                        evidencia_inspeccion.extension != ".JPG" &&
                        evidencia_inspeccion.extension != ".PNG" &&
                        evidencia_inspeccion.extension != ".JPGE" &&
                        evidencia_inspeccion.extension != null &&
                        evidencia_inspeccion.rowHeight != 0 &&
                        evidencia_inspeccion.rowHeight < row.Height)
                        evidencia_inspeccion.image.Resize(0.6, 0.2);

                    //->Index
                    row.CreateCell(1).SetCellValue(obsIndex);
                    row.GetCell(1).CellStyle = customCellStyle;
                    //->TIPO OBSERVACION
                    row.CreateCell(2).SetCellValue(observacion.TipoObservacion);
                    row.GetCell(2).CellStyle = customCellStyle;
                    //->Acto/Condición Subestándar Identificada
                    row.CreateCell(3).SetCellValue(observacion.DescripcionCondicion);
                    row.GetCell(3).CellStyle = customCellStyle;
                    //->Tipo de Condición / Acto Subestándar
                    row.CreateCell(5).SetCellValue(observacion.TipoCondicion.Descripcion);
                    row.GetCell(5).CellStyle = customCellStyle;
                    //->Nivel de Riesgo del Aspecto Ambiental
                    row.CreateCell(6).SetCellValue(observacion.NivelRiesgo);
                    row.GetCell(6).CellStyle = customCellStyle;
                    //->Acción Preventiva / Correctiva Tomada
                    row.CreateCell(7).SetCellValue(observacion.DescripcionAccion);
                    row.GetCell(7).CellStyle = customCellStyle;
                    //->Responsable del Levanamiento del Acto / Condición Subestándar
                    row.CreateCell(8).SetCellValue(observacion.ResponsableMarcobre.Nombres + " " + observacion.ResponsableMarcobre.Apellidos);
                    row.GetCell(8).CellStyle = customCellStyle;
                    //->Fecha Programada
                    row.CreateCell(9).SetCellValue(observacion.FechaProgramada.ToShortDateString());
                    row.GetCell(9).CellStyle = customCellStyle;

                    row.CreateCell(11).SetCellValue(observacion.FechaCierre.HasValue ? observacion.FechaCierre.Value.ToString("dd/MM/yyyy HH:mm") : "");
                    row.GetCell(11).CellStyle = customCellStyle;


                    obsIndex++;
                    fila++;
                }
                // Force formulas to update with new data we added
                sheet.ForceFormulaRecalculation = true;
                // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
                MemoryStream ms = new MemoryStream();
                templateWorkbook.Write(ms);

                // Send the memory stream to the browser
                //ExportDataTableToExcel(ms, "EventExpenseReport.xls");
                ms.Position = 0;
                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteInspección-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls");
            }
        }        
        private double GetRowHeight(int image_width, int image_height)
        {
            double cellWidth = 7.16;//cm
            return (image_height * cellWidth) / image_width;
        }
        //Retorna el tamaño de la fila para la imagen agregada
        private async Task<ImageProperties> AddImageToCellAsync(HSSFWorkbook templateWorkbook, ISheet sheet,string path,int row, int column, short rowHeight,int imageIndex,string tipoEvidencia)
        {
            var output = new ImageProperties();
            var evidenciasCondicion = JArray.Parse(path);
            FtpWebRequest ftpRequest;
            foreach (var evidencia in evidenciasCondicion)
            {
                //1. Vaciar carpeta temporal del usuario 
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "SGA-Marcobre\\";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission\\" + User.Identity.Name.Split("\\")[1] + "\\";
                var archiveFile = SGATempPath + "zip\\" + User.Identity.Name.Split("\\")[1] + "\\archive.zip";
                var archivePath = SGATempPath + "zip\\" + User.Identity.Name.Split("\\")[1] + "\\";

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
                string route = "", user = "", pass = "";
                var serverInfo = new Info_SGAOutput();
                if (tipoEvidencia == "condicion")
                {
                    serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaR);
                    foreach (var item in serverInfo.Info)
                    {
                        switch (item.Key)
                        {
                            case "route": route = item.Value; break;
                            case "user": user = item.Value; break;
                            case "password": pass = item.Value; break;
                        }
                    }
                }
                else
                {
                    serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaI);
                    foreach (var item in serverInfo.Info)
                    {
                        switch (item.Key)
                        {
                            case "route": route = item.Value; break;
                            case "user": user = item.Value; break;
                            case "password": pass = item.Value; break;
                        }
                    }
                }
                

                ftpRequest = (FtpWebRequest)WebRequest.Create(route + evidencia);
                ftpRequest.Credentials = new NetworkCredential(user,pass);
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                var ftpStream = ftpResponse.GetResponseStream();
                var fileStream = System.IO.File.Create(archiveFile);
                ftpStream.CopyTo(fileStream);
                ftpStream.Close();
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

                    IDrawing drawing = sheet.CreateDrawingPatriarch();
                    IClientAnchor anchor = templateWorkbook.GetCreationHelper().CreateClientAnchor();
                    anchor.Dx1 = 1;
                    anchor.Dy1 = 1;
                    anchor.Col1 = column;
                    anchor.Row1 = row;
                    IPicture picture = drawing.CreatePicture(anchor, imageIndex + 1);
                    var image_dim = picture.GetImageDimension();
                    rowHeight = (short)(GetRowHeight(image_dim.Width, image_dim.Height) * 612);
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

                    IDrawing drawing = sheet.CreateDrawingPatriarch();
                    IClientAnchor anchor = templateWorkbook.GetCreationHelper().CreateClientAnchor();
                    anchor.Dx1 = 1;
                    anchor.Dy1 = 1;
                    anchor.Col1 = column;
                    anchor.Row1 = row;
                    IPicture picture = drawing.CreatePicture(anchor, imageIndex + 1);
                    var image_dim = picture.GetImageDimension();
                    rowHeight = (short)(GetRowHeight(image_dim.Width, image_dim.Height) * 612);
                    if(rowHeight > 2360)
                        picture.Resize(1.0,0.2);
                    else
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