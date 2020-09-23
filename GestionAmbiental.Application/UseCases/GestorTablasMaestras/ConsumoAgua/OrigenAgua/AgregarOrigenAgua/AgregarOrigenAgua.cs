using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.OrigenAgua.AgregarOrigenAgua
{
    public class AgregarOrigenAgua : IAgregarOrigenAgua
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarOrigenAgua(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string descripcion)
        {
            var input = new Domain.ConsumoAgua.OrigenAgua()
            {
                Descripcion = descripcion
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddOrigenAgua(input);
        }
    }
}
