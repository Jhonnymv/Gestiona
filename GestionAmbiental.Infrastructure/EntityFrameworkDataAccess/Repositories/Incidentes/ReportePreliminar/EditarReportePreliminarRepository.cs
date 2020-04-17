using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Incidentes.ReportePreliminar
{
    public class EditarReportePreliminarRepository : IEditarReportePreliminarReadOnlyRepository
    {
        private readonly Context _context;

        public EditarReportePreliminarRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> EditarReportePreliminar(Reporte_Preliminar_Incidente reporte)
        {
            var entity = await _context.Reporte_Preliminar_Incidente.FirstOrDefaultAsync(x => x.Id_Reporte_Preliminar.Equals(reporte.Id_Reporte_Preliminar));
            entity.Codigo = reporte.Codigo;
            entity.Num_Trabajadores = reporte.Num_Trabajadores;
            entity.Razon_Social_Contratista = reporte.Razon_Social_Contratista;
            entity.RUC_Contratista = reporte.RUC_Contratista;
            entity.Actividad_Contratista = reporte.Actividad_Contratista;
            entity.Domicilio_Contratista = reporte.Domicilio_Contratista;
            entity.Num_Trabajadores_Contratista = reporte.Num_Trabajadores_Contratista;
            entity.Id_Tipo_Incidente = reporte.Tipo_Incidente.Id_Tipo_Incidente;
            entity.Id_Nivel_Incidente = reporte.Nivel_Incidente.Id_Nivel_Incidente;
            entity.Fecha_Incidente = reporte.Fecha_Incidente;
            entity.Producto_Derramado = reporte.Producto_Derramado;
            entity.Cantidad_Derramado = reporte.Cantidad_Derramado;
            entity.Id_Unidad_Cantidad_Derramado = reporte.Unidad_Cantidad_Derramado.Id_Unidad_Medida;
            entity.Area_Afectada = reporte.Area_Afectada;
            entity.Id_Unidad_Area_Afectada = reporte.Unidad_Area_Afectada.Id_Unidad_Medida;
            entity.Lugar_Incidente = reporte.Lugar_Incidente;
            entity.Equipo_Involucrado = reporte.Equipo_Involucrado;
            entity.Id_Componente_Ambiental = reporte.Componente_Ambiental.Id_Componente_Ambiental;
            entity.Descripcion_Incidente = reporte.Descripcion_Incidente;

            entity.Otra_Causa_Incidente = reporte.Otra_Causa_Incidente;
            entity.Id_Gerencia_Responsable = reporte.Gerencia_Responsable.IdGerencia;
            entity.Evidencia = reporte.Evidencia;
            entity.Comentario_Accion = reporte.Comentario_Accion;
            entity.Id_Supervisor_MA = reporte.Supervisor_MA.IdUsuario;
            entity.Estado = reporte.Estado;

            _context.Reporte_Preliminar_Incidente.Update(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Reporte_Preliminar;
        }
    }
}
