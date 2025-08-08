using GestionAmbiental.Application.Repositories.Incidentes.ReportePreliminar;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.ReportePreliminar;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            if(reporte.Gerencia_Responsable != null)
                entity.Id_Gerencia_Responsable = reporte.Gerencia_Responsable.IdGerencia;
            entity.Evidencia = reporte.Evidencia;
            entity.Nombres_Archivos = reporte.NombresArchivos;
            entity.Comentario_Accion = reporte.Comentario_Accion;
            if(reporte.Supervisor_MA != null)
                entity.Id_Supervisor_MA = reporte.Supervisor_MA.IdUsuario;
            if (reporte.Estado != null)
            {
                //entity.Codigo = reporte.Codigo;
                entity.Estado = reporte.Estado;
            }

            _context.Reporte_Preliminar_Incidente.Update(entity);
            await _context.SaveChangesAsync();
            return entity.Id_Reporte_Preliminar;
        }
        public async Task<int> EditarAccionCorrectivaReportePreliminar(Accion_Incidente_Reporte accion)
        {
            var entity = await _context.Accion_Incidente_Reporte.FirstOrDefaultAsync(x => x.Id_Accion_Incidente_Reporte.Equals(accion.Id_Accion_Incidente_Reporte));
            entity.Accion_Inmediata = accion.Accion_Inmediata != null ? accion.Accion_Inmediata : entity.Accion_Inmediata;
            entity.Id_Usuario = accion.Id_Usuario != null ? accion.Id_Usuario : entity.Id_Usuario;
            entity.Responsable = accion.Responsable != null ? accion.Responsable : entity.Responsable ;
            entity.Fecha = accion.Fecha != null ? accion.Fecha : entity.Fecha;
            entity.Estatus = accion.Estatus != null ? accion.Estatus : entity.Estatus;
            entity.Estado = accion.Estado != null ? accion.Estado : entity.Estado;

            _context.Accion_Incidente_Reporte.Update(entity);

            await _context.SaveChangesAsync();

            return entity.Id_Accion_Incidente_Reporte;
        }
        public async Task<int> EditarCausaIncidenteReporte(Causa_Incidente_Reporte causa)
        {
            var entity = await _context.Causa_Incidente_Reporte.FirstOrDefaultAsync(x => x.Id_Causa_Incidente_Reporte.Equals(causa.Id_Causa_Incidente_Reporte));
            entity.Id_Acto_Condicion = causa.Id_Acto_Condicion;
            entity.Tipo_Acto_Condicion = causa.Tipo_Acto_Condicion;
            entity.Estado = causa.Estado;

            _context.Causa_Incidente_Reporte.Update(entity);

            await _context.SaveChangesAsync();

            return entity.Id_Causa_Incidente_Reporte;
        }
        public async Task<int> EditarInvolucradoReportePreliminar(Involucrado_Incidente_Reporte involucrado)
        {
            var entity = await _context.Involucrado_Incidente_Reporte.FirstOrDefaultAsync(x => x.Id_Involucrado_Incidente_Reporte.Equals(involucrado.Id_Involucrado_Incidente_Reporte));
            entity.Id_Reporte_Preliminar = involucrado.Id_Reporte_Preliminar != null ? involucrado.Id_Reporte_Preliminar : entity.Id_Reporte_Preliminar;
            entity.Id_Usuario = involucrado.Id_Usuario != null ? involucrado.Id_Usuario : entity.Id_Usuario;
            entity.Nombres = involucrado.Nombres != null ? involucrado.Nombres : entity.Nombres;
            entity.Apellidos = involucrado.Apellidos != null ? involucrado.Apellidos : entity.Apellidos;
            entity.Email = involucrado.Email;
            entity.Puesto_Trabajo = involucrado.Puesto_Trabajo != null ? involucrado.Puesto_Trabajo : entity.Puesto_Trabajo;
            entity.Celular = involucrado.Celular;
            entity.Turno = involucrado.Turno;
            entity.Tipo_Involucrado = involucrado.Tipo_Involucrado != null ? involucrado.Tipo_Involucrado : entity.Tipo_Involucrado;
            entity.Estado = involucrado.Estado;

            _context.Involucrado_Incidente_Reporte.Update(entity);
            await _context.SaveChangesAsync();

            return entity.Id_Involucrado_Incidente_Reporte;
        }
        public async Task<bool> EliminarEvidencia(int IdReportePreliminar, string NombreArchivo)
        {
            var reporte = await _context.Reporte_Preliminar_Incidente.FirstOrDefaultAsync(x => x.Id_Reporte_Preliminar.Equals(IdReportePreliminar));
            if (reporte != null && reporte.Evidencia!=null)
            {
                var evidencias = JArray.Parse(reporte.Evidencia);
                var temp = new List<string>();
                foreach (var evidencia in evidencias)
                {
                    temp.Add((string)evidencia);
                }
                if (temp.Remove(NombreArchivo))
                {
                    if (reporte.Nombres_Archivos != null)
                    {
                        var nombresArchivos = JsonConvert.DeserializeObject<List<NombreEvidencia>>(reporte.Nombres_Archivos);
                        nombresArchivos.Remove(nombresArchivos.FirstOrDefault(x => x.NombreCarga.Equals(NombreArchivo)));
                        reporte.Nombres_Archivos = JsonConvert.SerializeObject(nombresArchivos);
                    }
                    else
                    {
                        var nombresArchivos = temp.Select(x=> 
                        new NombreEvidencia {
                            NombreOrigen = x,
                            NombreCarga = x,
                            ExtOrigen = Path.GetExtension(x),
                            ExtCarga = Path.GetExtension(x)
                        });
                    }
                    reporte.Evidencia = JsonConvert.SerializeObject(temp);
                }
                else
                    return false;


                _context.Reporte_Preliminar_Incidente.Update(reporte);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int> EliminarReportePreliminar(int IdReportePreliminar)
        {
            var entity = await _context.Reporte_Preliminar_Incidente.FirstOrDefaultAsync(x => x.Id_Reporte_Preliminar.Equals(IdReportePreliminar));
            entity.Estado = EstadosIncidentes.Eliminado.ToString();

            var acciones = await _context.Accion_Incidente_Reporte.Where(x => x.Id_Reporte_Preliminar.Equals(IdReportePreliminar)).ToListAsync();
            acciones.ForEach(item => { item.Estado = EstadosIncidentes.Eliminado.ToString(); });
            var causas = await _context.Causa_Incidente_Reporte.Where(x => x.Id_Reporte_Preliminar.Equals(IdReportePreliminar)).ToListAsync();
            causas.ForEach(item => { item.Estado = EstadosIncidentes.Eliminado.ToString(); });
            var involucrados = await _context.Involucrado_Incidente_Reporte.Where(x => x.Id_Reporte_Preliminar.Equals(IdReportePreliminar)).ToListAsync();
            involucrados.ForEach(item => { item.Estado = EstadosIncidentes.Eliminado.ToString(); });

            _context.Reporte_Preliminar_Incidente.Update(entity);
            _context.Accion_Incidente_Reporte.UpdateRange(acciones);
            _context.Causa_Incidente_Reporte.UpdateRange(causas);
            _context.Involucrado_Incidente_Reporte.UpdateRange(involucrados);

            await _context.SaveChangesAsync();

            return entity.Id_Reporte_Preliminar;
        }

        public async Task<bool> EditarEvidenciasReporte(Reporte_Preliminar_Incidente input)
        {
            try
            {
                var entity = await _context.Reporte_Preliminar_Incidente.FirstOrDefaultAsync(x => x.Id_Reporte_Preliminar.Equals(input.Id_Reporte_Preliminar));
                entity.Evidencia = input.Evidencia;
                entity.Nombres_Archivos = input.NombresArchivos;

                _context.Reporte_Preliminar_Incidente.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public class NombreEvidencia
        {
            public string NombreOrigen { get; set; }
            public string NombreCarga { get; set; }
            public string ExtOrigen { get; set; }
            public string ExtCarga { get; set; }
        }
    }
}
