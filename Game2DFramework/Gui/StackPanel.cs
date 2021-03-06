﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Game2DFramework.Extensions;
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

            return ApplyMarginAndHandleSize(size);
        }

        public override void Arrange(Rectangle target)
        {
            Bounds = RemoveMargin(target);

            var arrangedBounds = new Rectangle();
            var startX = Bounds.X;
            var startY = Bounds.Y;
            arrangedBounds.X = startX;
            arrangedBounds.Y = startY;

            if (Orientation == Orientation.Vertical)
            {
                arrangedBounds.Width = target.Width;
            }
            else
            {
                arrangedBounds.Height = target.Height;
            }

            var childBounds = new List<Rectangle>();

            foreach (var guiElement in Children)
            {
                var minSize = guiElement.GetMinSize();
                var childArrange = Orientation == Orientation.Vertical
                    ? new Rectangle(startX, startY, Bounds.Width, minSize.Height)
                    : new Rectangle(startX, startY, minSize.Width, Bounds.Height);

                if (Orientation == Orientation.Vertical)
                {
                    startY += minSize.Height;
                    arrangedBounds.Height += minSize.Height;
                }
                else
                {
                    startX += minSize.Width;
                    arrangedBounds.Width += minSize.Width;
                }

                childBounds.Add(childArrange);
            }

            var newBoundsAligned = ArrangeToAlignments(target, arrangedBounds);
            var differenceX = newBoundsAligned.X - target.X;
            var differenceY = newBoundsAligned.Y - target.Y;

            var i = 0;
            foreach (var guiElement in Children)
            {
                var rectangle = childBounds.ElementAt(i++).Translate(differenceX, differenceY);
                guiElement.Arrange(rectangle);
            }
        }

        public override void Translate(int x, int y)
        {
            Bounds = Bounds.Translate(x, y);
            foreach(var child in Children) child.Translate(x,y);
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
