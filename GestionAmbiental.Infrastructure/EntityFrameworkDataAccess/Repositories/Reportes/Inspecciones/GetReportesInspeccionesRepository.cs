using GestionAmbiental.Application.Repositories.Reportes;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using GestionAmbiental.Domain.Reportes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Reportes.Inspecciones
{
    public class GetReportesInspeccionesRepository : IGetReportesInspeccionesReadOnlyRepository
    {
        private Context _context;

        public GetReportesInspeccionesRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Dimension> GetObservacionesMAP()
        {
            var data = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("MAP")).CountAsync();
            var output = new Dimension();
            output.Nombre = "MAP";
            output.Valor = data;
            return output;
        }
        public async Task<Dimension> GetObservacionesSSO()
        {
            var data = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("SSO")).CountAsync();
            var output = new Dimension();
            output.Nombre = "SSO";
            output.Valor = data;
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesXUsuario()
        {
            var cabeceras_registradas = await _context.Inspecciones_Cabecera.Where(x => x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).ToListAsync();
            var observaciones = await _context.Inspecciones_Detalle.Where(x => cabeceras_registradas.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) 
            && x.Estado!=EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            var output = new List<Dimension>();
            foreach (var item in usuarios)
            {
                var nObservaciones = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(item.Id_Usuario)).Count();
                if (nObservaciones > 0)
                    output.Add(new Dimension(item.Nombres + " " + item.Apellidos, nObservaciones));
            }
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesXEstado()
        {
            var cabeceras_registradas = await _context.Inspecciones_Cabecera.Where(x => x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).ToListAsync();
            var inspecciones = await _context.Inspecciones_Detalle.Where(x => cabeceras_registradas.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) 
            && x.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();

            var output = new List<Dimension>();

            foreach (var item in Enum.GetNames(typeof(EstadosInspeccion)))
            {
                output.Add(new Dimension(item, inspecciones.Where(x => x.Estado.Equals(item)).Count()));
            }

            return output;
        }
        public async Task<List<ObservacionesUsuario>> GetResumenObservacionesXUsuario(string filtro)
        {
            var output = new List<ObservacionesUsuario>();

            var cabeceras_registradas = await _context.Inspecciones_Cabecera.Where(x => x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).ToListAsync();
            var observaciones = await _context.Inspecciones_Detalle.Where(x => cabeceras_registradas.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) 
            && x.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();

            foreach (var usuario in usuarios)
            {
                var detalle = new ObservacionesUsuario();
                detalle.usuario = new Domain.ValueObjects.Usuario(usuario.Id_Usuario, usuario.Id_Rol, usuario.Nombres, usuario.Apellidos, usuario.Email);
                detalle.pendientes = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(usuario.Id_Usuario) && x.Estado.Equals(EstadosInspeccion.Registrado.ToString())).Count();
                detalle.implementadas = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(usuario.Id_Usuario) && x.Estado.Equals(EstadosInspeccion.Implementado.ToString())).Count();
                detalle.aprobadas = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(usuario.Id_Usuario) && x.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).Count();
                detalle.rechazadas = observaciones.Where(x => x.Id_Responsable_Marcobre.Equals(usuario.Id_Usuario) && x.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).Count();

                output.Add(detalle);
            }

            return output;
        }
        public async Task<List<Dimension>> GetInspeccionesDiarias()
        {
            var output = new List<Dimension>();
            var ndias = 30;
            var inspecciones = await _context.Inspecciones_Cabecera
                .Where(x => x.Fecha_Inspeccion > DateTime.Now.AddDays(-ndias) && x.Estado.Equals(EstadosInspeccion.Registrado.ToString()))
                .OrderBy(x => x.Fecha_Inspeccion)
                .ToListAsync();

            var inspecciones_diarias = inspecciones.Select(x => x.Fecha_Inspeccion).GroupBy(y => y.Date).ToList();

            var list_dias = new List<Dimension>();
            var list_inspecciones = new List<Dimension>();
            for (int i = ndias; i >= 0; i--)
            {
                list_dias.Add(new Dimension(DateTime.Now.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, i));
            }
            foreach (var item in inspecciones_diarias)
            {
                list_inspecciones.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Count(), (int)(DateTime.Now - item.Key).TotalDays));
            }
            var distinct = list_dias.Where(x => !list_inspecciones.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();

            output = distinct.Union(list_inspecciones).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();

            //var union = distinct.Union(output).OrderBy(x=>x.Nombre).ToList();
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesNivelRiesgoMAP()
        {
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("MAP") && x.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var id_inspecciones = await _context.Inspecciones_Cabecera.Where(x =>
            x.Estado.Equals(EstadosInspeccion.Registrado.ToString())
            && observaciones.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera))
                .Select(x => x.Id_Inspeccion_Cabecera)
                .ToListAsync();
            var map = observaciones.Where(x => id_inspecciones.Contains(x.Id_Inspeccion_Cabecera)).GroupBy(x => x.Nivel_Riesgo).ToList();

            var output = new List<Dimension>();
            if (map.Count == 3)
                foreach (var item in map)
                {
                    switch (item.Key)
                    {
                        case "Tolerable": output.Add(new Dimension(item.Key, item.Count(), 0)); break;
                        case "Alerta": output.Add(new Dimension(item.Key, item.Count(), 1)); break;
                        case "No Aceptable": output.Add(new Dimension(item.Key, item.Count(), 2)); break;
                    }
                }
            else
            {
                output.Add(new Dimension("Tolerable", 0, 0));
                output.Add(new Dimension("Alerta", 0, 1));
                output.Add(new Dimension("No Aceptable", 0, 2));
                foreach (var item in map)
                {
                    switch (item.Key)
                    {
                        case "Tolerable": output.FirstOrDefault(x => x.Nombre.Equals("Tolerable")).Valor = item.Count(); break;
                        case "Alerta": output.FirstOrDefault(x => x.Nombre.Equals("Alerta")).Valor = item.Count(); break;
                        case "No Aceptable": output.FirstOrDefault(x => x.Nombre.Equals("No Aceptable")).Valor = item.Count(); break;
                    }
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesNivelRiesgoSSO()
        {
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("SSO") && x.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var id_inspecciones = await _context.Inspecciones_Cabecera.Where(x =>
            x.Estado.Equals(EstadosInspeccion.Registrado.ToString())
            && observaciones.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera))
                .Select(x => x.Id_Inspeccion_Cabecera)
                .ToListAsync();

            var sso = observaciones.Where(x => id_inspecciones.Contains(x.Id_Inspeccion_Cabecera)).GroupBy(x => x.Nivel_Riesgo).ToList();

            var output = new List<Dimension>();
            if (sso.Count == 3)
                foreach (var item in sso)
                {
                    switch (item.Key)
                    {
                        case "Bajo": output.Add(new Dimension(item.Key, item.Count(), 3)); break;
                        case "Medio": output.Add(new Dimension(item.Key, item.Count(), 4)); break;
                        case "Alto": output.Add(new Dimension(item.Key, item.Count(), 5)); break;
                    }
                }
            else
            {
                output.Add(new Dimension("Bajo", 0, 3));
                output.Add(new Dimension("Medio", 0, 4));
                output.Add(new Dimension("Alto", 0, 5));
                foreach (var item in sso)
                {
                    switch (item.Key)
                    {
                        case "Bajo": output.FirstOrDefault(x => x.Nombre.Equals("Bajo")).Valor = item.Count(); break;
                        case "Medio": output.FirstOrDefault(x => x.Nombre.Equals("Medio")).Valor = item.Count(); break;
                        case "Alto": output.FirstOrDefault(x => x.Nombre.Equals("Alto")).Valor = item.Count(); break;
                    }
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesTipoInspeccionMAP()
        {
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("MAP") && x.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var id_inspecciones = await _context.Inspecciones_Cabecera.Where(x =>
            x.Estado.Equals(EstadosInspeccion.Registrado.ToString())
            && observaciones.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera))
                .Select(x => x.Id_Inspeccion_Cabecera)
                .ToListAsync();

            var map = observaciones.Where(x => id_inspecciones.Contains(x.Id_Inspeccion_Cabecera)).GroupBy(x => x.Descripcion_Condicion).ToList();
            var output = new List<Dimension>();
            if (map.Count == 2)
                foreach (var item in map)
                {
                    switch (item.Key)
                    {
                        case "Condición Subestándar": output.Add(new Dimension(item.Key, item.Count(), 1)); break;
                        case "Acto Subestándar": output.Add(new Dimension(item.Key, item.Count(), 2)); break;
                    }
                }
            else
            {
                output.Add(new Dimension("Condición Subestándar", 0, 1));
                output.Add(new Dimension("Acto Subestándar", 0, 2));
                foreach (var item in map)
                {
                    switch (item.Key)
                    {
                        case "Condición Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Condición Subestándar")).Valor = item.Count(); break;
                        case "Acto Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Acto Subestándar")).Valor = item.Count(); break;
                    }
                }
            }
            return output;
        }
        public async Task<List<Dimension>> GetObservacionesTipoInspeccionSSO()
        {
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("SSO") && x.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var id_inspecciones = await _context.Inspecciones_Cabecera.Where(x =>
            x.Estado.Equals(EstadosInspeccion.Registrado.ToString())
            && observaciones.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera))
                .Select(x => x.Id_Inspeccion_Cabecera)
                .ToListAsync();

            var sso = observaciones.Where(x => id_inspecciones.Contains(x.Id_Inspeccion_Cabecera)).GroupBy(x => x.Descripcion_Condicion).ToList();
            var output = new List<Dimension>();
            if (sso.Count == 2)
                foreach (var item in sso)
                {
                    switch (item.Key)
                    {
                        case "Condición Subestándar": output.Add(new Dimension(item.Key, item.Count(), 1)); break;
                        case "Acto Subestándar": output.Add(new Dimension(item.Key, item.Count(), 2)); break;
                    }
                }
            else
            {
                output.Add(new Dimension("Condición Subestándar", 0, 1));
                output.Add(new Dimension("Acto Subestándar", 0, 2));
                foreach (var item in sso)
                {
                    switch (item.Key)
                    {
                        case "Condición Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Condición Subestándar")).Valor = item.Count(); break;
                        case "Acto Subestándar": output.FirstOrDefault(x => x.Nombre.Equals("Acto Subestándar")).Valor = item.Count(); break;
                    }
                }
            }
            return output;
        }
        public async Task<List<DetalleObservacionXEstado>> GetDetalleObservacionXEstados(EstadosInspeccion estado)
        {
            var output = new List<DetalleObservacionXEstado>();
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Estado.Equals(estado.ToString()) && x.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();
            var inspecciones = await _context.Inspecciones_Cabecera.Where(x =>
            observaciones.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)
            && x.Estado.Equals(EstadosInspeccion.Registrado.ToString()))
                .ToListAsync();
            observaciones = observaciones.Where(x => inspecciones.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera)).ToList();
            var usuarios = await _context.Usuarios.ToListAsync();
            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();

            foreach (var observacion in observaciones)
            {
                var item = new DetalleObservacionXEstado();
                var asesor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(observacion.Id_Asesor));
                var responsable_marcobre = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(observacion.Id_Responsable_Marcobre));
                var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(observacion.Id_Tipo_Condicion));

                item.IdInspeccionCabecera = observacion.Id_Inspeccion_Cabecera;
                item.Codigo = inspecciones.FirstOrDefault(x => x.Id_Inspeccion_Cabecera.Equals(observacion.Id_Inspeccion_Cabecera)).Codigo;

                item.TipoObservacion = observacion.Tipo_Observacion;
                item.Asesor = asesor.Nombres.ToString() + " " + asesor.Apellidos.ToString();
                item.DescripcionCondicion = observacion.Descripcion_Condicion;
                item.TipoCondicion = tipo_condicion.Descripcion;
                item.NivelRiesgo = observacion.Nivel_Riesgo;
                item.DescripcionAccion = observacion.Descripcion_Accion;
                item.ResponsableMarcobre = responsable_marcobre.Nombres + " " + responsable_marcobre.Apellidos;
                item.ResponsableContratista = observacion.Responsable_Contratista;
                item.FechaProgramada = observacion.Fecha_Programada;

                output.Add(item);
            }
            return output;
        }
        public async Task<List<List<Dimension>>> GetObservacionesMAPXTipoCondicion()
        {
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("MAP") && x.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();

            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();

            var agrupado = observaciones.GroupBy(x => x.Nivel_Riesgo).ToList();

            var output = new List<List<Dimension>>();

            var tolerables = new List<Dimension>();
            var alerta = new List<Dimension>();
            var no_aceptable = new List<Dimension>();

            //tolerables
            if (agrupado.FirstOrDefault(x => x.Key.Equals("Tolerable")) != null)
                foreach (var item in agrupado.FirstOrDefault(x => x.Key.Equals("Tolerable")).GroupBy(y => y.Id_Tipo_Condicion))
                {
                    tolerables.Add(new Dimension(tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Key)).Descripcion, item.Count()));
                }
            //alerta
            if (agrupado.FirstOrDefault(x => x.Key.Equals("Alerta")) != null)
                foreach (var item in agrupado.FirstOrDefault(x => x.Key.Equals("Alerta")).GroupBy(y => y.Id_Tipo_Condicion))
                {
                    alerta.Add(new Dimension(tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Key)).Descripcion, item.Count()));
                }
            //no aceptable
            if (agrupado.FirstOrDefault(x => x.Key.Equals("No Aceptable")) != null)
                foreach (var item in agrupado.FirstOrDefault(x => x.Key.Equals("No Aceptable")).GroupBy(y => y.Id_Tipo_Condicion))
                {
                    if (agrupado.Count != 0)
                        no_aceptable.Add(new Dimension(tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Key)).Descripcion, item.Count()));
                }
            output.Add(tolerables);
            output.Add(alerta);
            output.Add(no_aceptable);

            return output;

        }
        public async Task<List<List<Dimension>>> GetObservacionesSSOXTipoCondicion()
        {
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Tipo_Observacion.Equals("SSO") && x.Estado != EstadosInspeccion.Eliminado.ToString()).ToListAsync();

            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();

            var agrupado = observaciones.GroupBy(x => x.Nivel_Riesgo).ToList();

            var output = new List<List<Dimension>>();

            var bajo = new List<Dimension>();
            var medio = new List<Dimension>();
            var alto = new List<Dimension>();

            //bajo
            if (agrupado.FirstOrDefault(x => x.Key.Equals("Bajo")) != null)
                foreach (var item in agrupado.FirstOrDefault(x => x.Key.Equals("Bajo")).GroupBy(y => y.Id_Tipo_Condicion).ToList())
                {
                    bajo.Add(new Dimension(tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Key)).Descripcion, item.Count()));
                }
            //medio
            if (agrupado.FirstOrDefault(x => x.Key.Equals("Medio")) != null)
                foreach (var item in agrupado.FirstOrDefault(x => x.Key.Equals("Medio")).GroupBy(y => y.Id_Tipo_Condicion).ToList())
                {
                    medio.Add(new Dimension(tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Key)).Descripcion, item.Count()));
                }
            //alto
            if (agrupado.FirstOrDefault(x => x.Key.Equals("Alto")) != null)
                foreach (var item in agrupado.FirstOrDefault(x => x.Key.Equals("Alto")).GroupBy(y => y.Id_Tipo_Condicion).ToList())
                {
                    alto.Add(new Dimension(tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Key)).Descripcion, item.Count()));
                }
            output.Add(bajo);
            output.Add(medio);
            output.Add(alto);

            return output;

        }
        /*
        public async Task<List<Dimension>> GetObservacionesXEstado(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()))
                .ToListAsync();

            var observaciones = await _context.Inspecciones_Detalle.Where(x =>
            cabeceras.Select(y=>y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
            x.Id_Responsable_Marcobre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Marcobre) &&
            x.Fecha_Programada >= FechaInicio &&
            x.Fecha_Programada <= FechaFin &&
            !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()))
                .ToListAsync();

            var output = new List<Dimension>();

            foreach (var item in Enum.GetNames(typeof(EstadosRAC)))
            {
                output.Add(new Dimension(item, observaciones.Where(x => x.Estado.Equals(item)).Count()));
            }
            var totalRAC = await _context.RACs.CountAsync();
            output.Add(new Dimension("total", totalRAC));
            return output;
        }

        public async Task<List<DetalleObservacionXEstado>> GetDetalleReportesXEstado(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin, EstadosRAC estado)
        {
            var output = new List<DetalleObservacionXEstado>();
            var cabeceras = await _context.Inspecciones_Cabecera.Where(x =>
               x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
               x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
               !x.Estado.Equals(EstadosInspeccion.Eliminado.ToString()))
               .ToListAsync();

            var observaciones = await _context.Inspecciones_Detalle.Where(x =>
            cabeceras.Select(y=>y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
            x.Id_Responsable_Marcobre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Marcobre) &&
            x.Fecha_Programada >= FechaInicio &&
            x.Fecha_Programada <= FechaFin &&
            !x.Estado.Equals(estado.ToString()))
                .ToListAsync();

            var usuarios = await _context.Usuarios.ToListAsync();
            var tipos_condicion = await _context.Tipos_Condicion.ToListAsync();

            foreach (var item in observaciones)
            {
                var asesor = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Asesor));
                var responsableMarcobre = usuarios.FirstOrDefault(x => x.Id_Usuario.Equals(item.Id_Responsable_Marcobre));
                var tipo_condicion = tipos_condicion.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(item.Id_Tipo_Condicion));
                
                output.Add(new DetalleObservacionXEstado
                {
                    IdInspeccionCabecera = item.Id_Inspeccion_Cabecera,
                    Codigo = cabeceras.FirstOrDefault(x => x.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Codigo,
                    IdInspeccionDetalle = item.Id_Inspeccion_Detalle,
                    Asesor = asesor.Nombres + " "+asesor.Apellidos,
                    DescripcionCondicion = item.Descripcion_Condicion,
                    TipoObservacion = item.Tipo_Observacion,
                    TipoCondicion = tipo_condicion.Descripcion,
                    NivelRiesgo = item.Nivel_Riesgo,
                    DescripcionAccion = item.Descripcion_Accion,
                    ResponsableMarcobre = responsableMarcobre.Nombres + " "+responsableMarcobre.Apellidos,
                    FechaProgramada = item.Fecha_Programada,
                    ResponsableContratista = item.Responsable_Contratista,
                    Estado = item.Estado
                }) ;
            }

            return output;
        }

        public async Task<List<Dimension>> GetInspeccionesXFecha(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            var output = new List<Dimension>();

            var observaciones = await _context.Inspecciones_Detalle.Where(x =>
            cabeceras.Select(y => y.Id_Inspeccion_Cabecera).Contains(x.Id_Inspeccion_Cabecera) &&
            x.Id_Responsable_Marcobre.Equals(IdUsuario > 0 ? IdUsuario : x.Id_Responsable_Marcobre) &&
            x.Fecha_Programada >= FechaInicio &&
            x.Fecha_Programada <= FechaFin &&
            !x.Estado.Equals(estado.ToString()))
                .ToListAsync();

            var cabeceras = await _context.Inspecciones_Cabecera
                .Where(x =>
                x.Id_Gerencia.Equals(IdGerencia > 0 ? IdGerencia : x.Id_Gerencia) &&
                x.Id_Empresa.Equals(IdEmpresa > 0 ? IdEmpresa : x.Id_Empresa) &&
                x.Fecha >= FechaInicio &&
                x.Fecha <= FechaFin)
                .ToListAsync();

            var nReportesxFecha = reportes.Select(x => x.Fecha).GroupBy(y => y.Date).ToList();

            int nDias = 0;
            //Calcular n días entre fechas de distinto año (considerando años biciestos)
            if (FechaFin.Year > FechaInicio.Year)
                nDias = FechaFin.DayOfYear + (FechaInicio.Year % 4 == 0 ? 366 : 365) - FechaInicio.DayOfYear;
            else
                nDias = FechaFin.DayOfYear - FechaInicio.DayOfYear;
            var lstDiasSinValor = new List<Dimension>();
            var lstDiasConValor = new List<Dimension>();

            for (int i = nDias; i >= 0; i--)
            {
                lstDiasSinValor.Add(new Dimension(FechaFin.AddDays(-i).ToString("dd MMMM").Substring(0, 6), 0, i));
            }

            foreach (var item in nReportesxFecha)
            {
                lstDiasConValor.Add(new Dimension(item.Key.ToString("dd MMMM").Substring(0, 6), item.Count(), FechaFin.DayOfYear - item.Key.DayOfYear));
            }
            var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
            //var distinct = lstDiasSinValor.Where(x => !lstDiasConValor.Select(y => y.Nombre).ToList().Contains(x.Nombre)).ToList();
            output = distinct.Union(lstDiasConValor).Where(x => x.Index >= 0).OrderByDescending(x => x.Index).ToList();

            //var union = distinct.Union(output).OrderBy(x=>x.Nombre).ToList();
            return output;
        }

        public Task<List<Dimension>> GetObservacionesNivelRiesgoMA(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            throw new NotImplementedException();
        }

        public Task<List<Dimension>> GetObservacionesNivelRiesgoSSO(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            throw new NotImplementedException();
        }

        public Task<List<Dimension>> GetObservacionesClasificacionMA(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            throw new NotImplementedException();
        }

        public Task<List<Dimension>> GetObservacionesClasificacionSSO(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            throw new NotImplementedException();
        }

        public Task<List<List<Dimension>>> GetObservacionesTipoCondicionxNivelRiesgoMA(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            throw new NotImplementedException();
        }

        public Task<List<List<Dimension>>> GetRObservacionesTipoCondicionxNivelRiesgoSSO(int IdGerencia, int IdEmpresa, int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            throw new NotImplementedException();
        }*/
    }
}
