using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exodus.GUI
{
    public struct Padding
    {
        public int Top, Right, Bottom, Left;

        public Padding(int u, int r, int b, int l)
        {
            Top = u;
            Right = r;
            Bottom = b;
            Left = l;
        }

        public Padding(int v, int h)
        {
            Top = v;
            Right = h;
            Bottom = v;
            Left = h;
        }

        public Padding(int a)
        {
            Top = a;
            Right = a;
            Bottom = a;
            Left = a;
        }
    }
}
