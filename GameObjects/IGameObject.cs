using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OZ.MonoGame.GameObjects
{
    public interface IGameObject
    {

        void Initialize();
        void LoadContent(ContentManager content);
        void Update(GameTime gameTime);
        void UnloadContent(ContentManager content);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
