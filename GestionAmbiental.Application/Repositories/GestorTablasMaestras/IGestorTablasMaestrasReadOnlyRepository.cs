using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.Items;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.GestorTablasMaestras
{
    public interface IGestorTablasMaestrasReadOnlyRepository
    {
        //Tb_Usuario
        Task<List<Usuario>> GetUsuarios(string filtro);
        Task<Usuario> GetUsuario(int idUsuario);
        Task UpdateUsuario(Usuario usuario);
        //Task<int> AddUsuario(Usuario usuario);
        //Tb_Empresa
        Task<List<Empresa>> GetEmpresas(string filtro);
        Task<Empresa> GetEmpresa(int idEmpresa);
        Task UpdateEmpresa(Empresa empresa);
        Task<int> AddEmpresa(Empresa empresa);
        //Tb_Gerencia
        Task<List<Gerencia>> GetGerencias(string filtro);
        Task<Gerencia> GetGerencia(int idGerencia);
        Task UpdateGerencia(Gerencia gerencia);
        Task <int>AddGerencia(Gerencia gerencia);
        //Tb_TipoCondicion
        Task<List<TipoCondicion>> GetTiposCondicion(string filtro);
        Task<TipoCondicion> GetTipoCondicion(int idTipoCondicion);
        Task UpdateTipoCondicion(TipoCondicion tipoCondicion);
        Task <int>AddTipoCondicion(TipoCondicion tipoCondicion);
        //Tb_EO_RS
        Task<List<EO_RS>> GetEO_RS(string filtro);
        Task<EO_RS> GetEO_RS(int idEO_RS);
        Task UpdateEO_RS(EO_RS eors);
        Task<int> AddEO_RS(EO_RS eors);
        //Tb_ACR_CMRS
        Task<List<ACR_CMRS>> GetACR_CMRS(string filtro);
        Task<ACR_CMRS> GetACR_CMRS(int id_acr_cmrs);
        Task UpdateACR_CMRS(ACR_CMRS acr_cmrs);
        Task<int> AddACR_CMRS(ACR_CMRS acr_cmrs);
        //Tb_Conductor_RS
        Task<List<Conductor_RS>> GetConductor_RS(string filtro);
        Task<Conductor_RS> GetConductor_RS(int id_conductor);
        Task UpdateConductor_RS(Conductor_RS conductor_RS);
        Task<int> AddConductor_RS(Conductor_RS conductor_RS);
        //Tb_Motivo_Ingreso_Salida
        Task<List<Motivo_Ingreso_Salida>> GetMotivo_Ingreso_Salida(string filtro);
        Task<Motivo_Ingreso_Salida> GetMotivo_Ingreso_Salida(int id_motivo);
        Task UpdateMotivo_Ingreso_Salida(Motivo_Ingreso_Salida motivo);
        Task<int> AddMotivo_Ingreso_Salida(Motivo_Ingreso_Salida motivo);
        //Tb_Residuo_Solido
        Task<List<Residuo_Solido>> GetResiduo_Solido(string filtro);
        Task<Residuo_Solido> GetResiduo_Solido(int id_residuo_solido);
        Task UpdateResiduo_Solido(Residuo_Solido residuo_solido);
        Task<int> AddResiduo_Solido(Residuo_Solido residuo_solido);
        //Tb_Responsable_ACR_CMRS
        Task<List<Responsable_ARC_CRMS>> GetResponsable_ARC_CRMS(string filtro);
        Task<Responsable_ARC_CRMS> GetResponsable_ARC_CRMS(int id_responsable);
        Task UpdateResponsable_ARC_CRMS(Responsable_ARC_CRMS responsable);
        Task<int> AddResponsable_ARC_CRMS(Responsable_ARC_CRMS responsable);
        //Tb_Vehiculo_RS
        Task<List<Vehiculo_RS>> GetVehiculo_RS(string filtro);
        Task<Vehiculo_RS> GetVehiculo_RS(int id_vehiculo_rs);
        Task UpdateVehiculo_RS(Vehiculo_RS vehiculo_rs);
        Task<int> AddVehiculo_RS(Vehiculo_RS vehiculo_rs);
        //Tb_Usuario_Notificar_RS
        Task<List<Usuario_Notificar_RS>> GetUsuario_Notificar_RS(string filtro);
        Task<Usuario_Notificar_RS> GetUsuario_Notificar_RS(int id_usuario_notificar);
        Task UpdateUsuario_Notificar_RS(Usuario_Notificar_RS usuario_notifcar);
        Task QuitarUsuario_Notificar_RS(int id_usuario_notificar);
        Task<int> AddUsuario_Notificar_RS(int Id_Usuario_AD);

        //Tb_Usuario_Notificar_Incidente
        Task<List<Usuario_Notificar_Incidente>> GetUsuario_Notificar_Incidente(string filtro);
        Task<Usuario_Notificar_Incidente> GetUsuario_Notificar_Incidente(int id_usuario_notificar);
        Task UpdateUsuario_Notificar_Incidente(Usuario_Notificar_Incidente usuario_notifcar);
        Task QuitarUsuario_Notificar_Incidente(int id_usuario_notificar);
        Task<int> AddUsuario_Notificar_Incidente(int Id_Usuario_AD);
        //Tb_Componente_Ambiental
        Task<List<Componente_Ambiental>> GetComponente_Ambientales(string filtro);
        Task<Componente_Ambiental> GetComponente_Ambiental(int id_componente);
        Task<int> AddComponente_Ambiental(Componente_Ambiental componente);
        Task UpdateComponente_Ambiental(Componente_Ambiental componente);

        //Tb_Tipo_Incidente
        Task<List<Tipo_Incidente>> GetTipos_Incidente(string filtro);
        Task<Tipo_Incidente> GetTipo_Incidente(int id_tipo);
        Task<int> AddTipo_Incidente(Tipo_Incidente tipo);
        Task UpdateTipo_Incidente(Tipo_Incidente tipo);

        //Tb_Unidad_Medida
        Task<List<Unidad_Medida>> GetUnidad_Medida(string filtro);
        Task<Unidad_Medida> GetUnidad_Medida(int id_unidad);
        Task<int> AddUnidad_Medida(Unidad_Medida unidad);
        Task UpdateUnidad_Medida(Unidad_Medida unidad);

        //Tb_Nivel_Incidente
        Task<List<Nivel_Incidente>> GetNivel_Incidente(string filtro);
        Task<Nivel_Incidente> GetNivel_Incidente(int id_nivel);
        Task<int> AddNivel_Incidente(Nivel_Incidente nivel);
        Task UpdateNivel_Incidente(Nivel_Incidente nivel);

        //Tb_Producto_Incidente
        Task<List<Producto_Incidente>> GetProducto_Incidente(string filtro);
        Task<Producto_Incidente> GetProducto_Incidente(int id_producto);
        Task<int> AddProducto_Incidente(Producto_Incidente producto);
        Task UpdateProducto_Incidente(Producto_Incidente producto);

        //Tb_Tipo_Producto_Incidente
        Task<List<Tipo_Producto_Incidente>> GetTipo_Producto_Incidente(string filtro);
        Task<Tipo_Producto_Incidente> GetTipo_Producto_Incidente(int id_tipo);
        Task<int> AddTipo_Producto_Incidente(Tipo_Producto_Incidente tipo);
        Task UpdateTipo_Producto_Incidente(Tipo_Producto_Incidente tipo);

        //Tb_Estado_Equipo_Incidente
        Task<List<Estado_Equipo_Incidente>> GetEstado_Equipo_Incidente(string filtro);
        Task<Estado_Equipo_Incidente> GetEstado_Equipo_Incidente(int id_tipo);
        Task<int> AddEstado_Equipo_Incidente(Estado_Equipo_Incidente estado);
        Task UpdateEstado_Equipo_Incidente(Estado_Equipo_Incidente estado);

        //Tb_Lugar_Disposicion
        Task<List<Lugar_Disposicion>> GetLugar_Disposicion(string filtro);
        Task<Lugar_Disposicion> GetLugar_Disposicion(int id_lugar);
        Task<int> AddLugar_Disposicion(Lugar_Disposicion estado);
        Task UpdateLugar_Disposicion(Lugar_Disposicion estado);

        //Tb_Tipo_Agua
        Task<List<Tipo_Agua>> GetTipo_Agua(string filtro);
        Task<Tipo_Agua> GetTipo_Agua(int id_tipo);
        Task<int> AddTipo_Agua(Tipo_Agua tipo);
        Task UpdateTipo_Agua(Tipo_Agua tipo);

        //Tb_Tipo_Suelo
        Task<List<Tipo_Suelo>> GetTipo_Suelo(string filtro);
        Task<Tipo_Suelo> GetTipo_Suelo(int id_tipo);
        Task<int> AddTipo_Suelo(Tipo_Suelo tipo);
        Task UpdateTipo_Suelo(Tipo_Suelo tipo);

        //Tb_Especie_Flora
        Task<List<Especie_Flora>> GetEspecie_Flora(string filtro);
        Task<Especie_Flora> GetEspecie_Flora(int id_tipo);
        Task<int> AddEspecie_Flora(Especie_Flora tipo);
        Task UpdateEspecie_Flora(Especie_Flora tipo);

        //Tb_Especie_Fauna
        Task<List<Especie_Fauna>> GetEspecie_Fauna(string filtro);
        Task<Especie_Fauna> GetEspecie_Fauna(int id_tipo);
        Task<int> AddEspecie_Fauna(Especie_Fauna tipo);
        Task UpdateEspecie_Fauna(Especie_Fauna tipo);
        //Tb_Factor_Causa_Incidente
        Task<List<Factor_Causa_Incidente>> GetFactor_Causa_Incidente(string filtro);
        Task<Factor_Causa_Incidente> GetFactor_Causa_Incidente(int id_factor);
        Task<int> AddFactor_Causa_Incidente(Factor_Causa_Incidente factor);
        Task UpdateFactor_Causa_Incidente(Factor_Causa_Incidente factor);
    }
}
