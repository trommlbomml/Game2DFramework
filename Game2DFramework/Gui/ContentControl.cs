using System.Linq;
using System.Xml;

namespace Game2DFramework.Gui
{
    public abstract class ContentControl : GuiElement
    {
        public GuiElement Child
        {
            get { return Children.FirstOrDefault(); } 
            set 
            {
                if (value == null)
                {
                    Children.Clear();
                }
                else
                {
                    if (Children.Count == 0)
                    {
                        Children.Add(value);
                    }
                    else
                    {
                        Children[0] = value;
                    }
                }
            }
        }

        protected ContentControl(GuiSystem guiSystem) : base(guiSystem)
        {
        }

        protected ContentControl(GuiSystem guiSystem, XmlElement element) : base(guiSystem, element)
        {
            if (element.HasChildNodes)
            {
                Child = CreateFromXmlType(guiSystem, (XmlElement)element.FirstChild);
            }
        }

        public override void Update(float elapsedTime)
        {
            if (Child != null) Child.Update(elapsedTime);
        }

        public override void Draw()
        {
            if (Child != null) Child.Draw();
        }
    }
}
