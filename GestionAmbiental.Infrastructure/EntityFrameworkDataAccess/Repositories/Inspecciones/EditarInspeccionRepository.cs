using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var cabeceraEntity = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x=>x.Id_Inspeccion_Cabecera.Equals(cabecera.Id_Cabecera));

            cabeceraEntity.Objetivo = cabecera.Objetivo;
            cabeceraEntity.Fecha_Inspeccion = cabecera.Fecha_Inspeccion;
            cabeceraEntity.Id_Empresa_Contratista = cabecera.Id_Empresa_Contratista != 0 ? cabecera.Id_Empresa_Contratista: 1;
            cabeceraEntity.Id_Empresa_Contratista_EPCM = cabecera.Id_Empresa_Contratista_EPCM != 0 ? cabecera.Id_Empresa_Contratista_EPCM : 1;
            cabeceraEntity.Id_Gerencia_Ejecutora = cabecera.Id_Gerencia_Ejecutora != 0 ? cabecera.Id_Gerencia_Ejecutora : 1;
            cabeceraEntity.Id_Gerencia_EPCM = cabecera.Id_Gerencia_EPCM != 0 ? cabecera.Id_Gerencia_EPCM : 1;
            cabeceraEntity.Lugar_Inspeccionado = cabecera.Lugar_Inspeccionado;
            cabeceraEntity.Id_Responsable_MA = cabecera.Id_Responsable_MA;
            cabeceraEntity.Id_Responsable_Lugar = cabecera.Id_Responsable_Lugar;
            cabeceraEntity.Inspectores = cabecera.Inspectores;
            cabeceraEntity.Estado = "Registrado";

            _context.Inspecciones_Cabecera.Update(cabeceraEntity);
            await _context.SaveChangesAsync();

        }

        public Task EditarInspeccionDetalle(NuevaInspeccionDetalle detalle)
        {
            throw new NotImplementedException();
        }
    }
}
