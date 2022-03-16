using GestionAmbiental.Application.Repositories.CompromisosMCB.SustentoCMCB;
using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.CompromisosMCB.SustentoCMCB.EditarRegistro
{
    public class EditarRegistro : IEditarRegistro
    {
        private readonly ISustentoReadOnlyRepository _sustentoReadOnlyRepository;

        public EditarRegistro(ISustentoReadOnlyRepository sustentoReadOnlyRepository)
        {
            _sustentoReadOnlyRepository = sustentoReadOnlyRepository ?? throw new ArgumentNullException(nameof(sustentoReadOnlyRepository));
        }
        /*
         ,int Id_Compromiso
        ,string Fecha_limite
        ,string Fecha_entrega
        ,string Fecha_validacion
        ,int Id_Responsable1
        ,int Id_Responsable2
        ,int Id_Asesor
        ,string Estado
        ,string Sustento_cumplimiento
        ,string Detalle_compromiso
        ,string Evidencia
        ,string Comentario_implementacion
        ,string Comentario_verificacion
        ,string Medio_verificacion);
         */
        public async Task Execute(int Id, int Id_Compromiso, string Fecha_limite, string Fecha_entrega, string Fecha_validacion, int Id_Responsable1, int Id_Responsable2, int Id_Asesor, string Estado, string Sustento_cumplimiento, string Detalle_compromiso, string Evidencia, string Comentario_implementacion, string Comentario_verificacion, string Medio_verificacion)
        {
            var input = await _sustentoReadOnlyRepository.ObtenerSustento(Id);

            input.CompromisoCMCB = new Domain.CompromisosMCB.CompromisoCMCB { Id = Id_Compromiso};
            input.Responsable1 = new Domain.ValueObjects.Usuario { IdUsuario = Id_Responsable1};
            input.Responsable2 = new Domain.ValueObjects.Usuario { IdUsuario = Id_Responsable2};
            input.Asesor = new Domain.ValueObjects.Usuario { IdUsuario = Id_Asesor};
            input.Estado = Estado;
            input.Sustento_cumplimiento = Sustento_cumplimiento;
            input.Detalle_compromiso = Detalle_compromiso;
            input.Evidencia = Evidencia;
            input.Comentario_implementacion = Comentario_implementacion;
            input.Comentario_verificacion = Comentario_verificacion;
            input.Medio_verificacion = Medio_verificacion;
            
            if (Fecha_limite == null && Fecha_limite.Length > 0)
                input.Fecha_limite = Convert.ToDateTime(Fecha_limite);
            
            if (Fecha_entrega == null && Fecha_entrega.Length > 0)
                input.Fecha_entrega = Convert.ToDateTime(Fecha_entrega);

            if (Fecha_validacion == null && Fecha_validacion.Length > 0)
                input.Fecha_validacion = Convert.ToDateTime(Fecha_validacion);

            await _sustentoReadOnlyRepository.EditarSustento(input);
        }

        public async Task ExecuteImplementar(int Id, string Fecha_entrega, string Sustento_cumplimiento, string Evidencia, string Comentario_implementacion)
        {
            var input = await _sustentoReadOnlyRepository.ObtenerSustento(Id);

            input.Sustento_cumplimiento = Sustento_cumplimiento;
            input.Evidencia = Evidencia;
            input.Comentario_implementacion = Comentario_implementacion;
            input.Estado = EstadosSustento.Implementado.ToString();
            
            if (Fecha_entrega == null && Fecha_entrega.Length > 0)
                input.Fecha_entrega = Convert.ToDateTime(Fecha_entrega);
            
            await _sustentoReadOnlyRepository.EditarSustento(input);
        }

        public async Task ExecuteVerificar(int Id, int Id_Asesor, string Fecha_validacion, string Comentario_verificacion, string Medio_verificacion)
        {
            var input = await _sustentoReadOnlyRepository.ObtenerSustento(Id);

            input.Asesor = new Domain.ValueObjects.Usuario { IdUsuario = Id_Asesor };
            input.Comentario_verificacion = Comentario_verificacion;
            input.Medio_verificacion = Medio_verificacion;
            input.Estado = EstadosSustento.Verificado.ToString();

            if (Fecha_validacion == null && Fecha_validacion.Length > 0)
                input.Fecha_validacion = Convert.ToDateTime(Fecha_validacion);

            await _sustentoReadOnlyRepository.EditarSustento(input);
        }
    }
}
