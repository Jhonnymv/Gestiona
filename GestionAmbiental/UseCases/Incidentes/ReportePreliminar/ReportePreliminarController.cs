using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Incidentes.Items;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.GetInvolucradosIncidente;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarAccionCorrectivaIncidente;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarInvolucradoIncidente;
using GestionAmbiental.Application.UseCases.Incidentes.ReportePreliminar.RegistrarParte1;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionAmbiental.UseCases.Incidentes.ReportePreliminar
{
    public class ReportePreliminarController : Controller
    {

        private readonly IRegistrarParte1 _registrarReporteParte1;
        private readonly ISelectItems _selectItems;
        private readonly IGetInvolucradosIncidente _getInvolucradosIncidente;
        private readonly IRegistrarInvolucradoIncidente _registrarInvolucradoIncidente;
        private readonly IRegistrarAccionCorrectivaIncidente _registrarAccionCorrectivaIncidente;

        public ReportePreliminarController(
            IRegistrarParte1 registrarReporteParte1, 
            ISelectItems selectItems,
            IGetInvolucradosIncidente getInvolucradosIncidente,
            IRegistrarInvolucradoIncidente registrarInvolucradoIncidente,
            IRegistrarAccionCorrectivaIncidente registrarAccionCorrectivaIncidente)
        {
            _registrarReporteParte1 = registrarReporteParte1;
            _selectItems = selectItems;
            _getInvolucradosIncidente = getInvolucradosIncidente;
            _registrarInvolucradoIncidente = registrarInvolucradoIncidente;
            _registrarAccionCorrectivaIncidente = registrarAccionCorrectivaIncidente;
        }

        public async Task<IActionResult> NuevoReporte()
        {
            var items = await _selectItems.GetSelectItems();

            var tipo_Incidente = items.ItemsTipoIncidente.Select(g => new SelectListItem() { Value = g.Id_Tipo_Incidente.ToString(), Text = g.Descripcion }).ToList();
            tipo_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var nivel_Incidente = items.ItemsNivelIncidente.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Nivel_Incidente.ToString(), Text = g.Descripcion }).ToList();
            nivel_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var unidad_Derramado = items.ItemsUnidadMedida.Where(
                x => x.Tipo.Equals(TiposUnidadMedida.General.ToString()) || 
                x.Tipo.Equals(TiposUnidadMedida.Sustancia.ToString()) || 
                x.Tipo.Equals(TiposUnidadMedida.Peso.ToString()))
                .OrderBy(x => x.Descripcion)
                .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
            unidad_Derramado.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var unidad_Area = items.ItemsUnidadMedida.Where(
                x => x.Tipo.Equals(TiposUnidadMedida.General.ToString()) || 
                x.Tipo.Equals(TiposUnidadMedida.Area.ToString()))
                .OrderBy(x => x.Descripcion)
                .Select(g => new SelectListItem() { Value = g.Id_Unidad_Medida.ToString(), Text = g.Descripcion }).ToList();
            unidad_Area.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var componente_Ambiental = items.ItemsCompAmb.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.Id_Componente_Ambiental.ToString(), Text = g.Descripcion }).ToList();
            componente_Ambiental.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            //Causa Incidente
            var grupoActo = new SelectListGroup() { Name = "Acto Subestándar", Disabled = false };
            var grupoCondicion = new SelectListGroup() { Name = "Condición Subestándar", Disabled = false };

            var causa_Incidente = items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.ASA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion, Group = grupoActo }).ToList();
            causa_Incidente.AddRange(items.ItemsTipoCondicion.Where(x => x.Grupo.Equals(TiposCondicion.CSA.ToString())).OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdTipoCondicion.ToString(), Text = g.Descripcion, Group = grupoCondicion }).ToList());
            causa_Incidente.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

           

            ViewBag.Tipo_Incidente = tipo_Incidente;
            ViewBag.Nivel_Incidente = nivel_Incidente;
            ViewBag.Unidad_Derramado = unidad_Derramado;
            ViewBag.Unidad_Area = unidad_Area;
            ViewBag.Componente_Ambiental = componente_Ambiental;
            ViewBag.Causa_Incidente = causa_Incidente;
           

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NuevoReporte(ReporteParte1Model model)
        {
            if (ModelState.IsValid)
            {
                var IdReporte = await _registrarReporteParte1.Execute(
                    model.Num_Trabajadores,
                    model.Razon_Social_Contratista,
                    model.RUC_Contratista,
                    model.Actividad_Contratista,
                    model.Domicilio_Contratista,
                    model.Num_Trabajadores_Contratista,
                    model.Id_Tipo_Incidente,
                    model.Id_Nivel_Incidente,
                    model.Fecha_Incidente,
                    model.Hora_Incidente,
                    model.Producto_Derramado,
                    model.Cantidad_Derramado,
                    model.Id_Unidad_Cantidad_Derramado,
                    model.Area_Afectada,
                    model.Id_Unidad_Area_Afectada,
                    model.Lugar_Incidente,
                    model.Equipo_Involucrado,
                    model.Id_Componente_Ambiental,
                    model.Descripcion_Incidente,
                    model.Id_Causa_Incidente,
                    model.Otra_Causa_Incidente);
                if(model.Opcion.Equals("Guardar"))
                    return RedirectToAction("NuevoReporte");
                else
                    return RedirectToAction("NuevoReporteParte2", new { IdReporte = IdReporte });
            }
            return RedirectToAction("NuevoReporte");
        }
        public async Task<IActionResult> NuevoReporteParte2(int IdReporte)
        {
            var items = await _selectItems.GetSelectItems();

            var gerencias = items.ItemsGerencia.OrderBy(x => x.Descripcion).Select(g => new SelectListItem() { Value = g.IdGerencia.ToString(), Text = g.Descripcion }).ToList();
            gerencias.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var supervisores = items.ItemsUsuarioMA.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres+" "+g.Apellidos }).ToList();
            supervisores.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));

            var usuario = items.ItemsUsuario.OrderBy(x => x.Nombres).Select(g => new SelectListItem() { Value = g.IdUsuario.ToString(), Text = g.Nombres + " " + g.Apellidos }).ToList();
            usuario.Insert(0, new SelectListItem("--SELECCIONE--", "0", true));


            ViewBag.Gerencias = gerencias;
            ViewBag.Supervisor = supervisores;
            ViewBag.Usuario = usuario;

            var model = new ReporteParte2Model();
            model.Id_Reporte_Preliminar = IdReporte;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> NuevoInvolucrado(int IdReporte, string tipo, int usuario, string nombres,string apellidos, string correo, string puesto, string celular, string turno)
        {
            await _registrarInvolucradoIncidente.Execute(IdReporte, tipo, usuario, nombres,apellidos, correo, puesto, celular, turno);
            var involucrados = await _getInvolucradosIncidente.Execute(IdReporte);
            var filas_involucrados = "";
            foreach(var fila in involucrados)
            {
                filas_involucrados += "<tr>";
                filas_involucrados += "<td>" + fila.Tipo_Involucrado + "</td>";
                filas_involucrados += "<td>" + fila.Nombres+" "+ fila.Apellidos + "</td>";
                filas_involucrados += "<td>" + fila.Email + "</td>";
                filas_involucrados += "<td>" + fila.Celular + "</td>";
                filas_involucrados += "<td>" + fila.Puesto_Trabajo + "</td>";
                filas_involucrados += "<td>" + fila.Turno + "</td>";
                filas_involucrados += "</tr>";
            }
            dynamic response = new
            {
                filas_involucrados = filas_involucrados,
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> NuevaAccion(int IdReporte, string descripcion, int usuario, string contratista, string fecha, string estado)
        {
            await _registrarAccionCorrectivaIncidente.Execute(IdReporte, descripcion, usuario, contratista, fecha, estado);
            return Ok();
        }
    }
}