using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral;
using GestionAmbiental.Domain.DashboardGeneral.ResiduosSolidos.IngresoSalida;
using GestionAmbiental.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.DashboardGeneral
{
    public class GetDataIngresoSalidaResiduosDashboardRepository : IGetDataIngresoSalidaResiduosDashboardReadOnlyRepository
    {
        private readonly Context _context;

        public GetDataIngresoSalidaResiduosDashboardRepository(Context context)
        {
            _context = context;
        }

        //Obtener la cantidad de INGRESO o SALIDA según el NOMBRE DE RESIDUO
        public async Task<List<NombreResiduoxIngresoSalida>> GetNombreResiduoxIngresoSalida(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa)
        {
            var output = new List<NombreResiduoxIngresoSalida>();

            var residuosSolido = await _context.Residuos_Solidos.Where(x =>
                x.Id_Tipo_Residuo.Equals(tipo_residuo != 0 ? tipo_residuo : x.Id_Tipo_Residuo))                
                .ToListAsync();

            var registros = await _context.Ingresos_Salidas_RS.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosResiduosSolidos.Pendiente.ToString()) &&
                x.Id_ACR_CMRS.Equals(acr_cmrs != 0 ? acr_cmrs : x.Id_ACR_CMRS) &&
                x.Id_Ingreso_Salida.Equals(ingreso_salida != 0 ? ingreso_salida : x.Id_Ingreso_Salida) &&
                residuosSolido.Select(y => y.Id_Residuo_Solido).Contains(x.Id_Residuo_Solido) &&
                x.Id_Motivo.Equals(motivo != 0 ? motivo : x.Id_Motivo) &&
                x.Id_Empresa_RS.Equals(empresa != 0 ? empresa : x.Id_Empresa_RS) &&
                x.Id_Gerencia.Equals(responsable != 0 ? responsable : x.Id_Gerencia) &&
                x.Fecha_Ingreso_Salida >= FechaInicio &&
                x.Fecha_Ingreso_Salida <= FechaFin)
                .ToListAsync();

            var registrosxNombreResiduo = registros.GroupBy(x => x.Id_Residuo_Solido).ToList();

            foreach (var item in registrosxNombreResiduo)
            {
                var nombreResiduo = residuosSolido.Find(x => x.Id_Residuo_Solido.Equals(item.Key));
                var nombreItem = $"{nombreResiduo.Descripcion} ({(nombreResiduo.Id_Tipo_Residuo==1?"P":"NP")})";

                output.Add(new NombreResiduoxIngresoSalida{
                    NombreResiduo = nombreItem,
                    Ingreso = item.Where(x => x.Id_Ingreso_Salida.Equals(1)).Sum(x => x.Cantidad),
                    Salida = item.Where(x => x.Id_Ingreso_Salida.Equals(2)).Sum(x => x.Cantidad)
                });
            }
            output = output.OrderByDescending(x => x.Ingreso + x.Salida).ToList();
            return output;
        }
        //Obtener la cantidad de ingreso o salida según el P y NP (Peligroso / No Peligroso)
        public async Task<IngresoSalidaPyNP> GetCantidadIngresoSalidaxPyNP(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa, string frecuencia)
        {
            var output = new IngresoSalidaPyNP();
            var ingresoP = new List<Dimension>();
            var ingresoNP = new List<Dimension>();
            var salidaP = new List<Dimension>();
            var salidaNP = new List<Dimension>();

            var residuosSolido = await _context.Residuos_Solidos.Where(x =>
                x.Estado.Equals("ACT") &&
                x.Id_Tipo_Residuo.Equals(tipo_residuo != 0 ? tipo_residuo : x.Id_Tipo_Residuo))
                .Select(y => y.Id_Residuo_Solido)
                .ToListAsync();

            var residuosPeligrosos = await _context.Residuos_Solidos.Where(x =>
                //x.Estado.Equals("ACT")&&
                x.Id_Tipo_Residuo.Equals(1))
                .Select(y => y.Id_Residuo_Solido)
                .ToListAsync();
            var residuosNoPeligrosos = await _context.Residuos_Solidos.Where(x =>
                //x.Estado.Equals("ACT")&&
                x.Id_Tipo_Residuo.Equals(2))
                .Select(y => y.Id_Residuo_Solido)
                .ToListAsync();

            var registros = await _context.Ingresos_Salidas_RS.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosResiduosSolidos.Pendiente.ToString()) &&
                x.Id_ACR_CMRS.Equals(acr_cmrs != 0 ? acr_cmrs : x.Id_ACR_CMRS) &&
                x.Id_Ingreso_Salida.Equals(ingreso_salida != 0 ? ingreso_salida : x.Id_Ingreso_Salida) &&
                residuosSolido.Contains(x.Id_Residuo_Solido) &&
                x.Id_Motivo.Equals(motivo != 0 ? motivo : x.Id_Motivo) &&
                x.Id_Empresa_RS.Equals(empresa != 0 ? empresa : x.Id_Empresa_RS) &&
                x.Id_Gerencia.Equals(responsable != 0 ? responsable : x.Id_Gerencia) &&
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
                //var lstDiasConValorIngreso = new List<Dimension>();
                var lstDiasConValorIngresoP = new List<Dimension>();
                var lstDiasConValorIngresoNP = new List<Dimension>();
                //var lstDiasConValorSalida = new List<Dimension>();
                var lstDiasConValorSalidaP = new List<Dimension>();
                var lstDiasConValorSalidaNP = new List<Dimension>();
                for (int i = nDias; i >= 0; i--)
                {
                    var diaAnio = FechaFin.AddDays(-i).DayOfYear;
                    lstDiasSinValor.Add(new Dimension(FechaFin.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, (FechaFin.AddDays(-i).Year * 1000) + diaAnio));
                }
                foreach (var item in frec)
                {
                    var diaAnio = item.Key.DayOfYear;
                    /* Checking if the list contains any items with Id_Ingreso_Salida = 1 and
                    Id_Residuo_Solido in the list of residuosPeligrosos. */
                    if (item.Where(x => x.Id_Ingreso_Salida.Equals(1)).ToList().Count > 0 && item.Where(x => residuosPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                        lstDiasConValorIngresoP.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Where(x => x.Id_Ingreso_Salida.Equals(1) && residuosPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), (item.Key.Year * 1000) + item.Key.DayOfYear));

                    if (item.Where(x => x.Id_Ingreso_Salida.Equals(1)).ToList().Count > 0 && item.Where(x => residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                        lstDiasConValorIngresoNP.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Where(x => x.Id_Ingreso_Salida.Equals(1) && residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), (item.Key.Year * 1000) + item.Key.DayOfYear));

                    if (item.Where(x => x.Id_Ingreso_Salida.Equals(2)).ToList().Count > 0 && item.Where(x => residuosPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                        lstDiasConValorSalidaP.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Where(x => x.Id_Ingreso_Salida.Equals(2) && residuosPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), (item.Key.Year * 1000) + item.Key.DayOfYear));

                    if (item.Where(x => x.Id_Ingreso_Salida.Equals(2)).ToList().Count > 0 && item.Where(x => residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                        lstDiasConValorSalidaNP.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Where(x => x.Id_Ingreso_Salida.Equals(2) && residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), (item.Key.Year * 1000) + item.Key.DayOfYear));
                }
                var distinctIngresoP = lstDiasSinValor.Where(x => !lstDiasConValorIngresoP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                ingresoP = distinctIngresoP.Union(lstDiasConValorIngresoP).OrderBy(x => x.Index).ToList();

                var distinctIngresoNP = lstDiasSinValor.Where(x => !lstDiasConValorIngresoNP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                ingresoNP = distinctIngresoNP.Union(lstDiasConValorIngresoNP).OrderBy(x => x.Index).ToList();

                var distinctSalidaP = lstDiasSinValor.Where(x => !lstDiasConValorSalidaP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                salidaP = distinctSalidaP.Union(lstDiasConValorSalidaP).OrderBy(x => x.Index).ToList();

                var distinctSalidaNP = lstDiasSinValor.Where(x => !lstDiasConValorSalidaNP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                salidaNP = distinctSalidaNP.Union(lstDiasConValorSalidaNP).OrderBy(x => x.Index).ToList();
            }
            else if (frecuencia.Equals("Semanal"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = registros.Select(x => new { woy = cul.Calendar.GetWeekOfYear(x.Fecha_Ingreso_Salida, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString().PadLeft(2, '0') + " " + x.Fecha_Ingreso_Salida.ToString("yyyy"), Cantidad = x.Cantidad, ingreso_salida = x.Id_Ingreso_Salida, Id_Residuo_Solido = x.Id_Residuo_Solido }).ToList();
                var frec = data.GroupBy(x => x.woy).ToList();

                int nSemInicial = cul.Calendar.GetWeekOfYear(FechaInicio, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int nSemFinal = cul.Calendar.GetWeekOfYear(FechaFin, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                var lstSemSinValor = new List<Dimension>();
                var lstSemConValorIngresoP = new List<Dimension>();
                var lstSemConValorIngresoNP = new List<Dimension>();
                var lstSemConValorSalidaP = new List<Dimension>();
                var lstSemConValorSalidaNP = new List<Dimension>();

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
                        /* Checking if the list contains any items with Id_Ingreso_Salida = 1 and
                        Id_Residuo_Solido in the list of residuosPeligrosos. */
                        if (item.Where(x => x.ingreso_salida.Equals(1)).ToList().Count > 0 && item.Where(x => residuosPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                            lstSemConValorIngresoP.Add(new Dimension("Sem " + week + " - " + year, item.Where(x => x.ingreso_salida.Equals(1) && residuosPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), year * 100 + week));

                        if (item.Where(x => x.ingreso_salida.Equals(1)).ToList().Count > 0 && item.Where(x => residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                            lstSemConValorIngresoNP.Add(new Dimension("Sem " + week + " - " + year, item.Where(x => x.ingreso_salida.Equals(1) && residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), year * 100 + week));

                        if (item.Where(x => x.ingreso_salida.Equals(2)).ToList().Count > 0 && item.Where(x => residuosPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                            lstSemConValorSalidaP.Add(new Dimension("Sem " + week + " - " + year, item.Where(x => x.ingreso_salida.Equals(2) && residuosPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), year * 100 + week));

                        if (item.Where(x => x.ingreso_salida.Equals(2)).ToList().Count > 0 && item.Where(x => residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                            lstSemConValorSalidaNP.Add(new Dimension("Sem " + week + " - " + year, item.Where(x => x.ingreso_salida.Equals(2) && residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), year * 100 + week));
                    }
                    var distinctIngresoP = lstSemSinValor.Where(x => !lstSemConValorIngresoP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    ingresoP = distinctIngresoP.Union(lstSemConValorIngresoP).OrderBy(x => x.Index).ToList();

                    var distinctIngresoNP = lstSemSinValor.Where(x => !lstSemConValorIngresoNP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    ingresoNP = distinctIngresoNP.Union(lstSemConValorIngresoNP).OrderBy(x => x.Index).ToList();

                    var distinctSalidaP = lstSemSinValor.Where(x => !lstSemConValorSalidaP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    salidaP = distinctSalidaP.Union(lstSemConValorSalidaP).OrderBy(x => x.Index).ToList();

                    var distinctSalidaNP = lstSemSinValor.Where(x => !lstSemConValorSalidaNP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    salidaNP = distinctSalidaNP.Union(lstSemConValorSalidaNP).OrderBy(x => x.Index).ToList();
                    /*
                    foreach (var item in frec)
                    {
                        var week = Convert.ToInt32(item.Key.Substring(0, 2));
                        var year = Convert.ToInt32(item.Key.Substring(3, 4));
                        if (item.Where(x => x.ingreso_salida.Equals(1)).ToList().Count > 0)
                            lstSemConValorIngreso.Add(new Dimension("Sem " + week + " - " + year, item.Where(x => x.ingreso_salida.Equals(1)).Sum(x => x.cantidad), year * 100 + week));
                        if (item.Where(x => x.ingreso_salida.Equals(2)).ToList().Count > 0)
                            lstSemConValorSalida.Add(new Dimension("Sem " + week + " - " + year, item.Where(x => x.ingreso_salida.Equals(2)).Sum(x => x.cantidad), year * 100 + week));
                    }
                    var distinctIngreso = lstSemSinValor.Where(x => !lstSemConValorIngreso.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    ingreso = distinctIngreso.Union(lstSemConValorIngreso).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    ingreso = ingreso.OrderBy(x => x.Index).ToList();

                    var distinctSalida = lstSemSinValor.Where(x => !lstSemConValorSalida.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    salida = distinctSalida.Union(lstSemConValorSalida).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    salida = salida.OrderBy(x => x.Index).ToList();
                    */
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
                        if (item.Where(x => x.ingreso_salida.Equals(1)).ToList().Count > 0 && item.Where(x => residuosPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                            lstSemConValorIngresoP.Add(new Dimension(nombre, item.Where(x => x.ingreso_salida.Equals(1) && residuosPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), (anioAct * 100) + semAct));

                        if (item.Where(x => x.ingreso_salida.Equals(1)).ToList().Count > 0 && item.Where(x => residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                            lstSemConValorIngresoNP.Add(new Dimension(nombre, item.Where(x => x.ingreso_salida.Equals(1) && residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), (anioAct * 100) + semAct));

                        if (item.Where(x => x.ingreso_salida.Equals(2)).ToList().Count > 0 && item.Where(x => residuosPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                            lstSemConValorSalidaP.Add(new Dimension(nombre, item.Where(x => x.ingreso_salida.Equals(2) && residuosPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), (anioAct * 100) + semAct));

                        if (item.Where(x => x.ingreso_salida.Equals(2)).ToList().Count > 0 && item.Where(x => residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                            lstSemConValorSalidaNP.Add(new Dimension(nombre, item.Where(x => x.ingreso_salida.Equals(2) && residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), (anioAct * 100) + semAct));
                    }
                    var distinctIngresoP = lstSemSinValor.Where(x => !lstSemConValorIngresoP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    ingresoP = distinctIngresoP.Union(lstSemConValorIngresoP).OrderBy(x => x.Index).ToList();

                    var distinctIngresoNP = lstSemSinValor.Where(x => !lstSemConValorIngresoNP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    ingresoNP = distinctIngresoNP.Union(lstSemConValorIngresoNP).OrderBy(x => x.Index).ToList();

                    var distinctSalidaP = lstSemSinValor.Where(x => !lstSemConValorSalidaP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    salidaP = distinctSalidaP.Union(lstSemConValorSalidaP).OrderBy(x => x.Index).ToList();

                    var distinctSalidaNP = lstSemSinValor.Where(x => !lstSemConValorSalidaNP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    salidaNP = distinctSalidaNP.Union(lstSemConValorSalidaNP).OrderBy(x => x.Index).ToList();
                    /*
                    foreach (var item in frec)
                    {
                        var semAct = Convert.ToInt32(item.Key.Substring(0, 2));
                        var anioAct = Convert.ToInt32(item.Key.Substring(3, 4));
                        var nombre = "Sem " + semAct + " - " + anioAct.ToString().Substring(2, 2);
                        if (semAnt > semAct)
                            nAnio++;
                        if (item.Where(x => x.ingreso_salida.Equals(1)).ToList().Count > 0)
                            lstSemConValorIngreso.Add(new Dimension(nombre, item.Where(x => x.ingreso_salida.Equals(1)).Sum(x => x.cantidad), (anioAct * 100) + semAct));
                        if (item.Where(x => x.ingreso_salida.Equals(2)).ToList().Count > 0)
                            lstSemConValorSalida.Add(new Dimension(nombre, item.Where(x => x.ingreso_salida.Equals(2)).Sum(x => x.cantidad), (anioAct * 100) + semAct)));
                        semAnt = semAct;
                    }
                    var distinctIngreso = lstSemSinValor.Where(x => !lstSemConValorIngreso.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    ingreso = distinctIngreso.Union(lstSemConValorIngreso).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    ingreso = ingreso.OrderBy(x => x.Index).ToList();

                    var distinctSalida = lstSemSinValor.Where(x => !lstSemConValorSalida.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    salida = distinctSalida.Union(lstSemConValorSalida).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                    salida = salida.OrderBy(x => x.Index).ToList();
                    */
                }
            }
            else if (frecuencia.Equals("Mensual"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var data = registros.Select(x => new { date = x.Fecha_Ingreso_Salida.ToString("MM yyyy"), Cantidad = x.Cantidad, ingreso_salida = x.Id_Ingreso_Salida, Id_Residuo_Solido = x.Id_Residuo_Solido }).ToList();
                var frec = data.GroupBy(x => x.date).ToList();

                int nMeses;
                var lstMesSinValor = new List<Dimension>();
                var lstMesConValorIngresoP = new List<Dimension>();
                var lstMesConValorIngresoNP = new List<Dimension>();
                var lstMesConValorSalidaP = new List<Dimension>();
                var lstMesConValorSalidaNP = new List<Dimension>();

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
                    
                        if (item.Where(x => x.ingreso_salida.Equals(1)).ToList().Count > 0 && item.Where(x => residuosPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                            lstMesConValorIngresoP.Add(new Dimension(nombre, item.Where(x => x.ingreso_salida.Equals(1) && residuosPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), anio * 100 + mes));

                        if (item.Where(x => x.ingreso_salida.Equals(1)).ToList().Count > 0 && item.Where(x => residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                            lstMesConValorIngresoNP.Add(new Dimension(nombre, item.Where(x => x.ingreso_salida.Equals(1) && residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), anio * 100 + mes));

                        if (item.Where(x => x.ingreso_salida.Equals(2)).ToList().Count > 0 && item.Where(x => residuosPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                            lstMesConValorSalidaP.Add(new Dimension(nombre, item.Where(x => x.ingreso_salida.Equals(2) && residuosPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), anio * 100 + mes));

                        if (item.Where(x => x.ingreso_salida.Equals(2)).ToList().Count > 0 && item.Where(x => residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).ToList().Count > 0)
                            lstMesConValorSalidaNP.Add(new Dimension(nombre, item.Where(x => x.ingreso_salida.Equals(2) && residuosNoPeligrosos.Contains(x.Id_Residuo_Solido)).Sum(x => x.Cantidad), anio * 100 + mes));
                    }
                    var distinctIngresoP = lstMesSinValor.Where(x => !lstMesConValorIngresoP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    ingresoP = distinctIngresoP.Union(lstMesConValorIngresoP).OrderBy(x => x.Index).ToList();

                    var distinctIngresoNP = lstMesSinValor.Where(x => !lstMesConValorIngresoNP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    ingresoNP = distinctIngresoNP.Union(lstMesConValorIngresoNP).OrderBy(x => x.Index).ToList();

                    var distinctSalidaP = lstMesSinValor.Where(x => !lstMesConValorSalidaP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    salidaP = distinctSalidaP.Union(lstMesConValorSalidaP).OrderBy(x => x.Index).ToList();

                    var distinctSalidaNP = lstMesSinValor.Where(x => !lstMesConValorSalidaNP.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                    salidaNP = distinctSalidaNP.Union(lstMesConValorSalidaNP).OrderBy(x => x.Index).ToList();
                    /*
                foreach (var item in frec)
                {
                    var mes = Convert.ToInt32(item.Key.Substring(0, 2));
                    var anio = Convert.ToInt32(item.Key.Substring(3, 4));
                    var fecha = new DateTime(anio, mes, 1);
                    var nombre = fecha.ToString("MMMM").Substring(0, 3) + " " + fecha.ToString("yyyy");

                    if (item.Where(x => x.ingreso_salida.Equals(1)).ToList().Count > 0)
                        lstMesConValorIngreso.Add(new Dimension(nombre, item.Where(x => x.ingreso_salida.Equals(1)).Sum(x => x.cantidad), anio * 100 + mes));
                    if (item.Where(x => x.ingreso_salida.Equals(2)).ToList().Count > 0)
                        lstMesConValorSalida.Add(new Dimension(nombre, item.Where(x => x.ingreso_salida.Equals(2)).Sum(x => x.cantidad), anio * 100 + mes));

                }
                var distinctIngreso = lstMesSinValor.Where(x => !lstMesConValorIngreso.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                ingreso = distinctIngreso.Union(lstMesConValorIngreso).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();

                var distinctSalida = lstMesSinValor.Where(x => !lstMesConValorSalida.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                salida = distinctSalida.Union(lstMesConValorSalida).Where(x => x.Index >= 0).OrderBy(x => x.Index).ToList();
                */
            }
            ingresoP = ingresoP.OrderBy(x => x.Index).ToList();
            ingresoNP = ingresoNP.OrderBy(x => x.Index).ToList();
            salidaP = salidaP.OrderBy(x => x.Index).ToList();
            salidaNP = salidaNP.OrderBy(x => x.Index).ToList();

            output.Peligrosos.Ingreso = ingresoP;
            output.Peligrosos.Salida = salidaP;
            output.NoPeligrosos.Ingreso = ingresoNP;
            output.NoPeligrosos.Salida = salidaNP;
            return output;
        }
        //Obtener la cantidad de INGRESO y SALIDA de cada NOMBRE DE RESIDUO y agruparlo según DESTINO (empresa)
        public async Task<List<DestinoResiduos>> GetDestinoxNombreResiduoxIngresoSalida(int acr_cmrs, int ingreso_salida, int tipo_residuo, int motivo, DateTime FechaInicio, DateTime FechaFin, int responsable, int empresa)
        {
            var output = new List<DestinoResiduos>();

            var residuosSolido = await _context.Residuos_Solidos.Where(x =>
                x.Id_Tipo_Residuo.Equals(tipo_residuo != 0 ? tipo_residuo : x.Id_Tipo_Residuo))                
                .ToListAsync();

            var residuosPeligrosos = await _context.Residuos_Solidos.Where(x =>
                x.Id_Tipo_Residuo.Equals(1))
                .Select(y => y.Id_Residuo_Solido)
                .ToListAsync();
            var residuosNoPeligrosos = await _context.Residuos_Solidos.Where(x =>
                x.Id_Tipo_Residuo.Equals(2))
                .Select(y => y.Id_Residuo_Solido)
                .ToListAsync();

            var registros = await _context.Ingresos_Salidas_RS.Where(x =>
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()) &&
                !x.Estado.Equals(EstadosResiduosSolidos.Pendiente.ToString()) &&
                x.Id_ACR_CMRS.Equals(acr_cmrs != 0 ? acr_cmrs : x.Id_ACR_CMRS) &&
                x.Id_Ingreso_Salida.Equals(ingreso_salida != 0 ? ingreso_salida : x.Id_Ingreso_Salida) &&
                residuosSolido.Select(y => y.Id_Residuo_Solido).Contains(x.Id_Residuo_Solido) &&
                x.Id_Motivo.Equals(motivo != 0 ? motivo : x.Id_Motivo) &&
                x.Id_Empresa_RS.Equals(empresa != 0 ? empresa : x.Id_Empresa_RS) &&
                x.Id_Gerencia.Equals(responsable != 0 ? responsable : x.Id_Gerencia) &&
                x.Fecha_Ingreso_Salida >= FechaInicio &&
                x.Fecha_Ingreso_Salida <= FechaFin)
                .ToListAsync();

            var destinos = await _context.Empresas.ToListAsync();
            //Agrupar por destino (empresa)
            var registrosxDestino = registros.GroupBy(x => x.Id_Empresa_RS).ToList();
            
            foreach (var item in registrosxDestino)
            {
                var nombre = destinos.Find(x => x.Id_Empresa.Equals(item.Key)).Razon_Social;
                //cada destino agrupa por residuo
                var grupoResiduos = item.GroupBy(x=>x.Id_Residuo_Solido).ToList();

                var outputItem = new DestinoResiduos();
                outputItem.Destino = nombre;

                foreach(var datosResiduo in grupoResiduos){
                    outputItem.NombreResiduos.Add(new NombreResiduoxIngresoSalida{
                        NombreResiduo = residuosSolido.Find(x=> x.Id_Residuo_Solido.Equals(datosResiduo.Key)).Descripcion,
                        Ingreso = datosResiduo.Where(x => x.Id_Ingreso_Salida.Equals(1)).Sum(x => x.Cantidad),                     
                        Salida = datosResiduo.Where(x => x.Id_Ingreso_Salida.Equals(2)).Sum(x => x.Cantidad),
                        Total = datosResiduo.Sum(x => x.Cantidad)
                    });
               }

               outputItem.NombreResiduos = outputItem.NombreResiduos.OrderBy(x=>x.Total).ToList();
               outputItem.TotalIngreso = outputItem.NombreResiduos.Sum(x=>x.Ingreso);
               outputItem.TotalSalida = outputItem.NombreResiduos.Sum(x=>x.Salida);
               outputItem.Total = outputItem.TotalIngreso + outputItem.TotalSalida;
               output.Add(outputItem);
            }

            output = output.OrderByDescending(x => x.Total).ToList();

            return output;
        }
    }
}