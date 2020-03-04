using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using GestionAmbiental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ResiduosSolidos.ControlManifiesto
{
    public class EditarManifiestoRSRepository : IEditarManifiestoRSReadOnlyRepository
    {
        private readonly Context _context;
        public EditarManifiestoRSRepository(Context context)
        {
            _context = context;
        }
        public async Task CompletarManifiestoRS(Domain.ResiduosSolidos.Manifiesto_RS manifiesto_RS)
        {
            var entity = await _context.Manifiesto_RSs.FirstOrDefaultAsync(x => x.Id_Manifiesto_RS.Equals(manifiesto_RS.Id_Manifiesto_RS));
            entity.Certificado_Trat_Disp = manifiesto_RS.Certificado_Trat_Disp;
            entity.Cantidad_RS_Dispuesto = manifiesto_RS.Cantidad_RS_Dispuesto;
            entity.Fecha_Devolucion = manifiesto_RS.Fecha_Devolucion;
            entity.Informe_Transporte = manifiesto_RS.Informe_Transporte;
            entity.Manifiesto = manifiesto_RS.Manifiesto;
            entity.Id_Asesor_MAP = manifiesto_RS.Asesor_MAP.IdUsuario;
            entity.Estado = EstadosResiduosSolidos.Cerrado.ToString();

            _context.Manifiesto_RSs.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
