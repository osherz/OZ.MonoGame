using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.GameObjects.UI
{
    public interface ITextDrawing
    {
        string Text { get; set; }
        //Vector2 Location { get; set; }
        float TextScale { get; set; }
        SpriteFont Font { get; set; }
        Color ForeColor { get; set; }

        Anchor TextAnchor { get; set; }
     }
}
