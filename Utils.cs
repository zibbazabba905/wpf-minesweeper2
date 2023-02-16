using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sweeper06
{
    public struct Tuple
    {
        public Tuple(int inX, int inY)
        {
            x = inX;
            y = inY;
        }
        public int x { get; set; }
        public int y { get; set; }
        public override string ToString()
        {
            return $"{x}:{y}";
        }
        public override bool Equals(object obj)
        {
            Tuple p = (Tuple)obj;
            if ((p.x == x) && (p.y == y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public struct GridSizeComponent
    {
        public GridSizeComponent(string inName, Tuple inDimensions)
        {
            Name = inName;
            Dimensions = inDimensions;
        }
        public string Name { get; set; }
        public Tuple Dimensions { get; set; }
        public override string ToString()
        {
            return $"{Name}, {Dimensions.x}:{Dimensions.y}" + base.ToString();
        }
    }
}
