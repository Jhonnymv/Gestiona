using GestionAmbiental.Application.Repositories.Inspecciones;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Inspecciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Inspecciones
{
    public class CabeceraInspeccionRepository : ICabeceraInspeccionReadOnlyRepository
    {
        private readonly Context _context;

        public CabeceraInspeccionRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<InspeccionCabecera>> GetInspeccionCabecera(DateTime fecha, int gerencia, string codigo)
        {
            var fechaFiltro = (fecha == DateTime.MinValue) ? (DateTime)SqlDateTime.MinValue : fecha;
            var cabeceras = await _context.Inspecciones_Cabecera.Where(c =>
                c.Fecha_Inspeccion.Date.Equals(fechaFiltro != (DateTime)SqlDateTime.MinValue ? fechaFiltro.Date : c.Fecha_Inspeccion.Date)
                && c.Id_Gerencia.Equals(gerencia != 0 ? gerencia : c.Id_Gerencia)
                && c.Codigo.Contains(codigo != "" ? codigo : c.Codigo))
                .OrderByDescending(x=>x.Id_Inspeccion_Cabecera)
                .ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var detalles_cabecera = await _context.Inspecciones_Detalle.Where(e => cabeceras.Select(c => c.Id_Inspeccion_Cabecera).Contains(e.Id_Inspeccion_Cabecera)).ToListAsync();

            var TablaResumen = new List<InspeccionCabecera>();
            foreach (var item in cabeceras)
            {
                var gerEjectMar = gerencias.Where(g => g.Id_Gerencia.Equals(item.Id_Gerencia)).FirstOrDefault();
                var empContMar = empresas.Where(e => e.Id_Empresa.Equals(item.Id_Empresa)).FirstOrDefault();
                var nObservaciones = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Count();
                var nObsImplementadas = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera) && d.Estado.Equals(EstadosInspeccion.Implementado.ToString())).Count();
                var nObsAprobadas = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera) && d.Estado.Equals(EstadosInspeccion.Aprobado.ToString())).Count();
                var nObsRechazadas = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera) && d.Estado.Equals(EstadosInspeccion.Rechazado.ToString())).Count();

                var cabecera = new InspeccionCabecera();
                cabecera.IdCabecera = item.Id_Inspeccion_Cabecera;
                cabecera.Codigo = item.Codigo;
                cabecera.Gerencia = gerEjectMar.Descripcion;
                cabecera.Empresa = empContMar.Razon_Social;
                cabecera.AreaInspeccionada = item.Area_Inspeccionada;
                cabecera.FechaInspeccion = item.Fecha_Inspeccion;
                cabecera.NObservaciones = nObservaciones;
                cabecera.Estado = item.Estado;
                cabecera.NObsImplementadas = nObsImplementadas;
                cabecera.NObsAprobadas = nObsAprobadas;
                cabecera.NObsRechazadas = nObsRechazadas;
                cabecera.TipoInspeccion = item.Tipo_Inspeccion;
                TablaResumen.Add(cabecera);

                /*TablaResumen.Add(new InspeccionCabecera(item.Id_Inspeccion_Cabecera, item.Codigo, gerEjectMar.Descripcion,
                    empContMar.Razon_Social, gerEjectEPCM.Descripcion, empContEPCM.Razon_Social,
                    item.Lugar_Inspeccionado, item.Fecha_Inspeccion, nObservaciones, item.Estado));*/
            }

            return TablaResumen;
        }

        public async Task<InspeccionCabecera> GetInspeccionCabecera(int idCabecera)
        {
            var cabeccera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)).ToListAsync();
            var output = new InspeccionCabecera();
            output.IdCabecera = cabeccera.Id_Inspeccion_Cabecera;
            output.IdRegistradoPor = cabeccera.Id_Registrado_Por;
            output.Codigo = cabeccera.Codigo;
            output.IdGerencia = cabeccera.Id_Gerencia;
            output.IdEmpresa = cabeccera.Id_Empresa;
            output.TipoInspeccion = cabeccera.Tipo_Inspeccion;
            output.AreaInspeccionada = cabeccera.Area_Inspeccionada;
            output.FechaInspeccion = cabeccera.Fecha_Inspeccion;
            output.NObservaciones = observaciones.Count;
            output.Estado = cabeccera.Estado;
            output.IdResponsableArea = cabeccera.Id_Responsable_Area;
            output.Objetivo = cabeccera.Objetivo;
            output.ResponsableInspeccion = cabeccera.Responsable_Inspeccion;
            output.Participantes = cabeccera.Participantes;

            return output;
        }

        public async Task<List<InspeccionCabecera>> GetInspeccionCabeceraImplementacion(DateTime fecha, int gerencia, string codigo,int id_usuario)
        {
            //var observaciones_usuario = await _context.Inspecciones_Detalle.Where(o=>o.Id_Responsable_Marcobre.Equals(id_usuario)).ToListAsync();

            var fechaFiltro = (fecha == DateTime.MinValue) ? (DateTime)SqlDateTime.MinValue : fecha;
            var cabeceras = await _context.Inspecciones_Cabecera.Where(c =>
            //    observaciones_usuario.Select(x=>x.Id_Inspeccion_Cabecera).Contains(c.Id_Inspeccion_Cabecera) &&
                c.Fecha_Inspeccion.Date.Equals(fechaFiltro != (DateTime)SqlDateTime.MinValue ? fechaFiltro.Date : c.Fecha_Inspeccion.Date)
                && c.Id_Gerencia.Equals(gerencia != 0 ? gerencia : c.Id_Gerencia)
                && c.Codigo.Contains(codigo != "" ? codigo : c.Codigo)
                && c.Estado.Equals(EstadosInspeccion.Registrado.ToString())
                )
                .OrderByDescending(x => x.Id_Inspeccion_Cabecera)
                .ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var detalles_cabecera = await _context.Inspecciones_Detalle.Where(e => cabeceras.Select(c => c.Id_Inspeccion_Cabecera).Contains(e.Id_Inspeccion_Cabecera)).ToListAsync();

            var TablaResumen = new List<InspeccionCabecera>();
            foreach (var item in cabeceras)
            {
                var gerEjectMar = gerencias.Where(g => g.Id_Gerencia.Equals(item.Id_Gerencia)).FirstOrDefault();
                var empContMar = empresas.Where(e => e.Id_Empresa.Equals(item.Id_Empresa)).FirstOrDefault();
                var nObservaciones = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Count();

                TablaResumen.Add(new InspeccionCabecera(item.Id_Inspeccion_Cabecera, item.Codigo, gerEjectMar.Descripcion,
                    empContMar.Razon_Social, item.Area_Inspeccionada, item.Fecha_Inspeccion, nObservaciones, item.Estado));
            }
            return TablaResumen;
        }
        public async Task<InspeccionCabecera> GetInspeccionCabeceraImplementacion(int idCabecera)
        {
            var cabeccera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)).ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var usuarios = await _context.Usuarios.ToListAsync();
            var usuarios_domain = new List<Domain.ValueObjects.Usuario>();
            foreach(var item in usuarios)
            {
                usuarios_domain.Add(new Domain.ValueObjects.Usuario(item.Id_Usuario,item.Id_Rol,item.Nombres,item.Apellidos,item.Email));
            }
            var output = new InspeccionCabecera();
            output.IdCabecera = cabeccera.Id_Inspeccion_Cabecera;
            output.Codigo = cabeccera.Codigo;
            output.Objetivo = cabeccera.Objetivo;
            output.Gerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabeccera.Id_Gerencia)).Descripcion;
            output.Empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(cabeccera.Id_Empresa)).Razon_Social;
            output.AreaInspeccionada = cabeccera.Area_Inspeccionada;
            output.ResponsableArea = usuarios_domain.FirstOrDefault(x => x.IdUsuario.Equals(cabeccera.Id_Responsable_Area));
            output.FechaInspeccion = cabeccera.Fecha_Inspeccion;
            output.NObservaciones = observaciones.Count;
            output.Participantes = cabeccera.Participantes;
            output.Estado = cabeccera.Estado;
            output.TipoInspeccion = cabeccera.Tipo_Inspeccion;
            output.ResponsableInspeccion = cabeccera.Responsable_Inspeccion;
            return output;
        }

        public async Task<List<InspeccionCabecera>> GetInspeccionCabeceraVerificacion(DateTime fecha, int gerencia, string codigo)
        {
            var fechaFiltro = (fecha == DateTime.MinValue) ? (DateTime)SqlDateTime.MinValue : fecha;
            var cabeceras = await _context.Inspecciones_Cabecera.Where(c =>
                c.Estado.Equals(EstadosInspeccion.Registrado.ToString()) &&
                c.Fecha_Inspeccion.Date.Equals(fechaFiltro != (DateTime)SqlDateTime.MinValue ? fechaFiltro.Date : c.Fecha_Inspeccion.Date)
                && c.Id_Gerencia.Equals(gerencia != 0 ? gerencia : c.Id_Gerencia)
                && c.Codigo.Contains(codigo != "" ? codigo : c.Codigo)
                )
                .OrderByDescending(x => x.Id_Inspeccion_Cabecera)
                .ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();
            var detalles_cabecera = await _context.Inspecciones_Detalle.Where(e => cabeceras.Select(c => c.Id_Inspeccion_Cabecera).Contains(e.Id_Inspeccion_Cabecera)).ToListAsync();

            var TablaResumen = new List<InspeccionCabecera>();
            foreach (var item in cabeceras)
            {
                var gerEjectMar = gerencias.Where(g => g.Id_Gerencia.Equals(item.Id_Gerencia)).FirstOrDefault();
                var empContMar = empresas.Where(e => e.Id_Empresa.Equals(item.Id_Empresa)).FirstOrDefault();
                var nObservaciones = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Count();

                TablaResumen.Add(new InspeccionCabecera(item.Id_Inspeccion_Cabecera, item.Codigo, gerEjectMar.Descripcion,
                    empContMar.Razon_Social, item.Area_Inspeccionada, item.Fecha_Inspeccion, nObservaciones, item.Estado));
            }

            return TablaResumen;
        }

        public async Task<InspeccionCabecera> GetInspeccionCabeceraVerificacion(int idCabecera)
        {
            var cabeccera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)).ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();

            var usuarios = await _context.Usuarios.ToListAsync();
            var usuarios_domain = new List<Domain.ValueObjects.Usuario>();
            foreach (var item in usuarios)
            {
                usuarios_domain.Add(new Domain.ValueObjects.Usuario(item.Id_Usuario, item.Id_Rol, item.Nombres, item.Apellidos, item.Email));
            }

            var output = new InspeccionCabecera();
            output.IdCabecera = cabeccera.Id_Inspeccion_Cabecera;
            output.Codigo = cabeccera.Codigo;
            output.Objetivo = cabeccera.Objetivo;
            output.Gerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabeccera.Id_Gerencia)).Descripcion;
            output.Empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(cabeccera.Id_Empresa)).Razon_Social;
            output.AreaInspeccionada = cabeccera.Area_Inspeccionada;
            output.ResponsableArea = usuarios_domain.FirstOrDefault(x => x.IdUsuario.Equals(cabeccera.Id_Responsable_Area));
            output.FechaInspeccion = cabeccera.Fecha_Inspeccion;
            output.NObservaciones = observaciones.Count;
            output.Participantes = cabeccera.Participantes;
            output.Estado = cabeccera.Estado;
            output.TipoInspeccion = cabeccera.Tipo_Inspeccion;
            output.ResponsableInspeccion = cabeccera.Responsable_Inspeccion;
            return output;
        }
        public async Task<InspeccionCabecera> GetInspeccionCabeceraDescarga(int idCabecera)
        {
            //Cabecera
            var cabecera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)).ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas.ToListAsync();

            var usuarios = await _context.Usuarios.ToListAsync();
            var usuarios_domain = new List<Domain.ValueObjects.Usuario>();
            foreach (var item in usuarios)
            {
                usuarios_domain.Add(new Domain.ValueObjects.Usuario(item.Id_Usuario, item.Id_Rol, item.Nombres, item.Apellidos, item.Email));
            }
            //Detalle
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();
            var tiposCondicion_domain = new List<Domain.ValueObjects.TipoCondicion>();
            foreach (var item in tiposCondicion)
            {
                tiposCondicion_domain.Add(new Domain.ValueObjects.TipoCondicion(item.Id_Tipo_Condicion, item.Descripcion,item.Grupo));
            }


            var output = new InspeccionCabecera();
            output.IdCabecera = cabecera.Id_Inspeccion_Cabecera;
            output.Codigo = cabecera.Codigo;
            output.Objetivo = cabecera.Objetivo;
            output.Gerencia = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabecera.Id_Gerencia)).Descripcion;
            output.Empresa = empresas.FirstOrDefault(x => x.Id_Empresa.Equals(cabecera.Id_Empresa)).Razon_Social;
            output.AreaInspeccionada = cabecera.Area_Inspeccionada;
            output.ResponsableArea = usuarios_domain.FirstOrDefault(x => x.IdUsuario.Equals(cabecera.Id_Responsable_Area));
            output.FechaInspeccion = cabecera.Fecha_Inspeccion;
            output.Participantes = cabecera.Participantes;
            output.Estado = cabecera.Estado;
            output.TipoInspeccion = cabecera.Tipo_Inspeccion;
            output.ResponsableInspeccion = cabecera.Responsable_Inspeccion;
            

            output.Observaciones = new List<InspeccionDetalle>();
            foreach(var observacion in observaciones)
            {
                var obs = new InspeccionDetalle();
                obs.IdInspeccionCabecera = idCabecera;
                obs.IdInspeccionDetalle = observacion.Id_Inspeccion_Detalle;
                obs.TipoObservacion = observacion.Tipo_Observacion;
                obs.DescripcionCondicion = observacion.Descripcion_Condicion;
                obs.TipoCondicion = tiposCondicion_domain.FirstOrDefault(x => x.IdTipoCondicion.Equals(observacion.Id_Tipo_Condicion));
                obs.NivelRiesgo = observacion.Nivel_Riesgo;
                obs.DescripcionAccion = observacion.Descripcion_Accion;
                obs.ResponsableMarcobre = usuarios_domain.FirstOrDefault(x => x.IdUsuario.Equals(observacion.Id_Responsable_Marcobre));
                obs.ResponsableContratista = observacion.Responsable_Contratista;
                obs.FechaProgramada = observacion.Fecha_Programada;
                obs.ComentarioImplementacion = observacion.Comentario_Implementacion;
                obs.FechaCierre = observacion.Fecha_Cierre;
                obs.ComentarioVerificacion = observacion.Comentario_Verificacion;
                obs.FechaVerificacion = observacion.Fecha_Verificacion;
                obs.Estado = observacion.Estado;
                obs.EvidenciaImplementacion = observacion.Evidencia_Implementacion;
                obs.EvidenciaCondicion = observacion.Evidencia_Condicion;
                output.Observaciones.Add(obs);
            }

            return output;
        }
    }
}
