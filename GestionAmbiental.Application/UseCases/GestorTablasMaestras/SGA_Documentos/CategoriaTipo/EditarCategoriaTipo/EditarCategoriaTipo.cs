using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.EditarCategoriaTipo
{
    public class EditarCategoriaTipo : IEditarCategoriaTipo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarCategoriaTipo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int IdCategoriaTipo, int IdCategoria, int IdTipoDoc, string Estado)
        {
            var input = new Domain.SGA_Documentos.CategoriaTipo();
            input.IdSGACategoriaTipo = IdCategoriaTipo;
            input.CategoriaDocumento = new Domain.SGA_Documentos.CategoriaDocumento { 
                Id_SGA_Categoria_Documento = IdCategoria
            };
            input.TipoDocumento = new Domain.SGA_Documentos.TipoDocumento { 
                IdSGATipoDocumento = IdTipoDoc
            };
            input.Estado = Estado;

            await _gestorTablasMaestrasReadOnlyRepository.UpdateCategoriaTipo(input);
        }
    }
}
