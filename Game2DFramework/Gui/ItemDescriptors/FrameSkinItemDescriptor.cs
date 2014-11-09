using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Game2DFramework.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui.ItemDescriptors
{
    internal class FrameSkinItemDescriptor : SkinItemDescriptor
    {
        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }
        public string NodeName { get { return "Frame"; } }

        public void Deserialize(XmlElement element)
        {
            FrameBorder = Thickness.Parse(element.GetAttribute("Border"));
            SourceRectangle = element.GetAttribute("NormalRectangle").ParseRectangle();
        }

        public Thickness FrameBorder { get; set; }
        public Rectangle SourceRectangle { get; set; }
    }
}
