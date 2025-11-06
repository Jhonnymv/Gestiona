using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.Accion.GetAccionesFlashReport;
using GestionAmbiental.Application.UseCases.Incidentes.FlashReport.GetFlashReport;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.Util.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static GestionAmbiental.UseCases.InspeccionesAmbientales.DescargarInspeccion.DescargaInspeccionController;

namespace GestionAmbiental.UseCases.Incidentes.DescargarFlashReport
{
    public class DescargarFlashReportController : Controller
    {
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IGestorArchivos _gestorArchivos;
        private readonly IGestorEncriptado _gestorEncriptado;
        private IWebHostEnvironment _hostingEnvironment;

        private readonly IGetFlashReport _getFlashReport;
        private readonly IGetAccionesFlashReport _getAccionesFlashReport;

        public DescargarFlashReportController(
            IGetInfo_SGA info_SGA,
            IGestorArchivos gestorArchivos,
            IGestorEncriptado gestorEncriptado,
            IWebHostEnvironment hostingEnvironment,
            IGetFlashReport getFlashReport,
            IGetAccionesFlashReport getAccionesFlashReport)
        {
            _info_SGA = info_SGA;
            _gestorArchivos = gestorArchivos;
            _gestorEncriptado = gestorEncriptado;
            _hostingEnvironment = hostingEnvironment;
            _getFlashReport = getFlashReport;
            _getAccionesFlashReport = getAccionesFlashReport;
        }

        public async Task<MemoryStream> PrepararFlashReport(int IdFlashReport, string NombreUsuario)
        {
            var flashReport = await _getFlashReport.Execute(IdFlashReport);
            var acciones = await _getAccionesFlashReport.Execute(IdFlashReport);
            acciones.AccionesFlashReport = acciones.AccionesFlashReport.Where(x => !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToList();

            string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "Incidentes", "FlashReport");
            string sFileName = @"FormatoFlashReport.xlsx";
            string sFilePath = Path.Combine(sWebRootFolder, sFileName);

            FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

            // Load the template into a NPOI workbook
            XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet("Hoja1");

            // Cell Style
            XSSFCellStyle customCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            customCellStyle.WrapText = true;
            customCellStyle.VerticalAlignment = VerticalAlignment.Center;
            customCellStyle.Alignment = HorizontalAlignment.Center;
            customCellStyle.BorderTop = BorderStyle.Medium;
            customCellStyle.BorderBottom = BorderStyle.Medium;
            customCellStyle.BorderLeft = BorderStyle.Medium;
            customCellStyle.BorderRight = BorderStyle.Medium;

            sheet.GetRow(6).GetCell(2).SetCellValue(flashReport.Gerencia_Nombre);
            sheet.GetRow(6).GetCell(4).SetCellValue(flashReport.Empresa_Nombre);

            sheet.GetRow(7).GetCell(2).SetCellValue(flashReport.Lugar);
            sheet.GetRow(7).GetCell(4).SetCellValue(flashReport.Fecha_Hora.ToString("dd/MM/yyyy HH:mm"));

            sheet.GetRow(8).GetCell(2).SetCellValue(flashReport.Nivel_Incidente_Descripcion);
            sheet.GetRow(8).GetCell(4).SetCellValue(flashReport.Componente_Ambiental_Descripcion);

            sheet.GetRow(9).GetCell(2).SetCellValue(flashReport.Producto_Incidente_Nombre);
            sheet.GetRow(9).GetCell(4).SetCellValue(flashReport.Cantidad_Derramada + " " + flashReport.Unidad_Cantidad_Derramada_Nombre);


            int filaAcciones = 15;
            //Acciones Inmediatas
            foreach (var accion in acciones.AccionesFlashReport)
            {
                IRow row = sheet.GetRow(filaAcciones);
                //Correr todas las filas y agregar una nueva
                if (row != null)
                {
                    sheet.ShiftRows(filaAcciones, sheet.LastRowNum, 1);
                    row = sheet.CreateRow(filaAcciones);
                }
                //Agregar las celdas necesarias y asignarles el estilo
                for (int currentCell = 1; currentCell < 5; currentCell++)
                {
                    row.CreateCell(currentCell);
                    row.GetCell(currentCell).CellStyle = customCellStyle;
                }
                //Completar la informacion
                row.GetCell(1).SetCellValue(accion.Descripcion);
                row.GetCell(2).SetCellValue(accion.Nombres_Usuario);
                row.GetCell(3).SetCellValue(accion.Fecha.ToString("dd/MM/yyyy"));
                row.GetCell(4).SetCellValue(accion.Estatus);
            }


            sheet.GetRow(filaAcciones + acciones.NAccionesFlashReports).GetCell(2).SetCellValue(flashReport.Supervisor_Marcobre_Nombre);
            sheet.GetRow(filaAcciones + acciones.NAccionesFlashReports).GetCell(4).SetCellValue(flashReport.Cargo_Supervisor_Marcobre);

            sheet.GetRow(filaAcciones + acciones.NAccionesFlashReports + 1).GetCell(2).SetCellValue(flashReport.Supervisor_Contratista);
            sheet.GetRow(filaAcciones + acciones.NAccionesFlashReports + 1).GetCell(4).SetCellValue(flashReport.Cargo_Supervisor_Contratista);

            sheet.GetRow(filaAcciones + acciones.NAccionesFlashReports + 2).GetCell(2).SetCellValue(flashReport.Registrado_Por_Nombre);

            //EVIDENCIAS
            int imageIndex = 1;
            int filaEvidencias = 11;

            var alturasFilasEvidencias = new List<short>();

            if (flashReport.Registro_Fotografico != null)
            {
                int[] posisicon_columnas = { 3, 4 };
                int contador_fila = 0;
                int contador_columnas = 0;

                var Evidencias = new List<string>();

                if (flashReport.Registro_Fotografico != null)
                {
                    var evidencias = JArray.Parse(flashReport.Registro_Fotografico);

                    foreach (var evidencia in evidencias)
                    {
                        Evidencias.Add((string)evidencia);
                    }
                }
                foreach (var evidencia in Evidencias)
                {
                    IRow filaEvidencia = sheet.GetRow(filaEvidencias + contador_fila);
                    if (filaEvidencia == null)
                        filaEvidencia = sheet.CreateRow(filaEvidencias + contador_fila);

                    for (int currentCell = 1; currentCell < 5; currentCell++)
                    {
                        filaEvidencia.CreateCell(currentCell);
                        filaEvidencia.GetCell(currentCell).CellStyle = customCellStyle;
                    }

                    //if (contador_columnas == 0)
                    //{
                    //    sheet.AddMergedRegion(new CellRangeAddress(filaEvidencias + contador_fila, filaEvidencias + contador_fila, 1, 9));
                    //    sheet.AddMergedRegion(new CellRangeAddress(filaEvidencias + contador_fila, filaEvidencias + contador_fila, 10, 16));
                    //    sheet.AddMergedRegion(new CellRangeAddress(filaEvidencias + contador_fila, filaEvidencias + contador_fila, 17, 25));
                    //}

                    short evidenciaRowHeight = filaEvidencia.Height;

                    ImageProperties propiedadEvidencia = new ImageProperties();
                    propiedadEvidencia = await AddImageToCellAsync(templateWorkbook, sheet, evidencia, filaEvidencias + contador_fila, posisicon_columnas[contador_columnas], posisicon_columnas[contador_columnas], filaEvidencia.Height, imageIndex, NombreUsuario);

                    evidenciaRowHeight = evidenciaRowHeight > propiedadEvidencia.rowHeight ? evidenciaRowHeight : propiedadEvidencia.rowHeight;
                    imageIndex++;

                    //adaptar el tamaño de la fila al mayor determinado por la imagen
                    //filaEvidencia.Height = evidenciaRowHeight;
                    if (contador_columnas == 0)
                        alturasFilasEvidencias.Add(evidenciaRowHeight);
                    else if (contador_columnas == 1 && alturasFilasEvidencias[contador_fila] < evidenciaRowHeight)
                        alturasFilasEvidencias[contador_fila] = evidenciaRowHeight;


                        contador_columnas++;
                    if (contador_columnas == 2)
                    {
                        contador_columnas = 0;
                        contador_fila++;
                        //alturasFilasEvidencias.Add(evidenciaRowHeight);

                        //Correr todas las filas y agregar una nueva
                        if (filaEvidencia != null)
                        {
                            sheet.ShiftRows(filaEvidencias + contador_fila - 1, sheet.LastRowNum, 1);
                        }
                    }
                }

                for (int i = 0; i < contador_fila; i++)
                {
                    IRow filaDescripción = sheet.CreateRow(11 + i);
                    for (int currentCell = 1; currentCell < 5; currentCell++)
                    {
                        filaDescripción.CreateCell(currentCell);
                        filaDescripción.GetCell(currentCell).CellStyle = customCellStyle;
                    }
                    filaDescripción.Height = alturasFilasEvidencias[i];
                }
                sheet.GetRow(11).GetCell(1).SetCellValue(flashReport.Descripcion);
                sheet.AddMergedRegion(new CellRangeAddress(11, 11 + contador_fila, 1, 2));
            }

            // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
            MemoryStream ms = new MemoryStream();
            templateWorkbook.Write(ms);

            // Send the memory stream to the browser
            ms.Position = 0;
            return ms;
            //return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReportePreliminarIncidente-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
        }
        public async Task<IActionResult> DescargarFlashReport(int IdFlashReport)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var archivoReporte = await PrepararFlashReport(IdFlashReport, userInfo.NombreUsuario);
            return File(archivoReporte, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FlashReportIncidente-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
        }

        //Añadir Imagenes al Excel
        private double GetRowHeight(int image_width, int image_height, double cellWidth)
        {
            return ((image_height * cellWidth) / image_width) / 3;
        }
        //Retorna el tamaño de la fila para la imagen agregada
        private async Task<ImageProperties> AddImageToCellAsync(XSSFWorkbook templateWorkbook, ISheet sheet, string path, int row, int startColumn, int endColumn, short rowHeight, int imageIndex, string NombreUsuario)
        {
            var output = new ImageProperties();
            var evidencia = path;

            //var imgNCol = 0;

            //1. Vaciar carpeta temporal del usuario 
            var tempPath = Path.GetTempPath();
            var SGATempPath = tempPath + "Descarga Temporal Gestiona V2\\";
            if (!Directory.Exists(SGATempPath))
            {
                Directory.CreateDirectory(SGATempPath);
            }
            var filePath = SGATempPath + "submission\\" + NombreUsuario + "\\";
            var archiveFile = SGATempPath + "zip\\" + NombreUsuario + "\\archive.zip";
            var archivePath = SGATempPath + "zip\\" + NombreUsuario + "\\";

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
            Tipo_Info_SGA directorio = Tipo_Info_SGA.FTPConnectionEvidenciaFlashReport;
            var downloadedFile = await _gestorArchivos.DownloadFTPFileAsync(evidencia.ToString(), NombreUsuario, directorio);
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
                anchor.Col1 = startColumn;
                anchor.Row1 = row;
                anchor.Col2 = endColumn;

                IDrawing drawing = sheet.CreateDrawingPatriarch();
                IPicture picture = drawing.CreatePicture(anchor, imageIndex + 1);

                var image_dim = picture.GetImageDimension();
                var mergedCellWidth = 0.0;
                for (int i = startColumn; i <= endColumn; i++)
                {
                    mergedCellWidth += sheet.GetColumnWidth(i);
                }

                rowHeight = Convert.ToInt16(GetRowHeight(image_dim.Width, image_dim.Height, mergedCellWidth));
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
