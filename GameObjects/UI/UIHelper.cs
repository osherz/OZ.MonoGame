using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.GameObjects.UI
{
    public static class UIHelper
    {
        public static Vector2 ToMiddle(Rectangle destination, Vector2 objSize)
        {
            Vector2 destLocation = destination.Location.ToVector2();
            Vector2 destSize = destination.Size.ToVector2();

            Vector2 middle = destLocation + (destSize - objSize) / 2;

            return middle;
        }

        public static Rectangle Power(Rectangle rect, float scale)
        {
            return Power(rect, new Vector2(scale));
        }

        public static Rectangle Power(Rectangle rect, Vector2 scale)
        {
            return new Rectangle()
            {
                Location = (rect.Location.ToVector2() * scale).ToPoint(),
                Size = (rect.Size.ToVector2() * scale).ToPoint()
            };
        }

        /// <summary>
        /// Using GamePrototype.SpriteBatch and GamePrototype.GraphicsDevice.
        /// Do not use between SpriteBatch.Begin and SpriteBatch.End.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="texture"></param>
        /// <param name=""></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public static Texture2D ActiveEffectOnTexture(this Texture2D texture, GamePrototype game, Effect effect)
        {
            var graphicsDevice = game.GraphicsDevice;

            int width = texture.Width;
            int height = texture.Height;

            RenderTarget2D effectedTexture = new RenderTarget2D(
                graphicsDevice,
                graphicsDevice.PresentationParameters.BackBufferWidth,
                graphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            graphicsDevice.SetRenderTarget(effectedTexture);
            graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // Draw the scene
            var spriteBatch = game.SpriteBatch;
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(SpriteSortMode.Deferred,
                              null,
                              null,
                              null,
                              null,
                              effect,
                              null);
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            spriteBatch.End();
            Texture2D newTexture = effectedTexture;

            // Drop the render target
            graphicsDevice.SetRenderTarget(null);

            return newTexture;

        }
    }
}
