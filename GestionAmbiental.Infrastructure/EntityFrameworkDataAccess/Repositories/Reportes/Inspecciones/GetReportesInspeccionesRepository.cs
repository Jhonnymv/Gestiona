using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Reportes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Reportes.Inspecciones
{
    public class GetReportesInspeccionesRepository : IGetReportesInspeccionesReadOnlyRepository
    {
        private Context _context;

        public GetReportesInspeccionesRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<Dimension>> GetNInspecciones(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin)
        {
            //Se considera también eliminados
            var observaciones = await _context.Inspecciones_Detalle.Where(x =>
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor) &&
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()))
                .Select(x => x.Id_Inspeccion_Cabecera)
                .Distinct()
                .ToListAsync();

            var temp_cabeceras = await _context.Inspecciones_Cabecera.Where(x => !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString())).ToListAsync();
            var cabeceras = temp_cabeceras.Where(x =>
                observaciones.Contains(x.Id_Inspeccion_Cabecera) &&
                x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FechaInicio &&
                x.Fecha_Inspeccion <= FechaFin)
                .GroupBy(x => x.Estado)
                .ToList();

            var output = new List<Dimension>();
            output.Add(new Dimension("Pendiente", 0, 0));
            output.Add(new Dimension("Registrado", 0, 1));
            output.Add(new Dimension("Total", temp_cabeceras.Count(), 2));

            foreach (var item in cabeceras)
            {
                output.FirstOrDefault(x => x.Nombre.Equals(item.Key)).Valor = item.Count();
            }
            return output;
        }
        public async Task<List<Dimension>> GetInspeccionesXFecha(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string frecuencia)
        {
            var output = new List<Dimension>();

            //Se considera también eliminados
            var observaciones = await _context.Inspecciones_Detalle.Where(x =>
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor) &&
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()))
                .Select(x => x.Id_Inspeccion_Cabecera)
                .Distinct()
                .ToListAsync();


            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                observaciones.Contains(x.Id_Inspeccion_Cabecera) &&
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FechaInicio &&
                x.Fecha_Inspeccion <= FechaFin)
                .ToListAsync();

            if (frecuencia.Equals("Diario"))
            {
                var frec_cabeceras = cabeceras.GroupBy(x => x.Fecha_Inspeccion.Date).ToList();
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
                foreach (var item in frec_cabeceras)
                {
                    var diaAnio = item.Key.DayOfYear;
                    lstDiasConValor.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Count(), (item.Key.Year * 1000) + item.Key.DayOfYear));
                }
                var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinct.Union(lstDiasConValor).OrderBy(x => x.Index).ToList();
            }
            else if (frecuencia.Equals("Semanal"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = cabeceras.Select(x => new { woy = cul.Calendar.GetWeekOfYear(x.Fecha_Inspeccion, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha_Inspeccion.ToString("yyyy") }).ToList();
                var frec_cabeceras = data.GroupBy(x => x.woy).ToList();

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
                        lstSemSinValor.Add(new Dimension("Sem " + (nSemInicial + i).ToString() + " - " + FechaInicio.Year, 0, nSemInicial + i));
                    }
                    foreach (var item in frec_cabeceras)
                    {
                        var week = Convert.ToInt32(item.Key.Substring(0, 2));
                        var year = Convert.ToInt32(item.Key.Substring(3, 4));
                        lstSemConValor.Add(new Dimension("Sem " + week + " - " + year, item.Count(), year * 100 + week));
                    }
                    var distinct = lstSemSinValor.Where(x => !lstSemConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    output = distinct.Union(lstSemConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    output = output.OrderBy(x => x.Index).ToList();
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
                    int semAnt = Convert.ToInt32(frec_cabeceras[0].Key.Substring(0, 2)) - 1;
                    int nAnio = 0;
                    foreach (var item in frec_cabeceras)
                    {
                        var semAct = Convert.ToInt32(item.Key.Substring(0, 2));
                        var anioAct = Convert.ToInt32(item.Key.Substring(3, 4));
                        var nombre = "Sem " + semAct + " - " + anioAct.ToString().Substring(2, 2);
                        if (semAnt > semAct)
                            nAnio++;
                        //lstSemConValor.Add(new Dimension("Sem " + semAct + " - " + (FechaInicio.Year + nAnio).ToString().Substring(2, 2), item.Count(), ((FechaInicio.Year + nAnio) * 100) + item.Key));
                        lstSemConValor.Add(new Dimension(nombre, item.Count(), (anioAct * 100) + semAct));
                        semAnt = semAct;
                    }
                    var distinct = lstSemSinValor.Where(x => !lstSemConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    output = distinct.Union(lstSemConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    output = output.OrderBy(x => x.Index).ToList();
                }
            }
            else if (frecuencia.Equals("Mensual"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = cabeceras.Select(x => new { date = x.Fecha_Inspeccion.ToString("MM yyyy") }).ToList();
                var frec_cabeceras = data.GroupBy(x => x.date).ToList();

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
                        lstMesSinValor.Add(new Dimension(nombre, 0, i));
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

                foreach (var item in frec_cabeceras)
                {
                    var mes = Convert.ToInt32(item.Key.Substring(0, 2));
                    var anio = Convert.ToInt32(item.Key.Substring(3, 4));
                    var fecha = new DateTime(anio, mes, 1);
                    var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");
                    lstMesConValor.Add(new Dimension(nombre, item.Count(), anio * 100 + mes));
                }
                var distinct = lstMesSinValor.Where(x => !lstMesConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinct.Union(lstMesConValor).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();
            }
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesXFecha(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, string frecuencia, EstadosInspeccion? estado)
        {
            var output = new List<Dimension>();

            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FechaInicio &&
                x.Fecha_Inspeccion <= FechaFin)
                .ToListAsync();
            var observaciones = await _context.Inspecciones_Detalle.ToListAsync();
            observaciones = observaciones.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                cabeceras.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
                x.Estado.Equals(estado.HasValue ? estado.Value.ToString() : x.Estado) &&
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor)).ToList();

            //Crear una lista de obs por cada inspeccion
            var obsXInsp = observaciones.GroupBy(x => x.Id_Inspeccion_Cabecera).ToList();
            if (frecuencia.Equals("Diario"))
            {
                //Agrupar inspecciones x fecha
                var frec_cabeceras = cabeceras.GroupBy(x => x.Fecha_Inspeccion.Date).ToList();

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
                if (obsXInsp.Count > 0)
                {
                    var frec_obs_x_fecha = new List<ObsXFecha>();
                    //Iterar cada fecha
                    foreach (var fecha in frec_cabeceras)
                    {
                        var contador = 0;
                        //Iterar las cabeceras de una fecha
                        foreach (var cabecera in fecha)
                        {
                            if (obsXInsp.FirstOrDefault(x => x.Key.Equals(cabecera.Id_Inspeccion_Cabecera)) != null)
                                contador += obsXInsp.FirstOrDefault(x => x.Key.Equals(cabecera.Id_Inspeccion_Cabecera)).Count();
                        }
                        frec_obs_x_fecha.Add(new ObsXFecha { Fecha = fecha.Key, Cantidad = contador });
                    }
                    foreach (var item in frec_obs_x_fecha)
                    {
                        var diaAnio = item.Fecha.DayOfYear;
                        lstDiasConValor.Add(new Dimension(item.Fecha.ToString("dd MMMM").Substring(0, 6), item.Cantidad, (item.Fecha.Year * 1000) + item.Fecha.DayOfYear));
                    }
                }
                var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinct.Union(lstDiasConValor).OrderBy(x => x.Index).ToList();
            }
            else if (frecuencia.Equals("Semanal"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = cabeceras.Select(x => new {
                    woy = cul.Calendar.GetWeekOfYear(x.Fecha_Inspeccion, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha_Inspeccion.ToString("yyyy"),
                    cabecera = x.Id_Inspeccion_Cabecera }).ToList();
                var frec_cabeceras = data.GroupBy(x => x.woy).ToList();

                //var frec_cabeceras = cabeceras.GroupBy(x => cul.Calendar.GetWeekOfYear(x.Fecha_Inspeccion, CalendarWeekRule.FirstDay, DayOfWeek.Monday)).ToList();
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
                        lstSemSinValor.Add(new Dimension("Sem " + (nSemInicial + i).ToString() + " - " + FechaInicio.Year, 0, nSemInicial + i));
                    }
                    if (obsXInsp.Count > 0)
                    {
                        //Iterar cada semana
                        foreach (var sem in frec_cabeceras)
                        {
                            var contador = 0;
                            //Iterar las cabeceras de cada semana
                            foreach (var cabecera in sem.Select(x => x.cabecera).ToList())
                            {
                                if (obsXInsp.FirstOrDefault(x => x.Key.Equals(cabecera)) != null)
                                    contador += obsXInsp.FirstOrDefault(x => x.Key.Equals(cabecera)).Count();
                            }
                            var semana = Convert.ToInt32(sem.Key.Substring(0, 2));
                            var anio = Convert.ToInt32(sem.Key.Substring(3, 4));
                            var nombre = "Sem " + semana.ToString() + " - " + anio.ToString();
                            lstSemConValor.Add(new Dimension(nombre, contador, (anio * 100) + semana));
                        }
                    }

                    var distinct = lstSemSinValor.Where(x => !lstSemConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    output = distinct.Union(lstSemConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    output = output.OrderBy(x => x.Index).ToList();
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
                    int semAnt = Convert.ToInt32(frec_cabeceras[0].Key.Substring(0, 2)) - 1;
                    int nAnio = 0;
                    foreach (var sem in frec_cabeceras)
                    {
                        var semAct = Convert.ToInt32(sem.Key.Substring(0, 2));
                        var anioAct = Convert.ToInt32(sem.Key.Substring(3, 4));
                        var nombre = "Sem " + semAct + " - " + anioAct.ToString().Substring(2, 2);
                        if (semAnt > semAct)
                            nAnio++;
                        //Iterar cada cabecera de la semana
                        var contador = 0;
                        foreach (var cabecera in sem.Select(x => x.cabecera).ToList())
                        {
                            if (obsXInsp.FirstOrDefault(x => x.Key.Equals(cabecera)) != null)
                                contador += obsXInsp.FirstOrDefault(x => x.Key.Equals(cabecera)).Count();
                        }
                        lstSemConValor.Add(new Dimension(nombre, contador, (anioAct * 100) + semAct));
                        semAnt = semAct;
                    }
                    var distinct = lstSemSinValor.Where(x => !lstSemConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    output = distinct.Union(lstSemConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    output = output.OrderBy(x => x.Index).ToList();
                }
            }
            else if (frecuencia.Equals("Mensual"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = cabeceras.Select(x => new { date = x.Fecha_Inspeccion.ToString("MM yyyy"), cabecera = x.Id_Inspeccion_Cabecera }).ToList();
                var frec_cabeceras = data.GroupBy(x => x.date).ToList();

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
                        lstMesSinValor.Add(new Dimension(nombre, 0, i));
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
                if (obsXInsp.Count > 0)
                {
                    //Iterar cada semana
                    foreach (var mes in frec_cabeceras)
                    {
                        var contador = 0;
                        //Iterar las cabeceras de cada semana
                        foreach (var cabecera in mes.Select(x => x.cabecera).ToList())
                        {
                            if (obsXInsp.FirstOrDefault(x => x.Key.Equals(cabecera)) != null)
                                contador += obsXInsp.FirstOrDefault(x => x.Key.Equals(cabecera)).Count();
                        }

                        var nMes = Convert.ToInt32(mes.Key.Substring(0, 2));
                        var anio = Convert.ToInt32(mes.Key.Substring(3, 4));
                        var fecha = new DateTime(anio, nMes, 1);
                        var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");
                        lstMesConValor.Add(new Dimension(nombre, contador, (anio * 100) + nMes));
                    }
                }
                var distinct = lstMesSinValor.Where(x => !lstMesConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinct.Union(lstMesConValor).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();
            }
            return output;
        }
        public async Task<List<Dimension>> GetNObservaciones(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FechaInicio &&
                x.Fecha_Inspeccion <= FechaFin)
                .ToListAsync();
            var tempObservaciones = await _context.Inspecciones_Detalle.Where(x => !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString())).ToListAsync();
            var observaciones = tempObservaciones.Where(x =>
                cabeceras.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
                x.Estado.Equals(estado.HasValue ? estado.Value.ToString() : x.Estado) &&
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor))
                .GroupBy(x => x.Estado)
                .ToList();
            //Se considera también eliminados            

            var output = new List<Dimension>();
            output.Add(new Dimension(EstadosInspeccion.Registrado.ToString(), 0, 0));
            output.Add(new Dimension(EstadosInspeccion.Implementado.ToString(), 0, 1));
            output.Add(new Dimension(EstadosInspeccion.Aprobado.ToString(), 0, 2));
            output.Add(new Dimension(EstadosInspeccion.Rechazado.ToString(), 0, 3));
            output.Add(new Dimension(EstadosInspeccion.Vencido.ToString(), 0, 4));
            output.Add(new Dimension(EstadosInspeccion.Ampliado.ToString(), 0, 5));
            output.Add(new Dimension("Total", tempObservaciones.Count(), 6));

            foreach (var item in observaciones)
            {
                output.FirstOrDefault(x => x.Nombre.Equals(item.Key)).Valor = item.Count();
            }
            return output;
        }
        public async Task<List<Dimension>> GetObsMAPvsSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                x.Fecha_Inspeccion >= FechaInicio &&
                x.Fecha_Inspeccion <= FechaFin)
                .ToListAsync();
            var tempObservaciones = await _context.Inspecciones_Detalle.ToListAsync();
            var observaciones = tempObservaciones.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                cabeceras.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
                x.Estado.Equals(estado.HasValue ? estado.Value.ToString() : x.Estado) &&
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor))
                .GroupBy(x => x.Tipo_Observacion)
                .ToList();
            var output = new List<Dimension>();
            output.Add(new Dimension("MAP", 0, 0));
            output.Add(new Dimension("SSO", 0, 1));
            foreach (var item in observaciones)
            {
                output.FirstOrDefault(x => x.Nombre.Equals(item.Key)).Valor = item.Count();
            }
            return output;
        }
        public async Task<List<List<Dimension>>> GetObsMAPvsSSO_Estado(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FechaInicio &&
                x.Fecha_Inspeccion <= FechaFin)
                .ToListAsync();
            var tempObservaciones = await _context.Inspecciones_Detalle.ToListAsync();
            var observaciones = tempObservaciones.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                cabeceras.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
                x.Estado.Equals(estado.HasValue ? estado.Value.ToString() : x.Estado) &&
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor))
                .GroupBy(x => x.Tipo_Observacion)
                .ToList();

            var output = new List<List<Dimension>>();
            var obsMAPxEstado = new List<Dimension>();
            var obsSSOxEstado = new List<Dimension>();

            obsMAPxEstado.Add(new Dimension("Pendientes de implementación", 0, 0));
            obsMAPxEstado.Add(new Dimension("Pendientes de verificación", 0, 1));
            obsMAPxEstado.Add(new Dimension("Cerradas", 0, 2));
            obsMAPxEstado.Add(new Dimension("Vencido", 0, 3));

            obsSSOxEstado.Add(new Dimension("Pendientes de implementación", 0, 0));
            obsSSOxEstado.Add(new Dimension("Pendientes de verificación", 0, 1));
            obsSSOxEstado.Add(new Dimension("Cerradas", 0, 2));
            obsSSOxEstado.Add(new Dimension("Vencido", 0, 3));

            foreach (var item in observaciones)
            {
                var obsXEstado = item.GroupBy(x => x.Estado).ToList();
                if (item.Key.Equals("MAP"))
                {
                    foreach (var subitem in obsXEstado)
                    {
                        switch (subitem.Key)
                        {
                            case "Registrado": obsMAPxEstado.FirstOrDefault(x => x.Index == 0).Valor += subitem.Count(); break;
                            case "Ampliado": obsMAPxEstado.FirstOrDefault(x => x.Index == 0).Valor += subitem.Count(); break;
                            case "Rechazado": obsMAPxEstado.FirstOrDefault(x => x.Index == 0).Valor += subitem.Count(); break;
                            case "Implementado": obsMAPxEstado.FirstOrDefault(x => x.Index == 1).Valor += subitem.Count(); break;
                            case "Aprobado": obsMAPxEstado.FirstOrDefault(x => x.Index == 2).Valor += subitem.Count(); break;
                            case "Vencido": obsMAPxEstado.FirstOrDefault(x => x.Index == 3).Valor += subitem.Count(); break;
                        }
                    }
                }
                else if (item.Key.Equals("SSO"))
                {
                    foreach (var subitem in obsXEstado)
                    {
                        switch (subitem.Key)
                        {
                            case "Registrado": obsSSOxEstado.FirstOrDefault(x => x.Index == 0).Valor += subitem.Count(); break;
                            case "Ampliado": obsSSOxEstado.FirstOrDefault(x => x.Index == 0).Valor += subitem.Count(); break;
                            case "Rechazado": obsSSOxEstado.FirstOrDefault(x => x.Index == 0).Valor += subitem.Count(); break;
                            case "Implementado": obsSSOxEstado.FirstOrDefault(x => x.Index == 1).Valor += subitem.Count(); break;
                            case "Aprobado": obsMAPxEstado.FirstOrDefault(x => x.Index == 2).Valor += subitem.Count(); break;
                            case "Vencido": obsSSOxEstado.FirstOrDefault(x => x.Index == 3).Valor += subitem.Count(); break;
                        }
                    }
                }
            }
            output.Add(obsMAPxEstado);
            output.Add(obsSSOxEstado);

            return output;
        }
        public async Task<List<Dimension>> GetObsNivelRiesgoMA(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FechaInicio &&
                x.Fecha_Inspeccion <= FechaFin)
                .ToListAsync();
            var tempObservaciones = await _context.Inspecciones_Detalle.ToListAsync();
            var observaciones = tempObservaciones.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                cabeceras.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
                x.Estado.Equals(estado.HasValue ? estado.Value.ToString() : x.Estado) &&
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor) &&
                x.Tipo_Observacion.Equals("MAP"))
                .GroupBy(x => x.Nivel_Riesgo)
                .ToList();

            var output = new List<Dimension>();

            output.Add(new Dimension("Tolerable", 0, 0));
            output.Add(new Dimension("Alerta", 0, 1));
            output.Add(new Dimension("No Aceptable", 0, 2));
            foreach (var item in observaciones)
            {
                switch (item.Key)
                {
                    case "Tolerable": output.FirstOrDefault(x => x.Nombre.Equals("Tolerable")).Valor = item.Count(); break;
                    case "Alerta": output.FirstOrDefault(x => x.Nombre.Equals("Alerta")).Valor = item.Count(); break;
                    case "No Aceptable": output.FirstOrDefault(x => x.Nombre.Equals("No Aceptable")).Valor = item.Count(); break;
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetObsNivelRiesgoSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FechaInicio &&
                x.Fecha_Inspeccion <= FechaFin)
                .ToListAsync();
            var tempObservaciones = await _context.Inspecciones_Detalle.ToListAsync();
            var observaciones = tempObservaciones.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                cabeceras.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
                x.Estado.Equals(estado.HasValue ? estado.Value.ToString() : x.Estado) &&
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor) &&
                x.Tipo_Observacion.Equals("SSO"))
                .GroupBy(x => x.Nivel_Riesgo)
                .ToList();

            var output = new List<Dimension>();

            output.Add(new Dimension("Bajo", 0, 0));
            output.Add(new Dimension("Medio", 0, 1));
            output.Add(new Dimension("Alto", 0, 2));
            foreach (var item in observaciones)
            {
                switch (item.Key)
                {
                    case "Bajo": output.FirstOrDefault(x => x.Nombre.Equals("Bajo")).Valor = item.Count(); break;
                    case "Medio": output.FirstOrDefault(x => x.Nombre.Equals("Medio")).Valor = item.Count(); break;
                    case "Alto": output.FirstOrDefault(x => x.Nombre.Equals("Alto")).Valor = item.Count(); break;
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetObsClasificacionMA(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
               x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
               x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
               x.Fecha_Inspeccion >= FechaInicio &&
               x.Fecha_Inspeccion <= FechaFin)
               .ToListAsync();
            var tempObservaciones = await _context.Inspecciones_Detalle.ToListAsync();
            var observaciones = tempObservaciones.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                cabeceras.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
                x.Estado.Equals(estado.HasValue ? estado.Value.ToString() : x.Estado) &&
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor) &&
                x.Tipo_Observacion.Equals("MAP"))
                .GroupBy(x => x.Descripcion_Condicion)
                .ToList();


            var output = new List<Dimension>();

            output.Add(new Dimension("Condición Subestándar", 0, 1));
            output.Add(new Dimension("Acto Subestándar", 0, 2));

            foreach (var item in observaciones)
            {
                switch (item.Key)
                {
                    case "Condición Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Condición Subestándar")).Valor = item.Count(); break;
                    case "Acto Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Acto Subestándar")).Valor = item.Count(); break;
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetObsClasificacionSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
               x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
               x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
               x.Fecha_Inspeccion >= FechaInicio &&
               x.Fecha_Inspeccion <= FechaFin)
               .ToListAsync();
            var tempObservaciones = await _context.Inspecciones_Detalle.ToListAsync();
            var observaciones = tempObservaciones.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                cabeceras.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
                x.Estado.Equals(estado.HasValue ? estado.Value.ToString() : x.Estado) &&
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor) &&
                x.Tipo_Observacion.Equals("SSO"))
                .GroupBy(x => x.Descripcion_Condicion)
                .ToList();


            var output = new List<Dimension>();

            output.Add(new Dimension("Condición Subestándar", 0, 1));
            output.Add(new Dimension("Acto Subestándar", 0, 2));

            foreach (var item in observaciones)
            {
                switch (item.Key)
                {
                    case "Condición Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Condición Subestándar")).Valor = item.Count(); break;
                    case "Acto Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Acto Subestándar")).Valor = item.Count(); break;
                }
            }
            return output;
        }
        public async Task<List<List<Dimension>>> GetObsTipoCondicionxNivelRiesgoMA(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
               x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
               x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
               x.Fecha_Inspeccion >= FechaInicio &&
               x.Fecha_Inspeccion <= FechaFin)
               .ToListAsync();
            var tempObservaciones = await _context.Inspecciones_Detalle.ToListAsync();
            var observaciones = tempObservaciones.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                cabeceras.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
                x.Estado.Equals(estado.HasValue ? estado.Value.ToString() : x.Estado) &&
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor) &&
                x.Tipo_Observacion.Equals("MAP"))
                .GroupBy(x => x.Nivel_Riesgo)
                .ToList();

            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();

            var output = new List<List<Dimension>>();
            var list_no_aceptable = new List<Dimension>();
            var list_alerta = new List<Dimension>();
            var list_tolerable = new List<Dimension>();

            foreach (var item in observaciones)
            {
                switch (item.Key)
                {
                    case "Tolerable":
                        var tipos_tolerable = item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
                        foreach (var subitem in tipos_tolerable)
                        {
                            var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Key));
                            list_tolerable.Add(new Dimension { Nombre = tipo_condicion.Descripcion, Valor = subitem.Count() });
                        }
                        break;
                    case "Alerta":
                        var tipos_alerta = item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
                        foreach (var subitem in tipos_alerta)
                        {
                            var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Key));
                            list_alerta.Add(new Dimension { Nombre = tipo_condicion.Descripcion, Valor = subitem.Count() });
                        }
                        break;
                    case "No Aceptable":
                        var tipos_no_aceptable = item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
                        foreach (var subitem in tipos_no_aceptable)
                        {
                            var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Key));
                            list_no_aceptable.Add(new Dimension { Nombre = tipo_condicion.Descripcion, Valor = subitem.Count() });
                        }
                        break;
                }
            }

            output.Add(list_tolerable);
            output.Add(list_alerta);
            output.Add(list_no_aceptable);

            return output;
        }
        public async Task<List<List<Dimension>>> GetObsTipoCondicionxNivelRiesgoSSO(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion? estado)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
               x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
               x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
               x.Fecha_Inspeccion >= FechaInicio &&
               x.Fecha_Inspeccion <= FechaFin)
               .ToListAsync();
            var tempObservaciones = await _context.Inspecciones_Detalle.ToListAsync();
            var observaciones = tempObservaciones.Where(x =>
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
                cabeceras.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
                x.Estado.Equals(estado.HasValue ? estado.Value.ToString() : x.Estado) &&
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor) &&
                x.Tipo_Observacion.Equals("SSO"))
                .GroupBy(x => x.Nivel_Riesgo)
                .ToList();

            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();

            var output = new List<List<Dimension>>();
            var list_alto = new List<Dimension>();
            var list_medio = new List<Dimension>();
            var list_bajo = new List<Dimension>();

            foreach (var item in observaciones)
            {
                switch (item.Key)
                {
                    case "Alto":
                        var tipos_alto = item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
                        foreach (var subitem in tipos_alto)
                        {
                            var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Key));
                            list_alto.Add(new Dimension { Nombre = tipo_condicion.Descripcion, Valor = subitem.Count() });
                        }
                        break;
                    case "Medio":
                        var tipos_medio = item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
                        foreach (var subitem in tipos_medio)
                        {
                            var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Key));
                            list_medio.Add(new Dimension { Nombre = tipo_condicion.Descripcion, Valor = subitem.Count() });
                        }
                        break;
                    case "Bajo":
                        var tipos_bajo = item.GroupBy(x => x.Id_Tipo_Condicion).ToList();
                        foreach (var subitem in tipos_bajo)
                        {
                            var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Key));
                            list_bajo.Add(new Dimension { Nombre = tipo_condicion.Descripcion, Valor = subitem.Count() });
                        }
                        break;
                }
            }

            output.Add(list_bajo);
            output.Add(list_medio);
            output.Add(list_alto);

            return output;
        }
        public async Task<List<Domain.Inspecciones.InspeccionCabecera>> GetDetalleObsxEstado(int empresa, int gerencia, int respLevObs, int Asesor, DateTime FechaInicio, DateTime FechaFin, EstadosInspeccion estado)
        {
            var output = new List<Domain.Inspecciones.InspeccionCabecera>();

            var observaciones = await _context.Inspecciones_Detalle.Where(x =>
                x.Estado.Equals(estado.ToString()) &&
                x.Id_Responsable_Marcobre.Equals(respLevObs > 0 ? respLevObs : x.Id_Responsable_Marcobre) &&
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor))
                .ToListAsync();

            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                observaciones.Select(y=>y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
                x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FechaInicio &&
                x.Fecha_Inspeccion <= FechaFin)
                .ToListAsync();

            var empresas = await _context.Empresas.ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();
            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();

            foreach (var item in cabeceras)
            {
                var obj_empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(item.Id_Empresa));
                var obj_gerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Id_Gerencia));
                var obs_item = observaciones.Where(x => x.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).ToList();
                var nObs = obs_item.Count;

                var outputItem = new Domain.Inspecciones.InspeccionCabecera();
                outputItem.Observaciones = new List<Domain.Inspecciones.InspeccionDetalle>();
                foreach (var subitem in obs_item)
                {
                    var obs = new Domain.Inspecciones.InspeccionDetalle();
                    
                    var responsable_obj = usuarios.FirstOrDefault(x=>x.Id_Usuario.Equals(subitem.Id_Responsable_Marcobre));
                    var asesor_obj = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(subitem.Id_Asesor));
                    var tipo_condicion_obj = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(subitem.Id_Tipo_Condicion));

                    obs.ResponsableMarcobre = new Domain.ValueObjects.Usuario(responsable_obj.Id_Usuario, responsable_obj.Id_Rol, responsable_obj.Nombres, responsable_obj.Apellidos, responsable_obj.Email);
                    obs.Asesor = new Domain.ValueObjects.Usuario(asesor_obj.Id_Usuario, asesor_obj.Id_Rol, asesor_obj.Nombres, asesor_obj.Apellidos, asesor_obj.Email);
                    obs.FechaProgramada = subitem.Fecha_Programada;
                    obs.NivelRiesgo = subitem.Nivel_Riesgo;
                    obs.TipoObservacion = subitem.Tipo_Observacion;
                    obs.TipoCondicion = new Domain.ValueObjects.TipoCondicion(tipo_condicion_obj.Id_Tipo_Condicion, tipo_condicion_obj.Descripcion, tipo_condicion_obj.Grupo);

                    outputItem.Observaciones.Add(obs);
                }
                outputItem.IdCabecera = item.Id_Inspeccion_Cabecera;
                outputItem.Codigo = item.Codigo;
                outputItem.Empresa = obj_empresa.Razon_Social;
                outputItem.Gerencia = obj_gerencia.Descripcion;
                outputItem.FechaInspeccion = item.Fecha_Inspeccion;

                outputItem.NObservaciones = nObs;

                output.Add(outputItem);
            }

            return output;
        }
        private class ObsXFecha
        {
            public DateTime Fecha { get; set; }
            public int Cantidad { get; set; }
        }
    }
}
