using Laboration_2OOP.Domän;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2OOP.Requests
{
    public class CreateEventInfo
    {
        public DateTime StartTid { get; set; }
        public string Plats { get; set; } = "";
        public AktivitetTyp AktivitetTyp { get; set; }
        public int MaxAntalDeltagare { get; set; }
        public int MinAntalDeltagare { get; set; }
        public string Tema { get; set; } = "";
        public int AnsvarigArrangorId { get; set; }

        public List<Spel> ValdaSpel { get; set; } = new();
    }
}
