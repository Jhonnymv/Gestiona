using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa.GetEmpresa
{
    public class GetEmpresa : IGetEmpresa
    {

        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetEmpresa(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }
        public async Task<EmpresaOutput> Execute(int idEmpresa)
        {
            var empresa = await _gestorTablasMaestrasReadOnlyRepository.GetEmpresa(idEmpresa);
            var output = new EmpresaOutput(empresa);
            return output;
        }
    }
}
