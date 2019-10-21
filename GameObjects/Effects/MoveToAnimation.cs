using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.GameObjects.Effects
{
    public class MoveToAnimation : Animation
    {
        public ILocation Obj { get; set; }
        public Vector2 Destination { get; set; }
        public double TimeToReach { get; set; }

        private Vector2 _velocity;
        public Vector2 Velocity => _velocity;
        private Vector2 _oldLocation;

        public override void StartAnimate()
        {
            _oldLocation = Obj.Location;

            _velocity = Destination - _oldLocation;
            _velocity.Normalize();
            _velocity *= (float)((Destination - _oldLocation).Length() / TimeToReach);

            base.StartAnimate();

        }

        public virtual void StartAnimate(ILocation obj, Vector2 destination, double timeToReach)
        {
            Obj = obj;
            Destination = destination;
            TimeToReach = timeToReach;

            StartAnimate();
        }

        protected override void InUpdate(GameTime gameTime)
        {
            // Check whether the star already reac
            if (IsReachDestination())
            {
                StopAnimate();
            }
            else
            {
                Obj.Location += _velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if(IsOverDestination())
                {
                    Obj.Location = Destination;
                }
            }
        }

        private bool IsReachDestination()
        {
            return Destination == Obj.Location;
        }

        private bool IsOverDestination()
        {
            return (Destination - _oldLocation).Length() <= (Obj.Location - _oldLocation).Length();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}
