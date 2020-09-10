using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.AvistamientoFauna;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.AvistamientoFauna
{
    public class EditarAvistamientoFaunaRepository : IEditarAvistamientoFaunaReadOnlyRepository
    {
        private readonly Context _context;

        public EditarAvistamientoFaunaRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task ActualizarAvistamiento(int idUsuario, Domain.AvistamientoFauna.AvistamientoFauna nuevoAvistamientoFauna)
        {
            var avistamiento = await _context.Avistamientos_Fauna.FirstOrDefaultAsync(x => x.Id_Avistamiento_Fauna.Equals(nuevoAvistamientoFauna.IdAvistamientoFauna));
            avistamiento.Codigo = nuevoAvistamientoFauna.Codigo;
            avistamiento.Fotos = nuevoAvistamientoFauna.Fotos;
            avistamiento.Fecha_Avistamiento = nuevoAvistamientoFauna.FechaAvistamiento;
            avistamiento.Ubicacion = nuevoAvistamientoFauna.Ubicacion;
            avistamiento.Cantidad = nuevoAvistamientoFauna.Cantidad;
            avistamiento.Id_Nombre_Comun_Fauna = nuevoAvistamientoFauna.NombreComun?.IdNombreComunFauna;
            avistamiento.Id_Tipo_Avistamiento = nuevoAvistamientoFauna.TipoAvistamiento?.IdTipoAvistamiento;
            avistamiento.Id_Estado_Fauna = nuevoAvistamientoFauna.EstadoFauna?.IdEstadoFauna;
            avistamiento.Id_Lugar_Referencia = nuevoAvistamientoFauna.LugarReferencia?.IdLugarReferencia;
            avistamiento.Id_Usuario = nuevoAvistamientoFauna.Usuario.IdUsuario;
            avistamiento.Estado = nuevoAvistamientoFauna.Estado;

            _context.Avistamientos_Fauna.Update(avistamiento);
            
            var log = new Log_Avistamiento_Fauna();
            var bkp = JsonConvert.SerializeObject(avistamiento);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Avistamiento_Fauna = avistamiento.Id_Avistamiento_Fauna;
            log.Id_Usuario = idUsuario;
            log.Tipo = TipoAccion.UPDATE.ToString();

            _context.Logs_Avistamiento_Fauna.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task CompletarAvistamiento(Domain.AvistamientoFauna.AvistamientoFauna avistamientoMA)
        {
            var avistamiento = await _context.Avistamientos_Fauna.FirstOrDefaultAsync(x => x.Id_Avistamiento_Fauna.Equals(avistamientoMA.IdAvistamientoFauna));
            avistamiento.Fotos = avistamientoMA.Fotos;
            avistamiento.Fecha_Avistamiento = avistamientoMA.FechaAvistamiento;
            avistamiento.Ubicacion = avistamientoMA.Ubicacion;
            avistamiento.Cantidad = avistamientoMA.Cantidad;
            avistamiento.Id_Nombre_Comun_Fauna = avistamientoMA.NombreComun?.IdNombreComunFauna;
            avistamiento.Id_Tipo_Avistamiento = avistamientoMA.TipoAvistamiento?.IdTipoAvistamiento;
            avistamiento.Id_Estado_Fauna = avistamientoMA.EstadoFauna?.IdEstadoFauna;
            avistamiento.Id_Lugar_Referencia = avistamientoMA.LugarReferencia?.IdLugarReferencia;
            avistamiento.Id_Supervisor = avistamientoMA.Supervisor.IdUsuario;
            avistamiento.Evidencia_Accion = avistamientoMA.EvidenciaAccion;
            avistamiento.Descripcion_Accion = avistamientoMA.DescripcionAccion;
            avistamiento.Estado = avistamientoMA.Estado;

            _context.Avistamientos_Fauna.Update(avistamiento);

            var log = new Log_Avistamiento_Fauna();
            var bkp = JsonConvert.SerializeObject(avistamiento);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Avistamiento_Fauna = avistamiento.Id_Avistamiento_Fauna;
            log.Id_Usuario = avistamiento.Id_Supervisor.Value;
            log.Tipo = TipoAccion.UPDATE.ToString();

            _context.Logs_Avistamiento_Fauna.Add(log);
            await _context.SaveChangesAsync();
        }
        public async Task EliminarFotoAvistamiento(int IdAvistamiento, string foto)
        {
            var avistamiento = await _context.Avistamientos_Fauna.FirstOrDefaultAsync(x => x.Id_Avistamiento_Fauna.Equals(IdAvistamiento));
            var values = JsonConvert.DeserializeObject<List<string>>(avistamiento.Fotos);
            values.Remove(foto);
            avistamiento.Fotos = JsonConvert.SerializeObject(values);

            _context.Avistamientos_Fauna.Update(avistamiento);

            await _context.SaveChangesAsync();
        }
        public async Task EliminarAvistamiento(int idUsuario, int IdAvistamiento)
        {
            var avistamiento = await _context.Avistamientos_Fauna.FirstOrDefaultAsync(x => x.Id_Avistamiento_Fauna.Equals(IdAvistamiento));
            avistamiento.Codigo = "TBD";
            avistamiento.Fotos = "[]";
            avistamiento.Estado = EstadosAvistamiento.Eliminado.ToString();

            _context.Avistamientos_Fauna.Update(avistamiento);

            var log = new Log_Avistamiento_Fauna();
            var bkp = JsonConvert.SerializeObject(avistamiento);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Avistamiento_Fauna = avistamiento.Id_Avistamiento_Fauna;
            log.Id_Usuario = idUsuario;
            log.Tipo = TipoAccion.DELETE.ToString();

            _context.Logs_Avistamiento_Fauna.Add(log);
            await _context.SaveChangesAsync();

        }
    }
}
