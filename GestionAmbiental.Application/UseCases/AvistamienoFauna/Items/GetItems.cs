using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.AvistamienoFauna.Items
{
    public class GetItems : IGetItems
    {
        private IGetItemsAvistamientoFaunaReadOnlyRepository _getItemsAvistamientoFauna;

        public GetItems(IGetItemsAvistamientoFaunaReadOnlyRepository getItemsAvistamientoFauna)
        {
            _getItemsAvistamientoFauna = getItemsAvistamientoFauna ?? throw new ArgumentNullException(nameof(getItemsAvistamientoFauna));
        }

        public async Task<ItemsOutput> Execute()
        {
            var output = new ItemsOutput();
            output.TiposFauna = await _getItemsAvistamientoFauna.GetTiposFauna();
            output.NombresComunes = await _getItemsAvistamientoFauna.GetNombresComunesFauna();
            output.TiposAvistamiento = await _getItemsAvistamientoFauna.GetTiposAvistamiento();
            output.EstadosFauna = await _getItemsAvistamientoFauna.GetEstadosFauna();
            output.AreasTrabajo = await _getItemsAvistamientoFauna.GetAreasTrabajo();
            output.Usuarios = await _getItemsAvistamientoFauna.GetUsuarios();
            output.Empresas = await _getItemsAvistamientoFauna.GetEmpresas();
            output.Supervisores = output.Usuarios.Where(x => x.IdRol.Equals((int)InspeccionRol.AsesorMAP)).ToList();
            output.LugaresReferencia = await _getItemsAvistamientoFauna.GetLugaresReferencia();
            output.ContactosEmergencia = await _getItemsAvistamientoFauna.GetContactosEmergencia();

            return output;
        }
    }
}
