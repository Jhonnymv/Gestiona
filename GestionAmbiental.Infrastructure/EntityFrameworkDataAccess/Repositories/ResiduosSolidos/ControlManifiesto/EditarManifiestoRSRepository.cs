using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ResiduosSolidos;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        public async Task CompletarManifiestoRS(Domain.ResiduosSolidos.Manifiesto_RS manifiesto_RS, int usuario)
        {            
            var entity = await _context.Manifiesto_RSs.FirstOrDefaultAsync(x => x.Id_Manifiesto_RS.Equals(manifiesto_RS.Id_Manifiesto_RS));
            
            entity.Serie_Manifiesto = manifiesto_RS.Serie_Manifiesto;
            entity.Num_Manifiesto = manifiesto_RS.Num_Manifiesto;
            entity.Fecha_Emision_Manifiesto = manifiesto_RS.Fecha_Emision_Manifiesto;
            entity.Observacion_Manifiesto = manifiesto_RS.Observacion_Manifiesto;
            entity.Id_Residuo_Solido = manifiesto_RS.Residuo_Solido.Id_Residuo_Solido;
            entity.Cantidad_RS_Evacuado = manifiesto_RS.Cantidad_RS_Evacuado;
            entity.Id_EO_RS_Salida = manifiesto_RS.EO_RS_Salida.Id_EO_RS;
            entity.Registro_EO_RS_Salida = manifiesto_RS.Registro_EO_RS_Salida;
            entity.Fecha_Vencimiento_Registro_Salida = manifiesto_RS.Fecha_Vencimiento_Registro_Salida;
            entity.Num_Autorizacion_Municipal_Salida = manifiesto_RS.Num_Autorizacion_Municipal_Salida;
            entity.Num_Autorizacion_Hoja_Ruta = manifiesto_RS.Num_Autorizacion_Hoja_Ruta;
            if (manifiesto_RS.Conductor.Id_Conductor_RS != 0)
                entity.Id_Conductor_RS = manifiesto_RS.Conductor.Id_Conductor_RS;
            else
                entity.Id_Conductor_RS = null;
            entity.Num_Licencia_Condutor = manifiesto_RS.Num_Licencia_Condutor;
            if (manifiesto_RS.Vehiculo.Id_Vehiculo_RS != 0)
                entity.Id_Vehiculo_RS = manifiesto_RS.Vehiculo.Id_Vehiculo_RS;
            else
                entity.Id_Vehiculo_RS = null;

            entity.Capacidad_Vehiculo = manifiesto_RS.Capacidad_Vehiculo;
            entity.Id_EO_RS_Llegada = manifiesto_RS.EO_RS_Llegada.Id_EO_RS;
            entity.Registro_EO_RS_Llegada = manifiesto_RS.Registro_EO_RS_Llegada;
            entity.Fecha_Vencimiento_Registro_Llegada = manifiesto_RS.Fecha_Vencimiento_Registro_Llegada;
            entity.Num_Autorizacion_Sanitaria_Llegada = manifiesto_RS.Num_Autorizacion_Sanitaria_Llegada;
            entity.Num_Autorizacion_Municipal_Llegada = manifiesto_RS.Num_Autorizacion_Municipal_Llegada;

            entity.Certificado_Trat_Disp = manifiesto_RS.Certificado_Trat_Disp;
            entity.Cantidad_RS_Dispuesto = manifiesto_RS.Cantidad_RS_Dispuesto;
            entity.Fecha_Devolucion = manifiesto_RS.Fecha_Devolucion;
            entity.Informe_Transporte = manifiesto_RS.Informe_Transporte;
            entity.Manifiesto = manifiesto_RS.Manifiesto;
            entity.Id_Asesor_MAP = manifiesto_RS.Asesor_MAP.IdUsuario;
            //entity.Estado = EstadosResiduosSolidos.Cerrado.ToString();
            entity.Estado = manifiesto_RS.Estado;

            _context.Manifiesto_RSs.Update(entity);
            await _context.SaveChangesAsync();

            var log = new Log_Manifiesto_RS();
            var bkp = JsonConvert.SerializeObject(entity);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Manifiesto_RS = entity.Id_Manifiesto_RS;
            log.Id_Usuario = usuario;
            log.Tipo = TipoAccion.UPDATE.ToString();

            _context.Log_Manifiesto_RS.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task EditarManifiestoContratista(Domain.ResiduosSolidos.Manifiesto_RS manifiesto_RS, int usuario)
        {
            try
            {
                var manifiesto_entity = await _context.Manifiesto_RSs.FirstOrDefaultAsync(x => x.Id_Manifiesto_RS.Equals(manifiesto_RS.Id_Manifiesto_RS));

                manifiesto_entity.Item = manifiesto_RS.Item.ToString();
                manifiesto_entity.Serie_Manifiesto = manifiesto_RS.Serie_Manifiesto;
                manifiesto_entity.Num_Manifiesto = manifiesto_RS.Num_Manifiesto;
                manifiesto_entity.Fecha_Emision_Manifiesto = manifiesto_RS.Fecha_Emision_Manifiesto;
                manifiesto_entity.Observacion_Manifiesto = manifiesto_RS.Observacion_Manifiesto;
                manifiesto_entity.Id_Residuo_Solido = manifiesto_RS.Residuo_Solido.Id_Residuo_Solido;
                manifiesto_entity.Cantidad_RS_Evacuado = manifiesto_RS.Cantidad_RS_Evacuado;
                manifiesto_entity.Id_EO_RS_Salida = manifiesto_RS.EO_RS_Salida.Id_EO_RS;
                manifiesto_entity.Registro_EO_RS_Salida = manifiesto_RS.Registro_EO_RS_Salida;
                manifiesto_entity.Fecha_Vencimiento_Registro_Salida = manifiesto_RS.Fecha_Vencimiento_Registro_Salida;
                manifiesto_entity.Num_Autorizacion_Municipal_Salida = manifiesto_RS.Num_Autorizacion_Municipal_Salida;
                manifiesto_entity.Num_Autorizacion_Hoja_Ruta = manifiesto_RS.Num_Autorizacion_Hoja_Ruta;
                manifiesto_entity.Id_Conductor_RS = manifiesto_RS.Conductor.Id_Conductor_RS;
                manifiesto_entity.Num_Licencia_Condutor = manifiesto_RS.Num_Licencia_Condutor;
                manifiesto_entity.Id_Vehiculo_RS = manifiesto_RS.Vehiculo.Id_Vehiculo_RS;
                manifiesto_entity.Capacidad_Vehiculo = manifiesto_RS.Capacidad_Vehiculo;
                manifiesto_entity.Id_EO_RS_Llegada = manifiesto_RS.EO_RS_Llegada.Id_EO_RS;
                manifiesto_entity.Registro_EO_RS_Llegada = manifiesto_RS.Registro_EO_RS_Llegada;
                manifiesto_entity.Fecha_Vencimiento_Registro_Llegada = manifiesto_RS.Fecha_Vencimiento_Registro_Llegada;
                manifiesto_entity.Num_Autorizacion_Sanitaria_Llegada = manifiesto_RS.Num_Autorizacion_Sanitaria_Llegada;
                manifiesto_entity.Num_Autorizacion_Municipal_Llegada = manifiesto_RS.Num_Autorizacion_Municipal_Llegada;
                manifiesto_entity.Fecha_Devolucion = null;
                /*manifiesto_entity.Certificado_Trat_Disp = manifiesto_RS.Certificado_Trat_Disp;
                manifiesto_entity.Cantidad_RS_Dispuesto = manifiesto_RS.Cantidad_RS_Dispuesto;
                manifiesto_entity.Fecha_Devolucion = manifiesto_RS.Fecha_Devolucion;
                manifiesto_entity.Informe_Transporte = manifiesto_RS.Informe_Transporte;
                manifiesto_entity.Manifiesto = manifiesto_RS.Manifiesto;
                manifiesto_entity.Id_Asesor_MAP = manifiesto_RS.Asesor_MAP.IdUsuario;*/
                //manifiesto_entity.Id_Asesor_MAP = null;
                manifiesto_entity.Id_Registrado_Por = manifiesto_RS.Id_Registrado_Por;
                manifiesto_entity.Fecha_Registro = DateTime.Now;
                manifiesto_entity.Estado = manifiesto_RS.Estado;

                _context.Manifiesto_RSs.Update(manifiesto_entity);
                await _context.SaveChangesAsync();

                var log = new Log_Manifiesto_RS();
                var bkp = JsonConvert.SerializeObject(manifiesto_entity);
                log.Bkp = bkp;
                log.Fecha = DateTime.Now;
                log.Id_Manifiesto_RS = manifiesto_entity.Id_Manifiesto_RS;
                log.Id_Usuario = usuario;
                log.Tipo = TipoAccion.UPDATE.ToString();

                _context.Log_Manifiesto_RS.Add(log);
                await _context.SaveChangesAsync();
            }catch(Exception e)
            {

            }
        }
    }
}
