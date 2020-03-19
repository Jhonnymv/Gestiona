using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.MotivoIngresoSalida.GetMotivo
{
    public class GetMotivo : IGetMotivo
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public GetMotivo(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<MotivoOutput> Execute(int idMotivo)
        {
            var motivo = await _gestorTablasMaestrasReadOnlyRepository.GetMotivo_Ingreso_Salida(idMotivo);
            var output = new MotivoOutput(motivo);
            return output;
        }
    }
}
