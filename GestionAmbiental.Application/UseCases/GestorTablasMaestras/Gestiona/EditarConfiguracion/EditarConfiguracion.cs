using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gestiona.EditarConfiguracion
{
    public class EditarConfiguracion : IEditarConfiguracion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public EditarConfiguracion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository;
        }

        public async Task Exectue(string nombre, string ruta, string texto)
        {
            var input = new ArchivoConfiguracion {
                Nombre = nombre,
                Ruta = ruta,
                Texto = texto
            };
            
            await _gestorTablasMaestrasReadOnlyRepository.UpdateArchivoConfiguracion(input);

        }
    }
}
