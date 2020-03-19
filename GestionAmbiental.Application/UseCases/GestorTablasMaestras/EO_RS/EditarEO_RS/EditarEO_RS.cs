using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS.EditarEO_RS
{
    public class EditarEO_RS : IEditarEO_RS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarEO_RS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int IdEO_RS, string Razon_Social, string Registro, string Fecha_Vencimiento_Registro, string Estado)
        {
            var eors = new Domain.ValueObjects.EO_RS();
            eors.Id_EO_RS = IdEO_RS;
            eors.Razon_Social = Razon_Social;
            eors.Registro = Registro;
            eors.Fecha_Vencimiento_Registro = DateTime.Parse(Fecha_Vencimiento_Registro);
            eors.Estado = Estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateEO_RS(eors);
        }
    }
}
