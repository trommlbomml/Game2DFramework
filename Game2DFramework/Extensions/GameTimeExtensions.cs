using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Extensions
{
    public static class GameTimeExtensions
    {
        public static float GetSeconds(this GameTime gameTime)
        {
            return gameTime.ElapsedGameTime.Milliseconds * 0.001f;
        }
    }
}
