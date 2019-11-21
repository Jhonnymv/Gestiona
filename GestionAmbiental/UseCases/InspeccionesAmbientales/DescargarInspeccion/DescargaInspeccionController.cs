using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccionDescarga;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
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

            string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath,"ExcelTemplates","Inspecciones");
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
            sheet.GetRow(3).GetCell(7).SetCellValue(cabecera.FechaInspec);
            //->ÁREA EJECUTORA:
            sheet.GetRow(5).GetCell(3).SetCellValue(cabecera.GerenEjecMar);
            //->EMPRESA CONTRATISTA:   
            sheet.GetRow(5).GetCell(8).SetCellValue(cabecera.EmpContratMarc);
            //->ÁREA EPCM *:
            sheet.GetRow(7).GetCell(3).SetCellValue(cabecera.GerenEjecEPCM);
            //->EMPRESA CONTRATISTA EPCM *:   
            sheet.GetRow(7).GetCell(8).SetCellValue(cabecera.EmpContratEPCM);
            //->LUGAR INSPECCIONADO :  
            sheet.GetRow(9).GetCell(3).SetCellValue(cabecera.LugarInspec);
            //->HORA:
            sheet.GetRow(9).GetCell(8).SetCellValue(cabecera.FechaInspec.Hour+":"+cabecera.FechaInspec.Minute);
            //->FECHA   :   
            sheet.GetRow(9).GetCell(11).SetCellValue(cabecera.FechaInspec.Date.ToShortDateString());
            //->RESPONSABLE LUGAR INSPECCIONADO:	
            sheet.GetRow(11).GetCell(3).SetCellValue(cabecera.Responsable_Lugar.Nombres + " "+ cabecera.Responsable_Lugar.Apellidos);
            //->INSPECTOR (ES) :  
            sheet.GetRow(13).GetCell(3).SetCellValue(cabecera.Inspectores);
            //->OBJETIVO DE LA INSPECCIÓN: 	
            sheet.GetRow(15).GetCell(3).SetCellValue(cabecera.Objetivo);

            //->Fila observaciones formato
            int fila = 18;
            //->Cell Style
            HSSFCellStyle customCellStyle = (HSSFCellStyle)templateWorkbook.CreateCellStyle();
            customCellStyle.WrapText = true;
            customCellStyle.VerticalAlignment = VerticalAlignment.Center;
            customCellStyle.BorderTop = BorderStyle.Thin;
            customCellStyle.BorderBottom = BorderStyle.Thin;
            customCellStyle.BorderLeft = BorderStyle.Thin;
            customCellStyle.BorderRight = BorderStyle.Thin;

            foreach (var observacion in cabecera.Observaciones)
            {
                IRow row = sheet.CreateRow(fila);
                
                //->TIPO OBSERVACION
                row.CreateCell(2).SetCellValue(observacion.Tipo_Observacion);
                row.GetCell(2).CellStyle = customCellStyle;
                //->Acto/Condición Subestándar Identificada
                row.CreateCell(3).SetCellValue(observacion.Descripcion_Condicion);
                row.GetCell(3).CellStyle = customCellStyle;
                //->Tipo de Condición / Acto Subestándar
                row.CreateCell(4).SetCellValue(observacion.Tipo_Condicion.Descripcion);
                row.GetCell(4).CellStyle = customCellStyle;
                //->Nivel de Riesgo del Aspecto Ambiental
                row.CreateCell(5).SetCellValue(observacion.Nivel_Riesgo_Ambiental);
                row.GetCell(5).CellStyle = customCellStyle;
                //->Acción Preventiva / Correctiva Tomada
                row.CreateCell(6).SetCellValue(observacion.Descripcion_Accion);
                row.GetCell(6).CellStyle = customCellStyle;
                //->Responsable del Levanamiento del Acto / Condición Subestándar
                row.CreateCell(7).SetCellValue(observacion.Responsable_Marcobre.Nombres + " " + observacion.Responsable_Marcobre.Apellidos);
                row.GetCell(7).CellStyle = customCellStyle;
                //->Responsable Contratista
                row.CreateCell(8).SetCellValue(observacion.Responsable_Contratista);
                row.GetCell(8).CellStyle = customCellStyle;
                //->Fecha Programada
                row.CreateCell(9).SetCellValue(observacion.Fecha_Programada.ToShortDateString());
                row.GetCell(9).CellStyle = customCellStyle;
                //->Comentario Implementación
                row.CreateCell(10).SetCellValue(observacion.Comentario_Inspeccion);
                row.GetCell(10).CellStyle = customCellStyle;
                //->Comentario Verificación
                row.CreateCell(11).SetCellValue(observacion.Comentario_MA);
                row.GetCell(11).CellStyle = customCellStyle;


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
    }
}