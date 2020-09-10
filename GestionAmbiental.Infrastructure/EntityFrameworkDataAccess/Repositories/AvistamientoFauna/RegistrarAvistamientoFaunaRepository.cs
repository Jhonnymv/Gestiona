using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Entities.AvistamientoFauna;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.AvistamientoFauna
{
    public class RegistrarAvistamientoFaunaRepository : IRegistrarAvistamientoFaunaReadOnlyRepository
    {
        public Context _context;

        public RegistrarAvistamientoFaunaRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<int> RegistrarAvistamientoFauna(int idUsuario, Domain.AvistamientoFauna.AvistamientoFauna avistamiento)
        {
            var entity = new Entities.AvistamientoFauna.Avistamiento_Fauna();
            entity.Codigo = avistamiento.Codigo;
            entity.Fotos = avistamiento.Fotos;
            entity.Fecha_Avistamiento = avistamiento.FechaAvistamiento;
            entity.Ubicacion = avistamiento.Ubicacion;
            entity.Cantidad = avistamiento.Cantidad;
            entity.Id_Nombre_Comun_Fauna = avistamiento.NombreComun?.IdNombreComunFauna;
            entity.Id_Tipo_Avistamiento = avistamiento.TipoAvistamiento?.IdTipoAvistamiento;
            entity.Id_Estado_Fauna = avistamiento.EstadoFauna?.IdEstadoFauna;
            entity.Id_Lugar_Referencia = avistamiento.LugarReferencia?.IdLugarReferencia;
            entity.Id_Usuario = avistamiento.Usuario.IdUsuario;
            entity.Estado = avistamiento.Estado;

            _context.Avistamientos_Fauna.Add(entity);
            await _context.SaveChangesAsync();

            var log = new Log_Avistamiento_Fauna();
            var bkp = JsonConvert.SerializeObject(entity);
            log.Bkp = bkp;
            log.Fecha = DateTime.Now;
            log.Id_Avistamiento_Fauna = entity.Id_Avistamiento_Fauna;
            log.Id_Usuario = idUsuario;
            log.Tipo = TipoAccion.CREATE.ToString();

            _context.Logs_Avistamiento_Fauna.Add(log);
            await _context.SaveChangesAsync();

            return entity.Id_Avistamiento_Fauna;
        }
        public async Task<string> GetUltimoCodigo()
        {

            var codigo = await _context.Avistamientos_Fauna.Where(x => !x.Codigo.Equals("TBD")).ToListAsync();
            if (codigo.Count > 0)
                return codigo.OrderBy(x => Int32.Parse(x.Codigo)).Select(x => x.Codigo).LastOrDefault();
            else
                return null;
        }
    }
}
