using GestionAmbiental.Application.UseCases.Test.EditarRegistro;
using GestionAmbiental.Application.UseCases.Test.EliminarRegistro;
using GestionAmbiental.Application.UseCases.Test.ListarRegistros;
using GestionAmbiental.Application.UseCases.Test.NuevoRegistro;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental.UseCases.Test
{
    public class TestController : Controller
    {
        private readonly IEditarRegistro _editarRegistro;
        private readonly IEliminarRegistro _eliminarRegistro;
        private readonly IListarRegistros _listarRegistros;
        private readonly INuevoRegistro _nuevoRegistro;

        public TestController(
            IEditarRegistro editarRegistro, 
            IEliminarRegistro eliminarRegistro,
            IListarRegistros listarRegistros,
            INuevoRegistro nuevoRegistro)
        {
            _editarRegistro = editarRegistro;
            _eliminarRegistro = eliminarRegistro;
            _listarRegistros = listarRegistros;
            _nuevoRegistro = nuevoRegistro;
        }

        public IActionResult Resumen()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Listar(int pagina, int rpp)
        {
            var datosTabla = await _listarRegistros.Execute(pagina, rpp);

            var filasTabla = "";
            var paginador = "";
            int nPaginas = datosTabla.NRegistros % rpp != 0 ? (datosTabla.NRegistros / rpp) + 1 : datosTabla.NRegistros / rpp;

            if (datosTabla.NRegistros > 0)
            {
                //Contenido de la tabla
                foreach (var fila in datosTabla.Registros)
                {
                    filasTabla += "<tr>";
                    filasTabla += "<td><a href=\"/Test/EditarRegistro?id=" + fila.Id+"\">" + fila.Id + "</a></td>";
                    filasTabla += "<td>" + fila.Nombre+"</td>";
                    filasTabla += "<td>" + fila.Numero + "</td>";
                    filasTabla += "<td>" + fila.Evidencia + "</td>";
                    filasTabla += "</tr>";
                }
                //Paginador
                paginador += "<ul class=\"pagination pagination-sm\">";
                if (pagina >= 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + 1 + ")\" aria-label=\"Inicio\"><span aria-hidden=\"true\">&laquo;</span><span class=\"sr-only\">Inicio</span></a></li>";

                for (int i = pagina - 2; i <= pagina + 2; i++)
                {
                    if (i > 0 && i <= nPaginas)
                    {
                        if (i == pagina)
                            paginador += "<li class=\"page-item active\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                        else
                            paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + i + ")\">" + i + "</a></li>";
                    }
                }
                if (pagina <= nPaginas - 4 && nPaginas > 5)
                    paginador += "<li class=\"page-item\"><a class=\"page-link\" onclick=\"filtrar(" + nPaginas + ")\" aria-label=\"Final\"><span aria-hidden=\"true\">&raquo;</span><span class=\"sr-only\">Final</span></a></li>";
                paginador += "</ul>";

            }
            else
            {
                filasTabla += "<tr>";
                filasTabla += "<td style=\"vertical-align:middle;\" colspan=\"100%\">Sin Datos</td>";
                filasTabla += "</tr>";
            }
            dynamic response = new
            {
                Data = filasTabla,
                Paginador = paginador
            };
            return Ok(response);
        }

        public IActionResult NuevoRegistro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NuevoRegistro(TestModel model)
        {
            if (ModelState.IsValid)
            {
                var id = await _nuevoRegistro.Execute(model.Nombre, model.Numero, "Evidencia");

                return RedirectToAction("EditarRegistro",new { id = id});
            }
            return View();
        }


        public async Task<IActionResult> EditarRegistro(int id)
        {
            var registros = await _listarRegistros.Execute(1, 100);
            var registroEncontrado = registros.Registros.Find(x => x.Id.Equals(id));

            var model = new TestModel
            {
                Id = registroEncontrado.Id,
                Nombre = registroEncontrado.Nombre,
                Numero = registroEncontrado.Numero
                //Evidencia = registroEncontrado.Evidencia
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditarRegistroAsync(TestModel model)
        {
            if (ModelState.IsValid)
            {
                await _editarRegistro.Execute(model.Id, model.Nombre, model.Numero, "Evidencia");

                return RedirectToAction("EditarRegistro", new { id = model.Id });
            }
            return RedirectToAction("EditarRegistro", new { id = model.Id });
        }


        [HttpPost]
        public async Task ElimnarRegistro(int id)
        {
            await _eliminarRegistro.Execute(id);
        }
    }
}
