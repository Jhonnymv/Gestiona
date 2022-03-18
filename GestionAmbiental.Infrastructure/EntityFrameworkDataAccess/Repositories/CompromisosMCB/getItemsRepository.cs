using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;
using GestionAmbiental.Domain.CompromisosMCB;
using GestionAmbiental.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class getItemsRepository : IGetItemsReadOnlyRepository
    {
        private readonly Context _context;
        public getItemsRepository(Context context)
        {
            _context = context;
        }
        public async Task<List<AreaCMCB>> GetAreas()
        {
            var ArearEntity = await _context.AreaCMCB.ToListAsync();
            var output = new List<AreaCMCB>();
            foreach (var item in ArearEntity)
            {
                output.Add(new AreaCMCB
                {
                    Id = item.Id,
                    Area = item.Area
                });
            }
            return output;
        }
        public async Task<List<AspectoCMCB>> GetAspectos()
        {
            var AspectoEntity = await _context.AspectoCMCB.ToListAsync();
            var ouput = new List<AspectoCMCB>();
            foreach (var item in AspectoEntity)
            {
                ouput.Add(new AspectoCMCB
                {
                    Id = item.Id,
                    Aspecto = item.Aspecto
                });
            }
            return ouput;
        }
        public async Task<List<AutoridadCMCB>> GetAutoridades()
        {
            var AutoridadEntity = await _context.AutoridadCMCB.ToListAsync();
            var ouput = new List<AutoridadCMCB>();
            foreach (var item in AutoridadEntity)
            {
                ouput.Add(new AutoridadCMCB
                {
                    Id = item.Id,
                    Autoridad = item.Autoridad,
                });
            }
            return ouput;
        }
        public async Task<List<ComponenteCMCB>> GetComponentes()
        {
            var ComponenteEntity = await _context.ComponenteCMCB.ToListAsync();
            var ouput = new List<ComponenteCMCB>();
            foreach (var item in ComponenteEntity)
            {
                ouput.Add(new ComponenteCMCB
                {
                    Id = item.Id,
                    Tipo=item.Tipo,
                    Componente=item.Componente,
                    Servicio=item.Servicio,
                });
            }
            return ouput;
        }
        public async Task<List<CriterioCMCB>> GetCriterios()
        {
            var CriterioEntity = await _context.CriterioCMCB.ToListAsync();
            var ouput = new List<CriterioCMCB>();
            foreach (var item in CriterioEntity)
            {
                ouput.Add(new CriterioCMCB
                {
                    Id = item.Id,
                    Criterio = item.Criterio,
                });
            }
            return ouput;
        }
        public async Task<List<EtapaCMCB>> GetEtapas()
        {
            var EtapaEntity = await _context.EtapaCMCB.ToListAsync();
            var ouput = new List<EtapaCMCB>();
            foreach (var item in EtapaEntity)
            {
                ouput.Add(new EtapaCMCB
                {
                    Id = item.Id,
                    Etapa = item.Etapa,
                });
            }
            return ouput;
        }
        public async Task<List<GerenciaCMCB>> GetGerencias()
        {
            var GerenciaEntity = await _context.GerenciaCMCB.ToListAsync();
            var ouput = new List<GerenciaCMCB>();
            foreach (var item in GerenciaEntity)
            {
                ouput.Add(new GerenciaCMCB
                {
                    Id = item.Id,
                    Gerencia = item.Gerencia,
                    Descripcion = item.Descripcion,
                });
            }
            return ouput;
        }

        public async Task<List<GerenciaUsuarioCMCB>> GetGerenciasUsuario()
        {
            var GerenciaUsuarioEntity = await _context.GerenciaUsuarioCMCB.ToListAsync();
            var ouput = new List<GerenciaUsuarioCMCB>();
            foreach (var item in GerenciaUsuarioEntity)
            {
                ouput.Add(new GerenciaUsuarioCMCB
                {
                    Id = item.Id,
                    Estado = item.Estado,
                   
                });
            }
            return ouput;
        }
        public async Task<List<MedioVerificacionCMCB>> GetMediosVerificacion()
        {
            var MedioVerificacionEntity = await _context.MedioVerificacionCMCB.ToListAsync();
            var ouput = new List<MedioVerificacionCMCB>();
            foreach (var item in MedioVerificacionEntity)
            {
                ouput.Add(new MedioVerificacionCMCB
                {
                    Id = item.Id,
                    Medio=item.Medio,
                });
            }
            return ouput;
        }

        public async Task<List<OrigenCMCB>> GetOrigenes()
        {
            var OrigenesEntity = await _context.OrigenCMCB.ToListAsync();
            var ouput = new List<OrigenCMCB>();
            foreach (var item in OrigenesEntity)
            {
                ouput.Add(new OrigenCMCB
                {
                    Id = item.Id,
                    Origen=item.Origen,
                });
            }
            return ouput;
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
    }
}
