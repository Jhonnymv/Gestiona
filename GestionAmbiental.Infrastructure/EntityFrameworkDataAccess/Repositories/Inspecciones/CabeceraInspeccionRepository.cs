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
        public async Task<List<InspeccionCabecera>> GetInspeccionCabecera(int pagina, int fpp, DateTime fecha, int gerencia, string codigo)
        {
            var fechaFiltro = (fecha == DateTime.MinValue) ? (DateTime)SqlDateTime.MinValue : fecha;
            var cabeceras = await _context.Inspecciones_Cabecera.Where(c =>
                c.Fecha_Inspeccion.Date.Equals(fechaFiltro != (DateTime)SqlDateTime.MinValue ? fechaFiltro.Date : c.Fecha_Inspeccion.Date)
                && c.Id_Gerencia_Ejecutora.Equals(gerencia != 0 ? gerencia : c.Id_Gerencia_Ejecutora)
                && c.Codigo.Contains(codigo != "" ? codigo : c.Codigo)
                )
                .OrderByDescending(x=>x.Id_Inspeccion_Cabecera)
                .ToListAsync();
            var gerencias = await _context.Gerencias.ToListAsync();
            var empresas = await _context.Empresas_Contratistas.ToListAsync();
            var detalles_cabecera = await _context.Inspecciones_Detalle.Where(e => cabeceras.Select(c => c.Id_Inspeccion_Cabecera).Contains(e.Id_Inspeccion_Cabecera)).ToListAsync();

            var TablaResumen = new List<InspeccionCabecera>();
            foreach (var item in cabeceras)
            {
                var gerEjectMar = gerencias.Where(g => g.Id_Gerencia.Equals(item.Id_Gerencia_Ejecutora)).FirstOrDefault();
                var gerEjectEPCM = gerencias.Where(g => g.Id_Gerencia.Equals(item.Id_Gerencia_EPCM)).FirstOrDefault();
                var empContMar = empresas.Where(e => e.Id_Empresa_Contratista.Equals(item.Id_Empresa_Contratista)).FirstOrDefault();
                var empContEPCM = empresas.Where(e => e.Id_Empresa_Contratista.Equals(item.Id_Empresa_Contratista_EPCM)).FirstOrDefault();
                var nObservaciones = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Count();
                var nObsImplementadas = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera) && d.Estado.Equals(EstadosInspeccion.Implementado.ToString())).Count();
                var nObsAprobadas = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera) && d.Estado.Equals(EstadosInspeccion.AprobadoMA.ToString())).Count();
                var nObsRechazadas = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera) && d.Estado.Equals(EstadosInspeccion.RechazadoMA.ToString())).Count();

                var cabecera = new InspeccionCabecera();
                cabecera.IdCabecera = item.Id_Inspeccion_Cabecera;
                cabecera.Codigo = item.Codigo;
                cabecera.GerenEjecMar = gerEjectMar.Descripcion;
                cabecera.EmpContratMarc = empContMar.Razon_Social;
                cabecera.GerenEjecEPCM = gerEjectEPCM.Descripcion;
                cabecera.EmpContratEPCM = empContEPCM.Razon_Social;
                cabecera.LugarInspec = item.Lugar_Inspeccionado;
                cabecera.FechaInspec = item.Fecha_Inspeccion;
                cabecera.NObservaciones = nObservaciones;
                cabecera.Estado = item.Estado;
                cabecera.NObsImplementadas = nObsImplementadas;
                cabecera.NObsAprobadas = nObsAprobadas;
                cabecera.NObsRechazadas = nObsRechazadas;
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
            output.Codigo = cabeccera.Codigo;
            output.idGerenciaEjecMar = cabeccera.Id_Gerencia_Ejecutora;
            output.idEmpContratMarc = cabeccera.Id_Empresa_Contratista;
            output.idGerencEjectEPCM = cabeccera.Id_Gerencia_EPCM;
            output.idEmpContratEPCM = cabeccera.Id_Empresa_Contratista_EPCM;
            output.LugarInspec = cabeccera.Lugar_Inspeccionado;
            output.FechaInspec = cabeccera.Fecha_Inspeccion;
            output.NObservaciones = observaciones.Count;
            output.Estado = cabeccera.Estado;
            output.idResponsableLugar = cabeccera.Id_Responsable_Lugar;
            output.Objetivo = cabeccera.Objetivo;
            output.Inspectores = cabeccera.Inspectores;

            return output;
        }
        public async Task<List<InspeccionCabecera>> GetInspeccionCabeceraImplementacion(int pagina, int fpp, DateTime fecha, int gerencia, string codigo)
        {
            var fechaFiltro = (fecha == DateTime.MinValue) ? (DateTime)SqlDateTime.MinValue : fecha;
            var cabeceras = await _context.Inspecciones_Cabecera.Where(c =>
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
                var gerEjectEPCM = gerencias.Where(g => g.Id_Gerencia.Equals(item.Id_Gerencia_EPCM)).FirstOrDefault();
                var empContMar = empresas.Where(e => e.Id_Empresa_Contratista.Equals(item.Id_Empresa_Contratista)).FirstOrDefault();
                var empContEPCM = empresas.Where(e => e.Id_Empresa_Contratista.Equals(item.Id_Empresa_Contratista_EPCM)).FirstOrDefault();
                var nObservaciones = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Count();

                TablaResumen.Add(new InspeccionCabecera(item.Id_Inspeccion_Cabecera, item.Codigo, gerEjectMar.Descripcion,
                    empContMar.Razon_Social, gerEjectEPCM.Descripcion, empContEPCM.Razon_Social,
                    item.Lugar_Inspeccionado, item.Fecha_Inspeccion, nObservaciones, item.Estado));
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
            output.GerenEjecEPCM = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabeccera.Id_Gerencia_EPCM)).Descripcion;
            output.EmpContratEPCM = empresas.FirstOrDefault(x => x.Id_Empresa_Contratista.Equals(cabeccera.Id_Empresa_Contratista_EPCM)).Razon_Social;
            output.LugarInspec = cabeccera.Lugar_Inspeccionado;
            output.Responsable_Lugar = usuarios_domain.FirstOrDefault(x => x.Id_Usuario.Equals(cabeccera.Id_Responsable_Lugar));
            output.FechaInspec = cabeccera.Fecha_Inspeccion;
            output.NObservaciones = observaciones.Count;
            output.Inspectores = cabeccera.Inspectores;
            output.Estado = cabeccera.Estado;
            return output;
        }

        public async Task<List<InspeccionCabecera>> GetInspeccionCabeceraVerificacion(int pagina, int fpp, DateTime fecha, int gerencia, string codigo)
        {
            var fechaFiltro = (fecha == DateTime.MinValue) ? (DateTime)SqlDateTime.MinValue : fecha;
            var cabeceras = await _context.Inspecciones_Cabecera.Where(c =>
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
                var gerEjectEPCM = gerencias.Where(g => g.Id_Gerencia.Equals(item.Id_Gerencia_EPCM)).FirstOrDefault();
                var empContMar = empresas.Where(e => e.Id_Empresa_Contratista.Equals(item.Id_Empresa_Contratista)).FirstOrDefault();
                var empContEPCM = empresas.Where(e => e.Id_Empresa_Contratista.Equals(item.Id_Empresa_Contratista_EPCM)).FirstOrDefault();
                var nObservaciones = detalles_cabecera.Where(d => d.Id_Inspeccion_Cabecera.Equals(item.Id_Inspeccion_Cabecera)).Count();

                TablaResumen.Add(new InspeccionCabecera(item.Id_Inspeccion_Cabecera, item.Codigo, gerEjectMar.Descripcion,
                    empContMar.Razon_Social, gerEjectEPCM.Descripcion, empContEPCM.Razon_Social,
                    item.Lugar_Inspeccionado, item.Fecha_Inspeccion, nObservaciones, item.Estado));
            }

            return TablaResumen;
        }

        public async Task<InspeccionCabecera> GetInspeccionCabeceraVerificacion(int idCabecera)
        {
            var cabeccera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera) && x.Estado.Equals("Implementado")).ToListAsync();
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
            output.GerenEjecEPCM = gerencias.FirstOrDefault(x => x.Id_Gerencia.Equals(cabeccera.Id_Gerencia_EPCM)).Descripcion;
            output.EmpContratEPCM = empresas.FirstOrDefault(x => x.Id_Empresa_Contratista.Equals(cabeccera.Id_Empresa_Contratista_EPCM)).Razon_Social;
            output.LugarInspec = cabeccera.Lugar_Inspeccionado;
            output.Responsable_Lugar = usuarios_domain.FirstOrDefault(x => x.Id_Usuario.Equals(cabeccera.Id_Responsable_Lugar));
            output.FechaInspec = cabeccera.Fecha_Inspeccion;
            output.NObservaciones = observaciones.Count;
            output.Inspectores = cabeccera.Inspectores;
            output.Estado = cabeccera.Estado;
            return output;
        }
    }
}
