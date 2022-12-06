using GestionAmbiental.Application.Repositories.CompromisosMCB.CompromisoCMCB;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.CompromisoCMCB.EditarRegistro
{
    public class EditarRegistro : IEditarRegistro
    {
        private readonly ICompromisoReadOnlyRepository _compromisoReadOnlyRepository;

        public EditarRegistro(ICompromisoReadOnlyRepository compromisoReadOnlyRepository)
        {
            _compromisoReadOnlyRepository = compromisoReadOnlyRepository ?? throw new ArgumentNullException(nameof(compromisoReadOnlyRepository));
        }
        public async Task Execute(int Id, string Codigo, int? Id_Criterio, string? Detalle_compromiso, int? Id_Etapa, string? Texto_compromiso, int? Id_Origen, string? Tomo, string? Capitulo,
            string? Seccion, string? Pagina, string? Resolucion, int? Id_Autoridad, DateTime? Fecha, int? Id_Compromiso, int? Id_Aspecto, int? Id_Componente, int? Id_Gerencia, int? Id_Area,
            string? Frecuencia, int? Id_Entidad, int? Id_Responsable1, int? Id_Responsable2, string? Frecuencia_reporte, string? Sustentos, string? Estado, int? Id_Aprueba, string? Medio_verificacion,
            string? Ano, string? Duplicado, int? Id_TipoCompromiso, string? Actividad, int? Id_ComponenteAux, int? Id_FactorAmbiental, string? Vigencia, string? Motivo, string? Comentario)
        {
            var input = await _compromisoReadOnlyRepository.ObtenerCompromiso(Id);

            input.Codigo = Codigo;
            input.CriterioCMCB = Id_Criterio != null ? new Domain.CompromisosMCB.CriterioCMCB { Id = (int)Id_Criterio } : null;
            input.Detalle_compromiso = Detalle_compromiso;
            input.EtapaCMCB = Id_Etapa != null ? new Domain.CompromisosMCB.EtapaCMCB { Id = (int)Id_Etapa } : null;
            input.Texto_compromiso = Texto_compromiso;
            input.OrigenCMCB = Id_Origen != null ? new Domain.CompromisosMCB.OrigenCMCB { Id = (int)Id_Origen } : null;
            input.Tomo = Tomo;
            input.Capitulo = Capitulo;
            input.Seccion = Seccion;
            input.Pagina = Pagina;
            input.Resolucion = Resolucion;
            input.AutoridadCMCB = Id_Autoridad != null ? new Domain.CompromisosMCB.AutoridadCMCB { Id = (int)Id_Autoridad } : null;
            input.Fecha = Convert.ToDateTime(Fecha);
            input.AspectoCMCB = Id_Aspecto != null ? new Domain.CompromisosMCB.AspectoCMCB { Id = (int)Id_Aspecto } : null;
            input.ComponenteCMCB = Id_Componente != null ? new Domain.CompromisosMCB.ComponenteCMCB { Id = (int)Id_Componente } : null;
            input.GerenciaCMCB = Id_Gerencia != null ? new Domain.CompromisosMCB.GerenciaCMCB { Id = (int)Id_Gerencia } : null;
            input.AreaCMCB = Id_Area != null ? new Domain.CompromisosMCB.AreaCMCB { Id = (int)Id_Area } : null;
            input.Frecuencia = Frecuencia;
            input.Responsable1 = Id_Responsable1 != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)Id_Responsable1 } : null;
            input.Responsable2 = Id_Responsable2 != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)Id_Responsable2 } : null;
            input.Frecuencia_reporte = Frecuencia_reporte;
            input.Sustentos = Sustentos;
            input.Estado = Estado;
            input.Aprueba = Id_Aprueba != null ? new Domain.ValueObjects.Usuario { IdUsuario = (int)Id_Aprueba } : null;
            input.Medio_verificacion = Medio_verificacion;

            input.Ano = Ano;
            input.Duplicado = Duplicado;

            if (Id_TipoCompromiso != null)
            {
                input.TipoCompromiso = new Domain.CompromisosMCB.TipoCompromisoCMCB { Id = (int)Id_TipoCompromiso };
            }
            input.Actividad = Actividad;
            if (Id_ComponenteAux != null)
            {
                input.ComponenteAux = new Domain.CompromisosMCB.ComponenteCMCB { Id = (int)Id_ComponenteAux };
            }
            if (Id_FactorAmbiental != null)
            {
                input.FactorAmbiental = new Domain.CompromisosMCB.FactorAmbientalCMCB { Id = (int)Id_FactorAmbiental };
            }
            input.Vigencia = Vigencia;
            input.Motivo = Motivo;
            input.Comentario = Comentario;

            await _compromisoReadOnlyRepository.EditarCompromiso(input);
        }

        //public async Task ExecuteImplementar(int Id, string Fecha_entrega, string Sustento_cumplimiento, string Evidencia, string Comentario_implementacion)
        //{
        //    var input = await _sustentoReadOnlyRepository.ObtenerSustento(Id);

        //    input.Sustento_cumplimiento = Sustento_cumplimiento;
        //    input.Evidencia = Evidencia;
        //    input.Comentario_implementacion = Comentario_implementacion;
        //    input.Estado = EstadosSustento.Implementado.ToString();

        //    if (Fecha_entrega == null && Fecha_entrega.Length > 0)
        //        input.Fecha_entrega = Convert.ToDateTime(Fecha_entrega);

        //    await _sustentoReadOnlyRepository.EditarSustento(input);
        //}

        //public async Task ExecuteVerificar(int Id, int Id_Asesor, string Fecha_validacion, string Comentario_verificacion, string Medio_verificacion)
        //{
        //    var input = await _sustentoReadOnlyRepository.ObtenerSustento(Id);

        //    input.Asesor = new Domain.ValueObjects.Usuario { IdUsuario = Id_Asesor };
        //    input.Comentario_verificacion = Comentario_verificacion;
        //    input.Medio_verificacion = Medio_verificacion;
        //    input.Estado = EstadosSustento.Verificado.ToString();

        //    if (Fecha_validacion == null && Fecha_validacion.Length > 0)
        //        input.Fecha_validacion = Convert.ToDateTime(Fecha_validacion);

        //    await _sustentoReadOnlyRepository.EditarSustento(input);
        //}
    }
}
