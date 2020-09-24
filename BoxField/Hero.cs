using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BoxField
{
    class Hero
    {
        public Color color;
        public int size, x, y;

        public Hero (int _x, int _y, int _size)
        {
            x = _x;
            y = _y;
            size = _size;
        }

        public void Move (int speed)
        {
            x += speed;
        }

    }
}
