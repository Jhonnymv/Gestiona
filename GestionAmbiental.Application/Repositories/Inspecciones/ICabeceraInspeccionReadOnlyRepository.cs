using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Inspecciones
{
    public interface ICabeceraInspeccionReadOnlyRepository
    {
        Task<List<InspeccionCabecera>> GetInspeccionCabecera(DateTime fecha, int empresa,int reg_por, string codigo);
        Task<InspeccionCabecera> GetInspeccionCabecera(int idCabecera);
        Task<List<InspeccionCabecera>> GetInspeccionCabeceraImplementacion(DateTime fecha, int empresa,int reg_por, string codigo,int id_usuario);
        Task<InspeccionCabecera> GetInspeccionCabeceraImplementacion(int idCabecera);
        Task<List<InspeccionCabecera>> GetInspeccionCabeceraVerificacion(DateTime fecha, int empresa,int reg_por, string codigo);
        Task<InspeccionCabecera> GetInspeccionCabeceraVerificacion(int idCabecera);
        Task<InspeccionCabecera> GetInspeccionCabeceraDescarga(int idCabecera);
    }
}
