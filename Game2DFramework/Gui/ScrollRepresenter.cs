using System;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui
{
    internal class ScrollRepresenter
    {
        private int _maxScrollY;
        private float _scrollCalculatedPerDisplayed;

        public ScrollRepresenter()
        {
            _maxScrollY = 0;
            _scrollCalculatedPerDisplayed = 0.0f;
        }

        public int ScrollValueDisplayed { get; private set; }
        public int ScrollValueCalculated { get; private set; }

        public void SetRange(int scrollableArea, int contentSize, int thumbSize)
        {
            _maxScrollY = scrollableArea - thumbSize;
            var invisibleSize = contentSize - scrollableArea;
            _scrollCalculatedPerDisplayed = invisibleSize / (float)_maxScrollY;
            
        }

        public void Scroll(int value)
        {
            ScrollValueDisplayed = MathHelper.Clamp(ScrollValueDisplayed + value, 0, _maxScrollY);
            ScrollValueCalculated = (int)Math.Round(ScrollValueDisplayed * _scrollCalculatedPerDisplayed);
        }
    }
}