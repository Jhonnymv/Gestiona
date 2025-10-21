using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Application.UseCases.Reportes.Incidentes;
using GestionAmbiental.Domain.Reportes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Reportes.Incidentes
{
    internal class GetReportesIncidentesReadOnlyRepository : IGetReportesIncidentesReadOnlyRepository
    {
        private readonly Context _context;

        public GetReportesIncidentesReadOnlyRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<ReporteIncidentes> GetNReportes(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin, string frecuencia)
        {
            var output = new ReporteIncidentes();

            var outputFlashReports = new List<Dimension>();
            var outputIncidentesMenores = new List<Dimension>();
            var outputIncidentesMayores = new List<Dimension>();

            var registrosIncidentesFlashReports = await _context.Flash_Report.Where(x =>
                x.Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Nivel_Incidente) &&
                x.Id_Gerencia.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia) &&
                x.Id_Supervisor_Marcobre.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_Marcobre) &&
                x.Fecha_Hora >= FechaInicio &&
                x.Fecha_Hora <= FechaFin)
                .ToListAsync();

            var registrosIncidentesMenores = await _context.Reporte_Preliminar_Incidente.Where(x =>
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .ToListAsync();


            var registrosIncidentesMayores = await _context.Investigacion_Final_Incidente.Where(x =>
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .ToListAsync();

            if (frecuencia.Equals("Diario"))
            {
                int nDias = 0;

                if (FechaFin.Year > FechaInicio.Year)
                    nDias = FechaFin.DayOfYear + (FechaInicio.Year % 4 == 0 ? 366 : 365) - FechaInicio.DayOfYear;
                else
                    nDias = FechaFin.DayOfYear - FechaInicio.DayOfYear;

                var lstDiasSinValorFlashReport = new List<Dimension>();
                var lstDiasConValorFlashReport = new List<Dimension>();

                var lstDiasSinValorIncidenteMenor = new List<Dimension>();
                var lstDiasConValorIncidenteMenor = new List<Dimension>();

                var lstDiasSinValorIncidenteMayor = new List<Dimension>();
                var lstDiasConValorIncidenteMayor = new List<Dimension>();

                for (int i = nDias; i >= 0; i--)
                {
                    var diaAnio = FechaFin.AddDays(-i).DayOfYear;
                    lstDiasSinValorFlashReport.Add(new Dimension(FechaFin.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, (FechaFin.AddDays(-i).Year * 1000) + diaAnio));
                    lstDiasSinValorIncidenteMenor.Add(new Dimension(FechaFin.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, (FechaFin.AddDays(-i).Year * 1000) + diaAnio));
                    lstDiasSinValorIncidenteMayor.Add(new Dimension(FechaFin.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, (FechaFin.AddDays(-i).Year * 1000) + diaAnio));
                }

                var frec1 = registrosIncidentesFlashReports.GroupBy(x => x.Fecha_Hora.Date).ToList();
                foreach (var item in frec1)
                {
                    var diaAnio = item.Key.DayOfYear;
                    lstDiasConValorFlashReport.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Count(), (item.Key.Year * 1000) + item.Key.DayOfYear));
                }

                var frec2 = registrosIncidentesMenores.GroupBy(x => x.Fecha_Incidente.Value.Date).ToList();
                foreach (var item in frec2)
                {
                    var diaAnio = item.Key.DayOfYear;
                    lstDiasConValorIncidenteMenor.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Count(), (item.Key.Year * 1000) + item.Key.DayOfYear));
                }
                var frec3 = registrosIncidentesMayores.GroupBy(x => x.Fecha_Incidente.Value.Date).ToList();
                foreach (var item in frec3)
                {
                    var diaAnio = item.Key.DayOfYear;
                    lstDiasConValorIncidenteMayor.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Count(), (item.Key.Year * 1000) + item.Key.DayOfYear));
                }
                var distinct = lstDiasSinValorFlashReport.Where(x => !lstDiasConValorFlashReport.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                outputFlashReports = distinct.Union(lstDiasConValorFlashReport).OrderBy(x => x.Index).ToList();

                distinct = lstDiasSinValorIncidenteMenor.Where(x => !lstDiasConValorIncidenteMenor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                outputIncidentesMenores = distinct.Union(lstDiasConValorIncidenteMenor).OrderBy(x => x.Index).ToList();

                distinct = lstDiasSinValorIncidenteMayor.Where(x => !lstDiasConValorIncidenteMayor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                outputIncidentesMayores = distinct.Union(lstDiasConValorIncidenteMayor).OrderBy(x => x.Index).ToList();

            }
            else if (frecuencia.Equals("Semanal"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;

                var dataFlashReports = registrosIncidentesFlashReports.Select(x => new { woy = cul.Calendar.GetWeekOfYear(x.Fecha_Hora, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha_Hora.ToString("yyyy") }).ToList();
                var frecFlashReports = dataFlashReports.GroupBy(x => x.woy).ToList();

                var dataIncidentesMenores = registrosIncidentesMenores.Select(x => new { woy = cul.Calendar.GetWeekOfYear(x.Fecha_Incidente.Value, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha_Incidente.Value.ToString("yyyy") }).ToList();
                var frecIncidentesMenores = dataIncidentesMenores.GroupBy(x => x.woy).ToList();

                var dataIncidentesMayores = registrosIncidentesMayores.Select(x => new { woy = cul.Calendar.GetWeekOfYear(x.Fecha_Incidente.Value, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha_Incidente.Value.ToString("yyyy") }).ToList();
                var frecIncidentesMayores = dataIncidentesMayores.GroupBy(x => x.woy).ToList();

                int nSemInicial = cul.Calendar.GetWeekOfYear(FechaInicio, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int nSemFinal = cul.Calendar.GetWeekOfYear(FechaFin, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                var lstSemSinValorFlashReport = new List<Dimension>();
                var lstSemConValorFlashReport = new List<Dimension>();

                var lstSemSinValorIncidenteMenor = new List<Dimension>();
                var lstSemConValorIncidenteMenor = new List<Dimension>();

                var lstSemSinValorIncidenteMayor = new List<Dimension>();
                var lstSemConValorIncidenteMayor = new List<Dimension>();

                int nSem = 0;
                //Mismo año
                if (FechaInicio.Year == FechaFin.Year)
                {
                    nSem = nSemFinal - nSemInicial;
                    for (int i = nSem; i >= 0; i--)
                    {
                        lstSemSinValorFlashReport.Add(new Dimension("Sem " + (nSemInicial + i).ToString() + " - " + FechaInicio.Year, 0, (FechaInicio.Year * 100) + nSemInicial + i));
                        lstSemSinValorIncidenteMenor.Add(new Dimension("Sem " + (nSemInicial + i).ToString() + " - " + FechaInicio.Year, 0, (FechaInicio.Year * 100) + nSemInicial + i));
                        lstSemSinValorIncidenteMayor.Add(new Dimension("Sem " + (nSemInicial + i).ToString() + " - " + FechaInicio.Year, 0, (FechaInicio.Year * 100) + nSemInicial + i));
                    }
                    foreach (var item in frecFlashReports)
                    {
                        var week = Convert.ToInt32(item.Key.Substring(0, 2));
                        var year = Convert.ToInt32(item.Key.Substring(3, 4));

                        lstSemConValorFlashReport.Add(new Dimension("Sem " + week + " - " + year, item.Count(), year * 100 + week));
                    }

                    foreach (var item in frecIncidentesMenores)
                    {
                        var week = Convert.ToInt32(item.Key.Substring(0, 2));
                        var year = Convert.ToInt32(item.Key.Substring(3, 4));

                        lstSemConValorIncidenteMenor.Add(new Dimension("Sem " + week + " - " + year, item.Count(), year * 100 + week));
                    }

                    foreach (var item in frecIncidentesMayores)
                    {
                        var week = Convert.ToInt32(item.Key.Substring(0, 2));
                        var year = Convert.ToInt32(item.Key.Substring(3, 4));

                        lstSemConValorIncidenteMayor.Add(new Dimension("Sem " + week + " - " + year, item.Count(), year * 100 + week));
                    }
                    var distinctIngreso = lstSemSinValorFlashReport.Where(x => !lstSemConValorFlashReport.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    outputFlashReports = distinctIngreso.Union(lstSemConValorFlashReport).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();

                    distinctIngreso = lstSemSinValorIncidenteMenor.Where(x => !lstSemConValorIncidenteMenor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    outputIncidentesMenores = distinctIngreso.Union(lstSemConValorIncidenteMenor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();

                    distinctIngreso = lstSemSinValorIncidenteMayor.Where(x => !lstSemConValorIncidenteMayor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    outputIncidentesMayores = distinctIngreso.Union(lstSemConValorIncidenteMayor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
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
                        lstSemSinValorFlashReport.Add(new Dimension("Sem " + (i).ToString() + " - " + FechaInicio.Year.ToString().Substring(2, 2), 0, (FechaInicio.Year * 100) + i));
                        lstSemSinValorIncidenteMenor.Add(new Dimension("Sem " + (i).ToString() + " - " + FechaInicio.Year.ToString().Substring(2, 2), 0, (FechaInicio.Year * 100) + i));
                        lstSemSinValorIncidenteMayor.Add(new Dimension("Sem " + (i).ToString() + " - " + FechaInicio.Year.ToString().Substring(2, 2), 0, (FechaInicio.Year * 100) + i));
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
                                lstSemSinValorFlashReport.Add(new Dimension("Sem " + (i).ToString() + " - " + FechaInicio.AddYears(j).Year.ToString().Substring(2, 2), 0, (FechaInicio.AddYears(j).Year * 100) + i));
                                lstSemSinValorIncidenteMenor.Add(new Dimension("Sem " + (i).ToString() + " - " + FechaInicio.AddYears(j).Year.ToString().Substring(2, 2), 0, (FechaInicio.AddYears(j).Year * 100) + i));
                                lstSemSinValorIncidenteMayor.Add(new Dimension("Sem " + (i).ToString() + " - " + FechaInicio.AddYears(j).Year.ToString().Substring(2, 2), 0, (FechaInicio.AddYears(j).Year * 100) + i));
                            }
                        }
                    }
                    //Las últimas semanas del último anio
                    for (int i = 1; i <= nSemFinal; i++)
                    {
                        lstSemSinValorFlashReport.Add(new Dimension("Sem " + (i).ToString() + " - " + FechaFin.Year.ToString().Substring(2, 2), 0, (FechaFin.Year * 100) + i));
                        lstSemSinValorIncidenteMenor.Add(new Dimension("Sem " + (i).ToString() + " - " + FechaFin.Year.ToString().Substring(2, 2), 0, (FechaFin.Year * 100) + i));
                        lstSemSinValorIncidenteMayor.Add(new Dimension("Sem " + (i).ToString() + " - " + FechaFin.Year.ToString().Substring(2, 2), 0, (FechaFin.Year * 100) + i));
                    }

                    int semAntFlashReport = Convert.ToInt32(frecFlashReports[0].Key.Substring(0, 2)) - 1;
                    int nAnio = 0;
                    foreach (var item in frecFlashReports)
                    {
                        var semAct = Convert.ToInt32(item.Key.Substring(0, 2));
                        var anioAct = Convert.ToInt32(item.Key.Substring(3, 4));
                        var nombre = "Sem " + semAct + " - " + anioAct.ToString().Substring(2, 2);
                        if (semAntFlashReport > semAct)
                            nAnio++;

                        lstSemConValorFlashReport.Add(new Dimension(nombre, item.Count(), (anioAct * 100) + semAct));

                        semAntFlashReport = semAct;
                    }

                    int semAntIncidenteMenores = Convert.ToInt32(frecIncidentesMenores[0].Key.Substring(0, 2)) - 1;
                    nAnio = 0;
                    foreach (var item in frecIncidentesMenores)
                    {
                        var semAct = Convert.ToInt32(item.Key.Substring(0, 2));
                        var anioAct = Convert.ToInt32(item.Key.Substring(3, 4));
                        var nombre = "Sem " + semAct + " - " + anioAct.ToString().Substring(2, 2);
                        if (semAntIncidenteMenores > semAct)
                            nAnio++;

                        lstSemConValorIncidenteMenor.Add(new Dimension(nombre, item.Count(), (anioAct * 100) + semAct));

                        semAntIncidenteMenores = semAct;
                    }

                    int semAntIncidenteMayor = Convert.ToInt32(frecIncidentesMayores[0].Key.Substring(0, 2)) - 1;
                    nAnio = 0;
                    foreach (var item in frecIncidentesMayores)
                    {
                        var semAct = Convert.ToInt32(item.Key.Substring(0, 2));
                        var anioAct = Convert.ToInt32(item.Key.Substring(3, 4));
                        var nombre = "Sem " + semAct + " - " + anioAct.ToString().Substring(2, 2);
                        if (semAntIncidenteMayor > semAct)
                            nAnio++;

                        lstSemConValorIncidenteMayor.Add(new Dimension(nombre, item.Count(), (anioAct * 100) + semAct));

                        semAntIncidenteMayor = semAct;
                    }

                    var distinctIngreso = lstSemSinValorFlashReport.Where(x => !lstSemConValorFlashReport.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    outputFlashReports = distinctIngreso.Union(lstSemConValorFlashReport).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    outputFlashReports = outputFlashReports.OrderBy(x => x.Index).ToList();

                    distinctIngreso = lstSemSinValorIncidenteMenor.Where(x => !lstSemConValorIncidenteMenor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    outputIncidentesMenores = distinctIngreso.Union(lstSemConValorIncidenteMenor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    outputIncidentesMenores = outputIncidentesMenores.OrderBy(x => x.Index).ToList();

                    distinctIngreso = lstSemSinValorIncidenteMayor.Where(x => !lstSemConValorIncidenteMayor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    outputIncidentesMayores = distinctIngreso.Union(lstSemConValorIncidenteMayor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    outputIncidentesMayores = outputIncidentesMayores.OrderBy(x => x.Index).ToList();
                }
            }
            else if (frecuencia.Equals("Mensual"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;

                var dataFlashReports = registrosIncidentesFlashReports.Select(x => new { date = x.Fecha_Hora.ToString("MM yyyy") }).ToList();
                var frecFlashReports = dataFlashReports.GroupBy(x => x.date).ToList();

                var dataIncidentesMenores = registrosIncidentesMenores.Select(x => new { date = x.Fecha_Incidente.Value.ToString("MM yyyy") }).ToList();
                var frecIncidentesMenores = dataIncidentesMenores.GroupBy(x => x.date).ToList();

                var dataIncidentesMayores = registrosIncidentesMayores.Select(x => new { date = x.Fecha_Incidente.Value.ToString("MM yyyy") }).ToList();
                var frecIncidentesMayores = dataIncidentesMayores.GroupBy(x => x.date).ToList();

                int nMeses;

                var lstMesSinValorFlashReports = new List<Dimension>();
                var lstMesConValorFlashReports = new List<Dimension>();

                var lstMesSinValorIncidentesMenores = new List<Dimension>();
                var lstMesConValorIncidentesMenores = new List<Dimension>();

                var lstMesSinValorIncidentesMayores = new List<Dimension>();
                var lstMesConValorIncidentesMayores = new List<Dimension>();

                if (FechaInicio.Year == FechaFin.Year)
                {
                    nMeses = FechaFin.Month - FechaInicio.Month + 1;

                    for (int i = 1; i <= nMeses; i++)
                    {
                        var mes = Convert.ToInt32(FechaInicio.AddMonths(i - 1).ToString("MM yy").Substring(0, 2));
                        var nombre = FechaInicio.AddMonths(i - 1).ToString("MMMM").Substring(0, 3) + " " + FechaInicio.AddMonths(i - 1).ToString("yyyy");
                        lstMesSinValorFlashReports.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + mes));
                        lstMesSinValorIncidentesMenores.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + mes));
                        lstMesSinValorIncidentesMayores.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + mes));
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
                        lstMesSinValorFlashReports.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + i));
                        lstMesSinValorIncidentesMenores.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + i));
                        lstMesSinValorIncidentesMayores.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100) + i));
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
                                lstMesSinValorFlashReports.Add(new Dimension(nombre, 0, (FechaInicio.AddYears(j).Year * 100) + i));
                                lstMesSinValorIncidentesMenores.Add(new Dimension(nombre, 0, (FechaInicio.AddYears(j).Year * 100) + i));
                                lstMesSinValorIncidentesMayores.Add(new Dimension(nombre, 0, (FechaInicio.AddYears(j).Year * 100) + i));
                            }
                        }
                    }
                    //Los últimos meses del último anio
                    for (int i = 1; i <= FechaFin.Month; i++)
                    {
                        var fecha = new DateTime(FechaFin.Year, i, 1);
                        var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");
                        lstMesSinValorFlashReports.Add(new Dimension(nombre, 0, (FechaFin.Year * 100) + i));
                        lstMesSinValorIncidentesMenores.Add(new Dimension(nombre, 0, (FechaFin.Year * 100) + i));
                        lstMesSinValorIncidentesMayores.Add(new Dimension(nombre, 0, (FechaFin.Year * 100) + i));
                    }
                }


                foreach (var item in frecFlashReports)
                {
                    var mes = Convert.ToInt32(item.Key.Substring(0, 2));
                    var anio = Convert.ToInt32(item.Key.Substring(3, 4));
                    var fecha = new DateTime(anio, mes, 1);
                    var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");

                    lstMesConValorFlashReports.Add(new Dimension(nombre, item.Count(), anio * 100 + mes));
                }

                foreach (var item in frecIncidentesMenores)
                {
                    var mes = Convert.ToInt32(item.Key.Substring(0, 2));
                    var anio = Convert.ToInt32(item.Key.Substring(3, 4));
                    var fecha = new DateTime(anio, mes, 1);
                    var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");

                    lstMesConValorIncidentesMenores.Add(new Dimension(nombre, item.Count(), anio * 100 + mes));
                }
                foreach (var item in frecIncidentesMayores)
                {
                    var mes = Convert.ToInt32(item.Key.Substring(0, 2));
                    var anio = Convert.ToInt32(item.Key.Substring(3, 4));
                    var fecha = new DateTime(anio, mes, 1);
                    var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");

                    lstMesConValorIncidentesMayores.Add(new Dimension(nombre, item.Count(), anio * 100 + mes));
                }
                var distinctIngreso = lstMesSinValorFlashReports.Where(x => !lstMesConValorFlashReports.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                outputFlashReports = distinctIngreso.Union(lstMesConValorFlashReports).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();

                distinctIngreso = lstMesSinValorIncidentesMenores.Where(x => !lstMesConValorIncidentesMenores.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                outputIncidentesMenores = distinctIngreso.Union(lstMesConValorIncidentesMenores).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();

                distinctIngreso = lstMesSinValorIncidentesMayores.Where(x => !lstMesConValorIncidentesMayores.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                outputIncidentesMayores = distinctIngreso.Union(lstMesConValorIncidentesMayores).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();
            }

            output.FlashReports = outputFlashReports;
            output.IncidentesMenores = outputIncidentesMenores;
            output.IncidentesMayores = outputIncidentesMayores;

            return output;
        }

        public async Task<ReporteIncidentes> GetNReportesCausasInmediatasIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new ReporteIncidentes();
            var outputIncidentesMenores = new List<Dimension>();
            var outputIncidentesMayores = new List<Dimension>();

            var list_tipos_condicion = await _context.Tipos_Condicion.ToListAsync();
            //Solo Causas Inmediatas (Id_Acto_Condicion != null)
            var list_causas_incidentesMenores = await _context.Causa_Incidente_Reporte.Where(x => x.Estado.Equals("Registrado") && x.Id_Acto_Condicion != null).ToListAsync();
            var list_causas_incidentesMayores = await _context.Causa_Incidente_Investigacion.Where(x => x.Estado.Equals("Registrado") && x.Id_Acto_Condicion != null).ToListAsync();

            var dataIncidentesMenores = await _context.Reporte_Preliminar_Incidente.ToListAsync();
            var registrosIncidentesMenores = dataIncidentesMenores.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .Select(x => x.Id_Reporte_Preliminar)
                .ToList();

            var dataIncidentesMayores = await _context.Investigacion_Final_Incidente.ToListAsync();
            var registrosIncidentesMayores = dataIncidentesMayores.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .Select(x => x.Id_Investigacion_Final)
                .ToList();

            var list_causas_filtro_menores = list_causas_incidentesMenores.Where(x => registrosIncidentesMenores.Contains(x.Id_Reporte_Preliminar)).GroupBy(x => x.Id_Acto_Condicion).ToList();

            foreach (var item in list_causas_filtro_menores)
            {
                var tipo_condicion = list_tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Key));
                outputIncidentesMenores.Add(new Dimension(tipo_condicion.Descripcion, item.Count(), tipo_condicion.Id_Tipo_Condicion));
            }

            var list_causas_filtro_mayores = list_causas_incidentesMayores.Where(x => registrosIncidentesMayores.Contains(x.Id_Investigacion_Final.Value)).GroupBy(x => x.Id_Acto_Condicion).ToList();

            foreach (var item in list_causas_filtro_mayores)
            {
                var tipo_condicion = list_tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Key));
                outputIncidentesMayores.Add(new Dimension(tipo_condicion.Descripcion, item.Count(), tipo_condicion.Id_Tipo_Condicion));
            }
            output.IncidentesMenores = outputIncidentesMenores;
            output.IncidentesMayores = outputIncidentesMayores;

            return output;
        }

        public async Task<ReporteIncidentes> GetNReportesCausasBasicasIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new ReporteIncidentes();
            var outputIncidentesMenores = new List<Dimension>();
            var outputIncidentesMayores = new List<Dimension>();

            var list_factor_causa = await _context.Factor_Causa_Incidente.ToListAsync();
            //Solo Causas Inmediatas (Id_Acto_Condicion != null)
            var list_causas_incidentesMenores = await _context.Causa_Incidente_Reporte.Where(x => x.Estado.Equals("Registrado") && x.Id_Factor_Causa_Incidente != null).ToListAsync();
            var list_causas_incidentesMayores = await _context.Causa_Incidente_Investigacion.Where(x => x.Estado.Equals("Registrado") && x.Id_Factor_Causa_Incidente != null).ToListAsync();

            var dataIncidentesMenores = await _context.Reporte_Preliminar_Incidente.ToListAsync();
            var registrosIncidentesMenores = dataIncidentesMenores.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .Select(x => x.Id_Reporte_Preliminar)
                .ToList();

            var dataIncidentesMayores = await _context.Investigacion_Final_Incidente.ToListAsync();
            var registrosIncidentesMayores = dataIncidentesMayores.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .Select(x => x.Id_Investigacion_Final)
                .ToList();

            var list_causas_filtro_menores = list_causas_incidentesMenores.Where(x => registrosIncidentesMenores.Contains(x.Id_Reporte_Preliminar)).GroupBy(x => x.Id_Factor_Causa_Incidente).ToList();

            foreach (var item in list_causas_filtro_menores)
            {
                var factor_causa = list_factor_causa.FirstOrDefault(x => x.Id_Factor_Causa_Incidente.Equals(item.Key));
                outputIncidentesMenores.Add(new Dimension(factor_causa.Descripcion, item.Count(), factor_causa.Id_Factor_Causa_Incidente));
            }

            var list_causas_filtro_mayores = list_causas_incidentesMayores.Where(x => registrosIncidentesMayores.Contains(x.Id_Investigacion_Final.Value)).GroupBy(x => x.Id_Factor_Causa_Incidente).ToList();

            foreach (var item in list_causas_filtro_mayores)
            {
                var factor_causa = list_factor_causa.FirstOrDefault(x => x.Id_Factor_Causa_Incidente.Equals(item.Key));
                outputIncidentesMayores.Add(new Dimension(factor_causa.Descripcion, item.Count(), factor_causa.Id_Factor_Causa_Incidente));
            }
            output.IncidentesMenores = outputIncidentesMenores;
            output.IncidentesMayores = outputIncidentesMayores;

            return output;
        }
        public async Task<ReporteIncidentes> GetNReportesGerencia(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {

            var output = new ReporteIncidentes();
            var outputFlashReports = new List<Dimension>();
            var outputIncidentesMenores = new List<Dimension>();
            var outputIncidentesMayores = new List<Dimension>();

            var gerencias = await _context.Gerencias.ToListAsync();

            var dataFlashReports = await _context.Flash_Report.ToListAsync();
            var registrosFlashReports = await _context.Flash_Report.Where(x =>
                x.Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Nivel_Incidente) &&
                x.Id_Gerencia.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia) &&
                x.Id_Supervisor_Marcobre.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_Marcobre) &&
                x.Fecha_Hora >= FechaInicio &&
                x.Fecha_Hora <= FechaFin)
                .GroupBy(x => x.Nivel_Incidente)
                .ToListAsync();

            var dataIncidentesMenores = await _context.Reporte_Preliminar_Incidente.ToListAsync();
            var registrosIncidentesMenores = dataIncidentesMenores.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .GroupBy(x => x.Id_Nivel_Incidente)
                .ToList();

            var dataIncidentesMayores = await _context.Investigacion_Final_Incidente.ToListAsync();
            var registrosIncidentesMayores = dataIncidentesMayores.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .GroupBy(x => x.Id_Gerencia_Responsable)
                .ToList();

            foreach (var item in registrosFlashReports)
            {
                var gerencia_data = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Key));
                outputFlashReports.Add(new Dimension(gerencia_data.Descripcion, item.Count(), gerencia_data.Id_Gerencia));
            }
            foreach (var item in registrosIncidentesMenores)
            {
                var gerencia_data = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Key));
                outputIncidentesMenores.Add(new Dimension(gerencia_data.Descripcion, item.Count(), gerencia_data.Id_Gerencia));
            }
            foreach (var item in registrosIncidentesMayores)
            {
                var gerencia_data = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Key));
                outputIncidentesMayores.Add(new Dimension(gerencia_data.Descripcion, item.Count(), gerencia_data.Id_Gerencia));
            }

            output.FlashReports = outputFlashReports.OrderByDescending(x => x.Valor).ToList();
            output.IncidentesMenores = outputIncidentesMenores.OrderByDescending(x => x.Valor).ToList();
            output.IncidentesMayores = outputIncidentesMayores.OrderByDescending(x => x.Valor).ToList();
            return output;
        }

        public async Task<ReporteIncidentes> GetNReportesNivelIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new ReporteIncidentes();
            var outputFlashReports = new List<Dimension>();
            var outputIncidentesMenores = new List<Dimension>();
            var outputIncidentesMayores = new List<Dimension>();
            var list_niveles_incidente = await _context.Nivel_Incidente.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var dataFlashReports = await _context.Flash_Report.ToListAsync();
            var registrosFlashReports = await _context.Flash_Report.Where(x =>
                x.Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Nivel_Incidente) &&
                x.Id_Gerencia.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia) &&
                x.Id_Supervisor_Marcobre.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_Marcobre) &&
                x.Fecha_Hora >= FechaInicio &&
                x.Fecha_Hora <= FechaFin)
                .GroupBy(x => x.Nivel_Incidente)
                .ToListAsync();

            var dataIncidentesMenores = await _context.Reporte_Preliminar_Incidente.ToListAsync();
            var registrosIncidentesMenores = dataIncidentesMenores.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .GroupBy(x => x.Id_Nivel_Incidente)
                .ToList();

            var dataIncidentesMayores = await _context.Investigacion_Final_Incidente.ToListAsync();
            var registrosIncidentesMayores = dataIncidentesMayores.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .GroupBy(x => x.Id_Nivel_Incidente)
                .ToList();

            foreach (var item in list_niveles_incidente)
            {
                outputFlashReports.Add(new Dimension(item.Descripcion, 0, item.Id_Nivel_Incidente));
                outputIncidentesMenores.Add(new Dimension(item.Descripcion, 0, item.Id_Nivel_Incidente));
                outputIncidentesMayores.Add(new Dimension(item.Descripcion, 0, item.Id_Nivel_Incidente));
            }
            foreach (var item in registrosFlashReports)
            {
                outputFlashReports.FirstOrDefault(x => x.Index.Equals(item.Key)).Valor = item.Count();
            }
            foreach (var item in registrosIncidentesMenores)
            {
                outputIncidentesMenores.FirstOrDefault(x => x.Index.Equals(item.Key)).Valor = item.Count();
            }
            foreach (var item in registrosIncidentesMayores)
            {
                outputIncidentesMayores.FirstOrDefault(x => x.Index.Equals(item.Key)).Valor = item.Count();
            }
            output.FlashReports = outputFlashReports.OrderByDescending(x => x.Valor).ToList();
            output.IncidentesMenores = outputIncidentesMenores.OrderByDescending(x => x.Valor).ToList();
            output.IncidentesMayores = outputIncidentesMayores.OrderByDescending(x => x.Valor).ToList();
            return output;
        }

        public async Task<ReporteIncidentes> GetNReportesTipoIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new ReporteIncidentes();
            var outputIncidentesMenores = new List<Dimension>();
            var outputIncidentesMayores = new List<Dimension>();
            var list_tipos_incidente = await _context.Tipo_Incidente.Where(x => x.Estado.Equals("ACT")).ToListAsync();


            var dataIncidentesMenores = await _context.Reporte_Preliminar_Incidente.ToListAsync();
            var registrosIncidentesMenores = dataIncidentesMenores.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                !x.Id_Tipo_Incidente.Equals(null) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .GroupBy(x => x.Id_Tipo_Incidente)
                .ToList();

            var dataIncidentesMayores = await _context.Investigacion_Final_Incidente.ToListAsync();
            var registrosIncidentesMayores = dataIncidentesMayores.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                !x.Id_Tipo_Incidente.Equals(null) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .GroupBy(x => x.Id_Tipo_Incidente)
                .ToList();

            foreach (var item in registrosIncidentesMenores)
            {
                var tipo_incidente = list_tipos_incidente.FirstOrDefault(x => x.Id_Tipo_Incidente.Equals(item.Key));
                outputIncidentesMenores.Add(new Dimension(tipo_incidente.Descripcion, item.Count(), tipo_incidente.Id_Tipo_Incidente));
            }
            foreach (var item in registrosIncidentesMayores)
            {
                var tipo_incidente = list_tipos_incidente.FirstOrDefault(x => x.Id_Tipo_Incidente.Equals(item.Key));
                outputIncidentesMayores.Add(new Dimension(tipo_incidente.Descripcion, item.Count(), tipo_incidente.Id_Tipo_Incidente));
            }
            output.IncidentesMenores = outputIncidentesMenores.OrderByDescending(x => x.Valor).ToList();
            output.IncidentesMayores = outputIncidentesMayores.OrderByDescending(x => x.Valor).ToList();

            return output;
        }
    }
}
