using GestionAmbiental.Domain.Incidentes.InvestigacionFinal;
using GestionAmbiental.Domain.Incidentes.Items;
using GestionAmbiental.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GestionAmbiental.Application.Repositories.Incidentes.Items
{
    public interface IGetItemsReadOnlyRepository
    {
        Task<List<Componente_Ambiental>> GetListComponenteAmbiental();
        Task<List<Especie_Fauna>> GetListEspecieFauna();
        Task<List<Especie_Flora>> GetListEspecieFlora();
        Task<List<Estado_Equipo_Incidente>> GetListEstadoEquipoIncidente();
        Task<List<Lugar_Disposicion>> GetListLugarDisposicion();
        Task<List<Nivel_Incidente>> GetListNivelIncidente();
        Task<List<Producto_Incidente>> GetListProductoIncidente();
        Task<List<Tipo_Agua>> GetListTipoAgua();
        Task<List<Tipo_Incidente>> GetListTipoIncidente();
        Task<List<Tipo_Producto_Incidente>> GetListTipoProducotIncidente();
        Task<List<Tipo_Suelo>> GetListTipoSuelo();
        Task<List<Unidad_Medida>> GetListUnidadMedida();
        Task<List<TipoCondicion>> GetListTipoCondicion();
        Task<List<Gerencia>> GetListGerencia();
        Task<List<Empresa>> GetListEmpresa();
        Task<List<Usuario>> GetListUsuarioMA();
        Task<List<Usuario>> GetListUsuarios();
        Task<List<Factor_Causa_Incidente>> GetListFactores();
    }
}
