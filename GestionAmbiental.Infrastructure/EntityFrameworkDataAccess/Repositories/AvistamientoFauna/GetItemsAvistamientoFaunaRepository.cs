using GestionAmbiental.Application.Repositories.AvistamientoFauna;
using GestionAmbiental.Domain.AvistamientoFauna.Items;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.AvistamientoFauna
{
    public class GetItemsAvistamientoFaunaRepository : IGetItemsAvistamientoFaunaReadOnlyRepository
    {
        private Context _context;

        public GetItemsAvistamientoFaunaRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<AreaTrabajo>> GetAreasTrabajo()
        {
            var areas = await _context.Areas_Trabajo.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<AreaTrabajo>();
            foreach (var item in areas)
            {
                var outputItem = new AreaTrabajo
                {
                    IdAreaTrabajo = item.Id_Area_Trabajo,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<ContactoEmergencia>> GetContactosEmergencia()
        {
            var contactos = await _context.Contactos_Emergencia_AF.ToListAsync();
            var output = new List<ContactoEmergencia>();
            foreach (var item in contactos)
            {
                var outputItem = new ContactoEmergencia
                {
                    IdContactoEmergencia = item.Id_Contacto_Emergencia,
                    Nombres = item.Nombres,
                    Apellidos = item.Apellidos,
                    Telefono = item.Telefono,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Empresa>> GetEmpresas()
        {
            var empresas = await _context.Empresas.ToListAsync();
            var output = new List<Empresa>();
            foreach (var item in empresas)
            {
                var outputItem = new Empresa
                {
                    IdEmpresa = item.Id_Empresa,
                    RazonSocial = item.Razon_Social,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<EstadoFauna>> GetEstadosFauna()
        {
            var estados = await _context.Estados_Fauna.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<EstadoFauna>();
            foreach (var item in estados)
            {
                var outputItem = new EstadoFauna
                {
                    IdEstadoFauna = item.Id_Estado_Fauna,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<LugarReferencia>> GetLugaresReferencia()
        {
            var lugares = await _context.Lugares_Referencia_AF.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<LugarReferencia>();
            foreach (var item in lugares)
            {
                var outputItem = new LugarReferencia
                {
                    IdLugarReferencia = item.Id_Lugar_Referencia,
                    Descripcion = item.Descripcion,
                    Latitud = item.Latitud,
                    Longitud = item.Longitud,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<NombreComunFauna>> GetNombresComunesFauna()
        {
            var nombresComunes = await _context.Nombres_Comunes_Fauna.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var tiposFauna = await GetTiposFauna();
            var output = new List<NombreComunFauna>();
            foreach (var item in nombresComunes)
            {
                var tipoFauna = tiposFauna.FirstOrDefault(x => x.IdTipoFauna.Equals(item.Id_Tipo_Fauna));
                var outputItem = new NombreComunFauna
                {
                    IdNombreComunFauna = item.Id_Nombre_Comun_Fauna,
                    Foto = item.Foto,
                    Descripcion = item.Descripcion,
                    TipoFauna = tipoFauna,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<TipoAvistamiento>> GetTiposAvistamiento()
        {
            var tiposAvistamiento = await _context.Tipos_Avistamiento.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<TipoAvistamiento>();
            foreach (var item in tiposAvistamiento)
            {
                var outputItem = new TipoAvistamiento
                {
                    IdTipoAvistamiento = item.Id_Tipo_Avistamiento,
                    Descripcion = item.Descripcion,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<TipoFauna>> GetTiposFauna()
        {
            var tiposFauna = await _context.Tipos_Fauna.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<TipoFauna>();
            foreach (var item in tiposFauna)
            {
                var outputItem = new TipoFauna
                {
                    IdTipoFauna = item.Id_Tipo_Fauna,
                    Descripcion = item.Descripcion,
                    Foto = item.Foto,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.Where(x => x.Estado.Equals("ACT")).ToListAsync();
            var output = new List<Usuario>();
            foreach (var item in usuarios)
            {
                var outputItem = new Usuario
                {
                    IdUsuario = item.Id_Usuario,
                    Nombres = item.Nombres,
                    Apellidos = item.Apellidos,
                    Email = item.Email,
                    IdRol = item.Id_Rol,
                    IdSuperior = item.Id_Superior,
                    UsuarioRed = item.usuario_red,
                    Estado = item.Estado
                };
                output.Add(outputItem);
            }
            return output;
        }
    }
}
