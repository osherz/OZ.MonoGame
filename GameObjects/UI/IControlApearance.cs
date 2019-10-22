using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;



namespace OZ.MonoGame.GameObjects.UI
{
    public interface IControlApearance
    {
        Texture2D Reg { get; }
        Texture2D MouseHover { get; }
        Texture2D MouseDown { get; }
        Texture2D NotEnable { get; }
        SpriteFont Font { get; }
        SoundEffect MouseHoverAudio { get; }
        SoundEffect MouseDownAudio { get;}

    }
}
