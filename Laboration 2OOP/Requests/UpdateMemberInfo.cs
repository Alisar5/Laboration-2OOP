using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2OOP.Requests
{
    public class UpdateMemberInfo
    {
        public string Förnamn { get; set; } = "";
        public string Efternamn { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefon { get; set; } = "";
    }
}
