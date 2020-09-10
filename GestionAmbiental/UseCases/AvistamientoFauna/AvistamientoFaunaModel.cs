using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.AvistamientoFauna
{
    public class AvistamientoFaunaModel
    {
        public int IdAvistamientoFauna { get; set; }
        public string IdEncriptado { get; set; }
        public string Codigo { get; set; }
        public string FotosCargadas { get; set; }
        public List<IFormFile> Fotos { get; set; }
        [DisplayName("Fecha:")]
        public string Fecha_Avistamiento { get; set; }
        [DisplayName("Hora:")]
        public string Hora_Avistamiento { get; set; }
        [DisplayName("Ubicación:")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Debe completar este campo")]
        public string Ubicacion { get; set; }
        [DisplayName("Cantidad:")]
        public int Cantidad { get; set; }
        [DisplayName("Nombre Común:")]
        public int IdNombreComun { get; set; }
        [DisplayName("Tipo de Fauna:")]
        public int IdTipoFauna { get; set; }
        [DisplayName("Tipo de Avistamiento:")]
        public int IdTipoAvistamiento { get; set; }
        [DisplayName("Estado de Fauna:")]
        public int IdEstadoFauna { get; set; }
        [DisplayName("Lugar Referencial de Avistamiento:")]
        public int IdLugarReferencia { get; set; }
        [DisplayName("Empresa:")]
        public int IdEmpresa { get; set; }
        [DisplayName("Nombres y Apellidos:")]
        public string Nombres_Apellidos { get; set; }
        public int IdUsuario { get; set; }
        [DisplayName("Área de Trabajo:")]
        public int IdAreaTrabajo { get; set; }
        [DisplayName("DNI:")]
        public string DNI { get; set; }
        [DisplayName("Descripción de la acción:")]
        public string Descripcion_Accion { get; set; }
        [DisplayName("Evidencia:")]
        public List<IFormFile> Evidencia_Accion { get; set; }
        [DisplayName("Evidencia:")]
        public string Evidencia_Cargada { get; set; }
        public string Estado { get; set; }
        public string Opcion { get; set; }
    }
}
