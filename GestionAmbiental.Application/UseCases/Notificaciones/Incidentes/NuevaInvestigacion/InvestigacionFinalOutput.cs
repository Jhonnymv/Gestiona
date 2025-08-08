using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Application.UseCases.Notificaciones.Incidentes.NuevaInvestigacion
{
    public class InvestigacionFinalOutput
    {
        public int? Id_Investigacion_Final { get; set; }
        public int? Id_Reporte_Preliminar { get; set; }
        public string Codigo { get; set; }
        //Titular minero
        public int? Num_Trabajadores { get; set; }
        //Contratista - opcional
        public string Razon_Social_Contratista { get; set; }
        public string RUC_Contratista { get; set; }
        public string Actividad_Contratista { get; set; }
        public string Domicilio_Contratista { get; set; }
        public int? Num_Trabajadores_Contratista { get; set; }
        public string Razon_Social_Subcontratista { get; set; }
        public string RUC_Subcontratista { get; set; }
        public string Actividad_Subcontratista { get; set; }
        public string Domicilio_Subcontratista { get; set; }
        public int? Num_Trabajadores_Subcontratista { get; set; }
        //Info Incidente
        public int? Id_Tipo_Incidente { get; set; }
        public string Tipo_Incidente { get; set; }
        public int? Id_Nivel_Incidente { get; set; }
        public string Nivel_Incidente { get; set; }
        public string Lugar_Incidente { get; set; }
        public string Coordenada_Este { get; set; }
        public string Coordenada_Norte { get; set; }
        public string Localidad { get; set; }
        public string Distrito { get; set; }
        public string Provincia { get; set; }
        public string Departamento { get; set; }
        public DateTime? Fecha_Incidente { get; set; }
        public DateTime? Fecha_Inicio_Investigacion { get; set; }
        public string Turno { get; set; }
        public string Descripcion_Incidente { get; set; }
        //Involucrados
        public int? Id_Gerencia_Responsable { get; set; }
        public string Gerencia_Responsable { get; set; }
        public int? Id_Supervisor_MA { get; set; }
        public string Supervisor_MA { get; set; }
        public string Supervisor_MA_Email { get; set; }
        public List<string> Evidencia_Papel { get; set; }
        public List<string> Evidencia_Persona { get; set; }
        public List<string> Evidencia_Posicion { get; set; }
        public List<string> Evidencia_Parte { get; set; }
        public string Estado { get; set; }

        public InvestigacionFinalOutput(Domain.Incidentes.InvestigacionFinal.Investigacion_Final_Incidente investigacion)
        {
            Id_Investigacion_Final = investigacion.Id_Investigacion_Final;
            Id_Reporte_Preliminar = investigacion.Id_Reporte_Preliminar;
            Codigo = investigacion.Codigo;
            Num_Trabajadores = investigacion.Num_Trabajadores;

            Razon_Social_Contratista = investigacion.Razon_Social_Contratista;
            RUC_Contratista = investigacion.RUC_Contratista;
            Actividad_Contratista = investigacion.Actividad_Contratista;
            Domicilio_Contratista = investigacion.Domicilio_Contratista;
            Num_Trabajadores_Contratista = investigacion.Num_Trabajadores_Contratista;
            Razon_Social_Subcontratista = investigacion.Razon_Social_Subcontratista;
            RUC_Subcontratista = investigacion.RUC_Subcontratista;
            Actividad_Subcontratista = investigacion.Actividad_Subcontratista;
            Domicilio_Subcontratista = investigacion.Domicilio_Subcontratista;
            Num_Trabajadores_Subcontratista = investigacion.Num_Trabajadores_Subcontratista;

            Id_Tipo_Incidente = investigacion.Tipo_Incidente?.Id_Tipo_Incidente;
            Tipo_Incidente = investigacion.Tipo_Incidente?.Descripcion;
            Id_Nivel_Incidente = investigacion.Nivel_Incidente?.Id_Nivel_Incidente;
            Nivel_Incidente = investigacion.Nivel_Incidente?.Descripcion;
            Lugar_Incidente = investigacion.Lugar_Incidente;
            Coordenada_Este = investigacion.Coordenada_Este;
            Coordenada_Norte = investigacion.Coordenada_Norte;
            Localidad = investigacion.Localidad;
            Distrito = investigacion.Distrito;
            Provincia = investigacion.Provincia;
            Departamento = investigacion.Departamento;
            Fecha_Incidente = investigacion.Fecha_Incidente;
            Fecha_Inicio_Investigacion = investigacion.Fecha_Inicio_Investigacion;
            Turno = investigacion.Turno;
            Descripcion_Incidente = investigacion.Descripcion_Incidente;

            Id_Gerencia_Responsable = investigacion.Gerencia_Responsable?.IdGerencia;
            Gerencia_Responsable = investigacion.Gerencia_Responsable?.Descripcion;
            Id_Supervisor_MA = investigacion.Supervisor_MA?.IdUsuario;
            Supervisor_MA = investigacion.Supervisor_MA != null ? investigacion.Supervisor_MA.Nombres + " " + investigacion.Supervisor_MA.Apellidos : null;
            Supervisor_MA_Email = investigacion.Supervisor_MA != null ? investigacion.Supervisor_MA.Email : null;
            //Papel
            if (investigacion.Evidencia_Papel != null)
            {
                var evidencias = JArray.Parse(investigacion.Evidencia_Papel);
                Evidencia_Papel = new List<string>();
                foreach (var evidencia in evidencias)
                {
                    Evidencia_Papel.Add((string)evidencia);
                }
            }
            //Persona
            if (investigacion.Evidencia_Persona != null)
            {
                var evidencias = JArray.Parse(investigacion.Evidencia_Persona);
                Evidencia_Persona = new List<string>();
                foreach (var evidencia in evidencias)
                {
                    Evidencia_Persona.Add((string)evidencia);
                }
            }
            //Posicion
            if (investigacion.Evidencia_Posicion != null)
            {
                var evidencias = JArray.Parse(investigacion.Evidencia_Posicion);
                Evidencia_Posicion = new List<string>();
                foreach (var evidencia in evidencias)
                {
                    Evidencia_Posicion.Add((string)evidencia);
                }
            }
            //Parte
            if (investigacion.Evidencia_Parte != null)
            {
                var evidencias = JArray.Parse(investigacion.Evidencia_Parte);
                Evidencia_Parte = new List<string>();
                foreach (var evidencia in evidencias)
                {
                    Evidencia_Parte.Add((string)evidencia);
                }
            }

            Estado = investigacion.Estado;
        }
    }
}
