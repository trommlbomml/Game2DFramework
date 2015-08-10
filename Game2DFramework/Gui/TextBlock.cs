using System;
using System.Runtime.InteropServices;
using System.Xml;
using Game2DFramework.Drawing;
using Game2DFramework.Gui.ItemDescriptors;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui
{
    public class TextBlock : GuiElement
    {
        private SpriteText _spriteText;

        public string Text
        {
            get { return _spriteText.Text; }
            set
            {
                _spriteText.Text = value;
            }
        }

        public Color Color
        {
            get { return _spriteText.Color; }
            set { _spriteText.Color = value; }
        }

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
            var descriptor = GuiSystem.GetSkinItemDescriptor<TextBlockSkinItemDescriptor>();
            _spriteText = new SpriteText(descriptor.NormalFont);

            _spriteText.HorizontalAlignment = HorizontalAlignment;
            _spriteText.VerticalAlignment = VerticalAlignment;
        }

        internal Rectangle GetMinSize(bool includeHeightForEmptyText)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return ApplyMarginAndHandleSize(includeHeightForEmptyText ? new Rectangle(0,0,0,_spriteText.LineSpacing) : new Rectangle());
            }

            return ApplyMarginAndHandleSize(new Rectangle(0, 0, (int)Math.Round(_spriteText.TextSize.X), (int)Math.Round(_spriteText.TextSize.Y)));
        }

        public override Rectangle GetMinSize()
        {
            return GetMinSize(false);
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = RemoveMargin(target);
            _spriteText.SetTargetRectangle(Bounds);
        }

        public override void Draw()
        {
            _spriteText.Draw(Game.SpriteBatch);
        }
    }
}
