using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Inspecciones
{
    public class EditarInspeccionRepository : IEditarInspeccionReadOnlyRepository
    {
        private readonly Context _context;
        public EditarInspeccionRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task EditarInspeccionCabecera(NuevaInspeccionCabecera cabecera)
        {
            var cabeceraEntity = new Inspeccion_Cabecera
            {
                Area = cabecera.Area,
                Codigo = cabecera.Codigo,
                Objetivo = cabecera.Objetivo,
                Fecha_Inspeccion = cabecera.Fecha_Inspeccion,
                Id_Empresa_Contratista = cabecera.Id_Empresa_Contratista,
                Id_Empresa_Contratista_EPCM = cabecera.Id_Empresa_Contratista_EPCM,
                Id_Gerencia_Ejecutora = cabecera.Id_Gerencia_Ejecutora,
                Id_Gerencia_EPCM = cabecera.Id_Gerencia_EPCM,
                Lugar_Inspeccionado = cabecera.Lugar_Inspeccionado,
                Id_Responsable_MA = cabecera.Id_Responsable_MA,
                Id_Responsable_Lugar = cabecera.Id_Responsable_Lugar,
                Inspectores = cabecera.Inspectores,
                Estado = "Registrado"
            };
            _context.Inspecciones_Cabecera.Update(cabeceraEntity);

            await _context.SaveChangesAsync();

        }

        public Task EditarInspeccionDetalle(NuevaInspeccionDetalle detalle)
        {
            throw new NotImplementedException();
        }
    }
}
