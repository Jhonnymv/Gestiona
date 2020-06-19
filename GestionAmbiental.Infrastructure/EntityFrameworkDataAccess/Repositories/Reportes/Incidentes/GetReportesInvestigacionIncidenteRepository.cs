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

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Reportes.Incidentes
{
    public class GetReportesInvestigacionIncidenteRepository : IGetReportesInvestigacionIncidenteReadOnlyRepository
    {
        private readonly Context _context;
        public GetReportesInvestigacionIncidenteRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Dimension>> GetNInvestigacion(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin, string frecuencia)
        {
            var output = new List<Dimension>();

            var registros = await _context.Investigacion_Final_Incidente.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .ToListAsync();

            if (frecuencia.Equals("Diario"))
            {
                var frec = registros.GroupBy(x => x.Fecha_Incidente.Value.Date).ToList();
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
                var data = registros.Select(x => new { woy = cul.Calendar.GetWeekOfYear(x.Fecha_Incidente.Value, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha_Incidente.Value.ToString("yyyy") }).ToList();
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
                var data = registros.Select(x => new { date = x.Fecha_Incidente.Value.ToString("MM yyyy") }).ToList();
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
        public async Task<List<Dimension>> GetNInvestigacionesTipoIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();
            var list_tipos_incidente = await _context.Tipo_Incidente.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var registros = await _context.Investigacion_Final_Incidente.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .GroupBy(x => x.Id_Tipo_Incidente)
                .ToListAsync();

            foreach (var item in registros)
            {
                var tipo_incidente = list_tipos_incidente.FirstOrDefault(x => x.Id_Tipo_Incidente.Equals(item.Key));
                output.Add(new Dimension(tipo_incidente.Descripcion, item.Count(), tipo_incidente.Id_Tipo_Incidente));
            }
            return output;
        }
        public async Task<List<Dimension>> GetNInvestigacionesNivelIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();
            var list_niveles_incidente = await _context.Nivel_Incidente.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var registros = await _context.Investigacion_Final_Incidente.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .GroupBy(x => x.Id_Nivel_Incidente)
                .ToListAsync();

            foreach (var item in list_niveles_incidente)
            {
                output.Add(new Dimension(item.Descripcion, 0, item.Id_Nivel_Incidente));
            }
            foreach (var item in registros)
            {
                output.FirstOrDefault(x => x.Index.Equals(item.Key)).Valor = item.Count();
            }
            return output;
        }
        public async Task<List<Dimension>> GetNInvestigacionesCausasInmediatasIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin) 
        {
            var output = new List<Dimension>();

            var list_tipos_condicion = await _context.Tipos_Condicion.ToListAsync();
            //Solo Causas Inmediatas (Id_Acto_Condicion != null)
            var list_causas = await _context.Causa_Incidente_Investigacion.Where(x => x.Estado.Equals("Registrado") && x.Id_Acto_Condicion != null).ToListAsync();

            var registros = await _context.Investigacion_Final_Incidente.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .Select(x => x.Id_Investigacion_Final)
                .ToListAsync();

            var list_causas_filtro = list_causas.Where(x => registros.Contains(x.Id_Investigacion_Final.Value)).GroupBy(x => x.Id_Acto_Condicion).ToList();

            foreach (var item in list_causas_filtro)
            {
                var tipo_condicion = list_tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Key));
                output.Add(new Dimension(tipo_condicion.Descripcion, item.Count(), tipo_condicion.Id_Tipo_Condicion));
            }
            return output;
        }
        public async Task<List<Dimension>> GetNInvestigacionesCausasBasicasIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin) 
        {
            var output = new List<Dimension>();

            var list_factor_causa = await _context.Factor_Causa_Incidente.ToListAsync();
            //Solo Causas Basicas (Id_Factor_Causa_Incidente != null)
            var list_causas = await _context.Causa_Incidente_Investigacion.Where(x => x.Estado.Equals("Registrado") && x.Id_Factor_Causa_Incidente != null).ToListAsync();

            var registros = await _context.Investigacion_Final_Incidente.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .Select(x => x.Id_Investigacion_Final)
                .ToListAsync();

            var list_causas_filtro = list_causas.Where(x => registros.Contains(x.Id_Investigacion_Final.Value)).GroupBy(x=>x.Id_Factor_Causa_Incidente).ToList();
            
            foreach (var item in list_causas_filtro)
            {
                var factor_causa = list_factor_causa.FirstOrDefault(x => x.Id_Factor_Causa_Incidente.Equals(item.Key));
                output.Add(new Dimension(factor_causa.Descripcion, item.Count(), factor_causa.Id_Factor_Causa_Incidente));
            }
            return output;
        }
        public async Task<List<Dimension>> GetNInvestigacionesGerenciaIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin) 
        {
            var output = new List<Dimension>();

            var gerencias = await _context.Gerencias.ToListAsync();

            var registros = await _context.Investigacion_Final_Incidente.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .GroupBy(x => x.Id_Gerencia_Responsable)
                .ToListAsync();

            foreach (var item in registros)
            {
                var gerencia_data = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Key));
                output.Add(new Dimension(gerencia_data.Descripcion, item.Count(), gerencia_data.Id_Gerencia));
            }
            return output;
        }
        public async Task<List<List<Dimension>>> GetCantidadProductoIncidente(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin) 
        {
            var output = new List<List<Dimension>>();

            var derramado = new List<Dimension>();
            var recuperado = new List<Dimension>();
            var no_recuperado = new List<Dimension>();

            var list_productos_incidente = await _context.Producto_Incidente.ToListAsync();

            var list_productos_derramado = await _context.Producto_Derramado_Incidente.Where(x=>x.Estado.Equals("Registrado")).ToListAsync();

            var registros = await _context.Investigacion_Final_Incidente.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .Select(x => x.Id_Investigacion_Final)
                .ToListAsync();

            var list_productos_derramado_filtro = list_productos_derramado.Where(x => registros.Contains(x.Id_Investigacion_Final)).GroupBy(x=>x.Id_Producto_Incidente).ToList();

            foreach (var item in list_productos_derramado_filtro)
            {
                var producto = list_productos_incidente.FirstOrDefault(x => x.Id_Producto_Incidente.Equals(item.Key));
                derramado.Add(new Dimension(producto.Descripcion, item.Sum(x=>x.Cantidad_Derramada.Value), producto.Id_Producto_Incidente));
                recuperado.Add(new Dimension(producto.Descripcion, item.Sum(x=>x.Cantidad_Recuperado.Value), producto.Id_Producto_Incidente));
                no_recuperado.Add(new Dimension(producto.Descripcion, item.Sum(x=>x.Cantidad_No_Recuperado.Value), producto.Id_Producto_Incidente));
            }

            output.Add(derramado);
            output.Add(recuperado);
            output.Add(no_recuperado);

            return output;
        }
        public async Task<List<Dimension>> GetImpactoSuelo(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin) 
        {
            var output = new List<Dimension>();

            var tipos_suelo = await _context.Tipo_Suelo.Where(x=>x.Estado.Equals("ACT")).ToListAsync();

            var impactos_suelo = await _context.Impacto_Ambiental_Incidente.Where(x => x.Area_Suelo != null).GroupBy(x=>x.Id_Tipo_Suelo).ToListAsync();

            foreach (var item in impactos_suelo)
            {
                var tipo_suelo = tipos_suelo.FirstOrDefault(x => x.Id_Tipo_Suelo.Equals(item.Key));
                output.Add(new Dimension(tipo_suelo.Descripcion, item.Sum(x=>x.Area_Suelo.Value), tipo_suelo.Id_Tipo_Suelo));
            }
            return output;
        }
        public async Task<List<Dimension>> GetResiduoImpactoSuelo(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin) 
        {
            var output = new List<Dimension>();

            var tipos_suelo = await _context.Tipo_Suelo.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var impactos_suelo = await _context.Impacto_Ambiental_Incidente.Where(x => x.Cantidad_Suelo != null).GroupBy(x => x.Id_Tipo_Suelo).ToListAsync();

            foreach (var item in impactos_suelo)
            {
                var tipo_suelo = tipos_suelo.FirstOrDefault(x => x.Id_Tipo_Suelo.Equals(item.Key));
                output.Add(new Dimension(tipo_suelo.Descripcion, item.Sum(x => x.Cantidad_Suelo.Value), tipo_suelo.Id_Tipo_Suelo));
            }
            return output;
        }
        public async Task<List<Dimension>> GetImpactoFlora(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin) 
        {
            var output = new List<Dimension>();

            var tipos_flora = await _context.Especie_Flora.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var impactos_flora = await _context.Impacto_Ambiental_Incidente.Where(x => x.Unidad_Flora != null).GroupBy(x => x.Id_Especie_Flora).ToListAsync();

            foreach (var item in impactos_flora)
            {
                var tipo_flora = tipos_flora.FirstOrDefault(x => x.Id_Especie_Flora.Equals(item.Key));
                output.Add(new Dimension(tipo_flora.Descripcion, item.Sum(x => x.Unidad_Flora.Value), tipo_flora.Id_Especie_Flora));
            }
            return output;
        }
        public async Task<List<Dimension>> GetResiduoImpactoFlora(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();

            var tipos_flora = await _context.Especie_Flora.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var impactos_flora = await _context.Impacto_Ambiental_Incidente.Where(x => x.Cantidad_Residuo_Flora != null).GroupBy(x => x.Id_Especie_Flora).ToListAsync();

            foreach (var item in impactos_flora)
            {
                var tipo_flora = tipos_flora.FirstOrDefault(x => x.Id_Especie_Flora.Equals(item.Key));
                output.Add(new Dimension(tipo_flora.Descripcion, item.Sum(x => x.Cantidad_Residuo_Flora.Value), tipo_flora.Id_Especie_Flora));
            }
            return output;
        }
        public async Task<List<Dimension>> GetImpactoFauna(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();

            var tipos_fauna = await _context.Especie_Fauna.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var impactos_fauna = await _context.Impacto_Ambiental_Incidente.Where(x => x.Unidad_Fauna != null).GroupBy(x => x.Id_Especie_Fauna).ToListAsync();

            foreach (var item in impactos_fauna)
            {
                var tipo_fauna = tipos_fauna.FirstOrDefault(x => x.Id_Especie_Fauna.Equals(item.Key));
                output.Add(new Dimension(tipo_fauna.Descripcion, item.Sum(x => x.Unidad_Fauna.Value), tipo_fauna.Id_Especie_Fauna));
            }
            return output;
        }
        public async Task<List<Dimension>> GetResiduoImpactoFauna(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();

            var tipos_fauna = await _context.Especie_Fauna.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var impactos_fauna = await _context.Impacto_Ambiental_Incidente.Where(x => x.Cantidad_Residuo_Fauna != null).GroupBy(x => x.Id_Especie_Fauna).ToListAsync();

            foreach (var item in impactos_fauna)
            {
                var tipo_fauna = tipos_fauna.FirstOrDefault(x => x.Id_Especie_Fauna.Equals(item.Key));
                output.Add(new Dimension(tipo_fauna.Descripcion, item.Sum(x => x.Cantidad_Residuo_Fauna.Value), tipo_fauna.Id_Especie_Fauna));
            }
            return output;
        }
        public async Task<List<Dimension>> GetImpactoAgua(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();

            var tipos_agua = await _context.Tipo_Agua.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var impactos_agua = await _context.Impacto_Ambiental_Incidente.Where(x => x.Volumen_Agua != null).GroupBy(x => x.Id_Tipo_Agua).ToListAsync();

            foreach (var item in impactos_agua)
            {
                var tipo_agua = tipos_agua.FirstOrDefault(x => x.Id_Tipo_Agua.Equals(item.Key));
                output.Add(new Dimension(tipo_agua.Descripcion, item.Sum(x => x.Volumen_Agua.Value), tipo_agua.Id_Tipo_Agua));
            }
            return output;
        }
        public async Task<List<Dimension>> GetResiduoImpactoAgua(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();

            var tipos_agua = await _context.Tipo_Agua.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var impactos_agua = await _context.Impacto_Ambiental_Incidente.Where(x => x.Cantidad_Residuo_Agua != null).GroupBy(x => x.Id_Tipo_Agua).ToListAsync();

            foreach (var item in impactos_agua)
            {
                var tipo_agua = tipos_agua.FirstOrDefault(x => x.Id_Tipo_Agua.Equals(item.Key));
                output.Add(new Dimension(tipo_agua.Descripcion, item.Sum(x => x.Cantidad_Residuo_Agua.Value), tipo_agua.Id_Tipo_Agua));
            }
            return output;
        }
        public async Task<List<Dimension>> GetImpactoPersona(int tipoIncidente, int nivelIncidente, int gerencia, int supervisor, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();

            var list_tipos_incidente = await _context.Tipo_Incidente.Where(x => x.Estado.Equals("ACT")).ToListAsync();

            var impactos = await _context.Impacto_Ambiental_Incidente.Where(x => x.Personas_Afectadas!=null).ToListAsync();

            var registros = await _context.Investigacion_Final_Incidente.Where(x =>
                //!x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Tipo_Incidente.Equals(tipoIncidente != 0 ? tipoIncidente : x.Id_Tipo_Incidente) &&
                x.Id_Nivel_Incidente.Equals(nivelIncidente != 0 ? nivelIncidente : x.Id_Nivel_Incidente) &&
                x.Id_Gerencia_Responsable.Equals(gerencia != 0 ? gerencia : x.Id_Gerencia_Responsable) &&
                x.Id_Supervisor_MA.Equals(supervisor != 0 ? supervisor : x.Id_Supervisor_MA) &&
                x.Fecha_Incidente >= FechaInicio &&
                x.Fecha_Incidente <= FechaFin)
                .GroupBy(x => x.Id_Tipo_Incidente)
                .ToListAsync();

            foreach (var item in registros)
            {
                var tipo_incidente = list_tipos_incidente.FirstOrDefault(x => x.Id_Tipo_Incidente.Equals(item.Key));
                var personas_afectadas = impactos.Where(x => item.Select(y => y.Id_Investigacion_Final).Contains(x.Id_Investigacion_Final)).Sum(x => x.Personas_Afectadas);
                if(personas_afectadas != null  && personas_afectadas !=0)
                    output.Add(new Dimension(tipo_incidente.Descripcion, personas_afectadas??personas_afectadas.Value, tipo_incidente.Id_Tipo_Incidente));
            }
            return output;
        }
    }
}
