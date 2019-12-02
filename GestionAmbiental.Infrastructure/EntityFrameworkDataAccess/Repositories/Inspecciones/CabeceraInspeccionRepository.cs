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
                && c.Id_Gerencia_Ejecutora.Equals(gerencia != 0 ? gerencia : c.Id_Gerencia_Ejecutora)
                && c.Codigo.Contains(codigo != "" ? codigo : c.Codigo))
                .OrderByDescending(x=>x.Id_Inspeccion_Cabecera)
                .ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas_Contratistas.ToListAsync();
            var detalles_cabecera = await _context.Inspecciones_Detalle.Where(e => cabeceras.Select(c => c.Id_Inspeccion_Cabecera).Contains(e.Id_Inspeccion_Cabecera)).ToListAsync();

            var TablaResumen = new List<InspeccionCabecera>();
            foreach (var item in cabeceras)
            {
                var gerEjectMar = gerencias.Where(g => g.Id_Gerencia.Equals(item.Id_Gerencia_Ejecutora)).FirstOrDefault();
                var empContMar = empresas.Where(e => e.Id_Empresa_Contratista.Equals(item.Id_Empresa_Contratista)).FirstOrDefault();
                var nObservaciones = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Count();
                var nObsImplementadas = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera) && d.Estado.Equals(EstadosInspeccion.Implementado.ToString())).Count();
                var nObsAprobadas = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera) && d.Estado.Equals(EstadosInspeccion.AprobadoMA.ToString())).Count();
                var nObsRechazadas = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera) && d.Estado.Equals(EstadosInspeccion.RechazadoMA.ToString())).Count();

                var cabecera = new InspeccionCabecera();
                cabecera.IdCabecera = item.Id_Inspeccion_Cabecera;
                cabecera.Codigo = item.Codigo;
                cabecera.GerenEjecMar = gerEjectMar.Descripcion;
                cabecera.EmpContratMarc = empContMar.Razon_Social;
                cabecera.LugarInspec = item.Lugar_Inspeccionado;
                cabecera.FechaInspec = item.Fecha_Inspeccion;
                cabecera.NObservaciones = nObservaciones;
                cabecera.Estado = item.Estado;
                cabecera.NObsImplementadas = nObsImplementadas;
                cabecera.NObsAprobadas = nObsAprobadas;
                cabecera.NObsRechazadas = nObsRechazadas;
                cabecera.Tipo_Inspeccion = item.Tipo_Inspeccion;
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
            output.Id_Registrado_Por = cabeccera.Id_Registrado_Por;
            output.Codigo = cabeccera.Codigo;
            output.idGerenciaEjecMar = cabeccera.Id_Gerencia_Ejecutora;
            output.idEmpContratMarc = cabeccera.Id_Empresa_Contratista;
            output.Tipo_Inspeccion = cabeccera.Tipo_Inspeccion;
            output.LugarInspec = cabeccera.Lugar_Inspeccionado;
            output.FechaInspec = cabeccera.Fecha_Inspeccion;
            output.NObservaciones = observaciones.Count;
            output.Estado = cabeccera.Estado;
            output.idResponsableLugar = cabeccera.Id_Responsable_Lugar;
            output.Objetivo = cabeccera.Objetivo;
            output.Responsable_Inspeccion = cabeccera.Responsable_Inspeccion;
            output.Inspectores = cabeccera.Inspectores;

            return output;
        }

        public async Task<List<InspeccionCabecera>> GetInspeccionCabeceraImplementacion(DateTime fecha, int gerencia, string codigo,int id_usuario)
        {
            //var observaciones_usuario = await _context.Inspecciones_Detalle.Where(o=>o.Id_Responsable_Marcobre.Equals(id_usuario)).ToListAsync();

            var fechaFiltro = (fecha == DateTime.MinValue) ? (DateTime)SqlDateTime.MinValue : fecha;
            var cabeceras = await _context.Inspecciones_Cabecera.Where(c =>
            //    observaciones_usuario.Select(x=>x.Id_Inspeccion_Cabecera).Contains(c.Id_Inspeccion_Cabecera) &&
                c.Fecha_Inspeccion.Date.Equals(fechaFiltro != (DateTime)SqlDateTime.MinValue ? fechaFiltro.Date : c.Fecha_Inspeccion.Date)
                && c.Id_Gerencia_Ejecutora.Equals(gerencia != 0 ? gerencia : c.Id_Gerencia_Ejecutora)
                && c.Codigo.Contains(codigo != "" ? codigo : c.Codigo)
                && c.Estado.Equals(EstadosInspeccion.Registrado.ToString())
                )
                .OrderByDescending(x => x.Id_Inspeccion_Cabecera)
                .ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas_Contratistas.ToListAsync();
            var detalles_cabecera = await _context.Inspecciones_Detalle.Where(e => cabeceras.Select(c => c.Id_Inspeccion_Cabecera).Contains(e.Id_Inspeccion_Cabecera)).ToListAsync();

            var TablaResumen = new List<InspeccionCabecera>();
            foreach (var item in cabeceras)
            {
                var gerEjectMar = gerencias.Where(g => g.Id_Gerencia.Equals(item.Id_Gerencia_Ejecutora)).FirstOrDefault();
                var empContMar = empresas.Where(e => e.Id_Empresa_Contratista.Equals(item.Id_Empresa_Contratista)).FirstOrDefault();
                var nObservaciones = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Count();

                TablaResumen.Add(new InspeccionCabecera(item.Id_Inspeccion_Cabecera, item.Codigo, gerEjectMar.Descripcion,
                    empContMar.Razon_Social, item.Lugar_Inspeccionado, item.Fecha_Inspeccion, nObservaciones, item.Estado));
            }
            return TablaResumen;
        }
        public async Task<InspeccionCabecera> GetInspeccionCabeceraImplementacion(int idCabecera)
        {
            var cabeccera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)).ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas_Contratistas.ToListAsync();
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
            output.GerenEjecMar = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabeccera.Id_Gerencia_Ejecutora)).Descripcion;
            output.EmpContratMarc = empresas.FirstOrDefault(x => x.Id_Empresa_Contratista.Equals(cabeccera.Id_Empresa_Contratista)).Razon_Social;
            output.LugarInspec = cabeccera.Lugar_Inspeccionado;
            output.Responsable_Lugar = usuarios_domain.FirstOrDefault(x => x.Id_Usuario.Equals(cabeccera.Id_Responsable_Lugar));
            output.FechaInspec = cabeccera.Fecha_Inspeccion;
            output.NObservaciones = observaciones.Count;
            output.Inspectores = cabeccera.Inspectores;
            output.Estado = cabeccera.Estado;
            output.Tipo_Inspeccion = cabeccera.Tipo_Inspeccion;
            output.Responsable_Inspeccion = cabeccera.Responsable_Inspeccion;
            return output;
        }

        public async Task<List<InspeccionCabecera>> GetInspeccionCabeceraVerificacion(DateTime fecha, int gerencia, string codigo)
        {
            var fechaFiltro = (fecha == DateTime.MinValue) ? (DateTime)SqlDateTime.MinValue : fecha;
            var cabeceras = await _context.Inspecciones_Cabecera.Where(c =>
                c.Estado.Equals(EstadosInspeccion.Registrado.ToString()) &&
                c.Fecha_Inspeccion.Date.Equals(fechaFiltro != (DateTime)SqlDateTime.MinValue ? fechaFiltro.Date : c.Fecha_Inspeccion.Date)
                && c.Id_Gerencia_Ejecutora.Equals(gerencia != 0 ? gerencia : c.Id_Gerencia_Ejecutora)
                && c.Codigo.Contains(codigo != "" ? codigo : c.Codigo)
                )
                .OrderByDescending(x => x.Id_Inspeccion_Cabecera)
                .ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas_Contratistas.ToListAsync();
            var detalles_cabecera = await _context.Inspecciones_Detalle.Where(e => cabeceras.Select(c => c.Id_Inspeccion_Cabecera).Contains(e.Id_Inspeccion_Cabecera)).ToListAsync();

            var TablaResumen = new List<InspeccionCabecera>();
            foreach (var item in cabeceras)
            {
                var gerEjectMar = gerencias.Where(g => g.Id_Gerencia.Equals(item.Id_Gerencia_Ejecutora)).FirstOrDefault();
                var empContMar = empresas.Where(e => e.Id_Empresa_Contratista.Equals(item.Id_Empresa_Contratista)).FirstOrDefault();
                var nObservaciones = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Count();

                TablaResumen.Add(new InspeccionCabecera(item.Id_Inspeccion_Cabecera, item.Codigo, gerEjectMar.Descripcion,
                    empContMar.Razon_Social, item.Lugar_Inspeccionado, item.Fecha_Inspeccion, nObservaciones, item.Estado));
            }

            return TablaResumen;
        }

        public async Task<InspeccionCabecera> GetInspeccionCabeceraVerificacion(int idCabecera)
        {
            var cabeccera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)).ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas_Contratistas.ToListAsync();

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
            output.GerenEjecMar = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabeccera.Id_Gerencia_Ejecutora)).Descripcion;
            output.EmpContratMarc = empresas.FirstOrDefault(x => x.Id_Empresa_Contratista.Equals(cabeccera.Id_Empresa_Contratista)).Razon_Social;
            output.LugarInspec = cabeccera.Lugar_Inspeccionado;
            output.Responsable_Lugar = usuarios_domain.FirstOrDefault(x => x.Id_Usuario.Equals(cabeccera.Id_Responsable_Lugar));
            output.FechaInspec = cabeccera.Fecha_Inspeccion;
            output.NObservaciones = observaciones.Count;
            output.Inspectores = cabeccera.Inspectores;
            output.Estado = cabeccera.Estado;
            output.Tipo_Inspeccion = cabeccera.Tipo_Inspeccion;
            output.Responsable_Inspeccion = cabeccera.Responsable_Inspeccion;
            return output;
        }
        public async Task<InspeccionCabecera> GetInspeccionCabeceraDescarga(int idCabecera)
        {
            //Cabecera
            var cabecera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)).ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas_Contratistas.ToListAsync();

            var usuarios = await _context.Usuarios.ToListAsync();
            var usuarios_domain = new List<Domain.ValueObjects.Usuario>();
            foreach (var item in usuarios)
            {
                usuarios_domain.Add(new Domain.ValueObjects.Usuario(item.Id_Usuario, item.Id_Rol, item.Nombres, item.Apellidos, item.Email));
            }
            //Detalle
            var tiposCondicion = await _context.Tipos_Condicion.ToListAsync();
            var tiposCondicion_domain = new List<Domain.ValueObjects.Tipo_Condicion>();
            foreach (var item in tiposCondicion)
            {
                tiposCondicion_domain.Add(new Domain.ValueObjects.Tipo_Condicion(item.Id_Tipo_Condicion, item.Descripcion,item.Grupo));
            }


            var output = new InspeccionCabecera();
            output.IdCabecera = cabecera.Id_Inspeccion_Cabecera;
            output.Codigo = cabecera.Codigo;
            output.Objetivo = cabecera.Objetivo;
            output.GerenEjecMar = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabecera.Id_Gerencia_Ejecutora)).Descripcion;
            output.EmpContratMarc = empresas.FirstOrDefault(x => x.Id_Empresa_Contratista.Equals(cabecera.Id_Empresa_Contratista)).Razon_Social;
            output.LugarInspec = cabecera.Lugar_Inspeccionado;
            output.Responsable_Lugar = usuarios_domain.FirstOrDefault(x => x.Id_Usuario.Equals(cabecera.Id_Responsable_Lugar));
            output.FechaInspec = cabecera.Fecha_Inspeccion;
            output.Inspectores = cabecera.Inspectores;
            output.Estado = cabecera.Estado;
            output.Tipo_Inspeccion = cabecera.Tipo_Inspeccion;
            output.Responsable_Inspeccion = cabecera.Responsable_Inspeccion;
            

            output.Observaciones = new List<InspeccionDetalle>();
            foreach(var observacion in observaciones)
            {
                var obs = new InspeccionDetalle();
                obs.Id_Inspeccion_Cabecera = idCabecera;
                obs.Id_Inspeccion_Detalle = observacion.Id_Inspeccion_Detalle;
                obs.Tipo_Observacion = observacion.Tipo_Observacion;
                obs.Descripcion_Condicion = observacion.Descripcion_Condicion;
                obs.Tipo_Condicion = tiposCondicion_domain.FirstOrDefault(x => x.Id_Tipo_Condicion.Equals(observacion.Id_Tipo_Condicion));
                obs.Nivel_Riesgo_Ambiental = observacion.Nivel_Riesgo_Ambiental;
                obs.Descripcion_Accion = observacion.Descripcion_Accion;
                obs.Responsable_Marcobre = usuarios_domain.FirstOrDefault(x => x.Id_Usuario.Equals(observacion.Id_Responsable_Marcobre));
                obs.Responsable_Contratista = observacion.Responsable_Contratista;
                obs.Fecha_Programada = observacion.Fecha_Programada;
                obs.Comentario_Inspeccion = observacion.Comentario_Inspeccion;
                obs.Fecha_Cierre = observacion.Fecha_Cierre;
                obs.Comentario_MA = observacion.Comentario_MA;
                obs.Fecha_Verificacion_MA = observacion.Fecha_Verificacion_MA;
                obs.Estado = observacion.Estado;
                obs.Evidencia_Inspeccion = observacion.Evidencia_Inspeccion;
                obs.Evidencia_Condicion = observacion.Evidencia_Condicion;
                output.Observaciones.Add(obs);
            }

            return output;
        }
    }
}
