using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Domain.CompromisosMCB;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        public async Task<List<Domain.CompromisosMCB.CompromisoCMCB>> ListarCompromisos(int[] gerencias, int[] areas, int[] criterios, string[] estados, int[] etapas)
        {
            //Consultar registros
            var compromisos = await _context.CompromisoCMCB.ToListAsync();

            if (gerencias.Length > 0)
            {
                compromisos = compromisos.Where(x => x.Id_Gerencia != null && gerencias.Contains(x.Id_Gerencia.Value)).ToList();
            }
            if (areas.Length > 0)
            {
                compromisos = compromisos.Where(x => x.Id_Area != null && areas.Contains(x.Id_Area.Value)).ToList();
            }
            if (criterios.Length > 0)
            {
                compromisos = compromisos.Where(x => x.Id_Criterio != null && criterios.Contains(x.Id_Criterio.Value)).ToList();
            }
            if (etapas.Length > 0)
            {
                compromisos = compromisos.Where(x => x.Id_Etapa != null && etapas.Contains(x.Id_Etapa.Value)).ToList();
            }
            if (estados.Length > 0)
            {
                compromisos = compromisos.Where(x => estados.Contains(x.Estado)).ToList();
            }

            //Nueva lista en blanco
            var output = new List<Domain.CompromisosMCB.CompromisoCMCB>();

            //var criterios = await _context.CriterioCMCB.ToListAsync();
            //Mapear registros en nueva lista
            foreach (var item in compromisos)
            {
                var dataCriterio = await _context.CriterioCMCB.FindAsync(item.Id_Criterio);
                var dataEtapa = await _context.EtapaCMCB.FindAsync(item.Id_Etapa);
                var dataOrigen = await _context.OrigenCMCB.FindAsync(item.Id_Origen);
                var dataAutoridad = await _context.AutoridadCMCB.FindAsync(item.Id_Autoridad);

                var dataAspecto = await _context.AspectoCMCB.FindAsync(item.Id_Aspecto);
                var dataComponente = await _context.ComponenteCMCB.FindAsync(item.Id_Componente);
                var dataGerencia = await _context.GerenciaCMCB.FindAsync(item.Id_Gerencia);
                var dataArea = await _context.AreaCMCB.FindAsync(item.Id_Area);
                var dataResp1 = await _context.Usuarios.FindAsync(item.Id_Responsable1);
                var dataResp2 = await _context.Usuarios.FindAsync(item.Id_Responsable2);
                var dataAprueba = await _context.Usuarios.FindAsync(item.Id_Aprueba);


                output.Add(new Domain.CompromisosMCB.CompromisoCMCB
                {
                    Id = item.Id,
                    Codigo = item.Codigo,
                    CriterioCMCB = item.Id_Criterio != null ? new Domain.CompromisosMCB.CriterioCMCB { Id = (int)item.Id_Criterio, Criterio = dataCriterio.Criterio, Detalle = dataCriterio.Detalle } : null,
                    Detalle_compromiso = item.Detalle_compromiso,
                    EtapaCMCB = item.Id_Etapa != null ? new Domain.CompromisosMCB.EtapaCMCB { Id = (int)item.Id_Etapa, Etapa = dataEtapa.Etapa } : null,
                    Texto_compromiso = item.Texto_compromiso,
                    OrigenCMCB = item.Id_Origen != null ? new Domain.CompromisosMCB.OrigenCMCB { Id = (int)item.Id_Origen, Origen = dataOrigen.Origen } : null,
                    Tomo = item.Tomo,
                    Capitulo = item.Capitulo,
                    Seccion = item.Seccion,
                    Pagina = item.Pagina,
                    Resolucion = item.Resolucion,
                    AutoridadCMCB = item.Id_Autoridad != null ? new Domain.CompromisosMCB.AutoridadCMCB { Id = (int)item.Id_Autoridad, Autoridad = dataAutoridad.Autoridad } : null,
                    Fecha = item.Fecha,

                    AspectoCMCB = item.Id_Aspecto != null ? new Domain.CompromisosMCB.AspectoCMCB { Id = (int)item.Id_Aspecto, Aspecto = dataAspecto.Aspecto } : null,
                    ComponenteCMCB = item.Id_Componente != null ? new Domain.CompromisosMCB.ComponenteCMCB { Id = (int)item.Id_Componente, Componente = dataComponente.Componente } : null,
                    GerenciaCMCB = item.Id_Gerencia != null ? new Domain.CompromisosMCB.GerenciaCMCB { Id = (int)item.Id_Gerencia, Gerencia = dataGerencia.Gerencia, Descripcion = dataGerencia.Descripcion } : null,
                    AreaCMCB = item.Id_Area != null ? new Domain.CompromisosMCB.AreaCMCB { Id = (int)item.Id_Area, Area = dataArea.Area } : null,
                    Frecuencia = item.Frecuencia,
                    Responsable1 = item.Id_Responsable1 != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)item.Id_Responsable1, Nombres = dataResp1.Nombres, DNI = dataResp1.DNI } : null,
                    Responsable2 = item.Id_Responsable2 != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)item.Id_Responsable2, Nombres = dataResp2.Nombres, DNI = dataResp2.DNI } : null,
                    Frecuencia_reporte = item.Frecuencia_reporte,
                    Sustentos = item.Sustentos,
                    Estado = item.Estado,
                    Aprueba = item.Id_Aprueba != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)item.Id_Responsable2, Nombres = dataAprueba.Nombres, DNI = dataAprueba.DNI } : null,
                    Medio_verificacion = item.Medio_verificacion
                });
            }
            //retornar nueva lista
            return output;
        }
        public async Task<Domain.CompromisosMCB.CompromisoCMCB> BuscarPorID(int Id)
        {
            //Consultar registros
            var data = await _context.CompromisoCMCB.FindAsync(Id);

            var dataCriterio = await _context.CriterioCMCB.FindAsync(data.Id_Criterio);
            var dataEtapa = await _context.EtapaCMCB.FindAsync(data.Id_Etapa);
            var dataOrigen = await _context.OrigenCMCB.FindAsync(data.Id_Origen);
            var dataAutoridad = await _context.AutoridadCMCB.FindAsync(data.Id_Autoridad);

            var dataAspecto = await _context.AspectoCMCB.FindAsync(data.Id_Aspecto);
            var dataComponente = await _context.ComponenteCMCB.FindAsync(data.Id_Componente);
            var dataGerencia = await _context.GerenciaCMCB.FindAsync(data.Id_Gerencia);
            var dataArea = await _context.AreaCMCB.FindAsync(data.Id_Area);
            var dataResp1 = await _context.Usuarios.FindAsync(data.Id_Responsable1);
            var dataResp2 = await _context.Usuarios.FindAsync(data.Id_Responsable2);
            var dataAprueba = await _context.Usuarios.FindAsync(data.Id_Aprueba);

            //Nueva lista en blanco
            var output = new Domain.CompromisosMCB.CompromisoCMCB
            {
                Id = Id,
                //Codigo = data.Codigo,
                //Detalle_compromiso = data.Detalle_compromiso,
                //Texto_compromiso = data.Texto_compromiso,
                //OrigenCMCB = data.Id_Origen != null ? new Domain.CompromisosMCB.OrigenCMCB { Id = (int)data.Id_Origen, Origen = dataOrigen.Origen } : null,
                //Tomo = data.Tomo,
                //Capitulo = data.Capitulo,
                //Seccion = data.Seccion,
                //Pagina = data.Pagina,
                //Resolucion = data.Resolucion,
                //AutoridadCMCB = data.Id_Autoridad != null ? new Domain.CompromisosMCB.AutoridadCMCB { Id = (int)data.Id_Autoridad, Autoridad = dataAutoridad.Autoridad } : null,
                //Fecha = data.Fecha,
                //Estado=data.Estado,
                //OrigenCMCB = new Domain.CompromisosMCB.OrigenCMCB { Id = (int)data.Id_Origen },
                Codigo = data.Codigo,
                CriterioCMCB = data.Id_Criterio != null ? new Domain.CompromisosMCB.CriterioCMCB { Id = (int)data.Id_Criterio, Criterio = dataCriterio.Criterio, Detalle = dataCriterio.Detalle } : null,
                Detalle_compromiso = data.Detalle_compromiso,
                EtapaCMCB = data.Id_Etapa != null ? new Domain.CompromisosMCB.EtapaCMCB { Id = (int)data.Id_Etapa, Etapa = dataEtapa.Etapa } : null,
                Texto_compromiso = data.Texto_compromiso,
                OrigenCMCB = data.Id_Origen != null ? new Domain.CompromisosMCB.OrigenCMCB { Id = (int)data.Id_Origen, Origen = dataOrigen.Origen } : null,
                Tomo = data.Tomo,
                Capitulo = data.Capitulo,
                Seccion = data.Seccion,
                Pagina = data.Pagina,
                Resolucion = data.Resolucion,
                AutoridadCMCB = data.Id_Autoridad != null ? new Domain.CompromisosMCB.AutoridadCMCB { Id = (int)data.Id_Autoridad, Autoridad = dataAutoridad.Autoridad } : null,
                Fecha = data.Fecha,

                AspectoCMCB = data.Id_Aspecto != null ? new Domain.CompromisosMCB.AspectoCMCB { Id = (int)data.Id_Aspecto, Aspecto = dataAspecto.Aspecto } : null,
                ComponenteCMCB = data.Id_Componente != null ? new Domain.CompromisosMCB.ComponenteCMCB { Id = (int)data.Id_Componente, Componente = dataComponente.Componente } : null,
                GerenciaCMCB = data.Id_Gerencia != null ? new Domain.CompromisosMCB.GerenciaCMCB { Id = (int)data.Id_Gerencia, Gerencia = dataGerencia.Gerencia, Descripcion = dataGerencia.Descripcion } : null,
                AreaCMCB = data.Id_Area != null ? new Domain.CompromisosMCB.AreaCMCB { Id = (int)data.Id_Area, Area = dataArea.Area } : null,
                Frecuencia = data.Frecuencia,
                Responsable1 = data.Id_Responsable1 != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)data.Id_Responsable1, Nombres = dataResp1.Nombres, DNI = dataResp1.DNI } : null,
                Responsable2 = data.Id_Responsable2 != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)data.Id_Responsable2, Nombres = dataResp2.Nombres, DNI = dataResp2.DNI } : null,
                Frecuencia_reporte = data.Frecuencia_reporte,
                Sustentos = data.Sustentos,
                Estado = data.Estado,
                Aprueba = data.Id_Aprueba != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)data.Id_Responsable2, Nombres = dataAprueba.Nombres, DNI = dataAprueba.DNI } : null,
                Medio_verificacion = data.Medio_verificacion
            };
            //retornar nueva lista
            return output;
        }

        public async Task EditarCompromiso(Domain.CompromisosMCB.CompromisoCMCB input)
        {
            var entity = await _context.CompromisoCMCB.FirstOrDefaultAsync(x => x.Id.Equals(input.Id));
            if (entity != null)
            {
                entity.Codigo = input.Codigo;
                entity.Detalle_compromiso = input.Detalle_compromiso;
                entity.Texto_compromiso = input.Texto_compromiso;
                entity.Tomo = input.Tomo;
                entity.Capitulo = input.Capitulo;
                entity.Seccion = input.Seccion;
                entity.Pagina = input.Pagina;
                entity.Resolucion = input.Resolucion;
                entity.Fecha = input.Fecha;
                entity.Frecuencia = input.Frecuencia;
                entity.Frecuencia_reporte = input.Frecuencia_reporte;
                entity.Sustentos = input.Sustentos;
                entity.Estado = input.Estado;
            }

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

            _context.CompromisoCMCB.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Domain.CompromisosMCB.CompromisoCMCB> ObtenerCompromiso(int Id)
        {
            var data = await _context.CompromisoCMCB.FindAsync(Id);
            var dataCriterio = await _context.CriterioCMCB.FindAsync(data.Id_Criterio);
            var dataEtapa = await _context.EtapaCMCB.FindAsync(data.Id_Etapa);
            var dataOrigen = await _context.OrigenCMCB.FindAsync(data.Id_Origen);
            var dataAutoridad = await _context.AutoridadCMCB.FindAsync(data.Id_Autoridad);
            var dataAspectod = await _context.AspectoCMCB.FindAsync(data.Id_Aspecto);
            var dataComponente = await _context.ComponenteCMCB.FindAsync(data.Id_Componente);
            var dataGerencia = await _context.GerenciaCMCB.FindAsync(data.Id_Gerencia);
            var dataArea = await _context.AreaCMCB.FindAsync(data.Id_Area);
            var dataResponsable1 = await _context.Usuarios.FindAsync(data.Id_Responsable1);
            var dataResponsable2 = await _context.Usuarios.FindAsync(data.Id_Responsable2);
            var dataAprueba = await _context.Usuarios.FindAsync(data.Id_Aprueba);


            //Nueva lista en blanco
            var output = new Domain.CompromisosMCB.CompromisoCMCB
            {
                Id = Id,
                Codigo = data.Codigo,
                CriterioCMCB = data.Id_Criterio != null ? new Domain.CompromisosMCB.CriterioCMCB { Id = (int)data.Id_Criterio, Criterio = dataCriterio.Criterio, Detalle = dataCriterio.Detalle } : null,
                Detalle_compromiso = data.Detalle_compromiso,
                EtapaCMCB = data.Id_Etapa != null ? new Domain.CompromisosMCB.EtapaCMCB { Id = (int)data.Id_Etapa, Etapa = dataEtapa.Etapa } : null,
                Texto_compromiso = data.Texto_compromiso,
                OrigenCMCB = data.Id_Origen != null ? new Domain.CompromisosMCB.OrigenCMCB { Id = (int)data.Id_Origen, Origen = dataOrigen.Origen } : null,
                Tomo = data.Tomo,
                Capitulo = data.Capitulo,
                Seccion = data.Seccion,
                Pagina = data.Pagina,
                Resolucion = data.Resolucion,
                AutoridadCMCB = data.Id_Autoridad != null ? new Domain.CompromisosMCB.AutoridadCMCB { Id = (int)data.Id_Autoridad, Autoridad = dataAutoridad.Autoridad } : null,
                Fecha = data.Fecha,
                AspectoCMCB = data.Id_Aspecto != null ? new Domain.CompromisosMCB.AspectoCMCB { Id = (int)data.Id_Aspecto, Aspecto = dataAspectod.Aspecto } : null,
                ComponenteCMCB = data.Id_Componente != null ? new Domain.CompromisosMCB.ComponenteCMCB { Id = (int)data.Id_Componente, Componente = dataComponente.Componente, Tipo = dataComponente.Tipo, Servicio = dataComponente.Servicio } : null,
                GerenciaCMCB = data.Id_Gerencia != null ? new Domain.CompromisosMCB.GerenciaCMCB { Id = (int)data.Id_Gerencia, Gerencia = dataGerencia.Gerencia, Descripcion = dataGerencia.Descripcion } : null,
                AreaCMCB = data.Id_Area != null ? new Domain.CompromisosMCB.AreaCMCB { Id = (int)data.Id_Area, Area = dataArea.Area } : null,
                Frecuencia = data.Frecuencia,
                Responsable1 = data.Id_Responsable1 != null ? new Domain.ValueObjects.Usuario
                {
                    IdUsuario = dataResponsable1.Id_Usuario,
                    Nombres = dataResponsable1.Nombres,
                    Apellidos = dataResponsable1.Apellidos,
                    DNI = dataResponsable1.DNI,
                    UsuarioRed = dataResponsable1.usuario_red,
                    Puesto = dataResponsable1.Puesto,
                    Celular = dataResponsable1.Celular,
                    Email = dataResponsable1.Email,
                    IdRol = dataResponsable1.Id_Rol,
                    Estado = dataResponsable1.Estado
                } : null,
                Responsable2 = data.Id_Responsable2 != null ? new Domain.ValueObjects.Usuario
                {
                    IdUsuario = dataResponsable2.Id_Usuario,
                    Nombres = dataResponsable2.Nombres,
                    Apellidos = dataResponsable2.Apellidos,
                    DNI = dataResponsable2.DNI,
                    UsuarioRed = dataResponsable2.usuario_red,
                    Puesto = dataResponsable2.Puesto,
                    Celular = dataResponsable2.Celular,
                    Email = dataResponsable2.Email,
                    IdRol = dataResponsable2.Id_Rol,
                    Estado = dataResponsable2.Estado
                } : null,
                Frecuencia_reporte = data.Frecuencia_reporte,
                Sustentos = data.Sustentos,
                Estado = data.Estado,
                Aprueba = data.Id_Aprueba != null ? new Domain.ValueObjects.Usuario
                {
                    IdUsuario = dataResponsable2.Id_Usuario,
                    Nombres = dataResponsable2.Nombres,
                    Apellidos = dataResponsable2.Apellidos,
                    DNI = dataResponsable2.DNI,
                    UsuarioRed = dataResponsable2.usuario_red,
                    Puesto = dataResponsable2.Puesto,
                    Celular = dataResponsable2.Celular,
                    Email = dataResponsable2.Email,
                    IdRol = dataResponsable2.Id_Rol,
                    Estado = dataResponsable2.Estado
                } : null,
                Medio_verificacion = data.Medio_verificacion,

                //OrigenCMCB = new Domain.CompromisosMCB.OrigenCMCB { Id = (int)data.Id_Origen },
            };
            //retornar nueva lista
            return output;
        }

    }
}
