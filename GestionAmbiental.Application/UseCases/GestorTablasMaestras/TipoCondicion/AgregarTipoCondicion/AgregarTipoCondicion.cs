using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.TipoCondicion.AgregarTipoCondicion
{
    public class AgregarTipoCondicion : IAgregarTipoCondicion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public AgregarTipoCondicion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async  Task<int> Execute(string descripcion, string grupo)
        {
            var tipoCondicion = new Domain.ValueObjects.TipoCondicion
            {
                Descripcion = descripcion,
                Grupo = grupo
            };
            return await _gestorTablasMaestrasReadOnlyRepository.AddTipoCondicion(tipoCondicion);
        }
    }
}
