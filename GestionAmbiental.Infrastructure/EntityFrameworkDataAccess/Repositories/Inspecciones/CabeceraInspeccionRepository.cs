using GestionAmbiental.Application.Repositories.Inspecciones;
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

        public async Task<InspeccionCabecera> GetInspeccionCabecera(int idCabecera)
        {
            var cabeccera = await _context.Inspecciones_Cabecera.FirstOrDefaultAsync(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera));
            var observaciones = await _context.Inspecciones_Detalle.Where(x => x.Id_Inspeccion_Cabecera.Equals(idCabecera)).ToListAsync();
            var output = new InspeccionCabecera(
                cabeccera.Id_Inspeccion_Cabecera, 
                cabeccera.Codigo, 
                cabeccera.Id_Gerencia_Ejecutora, 
                cabeccera.Id_Empresa_Contratista, 
                cabeccera.Id_Gerencia_EPCM, 
                cabeccera.Id_Empresa_Contratista_EPCM, 
                cabeccera.Lugar_Inspeccionado,
                cabeccera.Fecha_Inspeccion, 
                observaciones.Count, 
                cabeccera.Estado,
                cabeccera.Id_Responsable_Lugar,
                cabeccera.Id_Responsable_MA,
                cabeccera.Objetivo,
                cabeccera.Inspectores);
            return output;

            
        }
    }
}
