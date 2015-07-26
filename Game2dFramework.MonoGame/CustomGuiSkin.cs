﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game2DFramework.Gui;
using Game2DFramework.States;
using Microsoft.Xna.Framework;

namespace Game2DFramework.MonoGame
{
    class CustomGuiSkin : InitializableState
    {
        private Frame _frame;

        protected override void OnEntered(object enterInformation)
        {
            Game.GuiSystem.ArrangeCenteredToScreen(Game, _frame);
        }

        protected override void OnInitialize(object enterInformation)
        {
            _frame = Game.GuiSystem.CreateGuiHierarchyFromXml<Frame>("GuiSkin/CustomSkinMenu.xml");
        }

        public override void OnLeave()
        {
        }

        public override StateChangeInformation OnUpdate(float elapsedTime)
        {
            _frame.Update(elapsedTime);
            return StateChangeInformation.Empty;
        }

        public override void OnDraw(float elapsedTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            _frame.Draw();
        }
    }
}
