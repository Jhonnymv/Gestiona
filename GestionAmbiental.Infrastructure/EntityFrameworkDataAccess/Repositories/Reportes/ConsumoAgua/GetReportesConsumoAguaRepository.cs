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

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Reportes.ConsumoAgua
{
    public class GetReportesConsumoAguaRepository : IGetReportesConsumoAguaReadOnlyRepository
    {
        private readonly Context _context;

        public GetReportesConsumoAguaRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Dimension>> GetVolumenFecha(int resolucion, int origen, DateTime FechaInicio, DateTime FechaFin, string frecuencia)
        {
            var output = new List<Dimension>();

            var reg = await _context.Consumos_Agua.ToListAsync();
            var fuentes = await _context.Fuentes_Agua.Where(x => x.Id_Origen_Agua.Equals(origen)).Select(x => x.Id_Fuente_Agua).ToListAsync();
            var resoluciones = await _context.Resoluciones_Consumo_Agua.Where(x => fuentes.Contains(x.Id_Fuente_Agua)).Select(x => x.Id_Resolucion_Consumo_Agua).ToListAsync();

            if (resolucion != 0)
            {
                resoluciones = await _context.Resoluciones_Consumo_Agua.Where(x => x.Id_Resolucion_Consumo_Agua.Equals(resolucion)).Select(x => x.Id_Resolucion_Consumo_Agua).ToListAsync();
            }

            var registros = reg.Where(x =>
                !x.Estado.Equals(EstadosConsumo.Eliminado.ToString()) &&
                resoluciones.Contains(x.Id_Resolucion_Consumo_Agua) &&
                //x.Id_Resolucion_Consumo_Agua.Equals(resolucion != 0 ? resolucion : x.Id_Resolucion_Consumo_Agua) &&          
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin)
                .ToList();

            if (frecuencia.Equals("Diario"))
            {
                var frec = registros.GroupBy(x => x.Fecha.Date).ToList();
                int nDias = 0;

                if (FechaFin.Year > FechaInicio.Year)
                    nDias = FechaFin.DayOfYear + (FechaInicio.Year % 4 == 0 ? 366 : 365) - FechaInicio.DayOfYear;
                else
                    nDias = FechaFin.DayOfYear - FechaInicio.DayOfYear;

                var lstDiasSinValor = new List<Dimension>();
                var lstDiasConValor = new List<Dimension>();
                for (int i = nDias; i >= 0; i--)
                {
                    var diaAnio = FechaFin.AddDays(-i).DayOfYear;
                    lstDiasSinValor.Add(new Dimension(FechaFin.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, (FechaFin.AddDays(-i).Year * 1000) + diaAnio));
                }
                foreach (var item in frec)
                {
                    var diaAnio = item.Key.DayOfYear;
                    lstDiasConValor.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), Math.Round(item.Sum(x => x.Volumen_Bombeado),2), (item.Key.Year * 1000) + item.Key.DayOfYear));
                }
                var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinct.Union(lstDiasConValor).OrderBy(x => x.Index).ToList();
            }
            else if (frecuencia.Equals("Semanal"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = registros.Select(x => new { woy = cul.Calendar.GetWeekOfYear(x.Fecha, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha.ToString("yyyy"), volumen = x.Volumen_Bombeado }).ToList();
                var frec = data.GroupBy(x => x.woy).ToList();

                int nSemInicial = cul.Calendar.GetWeekOfYear(FechaInicio, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int nSemFinal = cul.Calendar.GetWeekOfYear(FechaFin, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                var lstSemSinValor = new List<Dimension>();
                var lstSemConValor = new List<Dimension>();

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

                        lstSemConValor.Add(new Dimension("Sem " + week + " - " + year, Math.Round(item.Sum(x => x.volumen),2), year * 100 + week));
                    }
                    var distinctIngreso = lstSemSinValor.Where(x => !lstSemConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    output = distinctIngreso.Union(lstSemConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
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

                        lstSemConValor.Add(new Dimension(nombre, Math.Round(item.Sum(x => x.volumen),2), (anioAct * 100) + semAct));

                        semAnt = semAct;
                    }
                    var distinctIngreso = lstSemSinValor.Where(x => !lstSemConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    output = distinctIngreso.Union(lstSemConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    output = output.OrderBy(x => x.Index).ToList();
                }
            }
            else if (frecuencia.Equals("Mensual"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = registros.Select(x => new { date = x.Fecha.ToString("MM yyyy"), volumen = x.Volumen_Bombeado }).ToList();
                var frec = data.GroupBy(x => x.date).ToList();

                int nMeses;
                var lstMesSinValor = new List<Dimension>();
                var lstMesConValor = new List<Dimension>();

                if (FechaInicio.Year == FechaFin.Year)
                {
                    nMeses = FechaFin.Month - FechaInicio.Month + 1;

                    for (int i = 1; i <= nMeses; i++)
                    {
                        var mes = Convert.ToInt32(FechaInicio.AddMonths(i - 1).ToString("MM yy").Substring(0, 2));
                        var nombre = FechaInicio.AddMonths(i - 1).ToString("MMMM").Substring(0, 3) + " " + FechaInicio.AddMonths(i - 1).ToString("yyyy");
                        lstMesSinValor.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + mes));
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

                    lstMesConValor.Add(new Dimension(nombre, Math.Round(item.Sum(x => x.volumen),2), anio * 100 + mes));

                }
                var distinctIngreso = lstMesSinValor.Where(x => !lstMesConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinctIngreso.Union(lstMesConValor).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();
            }
            return output;
        }

        public async Task<List<List<Dimension>>> GetVolumenRestante(int resolucion, int origen)
        {
            var output = new List<List<Dimension>>();
            var outputUsado = new List<Dimension>();
            var outputPorUsar = new List<Dimension>();

            var reg = await _context.Consumos_Agua.ToListAsync();
            var fuentes = await _context.Fuentes_Agua.Where(x => x.Id_Origen_Agua.Equals(origen)).Select(x => x.Id_Fuente_Agua).ToListAsync();
            var resoluciones = await _context.Resoluciones_Consumo_Agua.Where(x => fuentes.Contains(x.Id_Fuente_Agua)).ToListAsync();

            if (resolucion != 0)
            {
                resoluciones = await _context.Resoluciones_Consumo_Agua.Where(x => x.Id_Resolucion_Consumo_Agua.Equals(resolucion)).ToListAsync();
            }

            var registros = reg.Where(x =>
                !x.Estado.Equals(EstadosConsumo.Eliminado.ToString()) &&
                resoluciones.Select(y => y.Id_Resolucion_Consumo_Agua).Contains(x.Id_Resolucion_Consumo_Agua))
                .GroupBy(x => x.Id_Resolucion_Consumo_Agua)
                .ToList();

            foreach (var item in registros)
            {
                var resolucionConsumo = resoluciones.FirstOrDefault(x => x.Id_Resolucion_Consumo_Agua.Equals(item.Key));
                outputUsado.Add(new Dimension(resolucionConsumo.Resolucion, Math.Round(item.Sum(x => x.Volumen_Bombeado)), resolucionConsumo.Id_Resolucion_Consumo_Agua));
                outputPorUsar.Add(new Dimension(resolucionConsumo.Resolucion, Math.Round(resolucionConsumo.Volumen_Otorgado - item.Sum(x => x.Volumen_Bombeado),2), resolucionConsumo.Id_Resolucion_Consumo_Agua));
            }
            output.Add(outputUsado);
            output.Add(outputPorUsar);

            return output;
        }
        public async Task<List<Dimension>> GetCaudalPromedioFecha(int resolucion, int origen, DateTime FechaInicio, DateTime FechaFin, string frecuencia)
        {
            var output = new List<Dimension>();

            var reg = await _context.Consumos_Agua.ToListAsync();
            var fuentes = await _context.Fuentes_Agua.Where(x => x.Id_Origen_Agua.Equals(origen)).Select(x => x.Id_Fuente_Agua).ToListAsync();
            var resoluciones = await _context.Resoluciones_Consumo_Agua.Where(x => fuentes.Contains(x.Id_Fuente_Agua)).Select(x => x.Id_Resolucion_Consumo_Agua).ToListAsync();

            if (resolucion != 0)
            {
                resoluciones = await _context.Resoluciones_Consumo_Agua.Where(x => x.Id_Resolucion_Consumo_Agua.Equals(resolucion)).Select(x => x.Id_Resolucion_Consumo_Agua).ToListAsync();
            }

            var registros = reg.Where(x =>
                !x.Estado.Equals(EstadosConsumo.Eliminado.ToString()) &&
                resoluciones.Contains(x.Id_Resolucion_Consumo_Agua) &&
                //x.Id_Resolucion_Consumo_Agua.Equals(resolucion != 0 ? resolucion : x.Id_Resolucion_Consumo_Agua) &&          
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin)
                .ToList();

            if (frecuencia.Equals("Diario"))
            {
                var frec = registros.GroupBy(x => x.Fecha.Date).ToList();
                int nDias = 0;

                if (FechaFin.Year > FechaInicio.Year)
                    nDias = FechaFin.DayOfYear + (FechaInicio.Year % 4 == 0 ? 366 : 365) - FechaInicio.DayOfYear;
                else
                    nDias = FechaFin.DayOfYear - FechaInicio.DayOfYear;

                var lstDiasSinValor = new List<Dimension>();
                var lstDiasConValor = new List<Dimension>();
                for (int i = nDias; i >= 0; i--)
                {
                    var diaAnio = FechaFin.AddDays(-i).DayOfYear;
                    lstDiasSinValor.Add(new Dimension(FechaFin.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, (FechaFin.AddDays(-i).Year * 1000) + diaAnio));
                }
                foreach (var item in frec)
                {
                    var diaAnio = item.Key.DayOfYear;
                    var caudal =  Math.Round(item.Sum(x => x.Caudal_Promedio)/item.Count(),2);
                    lstDiasConValor.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), caudal, (item.Key.Year * 1000) + item.Key.DayOfYear));
                }
                var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinct.Union(lstDiasConValor).OrderBy(x => x.Index).ToList();
            }
            else if (frecuencia.Equals("Semanal"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = registros.Select(x => new { woy = cul.Calendar.GetWeekOfYear(x.Fecha, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha.ToString("yyyy"), caudal = x.Caudal_Promedio }).ToList();
                var frec = data.GroupBy(x => x.woy).ToList();

                int nSemInicial = cul.Calendar.GetWeekOfYear(FechaInicio, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int nSemFinal = cul.Calendar.GetWeekOfYear(FechaFin, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                var lstSemSinValor = new List<Dimension>();
                var lstSemConValor = new List<Dimension>();

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
                        var caudal = Math.Round(item.Sum(x => x.caudal) / item.Count(), 2);
                        lstSemConValor.Add(new Dimension("Sem " + week + " - " + year, caudal, year * 100 + week));
                    }
                    var distinctIngreso = lstSemSinValor.Where(x => !lstSemConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    output = distinctIngreso.Union(lstSemConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
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

                        var caudal = Math.Round(item.Sum(x => x.caudal) / item.Count(), 2);
                        lstSemConValor.Add(new Dimension(nombre, caudal, (anioAct * 100) + semAct));

                        semAnt = semAct;
                    }
                    var distinctIngreso = lstSemSinValor.Where(x => !lstSemConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    output = distinctIngreso.Union(lstSemConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    output = output.OrderBy(x => x.Index).ToList();
                }
            }
            else if (frecuencia.Equals("Mensual"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = registros.Select(x => new { date = x.Fecha.ToString("MM yyyy"), caudal = x.Caudal_Promedio }).ToList();
                var frec = data.GroupBy(x => x.date).ToList();

                int nMeses;
                var lstMesSinValor = new List<Dimension>();
                var lstMesConValor = new List<Dimension>();

                if (FechaInicio.Year == FechaFin.Year)
                {
                    nMeses = FechaFin.Month - FechaInicio.Month + 1;

                    for (int i = 1; i <= nMeses; i++)
                    {
                        var mes = Convert.ToInt32(FechaInicio.AddMonths(i - 1).ToString("MM yy").Substring(0, 2));
                        var nombre = FechaInicio.AddMonths(i - 1).ToString("MMMM").Substring(0, 3) + " " + FechaInicio.AddMonths(i - 1).ToString("yyyy");
                        lstMesSinValor.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + mes));
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

                    var caudal = Math.Round(item.Sum(x => x.caudal) / item.Count(), 2);
                    lstMesConValor.Add(new Dimension(nombre, caudal, anio * 100 + mes));

                }
                var distinctIngreso = lstMesSinValor.Where(x => !lstMesConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinctIngreso.Union(lstMesConValor).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();
            }
            return output;            
        }
        public async Task<List<Dimension>> GetTiempoBombeoFecha(int resolucion, int origen, DateTime FechaInicio, DateTime FechaFin, string frecuencia)
        {
            var output = new List<Dimension>();

            var reg = await _context.Consumos_Agua.ToListAsync();
            var fuentes = await _context.Fuentes_Agua.Where(x => x.Id_Origen_Agua.Equals(origen)).Select(x => x.Id_Fuente_Agua).ToListAsync();
            var resoluciones = await _context.Resoluciones_Consumo_Agua.Where(x => fuentes.Contains(x.Id_Fuente_Agua)).Select(x => x.Id_Resolucion_Consumo_Agua).ToListAsync();

            if (resolucion != 0)
            {
                resoluciones = await _context.Resoluciones_Consumo_Agua.Where(x => x.Id_Resolucion_Consumo_Agua.Equals(resolucion)).Select(x => x.Id_Resolucion_Consumo_Agua).ToListAsync();
            }

            var registros = reg.Where(x =>
                !x.Estado.Equals(EstadosConsumo.Eliminado.ToString()) &&
                resoluciones.Contains(x.Id_Resolucion_Consumo_Agua) &&
                //x.Id_Resolucion_Consumo_Agua.Equals(resolucion != 0 ? resolucion : x.Id_Resolucion_Consumo_Agua) &&          
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin)
                .ToList();

            if (frecuencia.Equals("Diario"))
            {
                var frec = registros.GroupBy(x => x.Fecha.Date).ToList();
                int nDias = 0;

                if (FechaFin.Year > FechaInicio.Year)
                    nDias = FechaFin.DayOfYear + (FechaInicio.Year % 4 == 0 ? 366 : 365) - FechaInicio.DayOfYear;
                else
                    nDias = FechaFin.DayOfYear - FechaInicio.DayOfYear;

                var lstDiasSinValor = new List<Dimension>();
                var lstDiasConValor = new List<Dimension>();
                for (int i = nDias; i >= 0; i--)
                {
                    var diaAnio = FechaFin.AddDays(-i).DayOfYear;
                    lstDiasSinValor.Add(new Dimension(FechaFin.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, (FechaFin.AddDays(-i).Year * 1000) + diaAnio));
                }
                foreach (var item in frec)
                {
                    var diaAnio = item.Key.DayOfYear;
                    var tiempoBombeo = item.Sum(x => x.Tiempo_Bombeo);
                    lstDiasConValor.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), Math.Round(tiempoBombeo, 2), (item.Key.Year * 1000) + item.Key.DayOfYear));
                }
                var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinct.Union(lstDiasConValor).OrderBy(x => x.Index).ToList();
            }
            else if (frecuencia.Equals("Semanal"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = registros.Select(x => new { woy = cul.Calendar.GetWeekOfYear(x.Fecha, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha.ToString("yyyy"), Tiempo_Bombeo = x.Tiempo_Bombeo }).ToList();
                var frec = data.GroupBy(x => x.woy).ToList();

                int nSemInicial = cul.Calendar.GetWeekOfYear(FechaInicio, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int nSemFinal = cul.Calendar.GetWeekOfYear(FechaFin, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                var lstSemSinValor = new List<Dimension>();
                var lstSemConValor = new List<Dimension>();

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
                        var tiempoBombeo = item.Sum(x => x.Tiempo_Bombeo);
                        var nRegistros = item.Count();
                        lstSemConValor.Add(new Dimension("Sem " + week + " - " + year, Math.Round(tiempoBombeo, 2), year * 100 + week));
                    }
                    var distinctIngreso = lstSemSinValor.Where(x => !lstSemConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    output = distinctIngreso.Union(lstSemConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
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
                        var tiempoBombeo = item.Sum(x => x.Tiempo_Bombeo);
                        var nRegistros = item.Count();
                        lstSemConValor.Add(new Dimension(nombre, Math.Round(tiempoBombeo, 2), (anioAct * 100) + semAct));

                        semAnt = semAct;
                    }
                    var distinctIngreso = lstSemSinValor.Where(x => !lstSemConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    output = distinctIngreso.Union(lstSemConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    output = output.OrderBy(x => x.Index).ToList();
                }
            }
            else if (frecuencia.Equals("Mensual"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = registros.Select(x => new { date = x.Fecha.ToString("MM yyyy"), Tiempo_Bombeo = x.Tiempo_Bombeo }).ToList();
                var frec = data.GroupBy(x => x.date).ToList();

                int nMeses;
                var lstMesSinValor = new List<Dimension>();
                var lstMesConValor = new List<Dimension>();

                if (FechaInicio.Year == FechaFin.Year)
                {
                    nMeses = FechaFin.Month - FechaInicio.Month + 1;

                    for (int i = 1; i <= nMeses; i++)
                    {
                        var mes = Convert.ToInt32(FechaInicio.AddMonths(i - 1).ToString("MM yy").Substring(0, 2));
                        var nombre = FechaInicio.AddMonths(i - 1).ToString("MMMM").Substring(0, 3) + " " + FechaInicio.AddMonths(i - 1).ToString("yyyy");
                        lstMesSinValor.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + mes));
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
                    var tiempoBombeo = item.Sum(x => x.Tiempo_Bombeo);
                    lstMesConValor.Add(new Dimension(nombre, Math.Round(tiempoBombeo, 2), anio * 100 + mes));

                }
                var distinctIngreso = lstMesSinValor.Where(x => !lstMesConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinctIngreso.Union(lstMesConValor).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();
            }
            return output;            

        }
    }
}
