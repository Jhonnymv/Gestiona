using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GestionAmbiental.UseCases.MonitoreoCampoInterno.AvistamientoFlora
{
    public class AvistamientoFloraModel
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Cod. Ficha:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Idficha { get; set; }
        [DisplayName("Fecha:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Fechavalidación { get; set; }
        public int IdZona { get; set; }
        [DisplayName("Ubicación Este:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Ubicacion_Este { get; set; }
        [DisplayName("Ubicación Norte:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Ubicacion_Norte { get; set; }
        [DisplayName("Altitud:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Ubicacion_Altitud { get; set; }


        public List<int> NombreCantidad { get; set; }
        public List<int> NombreId { get; set; }


      
        [DisplayName("Geneto:")]
        public string Geneto { get; set; }
        [DisplayName("Rameto:")]
        public string Rameto { get; set; }
        [DisplayName("Condición de reubicación:")]
        public string CondicionReubicacion { get; set; }
        [DisplayName("Sustrato")]
        public string Sustrato { get; set; }
        [DisplayName("Diámetro Mayor")]
        public string CoberturaDiametroMayor { get; set; }
        [DisplayName("Diámetro Menor")]
        public string CoberturaDiametroMenor { get; set; }
        [DisplayName("Altura")]
        public string CoberturaDiametroAltura { get; set; }

        public List<int> FenologiaCantidad { get; set; }
        public List<int> FenologiaId { get; set; }

        public List <int> VigorCantidad { get; set; }
        public List<int> VigorId { get; set; }


        public List<int> SaludCantidad { get; set; }
        public List<int> SaludId { get; set; }

        public List<int> UsoCantidad { get; set; }
        public List<int> UsoId { get; set; }

       
        public List<int> FaunaAsociadaId { get; set; }
        public List<int> FaunaAsociadaCantidad { get; set; }
        public int IdAvistamientoFlora { get; set; }
        public string DesAvistamientoFlora { get; set; }
        public string Opcion { get; set; }

        public List<IFormFile> Fotos { get; set; }

    }
}
