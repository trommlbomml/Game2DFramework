using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui
{
    public abstract class GuiElement : GameObject
    {
        public const int SizeNotSet = int.MinValue;

        protected GuiSystem GuiSystem { get; private set; }

        public static GuiElement CreateFromXmlType(GuiSystem guiSystem, XmlElement element)
        {
            switch (element.LocalName)
            {
                case "Frame": return new Frame(guiSystem, element);
                case "TextBlock": return new TextBlock(guiSystem, element);
                case "StackPanel": return new StackPanel(guiSystem, element);
                case "Image": return new Image(guiSystem, element);
                case "Grid": return new Grid(guiSystem, element);
                case "Button": return new Button(guiSystem, element);
            }

            throw new ArgumentException("Invalid Element Type", "element");
        }

        protected GuiElement(GuiSystem guiSystem)
            : base(guiSystem.Game)
        {
            GuiSystem = guiSystem;
            Children = new List<GuiElement>();
        }

        protected GuiElement(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem.Game)
        {
            GuiSystem = guiSystem;
            Children = new List<GuiElement>();

            if (element.HasAttribute("Width")) Width = int.Parse(element.GetAttribute("Width"));
            if (element.HasAttribute("Height")) Width = int.Parse(element.GetAttribute("Height"));
            if (element.HasAttribute("Margin")) Margin = Thickness.Parse(element.GetAttribute("Margin"));
            if (element.HasAttribute("Id")) Id = element.GetAttribute("Id");
        }

        public TGuiElement FindGuiElementById<TGuiElement>(string id) where TGuiElement : GuiElement
        {
            if (Id == id) return (TGuiElement)this;
            foreach (var child in Children)
            {
                var element = child.FindGuiElementById<TGuiElement>(id);
                if (element != null) return element;
            }

            return null;
        }

        protected Rectangle ApplyMarginAndHandleSize(Rectangle rectangle)
        {
            rectangle.X -= Margin.Left;
            rectangle.Y -= Margin.Top;
            rectangle.Width = Math.Max(Width, rectangle.Width + Margin.Horizontal);
            rectangle.Height = Math.Max(Height, rectangle.Height + Margin.Vertical);

            return rectangle;
        }

        protected Rectangle RemoveMargin(Rectangle rectangle)
        {
            rectangle.X += Margin.Left;
            rectangle.Y += Margin.Top;
            rectangle.Width -= Margin.Horizontal;
            rectangle.Height -= Margin.Vertical;
            return rectangle;
        }

        public string Id { get; private set; }
        public List<GuiElement> Children { get; private set; }
        public Rectangle Bounds { get; protected set; }
        public Thickness Margin { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public abstract Rectangle GetMinSize();

        public abstract void Arrange(Rectangle target);

        public abstract void Draw();

        public abstract void Update(float elapsedTime);
    }
}
