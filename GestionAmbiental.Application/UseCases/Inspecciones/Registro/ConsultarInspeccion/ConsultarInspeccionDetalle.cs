using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using Newtonsoft.Json;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarInspeccion
{
    public class ConsultarInspeccionDetalle : IConsultarInspeccionDetalle
    {
        private readonly IDetalleInspeccionReadOnlyRepository _detalleInspeccionReadOnlyRepository;
        private readonly ICabeceraInspeccionReadOnlyRepository _cabeceraInspeccionReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public ConsultarInspeccionDetalle(
            IDetalleInspeccionReadOnlyRepository detalleInspeccionReadOnlyRepository,
            IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository,
            ICabeceraInspeccionReadOnlyRepository cabeceraInspeccionReadOnlyRepository)
        {
            _detalleInspeccionReadOnlyRepository = detalleInspeccionReadOnlyRepository;
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository;
            _cabeceraInspeccionReadOnlyRepository = cabeceraInspeccionReadOnlyRepository;
        }
        public async Task<List<InspeccionDetalleOutput>> Execute(int idCabecera, string usuario_red)
        {
            var output = new List<InspeccionDetalleOutput>();
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var observaciones = await _detalleInspeccionReadOnlyRepository.GetInspeccionDetalles(idCabecera);
            foreach (var item in observaciones)
            {
                if (usuario.IdUsuario.Equals(item.ResponsableMarcobre.IdUsuario))
                    output.Add(new InspeccionDetalleOutput(item, true));
                else
                    output.Add(new InspeccionDetalleOutput(item, false));
            }
            return output;
        }
        public async Task<List<InspeccionDetalleOutput>> ExecuteVerificacion(int idCabecera, string usuario_red)
        {
            var output = new List<InspeccionDetalleOutput>();
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var observaciones = await _detalleInspeccionReadOnlyRepository.GetInspeccionDetalles(idCabecera);
            foreach (var item in observaciones)
            {
                if (usuario.IdUsuario.Equals(item.Asesor.IdUsuario))
                    output.Add(new InspeccionDetalleOutput(item, true));
                else
                    output.Add(new InspeccionDetalleOutput(item, false));
            }
            return output;
        }
        public async Task<List<InspeccionDetalleOutput>> ExecuteRegistro(int idCabecera,string usuario_red)
        {
            var output = new List<InspeccionDetalleOutput>();
            var observaciones = await _detalleInspeccionReadOnlyRepository.GetInspeccionDetalles(idCabecera);
            var cabecera = await _cabeceraInspeccionReadOnlyRepository.GetInspeccionCabecera(idCabecera);
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            foreach (var item in observaciones)
            {
                var outputItem = new InspeccionDetalleOutput(item, usuario.IdUsuario.Equals(cabecera.IdRegistradoPor)? true:false);

                if (usuario.IdRol.Equals((int)InspeccionRol.Admin))
                    outputItem.PermisosAdmin = true;
                else if (item.Asesor.IdUsuario.Equals(usuario.IdUsuario))
                    outputItem.Permisos = true;
                else
                    outputItem.PermisosAdmin = false;

                output.Add(outputItem);
            }
            return output;
        }
    }
}
