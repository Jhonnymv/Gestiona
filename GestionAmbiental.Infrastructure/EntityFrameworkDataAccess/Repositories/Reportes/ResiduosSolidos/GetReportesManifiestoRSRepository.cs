using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Reportes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Reportes.ResiduosSolidos
{
    public class GetReportesManifiestoRSRepository : IGetReportesManifiestoRSReadOnlyRepository
    {
        private readonly Context _context;
        public GetReportesManifiestoRSRepository(Context context)
        {
            _context = context;
        }               
        public async Task<List<List<Dimension>>> GetNManifiestosFechas(DateTime FechaInicio, DateTime FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada, string frecuencia)
        {
            var output = new List<List<Dimension>>();
            var anulado = new List<Dimension>();
            var conforme = new List<Dimension>();
            
            var registros = await _context.Manifiesto_RSs.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosResiduosSolidos.Pendiente.ToString()) &&
                x.Observacion_Manifiesto.Equals(observacion != null ? observacion : x.Observacion_Manifiesto) &&
                x.Id_Residuo_Solido.Equals(residuo_solido != 0 ? residuo_solido : x.Id_Residuo_Solido) &&
                x.Id_EO_RS_Salida.Equals(eors_salida != 0 ? eors_salida : x.Id_EO_RS_Salida) &&
                x.Id_EO_RS_Llegada.Equals(eors_llegada != 0 ? eors_llegada : x.Id_EO_RS_Llegada) &&
                x.Fecha_Emision_Manifiesto >= FechaInicio &&
                x.Fecha_Emision_Manifiesto <= FechaFin)
                .ToListAsync();


            if (frecuencia.Equals("Diario"))
            {
                var frec = registros.GroupBy(x => x.Fecha_Emision_Manifiesto.Date).ToList();
                int nDias = 0;

                if (FechaFin.Year > FechaInicio.Year)
                    nDias = FechaFin.DayOfYear + (FechaInicio.Year % 4 == 0 ? 366 : 365) - FechaInicio.DayOfYear;
                else
                    nDias = FechaFin.DayOfYear - FechaInicio.DayOfYear;

                var lstDiasSinValor = new List<Dimension>();
                var lstDiasConValorConforme = new List<Dimension>();
                var lstDiasConValorAnulado = new List<Dimension>();
                for (int i = nDias; i >= 0; i--)
                {
                    var diaAnio = FechaFin.AddDays(-i).DayOfYear;
                    lstDiasSinValor.Add(new Dimension(FechaFin.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, (FechaFin.AddDays(-i).Year * 1000) + diaAnio));
                }
                foreach (var item in frec)
                {
                    var diaAnio = item.Key.DayOfYear;
                    if (item.Where(x => x.Observacion_Manifiesto.Equals("Conforme")).ToList().Count > 0)
                        lstDiasConValorConforme.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Where(x => x.Observacion_Manifiesto.Equals("Conforme")).Count(), (item.Key.Year * 1000) + item.Key.DayOfYear));
                    if (item.Where(x => x.Observacion_Manifiesto.Equals("Anulado")).ToList().Count > 0)
                        lstDiasConValorAnulado.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Where(x => x.Observacion_Manifiesto.Equals("Anulado")).Count(), (item.Key.Year * 1000) + item.Key.DayOfYear));
                }
                var distinctConforme = lstDiasSinValor.Where(x => !lstDiasConValorConforme.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                conforme = distinctConforme.Union(lstDiasConValorConforme).OrderBy(x => x.Index).ToList();
                var distinctAnulado = lstDiasSinValor.Where(x => !lstDiasConValorAnulado.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                anulado = distinctAnulado.Union(lstDiasConValorAnulado).OrderBy(x => x.Index).ToList();
            }
            else if (frecuencia.Equals("Semanal"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = registros.Select(x => new { woy = cul.Calendar.GetWeekOfYear(x.Fecha_Emision_Manifiesto, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha_Emision_Manifiesto.ToString("yyyy"),observacion = x.Observacion_Manifiesto, id = x.Id_Manifiesto_RS }).ToList();
                var frec = data.GroupBy(x => x.woy).ToList();

                int nSemInicial = cul.Calendar.GetWeekOfYear(FechaInicio, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int nSemFinal = cul.Calendar.GetWeekOfYear(FechaFin, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                var lstSemSinValor = new List<Dimension>();
                var lstSemConValorConforme = new List<Dimension>();
                var lstSemConValorAnulado = new List<Dimension>();

                int nSem = 0;
                //Mismo año
                if (FechaInicio.Year == FechaFin.Year)
                {
                    nSem = nSemFinal - nSemInicial;
                    for (int i = nSem; i >= 0; i--)
                    {
                        lstSemSinValor.Add(new Dimension("Sem " + (nSemInicial + i).ToString() + " - " + FechaInicio.Year, 0, (FechaInicio.Year * 100) + nSemInicial + i));
                    }
                    foreach (var item in frec)
                    {
                        var week = Convert.ToInt32(item.Key.Substring(0, 2));
                        var year = Convert.ToInt32(item.Key.Substring(3, 4));
                        if (item.Where(x => x.observacion.Equals("Conforme")).ToList().Count > 0)
                            lstSemConValorConforme.Add(new Dimension("Sem " + week + " - " + year, item.Where(x => x.observacion.Equals("Conforme")).Count(), year * 100 + week));
                        if (item.Where(x => x.observacion.Equals("Anulado")).ToList().Count > 0)
                            lstSemConValorAnulado.Add(new Dimension("Sem " + week + " - " + year, item.Where(x => x.observacion.Equals("Anulado")).Count(), year * 100 + week));
                    }
                    var distinctConforme = lstSemSinValor.Where(x => !lstSemConValorConforme.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    conforme = distinctConforme.Union(lstSemConValorConforme).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    conforme = conforme.OrderBy(x => x.Index).ToList();

                    var distinctAnulado = lstSemSinValor.Where(x => !lstSemConValorAnulado.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    anulado = distinctAnulado.Union(lstSemConValorAnulado).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    anulado = anulado.OrderBy(x => x.Index).ToList();
                }
                //Diferentes años
                else
                {
                    int nAnios = FechaFin.Year - FechaInicio.Year + 1;

                    int nUltSem = cul.Calendar.GetWeekOfYear(new DateTime(FechaInicio.Year, 12, 31), CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                    nSem = (nUltSem - nSemInicial) + (52 * (nAnios - 2)) + (nSemFinal);
                    //Completar con ceros todas las semanas
                    //Las primeras semanas del último anio
                    for (int i = nSemInicial; i <= nUltSem; i++)
                    {
                        lstSemSinValor.Add(new Dimension("Sem " + (i).ToString() + " - " + FechaInicio.Year.ToString().Substring(2, 2), 0, (FechaInicio.Year * 100) + i));
                    }
                    //Si son 3 años o más
                    if (nAnios > 2)
                    {
                        //Considerar todos los años menos el de la fecha inicial y final
                        for (int j = 1; j <= nAnios - 2; j++)
                        {
                            int nSemAnio = cul.Calendar.GetWeekOfYear(new DateTime(FechaInicio.AddYears(j).Year, 12, 31), CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                            for (int i = 1; i <= nSemAnio; i++)
                            {
                                lstSemSinValor.Add(new Dimension("Sem " + (i).ToString() + " - " + FechaInicio.AddYears(j).Year.ToString().Substring(2, 2), 0, (FechaInicio.AddYears(j).Year * 100) + i));
                            }
                        }
                    }
                    //Las últimas semanas del último anio
                    for (int i = 1; i <= nSemFinal; i++)
                    {
                        lstSemSinValor.Add(new Dimension("Sem " + (i).ToString() + " - " + FechaFin.Year.ToString().Substring(2, 2), 0, (FechaFin.Year * 100) + i));
                    }
                    int semAnt = Convert.ToInt32(frec[0].Key.Substring(0, 2)) - 1;
                    int nAnio = 0;
                    foreach (var item in frec)
                    {
                        var semAct = Convert.ToInt32(item.Key.Substring(0, 2));
                        var anioAct = Convert.ToInt32(item.Key.Substring(3, 4));
                        var nombre = "Sem " + semAct + " - " + anioAct.ToString().Substring(2, 2);
                        if (semAnt > semAct)
                            nAnio++;
                        if (item.Where(x => x.observacion.Equals("Conforme")).ToList().Count > 0)
                            lstSemConValorConforme.Add(new Dimension(nombre, item.Where(x => x.observacion.Equals("Conforme")).Count(), (anioAct * 100) + semAct));
                        if (item.Where(x => x.observacion.Equals("Anulado")).ToList().Count > 0)
                            lstSemConValorAnulado.Add(new Dimension(nombre, item.Where(x => x.observacion.Equals("Anulado")).Count(), (anioAct * 100) + semAct));
                        semAnt = semAct;
                    }
                    var distinctConforme = lstSemSinValor.Where(x => !lstSemConValorConforme.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    conforme = distinctConforme.Union(lstSemConValorConforme).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    conforme = conforme.OrderBy(x => x.Index).ToList();

                    var distinctAnulado = lstSemSinValor.Where(x => !lstSemConValorAnulado.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    anulado = distinctAnulado.Union(lstSemConValorAnulado).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    anulado = anulado.OrderBy(x => x.Index).ToList();
                }
            }
            else if (frecuencia.Equals("Mensual"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = registros.Select(x => new { date = x.Fecha_Emision_Manifiesto.ToString("MM yyyy"), observacion = x.Observacion_Manifiesto, id = x.Id_Manifiesto_RS }).ToList();
                var frec = data.GroupBy(x => x.date).ToList();

                int nMeses;
                var lstMesSinValor = new List<Dimension>();
                var lstMesConValorIngreso = new List<Dimension>();
                var lstMesConValorSalida = new List<Dimension>();

                if (FechaInicio.Year == FechaFin.Year)
                {
                    nMeses = FechaFin.Month - FechaInicio.Month + 1;

                    for (int i = 1; i <= nMeses; i++)
                    {
                        var anio = Convert.ToInt32(FechaInicio.AddMonths(i - 1).ToString("MM yy").Substring(0, 2));
                        var mes = Convert.ToInt32(FechaInicio.AddMonths(i - 1).ToString("MM yy").Substring(2, 2));
                        var nombre = FechaInicio.AddMonths(i - 1).ToString("MMMM").Substring(0, 3) + " " + FechaInicio.AddMonths(i - 1).ToString("yyyy");
                        lstMesSinValor.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + i));
                    }
                }
                else
                {
                    int nAnios = FechaFin.Year - FechaInicio.Year + 1;

                    int contador = 0;
                    //Los primeros mese del último anio
                    for (int i = FechaInicio.Month; i <= 12; i++)
                    {
                        var nombre = FechaInicio.AddMonths(contador).ToString("MMMM").Substring(0, 3) + " " + FechaInicio.AddMonths(contador).ToString("yyyy");
                        lstMesSinValor.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + i));
                        contador++;
                    }
                    //Si son 3 años o más
                    if (nAnios > 2)
                    {
                        //Considerar todos los años menos el de la fecha inicial y final
                        for (int j = 1; j <= nAnios - 2; j++)
                        {
                            for (int i = 1; i <= 12; i++)
                            {
                                var fecha = new DateTime(FechaInicio.Year + j, i, 1);
                                var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");
                                lstMesSinValor.Add(new Dimension(nombre, 0, (FechaInicio.AddYears(j).Year * 100) + i));
                            }
                        }
                    }
                    //Los últimos meses del último anio
                    for (int i = 1; i <= FechaFin.Month; i++)
                    {
                        var fecha = new DateTime(FechaFin.Year, i, 1);
                        var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");
                        lstMesSinValor.Add(new Dimension(nombre, 0, (FechaFin.Year * 100) + i));
                    }
                }

                foreach (var item in frec)
                {
                    var mes = Convert.ToInt32(item.Key.Substring(0, 2));
                    var anio = Convert.ToInt32(item.Key.Substring(3, 4));
                    var fecha = new DateTime(anio, mes, 1);
                    var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");

                    if (item.Where(x => x.observacion.Equals("Conforme")).ToList().Count > 0)
                        lstMesConValorIngreso.Add(new Dimension(nombre, item.Where(x => x.observacion.Equals("Conforme")).Count(), anio * 100 + mes));
                    if (item.Where(x => x.observacion.Equals("Anulado")).ToList().Count > 0)
                        lstMesConValorSalida.Add(new Dimension(nombre, item.Where(x => x.observacion.Equals("Anulado")).Count(), anio * 100 + mes));

                }
                var distinctConforme = lstMesSinValor.Where(x => !lstMesConValorIngreso.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                conforme = distinctConforme.Union(lstMesConValorIngreso).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();

                var distinctAnulado = lstMesSinValor.Where(x => !lstMesConValorSalida.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                anulado = distinctAnulado.Union(lstMesConValorSalida).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();
            }
            output.Add(conforme);
            output.Add(anulado);
            return output;
        }
        public async Task<List<Dimension>> GetNManifiestosObservacion(DateTime FechaInicio, DateTime FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada)
        {
            var output = new List<Dimension>();
            var registros = await _context.Manifiesto_RSs.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosResiduosSolidos.Pendiente.ToString()) &&
                x.Observacion_Manifiesto.Equals(observacion != null ? observacion : x.Observacion_Manifiesto) &&
                x.Id_Residuo_Solido.Equals(residuo_solido != 0 ? residuo_solido : x.Id_Residuo_Solido) &&
                x.Id_EO_RS_Salida.Equals(eors_salida != 0 ? eors_salida : x.Id_EO_RS_Salida) &&
                x.Id_EO_RS_Llegada.Equals(eors_llegada != 0 ? eors_llegada : x.Id_EO_RS_Llegada) &&
                x.Fecha_Emision_Manifiesto >= FechaInicio &&
                x.Fecha_Emision_Manifiesto <= FechaFin)
                .GroupBy(x => x.Observacion_Manifiesto)
                .ToListAsync();

            output.Add(new Dimension("Conforme", 0, 1));
            output.Add(new Dimension("Anulado", 0, 2));
            foreach (var item in registros)
            {
                output.FirstOrDefault(x => x.Nombre.Equals(item.Key)).Valor = item.Count();
            }
            return output;
        }
        public async Task<List<Dimension>> GetNManifiestosCamion(DateTime FechaInicio, DateTime FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada)
        {
            var output = new List<Dimension>();
            //Tipo_Residuo = Peligroso (1)
            var list_camiones = await _context.Vehiculos_RS.Where(x=>x.Estado.Equals("ACT")).ToListAsync();

            var registros = await _context.Manifiesto_RSs.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosResiduosSolidos.Pendiente.ToString()) &&
                x.Observacion_Manifiesto.Equals("Conforme") &&
                //x.Observacion_Manifiesto.Equals(observacion != null ? observacion : x.Observacion_Manifiesto) &&
                x.Id_Residuo_Solido.Equals(residuo_solido != 0 ? residuo_solido : x.Id_Residuo_Solido) &&
                x.Id_EO_RS_Salida.Equals(eors_salida != 0 ? eors_salida : x.Id_EO_RS_Salida) &&
                x.Id_EO_RS_Llegada.Equals(eors_llegada != 0 ? eors_llegada : x.Id_EO_RS_Llegada) &&
                x.Fecha_Emision_Manifiesto >= FechaInicio &&
                x.Fecha_Emision_Manifiesto <= FechaFin)
                .GroupBy(x => x.Id_Vehiculo_RS)
                .ToListAsync();
            foreach (var item in list_camiones)
            {
                output.Add(new Dimension(item.Placa, 0, item.Id_Vehiculo_RS));
            }

            foreach (var item in registros)
            {
                output.FirstOrDefault(x => x.Index.Equals(item.Key)).Valor = item.Sum(x => x.Cantidad_RS_Evacuado);
            }
            return output;
        }
        public async Task<List<Dimension>> GetManifiestosTipoSalida(DateTime FechaInicio, DateTime FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada)
        {
            var output = new List<Dimension>();
            //Tipo_Residuo = Peligroso (1)
            var list_residuo_solido = await _context.Residuos_Solidos.Where(x=>x.Id_Tipo_Residuo.Equals(1)).ToListAsync();

            var registros = await _context.Manifiesto_RSs.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosResiduosSolidos.Pendiente.ToString()) &&
                x.Observacion_Manifiesto.Equals("Conforme") &&
                x.Id_Residuo_Solido.Equals(residuo_solido != 0 ? residuo_solido : x.Id_Residuo_Solido) &&
                x.Id_EO_RS_Salida.Equals(eors_salida != 0 ? eors_salida : x.Id_EO_RS_Salida) &&
                x.Id_EO_RS_Llegada.Equals(eors_llegada != 0 ? eors_llegada : x.Id_EO_RS_Llegada) &&
                x.Fecha_Emision_Manifiesto >= FechaInicio &&
                x.Fecha_Emision_Manifiesto <= FechaFin)
                .GroupBy(x => x.Id_Residuo_Solido)
                .ToListAsync();
            /*
            foreach(var item in list_residuo_solido)
            {
                output.Add(new Dimension(item.Descripcion, 0, item.Id_Residuo_Solido));
            }
            */
            foreach (var item in registros)
            {
                //output.FirstOrDefault(x => x.Index.Equals(item.Key)).Valor = item.Sum(x=>x.Cantidad_RS_Evacuado);
                var res_solid = list_residuo_solido.FirstOrDefault(x => x.Id_Residuo_Solido.Equals(item.Key));
                output.Add(new Dimension(res_solid.Descripcion, item.Sum(x=>x.Cantidad_RS_Evacuado), item.Key));
            }
            return output;
        }
        public async Task<List<Dimension>> GetManifiestosTipoLlegada(DateTime FechaInicio, DateTime FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada)
        {
            var output = new List<Dimension>();
            //Tipo_Residuo = Peligroso (1)
            var list_residuo_solido = await _context.Residuos_Solidos.Where(x => x.Id_Tipo_Residuo.Equals(1)).ToListAsync();

            var registros = await _context.Manifiesto_RSs.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosResiduosSolidos.Pendiente.ToString()) &&
                x.Observacion_Manifiesto.Equals("Conforme") &&
                x.Id_Residuo_Solido.Equals(residuo_solido != 0 ? residuo_solido : x.Id_Residuo_Solido) &&
                x.Id_EO_RS_Salida.Equals(eors_salida != 0 ? eors_salida : x.Id_EO_RS_Salida) &&
                x.Id_EO_RS_Llegada.Equals(eors_llegada != 0 ? eors_llegada : x.Id_EO_RS_Llegada) &&
                x.Fecha_Emision_Manifiesto >= FechaInicio &&
                x.Fecha_Emision_Manifiesto <= FechaFin)
                .GroupBy(x => x.Id_Residuo_Solido)
                .ToListAsync();

            /*foreach (var item in list_residuo_solido)
            {
                output.Add(new Dimension(item.Descripcion, 0, item.Id_Residuo_Solido));
            }*/

            foreach (var item in registros)
            {
                //output.FirstOrDefault(x => x.Index.Equals(item.Key)).Valor = item.Sum(x => x.Cantidad_RS_Dispuesto);
                var res_solid = list_residuo_solido.FirstOrDefault(x => x.Id_Residuo_Solido.Equals(item.Key)); 
                output.Add(new Dimension(res_solid.Descripcion, item.Sum(x => x.Cantidad_RS_Dispuesto), item.Key));
            }
            return output;
        }
        public async Task<List<Dimension>> GetManifiestosResiduoSolidoMensual(DateTime FechaInicio, DateTime FechaFin, string observacion, int residuo_solido, int eors_salida, int eors_llegada)
        {
            var output = new List<Dimension>();
            //Tipo_Residuo = Peligroso (1)
            var list_residuo_solido = await _context.Residuos_Solidos.Where(x => x.Id_Tipo_Residuo.Equals(1)).ToListAsync();

            var registros = await _context.Manifiesto_RSs.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosResiduosSolidos.Pendiente.ToString()) &&
                x.Observacion_Manifiesto.Equals("Conforme") &&
                x.Id_Residuo_Solido.Equals(residuo_solido != 0 ? residuo_solido : x.Id_Residuo_Solido) &&
                x.Id_EO_RS_Salida.Equals(eors_salida != 0 ? eors_salida : x.Id_EO_RS_Salida) &&
                x.Id_EO_RS_Llegada.Equals(eors_llegada != 0 ? eors_llegada : x.Id_EO_RS_Llegada) &&
                x.Fecha_Emision_Manifiesto >= FechaInicio &&
                x.Fecha_Emision_Manifiesto <= FechaFin)
                .ToListAsync();

            CultureInfo cul = CultureInfo.CurrentCulture;
            var data = registros.Select(x => new { date = x.Fecha_Emision_Manifiesto.ToString("MM yyyy"), observacion = x.Observacion_Manifiesto, cantidad = x.Cantidad_RS_Evacuado, id = x.Id_Manifiesto_RS }).ToList();
            var frec = data.GroupBy(x => x.date).ToList();

            int nMeses;
            var lstMesSinValor = new List<Dimension>();
            var lstMesConValor = new List<Dimension>();

            if (FechaInicio.Year == FechaFin.Year)
            {
                nMeses = FechaFin.Month - FechaInicio.Month + 1;

                for (int i = 1; i <= nMeses; i++)
                {
                    var anio = Convert.ToInt32(FechaInicio.AddMonths(i - 1).ToString("MM yy").Substring(0, 2));
                    var mes = Convert.ToInt32(FechaInicio.AddMonths(i - 1).ToString("MM yy").Substring(2, 2));
                    var nombre = FechaInicio.AddMonths(i - 1).ToString("MMMM").Substring(0, 3) + " " + FechaInicio.AddMonths(i - 1).ToString("yyyy");
                    lstMesSinValor.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + i));
                }
            }
            else
            {
                int nAnios = FechaFin.Year - FechaInicio.Year + 1;

                int contador = 0;
                //Los primeros mese del último anio
                for (int i = FechaInicio.Month; i <= 12; i++)
                {
                    var nombre = FechaInicio.AddMonths(contador).ToString("MMMM").Substring(0, 3) + " " + FechaInicio.AddMonths(contador).ToString("yyyy");
                    lstMesSinValor.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + i));
                    contador++;
                }
                //Si son 3 años o más
                if (nAnios > 2)
                {
                    //Considerar todos los años menos el de la fecha inicial y final
                    for (int j = 1; j <= nAnios - 2; j++)
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            var fecha = new DateTime(FechaInicio.Year + j, i, 1);
                            var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");
                            lstMesSinValor.Add(new Dimension(nombre, 0, (FechaInicio.AddYears(j).Year * 100) + i));
                        }
                    }
                }
                //Los últimos meses del último anio
                for (int i = 1; i <= FechaFin.Month; i++)
                {
                    var fecha = new DateTime(FechaFin.Year, i, 1);
                    var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");
                    lstMesSinValor.Add(new Dimension(nombre, 0, (FechaFin.Year * 100) + i));
                }
            }

            foreach (var item in frec)
            {
                var mes = Convert.ToInt32(item.Key.Substring(0, 2));
                var anio = Convert.ToInt32(item.Key.Substring(3, 4));
                var fecha = new DateTime(anio, mes, 1);
                var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");

                if (item.Where(x => x.observacion.Equals("Conforme")).ToList().Count > 0)
                    lstMesConValor.Add(new Dimension(nombre, item.Where(x => x.observacion.Equals("Conforme")).Sum(x=>x.cantidad), anio * 100 + mes));

            }
            var distinctConforme = lstMesSinValor.Where(x => !lstMesConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
            output = distinctConforme.Union(lstMesConValor).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();

            return output;
        }
    }
}
