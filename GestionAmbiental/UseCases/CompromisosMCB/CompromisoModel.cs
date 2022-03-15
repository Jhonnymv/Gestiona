using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestionAmbiental.UseCases.CompromisosMCB
{
    public class CompromisoModel
    {
        public int Id { get; set; }
        [DisplayName("Código:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe ingresar el código")]
        public string Codigo { get; set; }

        [DisplayName("Criterio:")]
        public int Id_Criterio { get; set; }
        [DisplayName("Detalle:")]
        public string Detalle_compromiso { get; set; }
        [DisplayName("Etapa:")]
        public int? Id_Etapa { get; set; }
        [DisplayName("Texto:")]
        public string? Texto_compromiso { get; set; }
        [DisplayName("Origen:")]
        public int? Id_Origen { get; set; }
        [DisplayName("Tomo:")]
        public string? Tomo { get; set; }
        [DisplayName("Capitulo:")]
        public string? Capitulo { get; set; }
        [DisplayName("Seccion:")]
        public string? Seccion { get; set; }
        [DisplayName("Pagina:")]
        public string? Pagina { get; set; }
        [DisplayName("Resolucion:")]
        public string? Resolucion { get; set; }
        [DisplayName("Autoridad:")]
        public int? Id_Autoridad { get; set; }
        [DisplayName("Fecha:")]
        public DateTime Fecha { get; set; }
        [DisplayName("Compromiso:")]
        public int? Id_Compromiso { get; set; }
        [DisplayName("Aspecto:")]
        public int? Id_Aspecto { get; set; }
        [DisplayName("Componente:")]
        public int? Id_Componente { get; set; }
        [DisplayName("Gerencia:")]
        public int? Id_Gerencia { get; set; }
        [DisplayName("Area:")]
        public int? Id_Area { get; set; }
        [DisplayName("Frecuencia:")]
        public string? Frecuencia { get; set; }
        [DisplayName("Entidad:")]
        public int? Id_Entidad { get; set; }
        [DisplayName("Responsable 1:")]
        public int? Id_Responsable1 { get; set; }
        [DisplayName("Responsable 2:")]
        public int? Id_Responsable2 { get; set; }
        [DisplayName("Frecuencia:")]
        public string? Frecuencia_reporte { get; set; }
        [DisplayName("Sustento:")]
        public string? Sustentos { get; set; }
        [DisplayName("Estado:")]
        public string? Estado { get; set; }
        [DisplayName("Aprueba:")]
        public int? Id_Aprueba { get; set; }
        [DisplayName("Medio Verificación:")]
        public string? Medio_verificacion { get; set; }
    }
}
