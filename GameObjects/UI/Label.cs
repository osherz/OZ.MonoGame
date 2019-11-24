using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace OZ.MonoGame.GameObjects.UI
{
    public class Label : ChangeApearanceRelativeMouseControl, ITextDrawing
    {
        #region Properties
        /// <summary>
        /// Location of text relative to control.
        /// </summary>
        protected Vector2 LocationOfText { get; private set; }
       
        /// <summary>
        /// Location of text relative to window.
        /// </summary>
        internal virtual Vector2 LocationOfTextInWindow => LocationInWindow + LocationOfText;

        public override Vector2 Location
        {
            get => base.Location;
            set
            {
                if (Location != value)
                {
                    base.Location = value;
                    OnLookingChanged();
                }
            }
        }

        private string _text;
        public virtual string Text
        {
            get => _text;
            set
            {
                if (Text != value)
                {
                    _text = value;
                    OnTextChanged(EventArgs.Empty);
                    OnLookingChanged();
                }
            }
        }

        private SpriteFont _font;
        public virtual SpriteFont Font
        {
            get => _font is null ? 
                            ControlApearance is null ? null : ControlApearance.Font
                        : _font;
            set
            {
                if (Font != value)
                {
                    _font = value;
                    OnFontChanged(EventArgs.Empty);
                    OnLookingChanged();
                }
            }
        }

        private float _textScale;
        public virtual float TextScale
        {
            get => _textScale;
            set
            {
                if (TextScale != value)
                {
                    _textScale = value;
                    OnTextScaleChanged(EventArgs.Empty);
                    OnLookingChanged();
                }
            }
        }

        private Color _foreColor;
        public virtual Color ForeColor
        {
            get => _foreColor;
            set
            {
                if (ForeColor != value)
                {
                    _foreColor = value;
                    OnForeColorChanged(EventArgs.Empty);
                }
            }
        }

        private Anchor _textAnchor;
        public Anchor TextAnchor
        {
            get => _textAnchor;
            set
            {
                if (TextAnchor != value)
                {
                    _textAnchor = value;
                    OnTextAnchorChanged(EventArgs.Empty);
                    OnLookingChanged();
                }
            }
        }

        /// <summary>
        /// (Original text size) * scale
        /// </summary>
        public Vector2 TextSize =>Font is null ? Vector2.Zero : Font.MeasureString(Text) * TextScale;
        #endregion Properties


        #region Events
        public event EventHandler TextChanged;
        public event EventHandler FontChanged;
        public event EventHandler TextScaleChanged;
        public event EventHandler ForeColorChanged;
        public event EventHandler DrawingRectangleChanged;
        public event EventHandler TextAnchorChanged;
        #endregion Events

        #region Triger Events Methods
        protected virtual void OnTextChanged(EventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }
        protected virtual void OnFontChanged(EventArgs e)
        {
            FontChanged?.Invoke(this, e);
        }
        protected virtual void OnForeColorChanged(EventArgs e)
        {
            ForeColorChanged?.Invoke(this, e);
        }
        protected virtual void OnDrawingRectangleChanged(EventArgs e)
        {
            DrawingRectangleChanged?.Invoke(this, e);
        }
        protected virtual void OnTextScaleChanged(EventArgs e)
        {
            TextScaleChanged?.Invoke(this, e);
        }
        protected virtual void OnTextAnchorChanged(EventArgs e)
        {
            TextAnchorChanged?.Invoke(this, e);
        }
        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
            {
                GameParent = Parent.GameParent;
            }
            base.OnGameParentChanged(e);
        }
        protected override void OnBeforeParentChanged()
        {
            base.OnBeforeParentChanged();
        }
        #endregion Triger Events Methods




        public Label(GamePrototype gameParent) : base(gameParent)
        {
            Text = "";
            TextScale = 1;
            LookingChanged+=(sender,e) => CalculateLocationOfText();
            BkgTransparent = true;
            ForeColor = Color.Black;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
        }

        public override void UnloadContent(ContentManager content)
        {
            base.UnloadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void InDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.InDraw(gameTime, spriteBatch);
            if (!string.IsNullOrEmpty(Text))
            {
                spriteBatch.DrawString(Font,
                                       Text,
                                       LocationOfTextInWindow,
                                       ForeColor,
                                       RotationInWindow,
                                       Origin,
                                       TextScale,
                                       SpriteEffect,
                                       1);
            }
        }


        #region Logical Methods
        /// <summary>
        /// Calculate location of text in label in consideration the "TextAnchor"
        /// </summary>
        private void CalculateLocationOfText()
        {
            Vector2 location = Vector2.Zero;
            Rectangle drawingRectangle = RectangleOfContentDrawingMultiplyScale;

            //Choose row
            if ((Anchor.Top & TextAnchor) != Anchor.None)
            {
                location.Y = drawingRectangle.Top;
            }
            else if ((Anchor.Center & TextAnchor) != Anchor.None)
            {
                location.Y = drawingRectangle.Top + (drawingRectangle.Height - TextSize.Y) / 2;
            }
            else //if ((Anchor.Bottom & Anchor) != Anchor.None)
            {
                location.Y = drawingRectangle.Bottom - TextSize.Y;
            }

            //Choose column
            if ((Anchor.Left & TextAnchor) != Anchor.None)
            {
                location.X = drawingRectangle.Left;
            }
            else if ((Anchor.Middle & TextAnchor) != Anchor.None)
            {
                location.X = drawingRectangle.Left + (drawingRectangle.Width - TextSize.X) / 2;
            }
            else //if ((Anchor.Right & Anchor) != Anchor.None)
            {
                location.X = drawingRectangle.Right - TextSize.X;
            }

            LocationOfText = location;
        }
        #endregion Logical Methods


    }
}
