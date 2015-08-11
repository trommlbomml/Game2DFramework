using System;
using System.Xml;
using Game2DFramework.Drawing;
using Game2DFramework.Gui.ItemDescriptors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui
{
    public class Frame : ContentControl
    {
        private SpriteFont _headerFont;
        private NinePatchSprite _contentBorder;
        private NinePatchSprite _headerBorder;
        
        public string Title { get; set; }

        public Frame(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            InitializeDrawElements(guiSystem);
     
            if (element.HasAttribute("Title"))
            {
                Title = element.GetAttribute("Title");
            }
        }

        public Color Color
        {
            get { return _contentBorder.Color; }
            set
            {
                _contentBorder.Color = value;
                _headerBorder.Color = value;
            }
        }

        public Frame(GuiSystem guiSystem)
            : base(guiSystem)
        {
        }

        private void InitializeDrawElements(GuiSystem guiSystem)
        {
            var itemDescriptor = guiSystem.GetSkinItemDescriptor<FrameSkinItemDescriptor>();

            _headerFont = itemDescriptor.BigFont;
            _contentBorder = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.SourceRectangle, itemDescriptor.FrameBorder);
            _headerBorder = new NinePatchSprite(itemDescriptor.SkinTexture, itemDescriptor.SourceRectangle, itemDescriptor.FrameBorder);
        }

        public override Rectangle GetMinSize()
        {
            if (Child == null)
            {
                if(string.IsNullOrEmpty(Title)) return _contentBorder.MinSize;

                var headerSize = GetHeaderSize();
                var size = _headerBorder.MinSize;
                size.Width = Math.Max(size.Width, headerSize.Width);
                size.Height = Math.Max(size.Height, headerSize.Height);
                return size;
            }

            var childMinSize = Child.GetMinSize();

            var minSize = new Rectangle(0, 0, childMinSize.Width + _headerBorder.MinSize.Width, childMinSize.Height + _contentBorder.MinSize.Height);
            if (!string.IsNullOrEmpty(Title))
            {
                var headerSize = GetHeaderSize();
                minSize.Width = Math.Max(minSize.Width, headerSize.Width + _headerBorder.MinSize.Width);
                minSize.Height = headerSize.Height + childMinSize.Height + _headerBorder.MinSize.Height;
            }
            else
            {
                minSize.Height += _headerBorder.MinSize.Height;
            }

            return ApplyMarginAndHandleSize(minSize);
        }

        private Rectangle GetHeaderSize()
        {
            if (string.IsNullOrEmpty(Title)) return Rectangle.Empty;

            var textSize = _headerFont.MeasureString(Title).ToPoint();
            return new Rectangle(0, 0, textSize.X + _headerBorder.MinSize.Width, textSize.Y + _headerBorder.MinSize.Height);
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = RemoveMargin(target);

            _contentBorder.SetBounds(Bounds);

            var headerSize = GetHeaderSize();

            if (!string.IsNullOrEmpty(Title))
            {
                _headerBorder.SetBounds(new Rectangle(Bounds.X + Bounds.Width / 2 - headerSize.Width / 2, Bounds.Y, headerSize.Width, headerSize.Height));
                var bounds = _contentBorder.Bounds;
                bounds.Y += headerSize.Height / 2;
                bounds.Height -= headerSize.Height / 2;
                _contentBorder.SetBounds(bounds);
            }

            if (Child != null)
            {
                var rectangle = Bounds;
                rectangle.X += _contentBorder.FixedBorder.Top;
                rectangle.Y += string.IsNullOrEmpty(Title) ? _contentBorder.FixedBorder.Left: headerSize.Height;
                rectangle.Width -= _contentBorder.FixedBorder.Vertical;
                rectangle.Height -= string.IsNullOrEmpty(Title) ? _contentBorder.FixedBorder.Vertical : _contentBorder.FixedBorder.Top + headerSize.Height;
                Child.Arrange(rectangle);
            }
        }

        public override void Draw()
        {
            _contentBorder.Draw(Game.SpriteBatch);
            if (!string.IsNullOrEmpty(Title))
            {
                _headerBorder.Draw(Game.SpriteBatch);
                Game.SpriteBatch.DrawString(_headerFont, Title, new Vector2(_headerBorder.Bounds.X + _headerBorder.FixedBorder.Left, _headerBorder.Bounds.Y + _headerBorder.FixedBorder.Top), Color.White);
            }

            base.Draw();
        }
    }
}
