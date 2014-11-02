using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui.ItemDescriptors
{
    public class ButtonSkinItemDescriptor : SkinItemDescriptor
    {
        public string NodeName { get { return "Button"; } }

        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }

        public void Deserialize(XmlElement element)
        {
            ButtonBorder = Thickness.Parse(element.GetAttribute("Border"));

            var normalElement = (XmlElement)element.SelectSingleNode("NormalRectangle");
            var hoverElement = (XmlElement)element.SelectSingleNode("HoverRectangle");

// ReSharper disable PossibleNullReferenceException
            NormalRectangle = new Rectangle(int.Parse(normalElement.GetAttribute("Left")),
                                            int.Parse(normalElement.GetAttribute("Top")),
                                            int.Parse(normalElement.GetAttribute("Width")),
                                            int.Parse(normalElement.GetAttribute("Height")));

            HoverRectangle = new Rectangle(int.Parse(hoverElement.GetAttribute("Left")),
                                            int.Parse(hoverElement.GetAttribute("Top")),
                                            int.Parse(hoverElement.GetAttribute("Width")),
                                            int.Parse(hoverElement.GetAttribute("Height")));
// ReSharper restore PossibleNullReferenceException
        }

        public Thickness ButtonBorder { get; set; }
        public Rectangle NormalRectangle { get; set; }
        public Rectangle HoverRectangle { get; set; }
    }
}
