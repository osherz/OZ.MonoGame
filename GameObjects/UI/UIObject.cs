using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.GameObjects.UI
{
    public abstract class UIObject : IGameObject
    {
        public UIObject(GamePrototype gameParent)
        {
            GameParent = gameParent;
        }



        #region Properties
        private GamePrototype _gameParent;
        public virtual GamePrototype GameParent
        {
            get => _gameParent;
            set
            {
                if (GameParent != value)
                {
                    ResetEventsOfGameParent();
                    OnBeforeGameParentChanged();
                    _gameParent = value;
                    RegisterEventsToGameParent();
                    OnGameParentChanged(EventArgs.Empty);
                    OnLookingChanged();
                }
            }
        }

        private Control _parent;
        public Control Parent
        {
            get => _parent;
            set
            {
                OnBeforeParentChanged();
                _parent = value;
                OnParentChanged(EventArgs.Empty);
                OnLookingChanged();
            }
        }

        private object _tag;
        public virtual object Tag
        {
            get => _tag;
            set
            {
                if (Tag != value)
                {
                    _tag = value;
                    OnTagChanged(EventArgs.Empty);
                }
            }
        }

        #region Drawing Peoperties
        /// <summary>
        /// Inner Location in parent.
        /// </summary>
        public virtual Vector2 Location { get; set; }

        /// <summary>
        /// Unlike Location, this location is ratio the window, taking in consideration the Parent location.
        /// Using for drawing.
        /// </summary>
        internal abstract Vector2 LocationInWindow { get; }


        private float _rotation;
        public virtual float Rotation
        {
            get => _rotation;
            set
            {
                if (Rotation != value)
                {
                    _rotation = value;
                    OnRotationChanged(EventArgs.Empty);
                    OnLookingChanged();
                }
            }
        }
        
        /// <summary>
        /// Unlike Rotation, this rotation is ratio the window, taking in consideration the Parent rotation.
        /// Using for drawing.
        /// </summary>
        internal virtual float RotationInWindow => Parent is null ? Rotation : Rotation + Parent.Rotation;

        private Vector2 _origin;
        public virtual Vector2 Origin
        {
            get => _origin;
            set
            {
                if (Origin != value)
                {
                    _origin = value;
                    OnOriginChanged(EventArgs.Empty);
                    OnLookingChanged();
                }
            }
        }

        private SpriteEffects _spriteEffect;
        public virtual SpriteEffects SpriteEffect
        {
            get => _spriteEffect;
            set
            {
                if (SpriteEffect != value)
                {
                    _spriteEffect = value;
                    OnSpriteEffectChanged(EventArgs.Empty);
                    OnLookingChanged();
                }
            }
        }

        private Vector2 _size;
        public virtual Vector2 Size
        {
            get => _size;
            set
            {
                if (Size != value)
                {
                    _size = value;
                    OnSizeChanged(EventArgs.Empty);
                    OnLookingChanged();
                }
            }
        }


        private bool _isVisible = true;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (IsVisible != value)
                {
                    _isVisible = value;
                    OnIsVisibleChanged(EventArgs.Empty);
                }
            }
        }
        #endregion Drawing Peoperties

        #endregion Properties

        #region Events
        /// <summary>
        /// Occur when a property, that may requeire any calculation, changed.
        /// For exampe, when Size changed. 
        /// But, when IsVisible changed, this event won't occur.
        /// The string is the name of the property that changed.
        /// </summary>
        public event EventHandler<string> LookingChanged;
        public event EventHandler GameParentChanged;
        public event EventHandler ParentChanged;
        public event EventHandler RotationChanged;
        public event EventHandler OriginChanged;
        public event EventHandler SpriteEffectChanged;
        public event EventHandler SizeChanged;
        public event EventHandler TagChanged;
        public event EventHandler IsVisibleChanged;
        #endregion Events

        #region Events Occuration
        protected virtual void OnBeforeGameParentChanged()
        {
        }
        protected virtual void OnBeforeParentChanged()
        {
            UnregisterFromParentEvents();
        }
        protected void OnLookingChanged([CallerMemberName]string propertyChangedName = "")
        {
            if (!(GameParent is null) && !GameParent.IsLayoutSuspended)
            {
                LookingChanged?.Invoke(this, propertyChangedName);
            }
        }
        protected virtual void OnGameParentChanged(EventArgs e)
        {
            GameParentChanged?.Invoke(this, e);
        }
        protected virtual void OnParentChanged(EventArgs e)
        {
            RegisterToParentEvents();
            ParentChanged?.Invoke(this, e);
        }
        protected virtual void OnRotationChanged(EventArgs e)
        {
            RotationChanged?.Invoke(this, e);
        }
        protected virtual void OnOriginChanged(EventArgs e)
        {
            OriginChanged?.Invoke(this, e);
        }
        protected virtual void OnSpriteEffectChanged(EventArgs e)
        {
            SpriteEffectChanged?.Invoke(this, e);
        }
        protected virtual void OnSizeChanged(EventArgs e)
        {
            SizeChanged?.Invoke(this, e);
        }
        protected virtual void OnTagChanged(EventArgs e)
        {
            TagChanged?.Invoke(this, e);
        }
        protected virtual void OnIsVisibleChanged(EventArgs e)
        {
            IsVisibleChanged?.Invoke(this, e);
        }
        private void Parent_LookingChanged(object sender, string e)
        {
            OnLookingChanged("parent " + e);
        }
        private void GameParent_LayoutResumed(object sender, EventArgs e)
        {
            OnLookingChanged("LayoutResumed");
        }
        #endregion Events Occuration

        #region Register Events To...
        protected virtual void RegisterEventsToGameParent()
        {
            if(!(GameParent is null))
            {
                GameParent.LayoutResumed += GameParent_LayoutResumed;
            }
        }
        protected virtual void ResetEventsOfGameParent()
        {
            if(!(GameParent is null))
            {
                GameParent.LayoutResumed -= GameParent_LayoutResumed;
            }
        }

        protected virtual void RegisterToParentEvents()
        {
            if (!(Parent is null))
            {
                Parent.LookingChanged += Parent_LookingChanged;
            }
        }
        protected virtual void UnregisterFromParentEvents()
        {
            if (!(Parent is null))
            {
                Parent.LookingChanged -= Parent_LookingChanged;
            }

        }
        #endregion Register Events To...

        #region IGameObject implementation
        public abstract void Initialize();
        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void UnloadContent(ContentManager content);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        #endregion IGameObject implementation

    }
}
