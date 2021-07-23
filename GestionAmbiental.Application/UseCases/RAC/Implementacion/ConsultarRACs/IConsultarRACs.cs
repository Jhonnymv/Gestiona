using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.RAC.Implementacion.ConsultarRACs
{
    public interface IConsultarRACs
    {
        Task<RACsOutput> Execute(int pagina, int fpp, DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, int[] responsableFiltro, int[] asesorFiltro);
    }
}
