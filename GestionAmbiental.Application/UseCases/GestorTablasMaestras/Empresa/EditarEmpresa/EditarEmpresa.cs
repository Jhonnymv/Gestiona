using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa.EditarEmpresa
{
    public class EditarEmpresa : IEditarEmpresa
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarEmpresa(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int idEmpresa, string razonSocial,string estado)
        {
            var empresa = new Domain.ValueObjects.Empresa();
            empresa.IdEmpresa = idEmpresa;
            empresa.RazonSocial = razonSocial;
            empresa.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateEmpresa(empresa);
        }
    }
}
