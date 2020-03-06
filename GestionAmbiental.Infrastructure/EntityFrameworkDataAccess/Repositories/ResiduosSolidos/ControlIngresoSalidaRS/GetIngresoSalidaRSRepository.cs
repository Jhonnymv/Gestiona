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
    public class GetIngresoSalidaRSRepository : IGetIngresoSalidaRSReadOnlyRepository
    {
        private readonly Context _context;

        public GetIngresoSalidaRSRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Ingreso_Salida_RS>> GetIngreso_Salida_RSs(int acr_cmrs, int ingreso_salida, int tipo, DateTime? fecha, int empresa, int asesorMA)
        {
            var ids_residuos_solidos_tipo = await _context.Residuos_Solidos.Where(x =>
                x.Estado.Equals("ACT") &&
                x.Id_Tipo_Residuo.Equals(tipo != 0 ? tipo : x.Id_Tipo_Residuo))
                .Select(x => x.Id_Residuo_Solido)
                .ToListAsync();
            var ingresos_salidas_rs = await _context.Ingresos_Salidas_RS.ToListAsync();
            var data = ingresos_salidas_rs.Where(x =>
                x.Id_ACR_CMRS.Equals(acr_cmrs != 0 ? acr_cmrs : x.Id_ACR_CMRS) &&
                x.Id_Ingreso_Salida.Equals(ingreso_salida != 0 ? ingreso_salida : x.Id_Ingreso_Salida) &&
                ids_residuos_solidos_tipo.Contains(x.Id_Residuo_Solido) &&
                x.Fecha_Ingreso_Salida.Equals(fecha != null ? fecha.Value : x.Fecha_Ingreso_Salida) &&
                x.Id_Empresa_RS.Equals(empresa != 0 ? empresa : x.Id_Empresa_RS) &&
                x.Id_Responsable_Marcobre.Equals(asesorMA != 0 ? asesorMA : x.Id_Responsable_Marcobre) &&
                !x.Estado.Equals(EstadosResiduosSolidos.Eliminado.ToString()))
                .OrderByDescending(x => x.Item)
                .ToList();

            var acr_cmrs_entities = await _context.ACR_CMRs.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var ing_sal_entities = await _context.Ingresos_Salidas.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var motivos_entities = await _context.Motivos_Ingreso_Salida.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var residuo_solido_entities = await _context.Residuos_Solidos.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var tipo_residuo_entities = await _context.Tipo_Residuos.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var empresas_entities = await _context.Empresas.ToListAsync();
            var usuarios_entities = await _context.Usuarios.Where(x=>x.Estado.Equals("ACT")).ToListAsync();

            var output = new List<Ingreso_Salida_RS>();
            foreach (var item in data)
            {
                var acr_cmrs_model = acr_cmrs_entities.Find(x => x.Id_ACR_CMRS.Equals(item.Id_ACR_CMRS));
                var ing_sal_model = ing_sal_entities.Find(x => x.Id_Ingreso_Salida.Equals(item.Id_Ingreso_Salida));
                var motivo_model = motivos_entities.Find(x => x.Id_Motivo.Equals(item.Id_Motivo));
                var residuo_solido_model = residuo_solido_entities.Find(x => x.Id_Residuo_Solido.Equals(item.Id_Residuo_Solido));
                var tipo_residuo_model = tipo_residuo_entities.Find(x => x.Id_Tipo_Residuo.Equals(residuo_solido_model.Id_Tipo_Residuo));
                var empresa_model = empresas_entities.Find(x => x.Id_Empresa.Equals(item.Id_Empresa_RS));
                var asesorMA_model = usuarios_entities.Find(x => x.Id_Usuario.Equals(item.Id_Responsable_Marcobre));

                var outputItem = new Ingreso_Salida_RS();
                outputItem.Item = item.Item;
                outputItem.Id_Ingreso_Salida_RS = item.Id_Ingreso_Salida_RS;
                outputItem.ACR_CMRS = new Domain.ValueObjects.ACR_CMRS(acr_cmrs_model.Id_ACR_CMRS, acr_cmrs_model.Descripcion, acr_cmrs_model.Estado);
                outputItem.Responsable_ACR_CMRS = item.Responsable_ACR_CMRS;
                outputItem.Ingreso_Salida = new Domain.ValueObjects.Ingreso_Salida(ing_sal_model.Id_Ingreso_Salida, ing_sal_model.Descripcion, ing_sal_model.Estado);
                outputItem.Motivo = new Domain.ValueObjects.Motivo_Ingreso_Salida(motivo_model.Id_Motivo, motivo_model.Descripcion, motivo_model.Estado);
                outputItem.Fecha_Ingreso_Salida = item.Fecha_Ingreso_Salida;
                outputItem.Residuo_Solido = new Domain.ValueObjects.Residuo_Solido(
                    residuo_solido_model.Id_Tipo_Residuo,
                    new Domain.ValueObjects.Tipo_Residuo(tipo_residuo_model.Id_Tipo_Residuo, tipo_residuo_model.Descripcion, tipo_residuo_model.Estado),
                    residuo_solido_model.Descripcion,
                    residuo_solido_model.Estado);
                outputItem.Cantidad = item.Cantidad;
                outputItem.Empresa_RS = new Domain.ValueObjects.Empresa(empresa_model.Id_Empresa, empresa_model.Razon_Social);
                outputItem.Responsable_Marcobre = new Domain.ValueObjects.Usuario(asesorMA_model.Id_Usuario, asesorMA_model.Id_Rol, asesorMA_model.Nombres, asesorMA_model.Apellidos, asesorMA_model.Email);
                outputItem.Comentario = item.Comentario;
                outputItem.Id_Registrado_Por = item.Id_Registrado_Por;
                outputItem.Fecha_Registro = item.Fecha_Registro;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }
            return output;
        }
        public async Task<Ingreso_Salida_RS> GetIngreso_Salida_RS(int id_Ingreso_Salida_RS)
        {
            var output = new Ingreso_Salida_RS();
            var data = await _context.Ingresos_Salidas_RS.FirstOrDefaultAsync(x => x.Id_Ingreso_Salida_RS.Equals(id_Ingreso_Salida_RS));
            if (data != null)
            {
                var acr_cmrs_entity = await _context.ACR_CMRs.FirstOrDefaultAsync(x => x.Id_ACR_CMRS.Equals(data.Id_ACR_CMRS));
                var ing_sal_entity = await _context.Ingresos_Salidas.FirstOrDefaultAsync(x => x.Id_Ingreso_Salida.Equals(data.Id_Ingreso_Salida));
                var motivo_entity = await _context.Motivos_Ingreso_Salida.FirstOrDefaultAsync(x => x.Id_Motivo.Equals(data.Id_Motivo));
                var residuo_solido_entity = await _context.Residuos_Solidos.FirstOrDefaultAsync(x => x.Id_Residuo_Solido.Equals(data.Id_Residuo_Solido));
                var tipo_residuo_entity = await _context.Tipo_Residuos.FirstOrDefaultAsync(x => x.Id_Tipo_Residuo.Equals(residuo_solido_entity.Id_Tipo_Residuo));
                var empresa_entity = await _context.Empresas.FirstOrDefaultAsync(x => x.Id_Empresa.Equals(data.Id_Empresa_RS));
                var usuarios = await _context.Usuarios.Where(x => x.Estado.Equals("ACT")).ToListAsync();
                var responsable_entity = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(data.Id_Responsable_Marcobre));

                output.Item = data.Item;
                output.Id_Ingreso_Salida_RS = data.Id_Ingreso_Salida_RS;
                output.ACR_CMRS = new Domain.ValueObjects.ACR_CMRS(acr_cmrs_entity.Id_ACR_CMRS, acr_cmrs_entity.Descripcion, acr_cmrs_entity.Estado);
                output.Responsable_ACR_CMRS = data.Responsable_ACR_CMRS;
                output.Ingreso_Salida = new Domain.ValueObjects.Ingreso_Salida(ing_sal_entity.Id_Ingreso_Salida, ing_sal_entity.Descripcion, ing_sal_entity.Estado);
                output.Motivo = new Domain.ValueObjects.Motivo_Ingreso_Salida(motivo_entity.Id_Motivo, motivo_entity.Descripcion, motivo_entity.Estado);
                output.Fecha_Ingreso_Salida = data.Fecha_Ingreso_Salida;
                output.Residuo_Solido = new Domain.ValueObjects.Residuo_Solido(
                    residuo_solido_entity.Id_Residuo_Solido,
                    new Domain.ValueObjects.Tipo_Residuo(
                        tipo_residuo_entity.Id_Tipo_Residuo,
                        tipo_residuo_entity.Descripcion,
                        tipo_residuo_entity.Estado),
                    residuo_solido_entity.Descripcion,
                    residuo_solido_entity.Estado);
                output.Cantidad = data.Cantidad;
                output.Empresa_RS = new Domain.ValueObjects.Empresa(empresa_entity.Id_Empresa, empresa_entity.Razon_Social);
                output.Responsable_Marcobre = new Domain.ValueObjects.Usuario(responsable_entity.Id_Usuario, responsable_entity.Id_Rol, responsable_entity.Nombres, responsable_entity.Apellidos, responsable_entity.Email);
                output.Comentario = data.Comentario;
                output.Id_Registrado_Por = data.Id_Registrado_Por;
                output.Fecha_Registro = data.Fecha_Registro;
                output.Estado = data.Estado;
            }
            return output;
        }
        public async Task<string> GetLastItem()
        {
            var data = await _context.Ingresos_Salidas_RS.Where(x => x.Estado.Equals(EstadosResiduosSolidos.Registrado.ToString())).OrderBy(x => x.Item).LastOrDefaultAsync();
            if (data != null)
                return data.Item;
            else
                return "";
        }
    }
}
