using System;
using System.Collections.Generic;
using System.Linq;
using Game2DFramework.Animations;

namespace Game2DFramework.Gui2.Animations
{
    public abstract class UiAnimation : Animation
    {
        public virtual UiElement Owner { get; set; }

        protected UiAnimation(float durationSeconds) : base(durationSeconds, false)
        {
        }
    }
}