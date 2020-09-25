using GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua.CargaExcel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.GestorTablasMaestras.ConsumoAgua
{
    public class ExcelLecturaEscritura
    {
        protected IWorkbook Workbook;
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
            Fila = fila-1;
            Columna = columna-1;
        }
       public List<ConsumoAguaModel> ObtenerDatos()
        {
            var output = new List<ConsumoAguaModel>();
            ISheet sheet = Workbook.GetSheet(Pagina);

            for (int row = Fila; row <= sheet.LastRowNum; row++)
            {
                var sheetRow = sheet.GetRow(row);
                if (sheetRow != null) //null is when the row only contains empty cells 
                {
                    var outputItem = new ConsumoAguaModel();
                    for (int column = Columna; column < Columna+4; column++)
                    {
                        if (column == Columna)
                            outputItem.Fecha = sheetRow.GetCell(column).DateCellValue;
                        else if (column == Columna+1)
                            outputItem.TiempoBombeo = sheetRow.GetCell(column).NumericCellValue;
                        else if(column == Columna+2)
                            outputItem.LecturaInicial = sheetRow.GetCell(column).NumericCellValue;
                        else if(column == Columna+3)
                            outputItem.LecturaFinal = sheetRow.GetCell(column).NumericCellValue;
                    }
                    output.Add(outputItem);
                }                    
            }
            return output;
        }
    }
}
