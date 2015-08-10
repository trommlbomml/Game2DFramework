using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Drawing
{
    public class SpriteText
    {
        public SpriteFont Font { get; private set; }

        private string _text;

        public SpriteText(SpriteFont font, string text = null)
        {
            Font = font;
            Text = text;
            Color = Color.White;
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (Equals(_text, value)) return;
                _text = value;
                TextSize = string.IsNullOrEmpty(_text) ? Vector2.Zero : Font.MeasureString(_text);
            }
        }

        public Vector2 Position;
        public Vector2 TextSize { get; private set; }
        public Color Color { get; set; }
        public int LineSpacing { get { return Font.LineSpacing; } }

        public Rectangle GetBounds(bool lineSpacingHeightForEmpty)
        {
            var bounds = string.IsNullOrEmpty(_text) ? new Rectangle(0, 0, 0,lineSpacingHeightForEmpty ? LineSpacing : 0) : new Rectangle(0,0, (int) TextSize.X, (int) TextSize.Y);

            bounds.X = (int) Position.X;
            bounds.Y = (int) Position.Y;

            return bounds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(Text)) return;
            spriteBatch.DrawString(Font, Text, Position, Color);
        }
    }
}
