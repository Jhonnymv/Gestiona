using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Inspecciones
{
    public interface ICabeceraInspeccionReadOnlyRepository
    {
        Task<List<InspeccionCabecera>> GetInspeccionCabecera(int pagina, int fpp, DateTime fecha, int gerencia, string codigo);
        Task<InspeccionCabecera> GetInspeccionCabecera(int idCabecera);
        Task<List<InspeccionCabecera>> GetInspeccionCabeceraImplementacion(int pagina, int fpp, DateTime fecha, int gerencia, string codigo);
        Task<InspeccionCabecera> GetInspeccionCabeceraImplementacion(int idCabecera);
        Task<List<InspeccionCabecera>> GetInspeccionCabeceraVerificacion(int pagina, int fpp, DateTime fecha, int gerencia, string codigo);
        Task<InspeccionCabecera> GetInspeccionCabeceraVerificacion(int idCabecera);
        Task<InspeccionCabecera> GetInspeccionCabeceraDescarga(int idCabecera);
    }
}
