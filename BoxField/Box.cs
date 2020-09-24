using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BoxField
{
    class Box
    {
        public Color color;
        public int size, x, y;

        public Box (int _x, int _y, int _size)
        {
            x = _x;
            y = _y;
            size = _size;
        }

        public void Move(int speed)
        {
            y += speed;
        }
    }
}
