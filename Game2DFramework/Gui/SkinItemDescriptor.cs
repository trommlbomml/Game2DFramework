using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui
{
    internal interface SkinItemDescriptor
    {
        string NodeName { get;}
        Texture2D SkinTexture { get; set; }
        SpriteFont BigFont { get; set; }
        SpriteFont NormalFont { get; set; }

        void Deserialize(XmlElement element);
    }
}
