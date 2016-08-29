using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui2.Animations
{
    public class UiAlphaAnimation : UiAnimation
    {
        public float StartAlpha { get; set; }
        public float EndAlpha { get; set; }

        public UiAlphaAnimation(float durationSeconds, float startAlpha, float endAlpha) : base(durationSeconds)
        {
            StartAlpha = startAlpha;
            EndAlpha = endAlpha;
        }

        protected override void OnUpdate()
        {
            Owner.Alpha = MathHelper.Lerp(StartAlpha, EndAlpha, Delta);
        }
    }
}