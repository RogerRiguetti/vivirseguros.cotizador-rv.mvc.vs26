using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Estudio.Repository.Core.Domain.Views
{
    public class UserView
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public byte Active { get; set; }
        public string Pass { get; set; }
    }
}
