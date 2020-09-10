using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.GetAcciones;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetCausaIncidenteReporte;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.GetInvolucrados;
using GestionAmbiental.Application.UseCases.Info_SGA;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Renci.SshNet;

namespace GestionAmbiental.UseCases.Incidentes.DescargarReportePreliminar
{
    public class DescargarReportePreliminarController : Controller
    {
        private readonly IGetInfo_SGA _info_SGA;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IGetReportePreliminar _getReportePreliminar;

        private readonly IGetInvolucradosIncidente _getInvolucradosIncidente;
        private readonly IGetAccionesIncidente _getAccionesIncidente;
        private readonly IGetCausasIncidenteReporte _getCausasIncidenteReporte;

        public DescargarReportePreliminarController(
            IGetInfo_SGA info_SGA, 
            IHostingEnvironment hostingEnvironment,
            IGetReportePreliminar getReportePreliminar, 
            IGetInvolucradosIncidente getInvolucradosIncidente, 
            IGetAccionesIncidente getAccionesIncidente, 
            IGetCausasIncidenteReporte getCausasIncidenteReporte)
        {
            _info_SGA = info_SGA;
            _hostingEnvironment = hostingEnvironment;
            _getReportePreliminar = getReportePreliminar;
            _getInvolucradosIncidente = getInvolucradosIncidente;
            _getAccionesIncidente = getAccionesIncidente;
            _getCausasIncidenteReporte = getCausasIncidenteReporte;
        }

        public async Task<MemoryStream> PrepararReportePreliminar(int IdReporte,string NombreUsuario)
        {
            var reporte = await _getReportePreliminar.Execute(IdReporte);
            var involucrados = await _getInvolucradosIncidente.Execute(IdReporte);
            var acciones = await _getAccionesIncidente.Execute(IdReporte);
            var causas = await _getCausasIncidenteReporte.Execute(IdReporte);

            string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "Incidentes", "ReportePreliminar");
            string sFileName = @"JU-MAP-STD-001.F01_Reporte preliminar de incidente ambiental_V00.xlsx";
            string sFilePath = Path.Combine(sWebRootFolder, sFileName);

            FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

            // Load the template into a NPOI workbook
            XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet("RPIA");

            // Cell Style
            XSSFCellStyle customCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            customCellStyle.WrapText = true;
            customCellStyle.VerticalAlignment = VerticalAlignment.Center;
            customCellStyle.Alignment = HorizontalAlignment.Center;
            customCellStyle.BorderTop = BorderStyle.Thin;
            customCellStyle.BorderBottom = BorderStyle.Thin;
            customCellStyle.BorderLeft = BorderStyle.Thin;
            customCellStyle.BorderRight = BorderStyle.Thin;


            sheet.GetRow(7).GetCell(22).SetCellValue(reporte.Num_Trabajadores);

            sheet.GetRow(10).GetCell(2).SetCellValue(reporte.Razon_Social_Contratista!=null? reporte.Razon_Social_Contratista : "No Aplica");
            sheet.GetRow(10).GetCell(13).SetCellValue(reporte.RUC_Contratista!=null? reporte.RUC_Contratista : "No Aplica");
            sheet.GetRow(10).GetCell(22).SetCellValue(reporte.Actividad_Contratista != null ? reporte.Actividad_Contratista : "No Aplica");

            sheet.GetRow(11).GetCell(2).SetCellValue(reporte.Domicilio_Contratista != null ? reporte.Domicilio_Contratista : "No Aplica");
            sheet.GetRow(11).GetCell(22).SetCellValue(reporte.Num_Trabajadores_Contratista != null ? reporte.Num_Trabajadores_Contratista.ToString() : "No Aplica");

            sheet.GetRow(14).GetCell(2).SetCellValue(reporte.Tipo_Incidente);
            sheet.GetRow(14).GetCell(14).SetCellValue(reporte.Nivel_Incidente);
            sheet.GetRow(14).GetCell(22).SetCellValue(reporte.Fecha_Incidente != null ? reporte.Fecha_Incidente.Value.ToString("dd/MM/yyyy") : "");
            sheet.GetRow(14).GetCell(24).SetCellValue(reporte.Fecha_Incidente != null ? reporte.Fecha_Incidente.Value.ToString("HH:mm") : "");

            sheet.GetRow(15).GetCell(2).SetCellValue(reporte.Producto_Derramado);
            sheet.GetRow(15).GetCell(14).SetCellValue(reporte.Cantidad_Derramado != null ? reporte.Cantidad_Derramado.ToString() : "");
            sheet.GetRow(15).GetCell(16).SetCellValue(reporte.Unidad_Cantidad_Derramado);
            sheet.GetRow(15).GetCell(22).SetCellValue(reporte.Area_Afectada != null ? reporte.Area_Afectada.ToString() : "");
            sheet.GetRow(15).GetCell(24).SetCellValue(reporte.Unidad_Area_Afectada);

            sheet.GetRow(16).GetCell(2).SetCellValue(reporte.Lugar_Incidente);
            sheet.GetRow(16).GetCell(14).SetCellValue(reporte.Equipo_Involucrado);
            sheet.GetRow(16).GetCell(22).SetCellValue(reporte.Componente_Ambiental);

            sheet.GetRow(17).GetCell(5).SetCellValue(reporte.Descripcion_Incidente);


            int filaCausas = 20;
            //Causas
            foreach (var causa in causas)
            {
                IRow row = sheet.GetRow(filaCausas);
                //Correr todas las filas y agregar una nueva
                if (row != null)
                {
                    sheet.ShiftRows(filaCausas, sheet.LastRowNum, 1);
                    row = sheet.CreateRow(filaCausas);
                }
                //Agregar las celdas necesarias y asignarles el estilo
                for (int currentCell = 1; currentCell < 26; currentCell++)
                {
                    row.CreateCell(currentCell);
                    row.GetCell(currentCell).CellStyle = customCellStyle;
                }
                //Completar la informacion
                row.GetCell(5).SetCellValue(causa.Tipo_Acto_Condicion);
                row.GetCell(15).SetCellValue(causa.Acto_Condicion);
                //Unir las celdas
                sheet.AddMergedRegion(new CellRangeAddress(filaCausas, filaCausas, 5, 14));
                sheet.AddMergedRegion(new CellRangeAddress(filaCausas, filaCausas, 15, 25));
            }
            //Unir la primera columna y las n filas generadas
            sheet.AddMergedRegion(new CellRangeAddress(filaCausas - 1, (filaCausas + causas.Count) - 1, 1, 4));

            sheet.GetRow(filaCausas + causas.Count).GetCell(5).SetCellValue(reporte.Otra_Causa_Incidente);

            //Involucrados
            int filaInvolucrados = filaCausas + causas.Count + 3;
            foreach (var involucrado in involucrados)
            {
                IRow row = sheet.GetRow(filaInvolucrados);
                //Correr todas las filas y agregar una nueva
                if (row != null)
                {
                    sheet.ShiftRows(filaInvolucrados, sheet.LastRowNum, 1);
                    row = sheet.CreateRow(filaInvolucrados);
                }
                //Agregar las celdas necesarias y asignarles el estilo
                for (int currentCell = 1; currentCell < 26; currentCell++)
                {
                    row.CreateCell(currentCell);
                    row.GetCell(currentCell).CellStyle = customCellStyle;
                }
                //Completar la informacion
                row.GetCell(1).SetCellValue(involucrado.Tipo_Involucrado);
                row.GetCell(5).SetCellValue(involucrado.Nombres + " " + involucrado.Apellidos);
                row.GetCell(12).SetCellValue(involucrado.Email);
                row.GetCell(15).SetCellValue(involucrado.Celular);
                row.GetCell(20).SetCellValue(involucrado.Puesto_Trabajo);
                row.GetCell(23).SetCellValue(involucrado.Turno);
                //Unir las celdas
                sheet.AddMergedRegion(new CellRangeAddress(filaInvolucrados, filaInvolucrados, 1, 4));
                sheet.AddMergedRegion(new CellRangeAddress(filaInvolucrados, filaInvolucrados, 5, 11));
                sheet.AddMergedRegion(new CellRangeAddress(filaInvolucrados, filaInvolucrados, 12, 14));
                sheet.AddMergedRegion(new CellRangeAddress(filaInvolucrados, filaInvolucrados, 15, 19));
                sheet.AddMergedRegion(new CellRangeAddress(filaInvolucrados, filaInvolucrados, 20, 22));
                sheet.AddMergedRegion(new CellRangeAddress(filaInvolucrados, filaInvolucrados, 23, 25));
            }

            //Acciones
            int filaAcciones = filaInvolucrados + involucrados.Count + 2;
            foreach (var accion in acciones)
            {
                IRow row = sheet.GetRow(filaAcciones);
                //Correr todas las filas y agregar una nueva
                if (row != null)
                {
                    sheet.ShiftRows(filaAcciones, sheet.LastRowNum, 1);
                    row = sheet.CreateRow(filaAcciones);
                }
                //Agregar las celdas necesarias y asignarles el estilo
                for (int currentCell = 1; currentCell < 26; currentCell++)
                {
                    row.CreateCell(currentCell);
                    row.GetCell(currentCell).CellStyle = customCellStyle;
                }
                //Completar la informacion
                row.GetCell(5).SetCellValue(accion.Accion_Inmediata);
                row.GetCell(15).SetCellValue(accion.Responsable_Marcobre);
                row.GetCell(20).SetCellValue(accion.Responsable_Contratista != null ? accion.Responsable_Contratista : "No Aplica");
                row.GetCell(23).SetCellValue(accion.Fecha != null ? accion.Fecha.Value.ToString("dd/MM/yyyy") : "");
                row.GetCell(24).SetCellValue(accion.Estatus);
                //Unir las celdas
                sheet.AddMergedRegion(new CellRangeAddress(filaAcciones, filaAcciones, 5, 14));
                sheet.AddMergedRegion(new CellRangeAddress(filaAcciones, filaAcciones, 15, 19));
                sheet.AddMergedRegion(new CellRangeAddress(filaAcciones, filaAcciones, 20, 22));
                sheet.AddMergedRegion(new CellRangeAddress(filaAcciones, filaAcciones, 24, 25));
            }
            //Unir la primera columna y las n filas generadas
            sheet.AddMergedRegion(new CellRangeAddress(filaAcciones - 1, (filaAcciones + acciones.Count) - 1, 1, 4));

            sheet.GetRow(filaAcciones + acciones.Count).GetCell(5).SetCellValue(reporte.Comentario_Accion);
            var filaSupervisor = filaAcciones + acciones.Count + 2;
            sheet.GetRow(filaSupervisor).GetCell(12).SetCellValue(reporte.Supervisor_MA);

            int filaEvidencias = filaSupervisor + 2;
            int imageIndex = 1;

            

            if (reporte.Evidencia != null)
            {
                int[] posisicon_columnas = {1,10,17,26};
                int contador_fila = 0;
                int contador_columnas = 0;

                foreach(var evidencia in reporte.Evidencia)
                {
                    IRow filaEvidencia = sheet.CreateRow(filaEvidencias+ contador_fila);
                    for (int currentCell = 1; currentCell < 26 ; currentCell++)
                    {
                        filaEvidencia.CreateCell(currentCell);
                        filaEvidencia.GetCell(currentCell).CellStyle = customCellStyle;
                    }
                    if(contador_columnas == 0)
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(filaEvidencias + contador_fila, filaEvidencias + contador_fila, 1, 9));
                        sheet.AddMergedRegion(new CellRangeAddress(filaEvidencias + contador_fila, filaEvidencias + contador_fila, 10, 16));
                        sheet.AddMergedRegion(new CellRangeAddress(filaEvidencias + contador_fila, filaEvidencias + contador_fila, 17, 25));
                    }
                    
                    short evidenciaRowHeight = filaEvidencia.Height;

                    ImageProperties propiedadEvidencia = new ImageProperties();
                    propiedadEvidencia = await AddImageToCellAsync(templateWorkbook, sheet, evidencia, filaEvidencias+contador_fila, posisicon_columnas[contador_columnas], posisicon_columnas[contador_columnas+1]-1, filaEvidencia.Height, imageIndex, NombreUsuario);
                    
                    evidenciaRowHeight = evidenciaRowHeight > propiedadEvidencia.rowHeight ? evidenciaRowHeight : propiedadEvidencia.rowHeight;
                    imageIndex++;

                    //adaptar el tamaño de la fila al mayor determinado por la imagen
                    filaEvidencia.Height = evidenciaRowHeight;
                    
                    contador_columnas++;
                    if (contador_columnas == 3)
                    {
                        
                        contador_columnas = 0;
                        contador_fila++;
                    }
                }
            }

            // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
            MemoryStream ms = new MemoryStream();
            templateWorkbook.Write(ms);

            // Send the memory stream to the browser
            ms.Position = 0;
            return ms;
            //return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReportePreliminarIncidente-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
        }
        public async Task<IActionResult> DescargarReportePreliminar(int IdReporte)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var archivoReporte = await PrepararReportePreliminar(IdReporte, userInfo.NombreUsuario);
            return File(archivoReporte, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReportePreliminarIncidente-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
        }


        //Añadir Imagenes al Excel
        private double GetRowHeight(int image_width, int image_height, double cellWidth)
        {
            return ((image_height * cellWidth) / image_width) / 3;
        }
        //Retorna el tamaño de la fila para la imagen agregada
        private async Task<ImageProperties> AddImageToCellAsync(XSSFWorkbook templateWorkbook, ISheet sheet, string path, int row, int startColumn,int endColumn, short rowHeight, int imageIndex,string NombreUsuario)
        {
            var output = new ImageProperties();
            var evidencia = path;

            //var imgNCol = 0;

            //1. Vaciar carpeta temporal del usuario 
            var tempPath = Path.GetTempPath();
            var SGATempPath = tempPath + "SGA-Marcobre\\";
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
            string host = "", port = "", user = "", pass = "", dir = "";
            var serverInfo = new Info_SGAOutput();

            //imgNCol = 7;
            serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaIncR);
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
            if (extension.Equals(".jpg") || extension.Equals(".png") || extension.Equals(".jpge")|| extension.Equals(".jpeg") || extension.Equals(".JPG") || extension.Equals(".PNG") || extension.Equals(".JPGE")|| extension.Equals(".JPEG"))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                PictureType pType;
                if (extension.Equals(".jpg") || extension.Equals(".jpge") ||extension.Equals(".jpeg") || extension.Equals(".JPG") || extension.Equals(".JPGE")|| extension.Equals(".JPEG"))
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