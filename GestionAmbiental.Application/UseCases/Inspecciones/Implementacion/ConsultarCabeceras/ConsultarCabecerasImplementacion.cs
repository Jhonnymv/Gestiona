using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Implementacion.ConsultarCabeceras
{
    public class ConsultarCabecerasImplementacion : IConsultarCabecerasImplementacion
    {
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuariosReadOnlyRepository;
        public ConsultarCabecerasImplementacion(
            ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository,
            IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepositor
            )
        {
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository;
            _gestorUsuariosReadOnlyRepository = gestorUsuarioReadOnlyRepositor;
        }
        public async Task<List<InspeccionCabecera>> Execute(int pagina, int fpp, DateTime fecha, int gerencia, string codigo,string usuario_red)
        {
            var usuario = await _gestorUsuariosReadOnlyRepository.GetUsuario(usuario_red);
            var cabeceras = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabeceraImplementacion(pagina, fpp, fecha, gerencia, codigo,usuario.Id_Usuario);
            return cabeceras;
        }
    }
}
