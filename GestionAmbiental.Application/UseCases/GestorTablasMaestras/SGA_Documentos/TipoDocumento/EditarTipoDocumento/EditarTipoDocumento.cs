using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.EditarTipoDocumento
{
    public class EditarTipoDocumento : IEditarTipoDocumento
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarTipoDocumento(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int IdTipoDocumento, string Descripcion, string Codigo, string Formato, string Estado)
        {
            var input = new Domain.SGA_Documentos.TipoDocumento();
            input.IdSGATipoDocumento = IdTipoDocumento;
            input.Descripcion = Descripcion;
            input.Codigo = Codigo;
            input.Formato = Formato;
            input.Estado = Estado;

            await _gestorTablasMaestrasReadOnlyRepository.UpdateTipoDocumento(input);
        }
    }
}
