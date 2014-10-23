using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Drawing
{
    public class CursorType
    {
        private readonly Texture2D _texture;
        private readonly Vector2 _hotspot;
        private readonly Rectangle _sourceRectangle;

        public CursorType(Texture2D texture, Vector2? hotspot = null, Rectangle? sourceRectangle = null)
        {
            if (texture == null) throw new ArgumentNullException("texture");

            _texture = texture;
            _hotspot = hotspot.GetValueOrDefault(Vector2.Zero);
            _sourceRectangle = sourceRectangle.GetValueOrDefault(texture.Bounds);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(_texture, position, _sourceRectangle, Color.White, 0.0f, _hotspot, 1.0f, SpriteEffects.None, 0);
        }
    }
}