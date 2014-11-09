using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui.ItemDescriptors
{
    class TextBoxSkinItemDescriptor : SkinItemDescriptor
    {
        public string NodeName { get { return "TextBox"; } }

        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }

        public Rectangle NormalRectangle { get; set; }
        public Thickness Border { get; set; }
        
        public void Deserialize(XmlElement element)
        {
            
        }
    }
}
