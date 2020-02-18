using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.Reportes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor))
                .Select(x => x.Id_Inspeccion_Cabecera)
                .Distinct()
                .ToListAsync();

            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                observaciones.Contains(x.Id_Inspeccion_Cabecera) &&
                x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FechaInicio &&
                x.Fecha_Inspeccion <= FechaFin)
                .GroupBy(x => x.Estado)
                .ToListAsync();

            
            var output = new List<Dimension>();
            var temp = new List<Dimension>();
            output.Add(new Dimension("Pendiente", 0, 0));
            output.Add(new Dimension("Registrado",0, 1));
            output.Add(new Dimension("Eliminado", 0, 2));

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
                x.Id_Asesor.Equals(Asesor > 0 ? Asesor : x.Id_Asesor))
                .Select(x => x.Id_Inspeccion_Cabecera)
                .Distinct()
                .ToListAsync();


            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                observaciones.Contains(x.Id_Inspeccion_Cabecera) &&
                x.Id_Empresa.Equals(empresa > 0 ? empresa : x.Id_Empresa) &&
                x.Id_Gerencia.Equals(gerencia > 0 ? gerencia : x.Id_Gerencia) &&
                x.Fecha_Inspeccion >= FechaInicio &&
                x.Fecha_Inspeccion <= FechaFin)
                .ToListAsync();

            if (frecuencia.Equals("Diario"))
            {
                var frec_cabeceras = cabeceras.GroupBy(x => x.Fecha_Inspeccion).ToList();
                int nDias = 0;

                if (FechaFin.Year > FechaInicio.Year)
                    nDias = FechaFin.DayOfYear + (FechaInicio.Year % 4 == 0 ? 366 : 365) - FechaInicio.DayOfYear;
                else
                    nDias = FechaFin.DayOfYear - FechaInicio.DayOfYear;

                var lstDiasSinValor = new List<Dimension>();
                var lstDiasConValor = new List<Dimension>();

                for (int i = nDias; i >= 0; i--)
                {
                    lstDiasSinValor.Add(new Dimension(FechaFin.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, i));
                }

                foreach (var item in frec_cabeceras)
                {
                    lstDiasConValor.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Count(), FechaFin.DayOfYear - item.Key.DayOfYear));
                }
                var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinct.Union(lstDiasConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();

                return output;
            }
            else if (frecuencia.Equals("Semanal"))
            {
                CultureInfo cul = CultureInfo.CurrentCulture;
                var frec_cabeceras =cabeceras.GroupBy(x => cul.Calendar.GetWeekOfYear(x.Fecha_Inspeccion, CalendarWeekRule.FirstDay, DayOfWeek.Monday)).ToList();
                int nSemInicial = cul.Calendar.GetWeekOfYear(FechaInicio, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int nSemFinal = cul.Calendar.GetWeekOfYear(FechaFin, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                int nSem = nSemFinal - nSemInicial;

                var lstSemSinValor = new List<Dimension>();
                var lstSemConValor = new List<Dimension>();
                for (int i = nSem; i >= 0; i--)
                {
                    lstSemSinValor.Add(new Dimension("Sem " + (nSemInicial + i).ToString(), 0, nSemInicial + i));
                }
                foreach (var item in frec_cabeceras)
                {
                    lstSemConValor.Add(new Dimension("Sem "+item.Key.ToString(), item.Count(),item.Key));
                }
                var distinct = lstSemSinValor.Where(x => !lstSemConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
                output = distinct.Union(lstSemConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();
                output = output.OrderBy(x => x.Index).ToList();
                return output;
            }

            return output;
        }

        /*
        public async Task<List<Dimension>> GetObservacionesXEstado(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()))
                .ToListAsync();

            var observaciones = await _context.Inspecciones_Detalle.Where(x =>
            cabeceras.Select(y=>y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
            x.Id_Responsable_Marcobre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Marcobre) &&
            x.Fecha_Programada >= FechaInicio &&
            x.Fecha_Programada <= FechaFin &&
            !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()))
                .ToListAsync();

            var output = new List<Dimension>();

            foreach (var item in Enum.GetNames(typeof(EstadosRAC)))
            {
                output.Add(new Dimension(item, observaciones.Where(x => x.Estado.Equals(item)).Count()));
            }
            var totalRAC = await _context.RACs.CountAsync();
            output.Add(new Dimension("total", totalRAC));
            return output;
        }

        public async Task<List<DetalleObservacionXEstado>> GetDetalleReportesXEstado(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin, EstadosRAC estado)
        {
            var output = new List<DetalleObservacionXEstado>();
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
               x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
               x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
               !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()))
               .ToListAsync();

            var observaciones = await _context.Inspecciones_Detalle.Where(x =>
            cabeceras.Select(y=>y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
            x.Id_Responsable_Marcobre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Marcobre) &&
            x.Fecha_Programada >= FechaInicio &&
            x.Fecha_Programada <= FechaFin &&
            !x.Estado.Equals(estado.ToString()))
                .ToListAsync();

            var usuarios = await _context.Usuarios.ToListAsync();
            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();

            foreach (var item in observaciones)
            {
                var asesor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Asesor));
                var responsableMarcobre = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Responsable_Marcobre));
                var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Id_Tipo_Condicion));
                
                output.Add(new DetalleObservacionXEstado
                {
                    IdInspeccionCabecera = item.Id_Inspeccion_Cabecera,
                    Codigo = cabeceras.FirstOrDefault(x => x.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Codigo,
                    IdInspeccionDetalle = item.Id_Inspeccion_Detalle,
                    Asesor = asesor.Nombres + " "+asesor.Apellidos,
                    DescripcionCondicion = item.Descripcion_Condicion,
                    TipoObservacion = item.Tipo_Observacion,
                    TipoCondicion = tipo_condicion.Descripcion,
                    NivelRiesgo = item.Nivel_Riesgo,
                    DescripcionAccion = item.Descripcion_Accion,
                    ResponsableMarcobre = responsableMarcobre.Nombres + " "+responsableMarcobre.Apellidos,
                    FechaProgramada = item.Fecha_Programada,
                    ResponsableContratista = item.Responsable_Contratista,
                    Estado = item.Estado
                }) ;
            }

            return output;
        }
        */
    }
}
