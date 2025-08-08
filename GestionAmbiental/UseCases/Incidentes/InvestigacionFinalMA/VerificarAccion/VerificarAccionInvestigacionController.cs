using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.GetAcciones;
using GestionAmbiental.Application.UseCases.Incidentes.InvestigacionFinal.Accion.VerificarAccion;
using GestionAmbiental.Domain.Enum;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GestionAmbiental.UseCases.Incidentes.InvestigacionFinalMA.VerificarAccion
{
    public class VerificarAccionInvestigacionController : Controller
    {
        private readonly IGetAccioIncidenteInvestigacion _getAccioIncidenteInvestigacion;
        private readonly IVerificarAccion _verificarAccion;
        private readonly IDataProtector _protector;

        public VerificarAccionInvestigacionController(
            IGetAccioIncidenteInvestigacion getAccioIncidenteInvestigacion,
            IVerificarAccion verificarAccion,
            IDataProtectionProvider dataProtectionProvider,
            DataProtectionPurpouseStrings dataProtectionPurpouseStrings)
        {
            _getAccioIncidenteInvestigacion = getAccioIncidenteInvestigacion;
            _verificarAccion = verificarAccion;
            _protector = dataProtectionProvider.CreateProtector(dataProtectionPurpouseStrings.InvestigacionIdRouteValue);
        }

        public async Task<IActionResult> VerificarAccion(string IdAccion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdAccion=" + IdAccion });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdAccion));
                var accion = await _getAccioIncidenteInvestigacion.Execute(IdDesencriptado);
                if (accion.Estado.Equals(EstadosIncidentes.Implementado.ToString()))
                {
                    var model = new VerificarAccionModel();
                    model.Id_Accion_Incidente_Investigacion = accion.Id_Accion_Correctiva_Incidente;
                    model.Id_Encriptado = IdAccion;
                    model.Id_Investigacion_Final = accion.Id_Investigacion_Final;
                    model.Id_Investigacion_Encriptado = _protector.Protect(accion.Id_Investigacion_Final.ToString());
                    model.Tipo_Accion = accion.Tipo_Accion;
                    model.Accion_Inmediata = accion.Accion_Inmediata;
                    model.Responsable_Marcobre = accion.Responsable_Marcobre;
                    model.Responsable_Marcobre2 = accion.Responsable_Marcobre2;
                    model.Responsable_Contratista = accion.Responsable_Contratista;
                    model.Fecha = accion.Fecha;
                    model.Evidencias = accion.Evidencia;
                    model.Comentarios_Implementacion = accion.Comentarios_Implementacion;
                    model.Estatus = accion.Estatus;
                    model.Estado = accion.Estado;

                    return View(model);
                }
                else
                {
                    var IdEncriptado = _protector.Protect(accion.Id_Investigacion_Final.ToString());
                    return RedirectToAction("DetalleInvestigacion", "InvestigacionFinal", new { IdInvestigacion = IdEncriptado });
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> VerificarAccion(VerificarAccionModel model)
        {

            if (ModelState.IsValid)
            {
                await _verificarAccion.Execute(model.Id_Accion_Incidente_Investigacion, model.Opcion,model.Estatus, model.Comentarios_Verificacion);
                if(model.Opcion.Equals("Rechazar"))
                    return RedirectToAction("NotificarAccionRechazada", "NotificacionInvestigacionIncidente", new { IdAccion = model.Id_Accion_Incidente_Investigacion });
                else
                    return RedirectToAction("DetalleInvestigacion", "InvestigacionFinal", new { IdInvestigacion = model.Id_Investigacion_Encriptado });
            }

            return View(model);
        }
        public async Task<IActionResult> DetalleAccion(string IdAccion)
        {
            if (!(HttpContext.Session.GetString("SessionUser") != null))
            {
                return RedirectToAction("NoAutorizado", "Home", new { returnUrl = "/" + ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName + "?IdAccion=" + IdAccion });
            }
            else
            {
                var userInfo = JsonConvert.DeserializeObject<UserInfo>(HttpContext.Session.GetString("SessionUser"));

                ViewBag.Rol = userInfo.Rol.ToString();
                ViewBag.Usuario = userInfo.NombreUsuario;
                int IdDesencriptado = Convert.ToInt32(_protector.Unprotect(IdAccion));
                var accion = await _getAccioIncidenteInvestigacion.Execute(IdDesencriptado);
                var model = new VerificarAccionModel();
                model.Id_Accion_Incidente_Investigacion = accion.Id_Accion_Correctiva_Incidente;
                model.Id_Encriptado = IdAccion;
                model.Id_Investigacion_Final = accion.Id_Investigacion_Final;
                model.Id_Investigacion_Encriptado = _protector.Protect(accion.Id_Investigacion_Final.ToString());
                model.Tipo_Accion = accion.Tipo_Accion;
                model.Accion_Inmediata = accion.Accion_Inmediata;
                model.Responsable_Marcobre = accion.Responsable_Marcobre;
                model.Responsable_Contratista = accion.Responsable_Contratista;
                model.Fecha = accion.Fecha;
                model.Evidencias = accion.Evidencia;
                model.Comentarios_Implementacion = accion.Comentarios_Implementacion;
                model.Comentarios_Verificacion = accion.Comentarios_Verificacion;
                model.Estatus = accion.Estatus;
                model.Estado = accion.Estado;

                return View(model);
            }
        }

    }
}