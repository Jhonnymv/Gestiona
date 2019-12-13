using GestionAmbiental.Application.Repositories.Notificaciones;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Notificaciones
{
    public class GetInspeccionInfoRepository : IGetInspeccionInfoReadOnlyRepository
    {
        private Context _context;

        public GetInspeccionInfoRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<InspeccionCabecera> GetInfoCompletaInspeccion(int idCabecera)
        {
            var cabeccera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            var gerEjectMar = gerencias.Where(g => g.Id_Gerencia.Equals(cabeccera.Id_Gerencia)).FirstOrDefault().Descripcion;
            var empContMar = empresas.Where(e => e.Id_Empresa.Equals(cabeccera.Id_Empresa)).FirstOrDefault().Razon_Social;
           
            var responsableLugar = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(cabeccera.Id_Responsable_Area));
            var responsableLugarDomain = new Usuario(responsableLugar.Id_Usuario, responsableLugar.Id_Rol, responsableLugar.Nombres, responsableLugar.Apellidos, responsableLugar.Email);
            
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)).ToListAsync();
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();

            var output = new InspeccionCabecera();
            output.IdCabecera = cabeccera.Id_Inspeccion_Cabecera;
            output.Codigo = cabeccera.Codigo;
            output.Objetivo = cabeccera.Objetivo;
            output.Gerencia = gerEjectMar;
            output.Empresa = empContMar;
            output.AreaInspeccionada = cabeccera.Area_Inspeccionada;
            output.TipoInspeccion = cabeccera.Tipo_Inspeccion;
            output.ResponsableArea = responsableLugarDomain;
            output.ResponsableInspeccion = cabeccera.Responsable_Inspeccion;
            output.Participantes = cabeccera.Participantes;
            output.FechaInspeccion = cabeccera.Fecha_Inspeccion;
            output.Estado = cabeccera.Estado;

            output.Observaciones = new List<InspeccionDetalle>();

            foreach (var observacion in observaciones)
            {
                var tipoCondicion = tiposCondicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(observacion.Id_Tipo_Condicion));
                var tipoCondicion_Domain = new TipoCondicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion,tipoCondicion.Grupo);
                var responsableMarcobre = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(observacion.Id_Responsable_Marcobre));
                var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario, responsableMarcobre.Id_Rol, responsableMarcobre.Nombres, responsableMarcobre.Apellidos, responsableMarcobre.Email);
                var responsableMA = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(observacion.Id_Asesor));
                var responsableMA_Domain = new Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres, responsableMA.Apellidos, responsableMA.Email);

                var detalle = new InspeccionDetalle();
                detalle.IdInspeccionCabecera = observacion.Id_Inspeccion_Cabecera;
                detalle.IdInspeccionDetalle = observacion.Id_Inspeccion_Detalle;
                detalle.TipoObservacion = observacion.Tipo_Observacion;
                detalle.DescripcionCondicion = observacion.Descripcion_Condicion;
                detalle.EvidenciaCondicion = observacion.Evidencia_Condicion;
                detalle.NivelRiesgo = observacion.Nivel_Riesgo;
                detalle.TipoCondicion = tipoCondicion_Domain;
                detalle.DescripcionAccion = observacion.Descripcion_Accion;
                detalle.ResponsableMarcobre = responsableMarcobre_Domain;
                detalle.Asesor = responsableMA_Domain;
                detalle.ResponsableContratista = observacion.Responsable_Contratista;
                detalle.FechaProgramada = observacion.Fecha_Programada;

                output.Observaciones.Add(detalle);
            }
            return output;
        }
    }
}
