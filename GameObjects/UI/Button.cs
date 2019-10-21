using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.GameObjects.UI
{
    public class Button : Label, ITextDrawing
    {
        public Button(GamePrototype gameParent) : base(gameParent)
        {
            BkgTransparent = false;
            TextAnchor = Anchor.Center | Anchor.Middle;
        }


    }
}
