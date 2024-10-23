using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarCabeceras
{
    public class ConsultarCabeceras : IConsultarCabeceras
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;
        public ConsultarCabeceras(ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository)
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository;
        }
        public async Task<InspeccionesCabeceraOutput> Execute(int pagina, int fpp, DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, string[] estadoFiltro, int[] responsableFiltro, int[] asesorFiltro,string codigo, string[] tipo_inspeccion_filtro, string[] tipo_observacion_filtro)
        {
            var estados= new List<string>();
            if (estadoFiltro.Length > 0)
            {
                if (estadoFiltro.Contains("Implementacion"))
                {
                    estados.Add(EstadosInspeccion.Registrado.ToString());
                    estados.Add(EstadosInspeccion.Ampliado.ToString());
                    estados.Add(EstadosInspeccion.Rechazado.ToString());
                }
                if (estadoFiltro.Contains("Verificacion"))
                {
                    estados.Add(EstadosInspeccion.Implementado.ToString());
                    estados.Add(EstadosInspeccion.Rechazado.ToString());
                }
                if (estadoFiltro.Contains("Vencidos"))
                    estados.Add(EstadosInspeccion.Vencido.ToString());
                if (estadoFiltro.Contains("Aprobados"))
                    estados.Add(EstadosInspeccion.Aprobado.ToString());
            }
            var cabeceras = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabecera(fechaInicio, fechaFin, gerenciaFiltro, empresaFiltro, registradoPorFiltro, estados, responsableFiltro, asesorFiltro,codigo,tipo_inspeccion_filtro,tipo_observacion_filtro);
            var tbd = cabeceras.Where(x => x.Estado.Equals(EstadosInspeccion.Pendiente.ToString())).OrderByDescending(x=>x.FechaInspeccion).ToList();
            var noTbd = cabeceras.Where(x => !x.Estado.Equals(EstadosInspeccion.Pendiente.ToString())).OrderByDescending(x=>x.Codigo).ToList();
            cabeceras = tbd;
            cabeceras.AddRange(noTbd);

            var output = new InspeccionesCabeceraOutput
            {
                NCabeceras = cabeceras.Count,
                Cabeceras = cabeceras.Skip((pagina - 1) * fpp).Take(fpp).ToList()
            };
            return output;
        }
        public List<string> GetDataSourceTypes()
        {
            return Enum.GetNames(typeof(EstadosInspeccion)).ToList();
        }
    }
}
