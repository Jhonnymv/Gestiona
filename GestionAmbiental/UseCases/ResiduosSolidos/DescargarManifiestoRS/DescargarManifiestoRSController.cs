using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using GestionAmbiental.Application.UseCases.ResiduosSolidos.ControlManifiestoRS.ConsultarManifiestoRSDescarga;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace GestionAmbiental.UseCases.ResiduosSolidos.DescargarManifiestoRS
{
    public class DescargarManifiestoRSController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConsultarManifiestoRSDescarga _consultarManifiestoRSDescarga;
        private readonly IGetInfo_SGA _info_SGA;

        public DescargarManifiestoRSController(IHostingEnvironment hostingEnvironment, IConsultarManifiestoRSDescarga consultarManifiestoRSDescarga, IGetInfo_SGA info_SGA)
        {
            _hostingEnvironment = hostingEnvironment;
            _consultarManifiestoRSDescarga = consultarManifiestoRSDescarga;
            _info_SGA = info_SGA;
        }
        public async Task<IActionResult> DescargarManifiestoRS(string fechaInicio, string fechaFin)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home");
            }
            else
            {
                var manifiestos = await _consultarManifiestoRSDescarga.Execute(fechaInicio, fechaFin);

                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "ResiduoSolido");
                string sFileName = @"JU-MAP-PRO-003-F002 CONTROL INGRESO Y SALIDA RESIDUOS SÓLIDOS ACR O CMRS.xlsx";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

                // Load the template into a NPOI workbook
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheet = templateWorkbook.GetSheet("F003");

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

                for (int i = 0; i < manifiestos.Count; i++)
                {
                    IRow row = sheet.CreateRow(fila);

                    row.CreateCell(0).SetCellValue(manifiestos[i].Item);
                    row.GetCell(0).CellStyle = customCellStyle;

                    row.CreateCell(1).SetCellValue(manifiestos[i].Serie_Manifiesto);
                    row.GetCell(1).CellStyle = customCellStyle;

                    row.CreateCell(2).SetCellValue(manifiestos[i].Num_Manifiesto);
                    row.GetCell(2).CellStyle = customCellStyle;

                    row.CreateCell(3).SetCellValue(manifiestos[i].Fecha_Emision_Manifiesto.ToString("dd/MM/yyyy"));
                    row.GetCell(3).CellStyle = customCellStyle;

                    row.CreateCell(4).SetCellValue(manifiestos[i].Observacion_Manifiesto);
                    row.GetCell(4).CellStyle = customCellStyle;

                    row.CreateCell(5).SetCellValue(manifiestos[i].Residuo_Solido);
                    row.GetCell(5).CellStyle = customCellStyle;

                    row.CreateCell(6).SetCellValue(manifiestos[i].Cantidad_RS_Evacuado);
                    row.GetCell(6).CellStyle = customCellStyle;

                    row.CreateCell(7).SetCellValue(manifiestos[i].EO_RS_Salida);
                    row.GetCell(7).CellStyle = customCellStyle;

                    row.CreateCell(8).SetCellValue(manifiestos[i].Registro_EO_RS_Salida);
                    row.GetCell(8).CellStyle = customCellStyle;

                    row.CreateCell(9).SetCellValue(manifiestos[i].Fecha_Vencimiento_Registro_Salida.ToString("dd/MM/yyyy"));
                    row.GetCell(9).CellStyle = customCellStyle;

                    row.CreateCell(10).SetCellValue(manifiestos[i].Num_Autorizacion_Municipal_Salida);
                    row.GetCell(10).CellStyle = customCellStyle;

                    row.CreateCell(11).SetCellValue(manifiestos[i].Num_Autorizacion_Hoja_Ruta);
                    row.GetCell(11).CellStyle = customCellStyle;

                    row.CreateCell(12).SetCellValue(manifiestos[i].Nombre_Conductor);
                    row.GetCell(12).CellStyle = customCellStyle;

                    row.CreateCell(13).SetCellValue(manifiestos[i].Num_Licencia_Condutor);
                    row.GetCell(13).CellStyle = customCellStyle;

                    row.CreateCell(14).SetCellValue(manifiestos[i].Placa_Vehiculo);
                    row.GetCell(14).CellStyle = customCellStyle;

                    row.CreateCell(15).SetCellValue(manifiestos[i].Capacidad_Vehiculo);
                    row.GetCell(15).CellStyle = customCellStyle;

                    row.CreateCell(16).SetCellValue(manifiestos[i].EO_RS_Llegada);
                    row.GetCell(16).CellStyle = customCellStyle;

                    row.CreateCell(17).SetCellValue(manifiestos[i].Registro_EO_RS_Llegada);
                    row.GetCell(17).CellStyle = customCellStyle;

                    row.CreateCell(18).SetCellValue(manifiestos[i].Fecha_Vencimiento_Registro_Llegada.ToString("dd/MM/yyyy"));
                    row.GetCell(18).CellStyle = customCellStyle;

                    row.CreateCell(19).SetCellValue(manifiestos[i].Num_Autorizacion_Sanitaria_Llegada);
                    row.GetCell(19).CellStyle = customCellStyle;

                    row.CreateCell(20).SetCellValue(manifiestos[i].Num_Autorizacion_Municipal_Llegada);
                    row.GetCell(20).CellStyle = customCellStyle;

                    row.CreateCell(21).SetCellValue("Ver en la plataforma");
                    row.GetCell(21).CellStyle = customCellStyle;

                    row.CreateCell(22).SetCellValue(manifiestos[i].Cantidad_RS_Dispuesto);
                    row.GetCell(22).CellStyle = customCellStyle;

                    row.CreateCell(23).SetCellValue(manifiestos[i].Fecha_Devolucion != null ? manifiestos[i].Fecha_Devolucion.Value.ToString("dd/MM/yyyy") : "");
                    row.GetCell(23).CellStyle = customCellStyle;

                    row.CreateCell(24).SetCellValue("Ver en la plataforma");
                    row.GetCell(24).CellStyle = customCellStyle;

                    row.CreateCell(25).SetCellValue("Ver en la plataforma");
                    row.GetCell(25).CellStyle = customCellStyle;

                    row.CreateCell(26).SetCellValue(manifiestos[i].Asesor_MAP);
                    row.GetCell(26).CellStyle = customCellStyle;

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