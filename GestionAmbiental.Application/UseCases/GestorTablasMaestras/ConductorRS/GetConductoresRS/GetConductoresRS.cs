using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConductorRS.GetConductoresRS
{
    public class GetConductoresRS : IGetConductoresRS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetConductoresRS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ConductoresRSOutput> Execute(string filtro, int pagina, int rpp)
        {
            var conductores = await _gestorTablasMaestrasReadOnlyRepository.GetConductor_RS(filtro);

            var outputs = new List<ConductorRSOutput>();
            foreach (var item in conductores)
            {
                var temp = new ConductorRSOutput();
                temp.Id_Conductor_RS = item.Id_Conductor_RS;
                temp.Nombres = item.Nombres;
                temp.Apellidos = item.Apellidos;
                temp.NLicencia = item.NLicencia;
                temp.Estado = item.Estado;
                outputs.Add(temp);
            }
            var output = new ConductoresRSOutput
            {
                NConductores = conductores.Count,
                Conductores = outputs.Skip((pagina - 1) * rpp).Take(rpp).ToList()
            };

            return output;
        }
    }
}
