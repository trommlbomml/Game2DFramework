using System;
using Game2DFramework.Gui;
using Game2DFramework.States;
using Game2DFramework.States.Transitions;
using Microsoft.Xna.Framework;

namespace Game2DFramework.MonoGame
{
    class StartState : InitializableState
    {
        private GuiElement _root;
        private StateChangeInformation _stateChangeInformation;

        protected override void OnEntered(object enterInformation)
        {
            Game.GuiSystem.ArrangeCenteredToScreen(Game, _root);
        }

        protected override void OnInitialize(object enterInformation)
        {
            _root = Game.GuiSystem.CreateGuiHierarchyFromXml<GuiElement>("GuiSkin/StartStateLayout.xml");

            SetButtonTransitionTo("StackPanelWithFrameButton", typeof(StackPanelWithFrame));
            SetButtonTransitionTo("GridTestButton", typeof(GridTest));
            SetButtonTransitionTo("TextBoxTestButton", typeof(InputGuiTestState));
            SetButtonTransitionTo("AnimationsTestButton", typeof(AnimationTest));
        }

        private void SetButtonTransitionTo(string buttonId, Type targetState)
        {
            var button = _root.FindGuiElementById<Button>(buttonId);
            button.OnClick += () =>
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
