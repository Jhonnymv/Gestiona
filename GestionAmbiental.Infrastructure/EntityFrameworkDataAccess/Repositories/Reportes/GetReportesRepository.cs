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
            var inspecciones = await _context.Inspecciones_Detalle.Where(x => cabeceras_registradas.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)).ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            var output = new List<Dimension>();
            foreach(var item in usuarios) {
                var nObservaciones = inspecciones.Where(x => x.Id_Responsable_Marcobre.Equals(item.Id_Usuario)).Count();
                if(nObservaciones >0)
                    output.Add(new Dimension(item.Nombres + " " + item.Apellidos, nObservaciones));
            }
            return output;
        }
    }
}
