using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace OZ.MonoGame.GameObjects.UI
{
    public class ControlApearance : IControlApearance
    {
        public Texture2D Reg { get; set; }

        public Texture2D MouseHover { get; set; }

        public Texture2D MouseDown { get; set; }

        public Texture2D NotEnable { get; set; }

        public SpriteFont Font { get; set; }
        public SoundEffect MouseHoverAudio { get; set; }
        public SoundEffect MouseDownAudio { get; set; }
    }
}
