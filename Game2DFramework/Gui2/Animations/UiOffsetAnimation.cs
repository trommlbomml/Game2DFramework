using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Game2DFramework.Gui2.Animations
{
    public class UiOffsetAnimation : UiAnimation
    {
        private readonly Vector2 _offsetStart;
        private readonly Vector2 _offsetEnd;

        public UiOffsetAnimation(float durationSeconds, Vector2 offsetStart, Vector2 offsetEnd) : base(durationSeconds)
        {
            _offsetStart = offsetStart;
            _offsetEnd = offsetEnd;
        }

        protected override void OnUpdate()
        {
            Owner.Offset = Vector2.Lerp(_offsetStart, _offsetEnd, Delta);
        }
    }
}