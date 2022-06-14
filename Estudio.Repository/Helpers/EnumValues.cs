using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudio.Repository.Helpers
{
    public class EnumValues
    {
        public static string ActiveValue = "1";
        public static string InactiveValue = "0";
        public static string ActiveText = "Active";
        public static string InactiveText = "Inactive";
        public static int? AssignedValue = 1;
        public static int? UnassignedValue = null;
        public static DateTime DefaultDate = DateTime.Parse("01 / 01 / 1900");
    }
}
