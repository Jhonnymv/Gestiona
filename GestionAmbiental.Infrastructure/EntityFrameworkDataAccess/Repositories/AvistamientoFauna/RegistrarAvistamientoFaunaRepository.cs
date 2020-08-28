using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
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

        public async Task<int> RegistrarAvistamientoFauna(Domain.AvistamientoFauna.AvistamientoFauna avistamiento)
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
