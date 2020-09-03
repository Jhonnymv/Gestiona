using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.AvistamientoFauna.LugarReferencia.AgregarLugarReferencia
{
    public class AgregarLugarReferencia : IAgregarLugarReferencia
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarLugarReferencia(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string descripcion, string latitud, string longitud)
        {
            var input = new Domain.AvistamientoFauna.Items.LugarReferencia()
            {
                Descripcion = descripcion,
                Latitud = latitud,
                Longitud = longitud
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddLugarReferencia(input);
        }
    }
}
