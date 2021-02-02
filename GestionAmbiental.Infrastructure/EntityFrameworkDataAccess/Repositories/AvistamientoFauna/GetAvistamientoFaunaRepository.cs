using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.AvistamientoFauna
{
    public class GetAvistamientoFaunaRepository : IGetAvistamientoFaunaReadOnlyRepository
    {
        private readonly Context _context;

        public GetAvistamientoFaunaRepository(Context context)
        {
            _context = context;
        }

        public async Task<Domain.AvistamientoFauna.AvistamientoFauna> GetAvistamientoFauna(int id_avistamiento)
        {
            var avistamientoFauna = await _context.Avistamientos_Fauna.FirstOrDefaultAsync(x => x.Id_Avistamiento_Fauna.Equals(id_avistamiento));

            var output = new Domain.AvistamientoFauna.AvistamientoFauna();
            output.IdAvistamientoFauna = avistamientoFauna.Id_Avistamiento_Fauna;
            output.Codigo = avistamientoFauna.Codigo;
            output.Fotos = avistamientoFauna.Fotos;
            output.FechaAvistamiento = avistamientoFauna.Fecha_Avistamiento;
            output.Ubicacion = avistamientoFauna.Ubicacion;
            output.Cantidad = avistamientoFauna.Cantidad;


            if (avistamientoFauna.Id_Nombre_Comun_Fauna != null)
            {
                var nombreComun = await _context.Nombres_Comunes_Fauna.FirstOrDefaultAsync(x => x.Id_Nombre_Comun_Fauna.Equals(avistamientoFauna.Id_Nombre_Comun_Fauna));
                var tipoFauna = await _context.Tipos_Fauna.FirstOrDefaultAsync(x => x.Id_Tipo_Fauna.Equals(nombreComun.Id_Tipo_Fauna));
                var nombreComunDomain = new Domain.AvistamientoFauna.Items.NombreComunFauna
                {
                    IdNombreComunFauna = nombreComun.Id_Nombre_Comun_Fauna,
                    Descripcion = nombreComun.Descripcion,
                    Foto = nombreComun.Foto,
                    TipoFauna = new Domain.AvistamientoFauna.Items.TipoFauna
                    {
                        IdTipoFauna = tipoFauna.Id_Tipo_Fauna,
                        Descripcion = tipoFauna.Descripcion,
                        Estado = tipoFauna.Estado
                    },
                    Estado = nombreComun.Estado
                };

                output.NombreComun = nombreComunDomain;
            }
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(avistamientoFauna.Id_Usuario));
            var usuarioDomain = new Domain.ValueObjects.Usuario
            {
                IdUsuario = usuario.Id_Usuario,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Email = usuario.Email,
                UsuarioRed = usuario.usuario_red,
                IdRol = usuario.Id_Rol,
                Estado = usuario.Estado,
                IdSuperior = usuario.Id_Superior,
                DNI = usuario.DNI,
                Celular = usuario.Celular,
                Puesto = usuario.Puesto
            };
            output.Usuario = usuarioDomain;

            var empresa = await _context.Empresas.FirstOrDefaultAsync(x => x.Id_Empresa.Equals(usuario.Id_Empresa));
            var areaTrabajo = await _context.Areas_Trabajo.FirstOrDefaultAsync(x => x.Id_Area_Trabajo.Equals(usuario.Id_Area_Trabajo));

            if (empresa != null)
                usuarioDomain.Empresa = new Domain.ValueObjects.Empresa { IdEmpresa = empresa.Id_Empresa, RazonSocial = empresa.Razon_Social, Estado = empresa.Estado };
            if (areaTrabajo != null)
                usuarioDomain.AreaTrabajo = new Domain.ValueObjects.AreaTrabajo { IdAreaTrabajo = areaTrabajo.Id_Area_Trabajo, Descripcion = areaTrabajo.Descripcion, Estado = areaTrabajo.Estado };

            var supervisor = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id_Usuario.Equals(avistamientoFauna.Id_Supervisor));

            if (avistamientoFauna.Id_Tipo_Avistamiento != null)
            {
                var tipoAvistamiento = await _context.Tipos_Avistamiento.FirstOrDefaultAsync(x => x.Id_Tipo_Avistamiento.Equals(avistamientoFauna.Id_Tipo_Avistamiento));
                var tipoAvistamientoDomain = new Domain.AvistamientoFauna.Items.TipoAvistamiento
                {
                    IdTipoAvistamiento = tipoAvistamiento.Id_Tipo_Avistamiento,
                    Descripcion = tipoAvistamiento.Descripcion,
                    Estado = tipoAvistamiento.Estado
                };

                output.TipoAvistamiento = tipoAvistamientoDomain;
            }

            if (avistamientoFauna.Id_Estado_Fauna != null)
            {
                var estadoFauna = await _context.Estados_Fauna.FirstOrDefaultAsync(x => x.Id_Estado_Fauna.Equals(avistamientoFauna.Id_Estado_Fauna));
                var estadoFaunaDomain = new Domain.AvistamientoFauna.Items.EstadoFauna
                {
                    IdEstadoFauna = estadoFauna.Id_Estado_Fauna,
                    Descripcion = estadoFauna.Descripcion,
                    Estado = estadoFauna.Estado
                };

                output.EstadoFauna = estadoFaunaDomain;
            }
            if (avistamientoFauna.Id_Lugar_Referencia != null)
            {
                var lugarReferencia = await _context.Lugares_Referencia_AF.FirstOrDefaultAsync(x => x.Id_Lugar_Referencia.Equals(avistamientoFauna.Id_Lugar_Referencia));
                var lugarReferenciaDomain = new Domain.AvistamientoFauna.Items.LugarReferencia
                {
                    IdLugarReferencia = lugarReferencia.Id_Lugar_Referencia,
                    Descripcion = lugarReferencia.Descripcion,
                    Latitud = lugarReferencia.Latitud,
                    Longitud = lugarReferencia.Longitud,
                    Estado = lugarReferencia.Estado
                };

                output.LugarReferencia = lugarReferenciaDomain;
            }

            if (supervisor != null)
            {
                var supervisorDomain = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = supervisor.Id_Usuario,
                    Nombres = supervisor.Nombres,
                    Apellidos = supervisor.Apellidos,
                    Email = supervisor.Email,
                    UsuarioRed = supervisor.usuario_red,
                    IdRol = supervisor.Id_Rol,
                    Estado = supervisor.Estado,
                    IdSuperior = supervisor.Id_Superior
                };
                output.Supervisor = supervisorDomain;
            }
            output.DescripcionAccion = avistamientoFauna.Descripcion_Accion;
            output.EvidenciaAccion = avistamientoFauna.Evidencia_Accion;
            output.Estado = avistamientoFauna.Estado;

            return output;
        }

        public async Task<List<Domain.AvistamientoFauna.AvistamientoFauna>> GetAvistamientosFauna(DateTime? fecha, int tipo_fauna, int estado_fauna, int lugar_referencia)
        {
            var tiposFauna = await _context.Tipos_Fauna.Where(x => x.Id_Tipo_Fauna.Equals(tipo_fauna != 0 ? tipo_fauna : x.Id_Tipo_Fauna)).ToListAsync();
            var estadosFauna = await _context.Estados_Fauna.ToListAsync();
            var lugaresReferencia = await _context.Lugares_Referencia_AF.ToListAsync();
            var nombresComunes = await _context.Nombres_Comunes_Fauna.Where(x => x.Id_Tipo_Fauna.Equals(tipo_fauna != 0 ? tipo_fauna : x.Id_Tipo_Fauna)).ToListAsync();
            var tiposAvistamiento = await _context.Tipos_Avistamiento.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var areas = await _context.Areas_Trabajo.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            var avistamientos = await _context.Avistamientos_Fauna.ToListAsync();
            avistamientos = avistamientos.Where(x =>
            !x.Estado.Equals(EstadosAvistamiento.Eliminado.ToString()) &&
            x.Fecha_Avistamiento.Date.Equals(fecha.HasValue ? fecha.Value.Date : x.Fecha_Avistamiento.Date) &&
            nombresComunes.Select(y => y.Id_Nombre_Comun_Fauna).Contains(x.Id_Nombre_Comun_Fauna.Value) &&
            x.Id_Lugar_Referencia.Equals(lugar_referencia != 0 ? lugar_referencia : x.Id_Lugar_Referencia) &&
            x.Id_Estado_Fauna.Equals(estado_fauna != 0 ? estado_fauna : x.Id_Estado_Fauna))
                .ToList();

            var output = new List<Domain.AvistamientoFauna.AvistamientoFauna>();

            foreach (var item in avistamientos)
            {
                var outputItem = new Domain.AvistamientoFauna.AvistamientoFauna();
                outputItem.IdAvistamientoFauna = item.Id_Avistamiento_Fauna;
                outputItem.Codigo = item.Codigo;
                outputItem.Fotos = item.Fotos;
                outputItem.FechaAvistamiento = item.Fecha_Avistamiento;
                outputItem.Ubicacion = item.Ubicacion;
                outputItem.Cantidad = item.Cantidad;
                outputItem.DescripcionAccion = item.Descripcion_Accion;
                outputItem.EvidenciaAccion = item.Evidencia_Accion;
                outputItem.Estado = item.Estado;

                if (item.Id_Nombre_Comun_Fauna.HasValue)
                {
                    var nombreComun = nombresComunes.FirstOrDefault(x => x.Id_Nombre_Comun_Fauna.Equals(item.Id_Nombre_Comun_Fauna));
                    var tipoFauna = tiposFauna.FirstOrDefault(x => x.Id_Tipo_Fauna.Equals(nombreComun.Id_Tipo_Fauna));
                    var nombreComunDomain = new Domain.AvistamientoFauna.Items.NombreComunFauna
                    {
                        IdNombreComunFauna = nombreComun.Id_Nombre_Comun_Fauna,
                        Descripcion = nombreComun.Descripcion,
                        Foto = nombreComun.Foto,
                        TipoFauna = new Domain.AvistamientoFauna.Items.TipoFauna
                        {
                            IdTipoFauna = tipoFauna.Id_Tipo_Fauna,
                            Descripcion = tipoFauna.Descripcion,
                            Estado = tipoFauna.Estado
                        },
                        Estado = nombreComun.Estado
                    };

                    outputItem.NombreComun = nombreComunDomain;
                }

                if (item.Id_Tipo_Avistamiento.HasValue)
                {
                    var tipoAvistamiento = tiposAvistamiento.FirstOrDefault(x => x.Id_Tipo_Avistamiento.Equals(item.Id_Tipo_Avistamiento));
                    var tipoAvistamientoDomain = new Domain.AvistamientoFauna.Items.TipoAvistamiento
                    {
                        IdTipoAvistamiento = tipoAvistamiento.Id_Tipo_Avistamiento,
                        Descripcion = tipoAvistamiento.Descripcion,
                        Estado = tipoAvistamiento.Estado
                    };

                    outputItem.TipoAvistamiento = tipoAvistamientoDomain;
                }

                if (item.Id_Estado_Fauna.HasValue)
                {
                    var estadoFauna = estadosFauna.FirstOrDefault(x => x.Id_Estado_Fauna.Equals(item.Id_Estado_Fauna));
                    var estadoFaunaDomain = new Domain.AvistamientoFauna.Items.EstadoFauna
                    {
                        IdEstadoFauna = estadoFauna.Id_Estado_Fauna,
                        Descripcion = estadoFauna.Descripcion,
                        Estado = estadoFauna.Estado
                    };

                    outputItem.EstadoFauna = estadoFaunaDomain;
                }

                if (item.Id_Lugar_Referencia.HasValue)
                {
                    var lugarReferencia = lugaresReferencia.FirstOrDefault(x => x.Id_Lugar_Referencia.Equals(item.Id_Lugar_Referencia));
                    var lugarReferenciaDomain = new Domain.AvistamientoFauna.Items.LugarReferencia
                    {
                        IdLugarReferencia = lugarReferencia.Id_Lugar_Referencia,
                        Descripcion = lugarReferencia.Descripcion,
                        Latitud = lugarReferencia.Latitud,
                        Longitud = lugarReferencia.Longitud,
                        Estado = lugarReferencia.Estado
                    };

                    outputItem.LugarReferencia = lugarReferenciaDomain;
                }

                var usuario = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Usuario));
                var empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(usuario.Id_Empresa));
                var areaTrabajo = areas.FirstOrDefault(x => x.Id_Area_Trabajo.Equals(usuario.Id_Area_Trabajo));

                var usuarioDomain = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = usuario.Id_Usuario,
                    IdRol = usuario.Id_Rol,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    UsuarioRed = usuario.usuario_red,
                    Email = usuario.Email,
                    IdSuperior = usuario.Id_Superior,
                    Estado = usuario.Estado,
                    DNI = usuario.DNI,
                    Celular = usuario.Celular,
                    Puesto = usuario.Puesto
                };
                if (empresa != null)
                    usuarioDomain.Empresa = new Domain.ValueObjects.Empresa { IdEmpresa = empresa.Id_Empresa, RazonSocial = empresa.Razon_Social, Estado = empresa.Estado };
                if (areaTrabajo != null)
                    usuarioDomain.AreaTrabajo = new Domain.ValueObjects.AreaTrabajo { IdAreaTrabajo = areaTrabajo.Id_Area_Trabajo, Descripcion = areaTrabajo.Descripcion, Estado = areaTrabajo.Estado };

                outputItem.Usuario = usuarioDomain;

                output.Add(outputItem);
            }

            return output;
        }

        public async Task<List<Domain.AvistamientoFauna.AvistamientoFauna>> GetAvistamientosFauna(int nombre_comun, int tipo_avistamiento, int estado_fauna, int lugar_referencia, DateTime fechaInicio, DateTime fechaFin)
        {
            var tiposFauna = await _context.Tipos_Fauna.ToListAsync();
            var estadosFauna = await _context.Estados_Fauna.Where(x => x.Id_Estado_Fauna.Equals(estado_fauna != 0 ? estado_fauna : x.Id_Estado_Fauna)).ToListAsync();
            var lugaresReferencia = await _context.Lugares_Referencia_AF.Where(x => x.Id_Lugar_Referencia.Equals(lugar_referencia != 0 ? lugar_referencia : x.Id_Lugar_Referencia)).ToListAsync();
            var nombresComunes = await _context.Nombres_Comunes_Fauna.Where(x => x.Id_Nombre_Comun_Fauna.Equals(nombre_comun != 0 ? nombre_comun : x.Id_Nombre_Comun_Fauna)).ToListAsync();
            var tiposAvistamiento = await _context.Tipos_Avistamiento.Where(x => x.Id_Tipo_Avistamiento.Equals(tipo_avistamiento != 0 ? tipo_avistamiento : x.Id_Tipo_Avistamiento)).ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var areas = await _context.Areas_Trabajo.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            var avistamientos = await _context.Avistamientos_Fauna.ToListAsync();

            avistamientos = avistamientos.Where(x =>
            !x.Estado.Equals(EstadosAvistamiento.Eliminado.ToString()) &&
            x.Fecha_Avistamiento >= fechaInicio &&
            x.Fecha_Avistamiento <= fechaFin)
                .ToList();

            if (nombre_comun != 0)
                avistamientos = avistamientos.Where(x =>
                x.Id_Nombre_Comun_Fauna.HasValue &&
                nombresComunes.Select(y => y.Id_Nombre_Comun_Fauna).Contains(x.Id_Nombre_Comun_Fauna.Value))
                    .ToList();

            if (tipo_avistamiento != 0)
                avistamientos = avistamientos.Where(x =>
                x.Id_Tipo_Avistamiento.HasValue &&
                tiposAvistamiento.Select(y => y.Id_Tipo_Avistamiento).Contains(x.Id_Tipo_Avistamiento.Value))
                    .ToList();

            if (estado_fauna != 0)
                avistamientos = avistamientos.Where(x =>
                x.Id_Estado_Fauna.HasValue &&
                estadosFauna.Select(y => y.Id_Estado_Fauna).Contains(x.Id_Estado_Fauna.Value))
                    .ToList();

            if (lugar_referencia != 0)
                avistamientos = avistamientos.Where(x =>
                x.Id_Lugar_Referencia.HasValue &&
                lugaresReferencia.Select(y => y.Id_Lugar_Referencia).Contains(x.Id_Lugar_Referencia.Value))
                    .ToList();

            var output = new List<Domain.AvistamientoFauna.AvistamientoFauna>();

            foreach (var item in avistamientos)
            {
                var outputItem = new Domain.AvistamientoFauna.AvistamientoFauna();
                outputItem.IdAvistamientoFauna = item.Id_Avistamiento_Fauna;
                outputItem.Codigo = item.Codigo;
                outputItem.Fotos = item.Fotos;
                outputItem.FechaAvistamiento = item.Fecha_Avistamiento;
                outputItem.Ubicacion = item.Ubicacion;
                outputItem.Cantidad = item.Cantidad;
                outputItem.DescripcionAccion = item.Descripcion_Accion;
                outputItem.EvidenciaAccion = item.Evidencia_Accion;
                outputItem.Estado = item.Estado;

                if (item.Id_Nombre_Comun_Fauna.HasValue)
                {
                    var nombreComun = nombresComunes.FirstOrDefault(x => x.Id_Nombre_Comun_Fauna.Equals(item.Id_Nombre_Comun_Fauna));
                    var tipoFauna = tiposFauna.FirstOrDefault(x => x.Id_Tipo_Fauna.Equals(nombreComun.Id_Tipo_Fauna));
                    var nombreComunDomain = new Domain.AvistamientoFauna.Items.NombreComunFauna
                    {
                        IdNombreComunFauna = nombreComun.Id_Nombre_Comun_Fauna,
                        Descripcion = nombreComun.Descripcion,
                        Foto = nombreComun.Foto,
                        TipoFauna = new Domain.AvistamientoFauna.Items.TipoFauna
                        {
                            IdTipoFauna = tipoFauna.Id_Tipo_Fauna,
                            Descripcion = tipoFauna.Descripcion,
                            Estado = tipoFauna.Estado
                        },
                        Estado = nombreComun.Estado
                    };

                    outputItem.NombreComun = nombreComunDomain;
                }

                if (item.Id_Tipo_Avistamiento.HasValue)
                {
                    var tipoAvistamiento = tiposAvistamiento.FirstOrDefault(x => x.Id_Tipo_Avistamiento.Equals(item.Id_Tipo_Avistamiento));
                    var tipoAvistamientoDomain = new Domain.AvistamientoFauna.Items.TipoAvistamiento
                    {
                        IdTipoAvistamiento = tipoAvistamiento.Id_Tipo_Avistamiento,
                        Descripcion = tipoAvistamiento.Descripcion,
                        Estado = tipoAvistamiento.Estado
                    };

                    outputItem.TipoAvistamiento = tipoAvistamientoDomain;
                }

                if (item.Id_Estado_Fauna.HasValue)
                {
                    var estadoFauna = estadosFauna.FirstOrDefault(x => x.Id_Estado_Fauna.Equals(item.Id_Estado_Fauna));
                    var estadoFaunaDomain = new Domain.AvistamientoFauna.Items.EstadoFauna
                    {
                        IdEstadoFauna = estadoFauna.Id_Estado_Fauna,
                        Descripcion = estadoFauna.Descripcion,
                        Estado = estadoFauna.Estado
                    };

                    outputItem.EstadoFauna = estadoFaunaDomain;
                }

                if (item.Id_Lugar_Referencia.HasValue)
                {
                    var lugarReferencia = lugaresReferencia.FirstOrDefault(x => x.Id_Lugar_Referencia.Equals(item.Id_Lugar_Referencia));
                    var lugarReferenciaDomain = new Domain.AvistamientoFauna.Items.LugarReferencia
                    {
                        IdLugarReferencia = lugarReferencia.Id_Lugar_Referencia,
                        Descripcion = lugarReferencia.Descripcion,
                        Latitud = lugarReferencia.Latitud,
                        Longitud = lugarReferencia.Longitud,
                        Estado = lugarReferencia.Estado
                    };

                    outputItem.LugarReferencia = lugarReferenciaDomain;
                }

                var usuario = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Usuario));
                var empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(usuario.Id_Empresa));
                var areaTrabajo = areas.FirstOrDefault(x => x.Id_Area_Trabajo.Equals(usuario.Id_Area_Trabajo));

                var usuarioDomain = new Domain.ValueObjects.Usuario
                {
                    IdUsuario = usuario.Id_Usuario,
                    IdRol = usuario.Id_Rol,
                    Nombres = usuario.Nombres,
                    Apellidos = usuario.Apellidos,
                    UsuarioRed = usuario.usuario_red,
                    Email = usuario.Email,
                    IdSuperior = usuario.Id_Superior,
                    Estado = usuario.Estado,
                    DNI = usuario.DNI,
                    Celular = usuario.Celular,
                    Puesto = usuario.Puesto
                };
                if (empresa != null)
                    usuarioDomain.Empresa = new Domain.ValueObjects.Empresa { IdEmpresa = empresa.Id_Empresa, RazonSocial = empresa.Razon_Social, Estado = empresa.Estado };
                if (areaTrabajo != null)
                    usuarioDomain.AreaTrabajo = new Domain.ValueObjects.AreaTrabajo { IdAreaTrabajo = areaTrabajo.Id_Area_Trabajo, Descripcion = areaTrabajo.Descripcion, Estado = areaTrabajo.Estado };

                outputItem.Usuario = usuarioDomain;

                output.Add(outputItem);
            }
            return output;
        }
    }
}
