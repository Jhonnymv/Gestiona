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
    public class RegistarInspeccionRepository : IRegistarInspeccionReadOnlyRepository
    {
        private readonly Context _context;

        public RegistarInspeccionRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<int> RegistrarInspeccionCabecera(NuevaInspeccionCabecera cabecera)
        {

            var cabeceraEntity = new Inspeccion_Cabecera
            {
                Codigo = cabecera.Codigo,
                Id_Registrado_Por = cabecera.IdRegistradoPor,
                Objetivo = cabecera.Objetivo,
                Fecha_Inspeccion = cabecera.FechaInspeccion,
                Id_Empresa = cabecera.IdEmpresa != 0 ? cabecera.IdEmpresa : 1,
                Id_Gerencia = cabecera.IdGerencia != 0 ? cabecera.IdGerencia : 1,
                Responsable_Inspeccion = cabecera.ResponsableInspeccion,
                Tipo_Inspeccion = cabecera.TipoInspeccion,
                Area_Inspeccionada = cabecera.LugarInspeccionado,
                Id_Responsable_Area = cabecera.IdResponsableArea,
                Participantes = cabecera.Inspectores,
                Estado = cabecera.Estado
            };

            _context.Inspecciones_Cabecera.Add(cabeceraEntity);

            await _context.SaveChangesAsync();

            var log = new Log_Inspeccion();
            var bkp = JsonConvert.SerializeObject(cabeceraEntity);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Inspeccion_Cabecera = cabeceraEntity.Id_Inspeccion_Cabecera;
            log.Id_Usuario = cabeceraEntity.Id_Registrado_Por;
            log.Tipo = TipoAccion.CREATE.ToString();

            _context.Log_Inspecciones.Add(log);
            await _context.SaveChangesAsync();


            return cabeceraEntity.Id_Inspeccion_Cabecera;
        }

        public async Task<int> RegistrarInspeccionDetalle(NuevaInspeccionDetalle detalle,int Id_Usuario)
        {
           var detalleEntity = new Inspeccion_Detalle
                {
                    Id_Inspeccion_Cabecera = detalle.IdInspeccionCabecera,
                    Tipo_Observacion = detalle.TipoInspeccion,
                    Id_Asesor = detalle.IdAsesorMASeguridad,
                    Descripcion_Condicion = detalle.DescripcionCondicion,
                    Id_Tipo_Condicion = detalle.IdTipoCondicion,
                    Nivel_Riesgo = detalle.NivelRiesgo,
                    Descripcion_Accion = detalle.DescripcionAccion,
                    Id_Responsable_Marcobre = detalle.IdResponsableMarcobre,
                    Responsable_Contratista = detalle.ResponsableContratista,
                    Fecha_Programada = detalle.FechaProgramada,
                    Evidencia_Condicion = detalle.EvidenciaCondicion,
                    Estado = "Registrado",

                    Fecha_Cierre = null,
                    Fecha_Verificacion = null,
                };
                _context.Inspecciones_Detalle.Add(detalleEntity);
                await _context.SaveChangesAsync();

            var log = new Log_Inspeccion();
            var bkp = JsonConvert.SerializeObject(detalleEntity);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Inspeccion_Cabecera = detalleEntity.Id_Inspeccion_Cabecera;
            log.Id_Usuario = Id_Usuario;
            log.Tipo = TipoAccion.CREATE.ToString();

            _context.Log_Inspecciones.Add(log);
            await _context.SaveChangesAsync();

            return detalleEntity.Id_Inspeccion_Detalle;
        }        
    }
}
