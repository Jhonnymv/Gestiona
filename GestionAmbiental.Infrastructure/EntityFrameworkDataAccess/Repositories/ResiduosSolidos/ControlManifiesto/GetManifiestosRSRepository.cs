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
            var data = await _context.Manifiesto_RSs.Where(x =>
                x.Num_Manifiesto.Contains(nManifiesto!=""?nManifiesto:x.Num_Manifiesto) &&
                x.Fecha_Emision_Manifiesto.Equals(fechaEmision != null ? fechaEmision.Value : x.Fecha_Emision_Manifiesto) &&
                x.Observacion_Manifiesto.Equals(observacion!=""?observacion:x.Observacion_Manifiesto) &&
                x.Id_Residuo_Solido.Equals(residuoSolido!=0?residuoSolido:x.Id_Residuo_Solido)&&
                x.Id_EO_RS_Salida.Equals(eors_salida!=0?eors_salida:x.Id_EO_RS_Salida)&&
                x.Id_EO_RS_Llegada.Equals(eors_llegada!=0? eors_llegada : x.Id_EO_RS_Llegada)&&
                x.Id_Asesor_MAP.Equals(asesorMAP!=0?asesorMAP:x.Id_Asesor_MAP)&&
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()))
                .OrderBy(x=>x.Num_Manifiesto)
                .OrderBy(x=>x.Serie_Manifiesto)
                .ToListAsync();
            var residuosSolidos = await _context.Residuos_Solidos.ToListAsync();
            var tiposResiduosSolidos = await _context.Tipo_Residuos.ToListAsync();
            var eors = await _context.EO_RSs.ToArrayAsync();
            var usuarios = await _context.Usuarios.ToArrayAsync();

            var output = new List<Manifiesto_RS>();

            foreach (var item in data)
            {
                var itemResSolid = residuosSolidos.FirstOrDefault(x => x.Id_Residuo_Solido.Equals(item.Id_Residuo_Solido));
                var itemTipoResSolid = tiposResiduosSolidos.FirstOrDefault(x => x.Id_Tipo_Residuo.Equals(itemResSolid.Id_Tipo_Residuo));
                var itemEORS_Salida = eors.FirstOrDefault(x => x.Id_EO_RS.Equals(item.Id_EO_RS_Salida));
                var itemEORS_Llegada = eors.FirstOrDefault(x => x.Id_EO_RS.Equals(item.Id_EO_RS_Llegada));
                var asesor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Asesor_MAP));

                var outputItem = new Manifiesto_RS();
                outputItem.Id_Manifiesto_RS = item.Id_Manifiesto_RS;
                outputItem.Item = item.Item;
                outputItem.Serie_Manifiesto = item.Serie_Manifiesto;
                outputItem.Num_Manifiesto = item.Num_Manifiesto;
                outputItem.Fecha_Emision_Manifiesto = item.Fecha_Emision_Manifiesto;
                outputItem.Observacion_Manifiesto = item.Observacion_Manifiesto;
                outputItem.Residuo_Solido = new Residuo_Solido(
                    itemResSolid.Id_Residuo_Solido,
                    new Tipo_Residuo(itemTipoResSolid.Id_Tipo_Residuo, itemTipoResSolid.Descripcion, itemTipoResSolid.Estado),
                    itemResSolid.Descripcion,
                    itemResSolid.Estado);
                outputItem.Cantidad_RS_Evacuado = item.Cantidad_RS_Evacuado;
                outputItem.EO_RS_Salida = new EO_RS(itemEORS_Salida.Id_EO_RS, itemEORS_Salida.Razon_Social, itemEORS_Salida.Estado);
                outputItem.Registro_EO_RS_Salida = item.Registro_EO_RS_Salida;
                outputItem.Fecha_Vencimiento_Registro_Salida = item.Fecha_Vencimiento_Registro_Salida;
                outputItem.Num_Autorizacion_Municipal_Salida = item.Num_Autorizacion_Municipal_Salida;
                outputItem.Num_Autorizacion_Hoja_Ruta = item.Num_Autorizacion_Hoja_Ruta;
                outputItem.Nombre_Conductor = item.Nombre_Conductor;
                outputItem.Num_Licencia_Condutor = item.Num_Licencia_Condutor;
                outputItem.Placa_Vehiculo = item.Placa_Vehiculo;
                outputItem.Capacidad_Vehiculo = item.Capacidad_Vehiculo;
                outputItem.EO_RS_Llegada = new EO_RS(itemEORS_Llegada.Id_EO_RS, itemEORS_Llegada.Razon_Social, itemEORS_Llegada.Estado);
                outputItem.Registro_EO_RS_Llegada = item.Registro_EO_RS_Llegada;
                outputItem.Fecha_Vencimiento_Registro_Llegada = item.Fecha_Vencimiento_Registro_Llegada;
                outputItem.Num_Autorizacion_Sanitaria_Llegada = item.Num_Autorizacion_Sanitaria_Llegada;
                outputItem.Num_Autorizacion_Municipal_Llegada = item.Num_Autorizacion_Municipal_Llegada;
                outputItem.Certificado_Trat_Disp = item.Certificado_Trat_Disp;
                outputItem.Cantidad_RS_Dispuesto = item.Cantidad_RS_Dispuesto;
                outputItem.Fecha_Devolucion = item.Fecha_Devolucion;
                outputItem.Informe_Transporte = item.Informe_Transporte;
                outputItem.Manifiesto = item.Manifiesto;
                outputItem.Asesor_MAP = new Usuario(asesor.Id_Usuario, asesor.Id_Rol, asesor.Nombres, asesor.Apellidos, asesor.Email);
                outputItem.Id_Registrado_Por = item.Id_Registrado_Por;
                outputItem.Fecha_Registro = item.Fecha_Registro;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }
        public async Task<Manifiesto_RS> GetManifiestoRS(int IdManifiestoRS)
        {
            var manifiesto = await _context.Manifiesto_RSs.FirstOrDefaultAsync(x => x.Id_Manifiesto_RS.Equals(IdManifiestoRS));

            var manifiestoResSolid = await _context.Residuos_Solidos.FirstOrDefaultAsync(x => x.Id_Residuo_Solido.Equals(manifiesto.Id_Residuo_Solido));
            var manifiestoTipoResSolid = await _context.Tipo_Residuos.FirstOrDefaultAsync(x => x.Id_Tipo_Residuo.Equals(manifiestoResSolid.Id_Tipo_Residuo));
            var manifiestoEORS_Salida = await _context.EO_RSs.FirstOrDefaultAsync(x => x.Id_EO_RS.Equals(manifiesto.Id_EO_RS_Salida));
            var manifiestoEORS_Llegada = await _context.EO_RSs.FirstOrDefaultAsync(x => x.Id_EO_RS.Equals(manifiesto.Id_EO_RS_Llegada));
            var asesor = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(manifiesto.Id_Asesor_MAP));

            var output = new Manifiesto_RS();

            output.Id_Manifiesto_RS = manifiesto.Id_Manifiesto_RS;
            output.Item = manifiesto.Item;
            output.Serie_Manifiesto = manifiesto.Serie_Manifiesto;
            output.Num_Manifiesto = manifiesto.Num_Manifiesto;
            output.Fecha_Emision_Manifiesto = manifiesto.Fecha_Emision_Manifiesto;
            output.Observacion_Manifiesto = manifiesto.Observacion_Manifiesto;
            output.Residuo_Solido = new Residuo_Solido(
                manifiestoResSolid.Id_Residuo_Solido,
                new Tipo_Residuo(manifiestoTipoResSolid.Id_Tipo_Residuo, manifiestoTipoResSolid.Descripcion, manifiestoTipoResSolid.Estado),
                manifiestoResSolid.Descripcion,
                manifiestoResSolid.Estado);
            output.Cantidad_RS_Evacuado = manifiesto.Cantidad_RS_Evacuado;
            output.EO_RS_Salida = new EO_RS(manifiestoEORS_Salida.Id_EO_RS, manifiestoEORS_Salida.Razon_Social, manifiestoEORS_Salida.Estado);
            output.Registro_EO_RS_Salida = manifiesto.Registro_EO_RS_Salida;
            output.Fecha_Vencimiento_Registro_Salida = manifiesto.Fecha_Vencimiento_Registro_Salida;
            output.Num_Autorizacion_Municipal_Salida = manifiesto.Num_Autorizacion_Municipal_Salida;
            output.Num_Autorizacion_Hoja_Ruta = manifiesto.Num_Autorizacion_Hoja_Ruta;
            output.Nombre_Conductor = manifiesto.Nombre_Conductor;
            output.Num_Licencia_Condutor = manifiesto.Num_Licencia_Condutor;
            output.Placa_Vehiculo = manifiesto.Placa_Vehiculo;
            output.Capacidad_Vehiculo = manifiesto.Capacidad_Vehiculo;
            output.EO_RS_Llegada = new EO_RS(manifiestoEORS_Llegada.Id_EO_RS, manifiestoEORS_Llegada.Razon_Social, manifiestoEORS_Llegada.Estado);
            output.Registro_EO_RS_Llegada = manifiesto.Registro_EO_RS_Llegada;
            output.Fecha_Vencimiento_Registro_Llegada = manifiesto.Fecha_Vencimiento_Registro_Llegada;
            output.Num_Autorizacion_Sanitaria_Llegada = manifiesto.Num_Autorizacion_Sanitaria_Llegada;
            output.Num_Autorizacion_Municipal_Llegada = manifiesto.Num_Autorizacion_Municipal_Llegada;
            output.Certificado_Trat_Disp = manifiesto.Certificado_Trat_Disp;
            output.Cantidad_RS_Dispuesto = manifiesto.Cantidad_RS_Dispuesto;
            output.Fecha_Devolucion = manifiesto.Fecha_Devolucion;
            output.Informe_Transporte = manifiesto.Informe_Transporte;
            output.Manifiesto = manifiesto.Manifiesto;
            output.Asesor_MAP = new Usuario(asesor.Id_Usuario, asesor.Id_Rol, asesor.Nombres, asesor.Apellidos, asesor.Email);
            output.Id_Registrado_Por = manifiesto.Id_Registrado_Por;
            output.Fecha_Registro = manifiesto.Fecha_Registro;
            output.Estado = manifiesto.Estado;

            return output;
        }
    }
}
