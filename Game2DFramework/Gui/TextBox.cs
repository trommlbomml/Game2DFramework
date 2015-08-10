using System;
using System.Xml;
using Game2DFramework.Drawing;
using Game2DFramework.Gui.ItemDescriptors;
using Game2DFramework.Interaction;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui
{
    public class TextBox : GuiElement
    {
        private readonly TextBlock _contentElement;
        private SpriteFont _spriteFont;

        private NinePatchSprite _border;
        private ActionTimer _cursorAnimatorTimer;
        private bool _isCursorVisible;

        public TextBox(GuiSystem guiSystem) : base(guiSystem)
        {
            _contentElement = new TextBlock(GuiSystem);
            InitializeDrawElements();
        }

        public TextBox(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            _contentElement = new TextBlock(GuiSystem);
            InitializeDrawElements();
        }

        private void InitializeDrawElements()
        {
            var itemDescriptor = GuiSystem.GetSkinItemDescriptor<TextBoxSkinItemDescriptor>();

            _border = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.NormalRectangle, itemDescriptor.Border);
            _spriteFont = itemDescriptor.NormalFont;

            _cursorAnimatorTimer = new ActionTimer(OnCursorAnimateTick, 0.5f, true);
            _cursorAnimatorTimer.Start();
        }

        private void OnCursorAnimateTick()
        {
            _isCursorVisible = !_isCursorVisible;
        }

        public override Rectangle GetMinSize()
        {
            var minSizeOfTextBlock = _contentElement.GetMinSize(true);
            minSizeOfTextBlock.Width += _border.FixedBorder.Horizontal;
            minSizeOfTextBlock.Height += _border.FixedBorder.Vertical;

            return ApplyMarginAndHandleSize(minSizeOfTextBlock);
        }

        public override void Arrange(Rectangle target)
        {
            var borderBounds = RemoveMargin(target);
            _border.SetBounds(borderBounds);

            borderBounds.Width -= _border.FixedBorder.Horizontal;
            borderBounds.Height -= _border.FixedBorder.Vertical;
            borderBounds.X += _border.FixedBorder.Left;
            borderBounds.Y += _border.FixedBorder.Top;

            _contentElement.Arrange(borderBounds);
        }

        public override void Update(float elapsedTime)
        {
            _contentElement.Update(elapsedTime);
            _cursorAnimatorTimer.Update(elapsedTime);
        }

        public override void Draw()
        {
            _border.Draw(Game.SpriteBatch, Color.White);
            _contentElement.Draw();

            if (_isCursorVisible)
            {
                var cursorPos = new Vector2(_contentElement.Bounds.Left, _contentElement.Bounds.Top);
                Game.SpriteBatch.DrawString(_spriteFont, "I", cursorPos, _contentElement.Color);
            }
        }
    }
}
