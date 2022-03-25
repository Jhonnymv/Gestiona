using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.CompromisosMCB.MedioVerificacionCMCB
{
    public interface IMedioVerificacionReadOnlyRepository
    {
        //Listar
        Task<List<Domain.CompromisosMCB.MedioVerificacionCMCB>> ListarMedioVerificacion();

        //Registrar
        Task<int> RegistrarMedioVerificacion(Domain.CompromisosMCB.MedioVerificacionCMCB medioVerificacionCMCB);

        //Editar
        Task EditarMedioVerificacion(Domain.CompromisosMCB.MedioVerificacionCMCB medioVerificacionCMCB);
    }
}
