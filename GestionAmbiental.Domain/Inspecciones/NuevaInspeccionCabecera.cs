using System;
using System.Collections.Generic;
using System.Text;

namespace GestionAmbiental.Domain.Inspecciones
{
    public class NuevaInspeccionCabecera
    {
        public int Id_Cabecera { get; set; }
        public string Codigo { get; set; }
        public string Area { get; set; }
        public int Id_Gerencia_Ejecutora { get; set; }
        public int Id_Gerencia_EPCM { get; set; }
        public int Id_Empresa_Contratista { get; set; }
        public int Id_Empresa_Contratista_EPCM { get; set; }
        public DateTime Fecha_Inspeccion { get; set; }
        public string Lugar_Inspeccionado { get; set; }
        public int Id_Responsable_Lugar { get; set; }
        public int Id_Responsable_MA { get; set; }
        public string Inspectores { get; set; }
        public string Objetivo { get; set; }
        public string Estado { get; set; }

        public NuevaInspeccionCabecera(
            string codigo, string area, int id_Gerencia_Ejecutora, int id_Gerencia_EPCM, 
            int id_Empresa_Contratista, int id_Empresa_Contratista_EPCM, DateTime fecha_Inspeccion, 
            string lugar_Inspeccionado, int id_Responsable_Lugar, int id_Responsable_MA, string inspectores, string objetivo)
        {
            Codigo = codigo;
            Area = area;
            Id_Gerencia_Ejecutora = id_Gerencia_Ejecutora;
            Id_Gerencia_EPCM = id_Gerencia_EPCM;
            Id_Empresa_Contratista = id_Empresa_Contratista;
            Id_Empresa_Contratista_EPCM = id_Empresa_Contratista_EPCM;
            Fecha_Inspeccion = fecha_Inspeccion;
            Lugar_Inspeccionado = lugar_Inspeccionado;
            Id_Responsable_Lugar = id_Responsable_Lugar;
            Id_Responsable_MA = id_Responsable_MA;
            Inspectores = inspectores;
            Objetivo = objetivo;
        }
        public NuevaInspeccionCabecera(
            int id_Cabecera,string codigo, string area, int id_Gerencia_Ejecutora, int id_Gerencia_EPCM,
            int id_Empresa_Contratista, int id_Empresa_Contratista_EPCM, DateTime fecha_Inspeccion,
            string lugar_Inspeccionado, int id_Responsable_Lugar, int id_Responsable_MA, string inspectores, string objetivo)
        {
            Id_Cabecera = id_Cabecera;
            Codigo = codigo;
            Area = area;
            Id_Gerencia_Ejecutora = id_Gerencia_Ejecutora;
            Id_Gerencia_EPCM = id_Gerencia_EPCM;
            Id_Empresa_Contratista = id_Empresa_Contratista;
            Id_Empresa_Contratista_EPCM = id_Empresa_Contratista_EPCM;
            Fecha_Inspeccion = fecha_Inspeccion;
            Lugar_Inspeccionado = lugar_Inspeccionado;
            Id_Responsable_Lugar = id_Responsable_Lugar;
            Id_Responsable_MA = id_Responsable_MA;
            Inspectores = inspectores;
            Objetivo = objetivo;
        }

        public NuevaInspeccionCabecera(
            int id_Gerencia_Ejecutora, int id_Gerencia_EPCM, int id_Empresa_Contratista, int id_Empresa_Contratista_EPCM, 
            DateTime fecha_Inspeccion, string lugar_Inspeccionado, int id_Responsable_Lugar, int id_Responsable_MA, 
            string inspectores, string objetivo,string estado)
        {
            Id_Gerencia_Ejecutora = id_Gerencia_Ejecutora;
            Id_Gerencia_EPCM = id_Gerencia_EPCM;
            Id_Empresa_Contratista = id_Empresa_Contratista;
            Id_Empresa_Contratista_EPCM = id_Empresa_Contratista_EPCM;
            Fecha_Inspeccion = fecha_Inspeccion;
            Lugar_Inspeccionado = lugar_Inspeccionado;
            Id_Responsable_Lugar = id_Responsable_Lugar;
            Id_Responsable_MA = id_Responsable_MA;
            Inspectores = inspectores;
            Objetivo = objetivo;
            Estado = estado;
        }
        public NuevaInspeccionCabecera(
            int idCabecera,int id_Gerencia_Ejecutora, int id_Gerencia_EPCM, int id_Empresa_Contratista, int id_Empresa_Contratista_EPCM,
            DateTime fecha_Inspeccion, string lugar_Inspeccionado, int id_Responsable_Lugar, int id_Responsable_MA,
            string inspectores, string objetivo)
        {
            Id_Cabecera = idCabecera;
            Id_Gerencia_Ejecutora = id_Gerencia_Ejecutora;
            Id_Gerencia_EPCM = id_Gerencia_EPCM;
            Id_Empresa_Contratista = id_Empresa_Contratista;
            Id_Empresa_Contratista_EPCM = id_Empresa_Contratista_EPCM;
            Fecha_Inspeccion = fecha_Inspeccion;
            Lugar_Inspeccionado = lugar_Inspeccionado;
            Id_Responsable_Lugar = id_Responsable_Lugar;
            Id_Responsable_MA = id_Responsable_MA;
            Inspectores = inspectores;
            Objetivo = objetivo;
        }

        public NuevaInspeccionCabecera()
        {

        }
    }
}
