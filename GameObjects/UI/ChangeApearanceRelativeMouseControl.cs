using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.GameObjects.UI
{
    /// <summary>
    /// Change apearance and audio relative mouse.
    /// </summary>
    public abstract class ChangeApearanceRelativeMouseControl : Control
    {
        public ChangeApearanceRelativeMouseControl(GamePrototype gameParent) : base(gameParent)
        {
        }

        private Texture2D _hoverTexture;
        public Texture2D HoverTexture
        {
            get => _hoverTexture is null ? 
                                ControlApearance is null ? null : ControlApearance.MouseHover
                            : _hoverTexture;

            set
            {
                _hoverTexture = value;
            }
        }

        private Texture2D _pressedTexture;
        public Texture2D PressedTexture
        {
            get => _pressedTexture is null ? 
                                ControlApearance is null ? null : ControlApearance.MouseDown 
                            : _pressedTexture;

            set
            {
                _pressedTexture = value;
            }
        }

        protected override Texture2D TextureToDrawIfEnabled
        {
            get
            {
                if (PressedTexture is null && HoverTexture is null)
                {
                    return RegTexture;
                }


                if (IsPressing && !(PressedTexture is null))
                {
                    return PressedTexture;
                }

                if (IsHover && !(HoverTexture is null))
                {
                    return HoverTexture;
                }


                return RegTexture;
            }
        }

        #region AUDIO
        private SoundEffect _audioInHovered;
        public SoundEffect AudioInHovered 
        { 
            get
            {
                if(_audioInHovered is null)
                {
                    return ControlApearance is null ? null : ControlApearance.MouseHoverAudio;
                }

                return _audioInHovered;
            }
            set => _audioInHovered = value;
        }


        private SoundEffect _audioInPressed;
        public SoundEffect AudioInPressed
        {
            get
            {
                if (_audioInPressed is null)
                {
                    return ControlApearance is null ? null : ControlApearance.MouseDownAudio;
                }

                return _audioInPressed;
            }
            set => _audioInPressed = value;
        }
        #endregion AUDIO


        protected override void OnHovered()
        {
            OnLookingChanged();
            //TODO: I wanted to ignore audio when mouse hover but is not pressed. why?
            if (/*!IsPressed &&*/ !(AudioInHovered is null))
            {
                AudioInHovered.Play();
            }
            base.OnHovered();
        }
        protected override void OnClicked()
        {
            OnLookingChanged();
            if (!(AudioInPressed is null))
            {
                AudioInPressed.Play();
            }
            base.OnClicked();
        }

    }
}
