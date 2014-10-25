using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui.ItemDescriptors
{
    class TextBlockSkinItemDescriptor : SkinItemDescriptor
    {
        public string NodeName { get { return "TextBlock"; } }
        
        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }

        public void Deserialize(XmlElement element)
        {
        }
    }
}
