using GestionAmbiental.Application.Repositories.ResiduosSolidos;
using GestionAmbiental.Domain.ResiduosSolidos;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.RegistrarIngresoSalidaRS
{
    public class RegistrarIngresoSalidaRS : IRegistrarIngresoSalidaRS
    {
        private readonly IRegistrarIngresoSalidaRSReadOnlyRepository _registrarIngresoSalidaRSReadOnlyRepository;
        private readonly GetIngresoSalidaRSReadOnlyRepository _getIngresoSalidaRSReadOnlyRepository;

        public RegistrarIngresoSalidaRS(IRegistrarIngresoSalidaRSReadOnlyRepository registrarIngresoSalidaRSReadOnlyRepository, GetIngresoSalidaRSReadOnlyRepository getIngresoSalidaRSReadOnlyRepository)
        {
            _registrarIngresoSalidaRSReadOnlyRepository = registrarIngresoSalidaRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(registrarIngresoSalidaRSReadOnlyRepository));
            _getIngresoSalidaRSReadOnlyRepository = getIngresoSalidaRSReadOnlyRepository ?? throw new ArgumentNullException(nameof(getIngresoSalidaRSReadOnlyRepository));
        }

        public async Task<int> Execute(int arc_cmrs, int ingreso_salida, int motivo, int residuo_solido, string fecha, string responsable, double cantidad, int empresa)
        {
            var fecha_format = DateTime.Parse(fecha);
            var input = new Ingreso_Salida_RS();

            input.ACR_CMRS = new ACR_CMRS();
            input.Ingreso_Salida = new Ingreso_Salida();
            input.Motivo = new Motivo_Ingreso_Salida();
            input.Residuo_Solido = new Residuo_Solido();
            input.Empresa_RS = new Empresa();


            input.ACR_CMRS.Id_ACR_CMRS = arc_cmrs;
            input.Ingreso_Salida.Id_Ingreso_Salida = ingreso_salida;
            input.Motivo.Id_Motivo = motivo;
            input.Residuo_Solido.Id_Residuo_Solido = residuo_solido;
            input.Fecha_Ingreso_Salida = fecha_format;
            input.Responsable_ACR_CMRS = responsable;
            input.Cantidad = cantidad;
            input.Empresa_RS.IdEmpresa = empresa;
            var lastItem = await _getIngresoSalidaRSReadOnlyRepository.GetLastItem();
            input.Item = lastItem != null ? lastItem + 1 : 1;

            return await _registrarIngresoSalidaRSReadOnlyRepository.RegistrarIngresoSalida(input);
        }
    }
}
