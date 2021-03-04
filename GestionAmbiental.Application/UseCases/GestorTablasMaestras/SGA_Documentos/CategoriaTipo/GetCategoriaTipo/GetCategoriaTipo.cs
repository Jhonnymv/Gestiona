using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.SGA_Documentos.CategoriaTipo.GetCategoriaTipo
{
    public class GetCategoriaTipo : IGetCategoriaTipo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetCategoriaTipo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<CategoriaTipoOutput> Execute(int IdCategoriaTipo)
        {
            var categoriaTipo = await _gestorTablasMaestrasReadOnlyRepository.GetCategoriaTipo(IdCategoriaTipo);
            var output = new CategoriaTipoOutput(categoriaTipo);
            return output;
        }
    }
}
