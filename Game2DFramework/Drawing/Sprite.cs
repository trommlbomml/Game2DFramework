
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
        public Rectangle ?SourceRectangle;
        public Vector2 Origin;
        public float Alpha;

        public Sprite(Texture2D texture, Rectangle? sourceRectangle = null)
        {
            Texture = texture;
            SourceRectangle = sourceRectangle;
            Origin = Texture.GetCenter();
            Color = Color.White;
            Alpha = 1.0f;
            Scale = new Vector2(1,1);
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
