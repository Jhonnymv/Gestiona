using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.FOCAM.Documento
{
    public interface IDocumentoReadOnlyRepository
    {
        //Listar
        Task<List<Domain.FOCAM.Documento>> ListDocuemntos();

        //Registrar
        Task<int> RegistrarDocumento(Domain.FOCAM.Documento documento);

        //Editar
        Task EditarDocumento(Domain.FOCAM.Documento documento);
        ////Buscar por ID
        Task<Domain.FOCAM.Documento> BuscarPorID(int Id);
    }
}
