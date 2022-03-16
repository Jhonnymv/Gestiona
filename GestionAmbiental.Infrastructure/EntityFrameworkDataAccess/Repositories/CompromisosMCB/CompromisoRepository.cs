using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;
using Microsoft.EntityFrameworkCore;

namespace GestionAmbiental.Infrastructure.EntityFrameworkDataAccess.Repositories.CompromisosMCB
{
    public class CompromisoRepository : ICompromisoReadOnlyRepository
    {
        private readonly Context _context;
        public CompromisoRepository(Context context)
        {
            _context = context;
        }
        //return ID nuevo registro
        public async Task<int> RegistrarCompromiso(Domain.CompromisosMCB.CompromisoCMCB input)
        {
            var entity = new Entities.CompromisosMCB.CompromisoCMCB
            {
                Codigo = input.Codigo,
                Detalle_compromiso = input.Detalle_compromiso,
                Texto_compromiso = input.Texto_compromiso,
                Tomo = input.Tomo,
                Capitulo = input.Capitulo,
                Seccion = input.Seccion,
                Pagina = input.Pagina,
                Resolucion = input.Resolucion,
                Fecha = input.Fecha,
                Frecuencia = input.Frecuencia,
                Frecuencia_reporte = input.Frecuencia_reporte,
                Sustentos = input.Sustentos,
                Estado = input.Estado,

            };
            if (input.CriterioCMCB != null)
            {
                entity.Id_Criterio = input.CriterioCMCB.Id;
            };
            if (input.EtapaCMCB != null)
            {
                entity.Id_Etapa = input.EtapaCMCB.Id;
            };
            if (input.OrigenCMCB != null)
            {
                entity.Id_Origen = input.OrigenCMCB.Id;
            };
            if (input.AutoridadCMCB != null)
            {
                entity.Id_Autoridad = input.AutoridadCMCB.Id;
            };
            if (input.AspectoCMCB != null)
            {
                entity.Id_Aspecto = input.AspectoCMCB.Id;
            };
            if (input.ComponenteCMCB != null)
            {
                entity.Id_Componente = input.ComponenteCMCB.Id;
            };
            if (input.GerenciaCMCB != null)
            {
                entity.Id_Gerencia = input.GerenciaCMCB.Id;
            };
            if (input.AreaCMCB != null)
            {
                entity.Id_Area = input.AreaCMCB.Id;
            };
            if (input.Responsable1 != null)
            {
                entity.Id_Responsable1 = input.Responsable1.IdUsuario;
            };
            if (input.Responsable2 != null)
            {
                entity.Id_Responsable2 = input.Responsable2.IdUsuario;
            };

            await _context.CompromisoCMCB.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        //Return lista de registros filtrados
        public async Task<List<Domain.CompromisosMCB.CompromisoCMCB>> ListarCompromisos()
        {
            //Consultar registros
            var filtrados = await _context.CompromisoCMCB.ToListAsync();

            //Nueva lista en blanco
            var output = new List<Domain.CompromisosMCB.CompromisoCMCB>();

            var criterios = await _context.CriterioCMCB.ToListAsync();
            //Mapear registros en nueva lista
            foreach (var item in filtrados)
            {
                var dataCriterio = await _context.CriterioCMCB.FindAsync(item.Id_Criterio);
                var dataEtapa = await _context.EtapaCMCB.FindAsync(item.Id_Etapa);
                var dataAspecto = await _context.AspectoCMCB.FindAsync(item.Id_Aspecto);
                var dataComponente = await _context.ComponenteCMCB.FindAsync(item.Id_Componente);
                var dataGerencia = await _context.GerenciaCMCB.FindAsync(item.Id_Gerencia);
                var dataArea = await _context.AreaCMCB.FindAsync(item.Id_Area);
                var dataResp1 = await _context.Usuarios.FindAsync(item.Id_Responsable1);
                var dataResp2 = await _context.Usuarios.FindAsync(item.Id_Responsable2);

                output.Add(new Domain.CompromisosMCB.CompromisoCMCB
                {
                    Id = item.Id,
                    Codigo = item.Codigo,
                    CriterioCMCB = item.Id_Criterio != null ? new Domain.CompromisosMCB.CriterioCMCB { Id = (int)item.Id_Criterio, Criterio = dataCriterio.Criterio, Detalle = dataCriterio.Detalle } : null,
                    Detalle_compromiso = item.Detalle_compromiso,
                    EtapaCMCB = item.Id_Etapa != null ? new Domain.CompromisosMCB.EtapaCMCB { Id = (int)item.Id_Etapa, Etapa = dataEtapa.Etapa } : null,
                    AspectoCMCB = item.Id_Aspecto != null ? new Domain.CompromisosMCB.AspectoCMCB { Id = (int)item.Id_Aspecto, Aspecto = dataAspecto.Aspecto } : null,
                    ComponenteCMCB = item.Id_Componente != null ? new Domain.CompromisosMCB.ComponenteCMCB { Id = (int)item.Id_Componente, Componente = dataComponente.Componente } : null,
                    GerenciaCMCB = item.Id_Gerencia != null ? new Domain.CompromisosMCB.GerenciaCMCB { Id = (int)item.Id_Gerencia, Gerencia = dataGerencia.Gerencia, Descripcion = dataGerencia.Descripcion } : null,
                    AreaCMCB = item.Id_Area != null ? new Domain.CompromisosMCB.AreaCMCB { Id = (int)item.Id_Area, Area = dataArea.Area } : null,
                    Frecuencia = item.Frecuencia,
                    Responsable1 = item.Id_Responsable1 != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)item.Id_Responsable1, Nombres = dataResp1.Nombres, DNI=dataResp1.DNI } : null,
                    Responsable2 = item.Id_Responsable2 != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)item.Id_Responsable2, Nombres = dataResp2.Nombres, DNI=dataResp2.DNI } : null,
                    Frecuencia_reporte=item.Frecuencia_reporte,
                    Sustentos = item.Sustentos,
                    Estado= item.Estado,
                }) ;
            }
            //retornar nueva lista
            return output;
        }
        public async Task<Domain.CompromisosMCB.CompromisoCMCB> BuscarPorID(int Id)
        {
            //Consultar registros
            var data = await _context.CompromisoCMCB.FindAsync(Id);
            var dataOrigen = await _context.OrigenCMCB.FindAsync(data.Id_Origen);
            var dataAutoridad = await _context.AutoridadCMCB.FindAsync(data.Id_Autoridad);
            //Nueva lista en blanco
            var output = new Domain.CompromisosMCB.CompromisoCMCB
            {
                Id = Id,
                Codigo = data.Codigo,
                Detalle_compromiso = data.Detalle_compromiso,
                Texto_compromiso = data.Texto_compromiso,
                OrigenCMCB = data.Id_Origen != null ? new Domain.CompromisosMCB.OrigenCMCB { Id = (int)data.Id_Origen, Origen = dataOrigen.Origen } : null,
                Tomo= data.Tomo,
                Capitulo= data.Capitulo,
                Seccion= data.Seccion,
                Pagina= data.Pagina,
                Resolucion= data.Resolucion,
                AutoridadCMCB = data.Id_Autoridad != null ? new Domain.CompromisosMCB.AutoridadCMCB { Id = (int)data.Id_Autoridad, Autoridad = dataAutoridad.Autoridad } : null,
                Fecha=data.Fecha,
                //OrigenCMCB = new Domain.CompromisosMCB.OrigenCMCB { Id = (int)data.Id_Origen },
            };
            //retornar nueva lista
            return output;
        }
    }
}
