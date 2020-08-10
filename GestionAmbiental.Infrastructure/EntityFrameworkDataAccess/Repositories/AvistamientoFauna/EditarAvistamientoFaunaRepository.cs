using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Domain.Enum;
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

        public async Task ActualizarAvistamiento(Domain.AvistamientoFauna.AvistamientoFauna nuevoAvistamientoFauna)
        {
            var avistamiento = await _context.Avistamientos_Fauna.FirstOrDefaultAsync(x => x.Id_Avistamiento_Fauna.Equals(nuevoAvistamientoFauna.IdAvistamientoFauna));
            avistamiento.Codigo = nuevoAvistamientoFauna.Codigo;
            avistamiento.Fotos = nuevoAvistamientoFauna.Fotos;
            avistamiento.Fecha_Avistamiento = nuevoAvistamientoFauna.Fecha_Avistamiento;
            avistamiento.Ubicacion = nuevoAvistamientoFauna.Ubicacion;
            avistamiento.Cantidad = nuevoAvistamientoFauna.Cantidad;
            avistamiento.Id_Nombre_Comun_Fauna = nuevoAvistamientoFauna.NombreComun?.IdNombreComunFauna;
            avistamiento.Id_Tipo_Avistamiento = nuevoAvistamientoFauna.TipoAvistamiento?.IdTipoAvistamiento;
            avistamiento.Id_Estado_Fauna = nuevoAvistamientoFauna.EstadoFauna?.IdEstadoFauna;
            avistamiento.Lugar_Referencia = nuevoAvistamientoFauna.Lugar_Referencia;
            avistamiento.Id_Usuario = nuevoAvistamientoFauna.Usuario.IdUsuario;
            avistamiento.Id_Empresa = nuevoAvistamientoFauna.Empresa?.IdEmpresa;
            avistamiento.Id_Area_Trabajo = nuevoAvistamientoFauna.AreaTrabajo?.IdAreaTrabajo;
            avistamiento.N_Fotocheck = nuevoAvistamientoFauna.NFotocheck;
            avistamiento.Estado = nuevoAvistamientoFauna.Estado;

            _context.Avistamientos_Fauna.Update(avistamiento);
            await _context.SaveChangesAsync();
        }

        public async Task CompletarAvistamiento(Domain.AvistamientoFauna.AvistamientoFauna avistamientoMA)
        {
            var avistamiento = await _context.Avistamientos_Fauna.FirstOrDefaultAsync(x => x.Id_Avistamiento_Fauna.Equals(avistamientoMA.IdAvistamientoFauna));
            avistamiento.Id_Supervisor = avistamientoMA.Supervisor.IdUsuario;
            avistamiento.Evidencia_Accion = avistamientoMA.Evidencia_Accion;
            avistamiento.Descripcion_Accion = avistamientoMA.Descripcion_Accion;
            avistamiento.Estado = avistamientoMA.Estado;

            _context.Avistamientos_Fauna.Update(avistamiento);
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
        public async Task EliminarAvistamiento(int IdAvistamiento)
        {
            var avistamiento = await _context.Avistamientos_Fauna.FirstOrDefaultAsync(x => x.Id_Avistamiento_Fauna.Equals(IdAvistamiento));
            avistamiento.Fotos = "[]";
            avistamiento.Estado = EstadosAvistamiento.Eliminado.ToString();


            _context.Avistamientos_Fauna.Update(avistamiento);
            await _context.SaveChangesAsync();

        }
    }
}
