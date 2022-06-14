using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Estudio.Controllers.Assets
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

        #endregion

    }
}