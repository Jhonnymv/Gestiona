using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Reportes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Reportes
{
    public class GetReportesRepository : IGetReportesReadOnlyRepository
    {
        private Context _context;

        public GetReportesRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Dimension> GetObservacionesMAP()
        {
            var data = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("MAP")).CountAsync();
            var output = new Dimension();
            output.Nombre = "MAP";
            output.Valor = data;
            return output;
        }
        public async Task<Dimension> GetObservacionesSSO()
        {
            var data = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("SSO")).CountAsync();
            var output = new Dimension();
            output.Nombre = "SSO";
            output.Valor = data;
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesXUsuario()
        {
            var cabeceras_registradas = await _context.Inspecciones_Cabecera.Where(x => x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).ToListAsync();
            var observaciones = await _context.Inspecciones_Detalle.Where(x => cabeceras_registradas.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)).ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            var output = new List<Dimension>();
            foreach(var item in usuarios) {
                var nObservaciones = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(item.Id_Usuario)).Count();
                if(nObservaciones >0)
                    output.Add(new Dimension(item.Nombres + " " + item.Apellidos, nObservaciones));
            }
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesXEstado()
        {
            var cabeceras_registradas = await _context.Inspecciones_Cabecera.Where(x => x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).ToListAsync();
            var inspecciones = await _context.Inspecciones_Detalle.Where(x => cabeceras_registradas.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)).ToListAsync();

            var output = new List<Dimension>();

            foreach (var item in Enum.GetNames(typeof(EstadosInspeccion)))
            {
                output.Add(new Dimension(item, inspecciones.Where(x => x.Estado.Equals(item)).Count()));
            }

            return output;
        }
        public async Task<List<ObservacionesUsuario>> GetResumenObservacionesXUsuario(string filtro)
        {
            var output = new List<ObservacionesUsuario>();

            var cabeceras_registradas = await _context.Inspecciones_Cabecera.Where(x => x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).ToListAsync();
            var observaciones = await _context.Inspecciones_Detalle.Where(x => cabeceras_registradas.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)).ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            foreach(var usuario in usuarios)
            {
                var detalle = new ObservacionesUsuario();
                detalle.usuario = new Domain.ValueObjects.Usuario(usuario.Id_Usuario,usuario.Id_Rol,usuario.Nombres,usuario.Apellidos,usuario.Email);
                detalle.pendientes = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(usuario.Id_Usuario) && x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).Count();
                detalle.implementadas = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(usuario.Id_Usuario) && x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).Count();
                detalle.aprobadas = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(usuario.Id_Usuario) && x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).Count();
                detalle.rechazadas = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(usuario.Id_Usuario) && x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).Count();

                output.Add(detalle);
            }

            return output;
        }
    }
}
