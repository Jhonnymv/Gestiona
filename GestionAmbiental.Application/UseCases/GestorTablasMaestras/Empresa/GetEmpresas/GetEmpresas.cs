using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Empresa.GetEmpresas
{
    public class GetEmpresas : IGetEmpresas
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetEmpresas(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<EmpresasOutput> Execute(string filtro,int pagina,int rpp)
        {
            var empresas = await _gestorTablasMaestrasReadOnlyRepository.GetEmpresas(filtro);

            var empresasOutputs = new List<EmpresaOutput>();
            foreach (var item in empresas)
            {
                var temp = new EmpresaOutput();
                temp.IdEmpresa = item.IdEmpresa;
                temp.RazonSocial = item.RazonSocial;
                temp.Estado = item.Estado;
                empresasOutputs.Add(temp);
            }
            var output = new EmpresasOutput
            {
                nEmpresas = empresasOutputs.Count,
                Empresas = empresasOutputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
