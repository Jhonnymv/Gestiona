using GestionAmbiental.Application.Repositories.MonitoreoCampoInterno.AvistamientoFlora.AvistamientoFlora;
using GestionAmbiental.Domain.CompromisosMCB;
using GestionAmbiental.Domain.MonitoreoCampoInterno.AvistamientoFlora;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.MonitoreoCampoInterno.AvistamientoFlora
{
    public class getItemsRepository : IGetItemsReadOnlyRepository
    {
        private readonly Context _context;
        public getItemsRepository(Context context)
        {
            _context = context;
        }    

        public async Task<List<FaunaAsociada>> GetfaunasAsociadas()
        {
            var faunasAsociadasEntity = await _context.FaunaAsociadaMCIF.ToListAsync();
            var output = new List<FaunaAsociada>();
            foreach (var item in faunasAsociadasEntity)
            {
                output.Add(new FaunaAsociada
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion= item.Descripcion
                });
            }
            return output;
        }

        public async Task<List<Fenologia>> GetFenologias()
        {
            var Entity = await _context.FenologiaMCIF.ToListAsync();
            var output = new List<Fenologia>();
            foreach (var item in Entity)
            {
                output.Add(new Fenologia
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion
                });
            }
            return output;
        }

        public async Task<List<NombreCientifico>> GetNombrescientificos()
        {
            var Entity = await _context.NombreFientificoMCIF.ToListAsync();
            var output = new List<NombreCientifico>();
            foreach (var item in Entity)
            {
                output.Add(new NombreCientifico
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    NombreComun=item.NombreComun,
                    Descripcion = item.Descripcion
                });
            }
            return output;
        }

        
        public async Task<List<Salud>> GetSaluds()
        {
            var Entity = await _context.SaludMCIF.ToListAsync();
            var output = new List<Salud>();
            foreach (var item in Entity)
            {
                output.Add(new Salud
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion
                });
            }
            return output;
        }

        public async Task<List<Uso>> GetUsos()
        {
            var Entity = await _context.UsoMCIF.ToListAsync();
            var output = new List<Uso>();
            foreach (var item in Entity)
            {
                output.Add(new Uso
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion
                });
            }
            return output;
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            var UsuarioEntity = await _context.Usuarios.ToListAsync();
            var ouput = new List<Usuario>();
            foreach (var item in UsuarioEntity)
            {
                ouput.Add(new Usuario
                {
                    IdUsuario = item.Id_Usuario,
                    Nombres=item.Nombres+" "+item.Apellidos +" - "+item.DNI,
                });
            }
            return ouput;
        }

        public async Task<List<Vigor>> GetVigores()
        {
            var Entity = await _context.VigorMCIF.ToListAsync();
            var output = new List<Vigor>();
            foreach (var item in Entity)
            {
                output.Add(new Vigor
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion
                });
            }
            return output;
        }

        public async Task<List<Zona>> GetZonas()
        {
            var Entity = await _context.ZonaMCIF.ToListAsync();
            var output = new List<Zona>();
            foreach (var item in Entity)
            {
                output.Add(new Zona
                {
                    Id = item.Id,
                    Nombre = item.Nombre,
                    Descripcion = item.Descripcion
                });
            }
            return output;
        }
    }
}
