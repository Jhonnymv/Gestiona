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
            var empresas = await _context.Empresas_Contratistas.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            var gerEjectMar = gerencias.Where(g => g.Id_Gerencia.Equals(cabeccera.Id_Gerencia_Ejecutora)).FirstOrDefault().Descripcion;
            var gerEjectEPCM = gerencias.Where(g => g.Id_Gerencia.Equals(cabeccera.Id_Gerencia_EPCM)).FirstOrDefault().Descripcion;
            var empContMar = empresas.Where(e => e.Id_Empresa_Contratista.Equals(cabeccera.Id_Empresa_Contratista)).FirstOrDefault().Razon_Social;
            var empContEPCM = empresas.Where(e => e.Id_Empresa_Contratista.Equals(cabeccera.Id_Empresa_Contratista_EPCM)).FirstOrDefault().Razon_Social;
           
            var responsableLugar = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(cabeccera.Id_Responsable_Lugar));
            var responsableLugarDomain = new Usuario(responsableLugar.Id_Usuario, responsableLugar.Id_Rol, responsableLugar.Nombres, responsableLugar.Apellidos, responsableLugar.Email);
            var responsableMA = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(cabeccera.Id_Responsable_MA));
            var responsableMADomain = new Usuario(responsableMA.Id_Usuario, responsableMA.Id_Rol, responsableMA.Nombres, responsableMA.Apellidos, responsableMA.Email);

            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)).ToListAsync();
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();

            var output = new InspeccionCabecera();
            output.IdCabecera = cabeccera.Id_Inspeccion_Cabecera;
            output.Codigo = cabeccera.Codigo;
            output.Objetivo = cabeccera.Objetivo;
            output.GerenEjecMar = gerEjectMar;
            output.EmpContratMarc = empContMar;
            output.GerenEjecEPCM = gerEjectEPCM;
            output.EmpContratEPCM = empContEPCM;
            output.LugarInspec = cabeccera.Lugar_Inspeccionado;
            output.Responsable_Lugar = responsableLugarDomain;
            output.ResponsableMA = responsableMADomain;
            output.FechaInspec = cabeccera.Fecha_Inspeccion;
            output.Inspectores = cabeccera.Inspectores;
            output.Estado = cabeccera.Estado;
            output.Observaciones = new List<InspeccionDetalle>();

            foreach (var observacion in observaciones)
            {
                var tipoCondicion = tiposCondicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(observacion.Id_Tipo_Condicion));
                var tipoCondicion_Domain = new Tipo_Condicion(tipoCondicion.Id_Tipo_Condicion, tipoCondicion.Descripcion);
                var responsableMarcobre = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(observacion.Id_Responsable_Marcobre));
                var responsableMarcobre_Domain = new Usuario(responsableMarcobre.Id_Usuario, responsableMarcobre.Id_Rol, responsableMarcobre.Nombres, responsableMarcobre.Apellidos, responsableMarcobre.Email);

                var detalle = new InspeccionDetalle(
                observacion.Id_Inspeccion_Cabecera,
                observacion.Id_Inspeccion_Detalle,
                observacion.Descripcion_Condicion,
                tipoCondicion_Domain,
                observacion.Evidencia_Condicion,
                observacion.Nivel_Riesgo_Ambiental,
                observacion.Descripcion_Accion,
                responsableMarcobre_Domain,
                observacion.Responsable_Contratista,
                observacion.Fecha_Programada);
                
                output.Observaciones.Add(detalle);
            }
            return output;
        }
    }
}
