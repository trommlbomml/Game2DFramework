using System;
using System.Xml;
using Game2DFramework.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui
{
    public class ScrollViewer : ContentControl
    {
        public bool CanScrollHorizontally { get; set; }
        public bool CanScrollVertically { get; set; }

        private RasterizerState _scissorTestRasterizerState;
        private Rectangle _clientClipBounds;
        private bool _isOverScrollbar;
        private ScrollRepresenter _verticalScrollRepresenter;

        public ScrollViewer(GuiSystem guiSystem) : base(guiSystem)
        {
            InitializeDrawElements();
        }

        private void InitializeDrawElements()
        {
            var original = Game.GraphicsDevice.RasterizerState;

            _scissorTestRasterizerState = new RasterizerState
            {
                CullMode = original.CullMode,
                DepthBias = original.DepthBias,
                DepthClipEnable = original.DepthClipEnable,
                FillMode = original.FillMode,
                MultiSampleAntiAlias = original.MultiSampleAntiAlias,
                ScissorTestEnable = true,
                SlopeScaleDepthBias = original.SlopeScaleDepthBias,
            };

            _verticalScrollRepresenter = new ScrollRepresenter();
        }

        public ScrollViewer(GuiSystem guiSystem, XmlElement element) : base(guiSystem, element)
        {
            if (element.HasAttribute("CanScrollHorizontally"))
            {
                CanScrollHorizontally = Boolean.Parse(element.GetAttribute("CanScrollHorizontally"));
            }
            if (element.HasAttribute("CanScrollVertically"))
            {
                CanScrollVertically = Boolean.Parse(element.GetAttribute("CanScrollVertically"));
            }

            InitializeDrawElements();
        }

        private const int SliderSize = 20;

        public override Rectangle GetMinSize()
        {
            var childSize = Child != null ? Child.GetMinSize() : Rectangle.Empty;

            var size = new Rectangle
            {
                Width = CanScrollHorizontally ? SliderSize : (childSize.Width + (CanScrollVertically ? SliderSize : 0)),
                Height = CanScrollVertically ? SliderSize : (childSize.Height + (CanScrollHorizontally ? SliderSize : 0))
            };

            return ApplyMarginAndHandleSize(size);
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = RemoveMargin(target);

            var childSize = Child != null ? Child.GetMinSize() : Rectangle.Empty;

            var childArrange = Bounds;
            childArrange.Width = CanScrollHorizontally ? childSize.Width : Bounds.Width;
            childArrange.Height = CanScrollVertically ? childSize.Height : Bounds.Height;

            _clientClipBounds = Bounds;
            _clientClipBounds.Width -= CanScrollVertically ? SliderSize : 0;
            _clientClipBounds.Height -= CanScrollHorizontally ? SliderSize : 0;

            if (Child != null) Child.Arrange(childArrange);

            if (CanScrollVertically)
            {
                _verticalScrollRepresenter.SetRange(_clientClipBounds.Height, childArrange.Height, SliderSize);
            }
        }

        public override void Translate(int x, int y)
        {
            Bounds = Bounds.Translate(x,y);
            if (Child != null) Child.Translate(x,y);
        }

        public override void Draw()
        {
            var spriteBatch = Game.SpriteBatch;
            var graphicsDevice = Game.GraphicsDevice;
            
            spriteBatch.End();

            graphicsDevice.ScissorRectangle = _clientClipBounds;

            spriteBatch.Begin(rasterizerState: _scissorTestRasterizerState);
            if (Child != null) Child.Translate(0, -_verticalScrollRepresenter.ScrollValueCalculated);
            base.Draw();
            spriteBatch.End();
            if (Child != null) Child.Translate(0, _verticalScrollRepresenter.ScrollValueCalculated);

            spriteBatch.Begin();

            if (CanScrollVertically)
            {
                Game.ShapeRenderer.DrawFilledRectangle(_clientClipBounds.Right + 1, _clientClipBounds.Top, SliderSize, _clientClipBounds.Height, Color.Red);

                var thumb = GetThumbRectangleVertical();

                Game.ShapeRenderer.DrawFilledRectangle(thumb.X, thumb.Y, thumb.Width, thumb.Height, Color.Blue);
            }
        }

        private Rectangle GetThumbRectangleVertical()
        {
            return new Rectangle(_clientClipBounds.Right + 1, _clientClipBounds.Top + _verticalScrollRepresenter.ScrollValueDisplayed, SliderSize, SliderSize);
        }

        public override void OnMouseLeft(EventHandler handler)
        {
            _isOverScrollbar = false;
        }

        public override void OnMouseOver(EventHandler handler)
        {
            handler.Handled = GetThumbRectangleVertical().Contains(Game.Mouse.X, Game.Mouse.Y);
        }

        public override void OnMouseDown(EventHandler handler)
        {
            _isOverScrollbar = GetThumbRectangleVertical().Contains(Game.Mouse.X, Game.Mouse.Y);
            if (_isOverScrollbar)
            {
                handler.Handled = true;
            }
        }

        public override void OnMouseMove(MouseMovedEventHandler handler)
        {
            if (!_isOverScrollbar) return;
            _verticalScrollRepresenter.Scroll(handler.Y);
        }

        public override void OnMouseUp(EventHandler handler)
        {
            _isOverScrollbar = false;
        }
    }
}
