﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui2.Controls
{
    public class Image : UiElement
    {
        private readonly Texture2D _texture;
        private readonly Rectangle _sourceRectangle;

        public Image(Texture2D texture, Rectangle? sourceRectangle = null)
        {
            _texture = texture;
            _sourceRectangle = sourceRectangle.GetValueOrDefault(_texture.Bounds);
            Bounds = _sourceRectangle;
        }

        public override bool IsInteractable => false;
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            var bounds = GetBounds();
            spriteBatch.Draw(_texture, new Vector2(bounds.X, bounds.Y), _sourceRectangle, Color * Alpha, Rotation, Origin, Scale, SpriteEffects.None, 0);
        }
    }
}
