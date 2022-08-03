using GestionAmbiental.Application.Repositories.DashboardGeneral;
using GestionAmbiental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.DashboardGeneral
{
    public class GetRegistrosRepository : IGetRegistrosReadOnlyRepository
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
            var inspecciones = await _context.Inspecciones_Cabecera.Where(x =>
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

            foreach (var observacion in observaciones)
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
                        Nombres = responsable1.Nombres.ToUpper(),
                        Apellidos = responsable1.Apellidos.ToUpper()
                    },
                    ResponsableContratista = observacion.Responsable_Contratista,
                    CorreoContratista = observacion.Correo_Contratista,
                    FechaCierre = observacion.Fecha_Cierre,
                    Asesor = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = asesor.Id_Usuario,
                        Nombres = asesor.Nombres.ToUpper(),
                        Apellidos = asesor.Apellidos.ToUpper()
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
                        Nombres = responsable2.Nombres.ToUpper(),
                        Apellidos = responsable2.Apellidos.ToUpper()
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

            var racs = await _context.RACs.Where(x =>
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

            foreach (var rac in racs)
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
                        Nombres = responsable1.Nombres.ToUpper(),
                        Apellidos = responsable1.Apellidos.ToUpper()
                    },
                    Fecha_Cierre = rac.Fecha_Cierre,
                    Supervisor = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = asesor.Id_Usuario,
                        Nombres = asesor.Nombres.ToUpper(),
                        Apellidos = asesor.Apellidos.ToUpper()
                    },
                    Comentario_Cierre = rac.Comentario_Cierre,
                    Comentario_Verificacion = rac.Comentario_Verificacion,
                    Estado = rac.Estado
                };
                output.Add(item);
            }
            return output;
        }
        public async Task<List<Domain.Incidentes.ReportePreliminar.Accion_Incidente_Reporte>> AccionesReportePreliminar(DateTime startTime, DateTime endTime)
        {
            var output = new List<Domain.Incidentes.ReportePreliminar.Accion_Incidente_Reporte>();
            var reportesPrelim = await _context.Reporte_Preliminar_Incidente
                .Where(x =>
                    x.Fecha_Incidente >= startTime &&
                    x.Fecha_Incidente <= endTime)
                .ToListAsync();

            var acciones = await _context.Accion_Incidente_Reporte.Where(x =>
                !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                reportesPrelim.Select(y => y.Id_Reporte_Preliminar).Contains(x.Id_Reporte_Preliminar))
                .ToListAsync();

            //Todos los usuarios
            var usuarios = await _context.Usuarios.ToListAsync();
            //Todos las gerencias
            var gerencias = await _context.Gerencias.ToListAsync();

            foreach (var accion in acciones)
            {
                var responsable1 = usuarios.Find(x => x.Id_Usuario.Equals(accion.Id_Usuario));
                var reporte = reportesPrelim.Find(x => x.Id_Reporte_Preliminar.Equals(accion.Id_Reporte_Preliminar));
                var gerencia = gerencias.Find(x => x.Id_Gerencia.Equals(reporte.Id_Gerencia_Responsable));

                var item = new Domain.Incidentes.ReportePreliminar.Accion_Incidente_Reporte
                {
                    Reporte = new Domain.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente
                    {
                        Codigo = reporte.Codigo,
                        Fecha_Incidente = reporte.Fecha_Incidente,
                        Razon_Social_Contratista = reporte.Razon_Social_Contratista,
                        Actividad_Contratista = reporte.Actividad_Contratista,
                        Gerencia_Responsable = new Domain.ValueObjects.Gerencia
                        {
                            IdGerencia = gerencia.Id_Gerencia,
                            Descripcion = gerencia.Descripcion,
                            Estado = gerencia.Estado
                        }
                    },
                    Accion_Inmediata = accion.Accion_Inmediata,
                    Responsable = accion.Responsable,
                    Fecha = accion.Fecha,
                    Estatus = accion.Estatus,
                    Estado = accion.Estado
                };
                if (responsable1 != null)
                {
                    item.ResponsableMarcobre = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = responsable1.Id_Usuario,
                        Nombres = responsable1.Nombres.ToUpper(),
                        Apellidos = responsable1.Apellidos.ToUpper()
                    };
                }
                output.Add(item);
            }
            return output;
        }
        public async Task<List<Domain.Incidentes.ReportePreliminar.Causa_Incidente_Reporte>> CausasReportePreliminar(DateTime startTime, DateTime endTime)
        {
            var output = new List<Domain.Incidentes.ReportePreliminar.Causa_Incidente_Reporte>();
            var reportesPrelim = await _context.Reporte_Preliminar_Incidente
                .Where(x =>
                    x.Fecha_Incidente >= startTime &&
                    x.Fecha_Incidente <= endTime)
                .ToListAsync();

            var causas = await _context.Causa_Incidente_Reporte.Where(x =>
                !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                reportesPrelim.Select(y => y.Id_Reporte_Preliminar).Contains(x.Id_Reporte_Preliminar))
                .ToListAsync();

            //Todos las gerencias
            var gerencias = await _context.Gerencias.ToListAsync();
            //Todos los tipos Condicion
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();

            foreach (var causa in causas)
            {
                var reporte = reportesPrelim.Find(x => x.Id_Reporte_Preliminar.Equals(causa.Id_Reporte_Preliminar));
                var gerencia = gerencias.Find(x => x.Id_Gerencia.Equals(reporte.Id_Gerencia_Responsable));
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(causa.Id_Acto_Condicion));

                var item = new Domain.Incidentes.ReportePreliminar.Causa_Incidente_Reporte
                {
                    Reporte = new Domain.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente
                    {
                        Codigo = reporte.Codigo,
                        Fecha_Incidente = reporte.Fecha_Incidente,
                        Razon_Social_Contratista = reporte.Razon_Social_Contratista,
                        Actividad_Contratista = reporte.Actividad_Contratista,
                        Gerencia_Responsable = new Domain.ValueObjects.Gerencia
                        {
                            IdGerencia = gerencia.Id_Gerencia,
                            Descripcion = gerencia.Descripcion,
                            Estado = gerencia.Estado
                        }
                    },
                    Tipo_Acto_Condicion = causa.Tipo_Acto_Condicion,
                    Estado = causa.Estado
                };
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
        public async Task<List<Domain.Incidentes.InvestigacionFinal.Accion_Incidente_Investigacion>> AccionesInvestigacion(DateTime startTime, DateTime endTime)
        {
            var output = new List<Domain.Incidentes.InvestigacionFinal.Accion_Incidente_Investigacion>();
            var intigacionesFinales = await _context.Investigacion_Final_Incidente
                .Where(x =>
                    x.Fecha_Incidente >= startTime &&
                    x.Fecha_Incidente <= endTime)
                .ToListAsync();

            var acciones = await _context.Accion_Incidente_Investigacion.Where(x =>
                !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Investigacion_Final.HasValue &&
                intigacionesFinales.Select(y => y.Id_Investigacion_Final).Contains(x.Id_Investigacion_Final.Value))
                .ToListAsync();

            //Todos los usuarios
            var usuarios = await _context.Usuarios.ToListAsync();
            //Todos las gerencias
            var gerencias = await _context.Gerencias.ToListAsync();

            foreach (var accion in acciones)
            {
                var responsable1 = usuarios.Find(x => x.Id_Usuario.Equals(accion.Id_Usuario));
                var responsable2 = usuarios.Find(x => x.Id_Usuario.Equals(accion.Id_Usuario2));
                var investigacion = intigacionesFinales.Find(x => x.Id_Investigacion_Final.Equals(accion.Id_Investigacion_Final.Value));
                var gerencia = gerencias.Find(x => x.Id_Gerencia.Equals(investigacion.Id_Gerencia_Responsable));

                var item = new Domain.Incidentes.InvestigacionFinal.Accion_Incidente_Investigacion
                {
                    InvestigacionFinal = new Domain.Incidentes.InvestigacionFinal.Investigacion_Final_Incidente
                    {
                        Codigo = investigacion.Codigo,
                        Fecha_Incidente = investigacion.Fecha_Incidente,
                        Razon_Social_Contratista = investigacion.Razon_Social_Contratista,
                        Actividad_Contratista = investigacion.Actividad_Contratista,
                        Razon_Social_Subcontratista = investigacion.Razon_Social_Subcontratista,
                        Actividad_Subcontratista = investigacion.Actividad_Subcontratista,
                        Gerencia_Responsable = new Domain.ValueObjects.Gerencia
                        {
                            IdGerencia = gerencia.Id_Gerencia,
                            Descripcion = gerencia.Descripcion,
                            Estado = gerencia.Estado
                        }
                    },
                    Tipo_Accion = accion.Tipo_Accion,
                    Accion_Inmediata = accion.Accion_Inmediata,
                    Responsable = accion.Responsable,
                    Comentarios_Implementacion = accion.Comentarios_Implementacion,
                    Comentarios_Verificacion = accion.Comentarios_Verificacion,
                    Fecha = accion.Fecha,
                    Fecha_Implementacion = accion.Fecha_Implementacion,
                    Fecha_Verificacion = accion.Fecha_Verificacion,
                    Estatus = accion.Estatus,
                    Estado = accion.Estado
                };
                if (responsable1 != null)
                {
                    item.Usuario = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = responsable1.Id_Usuario,
                        Nombres = responsable1.Nombres.ToUpper(),
                        Apellidos = responsable1.Apellidos.ToUpper()
                    };
                }
                if (responsable2 != null)
                {
                    item.Usuario2 = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = responsable2.Id_Usuario,
                        Nombres = responsable2.Nombres.ToUpper(),
                        Apellidos = responsable2.Apellidos.ToUpper()
                    };
                }
                output.Add(item);
            }
            return output;
        }
        public async Task<List<Domain.Incidentes.InvestigacionFinal.Causa_Incidente_Investigacion>> CausasInvestigacion(DateTime startTime, DateTime endTime)
        {
            var output = new List<Domain.Incidentes.InvestigacionFinal.Causa_Incidente_Investigacion>();
            var investigacionesFinales = await _context.Investigacion_Final_Incidente
                .Where(x =>
                    x.Fecha_Incidente >= startTime &&
                    x.Fecha_Incidente <= endTime)
                .ToListAsync();

            var causas = await _context.Causa_Incidente_Investigacion.Where(x =>
                !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                x.Id_Investigacion_Final.HasValue &&
                investigacionesFinales.Select(y => y.Id_Investigacion_Final).Contains(x.Id_Investigacion_Final.Value))
                .ToListAsync();

            //Todos las gerencias
            var gerencias = await _context.Gerencias.ToListAsync();
            //Todos los tipos Condicion
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();
            //Todos los factores
            var factores = await _context.Factor_Causa_Incidente.ToListAsync();

            foreach (var causa in causas)
            {
                var investigacion = investigacionesFinales.Find(x => x.Id_Investigacion_Final.Equals(causa.Id_Investigacion_Final));
                var gerencia = gerencias.Find(x => x.Id_Gerencia.Equals(investigacion.Id_Gerencia_Responsable));
                var tipoCondicion = tiposCondicion.Find(x => x.Id_Tipo_Condicion.Equals(causa.Id_Acto_Condicion));
                var factorCausa = factores.Find(x => x.Id_Factor_Causa_Incidente.Equals(causa.Id_Factor_Causa_Incidente));

                var item = new Domain.Incidentes.InvestigacionFinal.Causa_Incidente_Investigacion
                {
                    Investigacion_Final = new Domain.Incidentes.InvestigacionFinal.Investigacion_Final_Incidente
                    {
                        Codigo = investigacion.Codigo,
                        Fecha_Incidente = investigacion.Fecha_Incidente,
                        Razon_Social_Contratista = investigacion.Razon_Social_Contratista,
                        Actividad_Contratista = investigacion.Actividad_Contratista,
                        Razon_Social_Subcontratista = investigacion.Razon_Social_Subcontratista,
                        Actividad_Subcontratista = investigacion.Actividad_Subcontratista,
                        Gerencia_Responsable = new Domain.ValueObjects.Gerencia
                        {
                            IdGerencia = gerencia.Id_Gerencia,
                            Descripcion = gerencia.Descripcion,
                            Estado = gerencia.Estado
                        }
                    },
                    Tipo_Acto_Condicion = causa.Tipo_Acto_Condicion,
                    Descripcion = causa.Descripcion,
                    Estado = causa.Estado
                };
                if (tipoCondicion != null)
                {
                    item.TipoCondicion = new Domain.ValueObjects.TipoCondicion
                    {
                        IdTipoCondicion = tipoCondicion.Id_Tipo_Condicion,
                        Descripcion = tipoCondicion.Descripcion,
                        Grupo = tipoCondicion.Grupo
                    };
                }
                if (factorCausa != null)
                {
                    item.Factor_Causa_Incidente = new Domain.Incidentes.InvestigacionFinal.Factor_Causa_Incidente
                    {
                        Id_Factor_Causa_Incidente = factorCausa.Id_Factor_Causa_Incidente,
                        Descripcion = factorCausa.Descripcion,
                        Estado = factorCausa.Estado,
                        Tipo_Factor = factorCausa.Tipo_Factor
                    };
                }
                output.Add(item);
            }
            return output;
        }
        public async Task<List<Domain.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente>> ProductosDerramadosRP(DateTime startTime, DateTime endTime)
        {
            var output = new List<Domain.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente>();
            var reportes = await _context.Reporte_Preliminar_Incidente
                .Where(x =>
                    !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                    x.Fecha_Incidente >= startTime &&
                    x.Fecha_Incidente <= endTime)
                .ToListAsync();

            //Todos las unidades de medida
            var unidades = await _context.Unidad_Medida.ToListAsync();
            //Todos los componentes 
            var componentes = await _context.Componente_Ambiental.ToListAsync();
            //Todos las gerencias
            var gerencias = await _context.Gerencias.ToListAsync();
            //Todos los usuarios
            var usuarios = await _context.Usuarios.ToListAsync();

            foreach (var reporte in reportes)
            {
                var unidadCantidad = unidades.Find(x => x.Id_Unidad_Medida.Equals(reporte.Id_Unidad_Cantidad_Derramado));
                var unidadArea = unidades.Find(x => x.Id_Unidad_Medida.Equals(reporte.Id_Unidad_Area_Afectada));
                var componente = componentes.Find(x => x.Id_Componente_Ambiental.Equals(reporte.Id_Componente_Ambiental));
                var gerencia = gerencias.Find(x => x.Id_Gerencia.Equals(reporte.Id_Gerencia_Responsable));
                var usuario = usuarios.Find(x => x.Id_Usuario.Equals(reporte.Id_Supervisor_MA));

                var item = new Domain.Incidentes.ReportePreliminar.Reporte_Preliminar_Incidente
                {
                    Codigo = reporte.Codigo,
                    Fecha_Incidente = reporte.Fecha_Incidente,
                    Razon_Social_Contratista = reporte.Razon_Social_Contratista,
                    Actividad_Contratista = reporte.Actividad_Contratista,
                    Gerencia_Responsable = new Domain.ValueObjects.Gerencia
                    {
                        IdGerencia = gerencia.Id_Gerencia,
                        Descripcion = gerencia.Descripcion,
                        Estado = gerencia.Estado
                    },
                    Producto_Derramado = reporte.Producto_Derramado,
                    Cantidad_Derramado = reporte.Cantidad_Derramado,
                    Unidad_Cantidad_Derramado = new Domain.Incidentes.Items.Unidad_Medida
                    {
                        Id_Unidad_Medida = unidadCantidad.Id_Unidad_Medida,
                        Descripcion = unidadCantidad.Descripcion,
                        Estado = unidadCantidad.Estado,
                        Tipo = unidadCantidad.Tipo
                    },
                    Area_Afectada = reporte.Area_Afectada,
                    Unidad_Area_Afectada = new Domain.Incidentes.Items.Unidad_Medida
                    {
                        Id_Unidad_Medida = unidadArea.Id_Unidad_Medida,
                        Descripcion = unidadArea.Descripcion,
                        Estado = unidadArea.Estado,
                        Tipo = unidadArea.Tipo
                    },
                    Lugar_Incidente = reporte.Lugar_Incidente,
                    Equipo_Involucrado = reporte.Equipo_Involucrado,
                    Componente_Ambiental = new Domain.Incidentes.Items.Componente_Ambiental
                    {
                        Id_Componente_Ambiental = componente.Id_Componente_Ambiental,
                        Descripcion = componente.Descripcion,
                        Estado = componente.Estado
                    },
                    Descripcion_Incidente = reporte.Descripcion_Incidente,
                    Otra_Causa_Incidente = reporte.Otra_Causa_Incidente,
                    Comentario_Accion = reporte.Comentario_Accion,
                    Estado = reporte.Estado
                };
                if (usuario != null)
                {
                    item.Supervisor_MA = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = usuario.Id_Usuario,
                        Nombres = usuario.Nombres.ToUpper(),
                        Apellidos = usuario.Apellidos.ToUpper()
                    };
                }
                output.Add(item);
            }
            return output;
        }
        public async Task<List<Domain.Incidentes.InvestigacionFinal.Producto_Derramado_Incidente>> ProductosDerramadosInvestigacion(DateTime startTime, DateTime endTime)
        {
            var output = new List<Domain.Incidentes.InvestigacionFinal.Producto_Derramado_Incidente>();
            var investigacionesFinales = await _context.Investigacion_Final_Incidente
                .Where(x =>
                    x.Fecha_Incidente >= startTime &&
                    x.Fecha_Incidente <= endTime)
                .ToListAsync();

            var productosDerramados = await _context.Producto_Derramado_Incidente.Where(x =>
                !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString()) &&
                investigacionesFinales.Select(y => y.Id_Investigacion_Final).Contains(x.Id_Investigacion_Final))
                .ToListAsync();

            //Todos las unidades de medida
            var unidades = await _context.Unidad_Medida.ToListAsync();
            //Todos los tipos de producto incidente 
            var productosIncidente = await _context.Producto_Incidente.ToListAsync();
            //Todos los tipos de producto incidente 
            var tiposProducto = await _context.Tipo_Producto_Incidente.ToListAsync();
            //Todos las estados del equipo
            var estadosEquipo = await _context.Estado_Equipo_Incidente.ToListAsync();
            //Todos los usuarios
            var usuarios = await _context.Usuarios.ToListAsync();
            //Todos las gerencias
            var gerencias = await _context.Gerencias.ToListAsync();

            foreach (var productoDerramado in productosDerramados)
            {
                var investigacion = investigacionesFinales.Find(x => x.Id_Investigacion_Final.Equals(productoDerramado.Id_Investigacion_Final));
                var productoIncidente = productosIncidente.Find(x => x.Id_Producto_Incidente.Equals(productoDerramado.Id_Producto_Incidente));
                var tipoProducto = tiposProducto.Find(x => x.Id_Tipo_Producto_Incidente.Equals(productoDerramado.Id_Tipo_Producto));
                var unidadMedida = unidades.Find(x => x.Id_Unidad_Medida.Equals(productoDerramado.Id_Unidad));
                var estadoEquipo = estadosEquipo.Find(x => x.Id_Estado_Equipo_Incidente.Equals(productoDerramado.Id_Estado_Equipo_Responsable));
                var gerencia = gerencias.Find(x => x.Id_Gerencia.Equals(investigacion.Id_Gerencia_Responsable));

                var item = new Domain.Incidentes.InvestigacionFinal.Producto_Derramado_Incidente
                {
                    Investigacion_Final = new Domain.Incidentes.InvestigacionFinal.Investigacion_Final_Incidente
                    {
                        Codigo = investigacion.Codigo,
                        Fecha_Incidente = investigacion.Fecha_Incidente,
                        Razon_Social_Contratista = investigacion.Razon_Social_Contratista,
                        Actividad_Contratista = investigacion.Actividad_Contratista,
                        Razon_Social_Subcontratista = investigacion.Razon_Social_Subcontratista,
                        Actividad_Subcontratista = investigacion.Actividad_Subcontratista,
                        Gerencia_Responsable = new Domain.ValueObjects.Gerencia
                        {
                            IdGerencia = gerencia.Id_Gerencia,
                            Descripcion = gerencia.Descripcion,
                            Estado = gerencia.Estado
                        }
                    },
                    Producto_Incidente = new Domain.Incidentes.Items.Producto_Incidente
                    {
                        Id_Producto_Incidente = productoIncidente.Id_Producto_Incidente,
                        Descripcion = productoIncidente.Descripcion,
                        Estado = productoIncidente.Estado
                    },
                    Cantidad_Derramada = productoDerramado.Cantidad_Derramada,
                    Cantidad_No_Recuperado = productoDerramado.Cantidad_No_Recuperado,
                    Cantidad_Recuperado = productoDerramado.Cantidad_Recuperado,
                    Equipo_Responsable = productoDerramado.Equipo_Responsable,
                    Estado_Equipo_Responsable = new Domain.Incidentes.Items.Estado_Equipo_Incidente
                    {
                        Id_Estado_Equipo_Incidente = estadoEquipo.Id_Estado_Equipo_Incidente,
                        Descripcion = estadoEquipo.Descripcion,
                        Estado = estadoEquipo.Estado
                    },
                    Estado_Producto = productoDerramado.Estado_Producto,
                    Tipo_Producto = new Domain.Incidentes.Items.Tipo_Producto_Incidente
                    {
                        Id_Tipo_Producto_Incidente = tipoProducto.Id_Tipo_Producto_Incidente,
                        Descripcion = tipoProducto.Descripcion,
                        Estado = tipoProducto.Estado
                    },
                    Unidad = new Domain.Incidentes.Items.Unidad_Medida
                    {
                        Id_Unidad_Medida = unidadMedida.Id_Unidad_Medida,
                        Estado = unidadMedida.Estado,
                        Descripcion = unidadMedida.Descripcion,
                        Tipo = unidadMedida.Tipo
                    },
                    Estado = productoDerramado.Estado
                };
                output.Add(item);
            }
            return output;
        }
        public async Task<List<Domain.AvistamientoFauna.AvistamientoFauna>>Avistamientos(DateTime startTime, DateTime endTime)
        {
            var output = new List<Domain.AvistamientoFauna.AvistamientoFauna>();
            var avistamientos = await _context.Avistamientos_Fauna
                .Where(x=>!x.Estado.Equals(EstadosAvistamiento.Eliminado.ToString()))
                .ToListAsync();

            //Todos los nombres comunes
            var nombresComunes = await _context.Nombres_Comunes_Fauna.ToListAsync();
            //Todos los tipos avistamiento
            var tiposAvistamiento = await _context.Tipos_Avistamiento.ToListAsync();
            //Todos los estados fauna
            var estadosFauna = await _context.Estados_Fauna.ToListAsync();
            //Todos los luegares referencia
            var lugaresReferencia = await _context.Lugares_Referencia_AF.ToListAsync();
            //Todos los usuarios
            var usuarios = await _context.Usuarios.ToListAsync();

            foreach(var avistamiento in avistamientos)
            {
                var nombreComun = nombresComunes.Find(x => x.Id_Nombre_Comun_Fauna.Equals(avistamiento.Id_Nombre_Comun_Fauna));
                var tipoAvistamiento = tiposAvistamiento.Find(x => x.Id_Tipo_Avistamiento.Equals(avistamiento.Id_Tipo_Avistamiento));
                var estadoFauna = estadosFauna.Find(x => x.Id_Estado_Fauna.Equals(avistamiento.Id_Estado_Fauna));
                var lugarReferencia = lugaresReferencia.Find(x => x.Id_Lugar_Referencia.Equals(avistamiento.Id_Lugar_Referencia));
                var usuario = usuarios.Find(x => x.Id_Usuario.Equals(avistamiento.Id_Usuario));
                var supervisor = usuarios.Find(x => x.Id_Usuario.Equals(avistamiento.Id_Supervisor));

                var item = new Domain.AvistamientoFauna.AvistamientoFauna
                {
                    IdAvistamientoFauna = avistamiento.Id_Avistamiento_Fauna,
                    Ubicacion = avistamiento.Ubicacion,
                    Cantidad = avistamiento.Cantidad,
                    Codigo = avistamiento.Codigo,
                    DescripcionAccion = avistamiento.Descripcion_Accion,
                    FechaAvistamiento = avistamiento.Fecha_Avistamiento,
                    EvidenciaAccion = avistamiento.Evidencia_Accion,
                    Fotos = avistamiento.Fotos,
                    Estado = avistamiento.Estado,
                    EstadoFauna = new Domain.AvistamientoFauna.Items.EstadoFauna
                    {
                        IdEstadoFauna = estadoFauna.Id_Estado_Fauna,
                        Descripcion = estadoFauna.Descripcion,
                        Estado = estadoFauna.Estado
                    },
                    Usuario = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = usuario.Id_Usuario,
                        Nombres = usuario.Nombres.ToUpper(),
                        Apellidos = usuario.Apellidos.ToUpper()
                    },
                    NombreComun = new Domain.AvistamientoFauna.Items.NombreComunFauna
                    {
                        IdNombreComunFauna = nombreComun.Id_Nombre_Comun_Fauna,
                        Descripcion = nombreComun.Descripcion,
                        Estado = nombreComun.Estado,
                        Foto = nombreComun.Foto                        
                    },
                    TipoAvistamiento = new Domain.AvistamientoFauna.Items.TipoAvistamiento
                    {
                        IdTipoAvistamiento = tipoAvistamiento.Id_Tipo_Avistamiento,
                        Descripcion = tipoAvistamiento.Descripcion,
                        Estado = tipoAvistamiento.Estado
                    },
                    LugarReferencia = new Domain.AvistamientoFauna.Items.LugarReferencia
                    {
                        IdLugarReferencia = lugarReferencia.Id_Lugar_Referencia,
                        Descripcion = lugarReferencia.Descripcion,
                        Estado = lugarReferencia.Estado,
                        Latitud = lugarReferencia.Latitud,
                        Longitud = lugarReferencia.Longitud
                    }                    
                };
                if (supervisor != null)
                {
                    item.Supervisor = new Domain.ValueObjects.Usuario
                    {
                        IdUsuario = supervisor.Id_Usuario,
                        Nombres = supervisor.Nombres.ToUpper(),
                        Apellidos = supervisor.Apellidos.ToUpper()
                    };
                }
                output.Add(item);
            }
            return output;
        }
    }
}
