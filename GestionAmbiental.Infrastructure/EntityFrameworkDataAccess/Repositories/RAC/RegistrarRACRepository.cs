using GestionAmbiental.Application.Repositories.RAC;
using GestionAmbiental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.RAC
{
    public class RegistrarRACRepository : IRegistrarRACReadOnlyRepository
    {
        private readonly Context _context;
        public RegistrarRACRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> RegistrarRAC(Domain.RAC.RAC nuevoRegistro)
        {
            var racEntity = new Entities.RAC
            {
                Id_Registrado_Por = nuevoRegistro.Id_Registrado_Por,
                Codigo = nuevoRegistro.Codigo,
                Nombres_Apellidos = nuevoRegistro.Nombres_Apellidos,
                Id_Gerencia = nuevoRegistro.Id_Gerencia,
                Id_Empresa = nuevoRegistro.Id_Empresa,
                Otra_Empresa = nuevoRegistro.Otra_Empresa ?? null,
                Id_Empresa_Obs = nuevoRegistro.Id_Empresa_Obs,
                Id_Gerencia_Obs = nuevoRegistro.Id_Gerencia_Obs,
                Ubicacion = nuevoRegistro.Ubicacion,
                Fecha = nuevoRegistro.Fecha,
                SSO_MA = nuevoRegistro.SSO_MA,
                Clasificacion = nuevoRegistro.Clasificacion,
                NivelRiesgo = nuevoRegistro.NivelRiesgo,
                Id_Tipo_Condicion = nuevoRegistro.Id_Tipo_Condicion,
                Descripcion_Acto_Cond = nuevoRegistro.Descripcion_Acto_Cond,
                Medida_Control = nuevoRegistro.Medida_Control,
                PAC = nuevoRegistro.PAC,
                Id_Responsable_Cierre = nuevoRegistro.Id_Responsable_Cierre,
                Fecha_Cierre = nuevoRegistro.Fecha_Cierre,
                Id_Supervisor = nuevoRegistro.Id_Supervisor,
                Evidencia_Obs = nuevoRegistro.Evidencia_Obs,
                Estado = nuevoRegistro.Estado
            };

            _context.RACs.Add(racEntity);
            await _context.SaveChangesAsync();
            return racEntity.Id_RAC;
        }
        public async Task<string> GetUltimoCodigo()
        {
            var codigo = await _context.RACs.OrderBy(x => x.Codigo).Select(x => x.Codigo).LastOrDefaultAsync();
            return codigo;
        }
    }
}
