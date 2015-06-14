using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Game2DFramework.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.Gui
{
    public class Image : GuiElement
    {
        private Vector2 _position;
        public Texture2D Texture { get; set; }
        public Rectangle SourceRectangle { get; set; }

        public Image(GuiSystem guiSystem)
            : base(guiSystem)
        {
        }

        public Image(GuiSystem guiSystem, XmlElement element)
            : base(guiSystem, element)
        {
            if(element.HasAttribute("Source"))
            {
                Texture = guiSystem.Game.Content.Load<Texture2D>("Textures/" + element.GetAttribute("Source"));
            }

            if(Texture != null)
            {
                SourceRectangle = element.HasAttribute("NormalRectangle") ? element.GetAttribute("NormalRectangle").ParseRectangle() : Texture.Bounds;   
            }
        }

        public override Rectangle GetMinSize()
        {
            return ApplyMarginAndHandleSize(SourceRectangle);
        }

        public override void Arrange(Rectangle target)
        {
            var localBounds = RemoveMargin(target);
            _position = localBounds.Center.ToVector2();
        }

        public override void Draw()
        {
            if(Texture == null) return;
            Game.SpriteBatch.Draw(Texture, _position, SourceRectangle, Color.White, 0.0f, SourceRectangle.Center.ToVector2(), 1.0f, SpriteEffects.None, 0);
        }

        public override void Update(float elapsedTime)
        {
        }
    }
}
