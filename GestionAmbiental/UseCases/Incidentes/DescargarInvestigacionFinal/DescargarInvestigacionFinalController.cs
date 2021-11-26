using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Causa.GetCausa;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetImpactoAmbiental;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.GetInvestigacionFinal;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Involucrado.GetInvolucrados;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Producto.GetProductos;
using GestionAmbiental.Application.UseCases.Info_SGA.GetInfo_SGA;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace GestionAmbiental.UseCases.Incidentes.DescargarInvestigacionFinal
{
    public class DescargarInvestigacionFinalController : Controller
    {
        private readonly IGetInfo_SGA _info_SGA;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IGetInvestigacionFinal _getInvestigacionFinal;
        private readonly IGetImpactoAmbiental _getImpactoAmbiental;

        private readonly IGetInvolucradosIncidente _getInvolucradosIncidente;
        private readonly IGetProductosDerramados _getProductosDerramados;
        private readonly IGetCausasIncidenteInvestigacion _getCausasIncidenteInvestigacion;
        private readonly IGetAccionesIncidenteInvestigacion _getAccionesIncidenteInvestigacion;

        public DescargarInvestigacionFinalController(
            IGetInfo_SGA info_SGA,
            IWebHostEnvironment hostingEnvironment,
            IGetInvestigacionFinal getInvestigacionFinal,
            IGetImpactoAmbiental getImpactoAmbiental, 
            IGetInvolucradosIncidente getInvolucradosIncidente,
            IGetProductosDerramados getProductosDerramados,
            IGetCausasIncidenteInvestigacion getCausasIncidenteInvestigacion,
            IGetAccionesIncidenteInvestigacion getAccioIncidenteInvestigacion)
        {
            _info_SGA = info_SGA;
            _hostingEnvironment = hostingEnvironment;
            _getInvestigacionFinal = getInvestigacionFinal;
            _getImpactoAmbiental = getImpactoAmbiental;
            _getInvolucradosIncidente = getInvolucradosIncidente;
            _getProductosDerramados = getProductosDerramados;
            _getCausasIncidenteInvestigacion = getCausasIncidenteInvestigacion;
            _getAccionesIncidenteInvestigacion = getAccioIncidenteInvestigacion;
        }

        public async Task<MemoryStream> PrepararInvestigacion(int IdInvestigacion)
        {
            var investigacion = await _getInvestigacionFinal.Execute(IdInvestigacion);
            var impacto = await _getImpactoAmbiental.Execute(IdInvestigacion);
            var involucrados = await _getInvolucradosIncidente.Execute(IdInvestigacion);
            var productos = await _getProductosDerramados.Execute(IdInvestigacion);
            var acciones = await _getAccionesIncidenteInvestigacion.Execute(IdInvestigacion);
            var causas = await _getCausasIncidenteInvestigacion.Execute(IdInvestigacion);

            string sWebRootFolder = Path.Combine(_hostingEnvironment.WebRootPath, "ExcelTemplates", "Incidentes", "Investigacion");
            string sFileName = @"JU-MAP-STD-001.F02_Investigación final de incidente ambiental_V00.xlsx";
            string sFilePath = Path.Combine(sWebRootFolder, sFileName);

            FileStream fs = new FileStream(sFilePath, FileMode.Open, FileAccess.Read);

            // Load the template into a NPOI workbook
            XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);

            // Load the sheet you are going to use as a template into NPOI
            ISheet sheet = templateWorkbook.GetSheet("IFIA");

            // Cell Style
            XSSFCellStyle customCellStyle = (XSSFCellStyle)templateWorkbook.CreateCellStyle();

            IFont hFont = (IFont)templateWorkbook.CreateFont();

            hFont.FontHeightInPoints = 10;
            hFont.FontName = "Arial";
            hFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;

            customCellStyle.WrapText = true;
            customCellStyle.VerticalAlignment = VerticalAlignment.Center;
            customCellStyle.Alignment = HorizontalAlignment.Center;
            customCellStyle.BorderTop = BorderStyle.Thin;
            customCellStyle.BorderBottom = BorderStyle.Thin;
            customCellStyle.BorderLeft = BorderStyle.Thin;
            customCellStyle.BorderRight = BorderStyle.Thin;
            customCellStyle.SetFont(hFont);

            var rowHeight = sheet.GetRow(36).Height;

            sheet.GetRow(7).GetCell(24).SetCellValue(investigacion.Num_Trabajadores);

            sheet.GetRow(11).GetCell(2).SetCellValue(investigacion.Razon_Social_Contratista != null ? investigacion.Razon_Social_Contratista : "No Aplica");
            sheet.GetRow(11).GetCell(14).SetCellValue(investigacion.RUC_Contratista != null ? investigacion.RUC_Contratista : "No Aplica");
            sheet.GetRow(11).GetCell(24).SetCellValue(investigacion.Actividad_Contratista != null ? investigacion.Actividad_Contratista : "No Aplica");

            sheet.GetRow(12).GetCell(2).SetCellValue(investigacion.Domicilio_Contratista != null ? investigacion.Domicilio_Contratista : "No Aplica");
            sheet.GetRow(12).GetCell(24).SetCellValue(investigacion.Num_Trabajadores_Contratista != null ? investigacion.Num_Trabajadores_Contratista.ToString() : "No Aplica");

            sheet.GetRow(14).GetCell(2).SetCellValue(investigacion.Razon_Social_Subcontratista != null ? investigacion.Razon_Social_Subcontratista : "No Aplica");
            sheet.GetRow(14).GetCell(14).SetCellValue(investigacion.RUC_Subcontratista != null ? investigacion.RUC_Subcontratista : "No Aplica");
            sheet.GetRow(14).GetCell(24).SetCellValue(investigacion.Actividad_Subcontratista != null ? investigacion.Actividad_Subcontratista : "No Aplica");

            sheet.GetRow(15).GetCell(2).SetCellValue(investigacion.Domicilio_Subcontratista != null ? investigacion.Domicilio_Subcontratista : "No Aplica");
            sheet.GetRow(15).GetCell(24).SetCellValue(investigacion.Num_Trabajadores_Subcontratista != null ? investigacion.Num_Trabajadores_Subcontratista.ToString() : "No Aplica");

            sheet.GetRow(19).GetCell(2).SetCellValue(investigacion.Tipo_Incidente);
            sheet.GetRow(19).GetCell(23).SetCellValue(investigacion.Nivel_Incidente);

            sheet.GetRow(20).GetCell(2).SetCellValue(investigacion.Lugar_Incidente);
            sheet.GetRow(20).GetCell(19).SetCellValue(investigacion.Coordenada_Este);
            sheet.GetRow(20).GetCell(27).SetCellValue(investigacion.Coordenada_Norte);

            sheet.GetRow(21).GetCell(2).SetCellValue(investigacion.Localidad);
            sheet.GetRow(21).GetCell(16).SetCellValue(investigacion.Distrito);
            sheet.GetRow(21).GetCell(23).SetCellValue(investigacion.Provincia);
            sheet.GetRow(21).GetCell(30).SetCellValue(investigacion.Departamento);

            sheet.GetRow(22).GetCell(2).SetCellValue(investigacion.Fecha_Incidente != null ? investigacion.Fecha_Incidente.Value.ToString("dd/MM/yyyy") : "");
            sheet.GetRow(22).GetCell(16).SetCellValue(investigacion.Fecha_Incidente != null ? investigacion.Fecha_Incidente.Value.ToString("HH:mm") : "");
            sheet.GetRow(22).GetCell(23).SetCellValue(investigacion.Fecha_Inicio_Investigacion != null ? investigacion.Fecha_Inicio_Investigacion.Value.ToString("dd/MM/yyyy") : "");
            sheet.GetRow(22).GetCell(30).SetCellValue(investigacion.Turno);

            sheet.GetRow(23).GetCell(2).SetCellValue(investigacion.Descripcion_Incidente);

            sheet.GetRow(25).GetCell(2).SetCellValue(investigacion.Gerencia_Responsable);

            //Involucrados
            int filaInvolucrados = 27;
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
                for (int currentCell = 1; currentCell < 33; currentCell++)
                {
                    row.CreateCell(currentCell);
                    row.GetCell(currentCell).CellStyle = customCellStyle;
                }
                //Completar la informacion
                row.GetCell(1).SetCellValue(involucrado.Tipo_Involucrado);
                row.GetCell(2).SetCellValue(involucrado.Nombres.ToUpper() + " " + involucrado.Apellidos.ToUpper());
                row.GetCell(16).SetCellValue(involucrado.DocIdentidad);
                row.GetCell(21).SetCellValue(involucrado.Edad != null ? involucrado.Edad.ToString() : "");
                row.GetCell(23).SetCellValue(involucrado.Puesto_Trabajo);
                row.GetCell(29).SetCellValue(involucrado.Empresa);
                //Unir las celdas
                sheet.AddMergedRegion(new CellRangeAddress(filaInvolucrados, filaInvolucrados, 2, 15));
                sheet.AddMergedRegion(new CellRangeAddress(filaInvolucrados, filaInvolucrados, 16, 20));
                sheet.AddMergedRegion(new CellRangeAddress(filaInvolucrados, filaInvolucrados, 21, 22));
                sheet.AddMergedRegion(new CellRangeAddress(filaInvolucrados, filaInvolucrados, 23, 28));
                sheet.AddMergedRegion(new CellRangeAddress(filaInvolucrados, filaInvolucrados, 29, 32));
            }

            //Productos
            int filaProductos = filaInvolucrados+involucrados.Count+2;
            foreach (var producto in productos)
            {
                IRow row = sheet.GetRow(filaProductos);
                //Correr todas las filas y agregar una nueva
                if (row != null)
                {
                    sheet.ShiftRows(filaProductos, sheet.LastRowNum, 1);
                    row = sheet.CreateRow(filaProductos);
                }
                //Agregar las celdas necesarias y asignarles el estilo
                for (int currentCell = 1; currentCell < 33; currentCell++)
                {
                    row.CreateCell(currentCell);
                    row.GetCell(currentCell).CellStyle = customCellStyle;
                }
                //Completar la informacion
                row.GetCell(1).SetCellValue(producto.Producto_Incidente);
                row.GetCell(2).SetCellValue(producto.Tipo_Producto);
                row.GetCell(8).SetCellValue(producto.Estado_Producto);
                row.GetCell(12).SetCellValue(producto.Cantidad_Derramada + " " +producto.Unidad);
                row.GetCell(15).SetCellValue(producto.Equipo_Responsable);
                row.GetCell(19).SetCellValue(producto.Estado_Equipo_Responsable);
                row.GetCell(23).SetCellValue(producto.Cantidad_Recuperado + " " +producto.Unidad);
                row.GetCell(29).SetCellValue(producto.Cantidad_No_Recuperado + " " + producto.Unidad);
                //Unir las celdas
                sheet.AddMergedRegion(new CellRangeAddress(filaProductos, filaProductos, 2, 7));
                sheet.AddMergedRegion(new CellRangeAddress(filaProductos, filaProductos, 8, 11));
                sheet.AddMergedRegion(new CellRangeAddress(filaProductos, filaProductos, 12, 14));
                sheet.AddMergedRegion(new CellRangeAddress(filaProductos, filaProductos, 15, 18));
                sheet.AddMergedRegion(new CellRangeAddress(filaProductos, filaProductos, 19, 22));
                sheet.AddMergedRegion(new CellRangeAddress(filaProductos, filaProductos, 23, 28));
                sheet.AddMergedRegion(new CellRangeAddress(filaProductos, filaProductos, 29, 32));
            }

            //Impacto Ambiental
            int filaImpacto = filaProductos + productos.Count + 1;
            //Suelo
            sheet.GetRow(filaImpacto).GetCell(5).SetCellValue(impacto.Tipo_Suelo!=null? impacto.Tipo_Suelo:"No Aplica");
            sheet.GetRow(filaImpacto).GetCell(12).SetCellValue(impacto.Area_Suelo!=null? impacto.Area_Suelo.ToString(): "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(14).SetCellValue(impacto.Unidad_Area_Suelo != null ? impacto.Unidad_Area_Suelo : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(21).SetCellValue(impacto.Cantidad_Suelo != null ? impacto.Cantidad_Suelo.ToString() : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(24).SetCellValue(impacto.Unidad_Cantidad_Suelo != null ? impacto.Unidad_Cantidad_Suelo.ToString() : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(29).SetCellValue(impacto.Lugar_Disposicion_Suelo != null ? impacto.Lugar_Disposicion_Suelo : "No Aplica");
            filaImpacto++;
            //Flora
            sheet.GetRow(filaImpacto).GetCell(5).SetCellValue(impacto.Especie_Flora != null ? impacto.Especie_Flora : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(12).SetCellValue(impacto.Unidad_Flora != null ? impacto.Unidad_Flora.ToString() : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(14).SetCellValue(impacto.UnidadM_Flora != null ? impacto.UnidadM_Flora : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(21).SetCellValue(impacto.Cantidad_Residuo_Flora != null ? impacto.Cantidad_Residuo_Flora.ToString() : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(24).SetCellValue(impacto.Unidad_Residuo_Flora != null ? impacto.Unidad_Residuo_Flora : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(29).SetCellValue(impacto.Lugar_Disposicion_Flora != null ? impacto.Lugar_Disposicion_Flora : "No Aplica");
            filaImpacto++;
            //Fauna
            sheet.GetRow(filaImpacto).GetCell(5).SetCellValue(impacto.Especie_Fauna != null ? impacto.Especie_Fauna : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(12).SetCellValue(impacto.Unidad_Fauna != null ? impacto.Unidad_Fauna.ToString() : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(14).SetCellValue(impacto.UnidadM_Fauna != null ? impacto.UnidadM_Fauna : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(21).SetCellValue(impacto.Cantidad_Residuo_Fauna != null ? impacto.Cantidad_Residuo_Fauna.ToString() : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(24).SetCellValue(impacto.Unidad_Residuo_Fauna != null ? impacto.Unidad_Residuo_Fauna : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(29).SetCellValue(impacto.Lugar_Disposicion_Fauna != null ? impacto.Lugar_Disposicion_Fauna : "No Aplica");
            filaImpacto++;
            //Agua
            sheet.GetRow(filaImpacto).GetCell(5).SetCellValue(impacto.Tipo_Agua != null ? impacto.Tipo_Agua : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(12).SetCellValue(impacto.Volumen_Agua != null ? impacto.Volumen_Agua.ToString() : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(14).SetCellValue(impacto.UnidadM_Agua != null ? impacto.UnidadM_Agua : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(21).SetCellValue(impacto.Cantidad_Residuo_Agua != null ? impacto.Cantidad_Residuo_Agua.ToString() : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(24).SetCellValue(impacto.Unidad_Residuo_Agua != null ? impacto.Unidad_Residuo_Agua : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(29).SetCellValue(impacto.Lugar_Disposicion_Agua != null ? impacto.Lugar_Disposicion_Agua : "No Aplica"); 
            filaImpacto++;
            //Restos Arqueológicos
            sheet.GetRow(filaImpacto).GetCell(2).SetCellValue(impacto.Restos_Arq != null? impacto.Restos_Arq : "No");
            sheet.GetRow(filaImpacto).GetCell(12).SetCellValue(impacto.Nombre_Resto_Arq != null ? impacto.Nombre_Resto_Arq : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(23).SetCellValue(impacto.Cord_Este != null ? impacto.Cord_Este : "No Aplica");
            sheet.GetRow(filaImpacto).GetCell(29).SetCellValue(impacto.Cord_Norte != null ? impacto.Cord_Norte : "No Aplica");
            filaImpacto++;
            //Personas
            sheet.GetRow(filaImpacto).GetCell(8).SetCellValue(impacto.Danio_Salud);
            sheet.GetRow(filaImpacto).GetCell(29).SetCellValue(impacto.Personas_Afectadas != null ? impacto.Personas_Afectadas.ToString() : "");
            filaImpacto++;
            //Residuos
            sheet.GetRow(filaImpacto).GetCell(2).SetCellValue(impacto.Cantidad_RS != null ? impacto.Cantidad_RS.ToString() : "");
            sheet.GetRow(filaImpacto).GetCell(10).SetCellValue(impacto.Unidad_Cantidad_RS);
            sheet.GetRow(filaImpacto).GetCell(16).SetCellValue(impacto.Volumen_Rs != null ? impacto.Volumen_Rs.ToString() : "");
            sheet.GetRow(filaImpacto).GetCell(21).SetCellValue(impacto.Unidad_Volumen_RS);
            sheet.GetRow(filaImpacto).GetCell(29).SetCellValue(impacto.EO_RS);

            //Causas Inmediatas
            int filaCausasInmediatas = filaImpacto + 5;
            var causasInmediatas = causas.Where(x => x.Id_Acto_Condicion != null).ToList();
            //Causas
            foreach (var causa in causasInmediatas)
            {
                IRow row = sheet.GetRow(filaCausasInmediatas);
                //Correr todas las filas y agregar una nueva
                if (row != null)
                {
                    sheet.ShiftRows(filaCausasInmediatas, sheet.LastRowNum, 1);
                    row = sheet.CreateRow(filaCausasInmediatas);
                }
                //Agregar las celdas necesarias y asignarles el estilo
                for (int currentCell = 1; currentCell < 33; currentCell++)
                {
                    row.CreateCell(currentCell);
                    row.GetCell(currentCell).CellStyle = customCellStyle;
                }
                //Completar la informacion
                row.GetCell(1).SetCellValue(causa.Acto_Condicion);
                row.GetCell(17).SetCellValue(causa.Descripcion != null ? causa.Descripcion : "No Aplica");
                //Unir las celdas
                sheet.AddMergedRegion(new CellRangeAddress(filaCausasInmediatas, filaCausasInmediatas, 1, 16));
                sheet.AddMergedRegion(new CellRangeAddress(filaCausasInmediatas, filaCausasInmediatas, 17, 32));
            }

            //Causas Basicas
            int filaCausasBasicas = filaCausasInmediatas + causasInmediatas.Count + 2;
            var causasBasicas = causas.Where(x => x.Id_Factor_Causa_Incidente != null).ToList();
            //Causas
            foreach (var causa in causasBasicas)
            {
                IRow row = sheet.GetRow(filaCausasBasicas);
                //Correr todas las filas y agregar una nueva
                if (row != null)
                {
                    sheet.ShiftRows(filaCausasBasicas, sheet.LastRowNum, 1);
                    row = sheet.CreateRow(filaCausasBasicas);
                }
                //Agregar las celdas necesarias y asignarles el estilo
                for (int currentCell = 1; currentCell < 33; currentCell++)
                {
                    row.CreateCell(currentCell);
                    row.GetCell(currentCell).CellStyle = customCellStyle;
                }
                //Completar la informacion
                row.GetCell(1).SetCellValue(causa.Factor_Causa_Incidente);
                row.GetCell(17).SetCellValue(causa.Descripcion != null ? causa.Descripcion : "No Aplica");
                //Unir las celdas
                sheet.AddMergedRegion(new CellRangeAddress(filaCausasBasicas, filaCausasBasicas, 1, 16));
                sheet.AddMergedRegion(new CellRangeAddress(filaCausasBasicas, filaCausasBasicas, 17, 32));
            }

            //Acciones Inmediatas
            int filaAccionesInmediatas = filaCausasBasicas + causasBasicas.Count + 3;
            var accionesInmediatas = acciones.Where(x => x.Tipo_Accion.Equals("Acción Inmediata")).ToList();

            foreach (var accion in accionesInmediatas)
            {
                IRow row = sheet.GetRow(filaAccionesInmediatas);
                //Correr todas las filas y agregar una nueva
                if (row != null)
                {
                    sheet.ShiftRows(filaAccionesInmediatas, sheet.LastRowNum, 1);
                    row = sheet.CreateRow(filaAccionesInmediatas);
                }
                //Agregar las celdas necesarias y asignarles el estilo
                for (int currentCell = 1; currentCell < 33; currentCell++)
                {
                    row.CreateCell(currentCell);
                    row.GetCell(currentCell).CellStyle = customCellStyle;
                }
                //Completar la informacion
                row.GetCell(1).SetCellValue(accion.Accion_Inmediata);
                row.GetCell(17).SetCellValue(accion.Fecha != null ? accion.Fecha.Value.ToString("dd/MM/yyyy") : "");
                row.GetCell(21).SetCellValue((accion.Id_Usuario!=null? accion.Responsable_Marcobre.ToUpper() : "")+ (accion.Id_Usuario2 != null ? (accion.Id_Usuario!=null?" - ":"") + accion.Responsable_Marcobre2.ToUpper() : ""));
                row.GetCell(25).SetCellValue(accion.Responsable_Contratista != null ? accion.Responsable_Contratista : "No Aplica");
                row.GetCell(29).SetCellValue(accion.Estatus);
                //Unir las celdas
                sheet.AddMergedRegion(new CellRangeAddress(filaAccionesInmediatas, filaAccionesInmediatas, 1, 16));
                sheet.AddMergedRegion(new CellRangeAddress(filaAccionesInmediatas, filaAccionesInmediatas, 17, 20));
                sheet.AddMergedRegion(new CellRangeAddress(filaAccionesInmediatas, filaAccionesInmediatas, 21, 24));
                sheet.AddMergedRegion(new CellRangeAddress(filaAccionesInmediatas, filaAccionesInmediatas, 25, 28));
                sheet.AddMergedRegion(new CellRangeAddress(filaAccionesInmediatas, filaAccionesInmediatas, 29, 32));
            }

            //Acciones Correctivas
            int filaAccionesCorrectivas = filaAccionesInmediatas + accionesInmediatas.Count + 2;
            var accionesCorrectivas = acciones.Where(x => x.Tipo_Accion.Equals("Acción Correctiva")).ToList();

            foreach (var accion in accionesCorrectivas)
            {
                IRow row = sheet.GetRow(filaAccionesCorrectivas);
                //Correr todas las filas y agregar una nueva
                if (row != null)
                {
                    sheet.ShiftRows(filaAccionesCorrectivas, sheet.LastRowNum, 1);
                    row = sheet.CreateRow(filaAccionesCorrectivas);
                }
                //Agregar las celdas necesarias y asignarles el estilo
                for (int currentCell = 1; currentCell < 33; currentCell++)
                {
                    row.CreateCell(currentCell);
                    row.GetCell(currentCell).CellStyle = customCellStyle;
                }
                //Completar la informacion
                row.GetCell(1).SetCellValue(accion.Accion_Inmediata);
                row.GetCell(17).SetCellValue(accion.Fecha != null ? accion.Fecha.Value.ToString("dd/MM/yyyy") : "");
                row.GetCell(21).SetCellValue((accion.Id_Usuario != null ? accion.Responsable_Marcobre.ToUpper() : "") + (accion.Id_Usuario2 != null ? (accion.Id_Usuario != null ? " - " : "") + accion.Responsable_Marcobre2.ToUpper() : ""));
                row.GetCell(25).SetCellValue(accion.Responsable_Contratista != null ? accion.Responsable_Contratista : "No Aplica");
                row.GetCell(29).SetCellValue(accion.Estatus);
                //Unir las celdas
                sheet.AddMergedRegion(new CellRangeAddress(filaAccionesCorrectivas, filaAccionesCorrectivas, 1, 16));
                sheet.AddMergedRegion(new CellRangeAddress(filaAccionesCorrectivas, filaAccionesCorrectivas, 17, 20));
                sheet.AddMergedRegion(new CellRangeAddress(filaAccionesCorrectivas, filaAccionesCorrectivas, 21, 24));
                sheet.AddMergedRegion(new CellRangeAddress(filaAccionesCorrectivas, filaAccionesCorrectivas, 25, 28));
                sheet.AddMergedRegion(new CellRangeAddress(filaAccionesCorrectivas, filaAccionesCorrectivas, 29, 32));
            }
            //Supervisor
            int filaSupervisor = filaAccionesCorrectivas + accionesCorrectivas.Count + 5;
            sheet.GetRow(filaSupervisor).GetCell(15).SetCellValue(investigacion.Supervisor_MA);

            for (int i = 8; i <= filaSupervisor; i++)
            {
                sheet.GetRow(i).Height = rowHeight;
            }

            // Save the NPOI workbook into a memory stream to be sent to the browser, could have saved to disk.
            MemoryStream ms = new MemoryStream();
            templateWorkbook.Write(ms);

            // Send the memory stream to the browser
            ms.Position = 0;
            return ms;
        }
        public async Task<IActionResult> DescargarInvestigacion(int IdInvestigacion)
        {
            var archivoInvestigacion = await PrepararInvestigacion(IdInvestigacion);
            return File(archivoInvestigacion, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "InvestigacionIncidente-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");
        }
    }
}