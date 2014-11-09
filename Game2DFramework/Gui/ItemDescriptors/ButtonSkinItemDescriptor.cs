using System.Xml;
using Game2DFramework.Extensions;
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
            NormalRectangle = element.GetAttribute("NormalRectangle").ParseRectangle();
            HoverRectangle = element.GetAttribute("HoverRectangle").ParseRectangle();
        }

        public Thickness ButtonBorder { get; set; }
        public Rectangle NormalRectangle { get; set; }
        public Rectangle HoverRectangle { get; set; }
    }
}
