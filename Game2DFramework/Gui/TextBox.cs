using System;
using System.Xml;
using Game2DFramework.Drawing;
using Game2DFramework.Gui.ItemDescriptors;
using Game2DFramework.Interaction;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui
{
    public class TextBox : GuiElement
    {
        private StringInputController _inputController;
        private SpriteText _spriteText;

        private NinePatchSprite _border;
        private ActionTimer _cursorAnimatorTimer;
        private bool _isCursorVisible;
        private bool _hasFocus;

        public InputType InputType
        {
            get { return _inputController.InputType; }
            set { _inputController.SetInputType(value); }
        }

        public TextBox(GuiSystem guiSystem) : base(guiSystem)
        {
            InitializeDrawElements();
        }

        public TextBox(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            InitializeDrawElements();

            if (element.HasAttribute("InputType"))
            {
                InputType = (InputType)Enum.Parse(typeof(InputType), element.GetAttribute("InputType"));
            }

            if (element.HasAttribute("MaxInputCharacters"))
            {
                MaxInputCharacters = int.Parse(element.GetAttribute("MaxInputCharacters"));
            }

            if (element.HasAttribute("Text"))
            {
                Text = element.GetAttribute("Text");
            }
        }

        private void InitializeDrawElements()
        {
            var itemDescriptor = GuiSystem.GetSkinItemDescriptor<TextBoxSkinItemDescriptor>();

            _border = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.NormalRectangle, itemDescriptor.Border);

            _spriteText = new SpriteText(itemDescriptor.NormalFont);
            _spriteText.HorizontalAlignment = HorizontalAlignment.Left; 

            _cursorAnimatorTimer = new ActionTimer(OnCursorAnimateTick, 0.5f, true);
            _cursorAnimatorTimer.Start();

            _inputController = new StringInputController(InputType.AlphaNumeric);
        }

        private void OnCursorAnimateTick()
        {
            _isCursorVisible = !_isCursorVisible;
        }

        public override Rectangle GetMinSize()
        {
            var minSizeOfTextBlock = _spriteText.GetBounds(true);
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

            _spriteText.SetTargetRectangle(borderBounds);
        }

        public override void Update(float elapsedTime)
        {
            _cursorAnimatorTimer.Update(elapsedTime);

            if (_hasFocus)
            {
                _inputController.Update(Game.Keyboard, elapsedTime);
                _spriteText.Text = _inputController.CurrentText;
            }
        }

        public override void Draw()
        {
            _border.Draw(Game.SpriteBatch);
            _spriteText.Draw(Game.SpriteBatch);

            if (_isCursorVisible && _hasFocus)
            {
                var bounds = _spriteText.GetBounds(true);
                Game.ShapeRenderer.DrawRectangle(bounds.Right + 1, bounds.Y, 1, bounds.Height, Color.White);
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

        public int MaxInputCharacters
        {
            get { return _inputController.MaxInputCharacters; }
            set { _inputController.MaxInputCharacters = value; }
        }

        public string Text
        {
            get { return _spriteText.Text; }
            set
            {
                _spriteText.Text = value;
                _inputController.CurrentText = value;
            }
        }
    }
}
