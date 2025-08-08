using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.Items;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Incidentes.InvestigacionFinal
{
    public class GetInvestigacionFinalRepository : IGetInvestigacionFinalReadOnlyRepository
    {
        private readonly Context _context;

        public GetInvestigacionFinalRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Investigacion_Final_Incidente>> GetInvestigacionesFinales(DateTime? fechaInicio, DateTime? fechaFin, int[] gerencia, int[] titular, List<string> estado, int[] responsable, int[] asesor, string codigo)
        {
            var investigaciones = await _context.Investigacion_Final_Incidente.ToListAsync();
            var filtrado = investigaciones
                .OrderByDescending(x=>x.Codigo)
                .ToList();

            if (fechaInicio.HasValue)
                filtrado = filtrado.Where(x => x.Fecha_Incidente.HasValue &&
                x.Fecha_Incidente.Value.Date >= fechaInicio.Value.Date).ToList();
            if (fechaFin.HasValue)
                filtrado = filtrado.Where(x => x.Fecha_Incidente.HasValue &&
                x.Fecha_Incidente.Value.Date <= fechaFin.Value.Date).ToList();
            if (gerencia.Length > 0)
                filtrado = filtrado.Where(x => x.Id_Gerencia_Responsable.HasValue &&
                gerencia.Contains(x.Id_Gerencia_Responsable.Value)).ToList();
            if (titular.Length > 0)
            {
                var investigaciones_titulares_minero = await _context.Involucrado_Incidente_Investigacion.Where(x =>
                x.Tipo_Involucrado.Equals("Supervisor Inmediato (Titular Minero)") &&
                x.Id_Usuario.HasValue &&
                titular.Contains(x.Id_Usuario.Value))
                    .Select(x => x.Id_Investigacion_Final).ToListAsync();

                filtrado = filtrado.Where(x => investigaciones_titulares_minero.Contains(x.Id_Investigacion_Final)).ToList();
            }
            if (asesor.Length > 0)
                filtrado = filtrado.Where(x => x.Id_Supervisor_MA.HasValue &&
                asesor.Contains(x.Id_Supervisor_MA.Value)).ToList();
            if (codigo != null && codigo.Length > 0)
                filtrado = filtrado.Where(x => x.Codigo.Contains(codigo)).ToList();
            if (responsable.Length > 0 || estado.Count > 0)
            {
                var acciones = await _context.Accion_Incidente_Investigacion.ToListAsync();
                if (responsable.Length > 0)
                {
                    acciones = acciones.Where(x => (x.Id_Usuario.HasValue &&
                    responsable.Contains(x.Id_Usuario.Value))|| (x.Id_Usuario2.HasValue &&
                    responsable.Contains(x.Id_Usuario2.Value))).ToList();

                    filtrado = filtrado.Where(x =>
                    acciones.Select(x => x.Id_Investigacion_Final).Contains(x.Id_Investigacion_Final)).ToList();
                }
                if (estado.Count > 0)
                {
                    acciones = acciones.Where(x =>
                        estado.Contains(x.Estado)).ToList();

                    filtrado = filtrado.Where(x =>
                    acciones.Select(x => x.Id_Investigacion_Final).Contains(x.Id_Investigacion_Final)).ToList();
                }
            }

            var tiposIncidente = await _context.Tipo_Incidente.ToListAsync();
            var nivelesIncidente = await _context.Nivel_Incidente.ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            var output = new List<Investigacion_Final_Incidente>();

            foreach (var item in filtrado)
            {
                var outputItem = new Investigacion_Final_Incidente();
                outputItem.Id_Investigacion_Final = item.Id_Investigacion_Final;
                outputItem.Id_Reporte_Preliminar = item.Id_Reporte_Preliminar;
                outputItem.Codigo = item.Codigo;
                outputItem.Num_Trabajadores = item.Num_Trabajadores;
                outputItem.Razon_Social_Contratista = item.Razon_Social_Contratista;
                outputItem.RUC_Contratista = item.RUC_Contratista;
                outputItem.Actividad_Contratista = item.Actividad_Contratista;
                outputItem.Domicilio_Contratista = item.Domicilio_Contratista;
                outputItem.Num_Trabajadores_Contratista = item.Num_Trabajadores_Contratista;
                outputItem.Id_Registrado_Por = item.Id_Registrado_Por;

                if (item.Id_Tipo_Incidente != null)
                {
                    var tipoIncidente = tiposIncidente.FirstOrDefault(x => x.Id_Tipo_Incidente.Equals(item.Id_Tipo_Incidente));
                    outputItem.Tipo_Incidente = new Tipo_Incidente
                    {
                        Id_Tipo_Incidente = tipoIncidente.Id_Tipo_Incidente,
                        Descripcion = tipoIncidente.Descripcion,
                        Estado = tipoIncidente.Estado
                    };
                }
                if (item.Id_Nivel_Incidente != null)
                {
                    var nivelIncidente = nivelesIncidente.FirstOrDefault(x => x.Id_Nivel_Incidente.Equals(item.Id_Nivel_Incidente));
                    outputItem.Nivel_Incidente = new Nivel_Incidente
                    {
                        Id_Nivel_Incidente = nivelIncidente.Id_Nivel_Incidente,
                        Descripcion = nivelIncidente.Descripcion,
                        Estado = nivelIncidente.Estado
                    };
                }
                outputItem.Fecha_Incidente = item.Fecha_Incidente;
                outputItem.Lugar_Incidente = item.Lugar_Incidente;
                outputItem.Descripcion_Incidente = item.Descripcion_Incidente;

                if (item.Id_Gerencia_Responsable != null)
                {
                    var gerenciaDomain = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Id_Gerencia_Responsable));
                    outputItem.Gerencia_Responsable = new Gerencia
                    {
                        IdGerencia = gerenciaDomain.Id_Gerencia,
                        Descripcion = gerenciaDomain.Descripcion,
                        Estado = gerenciaDomain.Estado
                    };
                }

                if (item.Id_Supervisor_MA != null)
                {
                    var supervisor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Supervisor_MA));
                    outputItem.Supervisor_MA = new Usuario
                    {
                        IdUsuario = supervisor.Id_Usuario,
                        Nombres = supervisor.Nombres,
                        Apellidos = supervisor.Apellidos,
                        Email = supervisor.Email,
                        IdRol = supervisor.Id_Rol,
                        UsuarioRed = supervisor.usuario_red,
                        Estado = supervisor.Estado
                    };
                }

                outputItem.Estado = item.Estado;
                var acciones = await _context.Accion_Incidente_Investigacion.Where(x =>x.Id_Investigacion_Final.Equals(item.Id_Investigacion_Final) && !x.Estado.Equals(EstadosIncidentes.Eliminado.ToString())).ToListAsync();
                outputItem.NAcciones = acciones.Count;
                outputItem.NAccionesAprobadas = acciones.Count(x => x.Estado.Equals(EstadosIncidentes.Aprobado.ToString()));

                output.Add(outputItem);
            }
            return output;
        }

        public async Task<Investigacion_Final_Incidente> GetInvestigacionFinal(int IdInvestigacion)
        {
            var investigacion = await _context.Investigacion_Final_Incidente.FirstOrDefaultAsync(x => x.Id_Investigacion_Final.Equals(IdInvestigacion));
            
            var output = new Investigacion_Final_Incidente();
            output.Id_Investigacion_Final = investigacion.Id_Investigacion_Final;
            output.Id_Reporte_Preliminar = investigacion.Id_Reporte_Preliminar;
            output.Codigo = investigacion.Codigo;
            output.Num_Trabajadores = investigacion.Num_Trabajadores;

            output.Razon_Social_Contratista = investigacion.Razon_Social_Contratista;
            output.RUC_Contratista = investigacion.RUC_Contratista;
            output.Actividad_Contratista = investigacion.Actividad_Contratista;
            output.Domicilio_Contratista = investigacion.Domicilio_Contratista;
            output.Num_Trabajadores_Contratista = investigacion.Num_Trabajadores_Contratista;

            output.Razon_Social_Subcontratista = investigacion.Razon_Social_Subcontratista;
            output.RUC_Subcontratista = investigacion.RUC_Subcontratista;
            output.Actividad_Subcontratista = investigacion.Actividad_Subcontratista;
            output.Domicilio_Subcontratista = investigacion.Domicilio_Subcontratista;
            output.Num_Trabajadores_Subcontratista = investigacion.Num_Trabajadores_Subcontratista;
            output.Id_Registrado_Por = investigacion.Id_Registrado_Por;

            if (investigacion.Id_Tipo_Incidente != null)
            {
                var tipoIncidente = await _context.Tipo_Incidente.FirstOrDefaultAsync(x => x.Id_Tipo_Incidente.Equals(investigacion.Id_Tipo_Incidente));
                output.Tipo_Incidente = new Tipo_Incidente
                {
                    Id_Tipo_Incidente = tipoIncidente.Id_Tipo_Incidente,
                    Descripcion = tipoIncidente.Descripcion,
                    Estado = tipoIncidente.Estado
                };
            }
            if (investigacion.Id_Nivel_Incidente != null)
            {
                var nivelIncidente = await _context.Nivel_Incidente.FirstOrDefaultAsync(x => x.Id_Nivel_Incidente.Equals(investigacion.Id_Nivel_Incidente));
                output.Nivel_Incidente = new Nivel_Incidente
                {
                    Id_Nivel_Incidente = nivelIncidente.Id_Nivel_Incidente,
                    Descripcion = nivelIncidente.Descripcion,
                    Estado = nivelIncidente.Estado
                };
            }
            output.Lugar_Incidente = investigacion.Lugar_Incidente;
            output.Coordenada_Este = investigacion.Coordenada_Este;
            output.Coordenada_Norte = investigacion.Coordenada_Norte;

            output.Localidad = investigacion.Localidad;
            output.Distrito = investigacion.Distrito;
            output.Provincia = investigacion.Provincia;
            output.Departamento = investigacion.Departamento;

            output.Fecha_Incidente = investigacion.Fecha_Incidente;

            output.Fecha_Inicio_Investigacion = investigacion.Fecha_Inicio_Investigacion;
            output.Turno = investigacion.Turno;

            output.Descripcion_Incidente = investigacion.Descripcion_Incidente;

            if (investigacion.Id_Gerencia_Responsable != null)
            {
                var gerencia = await _context.Gerencias.FirstOrDefaultAsync(x => x.Id_Gerencia.Equals(investigacion.Id_Gerencia_Responsable));
                output.Gerencia_Responsable = new Gerencia
                {
                    IdGerencia = gerencia.Id_Gerencia,
                    Descripcion = gerencia.Descripcion,
                    Estado = gerencia.Estado
                };
            }

            if (investigacion.Id_Supervisor_MA != null)
            {
                var supervisor = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(investigacion.Id_Supervisor_MA));
                output.Supervisor_MA = new Usuario
                {
                    IdUsuario = supervisor.Id_Usuario,
                    Nombres = supervisor.Nombres,
                    Apellidos = supervisor.Apellidos,
                    Email = supervisor.Email,
                    IdRol = supervisor.Id_Rol,
                    UsuarioRed = supervisor.usuario_red,
                    Estado = supervisor.Estado
                };
            }

            output.Evidencia_Papel = investigacion.Evidencia_Papel;
            output.Evidencia_Persona = investigacion.Evidencia_Persona;
            output.Evidencia_Posicion = investigacion.Evidencia_Posicion;
            output.Evidencia_Parte = investigacion.Evidencia_Parte;
            output.NombresArchivos = investigacion.Nombres_Archivos;

            output.Estado = investigacion.Estado;

            return output;
        }
        public async Task<Impacto_Ambiental_Incidente> GetImpactoAmbiental(int IdInvestigacion)
        {
            var impacto = await _context.Impacto_Ambiental_Incidente.FirstOrDefaultAsync(x => x.Id_Investigacion_Final.Equals(IdInvestigacion));
            if (impacto == null)
                return null;
            var tipoSuelo = await _context.Tipo_Suelo.FirstOrDefaultAsync(x => x.Id_Tipo_Suelo.Equals(impacto.Id_Tipo_Suelo));
            var unidadesMedida = await _context.Unidad_Medida.ToListAsync();
            var lugaresDisposicion = await _context.Lugar_Disposicion.ToListAsync();
            var especieFlora = await _context.Especie_Flora.FirstOrDefaultAsync(x => x.Id_Especie_Flora.Equals(impacto.Id_Especie_Flora));
            var especieFauna = await _context.Especie_Fauna.FirstOrDefaultAsync(x => x.Id_Especie_Fauna.Equals(impacto.Id_Especie_Fauna));
            var tipoAgua = await _context.Tipo_Agua.FirstOrDefaultAsync(x => x.Id_Tipo_Agua.Equals(impacto.Id_Tipo_Agua));

            var output = new Impacto_Ambiental_Incidente();
            output.Id_Impacto_Ambiental_Incidente = impacto.Id_Impacto_Ambiental_Incidente;
            output.Id_Investigacion_Final = impacto.Id_Investigacion_Final;

            if (tipoSuelo != null)
                output.Tipo_Suelo = new Tipo_Suelo {
                    Id_Tipo_Suelo = tipoSuelo.Id_Tipo_Suelo,
                    Descripcion = tipoSuelo.Descripcion,
                    Estado = tipoSuelo.Estado
                };
            output.Area_Suelo = impacto.Area_Suelo;

            var unidadAreaSuelo = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(impacto.Id_Unidad_Area_Suelo));
            if (unidadAreaSuelo != null)
                output.Unidad_Area_Suelo = new Unidad_Medida {
                    Id_Unidad_Medida = unidadAreaSuelo.Id_Unidad_Medida,
                    Tipo = unidadAreaSuelo.Tipo,
                    Descripcion = unidadAreaSuelo.Descripcion,
                    Estado = unidadAreaSuelo.Estado
                };

            output.Cantidad_Suelo = impacto.Cantidad_Suelo;
            var unidadCantidadSuelo = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(impacto.Id_Unidad_Cantidad_Suelo));
            if (unidadCantidadSuelo != null)
                output.Unidad_Cantidad_Suelo = new Unidad_Medida
                {
                    Id_Unidad_Medida = unidadCantidadSuelo.Id_Unidad_Medida,
                    Tipo = unidadCantidadSuelo.Tipo,
                    Descripcion = unidadCantidadSuelo.Descripcion,
                    Estado = unidadCantidadSuelo.Estado
                };
            var lugarDisposicionSuelo = lugaresDisposicion.FirstOrDefault(x => x.Id_Lugar_Disposicion.Equals(impacto.Id_Lugar_Disposicion_Suelo));
            if (lugarDisposicionSuelo != null)
                output.Lugar_Disposicion_Suelo = new Lugar_Disposicion { 
                    Id_Lugar_Disposicion = lugarDisposicionSuelo.Id_Lugar_Disposicion,
                    Descripcion = lugarDisposicionSuelo.Descripcion,
                    Estado = lugarDisposicionSuelo.Estado
                };

            if(especieFlora!=null)
                output.Especie_Flora = new Especie_Flora { 
                    Id_Especie_Flora = especieFlora.Id_Especie_Flora,
                    Descripcion = especieFlora.Descripcion,
                    Estado = especieFlora.Estado
                };

            output.Unidad_Flora = impacto.Unidad_Flora;

            var unidadFlora = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(impacto.Id_Unidad_Flora));
            if (unidadFlora != null)
                output.UnidadM_Flora = new Unidad_Medida {
                    Id_Unidad_Medida = unidadFlora.Id_Unidad_Medida,
                    Tipo = unidadFlora.Tipo,
                    Descripcion = unidadFlora.Descripcion,
                    Estado = unidadFlora.Estado
                };
            output.Cantidad_Residuo_Flora = impacto.Cantidad_Residuo_Flora;
            var unidadResiduoFlora = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(impacto.Id_Unidad_Residuo_Flora));
            if (unidadResiduoFlora != null)
                output.Unidad_Residuo_Flora = new Unidad_Medida
                {
                    Id_Unidad_Medida = unidadResiduoFlora.Id_Unidad_Medida,
                    Tipo = unidadResiduoFlora.Tipo,
                    Descripcion = unidadResiduoFlora.Descripcion,
                    Estado = unidadResiduoFlora.Estado
                };

            var lugarDisposicionFlora = lugaresDisposicion.FirstOrDefault(x => x.Id_Lugar_Disposicion.Equals(impacto.Id_Lugar_Disposicion_Flora));
            if (lugarDisposicionFlora != null)
                output.Lugar_Disposicion_Flora = new Lugar_Disposicion
                {
                    Id_Lugar_Disposicion = lugarDisposicionFlora.Id_Lugar_Disposicion,
                    Descripcion = lugarDisposicionFlora.Descripcion,
                    Estado = lugarDisposicionFlora.Estado
                };

            if (especieFauna != null)
                output.Especie_Fauna = new Especie_Fauna
                {
                    Id_Especie_Fauna = especieFauna.Id_Especie_Fauna,
                    Descripcion = especieFauna.Descripcion,
                    Estado = especieFauna.Estado
                };

            output.Unidad_Fauna = impacto.Unidad_Fauna;

            var unidadFauna = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(impacto.Id_Unidad_Fauna));
            if (unidadFauna != null)
                output.UnidadM_Fauna = new Unidad_Medida
                {
                    Id_Unidad_Medida = unidadFauna.Id_Unidad_Medida,
                    Tipo = unidadFauna.Tipo,
                    Descripcion = unidadFauna.Descripcion,
                    Estado = unidadFauna.Estado
                };

            output.Cantidad_Residuo_Fauna = impacto.Cantidad_Residuo_Fauna;

            var unidadResiduoFauna = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(impacto.Id_Unidad_Residuo_Fauna));
            if (unidadResiduoFauna != null)
                output.Unidad_Residuo_Fauna = new Unidad_Medida
                {
                    Id_Unidad_Medida = unidadResiduoFauna.Id_Unidad_Medida,
                    Tipo = unidadResiduoFauna.Tipo,
                    Descripcion = unidadResiduoFauna.Descripcion,
                    Estado = unidadResiduoFauna.Estado
                };

            var lugarDisposicionFauna = lugaresDisposicion.FirstOrDefault(x => x.Id_Lugar_Disposicion.Equals(impacto.Id_Lugar_Disposicion_Fauna));
            if (lugarDisposicionFauna != null)
                output.Lugar_Disposicion_Fauna = new Lugar_Disposicion
                {
                    Id_Lugar_Disposicion = lugarDisposicionFauna.Id_Lugar_Disposicion,
                    Descripcion = lugarDisposicionFauna.Descripcion,
                    Estado = lugarDisposicionFauna.Estado
                };

            if(tipoAgua!=null)
                output.Tipo_Agua = new Tipo_Agua
                {
                    Id_Tipo_Agua = tipoAgua.Id_Tipo_Agua,
                    Descripcion = tipoAgua.Descripcion,
                    Estado = tipoAgua.Estado
                };

            output.Volumen_Agua = impacto.Volumen_Agua;

            var unidadAgua = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(impacto.Id_Unidad_Agua));
            if (unidadAgua != null)
                output.Unidad_Agua = new Unidad_Medida
                {
                    Id_Unidad_Medida = unidadAgua.Id_Unidad_Medida,
                    Tipo = unidadAgua.Tipo,
                    Descripcion = unidadAgua.Descripcion,
                    Estado = unidadAgua.Estado
                };

            output.Cantidad_Residuo_Agua = impacto.Cantidad_Residuo_Agua;

            var unidadResiduoAgua = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(impacto.Id_Unidad_Residuo_Agua));
            if (unidadResiduoAgua != null)
                output.Unidad_Residuo_Agua = new Unidad_Medida
                {
                    Id_Unidad_Medida = unidadResiduoAgua.Id_Unidad_Medida,
                    Tipo = unidadResiduoAgua.Tipo,
                    Descripcion = unidadResiduoAgua.Descripcion,
                    Estado = unidadResiduoAgua.Estado
                };

            var lugarDisposicionAgua = lugaresDisposicion.FirstOrDefault(x => x.Id_Lugar_Disposicion.Equals(impacto.Id_Lugar_Disposicion_Agua));
            if (lugarDisposicionAgua != null)
                output.Lugar_Disposicion_Agua = new Lugar_Disposicion
                {
                    Id_Lugar_Disposicion = lugarDisposicionAgua.Id_Lugar_Disposicion,
                    Descripcion = lugarDisposicionAgua.Descripcion,
                    Estado = lugarDisposicionAgua.Estado
                };

            output.Restos_Arq = impacto.Restos_Arq;
            output.Nombre_Resto_Arq = impacto.Nombre_Resto_Arq;
            output.Cord_Este = impacto.Cord_Este;
            output.Cord_Norte = impacto.Cord_Norte;
            output.Danio_Salud = impacto.Danio_Salud;
            output.Personas_Afectadas = impacto.Personas_Afectadas;
            output.Cantidad_RS = impacto.Cantidad_RS;

            var unidadCantidadRS = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(impacto.Id_Unidad_Cantidad_RS));
            if (unidadCantidadRS != null)
            {
                output.Unidad_Cantidad_RS = new Unidad_Medida { 
                    Id_Unidad_Medida = unidadCantidadRS.Id_Unidad_Medida,
                    Descripcion = unidadCantidadRS.Descripcion,
                    Tipo = unidadCantidadRS.Tipo,
                    Estado = unidadCantidadRS.Estado
                }; 
            }

            output.Volumen_Rs = impacto.Volumen_Rs;

            var unidadVolumenRS = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(impacto.Id_Unidad_Volumen_RS));
            if (unidadVolumenRS != null)
            {
                output.Unidad_Volumen_RS = new Unidad_Medida
                {
                    Id_Unidad_Medida = unidadVolumenRS.Id_Unidad_Medida,
                    Descripcion = unidadVolumenRS.Descripcion,
                    Tipo = unidadVolumenRS.Tipo,
                    Estado = unidadVolumenRS.Estado
                };
            }
            output.EO_RS = impacto.EO_RS;

            return output;
        }
        public async Task<string> GetEstadoInvestigacion(int IdInvestigacion)
        {
            var investigacion = await _context.Investigacion_Final_Incidente.FirstOrDefaultAsync(x => x.Id_Investigacion_Final.Equals(IdInvestigacion));
            return investigacion.Estado;
        }
        public async Task<string> GetEvidenciaInvestigacion(int IdInvestigacion,string Tipo)
        {
            var investigacion = await _context.Investigacion_Final_Incidente.FirstOrDefaultAsync(x => x.Id_Investigacion_Final.Equals(IdInvestigacion));
            switch (Tipo)
            {
                case "Papel": return investigacion.Evidencia_Papel;
                case "Parte": return investigacion.Evidencia_Parte;
                case "Persona": return investigacion.Evidencia_Persona;
                default: return investigacion.Evidencia_Posicion;
            }
        }
        public async Task<string> GetEvidenciasNombreInvestigacion(int IdInvestigacion)
        {
            var investigacion = await _context.Investigacion_Final_Incidente.FirstOrDefaultAsync(x => x.Id_Investigacion_Final.Equals(IdInvestigacion));
            if (investigacion != null && investigacion.Nombres_Archivos != null)
                return investigacion.Nombres_Archivos;
            else
                return "[]";
        }
    }
}
