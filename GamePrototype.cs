using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OZ.MonoGame.GameObjects.Effects;
using OZ.MonoGame.GameObjects.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GamePrototype : Game
    {

        const double PANEL_TIME_TO_LEAVE = 500;
        public GraphicsDeviceManager Graphics { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        public Point Size => new Point(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

        List<Control> _controls;
        public List<Control> Controls => _controls;

        List<IAnimation> _animations;
        public List<IAnimation> Animations => _animations;

        internal bool IsLayoutSuspended { get; private set; }
        
        public bool IsLoadContentHappened { get; private set; }

        internal event EventHandler LayoutResumed;

        public GamePrototype()
        {
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            _controls = new List<Control>();
            _animations = new List<IAnimation>();
        }

        public void SuspendLayout() => IsLayoutSuspended = true;
        public void ResumeLayout()
        {
            IsLayoutSuspended = false;
            LayoutResumed?.Invoke(this,EventArgs.Empty);
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            foreach (var item in _controls)
            {
                item.Initialize();
            }
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            foreach (var item in _controls)
            {
                item.LoadContent(Content);
            }
            IsLoadContentHappened = true;
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();

            foreach (var control in _controls)
            {
                control.UnloadContent(Content);
            }

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            foreach (var item in _animations.ToArray())
            {
                item.Update(gameTime);
            }

            foreach (var control in _controls)
            {
                control.Update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            InDraw(gameTime);
            SpriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// This method already inside SpriteBatch.Begin() and SpriteBatch.End(), 
        /// so just draw.
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void InDraw(GameTime gameTime)
        {
            foreach (var control in _controls)
            {
                control.Draw(gameTime, SpriteBatch);
            }
            foreach (var item in _animations.ToArray())
            {
                item.Draw(gameTime, SpriteBatch);
            }

        }


    }

}
