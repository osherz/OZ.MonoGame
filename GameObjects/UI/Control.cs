using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.GameObjects.UI
{
    public abstract class Control : UIObject, IGameObject, ILocation
    {
        #region Properties

        private ControlsCollection _controls;
        public ControlsCollection Controls 
        { 
            get
            {
                if(_controls is null)
                {
                    _controls = new ControlsCollection(GameParent);
                }
                return _controls;
            }
        }

        #region Textures Properties
        public IControlApearance ControlApearance { get; set; }

        private Texture2D _regTexture;
        public Texture2D RegTexture
        {
            get => _regTexture is null ? ControlApearance.Reg : _regTexture;
            set
            {
                if (_regTexture != value)
                {
                    _regTexture = value;
                    OnRegTextureChanged(EventArgs.Empty);
                    OnLookingChanged();
                }
            }
        }

        /// <summary>
        /// Texture to draw if IsEnabled = true
        /// </summary>

        private Texture2D _textureForNotEnabled;
        public Texture2D TextureForNotEnabled
        {
            get
            {
                if (!(_textureForNotEnabled is null))
                {
                    return _textureForNotEnabled;
                }
                else if (ControlApearance.NotEnable is null)
                {
                    return RegTexture;
                }
                else return ControlApearance.NotEnable;

            }
            set
            {
                _textureForNotEnabled = value;
            }
        }

        protected virtual Texture2D TextureToDrawIfEnabled
        {
            get => RegTexture;
        }

        /// <summary>
        /// Texture to draw.
        /// Using this for drawing.
        /// </summary>
        protected Texture2D TextureToDraw
        {
            get
            {
                if (IsEnabled && IsParentsEnabled)
                {
                    return TextureToDrawIfEnabled;
                }
                else
                {

                    return TextureForNotEnabled;
                }
            }
        }
        #endregion Textures Properties

        #region Rectangle Of Drawing Content Properties
        private Rectangle _rectangleOfContentDrawing;

        /// <summary>
        /// The rectangle that the content will draw into.
        /// For example, the location (0,0) of child, is the left-top of the rectangle.
        /// If non tectangle given, so drawing in all control area is able.
        /// </summary>
        public virtual Rectangle RectangleOfContentDrawing
        {
            get
            {
                if (_rectangleOfContentDrawing.IsEmpty)
                {
                    // Return all control area.
                    return new Rectangle(Point.Zero, Size.ToPoint());
                }

                return _rectangleOfContentDrawing;
            }
            set
            {
                if (_rectangleOfContentDrawing != value)
                {
                    _rectangleOfContentDrawing = value;
                    OnRectangleOfContentDrawingChanged(EventArgs.Empty);
                    OnLookingChanged();
                }
            }
        }

        /// <summary>
        /// Indicates whether the given RectangleOfContentDrawing is after resizing the texture,
        /// Or of the original texture, so we know that the rectangke need to resize too.
        /// </summary>
        public bool RectangleOfContentAfterResizeing { get; set; }

        /// <summary>
        /// If RectangleOfContentAfterResizeing is true, so return RectangleOfContentDrawing,
        /// else return RectangleOfContentDrawing after resizing.
        /// </summary>
        public Rectangle RectangleOfContentDrawingMultiplyScale
        {
            get
            {
                if (RectangleOfContentAfterResizeing || _rectangleOfContentDrawing.IsEmpty)
                {
                    return RectangleOfContentDrawing;
                }
                return UIHelper.Power(RectangleOfContentDrawing, Scale);
            }
        }
        #endregion Rectangle Of Drawing Content Properties

        #region Drawing Properties
        internal override Vector2 LocationInWindow => Parent is null ? Location : Parent.LocationInWindow + Location;

        private Color _bkgColor = Color.White;
        /// <summary>
        /// The Color in SpriteBatch.Draw.
        /// </summary>
        public virtual Color BkgColor
        {
            get => _bkgColor;

            set
            {
                if (_bkgColor != value)
                {
                    _bkgColor = value;
                    OnBkgColorChanged();
                }
            }
        }

        private bool _bkgTransparent = false;
        /// <summary>
        /// If true, the reg texture won't draw at all.
        /// But won't drawing with Color.Transparent.
        /// </summary>
        public virtual bool BkgTransparent
        {
            get => _bkgTransparent;
            set
            {
                if (_bkgTransparent != value)
                {
                    _bkgTransparent = value;
                    OnBkgTransparentChanged(EventArgs.Empty);
                }
            }
        }

        private Vector2 _scale = Vector2.One;
        /// <summary>
        /// Scale for drawing. Calculate ratio expected size and current size of texture.
        /// </summary>
        protected internal Vector2 Scale
        {
            get => _scale;
            private set
            {
                if (_scale != value)
                {
                    _scale = value;
                    OnScaleChanged(EventArgs.Empty);
                }
            }
        }
        #endregion Drawing Properties

        #region Mouse Trigger Properties
        private bool _isHover;
        public bool IsHover
        {
            get { return _isHover; }
            set
            {
#if WINDOWS
                if (IsHover != value)
                {
                    _isHover = value;
                    if (IsHover) OnHovered();
                }
#endif
            }
        }

        private bool _isPressing;
        public bool IsPressing
        {
            get { return _isPressing; }
            set
            {
                if (_isPressing != value)
                {
                    _isPressing = value;
                    if (!IsPressing) // IsPressing was true, and now false - mouse clicked
                    {
#if WINDOWS
                        if (IsHover)
                        {
                            OnClicked();
                        }
#else
                        OnClicked();
#endif
                    }
                }
            }
        }

        private bool _isFocus;
        public bool IsFocus
        {
            get => _isFocus;

            set
            {
                if (IsFocus != value)
                {
                    _isFocus = value;
                    OnFocusChanged();
                }
            }
        }
        #endregion Mouse Trigger Properties

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (IsEnabled != value)
                {
                    _isEnabled = value;
                    OnEnabledChanged();

                }
            }
        }

        /// <summary>
        /// Is parent and all of the grand parents are enabled.
        /// </summary>
        protected bool IsParentsEnabled => Parent is null ? true : Parent.IsParentsEnabled && Parent.IsEnabled;
        #endregion Properties

        #region EVENTS
        public event EventHandler Hovered;
        public event EventHandler Clicked;
        public event EventHandler EnebledChanged;
        public event EventHandler<bool> FocusChanged;
        public event EventHandler BkgColorChanged;
        public event EventHandler BkgTransparentChanged;
        public event EventHandler RegTextureChanged;
        public event EventHandler RectangleOfContentDrawingChanged;
        protected event EventHandler ScaleChanged;
        #endregion EVENTS

        #region RAISE EVENTS METHOD
        protected virtual void OnHovered()
        {
            Hovered?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnEnabledChanged()
        {
            EnebledChanged?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnFocusChanged()
        {
            FocusChanged?.Invoke(this, IsFocus);
        }
        protected virtual void OnBkgColorChanged()
        {
            BkgColorChanged?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnBkgTransparentChanged(EventArgs e)
        {
            BkgTransparentChanged?.Invoke(this, e);
        }
        protected virtual void OnRegTextureChanged(EventArgs e)
        {
            RegTextureChanged?.Invoke(this, e);
        }
        protected virtual void OnRectangleOfContentDrawingChanged(EventArgs e)
        {
            RectangleOfContentDrawingChanged?.Invoke(this, e);
        }
        protected virtual void OnScaleChanged(EventArgs e)
        {
            ScaleChanged?.Invoke(this, e);
        }
        protected override void OnGameParentChanged(EventArgs e)
        {
            base.OnGameParentChanged(e);
            if (!(Controls is null))
            {
                Controls.GameParent = GameParent;
                foreach (var item in Controls)
                {
                    item.GameParent = GameParent;
                }
            }
        }
        #endregion RAISE EVENTS METHOD


        public Control(GamePrototype gameParent) : base(gameParent)
        {

            LookingChanged += (sender, e) =>
              {
                  CalculateScale();
                  if (_toMiddleAfterSuspendResume)
                  {
                      ToMiddle();
                      _toMiddleAfterSuspendResume = false;
                  }

              };
        }


        public override void Initialize()
        {
            Controls.Initialize();
        }

        public override void LoadContent(ContentManager content)
        {
            Controls.LoadContent(content);
        }

        public override void UnloadContent(ContentManager content)
        {
            Controls.UnloadContent(content);
        }

#if ANDROID
        bool _isTouched = false;
        Vector2 _oldTouchPosition;
#endif
        /// <summary>
        /// Prefer do not override this method, but to override InUpdate.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {

            if (IsParentsEnabled && IsEnabled && IsVisible)
            {
                Vector2 position = Vector2.Zero * -1;
                bool isPressed = false;
#if ANDROID
                var touch = TouchPanel.GetState();
                if (touch.IsConnected)
                {
                    if (touch.Count >= 1)
                    {
                        var item = touch[0];
                        _oldTouchPosition = position = item.Position;
                        isPressed = item.State == TouchLocationState.Moved || item.State == TouchLocationState.Pressed;
                        _isTouched = true;
                    }
                    else if (_isTouched)
                    {
                        position = _oldTouchPosition;
                        isPressed = false;
                        _isTouched = false;
                    }

                }
#elif WINDOWS
                var mouseState = Mouse.GetState();
                position = mouseState.Position.ToVector2();
                isPressed = mouseState.LeftButton == ButtonState.Pressed;
#endif

                CheckInputLocationRelativeToControl(position, isPressed);
            }

            InUpdate(gameTime);
            Controls.Update(gameTime);
        }

        /// <summary>
        /// Prefer to override Update method for updating, but this method.
        /// Only active when IsVisible and IsEnabled are both true
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void InUpdate(GameTime gameTime)
        {

        }


        /// <summary>
        /// Prefer do not override this method, but to override InDraw.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public sealed override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            if (IsVisible)
            {
                if (!BkgTransparent)
                {
                    spriteBatch.Draw(TextureToDraw,
                                    LocationInWindow,
                                    null,
                                    BkgColor,
                                    RotationInWindow,
                                    Origin,
                                    Scale,
                                    SpriteEffect,
                                    1);
                }
                InDraw(gameTime, spriteBatch);
            }
        }


        /// <summary>
        /// Prefer to override this method for drawing.
        /// Active only when IsVisible is true.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        protected virtual void InDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Controls.Draw(gameTime, spriteBatch);
        }


        #region Logical Methods
        public bool IsContains(Point point)
        {
            return point.X > LocationInWindow.X &&
                   point.Y > LocationInWindow.Y &&
                   point.X < LocationInWindow.X + Size.X &&
                   point.Y < LocationInWindow.Y + Size.Y;
        }

        /// <summary>
        /// Calculate scale ratio excpected size and RegTexture size
        /// </summary>
        protected void CalculateScale()
        {
            if (!(TextureToDraw is null))
            {
                Scale = new Vector2()
                {
                    X = Size.X / RegTexture.Width,
                    Y = Size.Y / RegTexture.Height
                };
            }
        }

        /// <summary>
        /// When ToMiddle method activate and layout suspended,
        /// so the flag _toMiddleAfterSuspendResume become true,
        /// so when layout resume we will know to middle the control in his Parent/GameParent.
        /// </summary>
        bool _toMiddleAfterSuspendResume = false;
        public void ToMiddle()
        {
            if (GameParent.IsLayoutSuspended)
            {
                _toMiddleAfterSuspendResume = true;
            }
            else
            {
                Rectangle dest = new Rectangle()
                {
                    Location = Parent is null ? Point.Zero : Parent.Location.ToPoint(),
                    Size = Parent is null ? new Point(GameParent.Graphics.PreferredBackBufferWidth, GameParent.Graphics.PreferredBackBufferHeight) : Parent.Size.ToPoint()
                };

                Location = UIHelper.ToMiddle(dest, Size);
            }
        }

        private void CheckInputLocationRelativeToControl(Vector2 inputPosition, bool isPressed)
        {
            if (IsContains(inputPosition.ToPoint())) // Cursor inside control
            {
                IsHover = true;


                IsFocus = IsPressing && !isPressed;
                IsPressing = isPressed;

            }
            else // If mouse not in the control
            {
                IsHover = false;
                //IsPressed = false;
                IsPressing = false;

                if (isPressed)
                {
                    IsFocus = false;
                }
            }


        }
        #endregion Logical Methods
    }
}
