using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2OOP.Domän
{
    public class DomanException : Exception
    {
        public DomanException(string message) : base(message) { }
    }

    public class ValideringsException : DomanException
    {
        public ValideringsException(string message) : base(message) { }
    }

    public class FullbokadException : DomanException
    {
        public FullbokadException(string message) : base(message) { }
    }
    public class DubbelAnmalanException : DomanException
    {
        public DubbelAnmalanException(string message) : base(message) { }
    }

    public class InaktivMedlemException : DomanException
    {
        public InaktivMedlemException(string message) : base(message) { }
    }

    public class ObjektHittasInteException : DomanException
    {
        public ObjektHittasInteException(string message) : base(message) { }
    }

    public class DubblettException : DomanException
    {
        public DubblettException(string message) : base(message) { }
    }
}


