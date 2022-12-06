using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace GestionAmbiental.UseCases.CompromisosMCB
{
    public class ExcelLecturaEscritura
    {
        protected IWorkbook Workbook;
        protected ISheet sheet;
        protected string Pagina;
        protected int Fila;
        protected int Columna;
        protected DataFormatter dataFormatter;
        protected IFormulaEvaluator formulaEvaluator;

        public ExcelLecturaEscritura(Stream excelFileStream, string pagina, int fila, int columna)
        {
            this.Workbook = WorkbookFactory.Create(excelFileStream);
            if (this.Workbook != null)
            {
                this.dataFormatter = new DataFormatter(CultureInfo.InvariantCulture);
                this.formulaEvaluator = WorkbookFactory.CreateFormulaEvaluator(this.Workbook);
            }
            Pagina = pagina;
            Fila = fila - 1;
            Columna = columna - 1;
        }
        public bool ValidarExistenciaPagina()
        {
            var result = true;
            sheet = Workbook.GetSheet(Pagina);
            if (sheet == null)
                result = false;

            return result;
        }
        public List<CompromisoModel> ObtenerDatos()
        {
            var output = new List<CompromisoModel>();

            if (sheet != null)
                for (int row = Fila; row <= sheet.LastRowNum; row++)
                {
                    var sheetRow = sheet.GetRow(row);
                    if (sheetRow != null && sheetRow.Cells.Count >= 6) //null is when the row only contains empty cells 
                    {
                        /*
                        if (sheetRow.GetCell(Columna).CellType != CellType.String && sheetRow.GetCell(Columna).DateCellValue == DateTime.MinValue)
                            break;
                        else if (sheetRow.GetCell(Columna).CellType == CellType.String && DateTime.Parse(sheetRow.GetCell(Columna).StringCellValue) == DateTime.MinValue)
                            break;
                        */
                        var outputItem = new CompromisoModel();
                        for (int column = Columna; column <= Columna + 29; column++)
                        {
                            //Codigo
                            if (column == Columna)
                                outputItem.Codigo = sheetRow.GetCell(column)?.StringCellValue;
                            //Detalle Compromiso
                            else if (column == Columna + 1)
                                outputItem.Detalle_compromiso = sheetRow.GetCell(column)?.StringCellValue;
                            //Texto compromiso
                            else if (column == Columna + 2)
                                outputItem.Texto_compromiso = sheetRow.GetCell(column)?.StringCellValue;
                            //Origen
                            else if (column == Columna + 3)
                                outputItem.Id_Origen = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            //Año
                            else if (column == Columna + 4)
                                outputItem.Ano = sheetRow.GetCell(column)?.StringCellValue;
                            //Tomo
                            else if (column == Columna + 5)
                                outputItem.Tomo = sheetRow.GetCell(column)?.StringCellValue;
                            //Capitulo
                            else if (column == Columna + 6)
                                outputItem.Capitulo = sheetRow.GetCell(column)?.StringCellValue;
                            //Seccion
                            else if (column == Columna + 7)
                                outputItem.Seccion = sheetRow.GetCell(column)?.StringCellValue;
                            //Pagina
                            else if (column == Columna + 8)
                            {
                                if (sheetRow.GetCell(column).CellType != CellType.String)
                                    outputItem.Pagina = sheetRow.GetCell(column)?.NumericCellValue.ToString();
                                else
                                    outputItem.Pagina = sheetRow.GetCell(column)?.StringCellValue;
                            }
                            //R.D. N°, INFORME N°
                            else if (column == Columna + 9)
                                outputItem.Resolucion = sheetRow.GetCell(column)?.StringCellValue;
                            //Autoridad competente (D)
                            else if (column == Columna + 10)
                                outputItem.Id_Autoridad = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            //Fecha
                            else if (column == Columna + 11)
                            {
                                if (sheetRow.GetCell(column).CellType != CellType.String)
                                    outputItem.Fecha = sheetRow.GetCell(column).DateCellValue;
                                else
                                    outputItem.Fecha = DateTime.Parse(sheetRow.GetCell(Columna).StringCellValue);
                            }
                            //Compromiso relacionado (D)
                            else if (column == Columna + 12)
                                outputItem.Duplicado = sheetRow.GetCell(column)?.StringCellValue;
                            //tipo compromiso
                            else if (column == Columna + 13)
                                outputItem.Id_TipoCompromiso = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            //actividad
                            else if (column == Columna + 14)
                                outputItem.Actividad = sheetRow.GetCell(column)?.StringCellValue;
                            //etapa
                            else if (column == Columna + 15)
                                outputItem.Id_Etapa = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            //Componente
                            else if (column == Columna + 16)
                                outputItem.Id_Componente = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            //Componente Auxiliar
                            else if (column == Columna + 17)
                                outputItem.Id_ComponenteAux = (int?)sheetRow.GetCell(column)?.NumericCellValue;

                            //Factor Ambiental
                            else if (column == Columna + 18)
                                outputItem.Id_FactorAmbiental = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            //Aspecto
                            else if (column == Columna + 19)
                                outputItem.Id_Aspecto = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            //Area responsable
                            else if (column == Columna + 20)
                                outputItem.Id_Area = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            //Vigencia
                            else if (column == Columna + 21)
                                outputItem.Vigencia = sheetRow.GetCell(column)?.StringCellValue;
                            //Motivo
                            else if (column == Columna + 22)
                                outputItem.Motivo = sheetRow.GetCell(column)?.StringCellValue;
                            //Frecuencia
                            else if (column == Columna + 23)
                                outputItem.Frecuencia = sheetRow.GetCell(column)?.StringCellValue;
                            //Reporte
                            else if (column == Columna + 24)
                                outputItem.Frecuencia_reporte = sheetRow.GetCell(column)?.StringCellValue;
                            //Validación
                            else if (column == Columna + 25)
                                outputItem.Estado = sheetRow.GetCell(column)?.StringCellValue;
                            //Comentario
                            else if (column == Columna + 26)
                                outputItem.Comentario = sheetRow.GetCell(column)?.StringCellValue;





                            ///*
                            //Código	
                            //Criterio (D)
                            //Detalle del compromiso u obligación ambiental
                            //Etapa (D)
                            //Texto original del compromiso u obligación
                            //Origen (D)
                            //Tomo o volumen	
                            //Cap.	
                            //Sec.	
                            //Pag.	
                            //R.D. N°, INFORME N°	
                            //Autoridad competente (D)
                            //Fecha
                            //Compromiso relacionado (D)
                            //Aspecto Ambiental (D)
                            //Componente (D) 
                            //Gerencia responsable (D)
                            //Area (D)
                            //Frecuencia de reporte  a la autoridad (D)
                            //Entidad reportable (D)
                            //Responsable de reportabilidad (D) 1
                            //Responsable de reportabilidad (D) 2
                            //Frecuencia de reportabilidad en el sistema (D)
                            //sustentos de cumplimiento
                            //Asesor MAP
                            //Validación de los sustentos (D)
                            //Estado (D)
                            // */
                            ////Código
                            //if (column == Columna)
                            //    outputItem.Codigo = sheetRow.GetCell(column)?.StringCellValue;
                            ////Criterio (D)
                            //else if (column == Columna + 1)
                            //    outputItem.Id_Criterio = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            ////Detalle del compromiso u obligación ambiental
                            //else if (column == Columna + 2)
                            //    outputItem.Detalle_compromiso = sheetRow.GetCell(column)?.StringCellValue;
                            ////Etapa (D)
                            //else if (column == Columna + 3)
                            //    outputItem.Id_Etapa = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            ////Texto original del compromiso u obligación
                            //else if (column == Columna + 4)
                            //    outputItem.Texto_compromiso = sheetRow.GetCell(column)?.StringCellValue;
                            ////Origen (D)
                            //else if (column == Columna + 5)
                            //    outputItem.Id_Origen = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            ////Tomo o volumen
                            //else if (column == Columna + 6)
                            //    outputItem.Tomo = sheetRow.GetCell(column)?.StringCellValue;
                            ////Capitulo
                            //else if (column == Columna + 7)
                            //    outputItem.Capitulo = sheetRow.GetCell(column)?.StringCellValue;
                            ////Seccion
                            //else if (column == Columna + 8)
                            //    outputItem.Seccion = sheetRow.GetCell(column)?.StringCellValue;
                            ////Pagina
                            //else if (column == Columna + 9)
                            //{
                            //    if (sheetRow.GetCell(column).CellType != CellType.String)
                            //        outputItem.Pagina = sheetRow.GetCell(column)?.NumericCellValue.ToString();
                            //    else
                            //        outputItem.Pagina = sheetRow.GetCell(column)?.StringCellValue;
                            //}
                            ////R.D. N°, INFORME N°
                            //else if (column == Columna + 10)
                            //    outputItem.Resolucion = sheetRow.GetCell(column)?.StringCellValue;
                            ////Autoridad competente (D)
                            //else if (column == Columna + 11)
                            //    outputItem.Id_Autoridad = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            ////Fecha
                            //else if (column == Columna + 12)
                            //{
                            //    if (sheetRow.GetCell(column).CellType != CellType.String)
                            //        outputItem.Fecha = sheetRow.GetCell(column).DateCellValue;
                            //    else
                            //        outputItem.Fecha = DateTime.Parse(sheetRow.GetCell(Columna).StringCellValue);
                            //}
                            ////Compromiso relacionado (D)
                            //else if (column == Columna + 13)
                            //    outputItem.Id_Compromiso = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            ////Aspecto Ambiental (D)
                            //else if (column == Columna + 14)
                            //    outputItem.Id_Aspecto = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            ////Componente (D)
                            //else if (column == Columna + 15)
                            //    outputItem.Id_Componente = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            ////Gerencia responsable (D)
                            //else if (column == Columna + 16)
                            //    outputItem.Id_Gerencia = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            ////Area (D)
                            //else if (column == Columna + 17)
                            //    outputItem.Id_Area = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            ////Frecuencia de reporte  a la autoridad (D)
                            //else if (column == Columna + 18)
                            //    outputItem.Frecuencia = sheetRow.GetCell(column)?.StringCellValue;
                            ////Entidad reportable (D)
                            //else if (column == Columna + 19)
                            //    outputItem.Id_Entidad = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            ////Responsable de reportabilidad (D) 1
                            //else if (column == Columna + 20)
                            //    outputItem.Id_Responsable1 = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            ////Responsable de reportabilidad (D) 2
                            //else if (column == Columna + 21)
                            //    outputItem.Id_Responsable2 = (int?)sheetRow.GetCell(column)?.NumericCellValue;
                            ////Frecuencia de reportabilidad en el sistema (D)
                            //else if (column == Columna + 22)
                            //    outputItem.Frecuencia_reporte = sheetRow.GetCell(column)?.StringCellValue;
                            // //Frecuencia de reportabilidad en el sistema (D)
                            //else if (column == Columna + 23)
                            //    outputItem.Estado = sheetRow.GetCell(column)?.StringCellValue;
                        }
                        output.Add(outputItem);
                    }
                }
            return output;
        }
    }
}
