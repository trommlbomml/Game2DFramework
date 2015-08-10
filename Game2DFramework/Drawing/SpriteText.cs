using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Drawing
{
    public class SpriteText
    {
        private readonly SpriteFont _font;
        private string _text;

        public SpriteText(SpriteFont font, string text = null)
        {
            _font = font;
            Text = text;
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (Equals(_text, value)) return;
                _text = value;
                TextSize = string.IsNullOrEmpty(_text) ? Vector2.Zero : _font.MeasureString(_text);
            }
        }

        public Vector2 Position;
        public Vector2 TextSize { get; private set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(Text)) return;
            spriteBatch.DrawString(_font, Text, Position, Color.White);
        }
    }
}
