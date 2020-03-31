using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlIngresoSalidaRS.ConsultarIngresoSalidaRSDescarga;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace GestionAmbiental.UseCases.ResiduosSolidos.DescargarIngresoSalida
{
    public class DescargaIngresoSalidaController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConsultarIngresoSalidaRSDescarga _consultarIngresoSalidaRSDescarga;

        public DescargaIngresoSalidaController(IHostingEnvironment hostingEnvironment, IConsultarIngresoSalidaRSDescarga consultarIngresoSalidaRSDescarga)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _consultarIngresoSalidaRSDescarga = consultarIngresoSalidaRSDescarga ?? throw new ArgumentNullException(nameof(consultarIngresoSalidaRSDescarga));
        }

        public async Task<IActionResult> DescargarIngresoSalida(string fechaInicio, string fechaFin)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var ingreso_salida = await _consultarIngresoSalidaRSDescarga.Execute(fechaInicio, fechaFin);

                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "ResiduoSolido");
                string sFileName = @"JU-MAP-PRO-003-F002 CONTROL INGRESO Y SALIDA RESIDUOS SÓLIDOS ACR O CMRS.xlsx";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

                // Load the template into a NPOI workbook
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheet = templateWorkbook.GetSheet("F002");

                //->Cell Style
                XSSFCellStyle customCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                customCellStyle.WrapText = true;
                customCellStyle.VerticalAlignment = VerticalAlignment.Center;
                customCellStyle.BorderTop = BorderStyle.Thin;
                customCellStyle.BorderBottom = BorderStyle.Thin;
                customCellStyle.BorderLeft = BorderStyle.Thin;
                customCellStyle.BorderRight = BorderStyle.Thin;
                customCellStyle.Alignment = HorizontalAlignment.Center;

                int fila = 9;

                for (int i = 0; i < ingreso_salida.Count; i++)
                {
                    IRow row = sheet.CreateRow(fila);

                    row.CreateCell(0).SetCellValue(ingreso_salida[i].Item);
                    row.GetCell(0).CellStyle = customCellStyle;

                    row.CreateCell(1).SetCellValue(ingreso_salida[i].ACR_CMRS);
                    row.GetCell(1).CellStyle = customCellStyle;

                    row.CreateCell(2).SetCellValue(ingreso_salida[i].Responsable_ACR_CMRS);
                    row.GetCell(2).CellStyle = customCellStyle;

                    row.CreateCell(3).SetCellValue(ingreso_salida[i].Ingreso_Salida);
                    row.GetCell(3).CellStyle = customCellStyle;

                    row.CreateCell(4).SetCellValue(ingreso_salida[i].Motivo);
                    row.GetCell(4).CellStyle = customCellStyle;

                    row.CreateCell(5).SetCellValue(ingreso_salida[i].Fecha_Ingreso_Salida.ToString("dd/MM/yyyy"));
                    row.GetCell(5).CellStyle = customCellStyle;

                    row.CreateCell(6).SetCellValue(ingreso_salida[i].Tipo_Residuo);
                    row.GetCell(6).CellStyle = customCellStyle;

                    row.CreateCell(7).SetCellValue(ingreso_salida[i].Residuo_Solido);
                    row.GetCell(7).CellStyle = customCellStyle;

                    row.CreateCell(8).SetCellValue(ingreso_salida[i].Cantidad);
                    row.GetCell(8).CellStyle = customCellStyle;

                    row.CreateCell(9).SetCellValue(ingreso_salida[i].Empresa);
                    row.GetCell(9).CellStyle = customCellStyle;                    

                    row.CreateCell(10).SetCellValue(ingreso_salida[i].Gerencia);
                    row.GetCell(10).CellStyle = customCellStyle;          

                    row.CreateCell(11).SetCellValue(ingreso_salida[i].Comentario);
                    row.GetCell(11).CellStyle = customCellStyle;

                    fila++;
                }
                // Force formulas to update with new data we added
                sheet.ForceFormulaRecalculation = true;
                // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
                MemoryStream ms = new MemoryStream();
                templateWorkbook.Write(ms);

                // Send the memory stream to the browser
                ms.Position = 0;
                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteManifiestos-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
            }
        }
    }
}