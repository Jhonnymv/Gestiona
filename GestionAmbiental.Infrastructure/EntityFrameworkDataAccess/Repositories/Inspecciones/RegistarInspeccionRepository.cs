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

            var cabeceraEntity = new Inspeccion_Cabecera
            {
                Area = "Medio Ambiente",
                Codigo = "DEFINIR",
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

            _context.Inspecciones_Cabecera.Add(cabeceraEntity);

            await _context.SaveChangesAsync();
            

            return cabeceraEntity.Id_Inspeccion_Cabecera;
        }

        public async Task<int> RegistrarInspeccionDetalle(NuevaInspeccionDetalle detalle)
        {
           var detalleEntity = new Inspeccion_Detalle
                {
                    Id_Inspeccion_Cabecera = detalle.Id_Inspeccion_Cabecera,
                    Descripcion_Condicion = detalle.Descripcion_Condicion,
                    Id_Tipo_Condicion = detalle.Id_Tipo_Condicion,
                    Nivel_Riesgo_Ambiental = detalle.Nivel_Riesgo_Ambiental,
                    Descripcion_Accion = detalle.Descripcion_Accion,
                    Id_Responsable_Marcobre = detalle.Id_Responsable_Marcobre,
                    Responsable_Contratista = detalle.Responsable_Contratista,
                    Fecha_Programada = detalle.Fecha_Programada,
                    Evidencia_Condicion = detalle.Evidencia_Condicion,
                    Estado = "Registrado",

                    Fecha_Cierre = null,
                    Fecha_Verificacion_Inspector = null,
                    Fecha_Verificacion_MA = null,
                };
                _context.Inspecciones_Detalle.Add(detalleEntity);
                await _context.SaveChangesAsync();
            return detalleEntity.Id_Inspeccion_Detalle;
        }
    }
}
