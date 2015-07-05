
using Game2DFramework.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Drawing
{
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        public Vector2 Position;
        public Vector2 Scale;
        public float Rotation;
        public Vector2 Origin;
        public float Alpha;

        public Sprite(Texture2D texture, Rectangle? sourceRectangle = null)
        {
            Texture = texture;
            SourceRectangle = sourceRectangle;
            Color = Color.White;
            Alpha = 1.0f;
            Scale = new Vector2(1,1);
            AutoCenter();
        }

        public Rectangle? SourceRectangle { get; private set; }

        private void AutoCenter()
        {
            var rectangle = SourceRectangle.GetValueOrDefault(new Rectangle(0, 0, Texture.Width, Texture.Height));
            Origin = new Vector2(rectangle.Width * 0.5f, rectangle.Height * 0.5f);
        }

        public void SetSourceRectangle(Rectangle? sourceRectangle, bool center = true)
        {
            SourceRectangle = sourceRectangle;
            if (center) AutoCenter();
        }

        public void SetScale(float scale)
        {
            Scale = new Vector2(scale, scale);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRectangle, Color * Alpha, Rotation, Origin, Scale, SpriteEffects.None, 0);
        }
    }
}
