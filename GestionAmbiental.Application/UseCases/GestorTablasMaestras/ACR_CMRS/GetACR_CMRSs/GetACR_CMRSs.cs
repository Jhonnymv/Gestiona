using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ACR_CMRS.GetACR_CMRSs
{
    public class GetACR_CMRSs : IGetACR_CMRSs
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetACR_CMRSs(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ACR_CMRSsOutput> Execute(string filtro, int pagina, int rpp)
        {
            var acr_cmrss = await _gestorTablasMaestrasReadOnlyRepository.GetACR_CMRS(filtro);

            var outputs = new List<ACR_CMRSOutput>();
            foreach (var item in acr_cmrss)
            {
                var temp = new ACR_CMRSOutput();
                temp.Id_ACR_CMRS = item.Id_ACR_CMRS;
                temp.Descripcion = item.Descripcion;
                outputs.Add(temp);
            }
            var output = new ACR_CMRSsOutput
            {
                nACR_CMRSs = acr_cmrss.Count,
                ACR_CMRSs = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
