using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.ConsumoAgua.FuenteAgua.EditarFuenteAgua
{
    public class EditarFuenteAgua : IEditarFuenteAgua
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarFuenteAgua(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task Execute(int id, string descripcion, string coordenadas, int idOrigen, string estado)
        {
            var input = new Domain.ConsumoAgua.FuenteAgua();
            input.IdFuenteAgua = id;
            input.Descripcion = descripcion;
            input.Coordenadas_UTM = coordenadas;
            input.OrigenAgua = new Domain.ConsumoAgua.OrigenAgua
            {
                IdOrigenAgua = idOrigen 
            };
            input.Estado = estado;
            await _gestorTablasMaestrasReadOnlyRepository.UpdateFuenteAgua(input);
        }
    }
}
