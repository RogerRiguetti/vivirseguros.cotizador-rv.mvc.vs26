using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain.Views
{
    public class AuthorizationView
    {
        public int Id { get; set; }
        public string System { get; set; }
        public string Module { get; set; }
        public string Page { get; set; }
        public string Permission { get; set; }
        public string Authorize { get; set; }

        public int? Unassigned { get; set; }
    }
}
