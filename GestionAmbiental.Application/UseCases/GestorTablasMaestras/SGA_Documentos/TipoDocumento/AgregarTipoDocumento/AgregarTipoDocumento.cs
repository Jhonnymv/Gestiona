using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.TipoDocumento.AgregarTipoDocumento
{
    public class AgregarTipoDocumento : IAgregarTipoDocumento
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarTipoDocumento(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string Descripcion, string Codigo, string Formato)
        {
            var input = new Domain.SGA_Documentos.TipoDocumento();
            input.Descripcion = Descripcion;
            input.Codigo = Codigo;
            input.Formato = Formato;

            return await _gestorTablasMaestrasReadOnlyRepository.AddTipoDocumento(input);
        }
    }
}
