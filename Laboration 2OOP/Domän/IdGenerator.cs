

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
