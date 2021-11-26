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
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace GestionAmbiental.UseCases.ConsumoAgua.DescargarConsumoAgua
{
    public class DescargarConsumoAguaController : Controller
    {
        private readonly IGetConsumosAgua _getConsumosAgua;

        private readonly IGetConsumosAguaDescarga _getConsumosAguaDescarga;
        private readonly ISelectItems _selectItems;
        private IWebHostEnvironment _hostingEnvironment;

        public DescargarConsumoAguaController(
            IGetConsumosAgua getConsumosAgua, 
            IGetConsumosAguaDescarga getConsumosAguaDescarga,
            ISelectItems selectItems,
            IWebHostEnvironment hostingEnvironment)
        {
            _getConsumosAgua = getConsumosAgua;
            _getConsumosAguaDescarga = getConsumosAguaDescarga;
            _selectItems = selectItems;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> DescargarReporteConsumo(string FInicio, string FFin, int resolucion,int anio)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?FInicio=" + FInicio+"&FFin="+FFin+"&resolucion="+resolucion+"&anio="+ anio });
            }
            else
            {
                var items = await _selectItems.GetSelectItems();
                var resolucionData = items.ItemsResoluciones.FirstOrDefault(x => x.IdResolucionConsumoAgua.Equals(resolucion));

                var consumosDiario = await _getConsumosAguaDescarga.ExecuteFechas(FInicio, FFin, resolucion);
                var consumosMensualAcumulado = await _getConsumosAguaDescarga.ExecuteMensual(resolucion,anio);

                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "ConsumoAgua");
                string sFileName = @"FORMATO REPORTE DE CONSUMO DE AGUA.xlsx";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

                // Load the template into a NPOI workbook
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheetDiario = templateWorkbook.GetSheet("REPORTE CONSUMO DIARIO");
                ISheet sheetAcumulado = templateWorkbook.GetSheet("REPORTE ACUMULADO");
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
                sheetMensual.GetRow(7).GetCell(2).SetCellValue(anio);
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

                for (int i = 0; i < consumosDiario.Count; i++)
                {
                    IRow row = sheetMensual.CreateRow(filaMensual);

                    row.CreateCell(0).SetCellValue(consumosDiario[i].Fecha.ToString("dd/MM/yyyy"));
                    row.GetCell(0).CellStyle = customCellStyle;
                    row.CreateCell(1).CellStyle = customCellStyle;
                    sheetMensual.AddMergedRegion(new CellRangeAddress(row.RowNum, row.RowNum, 0, 1));

                    row.CreateCell(2).SetCellValue(consumosDiario[i].TiempoBombeo);
                    row.GetCell(2).CellStyle = customCellStyle;
                    row.CreateCell(3).CellStyle = customCellStyle;
                    sheetMensual.AddMergedRegion(new CellRangeAddress(row.RowNum, row.RowNum, 2, 3));

                    row.CreateCell(4).SetCellValue(consumosDiario[i].CaudalPromedio);
                    row.GetCell(4).CellStyle = customCellStyle;
                    row.CreateCell(5).CellStyle = customCellStyle;
                    sheetMensual.AddMergedRegion(new CellRangeAddress(row.RowNum, row.RowNum, 4, 5));

                    row.CreateCell(6).SetCellValue(consumosDiario[i].VolumenBombeado);
                    row.GetCell(6).CellStyle = customCellStyle;

                    filaMensual++;
                }
                //Reporte Consumo Acumulado

                //Año
                sheetAcumulado.GetRow(7).GetCell(2).SetCellValue(anio);
                //Clase Derecho
                sheetAcumulado.GetRow(9).GetCell(2).SetCellValue(resolucionData.ClaseDerecho);
                //Resolucion
                sheetAcumulado.GetRow(10).GetCell(2).SetCellValue(resolucionData.Resolucion);
                //Origen
                sheetAcumulado.GetRow(11).GetCell(2).SetCellValue(resolucionData.FuenteAgua.OrigenAgua.Descripcion);
                //Fuente
                sheetAcumulado.GetRow(12).GetCell(2).SetCellValue(resolucionData.FuenteAgua.Descripcion);
                //Tipo Uso
                sheetAcumulado.GetRow(13).GetCell(2).SetCellValue(resolucionData.TipoUso);
                //Volumen Otorgado
                sheetAcumulado.GetRow(14).GetCell(2).SetCellValue(resolucionData.VolumenOtorgado.ToString() + " (m3/año)");
                //Fecha Reporte
                sheetAcumulado.GetRow(15).GetCell(2).SetCellValue(DateTime.Now.Date.ToString("dd-MMMM-yyyy"));
                                
                int filaAcumulado = 18;
                int mes = filaAcumulado - 17;
                if (consumosMensualAcumulado.Count > 0)
                {
                    //LLenar con 0 si los datos no empuezan en Enero
                    for (int i = 0; i < consumosMensualAcumulado[0].Mes - 1; i++)
                    {
                        IRow row = sheetAcumulado.GetRow(filaAcumulado);

                        row.GetCell(2).SetCellValue(0);
                        row.GetCell(2).CellStyle = customCellStyle;

                        row.GetCell(3).SetCellValue(0);
                        row.GetCell(3).CellStyle = customCellStyle;

                        row.GetCell(4).SetCellValue(0);
                        row.GetCell(4).CellStyle = customCellStyle;

                        row.GetCell(5).SetCellValue(0);
                        row.GetCell(5).CellStyle = customCellStyle;

                        filaAcumulado++; 
                        mes = filaAcumulado - 17;
                    }
                    
                    for (int i = 0; i < consumosMensualAcumulado.Count; i++)
                    {
                        IRow row = sheetAcumulado.GetRow(filaAcumulado);
                        
                        row.GetCell(2).SetCellValue(consumosMensualAcumulado[i].TiempoOperacion);
                        row.GetCell(2).CellStyle = customCellStyle;
                        
                        row.GetCell(3).SetCellValue(consumosMensualAcumulado[i].Caudal);
                        row.GetCell(3).CellStyle = customCellStyle;
                        
                        row.GetCell(4).SetCellValue(consumosMensualAcumulado[i].VolumenMensual);
                        row.GetCell(4).CellStyle = customCellStyle;
                        
                        row.GetCell(5).SetCellValue(consumosMensualAcumulado[i].VolumenAcumulado);
                        row.GetCell(5).CellStyle = customCellStyle;
                        
                        filaAcumulado++;                        
                    }
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
                sheetAcumulado.ForceFormulaRecalculation = true;
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
