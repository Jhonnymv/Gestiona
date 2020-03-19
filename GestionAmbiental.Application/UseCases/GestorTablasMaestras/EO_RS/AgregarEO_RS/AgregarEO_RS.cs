using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.EO_RS.AgregarEO_RS
{
    public class AgregarEO_RS : IAgregarEO_RS
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarEO_RS(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string razon_social, string registro, string fecha_vencimiento)
        {
            var eors = new Domain.ValueObjects.EO_RS
            {
                Razon_Social = razon_social,
                Registro = registro,
                Fecha_Vencimiento_Registro = DateTime.Parse(fecha_vencimiento)
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddEO_RS(eors);
        }
    }
}
