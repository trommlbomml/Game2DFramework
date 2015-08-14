using System;
using System.Diagnostics;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui
{
    public class ScrollViewer : ContentControl
    {
        public bool CanScrollHorizontally { get; set; }
        public bool CanScrollVertically { get; set; }
        public int ScrollOffsetX { get; set; }
        public int ScrollOffsetY { get; set; }

        private RasterizerState _scissorTestRasterizerState;
        private Rectangle _clientClipBounds;
        private float _scrollPixelsPerSlideUnitY;
        private bool _isOverScrollbar;
        private int _maxScrollY;

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

            if (Child != null)
            {
                Child.Arrange(childArrange);
            }

            if (CanScrollVertically)
            {
                _maxScrollY = childArrange.Height - _clientClipBounds.Height;
                var scrollableAreaY = _clientClipBounds.Height/4*3;

                _scrollPixelsPerSlideUnitY = _maxScrollY / (float)scrollableAreaY;
            }
            else
            {
                _scrollPixelsPerSlideUnitY = 0;
            }
        }

        public override void Draw()
        {
            var spriteBatch = Game.SpriteBatch;
            var graphicsDevice = Game.GraphicsDevice;
            
            spriteBatch.End();

            graphicsDevice.ScissorRectangle = _clientClipBounds;

            var translationY = (float)Math.Round(ScrollOffsetY*_scrollPixelsPerSlideUnitY);
            Debug.WriteLine("Translation: " + translationY);
            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(ScrollOffsetX, -translationY, 0.0f), rasterizerState: _scissorTestRasterizerState);
            base.Draw();
            spriteBatch.End();

            spriteBatch.Begin();

            if (CanScrollVertically)
            {
                Game.ShapeRenderer.DrawFilledRectangle(_clientClipBounds.Right + 1, _clientClipBounds.Top, SliderSize, _clientClipBounds.Height, Color.Red);

                var thumb = GetThumbRectangleVertical();

                Game.ShapeRenderer.DrawFilledRectangle(thumb.X, thumb.Y + ScrollOffsetY, thumb.Width, thumb.Height, Color.Blue);
            }
        }

        private Rectangle GetThumbRectangleVertical()
        {
            return new Rectangle(_clientClipBounds.Right + 1, _clientClipBounds.Top, SliderSize, _clientClipBounds.Height / 4);
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
            ScrollOffsetY = MathHelper.Clamp(ScrollOffsetY + handler.Y, 0, _maxScrollY);
        }

        public override void OnMouseUp(EventHandler handler)
        {
            _isOverScrollbar = false;
        }
    }
}
