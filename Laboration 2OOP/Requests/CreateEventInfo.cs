using Laboration_2OOP.Domän;


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
