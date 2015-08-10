using System;
using Game2DFramework.Gui;
using Game2DFramework.States;
using Game2DFramework.States.Transitions;
using Microsoft.Xna.Framework;

namespace Game2DFramework.MonoGame
{
    class StartState : InitializableState
    {
        private GuiPanel _root;
        private StateChangeInformation _stateChangeInformation;

        protected override void OnEntered(object enterInformation)
        {
        }

        protected override void OnInitialize(object enterInformation)
        {
            _root = new GuiPanel(Game);

            var root = Game.GuiSystem.CreateGuiHierarchyFromXml<GuiElement>("GuiSkin/StartStateLayout.xml");
            _root.AddElement(root);
            Game.GuiSystem.ArrangeCenteredToScreen(Game, root);

            SetButtonTransitionTo(root, "StackPanelWithFrameButton", typeof(StackPanelWithFrame));
            SetButtonTransitionTo(root, "GridTestButton", typeof(GridTest));
            SetButtonTransitionTo(root, "TextBoxTestButton", typeof(InputGuiTestState));
        }

        private void SetButtonTransitionTo(GuiElement root, string buttonId, Type targetState)
        {
            var button = root.FindGuiElementById<Button>(buttonId);
            button.Click += () =>
            {
                _stateChangeInformation = StateChangeInformation.StateChange(targetState, typeof (SlideTransition));
            };
        }

        public override void OnLeave()
        {
        }

        public override StateChangeInformation OnUpdate(float elapsedTime)
        {
            _stateChangeInformation = StateChangeInformation.Empty;
            _root.Update(elapsedTime);

            return _stateChangeInformation;
        }

        public override void OnDraw(float elapsedTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            _root.Draw();
        }
    }
}
