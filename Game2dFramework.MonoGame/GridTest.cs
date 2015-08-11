using Game2DFramework.Gui;
using Game2DFramework.States;
using Microsoft.Xna.Framework;

namespace Game2DFramework.MonoGame
{
    class GridTest : InitializableState
    {
        private GuiPanel _panel;
        private Frame _frame;

        protected override void OnEntered(object enterInformation)
        {
        }

        protected override void OnInitialize(object enterInformation)
        {
            _panel = new GuiPanel(Game);
            _frame = Game.GuiSystem.CreateGuiHierarchyFromXml<Frame>("GuiSkin/GridFrame.xml");
            _panel.AddElement(_frame);
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

