using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain
{
    public class gzUser
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Active { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int UserProfile_UserId { get; set; }
        public string Names { get; set; }
        public string NombreCompleto { get; set; }
        public UserProfile UserProfile { get; set; }
        public string LastNames { get; set; }
        public string Correo { get; set; }
        public string RolStr { get; set; }
        public int Rol { get; set; }
        public string Status { get; set; }
        public string DateCreatedStr { get; set; }
        public string DateModifiedStr { get; set; }
        public int IdSupervisor { get; set; }
        public string Supervisor { get; set; }
        public int NumeroAgente { get; set; }
    }
}
