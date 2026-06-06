using Laboration_2OOP.Domän;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2OOP.Requests
{
    public class RegisterMemberInfo
    {
        public string Förnamn { get; set; } = "";
        public string Efternamn { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefon { get; set; } = "";
        public Roll Roll { get; set; }
        public MedlemsStatus Status { get; set; }
        public DateTime RegistreradDatum { get; set; }
    }
}
