using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarCabeceras
{
    public class ConsultarCabecera : IConsultarCabecera
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;
        public ConsultarCabecera(ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository, IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository;
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository;
    }
        public async Task<InspeccionCabecera> Execute(int idCabecera,string usuario_red)
        {
            var output = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabecera(idCabecera);
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            if (usuario.IdUsuario.Equals(output.IdRegistradoPor))
                output.Permiso = true;
            else
                output.Permiso = false;

                return output;
        }
    }
}
