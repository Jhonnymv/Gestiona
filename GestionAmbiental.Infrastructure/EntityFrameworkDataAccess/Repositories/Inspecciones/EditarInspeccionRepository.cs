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
            log.Tipo = cabeceraEntity.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) ? TipoAccion.DELETE.ToString() : TipoAccion.UPDATE.ToString();

            _context.Log_Inspecciones.Add(log);
            await _context.SaveChangesAsync();

        }

        public async Task EditarInspeccionDetalle(NuevaInspeccionDetalle detalle, int idUsuario)
        {
            var detalleEntity = await _context.Inspecciones_Detalle.FirstOrDefaultAsync(x => x.Id_Inspeccion_Detalle.Equals(detalle.IdInspeccionDetalle));

            detalleEntity.Tipo_Observacion = detalle.TipoInspeccion == detalleEntity.Tipo_Observacion ? detalleEntity.Tipo_Observacion : detalle.TipoInspeccion;
            detalleEntity.Id_Asesor = detalle.IdAsesorMASeguridad == detalleEntity.Id_Asesor ? detalleEntity.Id_Asesor : detalle.IdAsesorMASeguridad;
            detalleEntity.Descripcion_Condicion = detalle.DescripcionCondicion == detalleEntity.Descripcion_Condicion ? detalleEntity.Descripcion_Condicion : detalle.DescripcionCondicion;
            detalleEntity.Id_Tipo_Condicion = detalle.IdTipoCondicion == detalleEntity.Id_Tipo_Condicion ? detalleEntity.Id_Tipo_Condicion : detalle.IdTipoCondicion;
            detalleEntity.Evidencia_Condicion = detalle.EvidenciaCondicion == detalleEntity.Evidencia_Condicion ? detalleEntity.Evidencia_Condicion : detalle.EvidenciaCondicion;
            detalleEntity.Nivel_Riesgo = detalle.NivelRiesgo == detalleEntity.Nivel_Riesgo ? detalleEntity.Nivel_Riesgo : detalle.NivelRiesgo;
            detalleEntity.Descripcion_Accion = detalle.DescripcionAccion == detalleEntity.Descripcion_Accion ? detalleEntity.Descripcion_Accion : detalle.DescripcionAccion;
            detalleEntity.Id_Responsable_Marcobre = detalle.IdResponsableMarcobre == detalleEntity.Id_Responsable_Marcobre ? detalleEntity.Id_Responsable_Marcobre : detalle.IdResponsableMarcobre;
            detalleEntity.Responsable_Contratista = detalleEntity.Responsable_Contratista == detalle.ResponsableContratista ? detalleEntity.Responsable_Contratista : detalle.ResponsableContratista;
                        
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
        public async Task EliminarInspeccionDetalle(int IdObservacion,int idUsuario)
        {
            var observacion = await _context.Inspecciones_Detalle.FirstAsync(x => x.Id_Inspeccion_Detalle.Equals(IdObservacion));
            observacion.Estado = EstadosInspeccion.Eliminado.ToString();
            _context.Inspecciones_Detalle.Update(observacion);
            await _context.SaveChangesAsync();

            var log = new Log_Inspeccion();
            var bkp = JsonConvert.SerializeObject(observacion);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Inspeccion_Cabecera = observacion.Id_Inspeccion_Cabecera;
            log.Id_Usuario = idUsuario;
            log.Tipo = TipoAccion.DELETE.ToString();

            _context.Log_Inspecciones.Add(log);
            await _context.SaveChangesAsync();
        }
        public async Task<string> GetUltimoCodigo()
        {
            var codigo = await _context.Inspecciones_Cabecera.Where(x => x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).OrderBy(x=>x.Codigo).Select(x => x.Codigo).LastOrDefaultAsync();
            return codigo;
        }
    }
}
