using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.GestorUsuarios;
using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using Newtonsoft.Json;

namespace GestionAmbiental.Application.UseCases.Inspecciones.Registro.ConsultarInspeccion
{
    public class ConsultarInspeccionDetalle : IConsultarInspeccionDetalle
    {
        private readonly IDetalleInspeccionReadOnlyRepository _detalleInspeccionReadOnlyRepository;
        private readonly IGestorUsuarioReadOnlyRepository _gestorUsuarioReadOnlyRepository;

        public ConsultarInspeccionDetalle(
            IDetalleInspeccionReadOnlyRepository detalleInspeccionReadOnlyRepository,
            IGestorUsuarioReadOnlyRepository gestorUsuarioReadOnlyRepository)
        {
            _detalleInspeccionReadOnlyRepository = detalleInspeccionReadOnlyRepository;
            _gestorUsuarioReadOnlyRepository = gestorUsuarioReadOnlyRepository;
        }
        public async Task<List<InspeccionDetalleOutput>> Execute(int idCabecera, string usuario_red)
        {
            var output = new List<InspeccionDetalleOutput>();
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            var observaciones = await _detalleInspeccionReadOnlyRepository.GetInspeccionDetalles(idCabecera);
            foreach (var item in observaciones)
            {
                if (usuario.Id_Usuario.Equals(item.Responsable_Marcobre.Id_Usuario))
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
                if (usuario.Id_Usuario.Equals(item.Responsable_MA.Id_Usuario))
                    output.Add(new InspeccionDetalleOutput(item, true));
                else
                    output.Add(new InspeccionDetalleOutput(item, false));
            }
            return output;
        }
        public async Task<List<InspeccionDetalleOutput>> Execute(int idCabecera)
        {
            var output = new List<InspeccionDetalleOutput>();
            var observaciones = await _detalleInspeccionReadOnlyRepository.GetInspeccionDetalles(idCabecera);
            foreach (var item in observaciones)
            {
                output.Add(new InspeccionDetalleOutput(item, false));
            }
            return output;
        }
    }
}
