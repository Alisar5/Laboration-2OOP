using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2OOP.Domän
{
    public enum MedlemsStatus { Aktiv, Inaktiv }

    public enum Roll { Medlem, Arrangör, Administratör }

    public enum AktivitetTyp { Öppenspelkväll, Temakväll, Introduktion, Turnering }

    public enum Spelkategori { Strategi, Familj, Samarbete, Fest, Kortspel, Sällskapsspel, Party }

    public enum Svårighetsgrad { Lätt, Medel, Svår }

    public enum TillgänglighetStatus { Tillgänglig, Reserverad, Otillgänglig }

    public enum AnmälanStatus { Aktiv, Avanmäld }
}
