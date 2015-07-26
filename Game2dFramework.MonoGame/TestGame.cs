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
            RegisterState(new CustomGuiSkin());
            return typeof(AnimationTest);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            var parameters = new GuiSystemSkinParameters
            {
                XmlSkinDescriptorFile = "GuiSkin/GuiSkin.xml",
                BigFont = Content.Load<SpriteFont>(ResourceNames.Spritefonts.BigFont),
                NormalFont = Content.Load<SpriteFont>(ResourceNames.Spritefonts.NormalFont),
                SkinTexture = Content.Load<Texture2D>(ResourceNames.Textures.bombrush_guiskin)
            };

            GuiSystem.SetSkin(parameters);

            IsMouseVisible = true;
        }
    }
}
