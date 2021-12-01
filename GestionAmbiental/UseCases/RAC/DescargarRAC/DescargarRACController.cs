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

namespace GestionAmbiental.UseCases.RAC.DescargarRAC
{
    public class DescargarRACController : Controller
    {
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IGetRACDescarga _getRACDescarga;
        private readonly IGetInfo_SGA _info_SGA;

        private readonly IDataProtector _protector;

        public DescargarRACController(
            IWebHostEnvironment hostingEnvironment,
            IGetRACDescarga getRACDescarga, 
            IGetInfo_SGA info_SGA,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _hostingEnvironment = hostingEnvironment;
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
                    Evidencia_Obs = await AddImageToCellAsync(templateWorkbook, sheet, rac.Evidencia_Obs, 28, 1, rowHeight, imageIndex, "Observacion");
                    rowHeight = rowHeight > Evidencia_Obs.rowHeight ? rowHeight : Evidencia_Obs.rowHeight;
                    imageIndex++;
                }
                //sheet.AddMergedRegion(new CellRangeAddress(27, 27, 8, 14));
                if (rac.Evidencia_Cierre != "" && rac.Evidencia_Cierre != null)
                {
                    Evidencia_Cierre = await AddImageToCellAsync(templateWorkbook, sheet, rac.Evidencia_Cierre, 28, 8, rowHeight, imageIndex, "Cierre");
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
        private double GetRowHeight(int image_width, int image_height, double cellWidth)
        {
            return ((image_height * cellWidth) / image_width) / 3;
        }
        //Retorna el tamaño de la fila para la imagen agregada
        private async Task<ImageProperties> AddImageToCellAsync(XSSFWorkbook templateWorkbook, ISheet sheet, string path, int row, int column, short rowHeight, int imageIndex, string tipoEvidencia)
        {
            
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var output = new ImageProperties();
            var evidencia = path;

            var imgNCol = 0;

            //1. Vaciar carpeta temporal del usuario 
            var tempPath = Path.GetTempPath();
            var SGATempPath = tempPath + "SGA-Marcobre-Prueba-v2\\";
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
            if (tipoEvidencia == "Observacion")
            {
                imgNCol = 7;
                serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaRACR);
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
                imgNCol = 7;
                serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaRACI);
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
            if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".png") || extension.ToLower().Equals(".jpeg") )
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