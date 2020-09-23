using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua.AgregarFuenteAgua
{
    public class AgregarFuenteAgua : IAgregarFuenteAgua
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarFuenteAgua(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<int> Execute(string descripcion, string coordenadas,int idOrigen)
        {
            var input = new Domain.ConsumoAgua.FuenteAgua()
            {
                Descripcion = descripcion,
                Coordenadas_UTM = coordenadas,
                OrigenAgua = new Domain.ConsumoAgua.OrigenAgua
                {
                    IdOrigenAgua = idOrigen
                }
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddFuenteAgua(input);
        }
    }
}
