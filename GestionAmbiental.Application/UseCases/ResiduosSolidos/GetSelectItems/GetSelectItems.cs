using GestionAmbiental.Application.Repositories.ResiduosSolidos;
using GestionAmbiental.Domain.Enum;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.ResiduosSolidos.GetSelectItems
{
    public class GetSelectItems : IGetSelectItems
    {
        private readonly IGetItemsReadOnlyRepository _getItemsReadOnlyRepository;

        public GetSelectItems(IGetItemsReadOnlyRepository getItemsReadOnlyRepository)
        {
            _getItemsReadOnlyRepository = getItemsReadOnlyRepository ?? throw new ArgumentNullException(nameof(getItemsReadOnlyRepository));
        }

        public async Task<SelectItemsOutput> Execute()
        {
            var acr_cmrs = await _getItemsReadOnlyRepository.GetACR_CMRS();
            var eo_rs = await _getItemsReadOnlyRepository.GetEO_RS();
            var ingreso_salidas = await _getItemsReadOnlyRepository.GetIngreso_Salida();
            var motivos = await _getItemsReadOnlyRepository.GetMotivos_Ingreso_Salida();
            var residuos = await _getItemsReadOnlyRepository.GetResiduos_Solidos();
            var tipos = await _getItemsReadOnlyRepository.GetTipos_Residuo();
            var empresas = await _getItemsReadOnlyRepository.GetEmpresa();
            var usuarios = await _getItemsReadOnlyRepository.GetUsuarios();
            var responsables_arc_cmrs = await _getItemsReadOnlyRepository.GetResponsables_ARC_CRM();
            var gerencias = await _getItemsReadOnlyRepository.GetGerencias();
            var conductores = await _getItemsReadOnlyRepository.GetConductores_RS();
            var vehiculos = await _getItemsReadOnlyRepository.GetVehiculos_RS();

            var output = new SelectItemsOutput();
            output.ItemsACR_CMRS = acr_cmrs;
            output.ItemsEO_RS = eo_rs;
            output.ItemsIngresoSalida = ingreso_salidas;
            output.ItemsMotivoIngresoSalida = motivos;
            output.ItemsResiduoSolido = residuos;
            output.ItemsTipoResiduo = tipos;
            output.ItemsUsuarios = usuarios;
            output.ItemsEmpresa = empresas;
            output.ItemsAsesoresMA = usuarios.Where(x => x.IdRol.Equals((int)RSRoles.AsesorMAP)).ToList();
            output.ItemsResponsableACR_CMRS = responsables_arc_cmrs;
            output.ItemsGerencia = gerencias;
            output.ItemsConductor = conductores;
            output.ItemsVehiculo = vehiculos;

            return output;
        }
    }
}
