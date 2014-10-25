using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Extensions
{
    public static class StringParseExtension
    {
        public static Rectangle ParseRectangle(this string value)
        {
            var token = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (token.Length < 4) return new Rectangle();

            return new Rectangle(int.Parse(token[0]), int.Parse(token[1]), int.Parse(token[2]), int.Parse(token[3]));
        }
    }
}
