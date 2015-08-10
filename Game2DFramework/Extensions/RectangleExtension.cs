using Microsoft.Xna.Framework;

namespace Game2DFramework.Extensions
{
    public static class RectangleExtension
    {
        public static Rectangle Translate(this Rectangle rectangle, int x, int y)
        {
            return new Rectangle(rectangle.X +x, rectangle.Y + y, rectangle.Width, rectangle.Height);
        }
    }
}
