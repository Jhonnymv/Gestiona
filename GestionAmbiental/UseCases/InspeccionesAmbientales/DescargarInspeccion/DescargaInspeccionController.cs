using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Info_SGA;
using GestionAmbiental.Application.UseCases.Inspecciones.ConsultarInspeccionDescarga;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using NPOI.XSSF.UserModel;
using NPOI.XSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Renci.SshNet;
using Microsoft.AspNetCore.DataProtection;
using NPOI.Util;
using Serilog;
using GestionAmbiental.UseCases.Util.Interfaces;
using NPOI.SS.Formula.Functions;
using NPOI.HSSF.UserModel;
using static GestionAmbiental.UseCases.InspeccionesAmbientales.DescargarInspeccion.DescargaInspeccionController;
using Microsoft.Extensions.Hosting;
using NPOI.HPSF;
using static Google.Cloud.RecaptchaEnterprise.V1.TransactionData.Types;

namespace GestionAmbiental.UseCases.InspeccionesAmbientales.DescargarInspeccion
{
    public class DescargaInspeccionController : Controller
    {
        private readonly IGestorArchivos _gestorArchivos;
        private readonly IGestorEncriptado _gestorEncriptado;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IConsultarInspeccionDescarga _consultarInspeccionDescarga;
        private readonly IGetInfo_SGA _info_SGA;
        private readonly IDataProtector _protector;
        public DescargaInspeccionController(
            IWebHostEnvironment hostingEnvironment,
            IGestorArchivos gestorArchivos,
            IGestorEncriptado gestorEncriptado,
            IConsultarInspeccionDescarga consultarInspeccionDescarga,
            IGetInfo_SGA info_SGA,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _gestorArchivos = gestorArchivos;
            _gestorEncriptado = gestorEncriptado;
            _hostingEnvironment = hostingEnvironment;
            _consultarInspeccionDescarga = consultarInspeccionDescarga;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InspeccionIdRouteValue);
            _info_SGA = info_SGA;
        }

        public async Task<IActionResult> DescargaInspeccion(int IdCabecera)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdCabecera=" + IdCabecera });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var cabecera = await _consultarInspeccionDescarga.Execute(IdCabecera);

                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "Inspecciones");
                string sFileName = @"JU-MAP-PRO-004.F01_Inspección_V00.xlsx";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

                // Load the template into a NPOI workbook
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheet = templateWorkbook.GetSheet("Inspección");

                // Insert data into template
                //->Codigo
                //sheet.GetRow(2).GetCell(4).SetCellValue(cabecera.Codigo);
                //->Fecha Aprobacion
                //sheet.GetRow(3).GetCell(7).SetCellValue(cabecera.FechaInspec);
                //->ÁREA EJECUTORA:
                sheet.GetRow(11).GetCell(7).SetCellValue(cabecera.Gerencia);
                //->LUGAR INSPECCIONADO
                sheet.GetRow(11).GetCell(15).SetCellValue(cabecera.GerenciaInspeccionada + " - " + cabecera.AreaInspeccionada);
                //->EMPRESA:   
                sheet.GetRow(11).GetCell(24).SetCellValue(cabecera.Empresa);
                //->TIPO DE INSPECCION
                sheet.GetRow(13).GetCell(7).SetCellValue(cabecera.TipoInspeccion);
                //->HORA
                sheet.GetRow(13).GetCell(15).SetCellValue(cabecera.FechaInspeccion.Hour + ":" + cabecera.FechaInspeccion.Minute);
                //->FECHA  
                sheet.GetRow(13).GetCell(18).SetCellValue(cabecera.FechaInspeccion.ToString("dd/MM/yyyy"));
                //->PARTICIPANTES DE LA INSPECCION :  
                sheet.GetRow(13).GetCell(24).SetCellValue(cabecera.Participantes);
                //->RESPONSABLE DEL ÁREA INSPECCIONADA:	
                sheet.GetRow(15).GetCell(7).SetCellValue(cabecera.ResponsableArea.Nombres + " " + cabecera.ResponsableArea.Apellidos);
                //->RESPONSABLE DE LA INSPECCION:	
                sheet.GetRow(15).GetCell(24).SetCellValue(cabecera.ResponsableInspeccion);
                //->OBJETIVO DE LA INSPECCIÓN: 	
                sheet.GetRow(17).GetCell(7).SetCellValue(cabecera.Objetivo);

                //->Fila observaciones en el formato
                int fila = 25;
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
                //indice de observacion
                int obsIndex = 1;

                foreach (var observacion in cabecera.Observaciones)
                {
                    short rowHeight = 850;
                    IRow row = sheet.CreateRow(fila);

                    //Aplicar cell style a una fila de observación
                    for (int i = 0; i <= 30; i++)
                    {
                        row.CreateCell(i);
                        row.GetCell(i).CellStyle = customCellStyle;
                    }
                    //Combinar celdas
                    //Nro
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 0, 1));
                    //Acto/Condición subestándar identificada
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 3, 8));
                    //Fotografía de la condición subestándar
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 9, 10));
                    //Tipo de Condición/Acto Subestándar
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 11, 13));
                    //Descripción de Condición/Acto Subestándar
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 14, 16));
                    //Acción preventiva/correctiva
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 18, 22));
                    //Levantamiento de la Observación
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(fila, fila, 25, 28));
                    //Fecha de cierre
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(fila, fila, 29, 30));
                    //Evidencia condición
                    row.GetCell(9).CellStyle = customCellStyle;
                    //Evidencia implementación
                    row.GetCell(25).CellStyle = customCellStyle;

                    ImageProperties evidencia_condicion = new ImageProperties();
                    ImageProperties evidencia_inspeccion = new ImageProperties();

                    if (observacion.EvidenciaCondicion != "[]" && observacion.EvidenciaCondicion != null)
                    {
                        evidencia_condicion = await AddImageToCellAsync(templateWorkbook, sheet, observacion.EvidenciaCondicion, fila, 9, rowHeight, imageIndex, "condicion", false);
                        if (evidencia_condicion != null)
                        {
                            rowHeight = rowHeight > evidencia_condicion.rowHeight ? rowHeight : evidencia_condicion.rowHeight;
                            imageIndex++;
                        }
                    }
                    if (observacion.EvidenciaImplementacion != "[]" && observacion.EvidenciaImplementacion != null)
                    {

                        evidencia_inspeccion = await AddImageToCellAsync(templateWorkbook, sheet, observacion.EvidenciaImplementacion, fila, 25, rowHeight, imageIndex, "inspeccion", false);
                        if (evidencia_condicion != null)
                        {
                            rowHeight = rowHeight > evidencia_inspeccion.rowHeight ? rowHeight : evidencia_inspeccion.rowHeight;
                            imageIndex++;
                        }

                    }
                    //adaptar el tamaño de la fila al mayor determinado por la imagen
                    row.Height = rowHeight;
                    //rezise images

                    if ((evidencia_condicion.extension == ".jpg" ||
                        evidencia_condicion.extension == ".png" ||
                        evidencia_condicion.extension == ".jpeg" ||
                        evidencia_condicion.extension == ".JPG" ||
                        evidencia_condicion.extension == ".PNG" ||
                        evidencia_condicion.extension == ".JPEG" &&
                        evidencia_condicion.extension != null) &&
                        evidencia_condicion.rowHeight != 0 &&
                        evidencia_condicion.rowHeight < row.Height)
                    {
                        evidencia_condicion.image.Resize(1.0, (double)evidencia_condicion.rowHeight / row.Height);
                    }

                    if ((evidencia_inspeccion.extension == ".jpg" ||
                        evidencia_inspeccion.extension == ".png" ||
                        evidencia_inspeccion.extension == ".jpeg" ||
                        evidencia_inspeccion.extension == ".JPG" ||
                        evidencia_inspeccion.extension == ".PNG" ||
                        evidencia_inspeccion.extension == ".JPEG") &&
                        evidencia_inspeccion.extension != null &&
                        evidencia_inspeccion.rowHeight != 0 &&
                        evidencia_inspeccion.rowHeight < row.Height)
                    {
                        evidencia_inspeccion.image.Resize(1.0, (double)evidencia_inspeccion.rowHeight / row.Height);
                    }

                    //->Index
                    row.GetCell(0).SetCellValue(obsIndex);
                    //->TIPO OBSERVACION
                    row.GetCell(2).SetCellValue(observacion.TipoObservacion);
                    //->Acto/Condición Subestándar Identificada
                    row.GetCell(3).SetCellValue(observacion.DescripcionCondicion);
                    //->Tipo de Condición / Acto Subestándar
                    row.GetCell(11).SetCellValue(observacion.TipoCondicion?.Descripcion);
                    //->Descripción de Condición / Acto Subestándar
                    row.GetCell(14).SetCellValue(observacion.DescripcionActoCondicion);
                    //->Nivel de Riesgo
                    row.GetCell(17).SetCellValue(observacion.NivelRiesgo);
                    //->Acción Preventiva / Correctiva Tomada
                    row.GetCell(18).SetCellValue(observacion.DescripcionAccion);
                    //->Responsable del Levanamiento del Acto / Condición Subestándar
                    row.GetCell(23).SetCellValue(observacion.ResponsableMarcobre2 != null ? (observacion.ResponsableMarcobre.Nombres + " " + observacion.ResponsableMarcobre.Apellidos + " - " + observacion.ResponsableMarcobre2.Nombres + " " + observacion.ResponsableMarcobre2.Apellidos) : observacion.ResponsableMarcobre.Nombres + " " + observacion.ResponsableMarcobre.Apellidos);
                    //->Fecha Programada
                    row.GetCell(24).SetCellValue(observacion.FechaProgramada.HasValue ? observacion.FechaProgramada.Value.ToString("dd/MM/yyyy") : "");

                    row.GetCell(29).SetCellValue(observacion.FechaCierre.HasValue ? observacion.FechaCierre.Value.ToString("dd/MM/yyyy") : "");

                    obsIndex++;
                    fila++;

                }

                // Force formulas to update with new data we added
                sheet.ForceFormulaRecalculation = true;
                // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
                MemoryStream ms = new MemoryStream();
                templateWorkbook.Write(ms);

                // Send the memory stream to the browser
                ms.Position = 0;
                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteInspección-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
            }
        }

        public async Task<IActionResult> DescargaInspeccionV2(int IdCabecera)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdCabecera=" + IdCabecera });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var cabecera = await _consultarInspeccionDescarga.Execute(IdCabecera);

                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "Inspecciones");
                string sFileName = @"JU-MAP-PRO-004.F01_Inspección_V01.xlsx";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

                // Load the template into a NPOI workbook
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheet = templateWorkbook.GetSheet("Inspección");

                // Insert data into template
                //->Codigo
                //sheet.GetRow(2).GetCell(4).SetCellValue(cabecera.Codigo);
                //->Fecha Aprobacion
                //sheet.GetRow(3).GetCell(7).SetCellValue(cabecera.FechaInspec);
                //->Cell Style
                XSSFCellStyle customCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                customCellStyle.WrapText = true;
                customCellStyle.VerticalAlignment = VerticalAlignment.Center;
                customCellStyle.BorderTop = BorderStyle.Thin;
                customCellStyle.BorderBottom = BorderStyle.Thin;
                customCellStyle.BorderLeft = BorderStyle.Thin;
                customCellStyle.BorderRight = BorderStyle.Thin;
                customCellStyle.Alignment = HorizontalAlignment.Center;

                XSSFCellStyle customRigthBorderCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                customRigthBorderCellStyle.BorderRight = BorderStyle.Thin;

                //->CODIGO FORMATO:
                sheet.GetRow(1).GetCell(8).SetCellValue(cabecera.Codigo_Formato);
                //->ÁREA FORMATO:
                sheet.GetRow(2).GetCell(8).SetCellValue(cabecera.Area_Formato);

                //->ÁREA EJECUTORA:
                sheet.GetRow(11).GetCell(7).SetCellValue(cabecera.Gerencia);
                //->LUGAR INSPECCIONADO
                sheet.GetRow(11).GetCell(15).SetCellValue(cabecera.GerenciaInspeccionada + " - " + cabecera.AreaInspeccionada);
                //->EMPRESA:   
                sheet.GetRow(11).GetCell(24).SetCellValue(cabecera.Empresa);
                //->TIPO DE INSPECCION
                sheet.GetRow(13).GetCell(7).SetCellValue(cabecera.TipoInspeccion);
                //->HORA
                sheet.GetRow(13).GetCell(15).SetCellValue(cabecera.FechaInspeccion.Hour + ":" + cabecera.FechaInspeccion.Minute);
                //->FECHA  
                sheet.GetRow(13).GetCell(18).SetCellValue(cabecera.FechaInspeccion.ToString("dd/MM/yyyy"));
                //->PARTICIPANTES DE LA INSPECCION :
                var contadorColumnasParticipantes = 0;
                var contadorFilasParticipantes = 0;

                //sheet.ShiftRows(13 + 1, sheet.LastRowNum, 1);
                int rowsToShift = cabecera.Participantes_Inspección_Detalle.Count % 2 == 0 ? Convert.ToInt32(cabecera.Participantes_Inspección_Detalle.Count / 2) - 1 : Convert.ToInt32(cabecera.Participantes_Inspección_Detalle.Count / 2);
                if(rowsToShift>0)
                    sheet.ShiftRows(14, sheet.LastRowNum, rowsToShift);
                
                foreach (var item in cabecera.Participantes_Inspección_Detalle)
                {
                    sheet.GetRow(13+ contadorFilasParticipantes).GetCell(24 + (contadorColumnasParticipantes*3)).SetCellValue(item.Nombres + " "+ item.Apellidos);
                    if (contadorColumnasParticipantes == 1 && rowsToShift>contadorFilasParticipantes)
                    {
                        contadorColumnasParticipantes = 0;
                        contadorFilasParticipantes++;

                        IRow row = sheet.CreateRow(13 + contadorFilasParticipantes);
                        row.CreateCell(24 + contadorColumnasParticipantes);
                        row.CreateCell(24 + contadorColumnasParticipantes + 1);
                        row.CreateCell(24 + contadorColumnasParticipantes + 2);
                        sheet.AddMergedRegion(new CellRangeAddress(13 + contadorFilasParticipantes, 13 + contadorFilasParticipantes, 24, 26));
                        row.GetCell(24 + contadorColumnasParticipantes).CellStyle = customCellStyle;
                        row.GetCell(24 + contadorColumnasParticipantes+1).CellStyle = customCellStyle;
                        row.GetCell(24 + contadorColumnasParticipantes+2).CellStyle = customCellStyle;
                    }
                    else
                    {
                        if (contadorColumnasParticipantes == 0)
                        {
                            contadorColumnasParticipantes++;
                            IRow row = sheet.GetRow(13 + contadorFilasParticipantes);
                            row.CreateCell(24 + (contadorColumnasParticipantes * 3));
                            row.CreateCell(24 + (contadorColumnasParticipantes * 3) + 1);
                            row.CreateCell(24 + (contadorColumnasParticipantes * 3) + 2);
                            row.CreateCell(24 + (contadorColumnasParticipantes * 3) + 3);
                            sheet.AddMergedRegion(new CellRangeAddress(13 + contadorFilasParticipantes, 13 + contadorFilasParticipantes, 27, 29));
                            row.GetCell(24 + (contadorColumnasParticipantes * 3)).CellStyle = customCellStyle;
                            row.GetCell(24 + (contadorColumnasParticipantes * 3) + 1).CellStyle = customCellStyle;
                            row.GetCell(24 + (contadorColumnasParticipantes * 3) + 2).CellStyle = customCellStyle;
                            row.GetCell(24 + (contadorColumnasParticipantes * 3) + 3).CellStyle = customRigthBorderCellStyle;
                        }
                    }
                }
                //->Otros Participantes, en caso aplique:					
                sheet.GetRow(15 + rowsToShift).GetCell(7).SetCellValue(cabecera.Participantes);
                sheet.GetRow(15 + rowsToShift).Height = 500;
                //->Nombre del Representante(s) de los Trabajadores que participó en la inspección, en caso aplique.
                sheet.GetRow(15+ rowsToShift).GetCell(24).SetCellValue(cabecera.Miembro_Comite);
                //->RESPONSABLE DEL ÁREA INSPECCIONADA:	
                sheet.GetRow(17 + rowsToShift).GetCell(7).SetCellValue(cabecera.ResponsableArea.Nombres + " " + cabecera.ResponsableArea.Apellidos);
                //->RESPONSABLE DE LA INSPECCION:	
                sheet.GetRow(17 + rowsToShift).GetCell(24).SetCellValue(cabecera.ResponsableInspeccion);
                sheet.GetRow(17 + rowsToShift).Height = 500;
                //->OBJETIVO DE LA INSPECCIÓN: 	
                sheet.GetRow(19 + rowsToShift).GetCell(7).SetCellValue(cabecera.Objetivo);
                sheet.GetRow(19 + rowsToShift).Height = 500;
                //->RESPONSALBE DEL REGISTRO: 	
                //sheet.GetRow(21 + rowsToShift).GetCell(7).SetCellValue(cabecera.RegistradoPor);
                //->CODIGO INSPECCION: 	
                sheet.GetRow(21 + rowsToShift).GetCell(23).SetCellValue(cabecera.Codigo);
                sheet.GetRow(21 + rowsToShift).Height = 500;

                //->Fila observaciones en el formato
                int fila = 27+ rowsToShift;
                


                //índice de la primera imagen a insertar en la colección de images del workbook
                int imageIndex = 4;
                //indice de observacion
                int obsIndex = 1;

                XSSFCellStyle obsRegistradoStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                obsRegistradoStyle.WrapText = true;
                obsRegistradoStyle.VerticalAlignment = VerticalAlignment.Center;
                obsRegistradoStyle.BorderTop = BorderStyle.Thin;
                obsRegistradoStyle.BorderBottom = BorderStyle.Thin;
                obsRegistradoStyle.BorderLeft = BorderStyle.Thin;
                obsRegistradoStyle.BorderRight = BorderStyle.Thin;
                obsRegistradoStyle.Alignment = HorizontalAlignment.Center;
                obsRegistradoStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                obsRegistradoStyle.FillPattern = FillPattern.SolidForeground;

                XSSFCellStyle obsImplementadoStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                obsImplementadoStyle.WrapText = true;
                obsImplementadoStyle.VerticalAlignment = VerticalAlignment.Center;
                obsImplementadoStyle.BorderTop = BorderStyle.Thin;
                obsImplementadoStyle.BorderBottom = BorderStyle.Thin;
                obsImplementadoStyle.BorderLeft = BorderStyle.Thin;
                obsImplementadoStyle.BorderRight = BorderStyle.Thin;
                obsImplementadoStyle.Alignment = HorizontalAlignment.Center;
                obsImplementadoStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightBlue.Index;
                obsImplementadoStyle.FillPattern = FillPattern.SolidForeground;

                XSSFCellStyle obsAprobadoStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                obsAprobadoStyle.WrapText = true;
                obsAprobadoStyle.VerticalAlignment = VerticalAlignment.Center;
                obsAprobadoStyle.BorderTop = BorderStyle.Thin;
                obsAprobadoStyle.BorderBottom = BorderStyle.Thin;
                obsAprobadoStyle.BorderLeft = BorderStyle.Thin;
                obsAprobadoStyle.BorderRight = BorderStyle.Thin;
                obsAprobadoStyle.Alignment = HorizontalAlignment.Center;
                obsAprobadoStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightGreen.Index;
                obsAprobadoStyle.FillPattern = FillPattern.SolidForeground;

                XSSFCellStyle obsVencidoStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                obsVencidoStyle.WrapText = true;
                obsVencidoStyle.VerticalAlignment = VerticalAlignment.Center;
                obsVencidoStyle.BorderTop = BorderStyle.Thin;
                obsVencidoStyle.BorderBottom = BorderStyle.Thin;
                obsVencidoStyle.BorderLeft = BorderStyle.Thin;
                obsVencidoStyle.BorderRight = BorderStyle.Thin;
                obsVencidoStyle.Alignment = HorizontalAlignment.Center;
                obsVencidoStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
                obsVencidoStyle.FillPattern = FillPattern.SolidForeground;

                foreach (var observacion in cabecera.Observaciones)
                {
                    short rowHeight = 850;
                    IRow row = sheet.CreateRow(fila);

                    //Aplicar cell style a una fila de observación
                    for (int i = 0; i <= 30; i++)
                    {
                        row.CreateCell(i);
                        row.GetCell(i).CellStyle = customCellStyle;
                    }
                    //Combinar celdas
                    //Nro
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 0, 1));
                    //Acto/Condición subestándar identificada
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 3, 8));
                    //Fotografía de la condición subestándar
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 9, 10));
                    //Tipo de Condición/Acto Subestándar
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 11, 13));
                    //Descripción de Condición/Acto Subestándar
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 14, 16));
                    //Acción preventiva/correctiva
                    sheet.AddMergedRegion(new CellRangeAddress(fila, fila, 18, 21));
                    //Levantamiento de la Observación
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(fila, fila, 24, 27));
                    //Fecha de cierre
                    //sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(fila, fila, 28, 29));
                    //Estado
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(fila, fila, 29, 30));
                    //Evidencia condición
                    row.GetCell(9).CellStyle = customCellStyle;
                    //Evidencia implementación
                    row.GetCell(25).CellStyle = customCellStyle;

                    ImageProperties evidencia_condicion = new ImageProperties();
                    ImageProperties evidencia_inspeccion = new ImageProperties();

                    if (observacion.EvidenciaCondicion != "[]" && observacion.EvidenciaCondicion != null)
                    {
                        evidencia_condicion = await AddImageToCellAsync(templateWorkbook, sheet, observacion.EvidenciaCondicion, fila, 9, rowHeight, imageIndex, "condicion", false);
                        if (evidencia_condicion != null)
                        {
                            rowHeight = rowHeight > evidencia_condicion.rowHeight ? rowHeight : evidencia_condicion.rowHeight;
                            imageIndex++;
                        }
                    }
                    if (observacion.EvidenciaImplementacion != "[]" && observacion.EvidenciaImplementacion != null)
                    {

                        evidencia_inspeccion = await AddImageToCellAsync(templateWorkbook, sheet, observacion.EvidenciaImplementacion, fila, 24, rowHeight, imageIndex, "inspeccion", false);
                        if (evidencia_condicion != null)
                        {
                            rowHeight = rowHeight > evidencia_inspeccion.rowHeight ? rowHeight : evidencia_inspeccion.rowHeight;
                            imageIndex++;
                        }

                    }
                    //adaptar el tamaño de la fila al mayor determinado por la imagen
                    row.Height = rowHeight;
                    //rezise images

                    if ((evidencia_condicion.extension == ".jpg" ||
                        evidencia_condicion.extension == ".png" ||
                        evidencia_condicion.extension == ".jpeg" ||
                        evidencia_condicion.extension == ".JPG" ||
                        evidencia_condicion.extension == ".PNG" ||
                        evidencia_condicion.extension == ".JPEG" &&
                        evidencia_condicion.extension != null) &&
                        evidencia_condicion.rowHeight != 0 &&
                        evidencia_condicion.rowHeight < row.Height)
                    {
                        evidencia_condicion.image.Resize(1.0, (double)evidencia_condicion.rowHeight / row.Height);
                    }

                    if ((evidencia_inspeccion.extension == ".jpg" ||
                        evidencia_inspeccion.extension == ".png" ||
                        evidencia_inspeccion.extension == ".jpeg" ||
                        evidencia_inspeccion.extension == ".JPG" ||
                        evidencia_inspeccion.extension == ".PNG" ||
                        evidencia_inspeccion.extension == ".JPEG") &&
                        evidencia_inspeccion.extension != null &&
                        evidencia_inspeccion.rowHeight != 0 &&
                        evidencia_inspeccion.rowHeight < row.Height)
                    {
                        evidencia_inspeccion.image.Resize(1.0, (double)evidencia_inspeccion.rowHeight / row.Height);
                    }

                    //->Index
                    row.GetCell(0).SetCellValue(obsIndex);
                    //->TIPO OBSERVACION
                    row.GetCell(2).SetCellValue(observacion.TipoObservacion.Equals("MAP")?"MA": observacion.TipoObservacion);
                    //->Acto/Condición Subestándar Identificada
                    row.GetCell(3).SetCellValue(observacion.DescripcionCondicion);
                    //->Tipo de Condición / Acto Subestándar
                    row.GetCell(11).SetCellValue(observacion.TipoCondicion?.Descripcion);
                    //->Descripción de Condición / Acto Subestándar
                    row.GetCell(14).SetCellValue(observacion.DescripcionActoCondicion);
                    //->Nivel de Riesgo
                    row.GetCell(17).SetCellValue(observacion.NivelRiesgo);
                    //->Acción Preventiva / Correctiva Tomada
                    row.GetCell(18).SetCellValue(observacion.DescripcionAccion);
                    //->Responsable del Levanamiento del Acto / Condición Subestándar
                    row.GetCell(22).SetCellValue(observacion.ResponsableMarcobre2 != null ? (observacion.ResponsableMarcobre.Nombres + " " + observacion.ResponsableMarcobre.Apellidos + " - " + observacion.ResponsableMarcobre2.Nombres + " " + observacion.ResponsableMarcobre2.Apellidos) : observacion.ResponsableMarcobre.Nombres + " " + observacion.ResponsableMarcobre.Apellidos);
                    //->Fecha Programada
                    row.GetCell(23).SetCellValue(observacion.FechaProgramada.HasValue ? observacion.FechaProgramada.Value.ToString("dd/MM/yyyy") : "");
                    //->Fecha Cierre
                    row.GetCell(28).SetCellValue(observacion.FechaCierre.HasValue ? observacion.FechaCierre.Value.ToString("dd/MM/yyyy") : "");
                    //->Estado
                    var estadoTexto = "";
                    switch (observacion.Estado)
                    {
                        case "Aprobado":
                            estadoTexto = "Aprobado"; row.GetCell(29).CellStyle = obsAprobadoStyle; break;
                        case "Vencido":
                            estadoTexto = "Vencido"; row.GetCell(29).CellStyle = obsVencidoStyle; break;
                        case "Implementado":
                            estadoTexto = "Pendiente de Verificación"; row.GetCell(29).CellStyle = obsImplementadoStyle; break;
                        default:
                            estadoTexto = "Pendiente de Implementación"; row.GetCell(29).CellStyle = obsRegistradoStyle; break;
                    }
                    row.GetCell(29).SetCellValue(estadoTexto);


                    obsIndex++;
                    fila++;

                }

                // Force formulas to update with new data we added
                sheet.ForceFormulaRecalculation = true;
                // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
                MemoryStream ms = new MemoryStream();
                templateWorkbook.Write(ms);

                // Send the memory stream to the browser
                ms.Position = 0;
                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteInspección-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
            }
        }
        public async Task<IActionResult> DescargarFechas(int empresa, int gerencia, int respLevObs, int asesor, string FechaInicio, string FechaFin, string estado, int idGerenciaInspeccionada, bool incluirEvidencias,int usuarioInvolucrado, string tipoObservacion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?empresa=" + empresa + "&gerencia=" + gerencia + "&respLevObs=" + respLevObs + "&asesor=" + asesor + "&FechaInicio=" + FechaInicio + "&FechaFin=" + FechaFin + "&estado=" + estado + "&idGerenciaInspeccionada=" + idGerenciaInspeccionada + "&incluirEvidencias=" + incluirEvidencias + "&usuarioInvolucrado=" + usuarioInvolucrado + "&tipoObservacion=" + tipoObservacion });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                var inspecciones = await _consultarInspeccionDescarga.Execute(empresa, gerencia, respLevObs, asesor, FechaInicio, FechaFin, estado, idGerenciaInspeccionada, usuarioInvolucrado, tipoObservacion);
                inspecciones = inspecciones.OrderBy(x => x.FechaInspeccion).ToList();
                var observaciones = new List<ObservacionReporte>();
                string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "Inspecciones");
                string sFileName = @"DescargaObservacionesInspección.xlsx";
                string sFilePath = Path.Combine(sWebRootFolder, sFileName);
                FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);
                // Load the template into a NPOI workbook
                XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

                // Load the sheet you are going to use as a template into NPOI
                ISheet sheet = templateWorkbook.GetSheet("Observaciones");
                //->Fila observaciones en el formato
                int fila = 2;
                //->Cell Style
                XSSFCellStyle estiloNormal = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                XSSFCellStyle estiloImplementacion = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                XSSFFont fuenteImplementacion = (XSSFFont)templateWorkbook.CreateFont();
                XSSFCellStyle estiloVerificacion = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                XSSFFont fuenteVerificacion = (XSSFFont)templateWorkbook.CreateFont();
                XSSFCellStyle estiloVencido = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                XSSFFont fuenteVencido = (XSSFFont)templateWorkbook.CreateFont();
                XSSFCellStyle estiloAprobado = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                XSSFFont fuenteAprobado = (XSSFFont)templateWorkbook.CreateFont();
                XSSFCellStyle estiloRechazado = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
                XSSFFont fuenteRechazado = (XSSFFont)templateWorkbook.CreateFont();

                var estilos = new List<Estilo>();
                estilos.Add(new Estilo("Pendiente de Implementación", templateWorkbook, (byte)255, (byte)240, (byte)128, IndexedColors.DarkYellow.Index));
                estilos.Add(new Estilo("Pendiente de Verificación", templateWorkbook, (byte)128, (byte)255, (byte)240, IndexedColors.DarkBlue.Index));
                estilos.Add(new Estilo("Vencido", templateWorkbook, (byte)255, (byte)103, (byte)103, IndexedColors.DarkRed.Index));
                estilos.Add(new Estilo("Aprobado", templateWorkbook, (byte)159, (byte)255, (byte)128, IndexedColors.DarkGreen.Index));
                estilos.Add(new Estilo("Rechazado", templateWorkbook, (byte)255, (byte)222, (byte)128, IndexedColors.Orange.Index));

                estiloNormal.WrapText = true;
                estiloNormal.VerticalAlignment = VerticalAlignment.Center;
                estiloNormal.BorderTop = BorderStyle.Thin;
                estiloNormal.BorderBottom = BorderStyle.Thin;
                estiloNormal.BorderLeft = BorderStyle.Thin;
                estiloNormal.BorderRight = BorderStyle.Thin;
                estiloNormal.Alignment = HorizontalAlignment.Center;

                foreach (var inspeccion in inspecciones)
                    foreach (var observacion in inspeccion.Observaciones)
                    {
                        var estadoObs = "";
                        switch (observacion.Estado)
                        {
                            case "Registrado":
                            case "Ampliado":
                                estadoObs = "Pendiente de Implementación";
                                break;
                            case "Implementado":
                                estadoObs = "Pendiente de Verificación";
                                break;
                            case "Vencido":
                                estadoObs = "Vencido";
                                break;
                            case "Aprobado":
                                estadoObs = "Aprobado";
                                break;
                            case "Rechazado":
                                if (observacion.DescripcionCondicion.Equals("Buenas Prácticas"))
                                    estadoObs = "Rechazado";
                                else
                                    estadoObs = "Pendiente de Implementación";
                                break;
                        }
                        observaciones.Add(new ObservacionReporte
                        {
                            IdInspeccion = inspeccion.IdCabecera.ToString(),
                            IdObservacion = observacion.IdInspeccionDetalle,
                            SSO_MA = observacion.TipoObservacion.Equals("MAP")?"MA": observacion.TipoObservacion,
                            ActoCondicion = observacion.DescripcionCondicion,
                            EvidenciaCondicion = observacion.EvidenciaCondicion,
                            DescripcionActoCondicion = observacion.DescripcionActoCondicion,
                            TipoCondicion = observacion.TipoCondicion != null ? observacion.TipoCondicion.Descripcion : "No Aplica",
                            NivelRiesgo = observacion.NivelRiesgo != null ? observacion.NivelRiesgo : "No Aplica",
                            Accion = observacion.DescripcionAccion != null ? observacion.DescripcionAccion : "No Aplica",
                            ResponsableLevantamiento = observacion.ResponsableMarcobre2 != null ?
                                                        (observacion.ResponsableMarcobre.Nombres + " " + observacion.ResponsableMarcobre.Apellidos + " - " + observacion.ResponsableMarcobre2.Nombres + " " + observacion.ResponsableMarcobre2.Apellidos) :
                                                        observacion.ResponsableMarcobre.Nombres + " " + observacion.ResponsableMarcobre.Apellidos,
                            FechaProgramada = observacion.FechaProgramada.HasValue ? observacion.FechaProgramada.Value.ToString("dd/MM/yyyy") : "No Aplica",
                            EvidenciaImplementacion = observacion.EvidenciaImplementacion,
                            FechaCierre = observacion.FechaCierre.HasValue ? observacion.FechaCierre.Value.ToString("dd/MM/yyyy") : "No Aplica",
                            Estado = estadoObs,
                            ResponsableInspeccion = inspeccion.ResponsableInspeccion,
                            FechaInspeccion = inspeccion.FechaInspeccion.ToString("dd/MM/yyyy"),
                            RiesgoFatalidad = "",
                            GerenciaLevantamiento = inspeccion.GerenciaInspeccionada,
                            GerenciaInspeccion = inspeccion.Gerencia,
                            EmpresaInspeccionada = inspeccion.Empresa,
                            ParticipantesInspeccion = inspeccion.Participantes_Inspección_Texto,
                            Asesor = observacion.Asesor.Nombres + " " + observacion.Asesor.Apellidos,
                            Codigo = inspeccion.Codigo,
                            AreaInspeccionada = inspeccion.AreaInspeccionada,
                            TipoInspeccion = inspeccion.TipoInspeccion
                        });
                    }
                observaciones = observaciones.OrderBy(x => x.FechaInspeccion).ToList();

                //índice de la primera imagen a insertar en la colección de images del workbook
                int imageIndex = -1;
                //indice de observacion
                int obsIndex = 1;

                //Primer recorrido para descargar todas las imágenes en la carpeta temporal
                var listaEvidenciasxObservacion = new List<Evidencia>();
                foreach (var observacion in observaciones)
                {
                    var item = new Evidencia();
                    item.IdObservacion = observacion.IdObservacion;


                    if (observacion.EvidenciaCondicion != "[]" && observacion.EvidenciaCondicion != null)
                    {
                        Tipo_Info_SGA directorio = Tipo_Info_SGA.FTPConnectionEvidenciaR;
                        item.NombreEvidenciaRegistro = (string)JArray.Parse(observacion.EvidenciaCondicion)[0];
                        ////Descargar
                        //var downloadedFile = await _gestorArchivos.DownloadFTPFileAsync(item.NombreEvidenciaRegistro, userInfo.NombreUsuario, directorio);
                        ////Desencriptar
                        //var pathDesencriptado = archivePath+ item.NombreEvidenciaRegistro + "_desencriptado.zip";
                        //await _gestorEncriptado.DecryptFileAsync(downloadedFile, pathDesencriptado);
                        ////Descomprimir
                        //if (!filePath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                        //    filePath += Path.DirectorySeparatorChar;

                        //using (ZipArchive archive = ZipFile.OpenRead(pathDesencriptado))
                        //{
                        //    foreach (ZipArchiveEntry entry in archive.Entries)
                        //    {
                        //        filePath = Path.GetFullPath(Path.Combine(filePath, entry.FullName));
                        //        if (filePath.StartsWith(filePath, StringComparison.Ordinal))
                        //            entry.ExtractToFile(filePath);
                        //    }
                        //}

                    }
                    if (observacion.EvidenciaImplementacion != "[]" && observacion.EvidenciaImplementacion != null)
                    {
                        Tipo_Info_SGA directorio = Tipo_Info_SGA.FTPConnectionEvidenciaI;
                        item.NombreEvidenciaImplementacion = (string)JArray.Parse(observacion.EvidenciaImplementacion)[0];
                        //var downloadedFile = await _gestorArchivos.DownloadFTPFileAsync(item.NombreEvidenciaImplementacion, userInfo.NombreUsuario, directorio);
                    }
                    listaEvidenciasxObservacion.Add(item);
                }

                //1. Vaciar carpeta temporal del usuario 
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "Descarga Temporal Gestiona-Pruebas\\";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                    Directory.CreateDirectory(filePath);
                }
                var archiveFile = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\archive.zip";
                if (Directory.Exists(archiveFile))
                {
                    Directory.Delete(archiveFile, true);
                    Directory.CreateDirectory(archiveFile);
                }
                var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";
                if (Directory.Exists(archivePath))
                {
                    Directory.Delete(archivePath, true);
                    Directory.CreateDirectory(archivePath);
                }
                //2. Abrir conexiones SFTP para Evidencias Registro e Implementacion
                var serverInfoRegistro = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaR);
                var serverInfoImplementacion = await _info_SGA.Execute(Tipo_Info_SGA.FTPConnectionEvidenciaI);
                
                string hostRegistro = "", portRegistro = "", userRegistro = "", passRegistro = "", dirRegistro = "";
                foreach (var item in serverInfoRegistro.Info)
                {
                    switch (item.Key)
                    {
                        case "host": hostRegistro = item.Value; break;
                        case "port": portRegistro = item.Value; break;
                        case "user": userRegistro = item.Value; break;
                        case "password": passRegistro = item.Value; break;
                        case "directory": dirRegistro = item.Value; break;
                    }
                }
                /*New file downloaded name*/
                /*Copy data to new temp file*/
                if (incluirEvidencias)
                {
                    using (SftpClient sftp = new SftpClient(hostRegistro, Convert.ToInt32(portRegistro), userRegistro, passRegistro))
                    {

                        sftp.Connect();
                        sftp.ChangeDirectory(dirRegistro);

                        //Descargar archivo de sftp a nuevo archivo temporal creado
                        foreach (var item in listaEvidenciasxObservacion)
                        {
                            if (item.NombreEvidenciaRegistro != null)
                            {
                                var downloadedFilePath = archivePath + item.NombreEvidenciaRegistro;
                                var fileStream = System.IO.File.Create(downloadedFilePath);
                                sftp.DownloadFile(item.NombreEvidenciaRegistro, fileStream);
                                fileStream.Dispose();

                                //Desencriptar
                                var pathDesencriptado = archivePath + item.NombreEvidenciaRegistro + "_desencriptado.zip";
                                await _gestorEncriptado.DecryptFileAsync(downloadedFilePath, pathDesencriptado);

                                //Descomprimir
                                var pathDescomprimido = filePath;
                                if (!pathDescomprimido.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                                    pathDescomprimido += Path.DirectorySeparatorChar;

                                using (ZipArchive archive = ZipFile.OpenRead(pathDesencriptado))
                                {
                                    foreach (ZipArchiveEntry entry in archive.Entries)
                                    {
                                        pathDescomprimido = Path.GetFullPath(Path.Combine(pathDescomprimido, "EvInspReg_" + item.IdObservacion + "_" + entry.FullName));
                                        if (pathDescomprimido.StartsWith(pathDescomprimido, StringComparison.Ordinal))
                                            entry.ExtractToFile(pathDescomprimido);
                                    }
                                }
                                item.RutaEvidenciaRegistro = pathDescomprimido;
                            }
                        }

                        sftp.Dispose();
                    }
                }

                string hostImplementacion = "", portImplementacion = "", userImplementacion = "", passImplementacion = "", dirImplementacion = "";
                foreach (var item in serverInfoImplementacion.Info)
                {
                    switch (item.Key)
                    {
                        case "host": hostImplementacion = item.Value; break;
                        case "port": portImplementacion = item.Value; break;
                        case "user": userImplementacion = item.Value; break;
                        case "password": passImplementacion = item.Value; break;
                        case "directory": dirImplementacion = item.Value; break;
                    }
                }
                if (incluirEvidencias)
                {
                    using (SftpClient sftp = new SftpClient(hostImplementacion, Convert.ToInt32(portImplementacion), userImplementacion, passImplementacion))
                    {

                        sftp.Connect();
                        sftp.ChangeDirectory(dirImplementacion);

                        //Descargar archivo de sftp a nuevo archivo temporal creado
                        foreach (var item in listaEvidenciasxObservacion)
                        {
                            if (item.NombreEvidenciaImplementacion != null)
                            {
                                var downloadedFilePath = archivePath + item.NombreEvidenciaImplementacion;
                                var fileStream = System.IO.File.Create(downloadedFilePath);
                                sftp.DownloadFile(item.NombreEvidenciaImplementacion, fileStream);
                                fileStream.Dispose();

                                //Desencriptar
                                var pathDesencriptado = archivePath + item.NombreEvidenciaImplementacion + "_desencriptado.zip";
                                await _gestorEncriptado.DecryptFileAsync(downloadedFilePath, pathDesencriptado);

                                //Descomprimir
                                var pathDescomprimido = filePath;
                                if (!pathDescomprimido.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                                    pathDescomprimido += Path.DirectorySeparatorChar;

                                using (ZipArchive archive = ZipFile.OpenRead(pathDesencriptado))
                                {
                                    foreach (ZipArchiveEntry entry in archive.Entries)
                                    {
                                        pathDescomprimido = Path.GetFullPath(Path.Combine(pathDescomprimido, "EvInspImpl_" + item.IdObservacion + "_" + entry.FullName));
                                        if (pathDescomprimido.StartsWith(pathDescomprimido, StringComparison.Ordinal))
                                            entry.ExtractToFile(pathDescomprimido);
                                    }
                                }
                                item.RutaEvidenciaImplementacion = pathDescomprimido;
                            }
                        }

                        sftp.Dispose();
                    }
                }


                foreach (var observacion in observaciones)
                {
                    short rowHeight = 850;
                    IRow row = sheet.CreateRow(fila);

                    //Aplicar cell style a una fila de observación
                    for (int i = 1; i <= 24; i++)
                    {
                        row.CreateCell(i);
                        row.GetCell(i).CellStyle = estiloNormal;
                    }
                    if (incluirEvidencias)
                    {

                        ImageProperties evidencia_condicion = new ImageProperties();
                        ImageProperties evidencia_inspeccion = new ImageProperties();

                        //adaptar el tamaño de la fila al mayor determinado por la imagen
                        row.Height = rowHeight;

                        if (observacion.EvidenciaCondicion != "[]" && observacion.EvidenciaCondicion != null)
                        {

                            //evidencia_condicion = await AddImageToCellAsync(templateWorkbook, sheet, observacion.EvidenciaCondicion, fila, 4, rowHeight, imageIndex, "condicion", true);
                            evidencia_condicion = await AddImageToCellAsyncWithoutDownload(templateWorkbook, sheet, listaEvidenciasxObservacion.Find(x=>x.IdObservacion.Equals(observacion.IdObservacion)).RutaEvidenciaRegistro, fila, 4, rowHeight, imageIndex, "condicion", true);
                            if (evidencia_condicion != null)
                            {
                                rowHeight = rowHeight > evidencia_condicion.rowHeight ? rowHeight : evidencia_condicion.rowHeight;
                                imageIndex++;
                            }
                        }
                        if (observacion.EvidenciaImplementacion != "[]" && observacion.EvidenciaImplementacion != null)
                        {
                            //evidencia_inspeccion = await AddImageToCellAsync(templateWorkbook, sheet, observacion.EvidenciaImplementacion, fila, 11, rowHeight, imageIndex, "inspeccion", true);
                            evidencia_inspeccion = await AddImageToCellAsyncWithoutDownload(templateWorkbook, sheet, listaEvidenciasxObservacion.Find(x => x.IdObservacion.Equals(observacion.IdObservacion)).RutaEvidenciaImplementacion, fila, 11, rowHeight, imageIndex, "inspeccion", true);
                            if (evidencia_inspeccion != null)
                            {
                                rowHeight = rowHeight > evidencia_inspeccion.rowHeight ? rowHeight : evidencia_inspeccion.rowHeight;
                                imageIndex++;
                            }
                        }
                        //adaptar el tamaño de la fila al mayor determinado por la imagen
                        row.Height = rowHeight;
                        //rezise images
                        if (evidencia_condicion != null)
                        {
                            if ((evidencia_condicion.extension == ".jpg" ||
                                evidencia_condicion.extension == ".png" ||
                                evidencia_condicion.extension == ".jpeg" ||
                                evidencia_condicion.extension == ".JPG" ||
                                evidencia_condicion.extension == ".PNG" ||
                                evidencia_condicion.extension == ".JPEG" &&
                                evidencia_condicion.extension != null) &&
                                evidencia_condicion.rowHeight != 0 &&
                                evidencia_condicion.rowHeight < row.Height)
                            {
                                //evidencia_condicion.image.Resize(1.0, (double)evidencia_condicion.rowHeight / row.Height);
                                evidencia_condicion.image.Resize(0.5, 1.0);
                            }
                        }
                        if (evidencia_inspeccion != null)
                        {
                            if ((evidencia_inspeccion.extension == ".jpg" ||
                                evidencia_inspeccion.extension == ".png" ||
                                evidencia_inspeccion.extension == ".jpeg" ||
                                evidencia_inspeccion.extension == ".JPG" ||
                                evidencia_inspeccion.extension == ".PNG" ||
                                evidencia_inspeccion.extension == ".JPEG") &&
                                evidencia_inspeccion.extension != null &&
                                evidencia_inspeccion.rowHeight != 0 &&
                                evidencia_inspeccion.rowHeight < row.Height)
                            {
                                //evidencia_inspeccion.image.Resize(1.0, (double)evidencia_inspeccion.rowHeight / row.Height);
                                evidencia_inspeccion.image.Resize(0.5, 1.0);
                            }
                        }
                    }
                    int startIndex = 1;
                    //->Index
                    row.GetCell(startIndex).SetCellValue(obsIndex);
                    //->SSO/MA
                    row.GetCell(startIndex + 1).SetCellValue(observacion.SSO_MA);
                    //->Acto/Condición Subestándar Identificada
                    row.GetCell(startIndex + 2).SetCellValue(observacion.ActoCondicion);
                    //->Descripción de Condición / Acto Subestándar
                    row.GetCell(startIndex + 4).SetCellValue(observacion.DescripcionActoCondicion);
                    //->Tipo de Condición / Acto Subestándar
                    row.GetCell(startIndex + 5).SetCellValue(observacion.TipoCondicion);
                    //->Nivel de Riesgo
                    row.GetCell(startIndex + 6).SetCellValue(observacion.NivelRiesgo);
                    //->Acción Preventiva / Correctiva Tomada
                    row.GetCell(startIndex + 7).SetCellValue(observacion.Accion);
                    //->Responsable del Levanamiento del Acto / Condición Subestándar
                    row.GetCell(startIndex + 8).SetCellValue(observacion.ResponsableLevantamiento);
                    //->Fecha Programada
                    row.GetCell(startIndex + 9).SetCellValue(observacion.FechaProgramada);
                    //->Fecha Cierre
                    row.GetCell(startIndex + 11).SetCellValue(observacion.FechaCierre);
                    //-> Estado
                    row.GetCell(startIndex + 12).SetCellValue(observacion.Estado);
                    var estilo = estilos.FirstOrDefault(x => x.Estado.Equals(observacion.Estado));
                    if (estilo != null)
                        row.GetCell(startIndex + 12).CellStyle = estilo.EstiloCelda;
                    //-> Lider de la inspección
                    row.GetCell(startIndex + 13).SetCellValue(observacion.ResponsableInspeccion);
                    //-> Fecha Inspección
                    row.GetCell(startIndex + 14).SetCellValue(observacion.FechaInspeccion);
                    //-> Área Inspeccionada
                    row.GetCell(startIndex + 15).SetCellValue(observacion.AreaInspeccionada);
                    //-> Riesgo de Fatalidad
                    row.GetCell(startIndex + 16).SetCellValue(observacion.RiesgoFatalidad);
                    //-> Gerencia Responsable Levantamiento
                    row.GetCell(startIndex + 17).SetCellValue(observacion.GerenciaLevantamiento);
                    //-> Gerencia de la inspección
                    row.GetCell(startIndex + 18).SetCellValue(observacion.GerenciaInspeccion);
                    //-> Empresa Inspeccionada
                    row.GetCell(startIndex + 19).SetCellValue(observacion.EmpresaInspeccionada);
                    //-> Participantes de la inspección
                    row.GetCell(startIndex + 20).SetCellValue(observacion.ParticipantesInspeccion);
                    //-> Asesor
                    row.GetCell(startIndex + 21).SetCellValue(observacion.Asesor);
                    //-> Tipo de Inspección
                    row.GetCell(startIndex + 22).SetCellValue(observacion.TipoInspeccion);
                    //-> N° Inspección GESTIONA
                    row.GetCell(startIndex + 23).SetCellValue(observacion.Codigo);
                    XSSFHyperlink UrlLinkLugReg = new XSSFHyperlink(HyperlinkType.Url)
                    {
                        //Address = "https://gestiona.marcobre.com:9190/Inspecciones/DetalleInspeccion?IdCabecera="+(_protector.Protect(observacion.IdInspeccion))
                        Address = "https://gestiona.marcobre.com:9190/Inspecciones/DetalleInspeccionNotificacion?IdCabecera=" + observacion.IdInspeccion
                    };
                    row.GetCell(startIndex + 23).Hyperlink = (UrlLinkLugReg);

                    obsIndex++;
                    fila++;
                }
                // Force formulas to update with new data we added
                sheet.ForceFormulaRecalculation = true;
                // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
                MemoryStream ms = new MemoryStream();
                templateWorkbook.Write(ms);

                // Send the memory stream to the browser
                ms.Position = 0;

                /*
                //VACIAR CARPETA TEMPORAL
                var tempPath = Path.GetTempPath();
                var SGATempPath = tempPath + "Descarga Temporal Gestiona-Pruebas\\";
                if (!Directory.Exists(SGATempPath))
                {
                    Directory.CreateDirectory(SGATempPath);
                }
                var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";

                if (Directory.Exists(filePath))
                {
                    Directory.Delete(filePath, true);
                }
                //Directory.CreateDirectory(filePath);
                */
                return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteObservaciones-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
            }
        }
        private double GetRowHeight(int image_width, int image_height, double cellWidth)
        {
            //double cellWidth = 7.16;//cm
            return ((image_height * cellWidth) / image_width) / 3;
        }
        //Retorna el tamaño de la fila para la imagen agregada
        private async Task<ImageProperties> AddImageToCellAsync(XSSFWorkbook templateWorkbook, ISheet sheet, string path, int row, int column, short rowHeight, int imageIndex, string tipoEvidencia, bool reporte)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var output = new ImageProperties();
            var evidenciasCondicion = JArray.Parse(path);
            var evidencia = evidenciasCondicion[0];
            //foreach (var evidencia in evidenciasCondicion)
            //{
            var imgNCol = 0;

            //1. Vaciar carpeta temporal del usuario 
            var tempPath = Path.GetTempPath();
            var SGATempPath = tempPath + "Descarga Temporal Gestiona-Pruebas\\";
            if (!Directory.Exists(SGATempPath))
            {
                Directory.CreateDirectory(SGATempPath);
            }
            var filePath = SGATempPath + "submission\\" + userInfo.NombreUsuario + "\\";
            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath, true);
                Directory.CreateDirectory(filePath);
            }
            var archiveFile = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\archive.zip";
            if (Directory.Exists(archiveFile))
            {
                Directory.Delete(archiveFile, true);
                Directory.CreateDirectory(archiveFile);
            }
            var archivePath = SGATempPath + "zip\\" + userInfo.NombreUsuario + "\\";
            if (Directory.Exists(archivePath))
            {
                Directory.Delete(archivePath, true);
                Directory.CreateDirectory(archivePath);
            }

            //2. Descargar .zip de ftp y guardar en carpeta temporal zip
            Tipo_Info_SGA directorio;
            if (tipoEvidencia == "condicion")
            {
                directorio = Tipo_Info_SGA.FTPConnectionEvidenciaR;
                if (!reporte)
                    imgNCol = 2;
            }
            else
            {
                directorio = Tipo_Info_SGA.FTPConnectionEvidenciaI;
                if (!reporte)
                    imgNCol = 4;
            }
            var downloadedFile = await _gestorArchivos.DownloadFTPFileAsync(evidencia.ToString(), userInfo.NombreUsuario, directorio);
            //3. Desencriptar archivo
            var pathDesencriptado = archivePath + "desencriptado" + imageIndex + ".zip";
            await _gestorEncriptado.DecryptFileAsync(downloadedFile, pathDesencriptado);
            // fileStream.Dispose();
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
            if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".png") || extension.ToLower().Equals(".jpeg"))
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
            //}
            //6. Eliminar pathtemporal
            // if (Directory.Exists(archivePath))
            // {
            //     Directory.Delete(archivePath, true);
            // }
            // Directory.CreateDirectory(archivePath);
            return output;
        }
        private async Task<ImageProperties> AddImageToCellAsyncWithoutDownload(XSSFWorkbook templateWorkbook, ISheet sheet, string filePath, int row, int column, short rowHeight, int imageIndex, string tipoEvidencia, bool reporte)
        {
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));
            var output = new ImageProperties();

            var imgNCol = 0;
            if (tipoEvidencia == "condicion" )
            {
                if (!reporte)
                    imgNCol = 2;
            }
            else
            {
                if (!reporte)
                    imgNCol = 4;
            }
            //5. validar extension y cargar en una celda
            var extension = Path.GetExtension(filePath);
            if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".png") || extension.ToLower().Equals(".jpeg"))
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
        private class ObservacionReporte
        {
            public string IdInspeccion { get; set; }
            public int IdObservacion { get; set; }
            public string SSO_MA { get; set; }
            public string DescripcionActoCondicion { get; set; }
            public string ActoCondicion { get; set; }
            public string EvidenciaCondicion { get; set; }
            public string TipoCondicion { get; set; }
            public string NivelRiesgo { get; set; }
            public string Accion { get; set; }
            public string ResponsableLevantamiento { get; set; }
            public string FechaProgramada { get; set; }
            public string EvidenciaImplementacion { get; set; }
            public string FechaCierre { get; set; }
            public string Estado { get; set; }
            public XSSFCellStyle EstadoEstilo { get; set; }
            public string ResponsableInspeccion { get; set; }
            public string FechaInspeccion { get; set; }
            public string RiesgoFatalidad { get; set; }
            public string GerenciaLevantamiento { get; set; }
            public string GerenciaInspeccion { get; set; }
            public string EmpresaInspeccionada { get; set; }
            public string ParticipantesInspeccion { get; set; }
            public string Asesor { get; set; }
            public string Codigo { get; set; }
            public string AreaInspeccionada { get; set; }
            public string TipoInspeccion { get; set; }

        }
        private class Estilo
        {
            public string Estado { get; set; }
            public XSSFCellStyle EstiloCelda { get; set; }
            public XSSFFont Fuente { get; set; }
            public Estilo()
            {

            }
            public Estilo(string estado, XSSFWorkbook libro, byte r, byte g, byte b, short colorTexto)
            {
                Estado = estado;
                EstiloCelda = (XSSFCellStyle)libro.CreateCellStyle();
                EstiloCelda.WrapText = true;
                EstiloCelda.VerticalAlignment = VerticalAlignment.Center;
                EstiloCelda.BorderTop = BorderStyle.Thin;
                EstiloCelda.BorderBottom = BorderStyle.Thin;
                EstiloCelda.BorderLeft = BorderStyle.Thin;
                EstiloCelda.BorderRight = BorderStyle.Thin;
                EstiloCelda.Alignment = HorizontalAlignment.Center;
                EstiloCelda.SetFillForegroundColor(new NPOI.XSSF.UserModel.XSSFColor(new byte[] { r, g, b }));
                EstiloCelda.FillPattern = FillPattern.SolidForeground;

                Fuente = (XSSFFont)libro.CreateFont();
                Fuente.Color = colorTexto;

                EstiloCelda.SetFont(Fuente);
            }
        }
        public class FileOutput
        {
            public string FilePath { get; set; }
            public bool Success { get; set; }
            public string ErroMessage { get; set; }
        }
        public class Evidencia
        {
            public int IdObservacion { get; set; }
            public string NombreEvidenciaRegistro { get; set; }
            public string RutaEvidenciaRegistro { get; set; }
            public string NombreEvidenciaImplementacion { get; set; }
            public string RutaEvidenciaImplementacion { get; set; }
        }
    }
}