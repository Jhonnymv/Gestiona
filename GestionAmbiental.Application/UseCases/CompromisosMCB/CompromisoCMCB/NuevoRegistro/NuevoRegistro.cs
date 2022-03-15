using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB.NuevoRegistro
{
    public class NuevoRegistro : INuevoRegistro
    {
        private readonly ICompromisoReadOnlyRepository _compromisoReadOnlyRepository;
        public NuevoRegistro(ICompromisoReadOnlyRepository compromisoReadOnlyRepository)
        {
            _compromisoReadOnlyRepository = compromisoReadOnlyRepository ?? throw new ArgumentNullException(nameof(compromisoReadOnlyRepository));
        }
        public async Task<int> Execute(string Codigo, int Id_Criterio, string? Detalle_compromiso, int? Id_Etapa, string? Texto_compromiso, int? Id_Origen, string? Tomo, string? Capitulo,
            string? Seccion, string? Pagina, string? Resolucion, int? Id_Autoridad, DateTime? Fecha, int? Id_Compromiso, int? Id_Aspecto, int? Id_Componente, int? Id_Gerencia, int? Id_Area,
            string? Frecuencia, int? Id_Entidad, int? Id_Responsable1, int? Id_Responsable2, string? Frecuencia_reporte, string? Sustentos, string? Estado, int? Id_Aprueba, string? Medio_verificacion)
        {
            var input = new Domain.CompromisosMCB.CompromisoCMCB();

            input.Codigo = Codigo;
            if (Id_Criterio != 0)
            {
                input.CriterioCMCB = new Domain.CompromisosMCB.CriterioCMCB { Id = Id_Criterio };
            };
            if (Id_Etapa != null)
            {
                input.EtapaCMCB = new Domain.CompromisosMCB.EtapaCMCB { Id = (int)Id_Etapa };
            };
            if (Id_Origen != null)
            {
                input.OrigenCMCB = new Domain.CompromisosMCB.OrigenCMCB { Id = (int)Id_Origen };
            };
            if (Id_Autoridad != null)
            {
                input.AutoridadCMCB = new Domain.CompromisosMCB.AutoridadCMCB { Id = (int)Id_Autoridad };
            };
            if (Id_Aspecto != null)
            {
                input.AspectoCMCB = new Domain.CompromisosMCB.AspectoCMCB { Id = (int)Id_Aspecto };
            };
            if (Id_Componente != null)
            {
                input.ComponenteCMCB = new Domain.CompromisosMCB.ComponenteCMCB { Id = (int)Id_Componente };
            };
            if (Id_Gerencia != null)
            {
                input.GerenciaCMCB = new Domain.CompromisosMCB.GerenciaCMCB { Id = (int)Id_Gerencia };
            };
            if (Id_Area != null)
            {
                input.AreaCMCB = new Domain.CompromisosMCB.AreaCMCB { Id = (int)Id_Area };
            };
            if (Id_Responsable1 != null)
            {
                input.Responsable1 = new Domain.ValueObjects.Usuario { IdUsuario = (int)Id_Responsable1 };
            };
            if (Id_Responsable2 != null)
            {
                input.Responsable2 = new Domain.ValueObjects.Usuario { IdUsuario = (int)Id_Responsable2 };
            };

            input.Detalle_compromiso = Detalle_compromiso;
            input.Texto_compromiso = Texto_compromiso;
            input.Tomo = Tomo;
            input.Capitulo = Capitulo;
            input.Seccion = Seccion;
            input.Pagina = Pagina;
            input.Resolucion = Resolucion;
            input.Fecha = Convert.ToDateTime(Fecha);
            input.Frecuencia = Frecuencia;
            input.Frecuencia_reporte = Frecuencia_reporte;
            input.Sustentos = Sustentos;
            input.Estado = Estado;

            return await _compromisoReadOnlyRepository.RegistrarCompromiso(input);
        }
    }
}
