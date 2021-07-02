using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientoFauna;
using GestionAmbiental.Application.UseCases.AvistamienoFauna.GetAvistamientosFauna;
using GestionAmbiental.Application.UseCases.GestorTablasMaestras.Usuario.GetUsuario;
using GestionAmbiental.Application.UseCases.Info_SGA;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.UseCases.Reportes.AvistamientoFauna;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Renci.SshNet;

namespace GestionAmbiental.UseCases.AvistamientoFauna.DescargarAvistamientoFauna
{
    public class DescargarAvistamientoFaunaController : Controller
    {
        private readonly IGetAvistamientoFauna _getAvistamientoFauna;
        private readonly IGetAvistamientosFauna _getAvistamientosFauna;
        private readonly IGetUsuario _getUsuario;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IGetInfo_SGA _info_SGA;

        public DescargarAvistamientoFaunaController(
            IGetAvistamientoFauna getAvistamientoFauna,
            IGetAvistamientosFauna getAvistamientosFauna,
            IGetUsuario getUsuario,
            IHostingEnvironment hostingEnvironment,
            IGetInfo_SGA info_SGA)
        {
            _getAvistamientoFauna = getAvistamientoFauna;
            _getAvistamientosFauna = getAvistamientosFauna;
            _getUsuario = getUsuario;
            _hostingEnvironment = hostingEnvironment;
            _info_SGA = info_SGA;
        }

        public async Task<MemoryStream> PrepararAvistamiento(int IdAvistamiento)
        {
            var avistamiento = await _getAvistamientoFauna.Execute(IdAvistamiento);

            string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "AvistamientoFauna");
            string sFileName = @"JU-MAP-PRO-015.F01 Registro de  Avistamiento de Fauna Silvestre.xlsx";
            string sFilePath = Path.Combine(sWebRootFolder, sFileName);
            FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

            // Load the template into a NPOI workbook
            XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet("Formato Ocurrencia Fauna_v3");

            // Insert data into template
            //->Fecha
            sheet.GetRow(11).GetCell(2).SetCellValue(avistamiento.FechaAvistamiento.ToString("dd/MM/yyyy"));
            //->Tipo de Fauna
            sheet.GetRow(11).GetCell(5).SetCellValue(avistamiento.TipoFauna);
            //->Tipo de Avistamiento
            sheet.GetRow(11).GetCell(8).SetCellValue(avistamiento.TipoAvistamiento);
            //-> Hora
            sheet.GetRow(12).GetCell(2).SetCellValue(avistamiento.FechaAvistamiento.ToString("HH:mm"));
            //->Nombre Común
            sheet.GetRow(12).GetCell(5).SetCellValue(avistamiento.NombreComun);
            //->Estado de fauna
            sheet.GetRow(12).GetCell(8).SetCellValue(avistamiento.EstadoFauna);
            //->Ubicacion:   
            sheet.GetRow(13).GetCell(2).SetCellValue(avistamiento.Ubicacion);
            //->Cantidad
            sheet.GetRow(13).GetCell(5).SetCellValue(avistamiento.Cantidad.ToString());
            //->Lugar Referencial
            sheet.GetRow(13).GetCell(8).SetCellValue(avistamiento.LugarReferencia);
            //->Empresa
            sheet.GetRow(17).GetCell(2).SetCellValue(avistamiento.Empresa);
            //->Nombres y Apellidos
            sheet.GetRow(17).GetCell(6).SetCellValue(avistamiento.Usuario);
            //->Área de Trabajo
            sheet.GetRow(19).GetCell(2).SetCellValue(avistamiento.AreaTrabajo);
            //->DNI
            sheet.GetRow(19).GetCell(6).SetCellValue(avistamiento.DNI);
            if (avistamiento.Estado.Equals(EstadosAvistamiento.Completado.ToString()))
                //->Acción
                sheet.GetRow(23).GetCell(1).SetCellValue(avistamiento.DescripcionAccion);
            else
                //->Acción
                sheet.GetRow(23).GetCell(1).SetCellValue("No Aplica");

            var usuario = await _getUsuario.Execute(avistamiento.IdUsuario);
            //->Fila fotos en el formato
            int fila = 8;
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
            var columna = 1;

            foreach (var foto in avistamiento.Fotos)
            {
                short rowHeight = 850;
                IRow row = sheet.GetRow(fila);
                if (row != null && fila != 8)
                {
                    sheet.ShiftRows(fila, sheet.LastRowNum, 1);
                }
                else
                {
                    row = sheet.CreateRow(fila);
                }                

                ImageProperties imageProperties = new ImageProperties();
                if (foto != "[]" && foto != null)
                {
                    imageProperties = await AddImageToCellAsync(templateWorkbook, sheet, foto, fila, columna, rowHeight, imageIndex, "foto",usuario.UsuarioRed);
                    rowHeight = rowHeight > imageProperties.rowHeight ? rowHeight : imageProperties.rowHeight;

                    imageIndex++;
                }
                //adaptar el tamaño de la fila al mayor determinado por la imagen
                row.Height = rowHeight;
                //rezise images

                if ((imageProperties.extension.ToLower() == ".jpg" ||
                    imageProperties.extension.ToLower() == ".png" ||
                    imageProperties.extension.ToLower() == ".jpeg" &&
                    imageProperties.extension != null) &&
                    imageProperties.rowHeight != 0 &&
                    imageProperties.rowHeight < row.Height)
                {
                    imageProperties.image.Resize(1.0, (double)imageProperties.rowHeight / row.Height);
                }
                if (imageIndex % 2 == 0)
                {
                    columna = 1;
                    fila++;
                }
                else
                    columna = 5;

            }
            /*
            //Eliminar fila siguiente a las fotos
            var rowToBeDeleted = sheet.GetRow(fila+1);
            if (rowToBeDeleted != null)
            {
                sheet.RemoveRow(rowToBeDeleted);
                sheet.ShiftRows(fila + 2, sheet.LastRowNum, -1);
            }
            */

            for(var i = fila +2; i< sheet.LastRowNum; i++)
            {
                var filaCambio = sheet.GetRow(i);
                if(filaCambio!=null)
                    for (var j = 0;j<filaCambio.LastCellNum;j++)
                        if(filaCambio.GetCell(j)!=null)
                            filaCambio.GetCell(j).CellStyle.WrapText = true;
            }

            // Force formulas to update with new data we added
            sheet.ForceFormulaRecalculation = true;
            // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
            MemoryStream ms = new MemoryStream();
            templateWorkbook.Write(ms);

            // Send the memory stream to the browser
            ms.Position = 0;
            return ms;
        }
        public async Task<IActionResult> DescargarAvistamiento(int IdAvistamiento)
        {
            var archivoAvistamiento = await PrepararAvistamiento(IdAvistamiento);
            return File(archivoAvistamiento, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AvistamientoFauna-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
        }
        public async Task<IActionResult> DescargarAvistamientosDashboard(FiltroReporteAvistamientoFaunaModel model)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var avistamientos = await _getAvistamientosFauna.Execute(model.NombreComun, model.TipoAvistamiento, model.EstadoFauna, model.LugarReferencia, model.FInicio, model.FFin);

                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "AvistamientoFauna");
                string sFileName = @"Formato de Descarga de Datos.xlsx";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

                // Load the template into a NPOI workbook
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheet = templateWorkbook.GetSheet("Hoja1");

                //->Cell Style
                XSSFCellStyle customCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                customCellStyle.WrapText = true;
                customCellStyle.VerticalAlignment = VerticalAlignment.Center;
                customCellStyle.BorderTop = BorderStyle.Thin;
                customCellStyle.BorderBottom = BorderStyle.Thin;
                customCellStyle.BorderLeft = BorderStyle.Thin;
                customCellStyle.BorderRight = BorderStyle.Thin;
                customCellStyle.Alignment = HorizontalAlignment.Center;

                int fila = 1;

                for (int i = 0; i < avistamientos.Avistamientos.Count; i++)
                {
                    IRow row = sheet.CreateRow(fila);

                    row.CreateCell(0).SetCellValue(i+1);
                    row.GetCell(0).CellStyle = customCellStyle;

                    row.CreateCell(1).SetCellValue(avistamientos.Avistamientos[i].Codigo);
                    row.GetCell(1).CellStyle = customCellStyle;

                    row.CreateCell(2).SetCellValue(avistamientos.Avistamientos[i].FechaAvistamiento.ToString("dd/MM/yyyy"));
                    row.GetCell(2).CellStyle = customCellStyle;

                    row.CreateCell(3).SetCellValue(avistamientos.Avistamientos[i].FechaAvistamiento.ToString("HH:mm"));
                    row.GetCell(3).CellStyle = customCellStyle;

                    row.CreateCell(4).SetCellValue(avistamientos.Avistamientos[i].TipoFauna);
                    row.GetCell(4).CellStyle = customCellStyle;

                    row.CreateCell(5).SetCellValue(avistamientos.Avistamientos[i].NombreComun);
                    row.GetCell(5).CellStyle = customCellStyle;

                    row.CreateCell(6).SetCellValue(avistamientos.Avistamientos[i].Cantidad);
                    row.GetCell(6).CellStyle = customCellStyle;

                    row.CreateCell(7).SetCellValue(avistamientos.Avistamientos[i].TipoAvistamiento);
                    row.GetCell(7).CellStyle = customCellStyle;

                    row.CreateCell(8).SetCellValue(avistamientos.Avistamientos[i].EstadoFauna);
                    row.GetCell(8).CellStyle = customCellStyle;

                    row.CreateCell(9).SetCellValue(avistamientos.Avistamientos[i].LugarReferencia);
                    row.GetCell(9).CellStyle = customCellStyle;

                    if (avistamientos.Avistamientos[i].Ubicacion != null || avistamientos.Avistamientos[i].Ubicacion != "")
                    {
                        var ubicacion = avistamientos.Avistamientos[i].Ubicacion.Replace(" ", "");
                        row.CreateCell(10).SetCellValue(ubicacion.Split(",")[0]);
                        row.GetCell(10).CellStyle = customCellStyle;

                        row.CreateCell(11).SetCellValue(ubicacion.Split(",")[1]);
                        row.GetCell(11).CellStyle = customCellStyle;

                        row.CreateCell(12).SetCellValue("Ver en Google Maps");
                        XSSFHyperlink UrlLink = new XSSFHyperlink(HyperlinkType.Url)
                        {
                            Address = "https://www.google.com/maps/place/" + avistamientos.Avistamientos[i].Ubicacion.Replace(" ", @"%20")
                        };
                        row.GetCell(12).Hyperlink = (UrlLink);
                        row.GetCell(12).CellStyle = customCellStyle;
                    }

                    row.CreateCell(13).SetCellValue(avistamientos.Avistamientos[i].LatLugarReferencia);
                    row.GetCell(13).CellStyle = customCellStyle;

                    row.CreateCell(14).SetCellValue(avistamientos.Avistamientos[i].LongLugarReferencia);
                    row.GetCell(14).CellStyle = customCellStyle;

                    row.CreateCell(15).SetCellValue("Ver en Google Maps");
                    XSSFHyperlink UrlLinkLugReg = new XSSFHyperlink(HyperlinkType.Url)
                    {
                        Address = "https://www.google.com/maps/place/" + avistamientos.Avistamientos[i].LatLugarReferencia + "," + avistamientos.Avistamientos[i].LongLugarReferencia
                    };
                    row.GetCell(15).Hyperlink = (UrlLinkLugReg);
                    row.GetCell(15).CellStyle = customCellStyle;

                    fila++;
                }
                // Force formulas to update with new data we added
                sheet.ForceFormulaRecalculation = true;
                // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
                MemoryStream ms = new MemoryStream();
                templateWorkbook.Write(ms);

                // Send the memory stream to the browser
                ms.Position = 0;
                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteAvistamientos-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
            }
        }
        private double GetRowHeight(int image_width, int image_height, double cellWidth)
        {
            //double cellWidth = 7.16;//cm
            return ((image_height * cellWidth) / image_width) / 3;
        }
        //Retorna las propiedades de la imagen agregada
        private async Task<ImageProperties> AddImageToCellAsync(XSSFWorkbook templateWorkbook, ISheet sheet, string path, int row, int column, short rowHeight, int imageIndex, string tipoArchivo,string usuarioRed)
        {           
            var output = new ImageProperties();

            var imgNCol = 0;

            //1. Vaciar carpeta temporal del usuario 
            var tempPath = Path.GetTempPath();
            var SGATempPath = tempPath + "SGA-Marcobre-Prueba-v2\\";
            if (!Directory.Exists(SGATempPath))
            {
                Directory.CreateDirectory(SGATempPath);
            }
            var filePath = SGATempPath + "submission\\" + usuarioRed + "\\";
            var archivePath = SGATempPath + "zip\\" + usuarioRed + "\\";

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
            if (tipoArchivo == "foto")
            {
                serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionAvistamientoFauna);
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
                serverInfo = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionAvistamientoFaunaAccion);
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
            
            imgNCol = 4;

            var pathEncriptado = archivePath + "encriptado" + imageIndex.ToString() + Path.GetExtension(path);
            var fileStream = System.IO.File.Create(pathEncriptado);
            using (SftpClient sftp = new SftpClient(host, Convert.ToInt32(port), user, pass))
            {
                sftp.Connect();
                sftp.ChangeDirectory(dir);

                //Descargar archivo de sftp a nuevo archivo temporal creado
                sftp.DownloadFile(path, fileStream);

                sftp.Dispose();
            }
            fileStream.Close();

            //3. Desencriptar archivo
            var pathDesencriptado = archivePath + "desencriptado" + imageIndex.ToString() + Path.GetExtension(path);
            await DecryptFileAsync(pathEncriptado, pathDesencriptado);
            //var pathDesencriptado = archivePath + "desencriptado.zip";
            if (tipoArchivo != "foto")
            {
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
            }
            else
                filePath = pathDesencriptado;
            //5. validar extension y cargar en una celda
            var extension = Path.GetExtension(filePath);
            if (extension.Equals(".jpg") || extension.Equals(".png") || extension.Equals(".jpeg") || extension.Equals(".JPG") || extension.Equals(".PNG") || extension.Equals(".JPEG"))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                PictureType pType;
                if (extension.Equals(".jpg") || extension.Equals(".jpeg") || extension.Equals(".JPG") || extension.Equals(".JPEG"))
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
