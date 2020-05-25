using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Accion.GetAcciones;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetCausaIncidenteReporte;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetReportePreliminar;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.Involucrado.GetInvolucrados;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

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

        public DescargarReportePreliminarController(IGetInfo_SGA info_SGA, IHostingEnvironment hostingEnvironment, IGetReportePreliminar getReportePreliminar, IGetInvolucradosIncidente getInvolucradosIncidente, IGetAccionesIncidente getAccionesIncidente, IGetCausasIncidenteReporte getCausasIncidenteReporte)
        {
            _info_SGA = info_SGA;
            _hostingEnvironment = hostingEnvironment;
            _getReportePreliminar = getReportePreliminar;
            _getInvolucradosIncidente = getInvolucradosIncidente;
            _getAccionesIncidente = getAccionesIncidente;
            _getCausasIncidenteReporte = getCausasIncidenteReporte;
        }

        public async Task<MemoryStream> PrepararReportePreliminar(int IdReporte)
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

            sheet.GetRow(10).GetCell(2).SetCellValue(reporte.Razon_Social_Contratista);
            sheet.GetRow(10).GetCell(13).SetCellValue(reporte.RUC_Contratista);
            sheet.GetRow(10).GetCell(22).SetCellValue(reporte.Actividad_Contratista);

            sheet.GetRow(11).GetCell(2).SetCellValue(reporte.Domicilio_Contratista);
            sheet.GetRow(11).GetCell(22).SetCellValue(reporte.Num_Trabajadores_Contratista != null ? reporte.Num_Trabajadores_Contratista.ToString() : "");

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
                row.GetCell(20).SetCellValue(accion.Responsable_Contratista);
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

            sheet.GetRow(filaAcciones + acciones.Count + 2).GetCell(12).SetCellValue(reporte.Supervisor_MA);

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
            var archivoReporte = await PrepararReportePreliminar(IdReporte);
            return File(archivoReporte, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReportePreliminarIncidente-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
        }
    }
}