using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumosAgua;
using GestionAmbiental.Application.UseCases.ConsumoAgua.GetConsumosAguaDescarga;
using GestionAmbiental.Application.UseCases.ConsumoAgua.Items;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace GestionAmbiental.UseCases.ConsumoAgua.DescargarConsumoAgua
{
    public class DescargarConsumoAguaController : Controller
    {
        private readonly IGetConsumosAgua _getConsumosAgua;
        private readonly IGetConsumosAguaDescarga _getConsumosAguaDescarga;
        private readonly ISelectItems _selectItems;
        private IHostingEnvironment _hostingEnvironment;

        public DescargarConsumoAguaController(
            IGetConsumosAgua getConsumosAgua, 
            IGetConsumosAguaDescarga getConsumosAguaDescarga,
            ISelectItems selectItems,
            IHostingEnvironment hostingEnvironment)
        {
            _getConsumosAgua = getConsumosAgua;
            _getConsumosAguaDescarga = getConsumosAguaDescarga;
            _selectItems = selectItems;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> DescargarReporteConsumo(string FInicio, string FFin, int resolucion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var items = await _selectItems.GetSelectItems();
                var resolucionData = items.ItemsResoluciones.FirstOrDefault(x => x.IdResolucionConsumoAgua.Equals(resolucion));

                var consumosDiario = await _getConsumosAguaDescarga.ExecuteFechas(FInicio, FFin, resolucion);
                var consumosMensual = await _getConsumosAguaDescarga.ExecuteMensual(resolucion);

                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "ConsumoAgua");
                string sFileName = @"FORMATO REPORTE DE CONSUMO DE AGUA.xlsx";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

                // Load the template into a NPOI workbook
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheetDiario = templateWorkbook.GetSheet("REPORTE CONSUMO DIARIO");
                ISheet sheetMensual = templateWorkbook.GetSheet("REPORTE MENSUAL");

                //->Cell Style
                XSSFCellStyle customCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                customCellStyle.WrapText = true;
                customCellStyle.VerticalAlignment = VerticalAlignment.Center;
                customCellStyle.BorderTop = BorderStyle.Thin;
                customCellStyle.BorderBottom = BorderStyle.Thin;
                customCellStyle.BorderLeft = BorderStyle.Thin;
                customCellStyle.BorderRight = BorderStyle.Thin;
                customCellStyle.Alignment = HorizontalAlignment.Center;
            //Reporte Consumo Mensual

                //Año
                sheetMensual.GetRow(7).GetCell(2).SetCellValue(resolucionData.Anio);
                //Clase Derecho
                sheetMensual.GetRow(9).GetCell(2).SetCellValue(resolucionData.ClaseDerecho);
                //Resolucion
                sheetMensual.GetRow(10).GetCell(2).SetCellValue(resolucionData.Resolucion);
                //Origen
                sheetMensual.GetRow(11).GetCell(2).SetCellValue(resolucionData.FuenteAgua.OrigenAgua.Descripcion);
                //Fuente
                sheetMensual.GetRow(12).GetCell(2).SetCellValue(resolucionData.FuenteAgua.Descripcion);
                //Tipo Uso
                sheetMensual.GetRow(13).GetCell(2).SetCellValue(resolucionData.TipoUso);
                //Volumen Otorgado
                sheetMensual.GetRow(14).GetCell(2).SetCellValue(resolucionData.VolumenOtorgado.ToString() + " (m3/año)");
                //Fecha Reporte
                sheetMensual.GetRow(15).GetCell(2).SetCellValue(DateTime.Now.Date.ToString("dd-MMMM-yyyy"));

                int filaMensual = 18;
                for (int i = 0; i < consumosMensual[0].Mes-1; i++)
                {
                    IRow row = sheetMensual.GetRow(filaMensual);

                    row.GetCell(2).SetCellValue(0);
                    row.GetCell(2).CellStyle = customCellStyle;

                    row.GetCell(3).SetCellValue(0);
                    row.GetCell(3).CellStyle = customCellStyle;

                    row.GetCell(4).SetCellValue(0);
                    row.GetCell(4).CellStyle = customCellStyle;

                    row.GetCell(5).SetCellValue(0);
                    row.GetCell(5).CellStyle = customCellStyle;

                    filaMensual++;
                }

                for (int i = 0; i < consumosMensual.Count; i++)
                {
                    IRow row = sheetMensual.GetRow(filaMensual);
                    
                    row.GetCell(2).SetCellValue(consumosMensual[i].TiempoOperacion);
                    row.GetCell(2).CellStyle = customCellStyle;

                    row.GetCell(3).SetCellValue(consumosMensual[i].Caudal);
                    row.GetCell(3).CellStyle = customCellStyle;

                    row.GetCell(4).SetCellValue(consumosMensual[i].VolumenMensual);
                    row.GetCell(4).CellStyle = customCellStyle;

                    row.GetCell(5).SetCellValue(consumosMensual[i].VolumenAcumulado);
                    row.GetCell(5).CellStyle = customCellStyle;

                    filaMensual++;
                }


            //Reporte Consumo Diario

                //Fuente de Agua
                sheetDiario.GetRow(4).GetCell(3).SetCellValue(resolucionData.FuenteAgua.Descripcion);

                int filaDiario = 7;

                for (int i = 0; i < consumosDiario.Count; i++)
                {
                    IRow row = sheetDiario.CreateRow(filaDiario);

                    row.CreateCell(1).SetCellValue(i + 1);
                    row.GetCell(1).CellStyle = customCellStyle;

                    row.CreateCell(2).SetCellValue(consumosDiario[i].Fecha.ToString("dd/MM/yyyy"));
                    row.GetCell(2).CellStyle = customCellStyle;

                    row.CreateCell(3).SetCellValue(consumosDiario[i].TiempoBombeo);
                    row.GetCell(3).CellStyle = customCellStyle;

                    row.CreateCell(4).SetCellValue(consumosDiario[i].LecturaInicial);
                    row.GetCell(4).CellStyle = customCellStyle;

                    row.CreateCell(5).SetCellValue(consumosDiario[i].LecturaFinal);
                    row.GetCell(5).CellStyle = customCellStyle;

                    row.CreateCell(6).SetCellValue(consumosDiario[i].VolumenBombeado);
                    row.GetCell(6).CellStyle = customCellStyle;

                    row.CreateCell(7).SetCellValue(consumosDiario[i].CaudalPromedio);
                    row.GetCell(7).CellStyle = customCellStyle;

                    filaDiario++;
                }
                // Force formulas to update with new data we added
                sheetDiario.ForceFormulaRecalculation = true;
                sheetMensual.ForceFormulaRecalculation = true;
                // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
                MemoryStream ms = new MemoryStream();
                templateWorkbook.Write(ms);

                // Send the memory stream to the browser
                ms.Position = 0;
                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteConsumoAgua-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
            }
        }
    }
}
