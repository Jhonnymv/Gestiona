using GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.AvistamientoFauna;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.InvestigacionFinal.Acciones;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.InvestigacionFinal.Causas;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.InvestigacionFinal.ProductoDerramado;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.ReportesPreliminares.Acciones;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.ReportesPreliminares.Causas;
using GestionAmbiental.Application.UseCases.DashboardGeneral.ObtenerRegistros.Incidentes.ReportesPreliminares.ProductoDerramado;
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
        private readonly IObtenerAccionesRP _obtenerAccionesRP;
        private readonly IObtenerCausasRP _obtenerCausasRP;
        private readonly IObtenerAccionesInvestigacion _obtenerAccionesInvestigacion;
        private readonly IObtenerCausasInvestigacion _obtenerCausasInvestigacion;
        private readonly IProductoDerramadoRP _productoDerramadoRP;
        private readonly IProductoDerramadoInvestigacion _productoDerramadoInvestigacion;
        private readonly IObtenerAvistamientos _obtenerAvistamientos;

        public DescargarRegistros(
            IObtenerRegistrosInspecciones obtenerRegistrosInspecciones, 
            IObtenerRegistrosRAC obtenerRegistrosRAC, IObtenerAccionesRP obtenerAccionesRP,
            IObtenerCausasRP obtenerCausasRP,
            IObtenerAccionesInvestigacion obtenerAccionesInvestigacion,
            IObtenerCausasInvestigacion obtenerCausasInvestigacion, 
            IProductoDerramadoRP productoDerramadoRP,
            IProductoDerramadoInvestigacion productoDerramadoInvestigacion,
            IObtenerAvistamientos obtenerAvistamientos)
        {
            _obtenerRegistrosInspecciones = obtenerRegistrosInspecciones;
            _obtenerRegistrosRAC = obtenerRegistrosRAC;
            _obtenerAccionesRP = obtenerAccionesRP;
            _obtenerCausasRP = obtenerCausasRP;
            _obtenerAccionesInvestigacion = obtenerAccionesInvestigacion;
            _obtenerCausasInvestigacion = obtenerCausasInvestigacion;
            _productoDerramadoRP = productoDerramadoRP;
            _productoDerramadoInvestigacion = productoDerramadoInvestigacion;
            _obtenerAvistamientos = obtenerAvistamientos;
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
            ISheet sheetAccionesRP = await DescargarAccionesRP(sTime, eTime, templateWorkbook, "Acciones Rep Preli - Incidentes");
            ISheet sheetCausasRP = await DescargarCausasRP(sTime, eTime, templateWorkbook, "Causas Reporte Prelim - Inciden");
            ISheet sheetAccionesInvestigacion = await DescargarAccionesInvestigacion(sTime, eTime, templateWorkbook, "Acciones Invest- Incidentes");
            ISheet sheetCausasInvestigacion = await DescargarCausasInvestigacion(sTime, eTime, templateWorkbook, "Causas Invest - Incidente");
            ISheet sheetProductosRP = await DescargarProductoDerramadoRP(sTime, eTime, templateWorkbook, "Producto Derramado - RP Inc ");
            ISheet sheetProductosInvestigacion = await DescargarProductoDerramadoInvestigacion(sTime, eTime, templateWorkbook, "Producto Derramado - Incidente");
            ISheet sheetAvistamientos = await DescargarAvistamientos(sTime, eTime, templateWorkbook, "Avistamiento de Fauna");

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
                IRow row = sheetObservaciones.CreateRow(i + 1);

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
                row.CreateCell(6).SetCellValue(observaciones[i].Asesor.Nombres + " " + observaciones[i].Asesor.Apellidos);
                row.GetCell(6).CellStyle = customCellStyle;
                //Tipo de Acto/Condición Subestándar
                row.CreateCell(7).SetCellValue(observaciones[i].DescripcionCondicion);
                row.GetCell(7).CellStyle = customCellStyle;
                //Tipo de Condición
                row.CreateCell(8).SetCellValue(observaciones[i].TipoCondicion != null ? observaciones[i].TipoCondicion.Descripcion : "");
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
                row.CreateCell(12).SetCellValue(observaciones[i].ResponsableMarcobre.Nombres + " " + observaciones[i].ResponsableMarcobre.Apellidos);
                row.GetCell(12).CellStyle = customCellStyle;
                //Responsable Contratista
                row.CreateCell(13).SetCellValue(observaciones[i].ResponsableContratista);
                row.GetCell(13).CellStyle = customCellStyle;
                //Correo Contratista
                row.CreateCell(14).SetCellValue(observaciones[i].CorreoContratista);
                row.GetCell(14).CellStyle = customCellStyle;
                //Fecha Límite de Implementación
                row.CreateCell(15).SetCellValue(observaciones[i].FechaProgramada != null ? observaciones[i].FechaProgramada.Value.ToString("dd/MM/yyyy") : "");
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
                IRow row = sheet.CreateRow(i + 1);
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
                row.CreateCell(18).SetCellValue(racs[i].Supervisor.Nombres + " " + racs[i].Supervisor.Apellidos);
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
        public async Task<ISheet> DescargarAccionesRP(string sTime, string eTime, XSSFWorkbook templateWorkbook, string sheetName)
        {
            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet(sheetName);
            var nColumns = sheet.GetRow(0).Cells.Count();

            XSSFCellStyle customCellStyle = GetDefaultCellStyle(templateWorkbook);

            var acciones = await _obtenerAccionesRP.Execute(sTime, eTime);
            for (int i = 0; i < acciones.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                //Codigo
                row.CreateCell(0).SetCellValue(acciones[i].Reporte.Codigo);
                row.GetCell(0).CellStyle = customCellStyle;
                //Fecha_Incidente
                row.CreateCell(1).SetCellValue(acciones[i].Reporte.Fecha_Incidente.Value.ToString("dd/MM/yyyy"));
                row.GetCell(1).CellStyle = customCellStyle;
                //Razon_Social_Contratista
                row.CreateCell(2).SetCellValue(acciones[i].Reporte.Razon_Social_Contratista);
                row.GetCell(2).CellStyle = customCellStyle;
                //Actividad_Contratista
                row.CreateCell(3).SetCellValue(acciones[i].Reporte.Actividad_Contratista);
                row.GetCell(3).CellStyle = customCellStyle;
                //Accion_Inmediata
                row.CreateCell(4).SetCellValue(acciones[i].Accion_Inmediata);
                row.GetCell(4).CellStyle = customCellStyle;
                //Gerencia_Responsable
                row.CreateCell(5).SetCellValue(acciones[i].Reporte.Gerencia_Responsable.Descripcion);
                row.GetCell(5).CellStyle = customCellStyle;
                //ResponsableMarcobre
                row.CreateCell(6).SetCellValue(acciones[i].ResponsableMarcobre.Nombres + " " + acciones[i].ResponsableMarcobre.Apellidos);
                row.GetCell(6).CellStyle = customCellStyle;
                //Responsable Contratista
                row.CreateCell(7).SetCellValue(acciones[i].Responsable);
                row.GetCell(7).CellStyle = customCellStyle;
                //Fecha
                row.CreateCell(8).SetCellValue(acciones[i].Fecha.Value.ToString("dd/MM/yyyy"));
                row.GetCell(8).CellStyle = customCellStyle;
                //Estatus
                row.CreateCell(9).SetCellValue(acciones[i].Estatus);
                row.GetCell(9).CellStyle = customCellStyle;
                //Estado
                row.CreateCell(10).SetCellValue(acciones[i].Estado);
                row.GetCell(10).CellStyle = customCellStyle;
            }
            return sheet;
        }
        public async Task<ISheet> DescargarCausasRP(string sTime, string eTime, XSSFWorkbook templateWorkbook, string sheetName)
        {
            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet(sheetName);
            var nColumns = sheet.GetRow(0).Cells.Count();

            XSSFCellStyle customCellStyle = GetDefaultCellStyle(templateWorkbook);

            var causas = await _obtenerCausasRP.Execute(sTime, eTime);
            for (int i = 0; i < causas.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                //Codigo
                row.CreateCell(0).SetCellValue(causas[i].Reporte.Codigo);
                row.GetCell(0).CellStyle = customCellStyle;
                //Fecha_Incidente
                row.CreateCell(1).SetCellValue(causas[i].Reporte.Fecha_Incidente.Value.ToString("dd/MM/yyyy"));
                row.GetCell(1).CellStyle = customCellStyle;
                //Tipo Condicion
                row.CreateCell(2).SetCellValue(causas[i].TipoCondicion.Descripcion);
                row.GetCell(2).CellStyle = customCellStyle;
                //Tipo_Acto_Condicion
                row.CreateCell(3).SetCellValue(causas[i].Tipo_Acto_Condicion);
                row.GetCell(3).CellStyle = customCellStyle;
                //Estado
                row.CreateCell(4).SetCellValue(causas[i].Estado);
                row.GetCell(4).CellStyle = customCellStyle;
            }
            return sheet;
        }
        public async Task<ISheet> DescargarAccionesInvestigacion(string sTime, string eTime, XSSFWorkbook templateWorkbook, string sheetName)
        {
            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet(sheetName);
            var nColumns = sheet.GetRow(0).Cells.Count();

            XSSFCellStyle customCellStyle = GetDefaultCellStyle(templateWorkbook);

            var acciones = await _obtenerAccionesInvestigacion.Execute(sTime, eTime);
            for (int i = 0; i < acciones.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                //Codigo
                row.CreateCell(0).SetCellValue(acciones[i].InvestigacionFinal.Codigo);
                row.GetCell(0).CellStyle = customCellStyle;
                //Fecha_Incidente
                row.CreateCell(1).SetCellValue(acciones[i].InvestigacionFinal.Fecha_Incidente.Value.ToString("dd/MM/yyyy"));
                row.GetCell(1).CellStyle = customCellStyle;
                //Razon_Social_Contratista
                row.CreateCell(2).SetCellValue(acciones[i].InvestigacionFinal.Razon_Social_Contratista);
                row.GetCell(2).CellStyle = customCellStyle;
                //Actividad_Contratista
                row.CreateCell(3).SetCellValue(acciones[i].InvestigacionFinal.Actividad_Contratista);
                row.GetCell(3).CellStyle = customCellStyle;
                //Accion_Inmediata
                row.CreateCell(4).SetCellValue(acciones[i].InvestigacionFinal.Razon_Social_Subcontratista);
                row.GetCell(4).CellStyle = customCellStyle;
                //Gerencia_Responsable
                row.CreateCell(5).SetCellValue(acciones[i].InvestigacionFinal.Actividad_Subcontratista);
                row.GetCell(5).CellStyle = customCellStyle;
                //ResponsableMarcobre
                row.CreateCell(6).SetCellValue(acciones[i].Tipo_Accion);
                row.GetCell(6).CellStyle = customCellStyle;
                //Responsable Contratista
                row.CreateCell(7).SetCellValue(acciones[i].InvestigacionFinal.Gerencia_Responsable.Descripcion);
                row.GetCell(7).CellStyle = customCellStyle;
                //Fecha
                row.CreateCell(8).SetCellValue(acciones[i].Accion_Inmediata);
                row.GetCell(8).CellStyle = customCellStyle;
                //Estatus
                row.CreateCell(9).SetCellValue(acciones[i].Usuario != null ? acciones[i].Usuario.Nombres + " " + acciones[i].Usuario.Apellidos : "");
                row.GetCell(9).CellStyle = customCellStyle;
                //Responsable Marcobre 2
                row.CreateCell(10).SetCellValue(acciones[i].Usuario2 != null ? acciones[i].Usuario2.Nombres + " " + acciones[i].Usuario2.Apellidos : "");
                row.GetCell(10).CellStyle = customCellStyle;
                //Responsable Contratista
                row.CreateCell(11).SetCellValue(acciones[i].Responsable);
                row.GetCell(11).CellStyle = customCellStyle;
                //Fecha
                row.CreateCell(12).SetCellValue(acciones[i].Fecha.Value.ToString("dd/MM/yyyy"));
                row.GetCell(12).CellStyle = customCellStyle;
                //Estatus
                row.CreateCell(13).SetCellValue(acciones[i].Estatus);
                row.GetCell(13).CellStyle = customCellStyle;
                //Fecha Implementacion
                row.CreateCell(14).SetCellValue(acciones[i].Fecha_Implementacion != null ? acciones[i].Fecha_Implementacion.Value.ToString("dd/MM/yyyy") : "");
                row.GetCell(14).CellStyle = customCellStyle;
                //Comentario Implementacion
                row.CreateCell(15).SetCellValue(acciones[i].Comentarios_Implementacion);
                row.GetCell(15).CellStyle = customCellStyle;
                //Fecha Verificacion
                row.CreateCell(16).SetCellValue(acciones[i].Fecha_Verificacion != null ? acciones[i].Fecha_Verificacion.Value.ToString("dd/MM/yyyy") : "");
                row.GetCell(16).CellStyle = customCellStyle;
                //Comentario Verificacion
                row.CreateCell(17).SetCellValue(acciones[i].Comentarios_Verificacion);
                row.GetCell(17).CellStyle = customCellStyle;
                //Estado
                row.CreateCell(18).SetCellValue(acciones[i].Estado);
                row.GetCell(18).CellStyle = customCellStyle;
            }
            return sheet;
        }
        public async Task<ISheet> DescargarCausasInvestigacion(string sTime, string eTime, XSSFWorkbook templateWorkbook, string sheetName)
        {
            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet(sheetName);
            var nColumns = sheet.GetRow(0).Cells.Count();

            XSSFCellStyle customCellStyle = GetDefaultCellStyle(templateWorkbook);

            var causas = await _obtenerCausasInvestigacion.Execute(sTime, eTime);
            for (int i = 0; i < causas.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                //Codigo
                row.CreateCell(0).SetCellValue(causas[i].Investigacion_Final.Codigo);
                row.GetCell(0).CellStyle = customCellStyle;
                //Fecha_Incidente
                row.CreateCell(1).SetCellValue(causas[i].Investigacion_Final.Fecha_Incidente.Value.ToString("dd/MM/yyyy"));
                row.GetCell(1).CellStyle = customCellStyle;
                //Tipo Acto/Condicion
                row.CreateCell(2).SetCellValue(causas[i].Tipo_Acto_Condicion);
                row.GetCell(2).CellStyle = customCellStyle;
                //Tipo Condicion
                row.CreateCell(3).SetCellValue(causas[i].TipoCondicion != null ? causas[i].TipoCondicion.Descripcion : "");
                row.GetCell(3).CellStyle = customCellStyle;
                //Tipo_Acto_Condicion
                row.CreateCell(4).SetCellValue(causas[i].Factor_Causa_Incidente != null ? causas[i].Factor_Causa_Incidente.Descripcion : "");
                row.GetCell(4).CellStyle = customCellStyle;
                //Estado
                row.CreateCell(5).SetCellValue(causas[i].Descripcion);
                row.GetCell(5).CellStyle = customCellStyle;
                //Estado
                row.CreateCell(6).SetCellValue(causas[i].Estado);
                row.GetCell(6).CellStyle = customCellStyle;
            }
            return sheet;
        }
        public async Task<ISheet> DescargarProductoDerramadoRP(string sTime, string eTime, XSSFWorkbook templateWorkbook, string sheetName)
        {
            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet(sheetName);
            var nColumns = sheet.GetRow(0).Cells.Count();

            XSSFCellStyle customCellStyle = GetDefaultCellStyle(templateWorkbook);

            var productos = await _productoDerramadoRP.Execute(sTime, eTime);
            for (int i = 0; i < productos.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                //Codigo
                row.CreateCell(0).SetCellValue(productos[i].Codigo);
                row.GetCell(0).CellStyle = customCellStyle;
                //Fecha_Incidente
                row.CreateCell(1).SetCellValue(productos[i].Fecha_Incidente.HasValue ? productos[i].Fecha_Incidente.Value.ToString("dd/MM/yyyy") : "");
                row.GetCell(1).CellStyle = customCellStyle;
                //Producto_Derramado
                row.CreateCell(2).SetCellValue(productos[i].Producto_Derramado);
                row.GetCell(2).CellStyle = customCellStyle;
                //Cantidad_Derramado
                row.CreateCell(3).SetCellValue(productos[i].Cantidad_Derramado != null ? productos[i].Cantidad_Derramado.ToString() : "");
                row.GetCell(3).CellStyle = customCellStyle;
                //Unidad_Cantidad_Derramado
                row.CreateCell(4).SetCellValue(productos[i].Unidad_Cantidad_Derramado.Descripcion);
                row.GetCell(4).CellStyle = customCellStyle;
                //Area_Afectada
                row.CreateCell(5).SetCellValue(productos[i].Area_Afectada != null ? productos[i].Area_Afectada.ToString() : "");
                row.GetCell(5).CellStyle = customCellStyle;
                //Unidad_Area_Afectada
                row.CreateCell(6).SetCellValue(productos[i].Unidad_Area_Afectada.Descripcion);
                row.GetCell(6).CellStyle = customCellStyle;
                //Lugar_Incidente
                row.CreateCell(7).SetCellValue(productos[i].Lugar_Incidente);
                row.GetCell(7).CellStyle = customCellStyle;
                //Equipo_Involucrado
                row.CreateCell(8).SetCellValue(productos[i].Equipo_Involucrado);
                row.GetCell(8).CellStyle = customCellStyle;
                //Componente_Ambiental
                row.CreateCell(9).SetCellValue(productos[i].Componente_Ambiental.Descripcion);
                row.GetCell(9).CellStyle = customCellStyle;
                //Descripcion_Incidente
                row.CreateCell(10).SetCellValue(productos[i].Descripcion_Incidente);
                row.GetCell(10).CellStyle = customCellStyle;
                //Otra_Causa_Incidente
                row.CreateCell(11).SetCellValue(productos[i].Otra_Causa_Incidente);
                row.GetCell(11).CellStyle = customCellStyle;
                //Gerencia_Responsable
                row.CreateCell(12).SetCellValue(productos[i].Gerencia_Responsable.Descripcion);
                row.GetCell(12).CellStyle = customCellStyle;
                //Comentario_Accion
                row.CreateCell(13).SetCellValue(productos[i].Comentario_Accion);
                row.GetCell(13).CellStyle = customCellStyle;
                //Supervisor_MA
                row.CreateCell(14).SetCellValue(productos[i].Supervisor_MA.Nombres + " " + productos[i].Supervisor_MA.Apellidos);
                row.GetCell(14).CellStyle = customCellStyle;
                //Estado
                row.CreateCell(15).SetCellValue(productos[i].Estado);
                row.GetCell(15).CellStyle = customCellStyle;
            }
            return sheet;
        }
        public async Task<ISheet> DescargarProductoDerramadoInvestigacion(string sTime, string eTime, XSSFWorkbook templateWorkbook, string sheetName)
        {
            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet(sheetName);
            var nColumns = sheet.GetRow(0).Cells.Count();

            XSSFCellStyle customCellStyle = GetDefaultCellStyle(templateWorkbook);

            var productosInvestigacion = await _productoDerramadoInvestigacion.Execute(sTime, eTime);
            for (int i = 0; i < productosInvestigacion.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                //Codigo
                row.CreateCell(0).SetCellValue(productosInvestigacion[i].Investigacion_Final.Codigo);
                row.GetCell(0).CellStyle = customCellStyle;
                //Reportante
                row.CreateCell(1).SetCellValue(productosInvestigacion[i].Investigacion_Final.Fecha_Incidente != null ? productosInvestigacion[i].Investigacion_Final.Fecha_Incidente.Value.ToString("dd/MM/yyyy") : "");
                row.GetCell(1).CellStyle = customCellStyle;
                //Gerencia
                row.CreateCell(2).SetCellValue(productosInvestigacion[i].Producto_Incidente.Descripcion);
                row.GetCell(2).CellStyle = customCellStyle;
                //Empresa
                row.CreateCell(3).SetCellValue(productosInvestigacion[i].Tipo_Producto.Descripcion);
                row.GetCell(3).CellStyle = customCellStyle;
                //Otra Empresa
                row.CreateCell(4).SetCellValue(productosInvestigacion[i].Estado_Producto);
                row.GetCell(4).CellStyle = customCellStyle;
                //Empresa Observada
                row.CreateCell(5).SetCellValue(productosInvestigacion[i].Cantidad_Derramada != null ? productosInvestigacion[i].Cantidad_Derramada.Value.ToString() : "");
                row.GetCell(5).CellStyle = customCellStyle;
                //Gerencia Observada
                row.CreateCell(6).SetCellValue(productosInvestigacion[i].Unidad.Descripcion);
                row.GetCell(6).CellStyle = customCellStyle;
                //Ubicacion
                row.CreateCell(7).SetCellValue(productosInvestigacion[i].Equipo_Responsable);
                row.GetCell(7).CellStyle = customCellStyle;
                //Fecha
                row.CreateCell(8).SetCellValue(productosInvestigacion[i].Estado_Equipo_Responsable.Descripcion);
                row.GetCell(8).CellStyle = customCellStyle;
                //SSO/MA
                row.CreateCell(9).SetCellValue(productosInvestigacion[i].Cantidad_Recuperado != null ? productosInvestigacion[i].Cantidad_Recuperado.Value.ToString() : "");
                row.GetCell(9).CellStyle = customCellStyle;
                //Clasificacion
                row.CreateCell(10).SetCellValue(productosInvestigacion[i].Cantidad_No_Recuperado != null ? productosInvestigacion[i].Cantidad_No_Recuperado.Value.ToString() : "");
                row.GetCell(10).CellStyle = customCellStyle;
                //Nivel de Riesgo
                row.CreateCell(11).SetCellValue(productosInvestigacion[i].Estado);
                row.GetCell(11).CellStyle = customCellStyle;
            }
            return sheet;
        }
        public async Task<ISheet> DescargarAvistamientos(string sTime, string eTime, XSSFWorkbook templateWorkbook, string sheetName)
        {
            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet(sheetName);
            var nColumns = sheet.GetRow(0).Cells.Count();

            XSSFCellStyle customCellStyle = GetDefaultCellStyle(templateWorkbook);

            var avistamientos = await _obtenerAvistamientos.Execute(sTime, eTime);
            for (int i = 0; i < avistamientos.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                //Codigo
                row.CreateCell(0).SetCellValue(avistamientos[i].Codigo);
                row.GetCell(0).CellStyle = customCellStyle;
                //FechaAvistamiento
                row.CreateCell(1).SetCellValue(avistamientos[i].FechaAvistamiento.ToString("dd/MM/yyyy"));
                row.GetCell(1).CellStyle = customCellStyle;
                //Ubicacion
                row.CreateCell(2).SetCellValue(avistamientos[i].Ubicacion);
                row.GetCell(2).CellStyle = customCellStyle;
                //NombreComun
                row.CreateCell(3).SetCellValue(avistamientos[i].NombreComun.Descripcion);
                row.GetCell(3).CellStyle = customCellStyle;
                //Cantidad
                row.CreateCell(4).SetCellValue(avistamientos[i].Cantidad.ToString());
                row.GetCell(4).CellStyle = customCellStyle;
                //TipoAvistamiento
                row.CreateCell(5).SetCellValue(avistamientos[i].TipoAvistamiento.Descripcion);
                row.GetCell(5).CellStyle = customCellStyle;
                //EstadoFauna
                row.CreateCell(6).SetCellValue(avistamientos[i].EstadoFauna.Descripcion);
                row.GetCell(6).CellStyle = customCellStyle;
                //LugarReferencia
                row.CreateCell(7).SetCellValue(avistamientos[i].LugarReferencia.Descripcion);
                row.GetCell(7).CellStyle = customCellStyle;
                //LugarReferencia.Latitud
                row.CreateCell(8).SetCellValue(avistamientos[i].LugarReferencia.Latitud);
                row.GetCell(8).CellStyle = customCellStyle;
                //LugarReferencia.Longitud
                row.CreateCell(9).SetCellValue(avistamientos[i].LugarReferencia.Longitud);
                row.GetCell(9).CellStyle = customCellStyle;
                //Usuario
                row.CreateCell(10).SetCellValue(avistamientos[i].Usuario.Nombres+ " "+ avistamientos[i].Usuario.Apellidos);
                row.GetCell(10).CellStyle = customCellStyle;
                //DescripcionAccion
                row.CreateCell(11).SetCellValue(avistamientos[i].DescripcionAccion);
                row.GetCell(11).CellStyle = customCellStyle;
                //Supervisor
                row.CreateCell(12).SetCellValue(avistamientos[i].Supervisor!=null? avistamientos[i].Supervisor.Nombres + " " + avistamientos[i].Supervisor.Apellidos:"");
                row.GetCell(12).CellStyle = customCellStyle;
                //Estado
                row.CreateCell(13).SetCellValue(avistamientos[i].Estado);
                row.GetCell(13).CellStyle = customCellStyle;
            }
            return sheet;
        }
    }
}
