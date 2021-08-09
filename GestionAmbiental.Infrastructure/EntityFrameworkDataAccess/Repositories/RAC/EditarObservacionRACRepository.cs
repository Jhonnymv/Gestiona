using GestionAmbiental.Application.Repositories.RAC;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.RAC;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.RAC
{
    public class EditarObservacionRACRepository : IEditarObservacionRACReadOnlyRepository
    {
        private readonly Context _context;

        public EditarObservacionRACRepository(Context context)
        {
            _context = context;
        }

        public async Task EditarRAC(Domain.RAC.RAC rac, int idUsuario)
        {
            var rac_entity = await _context.RACs.FirstOrDefaultAsync(x => x.Id_RAC.Equals(rac.Id_RAC));
            if (rac_entity != null)
            {
                if (rac.Nombres_Apellidos != null)
                    rac_entity.Nombres_Apellidos = rac.Nombres_Apellidos;
                if (rac.Id_Gerencia != null)
                    rac_entity.Id_Gerencia = rac.Id_Gerencia;
                if (rac.Id_Empresa != null)
                    rac_entity.Id_Empresa = rac.Id_Empresa;
                if (rac.Otra_Empresa != null)
                    rac_entity.Otra_Empresa = rac.Otra_Empresa;
                rac_entity.Id_Empresa_Obs = rac.Id_Empresa_Obs;
                rac_entity.Id_Gerencia_Obs = rac.Id_Gerencia_Obs;
                if (rac.Ubicacion != null)
                    rac_entity.Ubicacion = rac.Ubicacion;
                if (rac.SSO_MA != null)
                    rac_entity.SSO_MA = rac.SSO_MA;
                if (rac.Clasificacion != null)
                    rac_entity.Clasificacion = rac.Clasificacion;
                if (rac.NivelRiesgo != null)
                    rac_entity.NivelRiesgo = rac.NivelRiesgo;
                rac_entity.Id_Tipo_Condicion = rac.Id_Tipo_Condicion;
                if (rac.Descripcion_Acto_Cond != null)
                    rac_entity.Descripcion_Acto_Cond = rac.Descripcion_Acto_Cond;
                if (rac.Medida_Control != null)
                    rac_entity.Medida_Control = rac.Medida_Control;
                if (rac.PAC != null)
                    rac_entity.PAC = rac.PAC;
                rac_entity.Id_Responsable_Cierre = rac.Id_Responsable_Cierre;
                rac_entity.Id_Supervisor = rac.Id_Supervisor;
                if (rac.Evidencia_Obs != "")
                    rac_entity.Evidencia_Obs = rac.Evidencia_Obs;
                if (rac.Evidencia_Cierre != "")
                    rac_entity.Evidencia_Cierre = rac.Evidencia_Cierre;
                if (rac.Comentario_Cierre != null)
                    rac_entity.Comentario_Cierre = rac.Comentario_Cierre;
                if (rac.Comentario_Verificacion != null)
                    rac_entity.Comentario_Verificacion = rac.Comentario_Verificacion;
                if (rac.Fecha_Cierre != null)
                {
                    if (rac_entity.Estado.Equals(EstadosRAC.Vencido.ToString()) && rac_entity.Fecha_Cierre != rac.Fecha_Cierre)
                        rac_entity.Estado = EstadosRAC.Ampliado.ToString();
                    rac_entity.Fecha_Cierre = rac.Fecha_Cierre;
                    
                }
                if (rac.Fecha != null)
                    rac_entity.Fecha = rac.Fecha;
                _context.RACs.Update(rac_entity);
                await _context.SaveChangesAsync();

                var log = new Log_RAC();
                var bkp = JsonConvert.SerializeObject(rac_entity);
                log.Bkp = bkp;
                log.Fecha = DateTime.Now;
                log.Id_RAC = rac_entity.Id_RAC;
                log.Id_Usuario = idUsuario;
                log.Tipo = TipoAccion.UPDATE.ToString();

                _context.Log_RACs.Add(log);
                await _context.SaveChangesAsync();
            }
        }
        public async Task AmpliarFechaObservacionRAC(int idRAC, DateTime nuevaFecha,int idUsuario)
        {
            var rac_entity = await _context.RACs.FirstOrDefaultAsync(x => x.Id_RAC.Equals(idRAC));
            if (rac_entity != null)
            {
                rac_entity.Fecha_Cierre = nuevaFecha;
                rac_entity.Estado = EstadosRAC.Ampliado.ToString();
                _context.RACs.Update(rac_entity);
                await _context.SaveChangesAsync();

                var log = new Log_RAC();
                var bkp = JsonConvert.SerializeObject(rac_entity);
                log.Bkp = bkp;
                log.Fecha = DateTime.Now;
                log.Id_RAC = rac_entity.Id_RAC;
                log.Id_Usuario = idUsuario;
                log.Tipo = TipoAccion.UPDATE.ToString();

                _context.Log_RACs.Add(log);
                await _context.SaveChangesAsync();
            }
        }
        public async Task EliminarRAC(int idRAC,int idUsuario)
        {
            var rac_entity = await _context.RACs.FirstOrDefaultAsync(x => x.Id_RAC.Equals(idRAC));
            if (rac_entity != null)
            {
                rac_entity.Estado = EstadosRAC.Eliminado.ToString();
                _context.RACs.Update(rac_entity);
                await _context.SaveChangesAsync();

                var log = new Log_RAC();
                var bkp = JsonConvert.SerializeObject(rac_entity);
                log.Bkp = bkp;
                log.Fecha = DateTime.Now;
                log.Id_RAC = rac_entity.Id_RAC;
                log.Id_Usuario = idUsuario;
                log.Tipo = TipoAccion.DELETE.ToString();

                _context.Log_RACs.Add(log);
                await _context.SaveChangesAsync();
            }
        }
    }
}
