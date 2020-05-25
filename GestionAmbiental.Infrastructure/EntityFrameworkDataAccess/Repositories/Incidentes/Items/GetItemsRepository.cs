using GestionAmbiental.Application.Repositories.Incidentes.Items;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.Items;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.Incidentes.Items
{
    public class GetItemsRepository : IGetItemsReadOnlyRepository
    {
        private readonly Context _context;

        public GetItemsRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Componente_Ambiental>> GetListComponenteAmbiental()
        {
            var output = new List<Componente_Ambiental>();
            var items = await _context.Componente_Ambiental.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach(var item in items)
            {
                var outputItem = new Componente_Ambiental();
                outputItem.Id_Componente_Ambiental = item.Id_Componente_Ambiental;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Especie_Fauna>> GetListEspecieFauna()
        {
            var output = new List<Especie_Fauna>();
            var items = await _context.Especie_Fauna.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Especie_Fauna();
                outputItem.Id_Especie_Fauna = item.Id_Especie_Fauna;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Especie_Flora>> GetListEspecieFlora()
        {
            var output = new List<Especie_Flora>();
            var items = await _context.Especie_Flora.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Especie_Flora();
                outputItem.Id_Especie_Flora = item.Id_Especie_Flora;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Estado_Equipo_Incidente>> GetListEstadoEquipoIncidente()
        {
            var output = new List<Estado_Equipo_Incidente>();
            var items = await _context.Estado_Equipo_Incidente.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Estado_Equipo_Incidente();
                outputItem.Id_Estado_Equipo_Incidente = item.Id_Estado_Equipo_Incidente;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Lugar_Disposicion>> GetListLugarDisposicion()
        {
            var output = new List<Lugar_Disposicion>();
            var items = await _context.Lugar_Disposicion.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Lugar_Disposicion();
                outputItem.Id_Lugar_Disposicion = item.Id_Lugar_Disposicion;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Nivel_Incidente>> GetListNivelIncidente()
        {
            var output = new List<Nivel_Incidente>();
            var items = await _context.Nivel_Incidente.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Nivel_Incidente();
                outputItem.Id_Nivel_Incidente = item.Id_Nivel_Incidente;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Producto_Incidente>> GetListProductoIncidente()
        {
            var output = new List<Producto_Incidente>();
            var items = await _context.Producto_Incidente.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Producto_Incidente();
                outputItem.Id_Producto_Incidente = item.Id_Producto_Incidente;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Tipo_Agua>> GetListTipoAgua()
        {
            var output = new List<Tipo_Agua>();
            var items = await _context.Tipo_Agua.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Tipo_Agua();
                outputItem.Id_Tipo_Agua = item.Id_Tipo_Agua;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Tipo_Incidente>> GetListTipoIncidente()
        {
            var output = new List<Tipo_Incidente>();
            var items = await _context.Tipo_Incidente.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Tipo_Incidente();
                outputItem.Id_Tipo_Incidente = item.Id_Tipo_Incidente;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Tipo_Producto_Incidente>> GetListTipoProducotIncidente()
        {
            var output = new List<Tipo_Producto_Incidente>();
            var items = await _context.Tipo_Producto_Incidente.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Tipo_Producto_Incidente();
                outputItem.Id_Tipo_Producto_Incidente = item.Id_Tipo_Producto_Incidente;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Tipo_Suelo>> GetListTipoSuelo()
        {
            var output = new List<Tipo_Suelo>();
            var items = await _context.Tipo_Suelo.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Tipo_Suelo();
                outputItem.Id_Tipo_Suelo = item.Id_Tipo_Suelo;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Unidad_Medida>> GetListUnidadMedida()
        {
            var output = new List<Unidad_Medida>();
            var items = await _context.Unidad_Medida.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Unidad_Medida();
                outputItem.Id_Unidad_Medida = item.Id_Unidad_Medida;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Tipo = item.Tipo;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<TipoCondicion>> GetListTipoCondicion()
        {
            var output = new List<TipoCondicion>();
            var items = await _context.Tipos_Condicion.ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new TipoCondicion();
                outputItem.IdTipoCondicion = item.Id_Tipo_Condicion;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Grupo = item.Grupo;
                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<Gerencia>> GetListGerencia()
        {
            var output = new List<Gerencia>();
            var items = await _context.Gerencias.ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Gerencia();
                outputItem.IdGerencia = item.Id_Gerencia;
                outputItem.Descripcion = item.Descripcion;
                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<Usuario>> GetListUsuarioMA()
        {
            var output = new List<Usuario>();
            var items = await _context.Usuarios.Where(x=>x.Id_Rol.Equals((int)InspeccionRol.AsesorMAP) && x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Usuario();
                outputItem.IdUsuario = item.Id_Usuario;
                outputItem.Nombres = item.Nombres;
                outputItem.Apellidos = item.Apellidos;
                outputItem.Email = item.Email;
                outputItem.IdRol = item.Id_Rol;
                outputItem.UsuarioRed = item.usuario_red;
                output.Add(outputItem);
            }
            return output;
        }
        public async Task<List<Usuario>> GetListUsuarios()
        {
            var output = new List<Usuario>();
            var items = await _context.Usuarios.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Usuario();
                outputItem.IdUsuario = item.Id_Usuario;
                outputItem.Nombres = item.Nombres;
                outputItem.Apellidos = item.Apellidos;
                outputItem.Email = item.Email;
                outputItem.IdRol = item.Id_Rol;
                outputItem.UsuarioRed = item.usuario_red;
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Factor_Causa_Incidente>> GetListFactores()
        {
            var output = new List<Factor_Causa_Incidente>();
            var items = await _context.Factor_Causa_Incidente.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            foreach (var item in items)
            {
                var outputItem = new Factor_Causa_Incidente();
                outputItem.Id_Factor_Causa_Incidente = item.Id_Factor_Causa_Incidente;
                outputItem.Tipo_Factor = item.Tipo_Factor;
                outputItem.Descripcion = item.Descripcion;
                outputItem.Estado = item.Estado;
                output.Add(outputItem);
            }
            return output;
        }
    }
}
