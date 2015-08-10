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
        private StringInputController _inputController;
        private readonly TextBlock _contentElement;
        private SpriteFont _spriteFont;

        private NinePatchSprite _border;
        private ActionTimer _cursorAnimatorTimer;
        private bool _isCursorVisible;
        private bool _hasFocus;

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

            _inputController = new StringInputController(InputType.AlphaNumeric, 10);
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
            Bounds = RemoveMargin(target);
            _border.SetBounds(Bounds);

            var borderBounds = Bounds;
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

            if (_hasFocus)
            {
                _inputController.Update(Game.Keyboard, elapsedTime);
                _contentElement.Text = _inputController.CurrentText;
            }
        }

        public override void Draw()
        {
            _border.Draw(Game.SpriteBatch, Color.White);
            _contentElement.Draw();

            if (_isCursorVisible && _hasFocus)
            {
                var cursorPos = new Vector2(_contentElement.Bounds.Left, _contentElement.Bounds.Top);
                Game.SpriteBatch.DrawString(_spriteFont, "I", cursorPos, _contentElement.Color);
            }
        }

        public override GuiElement OnGotFocus()
        {
            _hasFocus = true;
            return this;
        }

        public override void OnFocusLost()
        {
            _hasFocus = false;
        }
    }
}
