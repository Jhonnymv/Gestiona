using GestionAmbiental.Application.Repositories.Notificaciones;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Notificaciones
{
    public class GetInspeccionDetalleInfoRepository : IGetInspeccionDetalleInfoReadOnlyRepository
    {
        private Context _context;

        public GetInspeccionDetalleInfoRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<InspeccionDetalle> GetInspeccionDetalle(int IdObservacion)
        {
            var inspeccionDetalle = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(IdObservacion));
            var tipoCondicion = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(inspeccionDetalle.Id_Tipo_Condicion));
            var tipoCondicion_Domain = new Tipo_Condicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion);
            var responsableMarcobre = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(inspeccionDetalle.Id_Responsable_Marcobre));
            var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario, responsableMarcobre.Id_Rol, responsableMarcobre.Nombres, responsableMarcobre.Apellidos, responsableMarcobre.Email);
            var detalle = new InspeccionDetalle(
                inspeccionDetalle.Id_Inspeccion_Cabecera,
                inspeccionDetalle.Id_Inspeccion_Detalle,
                inspeccionDetalle.Descripcion_Condicion,
                tipoCondicion_Domain,
                inspeccionDetalle.Evidencia_Condicion,
                inspeccionDetalle.Nivel_Riesgo_Ambiental,
                inspeccionDetalle.Descripcion_Accion,
                responsableMarcobre_Domain,
                inspeccionDetalle.Responsable_Contratista,
                inspeccionDetalle.Fecha_Programada
                );
            return detalle;

        }
    }
}
