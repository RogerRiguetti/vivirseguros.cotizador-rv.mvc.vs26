using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Logic
{
    public class Identity
    {


        public string UserId = "0";
        public string Account = "";
        public string Name = "";

        #region Administracion

        public bool SystemIndex = false;
        public bool SystemDetails = false;
        public bool SystemCreate = false;
        public bool SystemEdit = false;
        public bool SystemDelete = false;

        public bool ModuleIndex = false;
        public bool ModuleDetails = false;
        public bool ModuleCreate = false;
        public bool ModuleEdit = false;
        public bool ModuleDelete = false;

        public bool PageIndex = false;
        public bool PageDetails = false;
        public bool PageCreate = false;
        public bool PageEdit = false;
        public bool PageDelete = false;
        public bool PageConfigure = false;

        public bool PermissionIndex = false;
        public bool PermissionDetails = false;
        public bool PermissionCreate = false;
        public bool PermissionEdit = false;
        public bool PermissionDelete = false;

        public bool RoleIndex = false;
        public bool RoleDetails = false;
        public bool RoleCreate = false;
        public bool RoleEdit = false;
        public bool RoleDelete = false;
        public bool RoleConfigure = false;

        public bool UserIndex = false;
        public bool UserDetails = false;
        public bool UserCreate = false;
        public bool UserEdit = false;
        public bool UserDelete = false;
        public bool UserConfigure = false;
        public bool UserProfile = false;


        public bool MantenedorPerfilesIndex = false;
        public bool MantenedorPerfilesCreate = false;
        public bool MantenedorPerfilesEdit = false;
        public bool MantenedorPerfilesDelete = false;
        public bool MantenedorPerfilesDetails = false;
        public bool CotizacionIndex { get; set; }
        public bool CotizacionEdit { get; set; }
        //Nuevas
        public bool ExcepcionIndex { get; set; }
        public bool ExcepcionExterna { get; set; }
        public bool GastosSepelioIndex { get; set; }
        public bool GenArMelerIndex { get; set; }
        public bool EliminarCargasIndex { get; set; }
        public bool LimiteCotIniIndex { get; set; }
        public bool LimiteCotMejIndex { get; set; }
        public bool ManFecAcepCotIndex { get; set; }
        public bool MantenedorIpcIndex { get; set; }
        public bool MejorasIndex { get; set; }
        public bool ParametroGastoIndex { get; set; }
        public bool ProCarArchivoIndex { get; set; }
        public bool SolicitudCotIndex { get; set; }
        public bool TasaAnclajeIndex { get; set; }
        public bool TasaCalceIndex { get; set; }
        public bool TasaRentabilidadIndex { get; set; }
        public bool TasaMercadoIndex { get; set; }
        public bool ValoresMmIndex { get; set; }
        public bool ValoresMonedaIndex { get; set; }
        public bool LimiteCotExtraOIndex { get; set; }
        public bool TasaVtaPromIndex { get; set; }
        public bool TCDiarioIndex { get; set; }
        public bool OficialesIndex { get; set; }

        //Reservas
        public bool ReservasIndex { get; set; }
        //Renta Privada
        public bool RPMenu { get; set; }

        //Emision y Pago de Pensiones 
        public bool EmisionPPMenu { get; set; }

        public bool MantenimientosIndex { get; set; }
        public bool MantenedorFiltrosIndex { get; set; }
        public bool MantenedorCurvasIndex { get; set; }
        public bool ActualizaInfOfiIndex { get; set; }
        


        #endregion
    }
}
