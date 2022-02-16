using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Inspecciones
{
    public interface ICabeceraInspeccionReadOnlyRepository
    {
        Task<List<InspeccionCabecera>> GetInspeccionCabecera(DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, List<string> estadoFiltro, int[] responsableFiltro, int[] asesorFiltro,string codigo);
        Task<InspeccionCabecera> GetInspeccionCabecera(int idCabecera);
        Task<List<InspeccionCabecera>> GetInspeccionCabeceraImplementacion(DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigo);
        Task<InspeccionCabecera> GetInspeccionCabeceraImplementacion(int idCabecera);
        Task<List<InspeccionCabecera>> GetInspeccionCabeceraVerificacion(DateTime fechaInicio, DateTime fechaFin, int[] gerenciaFiltro, int[] empresaFiltro, int[] registradoPorFiltro, int[] responsableFiltro, int[] asesorFiltro, string codigo);
        Task<InspeccionCabecera> GetInspeccionCabeceraVerificacion(int idCabecera);
        Task<InspeccionCabecera> GetInspeccionCabeceraDescarga(int idCabecera);
        Task EliminarVencidos(DateTime fechaInicio, DateTime fechaFin);
    }
}
