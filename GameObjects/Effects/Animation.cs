using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.GameObjects.Effects
{
    public abstract class Animation : IAnimation
    {
        public virtual bool IsStarted { get; protected set; }
        public virtual bool IsFinished { get; protected set; }

        public event Action ActionInEnd;

        public void Update(GameTime gameTime)
        {
            if(IsStarted && !IsFinished)
            {
                InUpdate(gameTime);
            }
        }

        public virtual void StartAnimate()
        {

            IsStarted = true;
            IsFinished = false;
        }

        protected void StopAnimate()
        {
            OnEnd();
        }

        protected abstract void InUpdate(GameTime gameTime);

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        protected virtual void OnEnd()
        {
            ActionInEnd?.Invoke();
            IsStarted = false;
            IsFinished = true;
        }

    }
}
