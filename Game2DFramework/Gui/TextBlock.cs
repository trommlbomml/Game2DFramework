using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Game2DFramework.Extensions;
using Game2DFramework.Gui.ItemDescriptors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui
{
    class TextBlock : GuiElement
    {
        protected SpriteFont Font { get; private set; }

        public string Text { get; set; }
        public Color Color { get; set; }

        public TextBlock(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            InitializeDrawElements();
            if (element.HasAttribute("Text")) Text = element.GetAttribute("Text");
        }

        public TextBlock(GuiSystem guiSystem)
            : base(guiSystem)
        {
            InitializeDrawElements();
        }

        private void InitializeDrawElements()
        {
            Color = Color.White;
            var descriptor = GuiSystem.GetSkinItemDescriptor<TextBlockSkinItemDescriptor>();
            Font = descriptor.NormalFont;
        }

        public override Rectangle GetMinSize()
        {
            if (string.IsNullOrEmpty(Text)) return ApplyMarginAndHandleSize(new Rectangle());

            var size = Font.MeasureString(Text);
            return ApplyMarginAndHandleSize(new Rectangle(0, 0, (int)Math.Round(size.X), (int)Math.Round(size.Y)));
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = RemoveMargin(new Rectangle(target.X + Margin.Left,
                                   target.Y + Margin.Top,
                                   target.Width - Margin.Horizontal,
                                   target.Height - Margin.Vertical));
        }

        public override void Draw()
        {
            if (string.IsNullOrEmpty(Text)) return;

            var size = Font.MeasureString(Text);
            var offset = new Vector2(Bounds.Width * 0.5f - size.X * 0.5f, Bounds.Height * 0.5f - size.Y * 0.5f);

            Game.SpriteBatch.DrawString(Font, Text, (new Vector2(Bounds.X, Bounds.Y) + offset).SnapToPixels(), Color);
        }

        public override void Update(float elapsedTime)
        {

        }
    }
}
