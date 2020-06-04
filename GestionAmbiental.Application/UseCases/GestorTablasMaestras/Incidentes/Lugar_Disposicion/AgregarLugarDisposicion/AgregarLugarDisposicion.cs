using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Incidentes.Lugar_Disposicion.AgregarLugarDisposicion
{
    public class AgregarLugarDisposicion : IAgregarLugarDisposicion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarLugarDisposicion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string descripcion)
        {

            var item = new Domain.Incidentes.Items.Lugar_Disposicion
            {
                Descripcion = descripcion,
                Estado = "ACT"
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddLugar_Disposicion(item);
        }
    }
}
