using GestionAmbiental.Application.Repositories.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Enum;
using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.EditarInvestigacionFinal
{
    public class EditarInvestigacionFinal : IEditarInvestigacionFinal
    {
        private readonly IEditarInvestigacionFinalReadOnlyRepository _editarInvestigacionFinalReadOnlyRepository;

        public EditarInvestigacionFinal(IEditarInvestigacionFinalReadOnlyRepository editarInvestigacionFinalReadOnlyRepository)
        {
            _editarInvestigacionFinalReadOnlyRepository = editarInvestigacionFinalReadOnlyRepository ?? throw new ArgumentNullException(nameof(editarInvestigacionFinalReadOnlyRepository));
        }

        public async Task<int> Execute(
            int Id_Investigacion_Final,
            int Num_Trabajadores,
            string Razon_Social_Contratista,
            string RUC_Contratista,
            string Actividad_Contratista,
            string Domicilio_Contratista,
            int? Num_Trabajadores_Contratista,
            string Razon_Social_SubContratista,
            string RUC_SubContratista,
            string Actividad_SubContratista,
            string Domicilio_SubContratista,
            int? Num_Trabajadores_SubContratista,
            int Id_Tipo_Incidente,
            int Id_Nivel_Incidente,
            string Lugar_Incidente,
            string Coordenada_Este,
            string Coordenada_Norte,
            string Localidad,
            string Distrito,
            string Provincia,
            string Departamento,
            string Fecha_Incidente,
            string Hora_Incidente,
            string Fecha_Inicio_Investigacion,
            string Turno,
            string Descripcion_Incidente,
            int Id_Gerencia_Responsable,
            int Id_Supervisor,
            List<string> Evidencias_Papel,
            List<string> Evidencias_Persona,
            List<string> Evidencias_Posicion,
            List<string> Evidencias_Partes,
            string Nombres_Archivos,
            int Id_Impacto_Ambiental_Incidente,
            int? Id_Tipo_Suelo,
            double? Area_Suelo,
            int? Id_Unidad_Area_Suelo,
            double? Cantidad_Suelo,
            int? Id_Unidad_Cantidad_Suelo,
            int? Id_Lugar_Disposicion_Suelo,
            int? Id_Especie_Flora,
            double? Unidad_Flora,
            int? Id_Unidad_Flora,
            double? Cantidad_Residuo_Flora,
            int? Id_Unidad_Residuo_Flora,
            int? Id_Lugar_Disposicion_Flora,
            int? Id_Especie_Fauna,
            double? Unidad_Fauna,
            int? Id_Unidad_Fauna,
            double? Cantidad_Residuo_Fauna,
            int? Id_Unidad_Residuo_Fauna,
            int? Id_Lugar_Disposicion_Fauna,
            int? Id_Tipo_Agua,
            double? Volumen_Agua,
            int? Id_Unidad_Agua,
            double? Cantidad_Residuo_Agua,
            int? Id_Unidad_Residuo_Agua,
            int? Id_Lugar_Disposicion_Agua,
            string Restos_Arq,
            string Nombre_Resto_Arq,
            string Cord_Este,
            string Cord_Norte,
            string Danio_Salud,
            int? Personas_Afectadas,
            double? Cantidad_RS,
            int? Id_Unidad_Cantidad_RS,
            double? Volumen_Rs,
            int? Id_Unidad_Volumen_RS,
            string EO_RS,
            string Opcion)
        {
            var input_investigacion = new Investigacion_Final_Incidente();
            var input_impacto = new Impacto_Ambiental_Incidente();

            input_investigacion.Id_Investigacion_Final = Id_Investigacion_Final;
            input_investigacion.Num_Trabajadores = Num_Trabajadores;
            input_investigacion.Razon_Social_Contratista = Razon_Social_Contratista;
            input_investigacion.RUC_Contratista = RUC_Contratista;
            input_investigacion.Actividad_Contratista = Actividad_Contratista;
            input_investigacion.Domicilio_Contratista = Domicilio_Contratista;
            input_investigacion.Num_Trabajadores_Contratista = Num_Trabajadores_Contratista;
            input_investigacion.Razon_Social_Subcontratista = Razon_Social_SubContratista;
            input_investigacion.RUC_Subcontratista = RUC_SubContratista;
            input_investigacion.Actividad_Subcontratista = Actividad_SubContratista;
            input_investigacion.Domicilio_Subcontratista = Domicilio_SubContratista;
            input_investigacion.Num_Trabajadores_Subcontratista = Num_Trabajadores_SubContratista;
            if (Id_Tipo_Incidente != 0)
                input_investigacion.Tipo_Incidente = new Domain.Incidentes.Items.Tipo_Incidente { Id_Tipo_Incidente = Id_Tipo_Incidente};
            if (Id_Nivel_Incidente != 0)
                input_investigacion.Nivel_Incidente = new Domain.Incidentes.Items.Nivel_Incidente { Id_Nivel_Incidente = Id_Nivel_Incidente };
            input_investigacion.Coordenada_Este = Coordenada_Este;
            input_investigacion.Coordenada_Norte = Coordenada_Norte;
            input_investigacion.Departamento = Departamento;
            input_investigacion.Localidad = Localidad;
            input_investigacion.Distrito = Distrito;
            input_investigacion.Provincia = Provincia;
            if(Fecha_Incidente != null && Hora_Incidente != null)
            input_investigacion.Fecha_Incidente = Convert.ToDateTime(Fecha_Incidente+" "+Hora_Incidente);
            if (Fecha_Inicio_Investigacion != null)
                input_investigacion.Fecha_Inicio_Investigacion = Convert.ToDateTime(Fecha_Inicio_Investigacion);
            input_investigacion.Turno = Turno;
            input_investigacion.Descripcion_Incidente = Descripcion_Incidente;
            if (Id_Gerencia_Responsable != 0)
                input_investigacion.Gerencia_Responsable = new Domain.ValueObjects.Gerencia { IdGerencia = Id_Gerencia_Responsable };
            if(Id_Supervisor!=0)
                input_investigacion.Supervisor_MA = new Domain.ValueObjects.Usuario { IdUsuario = Id_Supervisor};
            if(Evidencias_Papel!=null)
            input_investigacion.Evidencia_Papel = JsonConvert.SerializeObject(Evidencias_Papel);
            if (Evidencias_Papel != null)
                input_investigacion.Evidencia_Persona = JsonConvert.SerializeObject(Evidencias_Persona);
            if (Evidencias_Papel != null)
                input_investigacion.Evidencia_Posicion = JsonConvert.SerializeObject(Evidencias_Posicion);
            if (Evidencias_Papel != null)
                input_investigacion.Evidencia_Parte = JsonConvert.SerializeObject(Evidencias_Partes);
            input_investigacion.NombresArchivos = Nombres_Archivos;

            switch (Opcion)
            {
                case "Pendiente": input_investigacion.Estado = EstadosIncidentes.PendienteInvestigacion.ToString();break;
                case "Completar": input_investigacion.Estado = EstadosIncidentes.Completado.ToString(); break;
            }

            input_impacto.Id_Investigacion_Final = Id_Investigacion_Final;
            if (Id_Tipo_Suelo != 0)
                input_impacto.Tipo_Suelo =new Domain.Incidentes.Items.Tipo_Suelo { Id_Tipo_Suelo = Id_Tipo_Suelo };
            input_impacto.Area_Suelo = Area_Suelo;
            if (Id_Unidad_Area_Suelo != 0)
                input_impacto.Unidad_Area_Suelo = new Domain.Incidentes.Items.Unidad_Medida { Id_Unidad_Medida = Id_Unidad_Area_Suelo };
            input_impacto.Cantidad_Suelo = Cantidad_Suelo;
            if (Id_Unidad_Cantidad_Suelo != 0)
                input_impacto.Unidad_Cantidad_Suelo = new Domain.Incidentes.Items.Unidad_Medida { Id_Unidad_Medida = Id_Unidad_Cantidad_Suelo };
            if (Id_Lugar_Disposicion_Suelo != 0)
                input_impacto.Lugar_Disposicion_Suelo = new Lugar_Disposicion { Id_Lugar_Disposicion = Id_Lugar_Disposicion_Suelo };

            if (Id_Especie_Flora != 0)
                input_impacto.Especie_Flora = new Especie_Flora { Id_Especie_Flora = Id_Especie_Flora };            
            input_impacto.Unidad_Flora = Unidad_Flora;
            if (Id_Unidad_Flora != 0)
                input_impacto.UnidadM_Flora = new Unidad_Medida { Id_Unidad_Medida = Id_Unidad_Flora };
            input_impacto.Cantidad_Residuo_Flora = Cantidad_Residuo_Flora;
            if (Id_Unidad_Residuo_Flora != 0)
                input_impacto.Unidad_Residuo_Flora = new Unidad_Medida { Id_Unidad_Medida = Id_Unidad_Residuo_Flora };
            if (Id_Lugar_Disposicion_Flora != 0)
                input_impacto.Lugar_Disposicion_Flora = new Lugar_Disposicion { Id_Lugar_Disposicion = Id_Lugar_Disposicion_Flora };
            
            if (Id_Especie_Fauna != 0)
                input_impacto.Especie_Fauna = new Especie_Fauna { Id_Especie_Fauna = Id_Especie_Fauna };            
            input_impacto.Unidad_Fauna = Unidad_Fauna;
            if (Id_Unidad_Fauna != 0)
                input_impacto.UnidadM_Fauna = new Unidad_Medida { Id_Unidad_Medida = Id_Unidad_Fauna };
            input_impacto.Cantidad_Residuo_Fauna = Cantidad_Residuo_Fauna;
            if (Id_Unidad_Residuo_Fauna != 0)
                input_impacto.Unidad_Residuo_Fauna = new Unidad_Medida { Id_Unidad_Medida = Id_Unidad_Residuo_Fauna };
            if (Id_Lugar_Disposicion_Fauna != 0)
                input_impacto.Lugar_Disposicion_Fauna = new Lugar_Disposicion { Id_Lugar_Disposicion = Id_Lugar_Disposicion_Fauna };

            if (Id_Tipo_Agua != 0)
                input_impacto.Tipo_Agua = new Tipo_Agua { Id_Tipo_Agua = Id_Tipo_Agua };
            input_impacto.Volumen_Agua = Volumen_Agua;
            if (Id_Unidad_Agua != 0)
                input_impacto.Unidad_Agua = new Unidad_Medida { Id_Unidad_Medida = Id_Unidad_Agua };
            input_impacto.Cantidad_Residuo_Agua = Cantidad_Residuo_Agua;
            if (Id_Unidad_Residuo_Agua != 0)
                input_impacto.Unidad_Residuo_Agua = new Unidad_Medida { Id_Unidad_Medida = Id_Unidad_Residuo_Agua };
            if (Id_Lugar_Disposicion_Agua != 0)
                input_impacto.Lugar_Disposicion_Agua = new Lugar_Disposicion { Id_Lugar_Disposicion = Id_Lugar_Disposicion_Agua };
            
            input_impacto.Restos_Arq = Restos_Arq;
            input_impacto.Nombre_Resto_Arq = Nombre_Resto_Arq;
            input_impacto.Cord_Este = Cord_Este;
            input_impacto.Cord_Norte = Cord_Norte;
            
            input_impacto.Danio_Salud = Danio_Salud;
            input_impacto.Personas_Afectadas = Personas_Afectadas;
            input_impacto.Cantidad_RS = Cantidad_RS;
            if (Id_Unidad_Cantidad_RS != 0)
                input_impacto.Unidad_Cantidad_RS = new Unidad_Medida { Id_Unidad_Medida = Id_Unidad_Cantidad_RS };
            input_impacto.Volumen_Rs = Volumen_Rs;
            if (Id_Unidad_Volumen_RS != 0)
                input_impacto.Unidad_Volumen_RS = new Unidad_Medida { Id_Unidad_Medida = Id_Unidad_Volumen_RS };
            input_impacto.EO_RS = EO_RS;

            return await _editarInvestigacionFinalReadOnlyRepository.EditarInvestigacionFinal(input_investigacion, input_impacto);
        }
    }
}
