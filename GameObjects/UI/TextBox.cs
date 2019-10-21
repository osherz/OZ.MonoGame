using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OZ.MonoGame.GameObjects.UI
{
    public class TextBox : Label, ITextDrawing
    {
        #region Saman Properties
        Texture2D _samanTexture;

        /// <summary>
        /// Delay for apearance
        /// </summary>
        public virtual double SamanDelay { get; set; } = 700;
        public virtual Color SamanColor { get; set; }

        public virtual double DelayOfGetKeys { get; set; } = 1500;

        private Vector2 LocationOfTextEndRelativeTextBox
        {
            get
            {
                return new Vector2()
                {
                    X = TextSize.X + LocationOfText.X,
                    Y = RectangleOfContentDrawingMultiplyScale.Y
                };
            }
        }
        private Vector2 LocationOfTextEndRelativeWindow
        {
            get
            {
                return LocationInWindow + LocationOfTextEndRelativeTextBox;
            }
        }
        #endregion Saman Properties

        public virtual int MaxCharacters { get; set; }



        public TextBox(GamePrototype gameParent) : base(gameParent)
        {
            BkgTransparent = false;
        }




        #region Triger Events Methods
        protected override void OnGameParentChanged(EventArgs e)
        {
            base.OnGameParentChanged(e);
            InitSaman();
        }

        #endregion Triger Events Methods

        private void InitSaman()
        {
            if (!(_samanTexture is null))
            {
                _samanTexture.Dispose();
            }

            if (!(GameParent is null))
            {
                _samanTexture = new Texture2D(GameParent.GraphicsDevice, 1, 1);
                _samanTexture.SetData(new[] { Color.White });
            }
        }


        public override void UnloadContent(ContentManager content)
        {
            if (!(_samanTexture is null))
            {
                _samanTexture.Dispose();
            }
            base.UnloadContent(content);
        }

        protected override void InUpdate(GameTime gameTime)
        {
            base.InUpdate(gameTime);
            if (IsFocus)
            {
                WriteText(gameTime);
                _timeElapsedSinceLastSamanVisible += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_timeElapsedSinceLastSamanVisible >= SamanDelay)
                {
                    _timeElapsedSinceLastSamanVisible = 0;
                    _samanIsVisible = !_samanIsVisible;
                }
            }
            else // Reset timer of saman
            {
                _timeElapsedSinceLastKey = -1;
            }
        }

        private void ShowSaman()
        {
            _timeElapsedSinceLastSamanVisible = 0;
            _samanIsVisible = true;
        }

        double _timeElapsedSinceLastSamanVisible = 0;
        bool _samanIsVisible = true;
        protected override void InDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.InDraw(gameTime, spriteBatch);

            if (IsFocus && _samanIsVisible)
            {

                DrawSaman(gameTime, spriteBatch);
            }

        }

        private void DrawSaman(GameTime gameTime, SpriteBatch spriteBatch)
        {

            float space = Font.MeasureString("|").X * TextScale / 2;
            Vector2 size = new Vector2()
            {
                Y = RectangleOfContentDrawingMultiplyScale.Height,
                X = space 
            } * 0.6f;

            Vector2 location = new Vector2()
            {
                X = LocationOfTextEndRelativeWindow.X,
                Y = LocationInWindow.Y+ RectangleOfContentDrawingMultiplyScale.Y + (RectangleOfContentDrawingMultiplyScale.Height - size.Y)/2
            };

            spriteBatch.Draw(_samanTexture,
                            location + new Vector2(space, 0),
                            null,
                            SamanColor,
                            RotationInWindow,
                            Vector2.Zero,
                            size,
                            SpriteEffect,
                            1);
        }

        #region Logical Method
        double _timeElapsedSinceLastKey = -1; // timer for write new characters
        Keys[] _lastKeys;
        /// <summary>
        /// Write characters only if they not pressed in last call for this function
        /// or timer got to zero.
        /// </summary>
        /// <param name="gameTime"></param>
        private void WriteText(GameTime gameTime)
        {

            KeyboardState keyboardState = Keyboard.GetState();

            Keys[] pressedKeys = keyboardState.GetPressedKeys();
            List<Keys> keysToCheck = new List<Keys>();

            CountDelayFromLastPressed(gameTime);

            if (keyboardState.GetPressedKeys().Count() > 0)
            {
                if (_timeElapsedSinceLastKey == -1) // Its the first key since focused so start count
                {
                    _timeElapsedSinceLastKey = 0;
                }


                // Check for keys that can to be pressed relative the DelayOfGetKeys
                foreach (var key in pressedKeys)
                {
                    if (_lastKeys is null || // First pressed since focused
                        !_lastKeys.Contains(key) || // This key not pressed in last update 
                        _timeElapsedSinceLastKey <= 0)
                    {
                        keysToCheck.Add(key);
                    }
                }

                WriteCharacters(keysToCheck);

            }
            _lastKeys = keyboardState.GetPressedKeys();

        }

        private void WriteCharacters(IEnumerable<Keys> keysToCheck)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            foreach (var key in keysToCheck)
            {
                char keyCh = (char)((int)key);
                if (IsWriteable(keyCh) && Text.Length <= MaxCharacters)
                {
                    if (char.IsLetter(keyCh) &&
                        keyboardState.CapsLock == (keyboardState.IsKeyDown(Keys.LeftShift) ||
                                                   keyboardState.IsKeyDown(Keys.RightShift)))
                    {
                        keyCh = char.ToLower(keyCh);
                    }

                    Text += keyCh;
                    ShowSaman();
                }
                else if (key == Keys.Back && !string.IsNullOrEmpty(Text))
                {
                    Text = Text.Remove(Text.Length - 1);
                    ShowSaman();
                }
            }

        }

        private void CountDelayFromLastPressed(GameTime gameTime)
        {
            if (_timeElapsedSinceLastKey >= 0) // In count time progress
            {
                _timeElapsedSinceLastKey += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_timeElapsedSinceLastKey >= DelayOfGetKeys)
                {
                    _timeElapsedSinceLastKey = 0;
                }
            }
        }

        protected virtual bool IsWriteable(char ch)
        {
            return Font.Characters.Contains(ch);
        }
        #endregion Logical Method
    }
}
