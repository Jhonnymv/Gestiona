using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.DashboardGeneral
{
    public class GetRegistrosRepository: IGetRegistrosReadOnlyRepository
    {
        private readonly Context _context;

        public GetRegistrosRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<List<Domain.Inspecciones.InspeccionDetalle>> Observaciones(DateTime startTime, DateTime endTime)
        {
            var output = new List<Domain.Inspecciones.InspeccionDetalle>();

            //Lista inspecciones
            var inspecciones = await _context.Inspecciones_Cabecera.Where(x=>
            !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
            x.Fecha_Inspeccion >= startTime &&
            x.Fecha_Inspeccion <= endTime)
                .ToListAsync();

            //Lista observaciones
            var observaciones = await _context.Inspecciones_Detalle.Where(x =>
            !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()) &&
            inspecciones.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)
            ).ToListAsync();

            //Todos los tipos de condicion
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();
            //Todos los usuarios
            var usuarios = await _context.Usuarios.ToListAsync();
            //Todos las gerencias
            var gerencias = await _context.Gerencias.ToListAsync();
            //Todos las empresas
            var empresas = await _context.Empresas.ToListAsync();

            foreach(var observacion in observaciones)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(observacion.Id_Tipo_Condicion));
                var responsable1 = usuarios.Find(x => x.Id_Usuario.Equals(observacion.Id_Responsable_Marcobre));
                var responsable2 = usuarios.Find(x => x.Id_Usuario.Equals(observacion.Id_Responsable_Marcobre2));
                var asesor = usuarios.Find(x => x.Id_Usuario.Equals(observacion.Id_Asesor));
                var inspeccion = inspecciones.Find(x => x.Id_Inspeccion_Cabecera.Equals(observacion.Id_Inspeccion_Cabecera));
                var gerencia = gerencias.Find(x => x.Id_Gerencia.Equals(inspeccion.Id_Gerencia));
                var gerenciaInspeccionada = gerencias.Find(x => x.Id_Gerencia.Equals(inspeccion.Id_Gerencia_Inspeccionada));
                var empresa = empresas.Find(x => x.Id_Empresa.Equals(inspeccion.Id_Empresa));

                var item = new Domain.Inspecciones.InspeccionDetalle
                {
                    IdInspeccionCabecera = observacion.Id_Inspeccion_Cabecera,
                    IdInspeccionDetalle = observacion.Id_Inspeccion_Detalle,
                    TipoObservacion = observacion.Tipo_Observacion,
                    DescripcionCondicion = observacion.Descripcion_Condicion,
                    
                    NivelRiesgo = observacion.Nivel_Riesgo,
                    DescripcionAccion = observacion.Descripcion_Accion,
                    ResponsableMarcobre = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = responsable1.Id_Usuario,
                        Nombres = responsable1.Nombres,
                        Apellidos = responsable1.Apellidos
                    },                   
                    ResponsableContratista = observacion.Responsable_Contratista,
                    CorreoContratista = observacion.Correo_Contratista,
                    FechaCierre = observacion.Fecha_Cierre,
                    Asesor = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = asesor.Id_Usuario,
                        Nombres = asesor.Nombres,
                        Apellidos = asesor.Apellidos
                    },
                    FechaProgramada = observacion.Fecha_Programada,
                    FechaVerificacion = observacion.Fecha_Verificacion,
                    Estado = observacion.Estado,
                    DescripcionActoCondicion = observacion.Descripcion_Acto_Condicion,
                    ComentarioImplementacion = observacion.Comentario_Implementacion,
                    ComentarioVerificacion = observacion.Comentario_Verificacion,
                    Cabecera = new Domain.Inspecciones.InspeccionCabecera
                    {
                        IdCabecera = inspeccion.Id_Inspeccion_Cabecera,
                        Codigo = inspeccion.Codigo,
                        FechaInspeccion = inspeccion.Fecha_Inspeccion,
                        Objetivo = inspeccion.Objetivo,
                        Gerencia = gerencia.Descripcion,
                        Empresa = empresa.Razon_Social,
                        TipoInspeccion = inspeccion.Tipo_Inspeccion,
                        GerenciaInspeccionada = gerenciaInspeccionada?.Descripcion,
                        AreaInspeccionada = inspeccion.Area_Inspeccionada
                    }
                };
                if (responsable2 != null)
                {
                    item.ResponsableMarcobre2 = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = responsable2.Id_Usuario,
                        Nombres = responsable2.Nombres,
                        Apellidos = responsable2.Apellidos
                    };
                }
                if (tipoCondicion != null)
                {
                    item.TipoCondicion = new Domain.ValueObjects.TipoCondicion
                    {
                        IdTipoCondicion = tipoCondicion.Id_Tipo_Condicion,
                        Descripcion = tipoCondicion.Descripcion,
                        Grupo = tipoCondicion.Grupo
                    };
                }
                output.Add(item);
            }
            return output;
        }

        public async Task<List<Domain.RAC.RAC>> RACs(DateTime startTime, DateTime endTime)
        {
            var output = new List<Domain.RAC.RAC>();

            var racs = await _context.RACs.Where(x=>
            x.Fecha >= startTime &&
            x.Fecha <= endTime)
                .ToListAsync();

            //Todos los tipos de condicion
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();
            //Todos los usuarios
            var usuarios = await _context.Usuarios.ToListAsync();
            //Todos las gerencias
            var gerencias = await _context.Gerencias.ToListAsync();
            //Todos las empresas
            var empresas = await _context.Empresas.ToListAsync();

            foreach(var rac in racs)
            {
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(rac.Id_Tipo_Condicion));
                var responsable1 = usuarios.Find(x => x.Id_Usuario.Equals(rac.Id_Responsable_Cierre));
                var asesor = usuarios.Find(x => x.Id_Usuario.Equals(rac.Id_Supervisor));
                var gerencia = gerencias.Find(x => x.Id_Gerencia.Equals(rac.Id_Gerencia));
                var gerenciaObs = gerencias.Find(x => x.Id_Gerencia.Equals(rac.Id_Gerencia_Obs));
                var empresa = empresas.Find(x => x.Id_Empresa.Equals(rac.Id_Empresa));
                var empresaObs = empresas.Find(x => x.Id_Empresa.Equals(rac.Id_Empresa_Obs));

                var item = new Domain.RAC.RAC
                {
                    Codigo = rac.Codigo,
                    Nombres_Apellidos = rac.Nombres_Apellidos,
                    Gerencia = new Domain.ValueObjects.Gerencia
                    {
                        IdGerencia = gerencia.Id_Gerencia,
                        Descripcion = gerencia.Descripcion,
                        Estado = gerencia.Estado
                    },
                    Empresa = new Domain.ValueObjects.Empresa
                    {
                        IdEmpresa = empresa.Id_Empresa,
                        RazonSocial = empresa.Razon_Social,
                        Estado = empresa.Estado
                    },
                    Otra_Empresa = rac.Otra_Empresa,
                    Empresa_Ob = new Domain.ValueObjects.Empresa
                    {
                        IdEmpresa = empresaObs.Id_Empresa,
                        RazonSocial = empresaObs.Razon_Social,
                        Estado = empresaObs.Estado
                    },
                    Gerencia_Obs = new Domain.ValueObjects.Gerencia
                    {
                        IdGerencia = gerenciaObs.Id_Gerencia,
                        Descripcion = gerenciaObs.Descripcion,
                        Estado = gerenciaObs.Estado
                    },
                    Ubicacion = rac.Ubicacion,
                    Fecha = rac.Fecha,
                    SSO_MA = rac.SSO_MA,
                    Clasificacion = rac.Clasificacion,
                    NivelRiesgo = rac.NivelRiesgo,
                    TipoCondicion = new Domain.ValueObjects.TipoCondicion
                    {
                        IdTipoCondicion = tipoCondicion.Id_Tipo_Condicion,
                        Descripcion = tipoCondicion.Descripcion,
                        Grupo = tipoCondicion.Grupo
                    },
                    Descripcion_Acto_Cond = rac.Descripcion_Acto_Cond,
                    Medida_Control = rac.Medida_Control,
                    PAC = rac.PAC,
                    Responsable_Cierre = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = responsable1.Id_Usuario,
                        Nombres = responsable1.Nombres,
                        Apellidos = responsable1.Apellidos
                    },
                    Fecha_Cierre = rac.Fecha_Cierre,
                    Supervisor = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = asesor.Id_Usuario,
                        Nombres = asesor.Nombres,
                        Apellidos = asesor.Apellidos
                    },
                    Comentario_Cierre = rac.Comentario_Cierre,
                    Comentario_Verificacion = rac.Comentario_Verificacion,
                    Estado = rac.Estado
                };
                output.Add(item);
            }
            return output;
        }
    }
}
