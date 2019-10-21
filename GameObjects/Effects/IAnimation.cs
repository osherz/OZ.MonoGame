using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OZ.MonoGame.GameObjects.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.GameObjects.Effects
{
    public interface IAnimation
    {
        bool IsStarted { get; }
        bool IsFinished { get; }

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
