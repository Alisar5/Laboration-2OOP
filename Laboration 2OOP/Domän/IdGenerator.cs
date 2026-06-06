using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboration_2OOP.Domän
{
    public class IdGenerator
    {
        private int _next;

        public IdGenerator(int start = 1)
        {
            _next = start;
        }

        public int NextId()
        {
            return _next++;
        }
    }
}
