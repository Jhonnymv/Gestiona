using GestionAmbiental.Domain.SGA_Documentos;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.SGA_Documentos
{
    public interface IGetItemsReadOnlyRepository
    {
        Task<List<TipoDocumento>> GetTiposDocumento();
        Task<List<CategoriaDocumento>> GetCategoriasDocumento();
        Task<List<CategoriaTipo>> GetCategoriasTipo();
        Task<List<Condicion>> GetCondiciones();
        Task<List<Usuario>> GetListUsuarios();
        Task<List<TipoRevision>> GetTiposRevision();
    }
}
