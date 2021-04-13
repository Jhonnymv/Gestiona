using GestionAmbiental.Application.Repositories.GestorTablasMaestras;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.GestorTablasMaestras.Gestiona.ObtenerConfiguracion
{
    public class ObtenerConfiguracion : IObtenerConfiguracion
    {
        private readonly IGestorTablasMaestrasReadOnlyRepository _gestorTablasMaestrasReadOnlyRepository;

        public ObtenerConfiguracion(IGestorTablasMaestrasReadOnlyRepository gestorTablasMaestrasReadOnlyRepository)
        {
            _gestorTablasMaestrasReadOnlyRepository = gestorTablasMaestrasReadOnlyRepository ?? throw new ArgumentNullException(nameof(gestorTablasMaestrasReadOnlyRepository));
        }

        public async Task<ArchivoConfiguracionOutput> Execute(string Nombre)
        {
            var data = await _gestorTablasMaestrasReadOnlyRepository.GetArchivoConfiguracion(Nombre);
            var output = new ArchivoConfiguracionOutput {
                Id = data.Id,
                Nombre = data.Nombre,
                Ruta = data.Ruta,
                Texto = data.Texto
            };
            return output;
        }
    }
}
