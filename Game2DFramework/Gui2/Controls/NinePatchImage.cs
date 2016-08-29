using Game2DFramework.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui2.Controls
{
    public class NinePatchImage : UiElement
    {
        public override bool IsInteractable => false;

        private readonly NinePatchSprite _sprite;

        public NinePatchImage(NinePatchSprite sprite, Rectangle bounds)
        {
            _sprite = sprite;
            Bounds = bounds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, GetBounds());
        }
    }
}
