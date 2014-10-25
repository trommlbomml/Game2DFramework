using System;
using System.Xml;
using Game2DFramework.Drawing;
using Game2DFramework.Extensions;
using Game2DFramework.Gui.ItemDescriptors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui
{
    public class Frame : GuiElement
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

            if (element.HasChildNodes) SetContent(CreateFromXmlType(guiSystem, (XmlElement)element.FirstChild));
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

        public void SetContent(GuiElement guiElement)
        {
            if (Children.Count == 1) Children.Clear();
            Children.Add(guiElement);
        }

        public override Rectangle GetMinSize()
        {
            if (Children.Count == 0)
            {
                if(string.IsNullOrEmpty(Title)) return _contentBorder.MinSize;

                var headerSize = GetHeaderSize();
                var size = _headerBorder.MinSize;
                size.Width = Math.Max(size.Width, headerSize.Width);
                size.Height = Math.Max(size.Height, headerSize.Height);
                return size;
            }

            var childMinSize = Children[0].GetMinSize();

            var minSize = new Rectangle(0, 0, childMinSize.Width + _headerBorder.MinSize.Width, _contentBorder.MinSize.Height);
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

            return minSize;
        }

        private Rectangle GetHeaderSize()
        {
            if (string.IsNullOrEmpty(Title)) return Rectangle.Empty;

            var textSize = _headerFont.MeasureString(Title).ToPoint();
            return new Rectangle(0, 0, textSize.X + _headerBorder.MinSize.Width, textSize.Y + _headerBorder.MinSize.Height);
        }

        public override void Arrange(Rectangle target)
        {
            _contentBorder.SetBounds(target);

            var headerSize = GetHeaderSize();

            if (!string.IsNullOrEmpty(Title))
            {
                _headerBorder.SetBounds(new Rectangle(target.X + target.Width / 2 - headerSize.Width / 2, target.Y, headerSize.Width, headerSize.Height));
                var bounds = _contentBorder.Bounds;
                bounds.Y += headerSize.Height / 2;
                bounds.Height -= headerSize.Height / 2;
                _contentBorder.SetBounds(bounds);
            }

            if (Children.Count == 1)
            {
                var rectangle = target;
                rectangle.X += _contentBorder.FixedBorder.Top;
                rectangle.Y += string.IsNullOrEmpty(Title) ? _contentBorder.FixedBorder.Left: headerSize.Height;
                rectangle.Width -= _contentBorder.FixedBorder.Vertical;
                rectangle.Height -= string.IsNullOrEmpty(Title) ? _contentBorder.FixedBorder.Vertical : _contentBorder.FixedBorder.Top + headerSize.Height;
                Children[0].Arrange(rectangle);
            }
        }

        public override void Draw()
        {
            _contentBorder.Draw(Game.SpriteBatch, Color.White);
            if (!string.IsNullOrEmpty(Title))
            {
                _headerBorder.Draw(Game.SpriteBatch, Color.White);
                Game.SpriteBatch.DrawString(_headerFont, Title, new Vector2(_headerBorder.Bounds.X + _headerBorder.FixedBorder.Left, _headerBorder.Bounds.Y + _headerBorder.FixedBorder.Top), Color.White);
            }

            if (Children.Count == 1)
            {
                var child = Children[0];
                child.Draw();
            }
        }

        public override void Update(float elapsedTime)
        {
            if (Children.Count == 1) Children[0].Update(elapsedTime);
        }
    }
}
