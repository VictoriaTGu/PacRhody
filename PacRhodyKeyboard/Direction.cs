using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsApplication22
{
    public class Direction
    {
        private int prevDirection = 1;
        private Random RandomGen = new Random();

        public int Direct()
        {
            int direction = RandomDirection(1, 4);
            prevDirection = direction;
            return direction;
        }

        private int RandomDirection(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
