using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccionDescarga;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.DescargarInspeccion
{
    public class DescargaInspeccionController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConsultarInspeccionDescarga _consultarInspeccionDescarga;

        public DescargaInspeccionController(IHostingEnvironment hostingEnvironment,
            IConsultarInspeccionDescarga consultarInspeccionDescarga)
        {
            _hostingEnvironment = hostingEnvironment;
            _consultarInspeccionDescarga = consultarInspeccionDescarga;
        }

        public async Task<IActionResult> DescargaInspeccion(int IdCabecera)
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
            sheet.GetRow(5).GetCell(3).SetCellValue(cabecera.GerenEjecMar);
            //->EMPRESA:   
            sheet.GetRow(5).GetCell(8).SetCellValue(cabecera.EmpContratMarc);
            //->LUGAR INSPECCIONADO
            sheet.GetRow(7).GetCell(3).SetCellValue(cabecera.LugarInspec);
            //->TIPO DE INSPECCION
            sheet.GetRow(7).GetCell(7).SetCellValue(cabecera.Tipo_Inspeccion);
            //->HORA
            sheet.GetRow(7).GetCell(9).SetCellValue(cabecera.FechaInspec.Hour + ":" + cabecera.FechaInspec.Minute);
            //->FECHA  
            sheet.GetRow(7).GetCell(11).SetCellValue(cabecera.FechaInspec.Date.ToShortDateString());
            //->RESPONSABLE DEL ÁREA INSPECCIONADA:	
            sheet.GetRow(9).GetCell(3).SetCellValue(cabecera.Responsable_Lugar.Nombres + " " + cabecera.Responsable_Lugar.Apellidos);
            //->RESPONSABLE DE LA INSPECCION:	
            sheet.GetRow(9).GetCell(8).SetCellValue(cabecera.Responsable_Inspeccion);
            //->PARTICIPANTES DE LA INSPECCION :  
            sheet.GetRow(11).GetCell(3).SetCellValue(cabecera.Inspectores);
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
            int imageIndex = 2;
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

                if (observacion.Evidencia_Condicion != "[]" && observacion.Evidencia_Condicion != null)
                {
                    evidencia_condicion = AddImageToCell(templateWorkbook, sheet, observacion.Evidencia_Condicion, fila, 4, rowHeight, imageIndex);
                    rowHeight = rowHeight > evidencia_condicion.rowHeight ? rowHeight : evidencia_condicion.rowHeight;
                    imageIndex++;
                }
                if (observacion.Evidencia_Inspeccion != "[]" && observacion.Evidencia_Inspeccion != null)
                {
                    evidencia_inspeccion = AddImageToCell(templateWorkbook, sheet, observacion.Evidencia_Inspeccion, fila, 10, rowHeight, imageIndex);
                    rowHeight = rowHeight > evidencia_inspeccion.rowHeight ? rowHeight : evidencia_inspeccion.rowHeight;
                    imageIndex++;
                }
                //adaptar el tamaño de la fila al mayor determinado por la imagen
                row.Height = rowHeight;
                //rezise images
                if( evidencia_condicion.extension != ".jpg" &&
                    evidencia_condicion.extension != ".png" &&
                    evidencia_condicion.extension != ".jpge"&&
                    evidencia_condicion.extension != ".JPG" &&
                    evidencia_condicion.extension != ".PNG" &&
                    evidencia_condicion.extension != ".JPGE"&&
                    evidencia_condicion.extension != null &&
                    evidencia_condicion.rowHeight !=0 &&
                    evidencia_condicion.rowHeight < row.Height
                    )
                    evidencia_condicion.image.Resize(0.6, 0.2) ;

                if (evidencia_inspeccion.extension != ".jpg" &&
                    evidencia_inspeccion.extension != ".png" &&
                    evidencia_inspeccion.extension != ".jpge"&&
                    evidencia_inspeccion.extension != ".JPG" &&
                    evidencia_inspeccion.extension != ".PNG" &&
                    evidencia_inspeccion.extension != ".JPGE"&&
                    evidencia_inspeccion.extension != null &&
                    evidencia_inspeccion.rowHeight != 0 &&
                    evidencia_inspeccion.rowHeight < row.Height)
                    evidencia_inspeccion.image.Resize(0.6, 0.2);

                //->Index
                row.CreateCell(1).SetCellValue(obsIndex);
                row.GetCell(1).CellStyle = customCellStyle;
                //->TIPO OBSERVACION
                row.CreateCell(2).SetCellValue(observacion.Tipo_Observacion);
                row.GetCell(2).CellStyle = customCellStyle;
                //->Acto/Condición Subestándar Identificada
                row.CreateCell(3).SetCellValue(observacion.Descripcion_Condicion);
                row.GetCell(3).CellStyle = customCellStyle;
                //->Tipo de Condición / Acto Subestándar
                row.CreateCell(5).SetCellValue(observacion.Tipo_Condicion.Descripcion);
                row.GetCell(5).CellStyle = customCellStyle;
                //->Nivel de Riesgo del Aspecto Ambiental
                row.CreateCell(6).SetCellValue(observacion.Nivel_Riesgo_Ambiental);
                row.GetCell(6).CellStyle = customCellStyle;
                //->Acción Preventiva / Correctiva Tomada
                row.CreateCell(7).SetCellValue(observacion.Descripcion_Accion);
                row.GetCell(7).CellStyle = customCellStyle;
                //->Responsable del Levanamiento del Acto / Condición Subestándar
                row.CreateCell(8).SetCellValue(observacion.Responsable_Marcobre.Nombres + " " + observacion.Responsable_Marcobre.Apellidos);
                row.GetCell(8).CellStyle = customCellStyle;
                //->Fecha Programada
                row.CreateCell(9).SetCellValue(observacion.Fecha_Programada.ToShortDateString());
                row.GetCell(9).CellStyle = customCellStyle;

                row.CreateCell(11).SetCellValue(observacion.Fecha_Cierre.HasValue? observacion.Fecha_Cierre.Value.ToString("dd/MM/yyyy HH:mm"):"");
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
            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);     
        }        
        private double GetRowHeight(int image_width, int image_height)
        {
            double cellWidth = 7.16;//cm
            return (image_height * cellWidth) / image_width;
        }
        //Retorna el tamaño de la fila para la imagen agregada
        private ImageProperties AddImageToCell(HSSFWorkbook templateWorkbook, ISheet sheet,string path,int row, int column, short rowHeight,int imageIndex)
        {
            var output = new ImageProperties();
            var evidenciasCondicion = JArray.Parse(path);
            foreach (var evidencia in evidenciasCondicion)
            {
                var extension = Path.GetExtension((string)evidencia);
                if (extension.Equals(".jpg") || extension.Equals(".png") || extension.Equals(".jpge") || extension.Equals(".JPG") || extension.Equals(".PNG") || extension.Equals(".JPGE"))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes((string)evidencia);
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
                    switch (Path.GetExtension((string)evidencia))
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
    }
}