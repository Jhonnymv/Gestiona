using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.EditarInspeccion
{
    public class EditarInspeccionCabecera : IEditarInspeccionCabecera
    {
        private readonly IEditarInspeccionReadOnlyRepository _editarInspeccionReadonlyRepository;

        public EditarInspeccionCabecera(IEditarInspeccionReadOnlyRepository editarInspeccionReadonlyRepository)
        {
            _editarInspeccionReadonlyRepository = editarInspeccionReadonlyRepository ?? throw new ArgumentNullException(nameof(editarInspeccionReadonlyRepository));
        }

        public async Task Execute(int idCabecera, string objetivo, int gerenciaMarc, int empresaMarc, int gerenciaEPCM, int empresaEPCM, string lugar, string fechaInspeccion, string hora, int responsableLugar, int responsableMA, string inspectores)
        {
            var cabecera = new NuevaInspeccionCabecera(
                idCabecera,
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

            await _editarInspeccionReadonlyRepository.EditarInspeccionCabecera(cabecera);
        }
    }
}
