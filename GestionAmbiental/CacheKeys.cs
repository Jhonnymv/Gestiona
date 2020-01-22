using GestionAmbiental.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionAmbiental
{
    public class CacheKeys
    {
        public static InspeccionRol RolUsuario { get; set; }
        public static string NombreUsuario { get { return "_NombreUsuario"; } }
    }
}
