using System;
using System.Xml;
using Game2DFramework.Drawing;
using Game2DFramework.Gui.ItemDescriptors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui
{
    public class TextBox : GuiElement
    {
        private SpriteFont _font;
        private NinePatchSprite _border;

        public string Text { get; set; }

        public TextBox(GuiSystem guiSystem) : base(guiSystem)
        {
            InitializeDrawElements();
        }

        public TextBox(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            InitializeDrawElements();
        }

        private void InitializeDrawElements()
        {
            var itemDescriptor = GuiSystem.GetSkinItemDescriptor<TextBoxSkinItemDescriptor>();

            _border = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.NormalRectangle, itemDescriptor.Border);
            _font = itemDescriptor.NormalFont;
        }

        public override Rectangle GetMinSize()
        {
            return ApplyMarginAndHandleSize(new Rectangle());
        }

        public override void Arrange(Rectangle target)
        {
            _border.SetBounds(RemoveMargin(target));
        }

        public override void Draw()
        {
            _border.Draw(Game.SpriteBatch, Color.White);
        }

        public override void Update(float elapsedTime)
        {
        }
    }
}
