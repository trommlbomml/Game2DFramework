using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui2.Animations
{
    public class UiColorAnimation : UiAnimation
    {
        private readonly Color _startColor;
        private readonly Color _endColor;

        public UiColorAnimation(float durationSeconds, Color startColor, Color endColor) : base(durationSeconds)
        {
            _startColor = startColor;
            _endColor = endColor;
        }

        protected override void OnUpdate()
        {
            Owner.Color = Color.Lerp(_startColor, _endColor, Delta);
        }
    }
}