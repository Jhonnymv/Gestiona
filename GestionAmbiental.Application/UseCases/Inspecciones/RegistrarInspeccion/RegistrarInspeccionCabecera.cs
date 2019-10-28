using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;

namespace GestionAmbiental.Application.UseCases.Inspecciones.RegistrarInspeccion
{
    public class RegistrarInspeccionCabecera : IRegistrarInspeccionCabecera
    {
        private readonly IRegistarInspeccionReadOnlyRepository _registarInspeccionReadOnlyRepository;

        public RegistrarInspeccionCabecera(IRegistarInspeccionReadOnlyRepository registarInspeccionReadOnlyRepository)
        {
            _registarInspeccionReadOnlyRepository = registarInspeccionReadOnlyRepository;
        }

        public async Task Execute(string objetivo, int gerenciaMarc, int empresaMarc, int gerenciaEPCM, int empresaEPCM, string lugar, string fechaInspeccion, string hora, int responsableLugar, int responsableMA, string inspectores)
        {
            var cabecera = new NuevaInspeccionCabecera(
                gerenciaMarc,
                gerenciaEPCM,
               empresaMarc,
                empresaEPCM,
                Convert.ToDateTime(fechaInspeccion + " " + hora),
                lugar,
                responsableLugar,
                responsableMA,
                inspectores,
                objetivo);


            //TODO pasar datos
            int idCabecera = await _registarInspeccionReadOnlyRepository.RegistrarInspeccionCabecera(cabecera);
        }
    }
}
