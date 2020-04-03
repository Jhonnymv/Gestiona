using GestionAmbiental.Application.Repositories.ResiduosSolidos.ControlManifiestoRS;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.ResiduosSolidos;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.ResiduosSolidos.ControlManifiesto
{
    public class GetManifiestosRSRepository : IGetManifiestosRSReadOnlyRepository
    {
        private readonly Context _context;

        public GetManifiestosRSRepository(Context context)
        {
            _context = context;
        }
        public async Task<List<Manifiesto_RS>> GetManifiestosRS(string nManifiesto, DateTime? fechaEmision, string observacion, int residuoSolido, int eors_salida, int eors_llegada, int asesorMAP)
        {
            var manifiestos = await _context.Manifiesto_RSs.ToListAsync();
           
            var data = manifiestos.Where(x =>
                x.Num_Manifiesto.Contains(nManifiesto != null ? nManifiesto : x.Num_Manifiesto) &&
                x.Fecha_Emision_Manifiesto.Equals(fechaEmision != null ? fechaEmision.Value : x.Fecha_Emision_Manifiesto) &&
                x.Observacion_Manifiesto.Equals(observacion!=null?observacion:x.Observacion_Manifiesto) &&
                x.Id_Residuo_Solido.Equals(residuoSolido!=0?residuoSolido:x.Id_Residuo_Solido)&&
                x.Id_EO_RS_Salida.Equals(eors_salida!=0?eors_salida:x.Id_EO_RS_Salida)&&
                x.Id_EO_RS_Llegada.Equals(eors_llegada!=0? eors_llegada : x.Id_EO_RS_Llegada)&&
                x.Id_Asesor_MAP.Equals(asesorMAP!=0?asesorMAP:x.Id_Asesor_MAP)&&
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()))
                .ToList();
            var residuosSolidos = await _context.Residuos_Solidos.ToListAsync();
            var tiposResiduosSolidos = await _context.Tipo_Residuos.ToListAsync();
            var eors = await _context.EO_RSs.ToArrayAsync();
            var usuarios = await _context.Usuarios.ToArrayAsync();
            var conductores = await _context.Condutores_RS.ToListAsync();
            var vehiculos = await _context.Vehiculos_RS.ToListAsync();

            var output = new List<Manifiesto_RS>();
            var tbd = new List<Manifiesto_RS>();
            var registrados = new List<Manifiesto_RS>();

            foreach (var item in data)
            {
                var outputItem = new Manifiesto_RS();
                outputItem.Id_Manifiesto_RS = item.Id_Manifiesto_RS;
                outputItem.Item = item.Item;
                outputItem.Serie_Manifiesto = item.Serie_Manifiesto;
                outputItem.Num_Manifiesto = item.Num_Manifiesto;
                outputItem.Fecha_Emision_Manifiesto = item.Fecha_Emision_Manifiesto;
                outputItem.Observacion_Manifiesto = item.Observacion_Manifiesto;

                if (item.Observacion_Manifiesto.Equals("Conforme"))
                {
                    var itemResSolid = residuosSolidos.FirstOrDefault(x => x.Id_Residuo_Solido.Equals(item.Id_Residuo_Solido));
                    var itemTipoResSolid = tiposResiduosSolidos.FirstOrDefault(x => x.Id_Tipo_Residuo.Equals(itemResSolid.Id_Tipo_Residuo));
                    var itemEORS_Salida = eors.FirstOrDefault(x => x.Id_EO_RS.Equals(item.Id_EO_RS_Salida));
                    var itemEORS_Llegada = eors.FirstOrDefault(x => x.Id_EO_RS.Equals(item.Id_EO_RS_Llegada));
                    var itemConductor = conductores.FirstOrDefault(x => x.Id_Conductor_RS.Equals(item.Id_Conductor_RS));
                    var itemVehiculo = vehiculos.FirstOrDefault(x => x.Id_Vehiculo_RS.Equals(item.Id_Vehiculo_RS));

                    //var asesor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Asesor_MAP));                   
                   
                    outputItem.Residuo_Solido = new Residuo_Solido(
                        itemResSolid.Id_Residuo_Solido,
                        new Tipo_Residuo(itemTipoResSolid.Id_Tipo_Residuo, itemTipoResSolid.Descripcion, itemTipoResSolid.Estado),
                        itemResSolid.Descripcion,
                        itemResSolid.Estado);
                    outputItem.Cantidad_RS_Evacuado = item.Cantidad_RS_Evacuado.Value;
                    outputItem.EO_RS_Salida = new EO_RS(itemEORS_Salida.Id_EO_RS, itemEORS_Salida.Razon_Social, itemEORS_Salida.Registro, itemEORS_Salida.Fecha_Vencimiento_Registro, itemEORS_Salida.Estado);
                    outputItem.Registro_EO_RS_Salida = item.Registro_EO_RS_Salida;
                    outputItem.Fecha_Vencimiento_Registro_Salida = item.Fecha_Vencimiento_Registro_Salida.Value;
                    outputItem.Num_Autorizacion_Municipal_Salida = item.Num_Autorizacion_Municipal_Salida;
                    outputItem.Num_Autorizacion_Hoja_Ruta = item.Num_Autorizacion_Hoja_Ruta;
                    outputItem.Conductor = itemConductor != null ? new Conductor_RS(itemConductor.Id_Conductor_RS, itemConductor.Nombres, itemConductor.Apellidos, itemConductor.NLicencia, itemConductor.Estado) : new Conductor_RS();
                    outputItem.Num_Licencia_Condutor = item.Num_Licencia_Condutor;
                    outputItem.Vehiculo = itemVehiculo != null ? new Vehiculo_RS(itemVehiculo.Id_Vehiculo_RS, itemVehiculo.Placa, itemVehiculo.Capacidad, itemVehiculo.Estado) : new Vehiculo_RS();
                    outputItem.Capacidad_Vehiculo = item.Capacidad_Vehiculo ?? 0;
                    outputItem.EO_RS_Llegada = new EO_RS(itemEORS_Llegada.Id_EO_RS, itemEORS_Llegada.Razon_Social, itemEORS_Llegada.Registro, itemEORS_Llegada.Fecha_Vencimiento_Registro, itemEORS_Llegada.Estado);
                    outputItem.Registro_EO_RS_Llegada = item.Registro_EO_RS_Llegada;
                    outputItem.Fecha_Vencimiento_Registro_Llegada = item.Fecha_Vencimiento_Registro_Llegada.Value;
                    outputItem.Num_Autorizacion_Sanitaria_Llegada = item.Num_Autorizacion_Sanitaria_Llegada;
                    outputItem.Num_Autorizacion_Municipal_Llegada = item.Num_Autorizacion_Municipal_Llegada;
                    /*
                    outputItem.Certificado_Trat_Disp = item.Certificado_Trat_Disp;
                    outputItem.Cantidad_RS_Dispuesto = item.Cantidad_RS_Dispuesto;
                    outputItem.Fecha_Devolucion = item.Fecha_Devolucion??item.Fecha_Devolucion.Value;
                    outputItem.Informe_Transporte = item.Informe_Transporte;
                    outputItem.Manifiesto = item.Manifiesto;
                    outputItem.Asesor_MAP = new Usuario(asesor.Id_Usuario, asesor.Id_Rol, asesor.Nombres, asesor.Apellidos, asesor.Email);
                    */                    
                }
                outputItem.Id_Registrado_Por = item.Id_Registrado_Por;
                outputItem.Fecha_Registro = item.Fecha_Registro;
                outputItem.Estado = item.Estado;

                if (outputItem.Item == "TBD")
                    tbd.Add(outputItem);
                else
                    registrados.Add(outputItem);
            }
            registrados = registrados.OrderByDescending(x => Convert.ToInt32(x.Item)).ToList();

            output.AddRange(tbd);
            output.AddRange(registrados);

            return output;
        }
        public async Task<Manifiesto_RS> GetManifiestoRS(int IdManifiestoRS)
        {
            var manifiesto = await _context.Manifiesto_RSs.FirstOrDefaultAsync(x => x.Id_Manifiesto_RS.Equals(IdManifiestoRS));

            var manifiestoResSolid = await _context.Residuos_Solidos.FirstOrDefaultAsync(x => x.Id_Residuo_Solido.Equals(manifiesto.Id_Residuo_Solido));

            var output = new Manifiesto_RS();
            
            output.Id_Manifiesto_RS = manifiesto.Id_Manifiesto_RS;
            output.Item = manifiesto.Item;
            output.Serie_Manifiesto = manifiesto.Serie_Manifiesto;
            output.Num_Manifiesto = manifiesto.Num_Manifiesto;
            output.Fecha_Emision_Manifiesto = manifiesto.Fecha_Emision_Manifiesto;
            output.Observacion_Manifiesto = manifiesto.Observacion_Manifiesto;
            if (manifiesto.Observacion_Manifiesto.Equals("Conforme"))
            {
                var manifiestoTipoResSolid = await _context.Tipo_Residuos.FirstOrDefaultAsync(x => x.Id_Tipo_Residuo.Equals(manifiestoResSolid.Id_Tipo_Residuo));
                var manifiestoEORS_Salida = await _context.EO_RSs.FirstOrDefaultAsync(x => x.Id_EO_RS.Equals(manifiesto.Id_EO_RS_Salida));
                var manifiestoEORS_Llegada = await _context.EO_RSs.FirstOrDefaultAsync(x => x.Id_EO_RS.Equals(manifiesto.Id_EO_RS_Llegada));
                var asesor = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(manifiesto.Id_Asesor_MAP));
                var conductor = await _context.Condutores_RS.FirstOrDefaultAsync(x => x.Id_Conductor_RS.Equals(manifiesto.Id_Conductor_RS));
                var vehiculo = await _context.Vehiculos_RS.FirstOrDefaultAsync(x => x.Id_Vehiculo_RS.Equals(manifiesto.Id_Vehiculo_RS));

                output.Residuo_Solido = new Residuo_Solido(
                manifiestoResSolid.Id_Residuo_Solido,
                new Tipo_Residuo(manifiestoTipoResSolid.Id_Tipo_Residuo, manifiestoTipoResSolid.Descripcion, manifiestoTipoResSolid.Estado),
                manifiestoResSolid.Descripcion,
                manifiestoResSolid.Estado);
                output.Cantidad_RS_Evacuado = manifiesto.Cantidad_RS_Evacuado.Value;
                output.EO_RS_Salida = new EO_RS(manifiestoEORS_Salida.Id_EO_RS, manifiestoEORS_Salida.Razon_Social, manifiestoEORS_Salida.Registro, manifiestoEORS_Salida.Fecha_Vencimiento_Registro, manifiestoEORS_Salida.Estado);
                output.Registro_EO_RS_Salida = manifiesto.Registro_EO_RS_Salida;
                output.Fecha_Vencimiento_Registro_Salida = manifiesto.Fecha_Vencimiento_Registro_Salida.Value;
                output.Num_Autorizacion_Municipal_Salida = manifiesto.Num_Autorizacion_Municipal_Salida;
                output.Num_Autorizacion_Hoja_Ruta = manifiesto.Num_Autorizacion_Hoja_Ruta;
                output.Conductor = conductor != null ? new Conductor_RS(conductor.Id_Conductor_RS, conductor.Nombres, conductor.Apellidos, conductor.NLicencia, conductor.Estado) : new Conductor_RS();
                output.Num_Licencia_Condutor = manifiesto.Num_Licencia_Condutor;
                output.Vehiculo = vehiculo != null ? new Vehiculo_RS(vehiculo.Id_Vehiculo_RS, vehiculo.Placa, vehiculo.Capacidad, vehiculo.Estado) : new Vehiculo_RS();
                output.Capacidad_Vehiculo = manifiesto.Capacidad_Vehiculo ?? 0;
                output.EO_RS_Llegada = new EO_RS(manifiestoEORS_Llegada.Id_EO_RS, manifiestoEORS_Llegada.Razon_Social, manifiestoEORS_Llegada.Registro, manifiestoEORS_Llegada.Fecha_Vencimiento_Registro, manifiestoEORS_Llegada.Estado);
                output.Registro_EO_RS_Llegada = manifiesto.Registro_EO_RS_Llegada;
                output.Fecha_Vencimiento_Registro_Llegada = manifiesto.Fecha_Vencimiento_Registro_Llegada.Value;
                output.Num_Autorizacion_Sanitaria_Llegada = manifiesto.Num_Autorizacion_Sanitaria_Llegada;
                output.Num_Autorizacion_Municipal_Llegada = manifiesto.Num_Autorizacion_Municipal_Llegada;
            }         
            output.Id_Registrado_Por = manifiesto.Id_Registrado_Por;
            output.Fecha_Registro = manifiesto.Fecha_Registro;
            output.Estado = manifiesto.Estado;

            return output;
        }
        public async Task<Manifiesto_RS> GetManifiesto_RS_Completo(int IdManifiesto)
        {
            var manifiesto = await _context.Manifiesto_RSs.FirstOrDefaultAsync(x => x.Id_Manifiesto_RS.Equals(IdManifiesto));

            var asesor = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(manifiesto.Id_Asesor_MAP));

            var output = new Manifiesto_RS();

            output.Id_Manifiesto_RS = manifiesto.Id_Manifiesto_RS;
            output.Item = manifiesto.Item;
            output.Serie_Manifiesto = manifiesto.Serie_Manifiesto;
            output.Num_Manifiesto = manifiesto.Num_Manifiesto;
            output.Fecha_Emision_Manifiesto = manifiesto.Fecha_Emision_Manifiesto;
            output.Observacion_Manifiesto = manifiesto.Observacion_Manifiesto;
            if (manifiesto.Observacion_Manifiesto.Equals("Conforme"))
            {
                var manifiestoResSolid = await _context.Residuos_Solidos.FirstOrDefaultAsync(x => x.Id_Residuo_Solido.Equals(manifiesto.Id_Residuo_Solido));
                var manifiestoTipoResSolid = await _context.Tipo_Residuos.FirstOrDefaultAsync(x => x.Id_Tipo_Residuo.Equals(manifiestoResSolid.Id_Tipo_Residuo));
                var manifiestoEORS_Salida = await _context.EO_RSs.FirstOrDefaultAsync(x => x.Id_EO_RS.Equals(manifiesto.Id_EO_RS_Salida));
                var manifiestoEORS_Llegada = await _context.EO_RSs.FirstOrDefaultAsync(x => x.Id_EO_RS.Equals(manifiesto.Id_EO_RS_Llegada));
                var conductor = await _context.Condutores_RS.FirstOrDefaultAsync(x => x.Id_Conductor_RS.Equals(manifiesto.Id_Conductor_RS));
                var vehiculo = await _context.Vehiculos_RS.FirstOrDefaultAsync(x => x.Id_Vehiculo_RS.Equals(manifiesto.Id_Vehiculo_RS));

                output.Residuo_Solido = new Residuo_Solido(
                manifiestoResSolid.Id_Residuo_Solido,
                new Tipo_Residuo(manifiestoTipoResSolid.Id_Tipo_Residuo, manifiestoTipoResSolid.Descripcion, manifiestoTipoResSolid.Estado),
                manifiestoResSolid.Descripcion,
                manifiestoResSolid.Estado);
                output.Cantidad_RS_Evacuado = manifiesto.Cantidad_RS_Evacuado.Value;
                output.EO_RS_Salida = new EO_RS(manifiestoEORS_Salida.Id_EO_RS, manifiestoEORS_Salida.Razon_Social, manifiestoEORS_Salida.Registro, manifiestoEORS_Salida.Fecha_Vencimiento_Registro, manifiestoEORS_Salida.Estado);
                output.Registro_EO_RS_Salida = manifiesto.Registro_EO_RS_Salida;
                output.Fecha_Vencimiento_Registro_Salida = manifiesto.Fecha_Vencimiento_Registro_Salida.Value;
                output.Num_Autorizacion_Municipal_Salida = manifiesto.Num_Autorizacion_Municipal_Salida;
                output.Num_Autorizacion_Hoja_Ruta = manifiesto.Num_Autorizacion_Hoja_Ruta;
                output.Conductor = conductor != null ? new Conductor_RS(conductor.Id_Conductor_RS, conductor.Nombres, conductor.Apellidos, conductor.NLicencia, conductor.Estado) : new Conductor_RS();
                output.Num_Licencia_Condutor = manifiesto.Num_Licencia_Condutor;
                output.Vehiculo = vehiculo != null ? new Vehiculo_RS(vehiculo.Id_Vehiculo_RS, vehiculo.Placa, vehiculo.Capacidad, vehiculo.Estado) : new Vehiculo_RS();
                output.Capacidad_Vehiculo = manifiesto.Capacidad_Vehiculo ?? 0;
                output.EO_RS_Llegada = new EO_RS(manifiestoEORS_Llegada.Id_EO_RS, manifiestoEORS_Llegada.Razon_Social, manifiestoEORS_Llegada.Registro, manifiestoEORS_Llegada.Fecha_Vencimiento_Registro, manifiestoEORS_Llegada.Estado);
                output.Registro_EO_RS_Llegada = manifiesto.Registro_EO_RS_Llegada;
                output.Fecha_Vencimiento_Registro_Llegada = manifiesto.Fecha_Vencimiento_Registro_Llegada.Value;
                output.Num_Autorizacion_Sanitaria_Llegada = manifiesto.Num_Autorizacion_Sanitaria_Llegada;
                output.Num_Autorizacion_Municipal_Llegada = manifiesto.Num_Autorizacion_Municipal_Llegada;

                output.Certificado_Trat_Disp = manifiesto.Certificado_Trat_Disp;
                output.Cantidad_RS_Dispuesto = manifiesto.Cantidad_RS_Dispuesto != null ? manifiesto.Cantidad_RS_Dispuesto.Value : 0;

                if (manifiesto.Fecha_Devolucion != null)
                    output.Fecha_Devolucion = manifiesto.Fecha_Devolucion.Value;
                else
                    output.Fecha_Devolucion = null;

                output.Informe_Transporte = manifiesto.Informe_Transporte;
            }
            output.Manifiesto = manifiesto.Manifiesto;

            if (asesor != null)
                output.Asesor_MAP = new Usuario(asesor.Id_Usuario, asesor.Id_Rol, asesor.Nombres, asesor.Apellidos, asesor.Email);
            else
                output.Asesor_MAP = null;
            
            output.Id_Registrado_Por = manifiesto.Id_Registrado_Por;
            output.Fecha_Registro = manifiesto.Fecha_Registro;
            output.Estado = manifiesto.Estado;

            return output;
        }
        public async Task<List<Manifiesto_RS>> GetManifiestosRSDescarga(DateTime fechaInicio, DateTime fechaFin)
        {
            var manifiestos = await _context.Manifiesto_RSs.ToListAsync();

            var data = manifiestos.Where(x => 
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString())&&
                !x.Estado.Equals(EstadosResiduosSolidos.Pendiente.ToString())&&
                x.Fecha_Emision_Manifiesto>=fechaInicio && 
                x.Fecha_Emision_Manifiesto<=fechaFin)
                .OrderBy(x=>Convert.ToInt32(x.Item))
                .ToList();

            var residuosSolidos = await _context.Residuos_Solidos.ToListAsync();
            var tiposResiduosSolidos = await _context.Tipo_Residuos.ToListAsync();
            var eors = await _context.EO_RSs.ToArrayAsync();
            var usuarios = await _context.Usuarios.ToArrayAsync();
            var conductores = await _context.Condutores_RS.ToListAsync();
            var vehiculos = await _context.Vehiculos_RS.ToListAsync();

            var output = new List<Manifiesto_RS>();

            foreach (var item in data)
            {
                var itemResSolid = item.Id_Residuo_Solido.HasValue? residuosSolidos.FirstOrDefault(x => x.Id_Residuo_Solido.Equals(item.Id_Residuo_Solido.Value)) : null;                
                var itemTipoResSolid = itemResSolid!=null? tiposResiduosSolidos.FirstOrDefault(x => x.Id_Tipo_Residuo.Equals(itemResSolid.Id_Tipo_Residuo)):null;
                var itemEORS_Salida = item.Id_EO_RS_Salida.HasValue? eors.FirstOrDefault(x => x.Id_EO_RS.Equals(item.Id_EO_RS_Salida)):null;
                var itemEORS_Llegada = item.Id_EO_RS_Llegada.HasValue? eors.FirstOrDefault(x => x.Id_EO_RS.Equals(item.Id_EO_RS_Llegada)):null;
                var itemConductor = item.Id_Conductor_RS.HasValue? conductores.FirstOrDefault(x => x.Id_Conductor_RS.Equals(item.Id_Conductor_RS)):null;
                var itemVehiculo = item.Id_Vehiculo_RS.HasValue? vehiculos.FirstOrDefault(x => x.Id_Vehiculo_RS.Equals(item.Id_Vehiculo_RS)):null;
                var itemAsesor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Asesor_MAP));

                var outputItem = new Manifiesto_RS();
                outputItem.Id_Manifiesto_RS = item.Id_Manifiesto_RS;
                outputItem.Item = item.Item;
                outputItem.Serie_Manifiesto = item.Serie_Manifiesto;
                outputItem.Num_Manifiesto = item.Num_Manifiesto;
                outputItem.Fecha_Emision_Manifiesto = item.Fecha_Emision_Manifiesto;
                outputItem.Observacion_Manifiesto = item.Observacion_Manifiesto;
                outputItem.Residuo_Solido = itemResSolid != null ? new Residuo_Solido(
                    itemResSolid.Id_Residuo_Solido,
                    new Tipo_Residuo(itemTipoResSolid.Id_Tipo_Residuo, itemTipoResSolid.Descripcion, itemTipoResSolid.Estado),
                    itemResSolid.Descripcion,
                    itemResSolid.Estado):new Residuo_Solido();
                outputItem.Cantidad_RS_Evacuado = item.Cantidad_RS_Evacuado.HasValue?item.Cantidad_RS_Evacuado.Value:0;
                outputItem.EO_RS_Salida = itemEORS_Salida != null ? new EO_RS(itemEORS_Salida.Id_EO_RS, itemEORS_Salida.Razon_Social, itemEORS_Salida.Registro, itemEORS_Salida.Fecha_Vencimiento_Registro, itemEORS_Salida.Estado):new EO_RS();
                outputItem.Registro_EO_RS_Salida = item.Registro_EO_RS_Salida;
                outputItem.Fecha_Vencimiento_Registro_Salida = item.Fecha_Vencimiento_Registro_Salida;
                outputItem.Num_Autorizacion_Municipal_Salida = item.Num_Autorizacion_Municipal_Salida;
                outputItem.Num_Autorizacion_Hoja_Ruta = item.Num_Autorizacion_Hoja_Ruta;
                outputItem.Conductor = itemConductor != null ? new Conductor_RS(itemConductor.Id_Conductor_RS, itemConductor.Nombres, itemConductor.Apellidos, itemConductor.NLicencia, itemConductor.Estado) : new Conductor_RS();
                outputItem.Num_Licencia_Condutor = item.Num_Licencia_Condutor;
                outputItem.Vehiculo = itemVehiculo != null ? new Vehiculo_RS(itemVehiculo.Id_Vehiculo_RS, itemVehiculo.Placa, itemVehiculo.Capacidad, itemVehiculo.Estado) : new Vehiculo_RS();
                outputItem.Capacidad_Vehiculo = item.Capacidad_Vehiculo ?? 0;
                outputItem.EO_RS_Llegada = itemEORS_Llegada!=null ? new EO_RS(itemEORS_Llegada.Id_EO_RS, itemEORS_Llegada.Razon_Social, itemEORS_Llegada.Registro, itemEORS_Llegada.Fecha_Vencimiento_Registro, itemEORS_Llegada.Estado):new EO_RS();
                outputItem.Registro_EO_RS_Llegada = item.Registro_EO_RS_Llegada;
                outputItem.Fecha_Vencimiento_Registro_Llegada = item.Fecha_Vencimiento_Registro_Llegada;
                outputItem.Num_Autorizacion_Sanitaria_Llegada = item.Num_Autorizacion_Sanitaria_Llegada;
                outputItem.Num_Autorizacion_Municipal_Llegada = item.Num_Autorizacion_Municipal_Llegada;                
                
                outputItem.Certificado_Trat_Disp = item.Certificado_Trat_Disp;
                outputItem.Cantidad_RS_Dispuesto = item.Cantidad_RS_Dispuesto;
                outputItem.Fecha_Devolucion = item.Fecha_Devolucion??item.Fecha_Devolucion;
                outputItem.Informe_Transporte = item.Informe_Transporte;
                outputItem.Manifiesto = item.Manifiesto;
                outputItem.Asesor_MAP = itemAsesor!=null?new Usuario(itemAsesor.Id_Usuario, itemAsesor.Id_Rol, itemAsesor.Nombres, itemAsesor.Apellidos, itemAsesor.Email):new Usuario();
                
                outputItem.Id_Registrado_Por = item.Id_Registrado_Por;
                outputItem.Fecha_Registro = item.Fecha_Registro;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }
        public async Task<string> GetLastItem()
        {
            var lista = await _context.Manifiesto_RSs.Where(x => x.Estado.Equals(EstadosResiduosSolidos.Registrado.ToString()) || x.Estado.Equals(EstadosResiduosSolidos.Cerrado.ToString())).ToListAsync();
            var data = lista.Select(x=>Convert.ToInt32(x.Item)).OrderBy(x=>x).LastOrDefault();            
            return data.ToString();
        }
    }
}
