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
                var outputItem = new InspeccionDetalleOutput(item);
                //Implementar
                if (usuario.IdUsuario.Equals(item.ResponsableMarcobre.IdUsuario)||(item.ResponsableMarcobre2!=null && usuario.IdUsuario.Equals(item.ResponsableMarcobre2.IdUsuario)))
                    outputItem.PermisosImplementar = true;
                else
                    outputItem.PermisosImplementar = false;
                //Asesor
                if (usuario.IdUsuario.Equals(item.Asesor.IdUsuario))
                    outputItem.PermisosAsesor = true;
                else
                    outputItem.PermisosAsesor = false;
                //Admin
                if (usuario.IdRol.Equals((int)InspeccionRol.Admin))
                    outputItem.PermisosAdmin = true;
                else
                    outputItem.PermisosAdmin = false;

                output.Add(outputItem);
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
                var outputItem = new InspeccionDetalleOutput(item);
                //Implementar
                if (usuario.IdUsuario.Equals(item.ResponsableMarcobre.IdUsuario) || (item.ResponsableMarcobre2 != null && usuario.IdUsuario.Equals(item.ResponsableMarcobre2.IdUsuario)))
                    outputItem.PermisosImplementar = true;
                else
                    outputItem.PermisosImplementar = false;
                //Asesor
                if (usuario.IdUsuario.Equals(item.Asesor.IdUsuario))
                    outputItem.PermisosAsesor = true;
                else
                    outputItem.PermisosAsesor = false;
                //Admin
                if (usuario.IdRol.Equals((int)InspeccionRol.Admin))
                    outputItem.PermisosAdmin = true;
                else
                    outputItem.PermisosAdmin = false;

                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<InspeccionDetalleOutput>> ExecuteRegistro(int idCabecera,string usuario_red)
        {
            var output = new List<InspeccionDetalleOutput>();
            var observaciones = await _detalleInspeccionReadOnlyRepository.GetInspeccionDetalles(idCabecera);
            var usuario = await _gestorUsuarioReadOnlyRepository.GetUsuario(usuario_red);
            foreach (var item in observaciones)
            {        
                var outputItem = new InspeccionDetalleOutput(item);
                //Implementar
                if (usuario.IdUsuario.Equals(item.ResponsableMarcobre.IdUsuario) || (item.ResponsableMarcobre2 != null && usuario.IdUsuario.Equals(item.ResponsableMarcobre2.IdUsuario)))
                    outputItem.PermisosImplementar = true;
                else
                    outputItem.PermisosImplementar = false;
                //Asesor
                if (usuario.IdUsuario.Equals(item.Asesor.IdUsuario))
                    outputItem.PermisosAsesor = true;
                else
                    outputItem.PermisosAsesor = false;
                //Admin
                if (usuario.IdRol.Equals((int)InspeccionRol.Admin))
                    outputItem.PermisosAdmin = true;
                else
                    outputItem.PermisosAdmin = false;

                output.Add(outputItem);
            }
            return output;
        }
    }
}
