using System;
using System.Collections.Generic;
using System.Linq;
using Game2DFramework.Gui;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.TestGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TestGame : Game2D
    {
        static class Program
        {
            static void Main()
            {
                using (var game = new TestGame()) game.Run();
            }
        }

        public TestGame()
            : base(1024, 768, false)
        {
        }

        protected override Type RegisterStates()
        {
            RegisterState(new StartState());
            RegisterState(new StackPanelWithFrame());
            RegisterState(new GridTest());
            return typeof(StartState);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            var parameters = new GuiSystemSkinParameters
            {
                XmlSkinDescriptorFile = "GuiSkin/GuiSkin.xml",
                BigFont = Content.Load<SpriteFont>("BigFont"),
                NormalFont = Content.Load<SpriteFont>("NormalFont"),
                SkinTexture = Content.Load<Texture2D>("border")
            };

            GuiSystem.SetSkin(parameters);

            IsMouseVisible = true;
        }
    }
}
