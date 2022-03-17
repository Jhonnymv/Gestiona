using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.SustentoCMCB
{
    public interface ISustentoReadOnlyRepository
    {
        //Registrar
        Task<int> RegistrarSustento(Domain.CompromisosMCB.SustentoCMCB sustentoCMCB);
        //Editar
        Task EditarSustento(Domain.CompromisosMCB.SustentoCMCB sustentoCMCB);
        //Buscar por ID
        Task<Domain.CompromisosMCB.SustentoCMCB> ObtenerSustento(int Id);
        //Eliminar
        Task EliminarSustento(int Id);
        //Listar
        Task<List<Domain.CompromisosMCB.SustentoCMCB>> ListarSustentos(int[] gerencias, int[] areas, int[] criterios, string[] estados, int[] etapas);
    }
}
