using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlIngresoSalidaRS;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ResiduosSolidos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ResiduosSolidos.ControlIngresoSalidaRS
{
    public class EditarIngresoSalidaRSRepository : IEditarIngresoSalidaRSReadOnlyRepository
    {
        private readonly Context _context;
        public EditarIngresoSalidaRSRepository(Context context)
        {
            _context = context;
        }
        public async Task EditarIngresoSalidaRS(Ingreso_Salida_RS ingreso_Salida_RS, int Id_Editado_Por)
        {
            var entity = await _context.Ingresos_Salidas_RS.FirstOrDefaultAsync(x => x.Id_Ingreso_Salida_RS.Equals(ingreso_Salida_RS.Id_Ingreso_Salida_RS));

            //entity.Item = ingreso_Salida_RS.Item;
            entity.Id_ACR_CMRS = ingreso_Salida_RS.ACR_CMRS.Id_ACR_CMRS;
            entity.Id_Responsable_ARC_CRMS = ingreso_Salida_RS.Responsable_ACR_CMRS.Id_Responsable_ACR_CRMS;
            entity.Id_Ingreso_Salida = ingreso_Salida_RS.Ingreso_Salida.Id_Ingreso_Salida;
            entity.Id_Motivo = ingreso_Salida_RS.Motivo.Id_Motivo;
            entity.Fecha_Ingreso_Salida = ingreso_Salida_RS.Fecha_Ingreso_Salida;
            entity.Id_Residuo_Solido = ingreso_Salida_RS.Residuo_Solido.Id_Residuo_Solido;
            entity.Cantidad = ingreso_Salida_RS.Cantidad;
            entity.Id_Empresa_RS = ingreso_Salida_RS.Empresa_RS.IdEmpresa;
            entity.Id_Gerencia = ingreso_Salida_RS.Gerencia.IdGerencia;
            entity.Comentario = ingreso_Salida_RS.Comentario;
            //entity.Estado = ingreso_Salida_RS.Estado;

            _context.Ingresos_Salidas_RS.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task EliminarRegistroIngresoSalidaRS(int Id_Ingreso_Salida_RS, int Id_Editado_Por)
        {
            var entity = await _context.Ingresos_Salidas_RS.FirstOrDefaultAsync(x => x.Id_Ingreso_Salida_RS.Equals(Id_Ingreso_Salida_RS));

            entity.Estado = EstadosResiduosSolidos.Eliminado.ToString();

            _context.Ingresos_Salidas_RS.Update(entity);
            await _context.SaveChangesAsync();


        }

    }
}
