using GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Inspecciones;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.RAC;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.DashboardGeneral
{
    public class DescargarRegistros
    {
        private readonly IObtenerRegistrosInspecciones _obtenerRegistrosInspecciones;
        private readonly IObtenerRegistrosRAC _obtenerRegistrosRAC;

        public DescargarRegistros(IObtenerRegistrosInspecciones obtenerRegistrosInspecciones, IObtenerRegistrosRAC obtenerRegistrosRAC)
        {
            _obtenerRegistrosInspecciones = obtenerRegistrosInspecciones;
            _obtenerRegistrosRAC = obtenerRegistrosRAC;
        }

        private XSSFCellStyle GetDefaultCellStyle(XSSFWorkbook templateWorkbook)
        {
            XSSFCellStyle customCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();
            customCellStyle.WrapText = true;
            customCellStyle.VerticalAlignment = VerticalAlignment.Center;
            customCellStyle.BorderTop = BorderStyle.Thin;
            customCellStyle.BorderBottom = BorderStyle.Thin;
            customCellStyle.BorderLeft = BorderStyle.Thin;
            customCellStyle.BorderRight = BorderStyle.Thin;
            customCellStyle.Alignment = HorizontalAlignment.Center;
            return customCellStyle;
        }
        public async Task<MemoryStream> DescargarInformacion(string sTime, string eTime, string templatePath)
        {

            string sWebRootFolder = templatePath;
            string sFileName = @"Registros Template.xlsx";
            string sFilePath = Path.Combine(sWebRootFolder, sFileName);
            FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

            // Load the template into a NPOI workbook
            XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);
            ISheet sheetObservaciones = await DescargarObservaciones(sTime, eTime, templateWorkbook, "Observaciones - Inspecciones");
            ISheet sheetRAC = await DescargarRACs(sTime, eTime, templateWorkbook, "RAC");
            
            MemoryStream ms = new MemoryStream();
            templateWorkbook.Write(ms);

            return ms;
        }
        public async Task<ISheet> DescargarObservaciones(string sTime, string eTime, XSSFWorkbook templateWorkbook, string sheetName)
        {            
            // Load the sheet you are going to use as a template into NPOI
            ISheet sheetObservaciones = templateWorkbook.GetSheet(sheetName);
            var nColumns = sheetObservaciones.GetRow(0).Cells.Count();

            XSSFCellStyle customCellStyle = GetDefaultCellStyle(templateWorkbook);

            var observaciones = await _obtenerRegistrosInspecciones.Execute(sTime, eTime);
            for (int i = 0; i < observaciones.Count; i++)
            {
                IRow row = sheetObservaciones.CreateRow(i+1);

                row.CreateCell(0).SetCellValue(observaciones[i].Cabecera.Codigo);
                row.GetCell(0).CellStyle = customCellStyle;

                row.CreateCell(1).SetCellValue(observaciones[i].Cabecera.FechaInspeccion.ToString("dd/MM/yyyy"));
                row.GetCell(1).CellStyle = customCellStyle;

                row.CreateCell(2).SetCellValue(observaciones[i].Cabecera.AreaInspeccionada);
                row.GetCell(2).CellStyle = customCellStyle;
                //Gerencia Inspeccionada
                row.CreateCell(3).SetCellValue(observaciones[i].Cabecera.GerenciaInspeccionada);
                row.GetCell(3).CellStyle = customCellStyle;
                //Gerencia Responsable
                row.CreateCell(4).SetCellValue(observaciones[i].Cabecera.Gerencia);
                row.GetCell(4).CellStyle = customCellStyle;
                //Tipo de Observación
                row.CreateCell(5).SetCellValue(observaciones[i].TipoObservacion);
                row.GetCell(5).CellStyle = customCellStyle;
                //Asesor
                row.CreateCell(6).SetCellValue(observaciones[i].Asesor.Nombres+" "+observaciones[i].Asesor.Apellidos);
                row.GetCell(6).CellStyle = customCellStyle;
                //Tipo de Acto/Condición Subestándar
                row.CreateCell(7).SetCellValue(observaciones[i].DescripcionCondicion);
                row.GetCell(7).CellStyle = customCellStyle;
                //Tipo de Condición
                row.CreateCell(8).SetCellValue(observaciones[i].TipoCondicion!=null? observaciones[i].TipoCondicion.Descripcion:"");
                row.GetCell(8).CellStyle = customCellStyle;
                //Descripción de Acto/Condición Subestándar
                row.CreateCell(9).SetCellValue(observaciones[i].DescripcionActoCondicion);
                row.GetCell(9).CellStyle = customCellStyle;
                //Nivel de Riesgo
                row.CreateCell(10).SetCellValue(observaciones[i].NivelRiesgo);
                row.GetCell(10).CellStyle = customCellStyle;
                //Descripción de Acción
                row.CreateCell(11).SetCellValue(observaciones[i].DescripcionAccion);
                row.GetCell(11).CellStyle = customCellStyle;
                //Responsable Marcobre
                row.CreateCell(12).SetCellValue(observaciones[i].ResponsableMarcobre.Nombres+" "+ observaciones[i].ResponsableMarcobre.Apellidos);
                row.GetCell(12).CellStyle = customCellStyle;
                //Responsable Contratista
                row.CreateCell(13).SetCellValue(observaciones[i].ResponsableContratista);
                row.GetCell(13).CellStyle = customCellStyle;
                //Correo Contratista
                row.CreateCell(14).SetCellValue(observaciones[i].CorreoContratista);
                row.GetCell(14).CellStyle = customCellStyle;
                //Fecha Límite de Implementación
                row.CreateCell(15).SetCellValue(observaciones[i].FechaProgramada!=null?observaciones[i].FechaProgramada.Value.ToString("dd/MM/yyyy"):"");
                row.GetCell(15).CellStyle = customCellStyle;
                //Comentario Implementacion
                row.CreateCell(16).SetCellValue(observaciones[i].ComentarioImplementacion);
                row.GetCell(16).CellStyle = customCellStyle;
                //Fecha de Implementación
                row.CreateCell(17).SetCellValue(observaciones[i].FechaCierre != null ? observaciones[i].FechaCierre.Value.ToString("dd/MM/yyyy") : "");
                row.GetCell(17).CellStyle = customCellStyle;
                //Fecha de Verificación
                row.CreateCell(18).SetCellValue(observaciones[i].FechaVerificacion != null ? observaciones[i].FechaVerificacion.Value.ToString("dd/MM/yyyy") : "");
                row.GetCell(18).CellStyle = customCellStyle;
                //Comentario Verificacion
                row.CreateCell(19).SetCellValue(observaciones[i].ComentarioVerificacion);
                row.GetCell(19).CellStyle = customCellStyle;
                //Estado
                row.CreateCell(20).SetCellValue(observaciones[i].Estado);
                row.GetCell(20).CellStyle = customCellStyle;
            }
            return sheetObservaciones;
        }
        public async Task<ISheet> DescargarRACs(string sTime, string eTime, XSSFWorkbook templateWorkbook, string sheetName)
        {            
            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet(sheetName);
            var nColumns = sheet.GetRow(0).Cells.Count();

            XSSFCellStyle customCellStyle = GetDefaultCellStyle(templateWorkbook);

            var racs = await _obtenerRegistrosRAC.Execute(sTime, eTime);
            for (int i = 0; i < racs.Count; i++)
            {
                IRow row = sheet.CreateRow(i+1);
                //Codigo
                row.CreateCell(0).SetCellValue(racs[i].Codigo);
                row.GetCell(0).CellStyle = customCellStyle;
                //Reportante
                row.CreateCell(1).SetCellValue(racs[i].Nombres_Apellidos);
                row.GetCell(1).CellStyle = customCellStyle;
                //Gerencia
                row.CreateCell(2).SetCellValue(racs[i].Gerencia.Descripcion);
                row.GetCell(2).CellStyle = customCellStyle;
                //Empresa
                row.CreateCell(3).SetCellValue(racs[i].Empresa.RazonSocial);
                row.GetCell(3).CellStyle = customCellStyle;
                //Otra Empresa
                row.CreateCell(4).SetCellValue(racs[i].Otra_Empresa);
                row.GetCell(4).CellStyle = customCellStyle;
                //Empresa Observada
                row.CreateCell(5).SetCellValue(racs[i].Empresa_Ob.RazonSocial);
                row.GetCell(5).CellStyle = customCellStyle;
                //Gerencia Observada
                row.CreateCell(6).SetCellValue(racs[i].Gerencia_Obs.Descripcion);
                row.GetCell(6).CellStyle = customCellStyle;
                //Ubicacion
                row.CreateCell(7).SetCellValue(racs[i].Ubicacion);
                row.GetCell(7).CellStyle = customCellStyle;
                //Fecha
                row.CreateCell(8).SetCellValue(racs[i].Fecha.ToString("dd/MM/yyyy"));
                row.GetCell(8).CellStyle = customCellStyle;
                //SSO/MA
                row.CreateCell(9).SetCellValue(racs[i].SSO_MA);
                row.GetCell(9).CellStyle = customCellStyle;
                //Clasificacion
                row.CreateCell(10).SetCellValue(racs[i].Clasificacion);
                row.GetCell(10).CellStyle = customCellStyle;
                //Nivel de Riesgo
                row.CreateCell(11).SetCellValue(racs[i].NivelRiesgo);
                row.GetCell(11).CellStyle = customCellStyle;
                //Tipo Condicion
                row.CreateCell(12).SetCellValue(racs[i].TipoCondicion.Descripcion);
                row.GetCell(12).CellStyle = customCellStyle;
                //Descripcion acto condicion
                row.CreateCell(13).SetCellValue(racs[i].Descripcion_Acto_Cond);
                row.GetCell(13).CellStyle = customCellStyle;
                //Medida Control
                row.CreateCell(14).SetCellValue(racs[i].Medida_Control);
                row.GetCell(14).CellStyle = customCellStyle;
                //PAC
                row.CreateCell(15).SetCellValue(racs[i].PAC);
                row.GetCell(15).CellStyle = customCellStyle;
                //Responsable Cierre
                row.CreateCell(16).SetCellValue(racs[i].Responsable_Cierre.Nombres + " " + racs[i].Responsable_Cierre.Apellidos);
                row.GetCell(16).CellStyle = customCellStyle;
                //Fecha Cierre
                row.CreateCell(17).SetCellValue(racs[i].Fecha_Cierre != null ? racs[i].Fecha_Cierre.ToString("dd/MM/yyyy") : "");
                row.GetCell(17).CellStyle = customCellStyle;
                //Superisor
                row.CreateCell(18).SetCellValue(racs[i].Supervisor.Nombres+" "+ racs[i].Supervisor.Apellidos);
                row.GetCell(18).CellStyle = customCellStyle;
                //Comentario Cierre
                row.CreateCell(19).SetCellValue(racs[i].Comentario_Cierre);
                row.GetCell(19).CellStyle = customCellStyle;
                //Comentario Verificacion
                row.CreateCell(20).SetCellValue(racs[i].Comentario_Verificacion);
                row.GetCell(20).CellStyle = customCellStyle;
                //Estado
                row.CreateCell(21).SetCellValue(racs[i].Estado);
                row.GetCell(21).CellStyle = customCellStyle;
            }
            return sheet;
        }
    }
}
