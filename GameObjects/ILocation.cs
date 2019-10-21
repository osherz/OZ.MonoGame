using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.GameObjects
{
    public interface ILocation
    {
        /// <summary>
        /// location
        /// </summary>
        Vector2 Location { get; set; }
    }
}
