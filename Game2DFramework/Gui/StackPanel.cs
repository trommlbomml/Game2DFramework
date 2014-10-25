using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui
{
    public enum Orientation
    {
        Vertical,
        Horizontal
    }

    public class StackPanel : GuiElement
    {
        public StackPanel(GuiSystem guiSystem)
            : base(guiSystem)
        {
        }

        public StackPanel(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            if(element.HasAttribute("Orientation"))
            {
                Orientation = (Orientation)Enum.Parse(typeof(Orientation), element.GetAttribute("Orientation"));
            }
            
            foreach (XmlElement childElement in element.ChildNodes)
            {
                AddChild(CreateFromXmlType(guiSystem, childElement));
            }
        }

        public Orientation Orientation { get; set; }

        public override Rectangle GetMinSize()
        {
            var size = new Rectangle();

            foreach (var guiElement in Children)
            {
                var sizeOfChildElement = guiElement.GetMinSize();

                if (Orientation == Orientation.Vertical)
                {
                    size.Width = Math.Max(size.Width, sizeOfChildElement.Width);
                    size.Height += sizeOfChildElement.Height;
                }
                else
                {
                    size.Height = Math.Max(size.Height, sizeOfChildElement.Height);
                    size.Width += sizeOfChildElement.Width;
                }
            }

            size.X -= Margin.Left;
            size.Y -= Margin.Top;
            size.Height += Margin.Vertical;
            size.Width += Margin.Horizontal;

            return size;
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = new Rectangle(target.X + Margin.Left, target.Y + Margin.Top, target.Width - Margin.Horizontal, target.Height - Margin.Vertical);

            var startX = Bounds.X;
            var startY = Bounds.Y;

            foreach (var guiElement in Children)
            {
                var minSize = guiElement.GetMinSize();
                var childArrange = Orientation == Orientation.Vertical
                    ? new Rectangle(startX, startY, Bounds.Width, minSize.Height)
                    : new Rectangle(startX, startY, minSize.Width, Bounds.Height);

                if (Orientation == Orientation.Vertical)
                {
                    startY += minSize.Height;
                }
                else
                {
                    startX += minSize.Width;
                }

                guiElement.Arrange(childArrange);
            }
        }

        public void AddChild(GuiElement child)
        {
            Children.Add(child);
        }

        public override void Draw()
        {
            Children.ForEach(c => c.Draw());
        }

        public override void Update(float elapsedTime)
        {
            foreach (var guiElement in Children)
            {
                guiElement.Update(elapsedTime);
            }
        }
    }
}
