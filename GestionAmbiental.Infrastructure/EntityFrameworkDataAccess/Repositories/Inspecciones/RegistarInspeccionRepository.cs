using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Inspecciones
{
    public class RegistarInspeccionRepository : IRegistarInspeccionReadOnlyRepository
    {
        private readonly Context _context;

        public RegistarInspeccionRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<int> RegistrarInspeccionCabecera(NuevaInspeccionCabecera cabecera)
        {
            
            var cabeceraEntity = new Inspeccion_Cabecera();

            cabeceraEntity.Area = "Medio Ambiente";
            cabeceraEntity.Codigo = "DEFINIR";
            cabeceraEntity.Objetivo = cabecera.Objetivo;
            cabeceraEntity.Fecha_Inspeccion = cabecera.Fecha_Inspeccion;
            cabeceraEntity.Id_Empresa_Contratista = cabecera.Id_Empresa_Contratista;
            cabeceraEntity.Id_Empresa_Contratista_EPCM = cabecera.Id_Empresa_Contratista_EPCM;
            cabeceraEntity.Id_Gerencia_Ejecutora = cabecera.Id_Gerencia_Ejecutora;
            cabeceraEntity.Id_Gerencia_EPCM = cabecera.Id_Gerencia_EPCM;
            cabeceraEntity.Lugar_Inspeccionado = cabecera.Lugar_Inspeccionado;
            cabeceraEntity.Id_Responsable_MA = cabecera.Id_Responsable_MA;
            cabeceraEntity.Id_Responsable_Lugar = cabecera.Id_Responsable_Lugar;
            cabeceraEntity.Inspectores = cabecera.Inspectores;
            cabeceraEntity.Estado = "Registrado";

            _context.Inspecciones_Cabecera.Add(cabeceraEntity);

            await _context.SaveChangesAsync();
            

            return cabeceraEntity.Id_Inspeccion_Cabecera;
        }

        public async Task RegistrarInspeccionDetalle(NuevaInspeccionDetalle detalle)
        {
            throw new NotImplementedException();
        }
    }
}
