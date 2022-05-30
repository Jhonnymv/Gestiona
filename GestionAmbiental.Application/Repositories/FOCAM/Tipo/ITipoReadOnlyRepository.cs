using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.FOCAM.Tipo
{
    public interface ITipoReadOnlyRepository
    {
        //Listar
        Task<List<Domain.FOCAM.Tipo>> ListTipos();

        //Registrar
        Task<int> RegistrarTipo(Domain.FOCAM.Tipo Tipo);

        //Editar
        Task EditarTipo(Domain.FOCAM.Tipo Tipo);

        ////Buscar por ID
        Task<Domain.FOCAM.Tipo> BuscarPorID(int Id);
    }
}
