using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Incidentes.Items;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Incidentes.ReportePreliminar
{
    public class GetReportePreliminarRepository : IGetReportePreliminarReadOnlyRepository
    {
        private readonly Context _context;

        public GetReportePreliminarRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<Reporte_Preliminar_Incidente>> GetReportes(DateTime? fecha, int id_Usuario, int id_Gerencia)
        {
            var reportes = await _context.Reporte_Preliminar_Incidente.OrderByDescending(x=>x.Codigo).ToListAsync();
            var reportesFiltro = reportes.Where(x =>
            x.Fecha_Incidente.Equals(fecha.HasValue?fecha.Value:x.Fecha_Incidente) &&   
            x.Id_Supervisor_MA.Equals(id_Usuario!=0?id_Usuario:x.Id_Supervisor_MA)&&
            x.Id_Gerencia_Responsable.Equals(id_Gerencia!=0?id_Gerencia:x.Id_Gerencia_Responsable))
                .ToList();

            var tiposIncidente = await _context.Tipo_Incidente.ToListAsync();
            var nivelesIncidente = await _context.Nivel_Incidente.ToListAsync();
            var unidadesMedida = await _context.Unidad_Medida.ToListAsync();
            var componentesAmbientales = await _context.Componente_Ambiental.ToListAsync();
            var causasIncidente = await _context.Tipos_Condicion.ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            var output = new List<Reporte_Preliminar_Incidente>();

            foreach(var item in reportesFiltro)
            {
                var outputItem = new Reporte_Preliminar_Incidente();

                outputItem.Id_Reporte_Preliminar = item.Id_Reporte_Preliminar;
                outputItem.Codigo = item.Codigo;
                outputItem.Num_Trabajadores = item.Num_Trabajadores;
                outputItem.Razon_Social_Contratista = item.Razon_Social_Contratista;
                outputItem.RUC_Contratista = item.RUC_Contratista;
                outputItem.Actividad_Contratista = item.Actividad_Contratista;
                outputItem.Domicilio_Contratista = item.Domicilio_Contratista;
                outputItem.Num_Trabajadores_Contratista = item.Num_Trabajadores_Contratista;

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
                outputItem.Producto_Derramado = item.Producto_Derramado;

                outputItem.Cantidad_Derramado = item.Cantidad_Derramado;
                if (item.Id_Unidad_Cantidad_Derramado != null)
                {
                    var unidadMedida = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(item.Id_Unidad_Cantidad_Derramado));
                    outputItem.Unidad_Cantidad_Derramado = new Unidad_Medida
                    {
                        Id_Unidad_Medida = unidadMedida.Id_Unidad_Medida,
                        Tipo = unidadMedida.Tipo,
                        Descripcion = unidadMedida.Descripcion,
                        Estado = unidadMedida.Estado
                    };
                }

                outputItem.Area_Afectada = item.Area_Afectada;
                if (item.Id_Unidad_Area_Afectada != null)
                {
                    var unidadMedida = unidadesMedida.FirstOrDefault(x => x.Id_Unidad_Medida.Equals(item.Id_Unidad_Area_Afectada));
                    outputItem.Unidad_Area_Afectada = new Unidad_Medida
                    {
                        Id_Unidad_Medida = unidadMedida.Id_Unidad_Medida,
                        Tipo = unidadMedida.Tipo,
                        Descripcion = unidadMedida.Descripcion,
                        Estado = unidadMedida.Estado
                    };
                }

                outputItem.Lugar_Incidente = item.Lugar_Incidente;
                outputItem.Equipo_Involucrado = item.Equipo_Involucrado;

                if (item.Id_Componente_Ambiental != null)
                {
                    var componenteAmbiental = componentesAmbientales.FirstOrDefault(x => x.Id_Componente_Ambiental.Equals(item.Id_Componente_Ambiental));
                    outputItem.Componente_Ambiental = new Componente_Ambiental
                    {
                        Id_Componente_Ambiental = componenteAmbiental.Id_Componente_Ambiental,
                        Descripcion = componenteAmbiental.Descripcion,                        
                        Estado = componenteAmbiental.Estado
                    };
                }

                outputItem.Descripcion_Incidente = item.Descripcion_Incidente;

                if (item.Id_Causa_Incidente != null)
                {
                    var causaIncidente = causasIncidente.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Id_Causa_Incidente));
                    outputItem.Causa_Incidente = new Domain.ValueObjects.TipoCondicion
                    {
                        IdTipoCondicion = causaIncidente.Id_Tipo_Condicion,
                        Descripcion = causaIncidente.Descripcion,
                        Grupo = causaIncidente.Grupo
                    };
                }

                outputItem.Otra_Causa_Incidente = item.Otra_Causa_Incidente;

                if (item.Id_Gerencia_Responsable != null)
                {
                    var gerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(item.Id_Gerencia_Responsable));
                    outputItem.Gerencia_Responsable = new Gerencia
                    {
                        IdGerencia = gerencia.Id_Gerencia,
                        Descripcion = gerencia.Descripcion
                    };
                }

                outputItem.Comentario_Accion = item.Comentario_Accion;
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

                outputItem.Evidencia = item.Evidencia;
                outputItem.Estado = item.Estado;

                output.Add(outputItem);
            }

            return output;
        }
        public async Task<Reporte_Preliminar_Incidente> GetReporte(int IdReporte)
        {
            var reporte = await _context.Reporte_Preliminar_Incidente.FirstOrDefaultAsync(x => x.Id_Reporte_Preliminar.Equals(IdReporte));

            var output = new Reporte_Preliminar_Incidente();
            output.Id_Reporte_Preliminar = reporte.Id_Reporte_Preliminar;
            output.Codigo = reporte.Codigo;
            output.Num_Trabajadores = reporte.Num_Trabajadores;
            output.Razon_Social_Contratista = reporte.Razon_Social_Contratista;
            output.RUC_Contratista = reporte.RUC_Contratista;
            output.Actividad_Contratista = reporte.Actividad_Contratista;
            output.Domicilio_Contratista = reporte.Domicilio_Contratista;
            output.Num_Trabajadores_Contratista = reporte.Num_Trabajadores_Contratista;

            if (reporte.Id_Tipo_Incidente != null)
            {
                var tipoIncidente = await _context.Tipo_Incidente.FirstOrDefaultAsync(x => x.Id_Tipo_Incidente.Equals(reporte.Id_Tipo_Incidente));
                output.Tipo_Incidente = new Tipo_Incidente
                {
                    Id_Tipo_Incidente = tipoIncidente.Id_Tipo_Incidente,
                    Descripcion = tipoIncidente.Descripcion,
                    Estado = tipoIncidente.Estado
                };
            }
            if (reporte.Id_Nivel_Incidente != null)
            {
                var nivelIncidente = await _context.Nivel_Incidente.FirstOrDefaultAsync(x => x.Id_Nivel_Incidente.Equals(reporte.Id_Nivel_Incidente));
                output.Nivel_Incidente = new Nivel_Incidente
                {
                    Id_Nivel_Incidente = nivelIncidente.Id_Nivel_Incidente,
                    Descripcion = nivelIncidente.Descripcion,
                    Estado = nivelIncidente.Estado
                };
            }

            output.Fecha_Incidente = reporte.Fecha_Incidente;
            output.Producto_Derramado = reporte.Producto_Derramado;

            output.Cantidad_Derramado = reporte.Cantidad_Derramado;
            if (reporte.Id_Unidad_Cantidad_Derramado != null)
            {
                var unidadMedida = await _context.Unidad_Medida.FirstOrDefaultAsync(x => x.Id_Unidad_Medida.Equals(reporte.Id_Unidad_Cantidad_Derramado));
                output.Unidad_Cantidad_Derramado = new Unidad_Medida
                {
                    Id_Unidad_Medida = unidadMedida.Id_Unidad_Medida,
                    Tipo = unidadMedida.Tipo,
                    Descripcion = unidadMedida.Descripcion,
                    Estado = unidadMedida.Estado
                };
            }

            output.Area_Afectada = reporte.Area_Afectada;
            if (reporte.Id_Unidad_Area_Afectada != null)
            {
                var unidadMedida = await _context.Unidad_Medida.FirstOrDefaultAsync(x => x.Id_Unidad_Medida.Equals(reporte.Id_Unidad_Area_Afectada));
                output.Unidad_Area_Afectada = new Unidad_Medida
                {
                    Id_Unidad_Medida = unidadMedida.Id_Unidad_Medida,
                    Tipo = unidadMedida.Tipo,
                    Descripcion = unidadMedida.Descripcion,
                    Estado = unidadMedida.Estado
                };
            }

            output.Lugar_Incidente = reporte.Lugar_Incidente;
            output.Equipo_Involucrado = reporte.Equipo_Involucrado;

            if (reporte.Id_Componente_Ambiental != null)
            {
                var componenteAmbiental = await _context.Componente_Ambiental.FirstOrDefaultAsync(x => x.Id_Componente_Ambiental.Equals(reporte.Id_Componente_Ambiental));
                output.Componente_Ambiental = new Componente_Ambiental
                {
                    Id_Componente_Ambiental = componenteAmbiental.Id_Componente_Ambiental,
                    Descripcion = componenteAmbiental.Descripcion,
                    Estado = componenteAmbiental.Estado
                };
            }

            output.Descripcion_Incidente = reporte.Descripcion_Incidente;

            if (reporte.Id_Causa_Incidente != null)
            {
                var causaIncidente = await _context.Tipos_Condicion.FirstOrDefaultAsync(x => x.Id_Tipo_Condicion.Equals(reporte.Id_Causa_Incidente));
                output.Causa_Incidente = new Domain.ValueObjects.TipoCondicion
                {
                    IdTipoCondicion = causaIncidente.Id_Tipo_Condicion,
                    Descripcion = causaIncidente.Descripcion,
                    Grupo = causaIncidente.Grupo
                };
            }

            output.Otra_Causa_Incidente = reporte.Otra_Causa_Incidente;

            if (reporte.Id_Gerencia_Responsable != null)
            {
                var gerencia = await _context.Gerencias.FirstOrDefaultAsync(x => x.Id_Gerencia.Equals(reporte.Id_Gerencia_Responsable));
                output.Gerencia_Responsable = new Gerencia
                {
                    IdGerencia = gerencia.Id_Gerencia,
                    Descripcion = gerencia.Descripcion
                };
            }

            output.Comentario_Accion = reporte.Comentario_Accion;

            if (reporte.Id_Supervisor_MA != null)
            {
                var supervisor = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(reporte.Id_Supervisor_MA));
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

            output.Evidencia = reporte.Evidencia;
            output.Estado = reporte.Estado;

            return output;
        }
    }
}
