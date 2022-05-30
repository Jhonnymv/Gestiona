using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.FOCAM.Tipo;

namespace GestionAmbiental.Application.UseCases.FOCAM.Tipo.NuevoRegistro
{
    public class NuevoRegistro:INuevoRegistro
    {
        private readonly ITipoReadOnlyRepository _tipoReadOnlyRepository;

        public NuevoRegistro(ITipoReadOnlyRepository tipoReadOnlyRepository)
        {
            _tipoReadOnlyRepository = tipoReadOnlyRepository;
        }

        public async Task<int> Execute(string Nombre, string Descripcion)
        {
            var input = new Domain.FOCAM.Tipo();
            input.Nombre = Nombre;
            input.Descripcion = Descripcion;

            return await _tipoReadOnlyRepository.RegistrarTipo(input);
        }
    }
}
