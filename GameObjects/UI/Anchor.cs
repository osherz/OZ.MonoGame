using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.GameObjects.UI
{
    [Flags]
    public enum Anchor
    {
        None =   0b0000_0000,
        Left =   0b0000_0001,
        Right =  0b0000_0010,
        Top =    0b0000_0100,
        Bottom = 0b0000_1000,
        // Between Top and Bottom
        Center = 0b0001_0000, 
        // Between Right and Left
        Middle = 0b0010_0000
    }
}
