using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Responsable_ACR_CMRS.GetResponsables_ACR_CMRS
{
    public class GetResponsablesACR_CMRS : IGetResponsablesACR_CMRS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetResponsablesACR_CMRS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ResponsablesACR_CMRSOutput> Execute(string filtro, int pagina, int rpp)
        {
            var responsables = await _gestorTablasMaestrasReadOnlyRepository.GetResponsable_ARC_CRMS(filtro);

            var outputs = new List<ResponsableACR_CMRSOutput>();
            foreach (var item in responsables)
            {
                var temp = new ResponsableACR_CMRSOutput();
                temp.Id_Responsable_ACR_CRMS = item.Id_Responsable_ACR_CRMS;
                temp.Nombres = item.Nombres;
                temp.Apellidos = item.Apellidos;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new ResponsablesACR_CMRSOutput
            {
                NResponsables = responsables.Count,
                responsables = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
