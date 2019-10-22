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
    public class Menu : Panel, ILocation, ITextDrawing
    {
        private Label _headLineLabel;

        public string Text { get => ((ITextDrawing)_headLineLabel).Text; set => ((ITextDrawing)_headLineLabel).Text = value; }
        public float TextScale { get => ((ITextDrawing)_headLineLabel).TextScale; set => ((ITextDrawing)_headLineLabel).TextScale = value; }
        public SpriteFont Font { get => ((ITextDrawing)_headLineLabel).Font; set => ((ITextDrawing)_headLineLabel).Font = value; }
        public Color ForeColor { get => ((ITextDrawing)_headLineLabel).ForeColor; set => ((ITextDrawing)_headLineLabel).ForeColor = value; }

        public Rectangle TextRectangle
        {
            get;
            set;
        }
        public Anchor TextAnchor { get => ((ITextDrawing)_headLineLabel).TextAnchor; set => ((ITextDrawing)_headLineLabel).TextAnchor = value; }

        public Menu(GamePrototype gameParent) : base(gameParent)
        {
            _headLineLabel = new Label(gameParent)
            {
                Text = "Head Line",
                Parent = this,
                ForeColor = Color.Black,
                RectangleOfContentAfterResizeing = true,
                BkgTransparent = true,
                TextAnchor = Anchor.Center | Anchor.Middle,
            };

            Controls.Add(_headLineLabel);

            LookingChanged += OnLookingChanged;
        }

        private void OnLookingChanged(object sender, string e)
        {
            LabelLocationCalc();
        }

        private void LabelLocationCalc()
        {
            _headLineLabel.Location = (TextRectangle.Location.ToVector2() * Scale);
            _headLineLabel.Size = (TextRectangle.Size.ToVector2() * Scale);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        private Texture2D _headerLineBkg;
        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            _headerLineBkg = new Texture2D(GameParent.GraphicsDevice, 1, 1);
            _headerLineBkg.SetData(new[] { Color.White });
            _headLineLabel.RegTexture = _headerLineBkg;

        }

        public override void UnloadContent(ContentManager content)
        {
            base.UnloadContent(content);

            _headerLineBkg.Dispose();
        }

        protected override void InUpdate(GameTime gameTime)
        {
            base.InUpdate(gameTime);
            _headLineLabel.Update(gameTime);
        }
        protected override void InDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.InDraw(gameTime, spriteBatch);

        }

        protected override void OnGameParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
        }

        protected override void OnScaleChanged(EventArgs e)
        {
            base.OnScaleChanged(e);
        }
    }
}
