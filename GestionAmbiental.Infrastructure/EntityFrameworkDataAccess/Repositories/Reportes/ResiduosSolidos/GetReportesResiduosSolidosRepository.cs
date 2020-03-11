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
    public class GetReportesResiduosSolidosRepository : IGetReportesResiduosSolidosReadOnlyRepository
    {
        private readonly Context _context;

        public GetReportesResiduosSolidosRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Dimension>> GetNRegistrosEntreFechas(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa,string frecuencia)
        {
            var output = new List<Dimension>();
            var residuosSolido = await _context.Residuos_Solidos.Where(x=>
                x.Estado.Equals("ACT")&&
                x.Id_Tipo_Residuo.Equals(tipo_residuo!=0?tipo_residuo:x.Id_Tipo_Residuo))
                .Select(x=>x.Id_Residuo_Solido)
                .ToListAsync();
            var registros = await _context.Ingresos_Salidas_RS.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString())&&
                x.Id_ACR_CMRS.Equals(acr_cmrs != 0 ? acr_cmrs : x.Id_ACR_CMRS) &&
                x.Id_Ingreso_Salida.Equals(ingreso_salida != 0 ? ingreso_salida : x.Id_Ingreso_Salida) &&
                residuosSolido.Contains(x.Id_Residuo_Solido) &&
                x.Id_Motivo.Equals(motivo != 0 ? motivo : x.Id_Motivo) &&
                x.Id_Empresa_RS.Equals(empresa != 0 ? empresa : x.Id_Empresa_RS) &&
                x.Id_Responsable_Marcobre.Equals(responsable != 0 ? responsable : x.Id_Responsable_Marcobre) &&
                x.Fecha_Ingreso_Salida >= FechaInicio &&
                x.Fecha_Ingreso_Salida <= FechaFin)
                .ToListAsync();


            if (frecuencia.Equals("Diario"))
            {
                var frec = registros.GroupBy(x => x.Fecha_Ingreso_Salida.Date).ToList();
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
                    lstDiasConValor.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Sum(x=>x.Cantidad), (item.Key.Year * 1000) + item.Key.DayOfYear));
                }
                var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinct.Union(lstDiasConValor).OrderBy(x => x.Index).ToList();
            }
            else if (frecuencia.Equals("Semanal"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = registros.Select(x => new { woy = cul.Calendar.GetWeekOfYear(x.Fecha_Ingreso_Salida, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha_Ingreso_Salida.ToString("yyyy"),cantidad = x.Cantidad }).ToList();
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
                        lstSemSinValor.Add(new Dimension("Sem " + (nSemInicial + i).ToString() + " - " + FechaInicio.Year, 0, (FechaInicio.Year * 100) + nSemInicial+i));
                    }
                    foreach (var item in frec)
                    {
                        var week = Convert.ToInt32(item.Key.Substring(0, 2));
                        var year = Convert.ToInt32(item.Key.Substring(3, 4));
                        lstSemConValor.Add(new Dimension("Sem " + week + " - " + year, item.Sum(x=>x.cantidad), year * 100 + week));
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
                    int semAnt = Convert.ToInt32(frec[0].Key.Substring(0, 2)) - 1;
                    int nAnio = 0;
                    foreach (var item in frec)
                    {
                        var semAct = Convert.ToInt32(item.Key.Substring(0, 2));
                        var anioAct = Convert.ToInt32(item.Key.Substring(3, 4));
                        var nombre = "Sem " + semAct + " - " + anioAct.ToString().Substring(2, 2);
                        if (semAnt > semAct)
                            nAnio++;
                        //lstSemConValor.Add(new Dimension("Sem " + semAct + " - " + (FechaInicio.Year + nAnio).ToString().Substring(2, 2), item.Count(), ((FechaInicio.Year + nAnio) * 100) + item.Key));
                        lstSemConValor.Add(new Dimension(nombre, item.Sum(x=>x.cantidad), (anioAct * 100) + semAct));
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
                var data = registros.Select(x => new { date = x.Fecha_Ingreso_Salida.ToString("MM yyyy"), cantidad = x.Cantidad }).ToList();
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
                        lstMesSinValor.Add(new Dimension(nombre, 0, (FechaInicio.Year * 100)+i));
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
                    lstMesConValor.Add(new Dimension(nombre, item.Sum(x=>x.cantidad), anio * 100 + mes));
                }
                var distinct = lstMesSinValor.Where(x => !lstMesConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinct.Union(lstMesConValor).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();                
            }
            return output;
        }
        public async Task<List<Dimension>> GetNRegistrosACR_CMRS(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa)
        {
            var output = new List<Dimension>();
            var residuosSolido = await _context.Residuos_Solidos.Where(x =>
                x.Estado.Equals("ACT") &&
                x.Id_Tipo_Residuo.Equals(tipo_residuo != 0 ? tipo_residuo : x.Id_Tipo_Residuo))
                .Select(x => x.Id_Residuo_Solido)
                .ToListAsync();
            var registros = await _context.Ingresos_Salidas_RS.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                x.Id_ACR_CMRS.Equals(acr_cmrs != 0 ? acr_cmrs : x.Id_ACR_CMRS) &&
                x.Id_Ingreso_Salida.Equals(ingreso_salida != 0 ? ingreso_salida : x.Id_Ingreso_Salida) &&
                residuosSolido.Contains(x.Id_Residuo_Solido) &&
                x.Id_Motivo.Equals(motivo != 0 ? motivo : x.Id_Motivo) &&
                x.Id_Empresa_RS.Equals(empresa != 0 ? empresa : x.Id_Empresa_RS) &&
                x.Id_Responsable_Marcobre.Equals(responsable != 0 ? responsable : x.Id_Responsable_Marcobre) &&
                x.Fecha_Ingreso_Salida >= FechaInicio &&
                x.Fecha_Ingreso_Salida <= FechaFin)
                .GroupBy(x=>x.Id_ACR_CMRS)
                .ToListAsync();

            output.Add(new Dimension("ACR", 0, 1));
            output.Add(new Dimension("CMRS", 0, 2));
            foreach (var item in registros)
            {
                output.FirstOrDefault(x => x.Index.Equals(item.Key)).Valor = item.Sum(x=>x.Cantidad);
            }
            return output;
        }
        public async Task<List<Dimension>> GetNRegistrosIngresoSalida(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa)
        {
            var output = new List<Dimension>();
            var residuosSolido = await _context.Residuos_Solidos.Where(x =>
                x.Estado.Equals("ACT") &&
                x.Id_Tipo_Residuo.Equals(tipo_residuo != 0 ? tipo_residuo : x.Id_Tipo_Residuo))
                .Select(x => x.Id_Residuo_Solido)
                .ToListAsync();
            var registros = await _context.Ingresos_Salidas_RS.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                x.Id_ACR_CMRS.Equals(acr_cmrs != 0 ? acr_cmrs : x.Id_ACR_CMRS) &&
                x.Id_Ingreso_Salida.Equals(ingreso_salida != 0 ? ingreso_salida : x.Id_Ingreso_Salida) &&
                residuosSolido.Contains(x.Id_Residuo_Solido) &&
                x.Id_Motivo.Equals(motivo != 0 ? motivo : x.Id_Motivo) &&
                x.Id_Empresa_RS.Equals(empresa != 0 ? empresa : x.Id_Empresa_RS) &&
                x.Id_Responsable_Marcobre.Equals(responsable != 0 ? responsable : x.Id_Responsable_Marcobre) &&
                x.Fecha_Ingreso_Salida >= FechaInicio &&
                x.Fecha_Ingreso_Salida <= FechaFin)
                .GroupBy(x => x.Id_Ingreso_Salida)
                .ToListAsync();

            output.Add(new Dimension("Ingreso", 0, 1));
            output.Add(new Dimension("Salida", 0, 2));
            foreach (var item in registros)
            {
                output.FirstOrDefault(x => x.Index.Equals(item.Key)).Valor = item.Sum(x => x.Cantidad);
            }
            return output;
        }
        public async Task<List<Dimension>> GetNRegistrosMotivo(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa)
        {
            var output = new List<Dimension>();
            var residuosSolido = await _context.Residuos_Solidos.Where(x =>
                x.Estado.Equals("ACT") &&
                x.Id_Tipo_Residuo.Equals(tipo_residuo != 0 ? tipo_residuo : x.Id_Tipo_Residuo))
                .Select(x => x.Id_Residuo_Solido)
                .ToListAsync();
            var registros = await _context.Ingresos_Salidas_RS.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                x.Id_ACR_CMRS.Equals(acr_cmrs != 0 ? acr_cmrs : x.Id_ACR_CMRS) &&
                x.Id_Ingreso_Salida.Equals(ingreso_salida != 0 ? ingreso_salida : x.Id_Ingreso_Salida) &&
                residuosSolido.Contains(x.Id_Residuo_Solido) &&
                x.Id_Motivo.Equals(motivo != 0 ? motivo : x.Id_Motivo) &&
                x.Id_Empresa_RS.Equals(empresa != 0 ? empresa : x.Id_Empresa_RS) &&
                x.Id_Responsable_Marcobre.Equals(responsable != 0 ? responsable : x.Id_Responsable_Marcobre) &&
                x.Fecha_Ingreso_Salida >= FechaInicio &&
                x.Fecha_Ingreso_Salida <= FechaFin)
                .GroupBy(x => x.Id_Motivo)
                .ToListAsync();

            var motivos = await _context.Motivos_Ingreso_Salida.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach(var item in motivos)
            {
                output.Add(new Dimension(item.Descripcion, 0, item.Id_Motivo));
            }

            foreach (var item in registros)
            {
                output.FirstOrDefault(x => x.Index.Equals(item.Key)).Valor = item.Sum(x => x.Cantidad);
            }
            return output;
        }
        public async Task<List<Dimension>> GetNRegistrosTipoResiduo(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa)
        {
            var output = new List<Dimension>();
            var residuosSolido = await _context.Residuos_Solidos.Where(x =>
                x.Estado.Equals("ACT") &&
                x.Id_Tipo_Residuo.Equals(tipo_residuo != 0 ? tipo_residuo : x.Id_Tipo_Residuo))
                .ToListAsync();
            var rsAgrupados = residuosSolido.GroupBy(x => x.Id_Tipo_Residuo).ToList();
            var registros = await _context.Ingresos_Salidas_RS.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                x.Id_ACR_CMRS.Equals(acr_cmrs != 0 ? acr_cmrs : x.Id_ACR_CMRS) &&
                x.Id_Ingreso_Salida.Equals(ingreso_salida != 0 ? ingreso_salida : x.Id_Ingreso_Salida) &&
                residuosSolido.Select(y=>y.Id_Residuo_Solido).Contains(x.Id_Residuo_Solido) &&
                x.Id_Motivo.Equals(motivo != 0 ? motivo : x.Id_Motivo) &&
                x.Id_Empresa_RS.Equals(empresa != 0 ? empresa : x.Id_Empresa_RS) &&
                x.Id_Responsable_Marcobre.Equals(responsable != 0 ? responsable : x.Id_Responsable_Marcobre) &&
                x.Fecha_Ingreso_Salida >= FechaInicio &&
                x.Fecha_Ingreso_Salida <= FechaFin)
                .Select(x => new { tipo = rsAgrupados.FirstOrDefault(y=>y.Select(z=>z.Id_Residuo_Solido).Contains(x.Id_Residuo_Solido)).Key, Cantidad=x.Cantidad })
                .GroupBy(x=>x.tipo)
                .ToListAsync();

            output.Add(new Dimension("Peligroso", 0, 1));
            output.Add(new Dimension("No Peligroso", 0, 2));

            foreach (var item in registros)
            {
                output.FirstOrDefault(x => x.Index.Equals(item.Key)).Valor = item.Sum(x => x.Cantidad);
            }
            return output;
        }
    }
}
