using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui
{
    public class GuiSystemSkinParameters
    {
        public string XmlSkinDescriptorFile { get; set; }
        public Texture2D SkinTexture { get; set; }
        public SpriteFont BigFont { get; set; }
        public SpriteFont NormalFont { get; set; }
    }
}