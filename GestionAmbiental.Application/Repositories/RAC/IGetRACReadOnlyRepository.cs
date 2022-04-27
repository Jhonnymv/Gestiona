using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.RAC
{
    public interface IGetRACReadOnlyRepository
    {
        Task<List<Domain.RAC.RAC>> GetRACs(DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] areasFiltro, int[] empresaFiltro, int[] registradoPorFiltro, List<string> estadoFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigoFiltro);
        Task<List<Domain.RAC.RAC>> GetRACsImplementacion(DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] areasFiltro, int[] empresaFiltro, int[] registradoPorFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigoFiltro);
        Task<Domain.RAC.RAC> GetRAC(int Id_RAC);
        Task<List<Domain.RAC.RAC>> GetRACsVerificacion(DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] areasFiltro, int[] empresaFiltro, int[] registradoPorFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigoFiltro);
        Task EliminarVencidos(DateTime fechaInicio, DateTime fechaFin);
    }
}
