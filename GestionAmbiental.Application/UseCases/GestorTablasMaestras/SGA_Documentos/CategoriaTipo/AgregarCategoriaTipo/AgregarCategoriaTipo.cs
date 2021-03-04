using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.AgregarCategoriaTipo
{
    public class AgregarCategoriaTipo : IAgregarCategoriaTipo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarCategoriaTipo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(int IdCategoria, int IdTipoDoc)
        {
            var input = new Domain.SGA_Documentos.CategoriaTipo();
            input.CategoriaDocumento = new Domain.SGA_Documentos.CategoriaDocumento
            {
                Id_SGA_Categoria_Documento = IdCategoria
            };
            input.TipoDocumento = new Domain.SGA_Documentos.TipoDocumento
            {
                IdSGATipoDocumento = IdTipoDoc
            };

            return await _gestorTablasMaestrasReadOnlyRepository.AddCategoriaTipo(input);
        }
    }
}
