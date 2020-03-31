using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.ResiduosSolidos;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ResiduosSolidos.ControlManifiesto
{
    public class RegistrarManifiestoRSRepository : IRegistrarManifiestoRSReadOnlyRepository
    {
        private readonly Context _context;
        public RegistrarManifiestoRSRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<int> RegistrarManifiestosRS(Domain.ResiduosSolidos.Manifiesto_RS manifiesto_RS)
        {
            
                var manifiesto_entity = new Manifiesto_RS();

                manifiesto_entity.Item = manifiesto_RS.Item.ToString();
                manifiesto_entity.Serie_Manifiesto = manifiesto_RS.Serie_Manifiesto;
                manifiesto_entity.Num_Manifiesto = manifiesto_RS.Num_Manifiesto;
                manifiesto_entity.Fecha_Emision_Manifiesto = manifiesto_RS.Fecha_Emision_Manifiesto;
                manifiesto_entity.Observacion_Manifiesto = manifiesto_RS.Observacion_Manifiesto;
            if (manifiesto_entity.Observacion_Manifiesto.Equals("Anulado"))
            {
                manifiesto_entity.Id_Residuo_Solido = null;
                manifiesto_entity.Cantidad_RS_Evacuado = null;
                manifiesto_entity.Id_EO_RS_Salida = null;
                manifiesto_entity.Registro_EO_RS_Salida = null;
                manifiesto_entity.Fecha_Vencimiento_Registro_Salida = null;
                manifiesto_entity.Num_Autorizacion_Municipal_Salida = null;
                manifiesto_entity.Num_Autorizacion_Hoja_Ruta = null;
                manifiesto_entity.Id_Conductor_RS = null;
                manifiesto_entity.Num_Licencia_Condutor = null;
                manifiesto_entity.Id_Vehiculo_RS = null;
                manifiesto_entity.Capacidad_Vehiculo = null;
                manifiesto_entity.Id_EO_RS_Llegada = null;
                manifiesto_entity.Registro_EO_RS_Llegada = null;
                manifiesto_entity.Fecha_Vencimiento_Registro_Llegada = null;
                manifiesto_entity.Num_Autorizacion_Sanitaria_Llegada = null;
                manifiesto_entity.Num_Autorizacion_Municipal_Llegada = null;
                manifiesto_entity.Fecha_Devolucion = null;
            }
            else
            {
                manifiesto_entity.Id_Residuo_Solido = manifiesto_RS.Residuo_Solido.Id_Residuo_Solido;
                manifiesto_entity.Cantidad_RS_Evacuado = manifiesto_RS.Cantidad_RS_Evacuado;
                manifiesto_entity.Id_EO_RS_Salida = manifiesto_RS.EO_RS_Salida.Id_EO_RS;
                manifiesto_entity.Registro_EO_RS_Salida = manifiesto_RS.Registro_EO_RS_Salida;
                manifiesto_entity.Fecha_Vencimiento_Registro_Salida = manifiesto_RS.Fecha_Vencimiento_Registro_Salida;
                manifiesto_entity.Num_Autorizacion_Municipal_Salida = manifiesto_RS.Num_Autorizacion_Municipal_Salida;
                manifiesto_entity.Num_Autorizacion_Hoja_Ruta = manifiesto_RS.Num_Autorizacion_Hoja_Ruta;
                if (manifiesto_RS.Conductor.Id_Conductor_RS != 0)
                    manifiesto_entity.Id_Conductor_RS = manifiesto_RS.Conductor.Id_Conductor_RS;
                else
                    manifiesto_entity.Id_Conductor_RS = null;
                manifiesto_entity.Num_Licencia_Condutor = manifiesto_RS.Num_Licencia_Condutor;
                if (manifiesto_RS.Vehiculo.Id_Vehiculo_RS != 0)
                    manifiesto_entity.Id_Vehiculo_RS = manifiesto_RS.Vehiculo.Id_Vehiculo_RS;
                else
                    manifiesto_entity.Id_Vehiculo_RS = null;

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
            }

            manifiesto_entity.Id_Registrado_Por = manifiesto_RS.Id_Registrado_Por;
            manifiesto_entity.Fecha_Registro = DateTime.Now;
            manifiesto_entity.Estado = manifiesto_RS.Estado;
            
            _context.Manifiesto_RSs.Add(manifiesto_entity);
            await _context.SaveChangesAsync();

            var log = new Log_Manifiesto_RS();
            var bkp = JsonConvert.SerializeObject(manifiesto_entity);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Manifiesto_RS = manifiesto_entity.Id_Manifiesto_RS;
            log.Id_Usuario = manifiesto_entity.Id_Registrado_Por;
            log.Tipo = TipoAccion.CREATE.ToString();

            _context.Log_Manifiesto_RS.Add(log);
            await _context.SaveChangesAsync();

            return manifiesto_entity.Id_Manifiesto_RS;

        }       
    }
}
