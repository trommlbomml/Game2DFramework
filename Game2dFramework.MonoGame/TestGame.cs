using System;
using Game2DFramework.Gui;
using Microsoft.Xna.Framework.Graphics;

namespace Game2DFramework.MonoGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TestGame : Game2D
    {
#if WINDOWS || LINUX
        static class Program
        {
            static void Main()
            {
                using (var game = new TestGame()) game.Run();
            }
        }
#endif

        public TestGame()
            : base(1024, 768)
        {
        }

        protected override Type RegisterStates()
        {
            RegisterState(new StartState());
            RegisterState(new StackPanelWithFrame());
            RegisterState(new GridTest());
            RegisterState(new InputGuiTestState());
            RegisterState(new AnimationTest());
            return typeof(StartState);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            var parameters = new GuiSystemSkinParameters
            {
                XmlSkinDescriptorFile = "GuiSkin/GuiSkin.xml",
                BigFont = Content.Load<SpriteFont>("Spritefonts/BigFont"),
                NormalFont = Content.Load<SpriteFont>("Spritefonts/NormalFont"),
                SkinTexture = Content.Load<Texture2D>("Textures/border")
            };

            GuiSystem.SetSkin(parameters);

            IsMouseVisible = true;
        }
    }
}
