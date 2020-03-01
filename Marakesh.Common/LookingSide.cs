using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum LookingSide
    {
        up,
        right,
        down,
        left
    }

    public static class LookSide
    {
        private static readonly int lookingSideLength = System.Enum.GetValues(typeof(LookingSide)).Length;
        public static LookingSide NextLookingSide(LookingSide lookingSide)
        {
            lookingSide++;
            if ((int)lookingSide == lookingSideLength)
                lookingSide = 0;
            return lookingSide;
        }
    }
}
