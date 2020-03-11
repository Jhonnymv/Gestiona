using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlIngresoSalidaRS;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ResiduosSolidos;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ResiduosSolidos.ControlIngresoSalidaRS
{
    public class RegistrarIngresoSalidaRSRepository : IRegistrarIngresoSalidaRSReadOnlyRepository
    {
        private readonly Context _context;
        public RegistrarIngresoSalidaRSRepository(Context context)
        {
            _context = context;
        }

        public async Task<int> RegistrarIngresoSalida(Domain.ResiduosSolidos.Ingreso_Salida_RS ingreso_Salida_RS)
        {
            var entity = new Entities.ResiduosSolidos.Ingreso_Salida_RS();
            entity.Item = ingreso_Salida_RS.Item;
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
            entity.Id_Registrado_Por = ingreso_Salida_RS.Id_Registrado_Por;
            entity.Fecha_Registro = ingreso_Salida_RS.Fecha_Registro;
            entity.Estado = ingreso_Salida_RS.Estado;

            _context.Ingresos_Salidas_RS.Add(entity);
            await _context.SaveChangesAsync();

            var log = new Log_Ingreso_Salida_RS();
            var bkp = JsonConvert.SerializeObject(entity);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Ingreso_Salida_RS = entity.Id_Ingreso_Salida_RS;
            log.Id_Usuario = entity.Id_Registrado_Por;
            log.Tipo = TipoAccion.CREATE.ToString();

            _context.Log_Ingreso_Salida_RSs.Add(log);
            await _context.SaveChangesAsync();

            return entity.Id_Ingreso_Salida_RS;
        }
    }
}
