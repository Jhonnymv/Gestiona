using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa.AgregarEmpresa
{
    public class AgregarEmpresa : IAgregarEmpresa
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarEmpresa(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string razonSocial)
        {
            var empresa = new Domain.ValueObjects.Empresa
            {
                RazonSocial = razonSocial,
                Estado = "ACT"
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddEmpresa(empresa);
        }
    }
}
