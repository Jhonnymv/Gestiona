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

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Reportes.AvistamientoFauna
{
    public class GetReportesAvistamientoFaunaRepository: IGetReportesAvistamientoFaunaReadOnlyRepository
    {
        private readonly Context _context;

        public GetReportesAvistamientoFaunaRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Dimension>> GetNRegistrosEntreFechas(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, DateTime FechaInicio, DateTime FechaFin,string frecuencia)
        {
            var output = new List<Dimension>();

            var registros = await _context.Avistamientos_Fauna.Where(x =>
                !x.Estado.Equals(EstadosAvistamiento.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosIncidentes.Pendiente.ToString()) &&
                x.Id_Nombre_Comun_Fauna.Equals(nombre_comun != 0 ? nombre_comun : x.Id_Nombre_Comun_Fauna) &&
                x.Id_Tipo_Avistamiento.Equals(tipo_avistamiento != 0 ? tipo_avistamiento : x.Id_Tipo_Avistamiento) &&
                x.Id_Estado_Fauna.Equals(estado_fauna != 0 ? estado_fauna : x.Id_Estado_Fauna) &&
                x.Id_Lugar_Referencia.Equals(lugar_referencia != 0 ? lugar_referencia : x.Id_Lugar_Referencia) &&
                x.Fecha_Avistamiento >= FechaInicio &&
                x.Fecha_Avistamiento <= FechaFin)
                .ToListAsync();

            if (frecuencia.Equals("Diario"))
            {
                var frec = registros.GroupBy(x => x.Fecha_Avistamiento.Date).ToList();
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
                    lstDiasConValor.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Count(), (item.Key.Year * 1000) + item.Key.DayOfYear));
                }
                var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinct.Union(lstDiasConValor).OrderBy(x => x.Index).ToList();
            }
            else if (frecuencia.Equals("Semanal"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = registros.Select(x => new { woy = cul.Calendar.GetWeekOfYear(x.Fecha_Avistamiento, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha_Avistamiento.ToString("yyyy") }).ToList();
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

                        lstSemConValor.Add(new Dimension("Sem " + week + " - " + year, item.Count(), year * 100 + week));
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

                        lstSemConValor.Add(new Dimension(nombre, item.Count(), (anioAct * 100) + semAct));

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
                var data = registros.Select(x => new { date = x.Fecha_Avistamiento.ToString("MM yyyy") }).ToList();
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

                    lstMesConValor.Add(new Dimension(nombre, item.Count(), anio * 100 + mes));

                }
                var distinctIngreso = lstMesSinValor.Where(x => !lstMesConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinctIngreso.Union(lstMesConValor).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();
            }
            return output;
        }

        public async Task<List<Dimension>> GetNRegistrosEstadoFauna(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();
            var lista_estados_fauna = await _context.Estados_Fauna.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var registros = await _context.Avistamientos_Fauna.Where(x =>
                 !x.Estado.Equals(EstadosAvistamiento.Eliminado.ToString()) &&
                 !x.Estado.Equals(EstadosIncidentes.Pendiente.ToString()) &&
                 x.Id_Nombre_Comun_Fauna.Equals(nombre_comun != 0 ? nombre_comun : x.Id_Nombre_Comun_Fauna) &&
                 x.Id_Tipo_Avistamiento.Equals(tipo_avistamiento != 0 ? tipo_avistamiento : x.Id_Tipo_Avistamiento) &&
                 x.Id_Lugar_Referencia.Equals(lugar_referencia != 0 ? lugar_referencia : x.Id_Lugar_Referencia) &&
                 x.Fecha_Avistamiento >= FechaInicio &&
                 x.Fecha_Avistamiento <= FechaFin)
                .GroupBy(x =>x.Id_Estado_Fauna)
                .ToListAsync();
            foreach (var item in registros)
            {
                var estadoFauna = lista_estados_fauna.FirstOrDefault(x => x.Id_Estado_Fauna.Equals(item.Key));
                output.Add(new Dimension(estadoFauna.Descripcion, item.Count(), estadoFauna.Id_Estado_Fauna));
            }
            return output;
        }

        public async Task<List<Dimension>> GetNRegistrosNombreComun(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();
            var lista_nombres_comunes = await _context.Nombres_Comunes_Fauna.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var registros = await _context.Avistamientos_Fauna.Where(x =>
                 !x.Estado.Equals(EstadosAvistamiento.Eliminado.ToString()) &&
                 !x.Estado.Equals(EstadosIncidentes.Pendiente.ToString()) &&
                 x.Id_Tipo_Avistamiento.Equals(tipo_avistamiento != 0 ? tipo_avistamiento : x.Id_Tipo_Avistamiento) &&
                 x.Id_Estado_Fauna.Equals(estado_fauna != 0 ? estado_fauna : x.Id_Estado_Fauna) &&
                 x.Id_Lugar_Referencia.Equals(lugar_referencia != 0 ? lugar_referencia : x.Id_Lugar_Referencia) &&
                 x.Fecha_Avistamiento >= FechaInicio &&
                 x.Fecha_Avistamiento <= FechaFin)
                .GroupBy(x => x.Id_Nombre_Comun_Fauna)
                .ToListAsync();
            foreach (var item in registros)
            {
                var nombreComun = lista_nombres_comunes.FirstOrDefault(x => x.Id_Nombre_Comun_Fauna.Equals(item.Key));
                output.Add(new Dimension(nombreComun.Descripcion, item.Count(), nombreComun.Id_Nombre_Comun_Fauna));
            }
            return output;
        }

        public async Task<List<Dimension>> GetNRegistrosTipoAvistamiento(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();
            var lista_tipos_avistamiento = await _context.Tipos_Avistamiento.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var registros = await _context.Avistamientos_Fauna.Where(x =>
                 !x.Estado.Equals(EstadosAvistamiento.Eliminado.ToString()) &&
                 !x.Estado.Equals(EstadosIncidentes.Pendiente.ToString()) &&
                 x.Id_Nombre_Comun_Fauna.Equals(nombre_comun != 0 ? nombre_comun : x.Id_Nombre_Comun_Fauna) &&
                 x.Id_Estado_Fauna.Equals(estado_fauna != 0 ? estado_fauna : x.Id_Estado_Fauna) &&
                 x.Id_Lugar_Referencia.Equals(lugar_referencia != 0 ? lugar_referencia : x.Id_Lugar_Referencia) &&
                 x.Fecha_Avistamiento >= FechaInicio &&
                 x.Fecha_Avistamiento <= FechaFin)
                .GroupBy(x => x.Id_Tipo_Avistamiento)
                .ToListAsync();
            foreach (var item in registros)
            {
                var estadoFauna = lista_tipos_avistamiento.FirstOrDefault(x => x.Id_Tipo_Avistamiento.Equals(item.Key));
                output.Add(new Dimension(estadoFauna.Descripcion, item.Count(), estadoFauna.Id_Tipo_Avistamiento));
            }
            return output;
        }
    }
}
