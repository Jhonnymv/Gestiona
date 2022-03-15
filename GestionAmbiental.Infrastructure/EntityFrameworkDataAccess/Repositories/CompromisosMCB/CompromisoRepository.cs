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
                Texto_compromiso=input.Texto_compromiso,
                Tomo=input.Tomo,
                Capitulo=input.Capitulo,
                Seccion=input.Seccion,
                Pagina=input.Pagina,
                Resolucion=input.Resolucion,
                Fecha=input.Fecha,
                Frecuencia=input.Frecuencia,
                Frecuencia_reporte=input.Frecuencia_reporte,
                Sustentos=input.Sustentos,
                Estado=input.Estado,

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

            var criterios= await _context.CriterioCMCB.ToListAsync();
            //Mapear registros en nueva lista
            foreach (var item in filtrados)
            {
                var dataCriterio= await _context.CriterioCMCB.FindAsync(item.Id_Criterio);
                var dataEtapa= await _context.EtapaCMCB.FindAsync(item.Id_Etapa);
                //var dataAspecto= await _context.AspectoCMCB.FindAsync(item.Id_Aspecto);
                //var dataComponente= await _context.ComponenteCMCB.FindAsync(item.Id_Componente);
                //var dataGerencia= await _context.GerenciaCMCB.FindAsync(item.Id_Gerencia);
                //var dataArea= await _context.AreaCMCB.FindAsync(item.Id_Area);

                output.Add(new Domain.CompromisosMCB.CompromisoCMCB
                {
                    Id = item.Id,
                    Codigo=item.Codigo,
                    CriterioCMCB = new Domain.CompromisosMCB.CriterioCMCB { Id = (int)item.Id_Criterio,Criterio=dataCriterio.Criterio,Detalle=dataCriterio.Detalle },
                    Detalle_compromiso = item.Detalle_compromiso,
                    EtapaCMCB = new Domain.CompromisosMCB.EtapaCMCB { Id = (int)item.Id_Etapa, Etapa = dataEtapa.Etapa},
                    //AspectoCMCB=    new Domain.CompromisosMCB.AspectoCMCB { Id = (int)item.Id_Aspecto,Aspecto=dataAspecto.Aspecto},
                    //ComponenteCMCB = new Domain.CompromisosMCB.ComponenteCMCB { Id = (int)item.Id_Componente,Componente=dataComponente.Componente},
                    //GerenciaCMCB = new Domain.CompromisosMCB.GerenciaCMCB { Id=(int)item.Id_Gerencia,Gerencia=dataGerencia.Gerencia,Descripcion=dataGerencia.Descripcion},
                    //AreaCMCB=new Domain.CompromisosMCB.AreaCMCB { Id=(int)item.Id_Area,Area=dataArea.Area},
                    //Texto_compromiso = item.Texto_compromiso,
                });
            }
            //retornar nueva lista
            return output;
        }
        public async Task<Domain.CompromisosMCB.CompromisoCMCB> BuscarPorID(int Id)
        {
            //Consultar registros
            var data = await _context.CompromisoCMCB.FindAsync(Id);

            //Nueva lista en blanco
            var output = new Domain.CompromisosMCB.CompromisoCMCB
            {
                Id = Id,
                Codigo = data.Codigo,
                //CriterioCMCB = new Domain.CompromisosMCB.CriterioCMCB { Id = (int)data.Id_Criterio },
                Detalle_compromiso = data.Detalle_compromiso,
                //EtapaCMCB = new Domain.CompromisosMCB.EtapaCMCB { Id = (int)data.Id_Etapa },
                Texto_compromiso = data.Texto_compromiso,
                //OrigenCMCB = new Domain.CompromisosMCB.OrigenCMCB { Id = (int)data.Id_Origen },
            };
            //retornar nueva lista
            return output;
        }
    }
}
