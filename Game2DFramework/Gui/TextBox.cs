using System;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui
{
    public class TextBox : GuiElement
    {
        public string Text { get; set; }

        public TextBox(GuiSystem guiSystem) : base(guiSystem)
        {

        }

        public TextBox(GuiSystem guiSystem, XmlElement element) : base(guiSystem, element)
        {
        }

        public override Rectangle GetMinSize()
        {
            throw new NotImplementedException();
        }

        public override void Arrange(Rectangle target)
        {
            throw new NotImplementedException();
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }

        public override void Update(float elapsedTime)
        {
            throw new NotImplementedException();
        }
    }
}
