using Game2DFramework.Gui;
using Game2DFramework.States;
using Microsoft.Xna.Framework;

namespace Game2DFramework.MonoGame
{
    class InputGuiTestState : InitializableState
    {
        private GuiPanel _panel;

        protected override void OnEntered(object enterInformation)
        {
            
        }

        protected override void OnInitialize(object enterInformation)
        {
            _panel = new GuiPanel(Game);
            var frame = Game.GuiSystem.CreateGuiHierarchyFromXml<Frame>("GuiSkin\\InputBoxes.xml");
            Game.GuiSystem.ArrangeCenteredToScreen(Game, frame);

            _panel.AddElement(frame);
        }

        public override void OnLeave()
        {
        }

        public override StateChangeInformation OnUpdate(float elapsedTime)
        {
            _panel.Update(elapsedTime);

            return StateChangeInformation.Empty;
        }

        public override void OnDraw(float elapsedTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            _panel.Draw();
        }
    }
}
