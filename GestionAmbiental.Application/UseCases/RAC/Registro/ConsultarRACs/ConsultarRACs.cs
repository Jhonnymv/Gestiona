using GestionAmbiental.Application.Repositories.RAC;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Registro.ConsultarRACs
{
    public class ConsultarRACs : IConsultarRACs
    {
        private readonly IGetRACReadOnlyRepository _RACReadOnlyRepository;
        public ConsultarRACs(IGetRACReadOnlyRepository rACReadOnlyRepository)
        {
            _RACReadOnlyRepository = rACReadOnlyRepository ?? throw new ArgumentNullException(nameof(rACReadOnlyRepository));
        }
        public async Task<RACsOutput> Execute(int pagina, int fpp, DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] areasFiltro, int[] empresaFiltro, int[] registradoPorFiltro, string[] estadoFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigoFiltro)
        {
            var estados = new List<string>();
            if (estadoFiltro.Length > 0)
            {
                if (estadoFiltro.Contains("Implementacion"))
                {
                    estados.Add(EstadosRAC.Registrado.ToString());
                    estados.Add(EstadosRAC.Ampliado.ToString());
                    estados.Add(EstadosRAC.Rechazado.ToString());
                }
                if (estadoFiltro.Contains("Verificacion"))
                {
                    estados.Add(EstadosRAC.Implementado.ToString());
                    estados.Add(EstadosRAC.Rechazado.ToString());
                }
                if (estadoFiltro.Contains("Vencidos"))
                    estados.Add(EstadosRAC.Vencido.ToString());
                if (estadoFiltro.Contains("Aprobados"))
                    estados.Add(EstadosRAC.Aprobado.ToString());
            }
            var registros = await _RACReadOnlyRepository.GetRACs(fechaInicio, fechaFin, gerenciaFiltro, areasFiltro, empresaFiltro, registradoPorFiltro, estados, responsableFiltro, asesorFiltro, codigoFiltro);
            var output = new RACsOutput
            {
                Registros = new List<RACOutput>(),
                NRegistros = registros.Count
            };
            registros = registros.Skip((pagina - 1) * fpp).Take(fpp).ToList();
            foreach (var item in registros)
            {
                output.Registros.Add(new RACOutput(item));
            }
            return output;
        }
    }
}
