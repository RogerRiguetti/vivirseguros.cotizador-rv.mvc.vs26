using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain.Views
{
    public class PermissionView
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public int? Unassigned { get; set; }
    }
}
