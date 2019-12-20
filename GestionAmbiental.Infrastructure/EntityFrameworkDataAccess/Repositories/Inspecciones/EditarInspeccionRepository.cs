using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        public async Task EditarInspeccionCabecera(NuevaInspeccionCabecera cabecera, int idUsuario)
        {
            var cabeceraEntity = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x=>x.Id_Inspeccion_Cabecera.Equals(cabecera.IdCabecera));

            cabeceraEntity.Codigo = cabecera.Codigo;
            cabeceraEntity.Objetivo = cabecera.Objetivo;
            cabeceraEntity.Fecha_Inspeccion = cabecera.FechaInspeccion;
            cabeceraEntity.Id_Empresa = cabecera.IdEmpresa != 0 ? cabecera.IdEmpresa: 1;
            cabeceraEntity.Id_Gerencia = cabecera.IdGerencia != 0 ? cabecera.IdGerencia : 1;
            cabeceraEntity.Area_Inspeccionada = cabecera.LugarInspeccionado;
            cabeceraEntity.Id_Responsable_Area = cabecera.IdResponsableArea;
            cabeceraEntity.Participantes = cabecera.Inspectores;
            cabeceraEntity.Estado = cabecera.Estado;

            _context.Inspecciones_Cabecera.Update(cabeceraEntity);
            await _context.SaveChangesAsync();

            var log = new Log_Inspeccion();            
            var bkp = JsonConvert.SerializeObject(cabeceraEntity);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Inspeccion_Cabecera = cabecera.IdCabecera;
            log.Id_Usuario = idUsuario;
            log.Tipo = TipoAccion.UPDATE.ToString();

            _context.Log_Inspecciones.Add(log);
            await _context.SaveChangesAsync();

        }

        public async Task EditarInspeccionDetalle(NuevaInspeccionDetalle detalle, int idUsuario)
        {
            var detalleEntity = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(detalle.IdInspeccionDetalle));

            detalleEntity.Fecha_Programada = detalle.FechaProgramada;
            detalleEntity.Estado = detalle.Estado;

            _context.Inspecciones_Detalle.Update(detalleEntity);
            await _context.SaveChangesAsync();

           var log = new Log_Inspeccion();
           var bkp = JsonConvert.SerializeObject(detalleEntity);
           log.Bkp = bkp;
           log.Fecha = DateTime.Now;
           log.Id_Inspeccion_Cabecera = detalleEntity.Id_Inspeccion_Cabecera;
           log.Id_Usuario = idUsuario;
           log.Tipo = TipoAccion.UPDATE.ToString();

           _context.Log_Inspecciones.Add(log);
           await _context.SaveChangesAsync();
        }
    }
}
