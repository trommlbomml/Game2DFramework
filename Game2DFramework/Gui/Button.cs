using System;
using System.Xml;
using Game2DFramework.Drawing;
using Game2DFramework.Gui.ItemDescriptors;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui
{
    public class Button : ContentControl
    {
        private NinePatchSprite _normalSprite;
        private NinePatchSprite _hoverSprite;
        private bool _isMouseOver;

        public event Action Click;

        public Button(GuiSystem guiSystem) : base(guiSystem)
        {
            InitializeDrawElements(guiSystem);
        }

        public Button(GuiSystem guiSystem, XmlElement element) : base(guiSystem, element)
        {
            InitializeDrawElements(guiSystem);
        }

        private void InitializeDrawElements(GuiSystem guiSystem)
        {
            var itemDescriptor = guiSystem.GetSkinItemDescriptor<ButtonSkinItemDescriptor>();

            _normalSprite = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.NormalRectangle, itemDescriptor.ButtonBorder);
            _hoverSprite = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.HoverRectangle, itemDescriptor.ButtonBorder);
        }

        public override Rectangle GetMinSize()
        {
            if (Child == null) return ApplyMarginAndHandleSize(_normalSprite.MinSize);

            var childMinSize = Child.GetMinSize();
            return ApplyMarginAndHandleSize(new Rectangle(0, 0, childMinSize.Width + _normalSprite.FixedBorder.Horizontal, childMinSize.Height + _normalSprite.FixedBorder.Vertical));
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = RemoveMargin(target);

            _normalSprite.SetBounds(Bounds);
            _hoverSprite.SetBounds(Bounds);

            if (Child != null)
            {
                var rectangle = target;
                rectangle.X += _normalSprite.FixedBorder.Top;
                rectangle.Y += _normalSprite.FixedBorder.Left;
                rectangle.Width -= _normalSprite.FixedBorder.Vertical;
                rectangle.Height -= _normalSprite.FixedBorder.Vertical;
                Child.Arrange(rectangle);
            }
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);

            if (Game.Mouse.IsLeftButtonClicked() && _isMouseOver && Click != null)
            {
                Click();
            }
        }

        public override void OnClick()
        {
            if (Click != null) Click();
        }

        public override GuiElement OnMouseOver()
        {
            _isMouseOver = true;
            return this;
        }

        public override void OnMouseLeft()
        {
            _isMouseOver = false;
        }

        public override void Draw()
        {
            if (_isMouseOver)
            {
                _hoverSprite.Draw(Game.SpriteBatch, Color.White);
            }
            else
            {
                _normalSprite.Draw(Game.SpriteBatch, Color.White);   
            }
            base.Draw();
        }
    }
}
